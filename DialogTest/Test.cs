using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
namespace DialogSystem
{
    public partial class MainUI : Form
    {
        public static string SrcPath= @"..\..\..\对话.json";
        public static JObject JsonSource = JObject.Parse(System.IO.File.ReadAllText(SrcPath));
        //层级设计:总对象(obj)->索引(array)->键值对(prop)
        public MainUI()
        {
            InitializeComponent();
        }
        private void txt_Click(object sender, EventArgs e)
        {
            if (!Dialog.DialogEnabled)
            {
                return;
            }
            Dialog.DisplayOne(Dialog.CurrentObj, this);
        }

        private void MainUI_Load(object sender, EventArgs e)
        {
            Dialog.SceneInit("开场");
            Dialog.DisplayOne(Dialog.CurrentObj, this);
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
    static class Dialog
    {
        //对于vs调试显示的json数据 外层都被加了一组{} 实际上是不存在的
        public static int Choice = 0;//注意 从1开始！！！
        public static int CurrentGroupObjIndex = 0;//目前遍历到的组内对话对象
        static JArray CurrentDialogArray;
        public static JObject CurrentObj;//目前遍历到的对话对象
        public static List<JObject> MainObj = new();//每个场景下的主线对话对象 默认下一组就是如此
        static bool IsChoice = false;
        public static bool EndDialog = false;//下一次点击直接关闭对话
        static int RestOfGroupMember = 0;//数组/对象内的剩余成员数 用于确定是否要跳出内层 #目前假定分支末端只会跳转到主线/使用next跳转至场景
        static string NextDialog = "";//指定next所指向的下一个对话场景 为空表示不跳转
        static List<ChoiceBtn> branch_btns = new();
        public static bool DialogEnabled = true;
        public static JToken DialogScene;
        static void Debug(object t)
        {
            MessageBox.Show(t.ToString());
        }
        public static void SceneInit(string _scene)
        {
            //??=如果为null才赋值 防止重复赋值
            DialogScene = MainUI.JsonSource[_scene];//根（场景）键值对的值为数组  Token代表任意数据节点 Prop代表键值对 Object代表{xxx}
            MainObj.Clear();
            CurrentGroupObjIndex = 0;
            NextDialog = "";
            IsChoice = false;
            RestOfGroupMember = 0;
            CurrentObj = (JObject)CurrentDialogArray[0];
        }

        private static void FakeBtnClick(object s, EventArgs e)//空选项 点了没用等于继续对话
        {
            ChoiceBtn click = (ChoiceBtn)s;
            foreach (var i in branch_btns)
            {
                i.Dispose();//关闭选项
            }
            branch_btns.Clear();
            DialogEnabled = true;//恢复对话框点击
        }
        private static void ChoiceBtn_Click(object sender, EventArgs e)//选项点击 也相当于点击了一次继续
        {
            ChoiceBtn clicked_btn = (ChoiceBtn)sender;
            Choice = clicked_btn.Choice;
            //
            Program.UI.Text = "选择了选项" + Choice.ToString();
            //
            CurrentDialogArray = (JArray)CurrentObj[clicked_btn.Text];//根据选项定位新的对话组
            CurrentObj = (JObject)CurrentDialogArray[0];//进入选项内部对话
            foreach (var i in branch_btns)
                i.Dispose();//关闭选项
            branch_btns.Clear();
            RestOfGroupMember = CurrentDialogArray.Count - 1;//本次会处理掉第一个成员
            DisplayOne(CurrentObj, Program.UI);
        }

        public static void DisplayOne(JObject crt_obj, MainUI ui)//传入一个对话对象 每个对话对象的根级必为obj 而深处必只有prop
        {
            #region 处理跳转和初始化
            if (EndDialog)
            {
                End(ui);
                EndDialog = false;
                return;
            }
            if (NextDialog != "")
            {
                SceneInit(NextDialog);
                DisplayOne(CurrentObj, Program.UI);
                return;
            }
            IsChoice = false;
            DialogEnabled = true;
            #endregion
            foreach (JProperty key in crt_obj.Properties())//遍历对象下所有一级子节点键值对
            {
                switch (key.Name)
                {
                    case "chr":
                        ui.spk.Text = Map.ChrMap[(int)key.Value];
                        break;
                    case "txt":
                        ui.txt.Text = key.Value.ToString();
                        break;
                    case "act":
                        foreach (JProperty acts in key.Value)
                        {
                            switch (acts.Name)
                            {
                                case "bgm":
                                    Method.Music(acts.Value.ToString());
                                    break;
                                case "fun":
                                    if (Map.ActMap.ContainsKey(acts.Value.ToString()))
                                        Map.ActMap[acts.Value.ToString()]();//对应委托
                                    break;
                                case "brc":
                                    Method.RecordBranch(acts.Value.ToString());
                                    break;
                            }
                        }
                        break;
                    case "opt":
                        DialogEnabled = false;
                        IsChoice = true;
                        RestOfGroupMember = 0;
                        CurrentObj = (JObject)CurrentObj["opt"];
                        int i = 1;
                        foreach (JProperty options in key.Value)//"option":[xxx]
                        {
                            ChoiceBtn btn = new();
                            branch_btns.Add(btn);
                            btn.Text = options.Name;
                            btn.Choice = i;
                            //
                            btn.Size = new Size(200, 50);
                            btn.Location = new Point(ui.Width - 200, 350 + btn.Size.Height * i);
                            btn.Click += ChoiceBtn_Click;
                            ui.Controls.Add(btn);
                            //
                            i++;
                        }
                        //按钮点击事件：向深处（子级对象）递归一次
                        break;
                    case "next":
                        NextDialog = key.Value.ToString();
                        break;
                }
            }
            if (!IsChoice && NextDialog == "" && RestOfGroupMember == 0 && CurrentGroupObjIndex + 1 < MainObj.Count)
                CurrentObj = MainObj[++CurrentGroupObjIndex];
            else if (RestOfGroupMember > 0)
                CurrentObj = (JObject)CurrentDialogArray[CurrentDialogArray.Count - RestOfGroupMember];
            else if (CurrentGroupObjIndex + 1 >= MainObj.Count && NextDialog == "")
                EndDialog = true;
        }
        public static void End(MainUI ui)
        {
            ui.txt.Dispose();
            ui.spk.Dispose();
        }
        class ChoiceBtn : Button
        {
            public int Choice = 0;
        }
    }
}