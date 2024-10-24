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
        public static int CurrentMainObjID = 0;//目前遍历到的主线对话对象
        static JArray CurrentGroup;
        public static JObject CurrentObj;//目前遍历到的对话对象
        public static List<JObject> MainObj = new();//每个场景下的主线对话对象 默认下一组就是如此
        static bool IsChoice = false;
        public static bool EndDialog = false;//下一次点击直接关闭对话
        static int RestOfGroupMember = 0;//数组/对象内的剩余成员数 用于确定是否要跳出内层 #目前假定分支末端只会跳转到主线/使用next跳转至场景
        static string NextDialog = "";//指定next所指向的下一个对话场景 为空表示不跳转
        static List<ChoiceBtn> branch_btns = new();
        public delegate void ActionHandler();
        public static Dictionary<string, ActionHandler> ActMap;
        public static Dictionary<int, string> ChrMap;
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
            CurrentMainObjID = 0;
            NextDialog = "";
            IsChoice = false;
            RestOfGroupMember = 0;
            foreach (JObject obj in DialogScene["dia"])
            {

            }
            CurrentObj = MainObj[0];
            ActMap ??= new Dictionary<string, ActionHandler>
            {
                    #region 行为映射表
                //绑定所有指令和函数
                    {"动作",Method.Animate }
                    #endregion 行为映射表
            };
            ChrMap ??= new Dictionary<int, string>()
            {
                    #region 角色映射表
                    {0,"我" },
                    {1,"..." },
                    { 2,"可可酱"}
                    #endregion 角色映射表
            };
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
            CurrentGroup = (JArray)CurrentObj[clicked_btn.Text];//根据选项定位
            RestOfGroupMember = CurrentGroup.Count - 1;//目前已经处理过第一个
            try
            {
                CurrentObj = (JObject)CurrentGroup[0];
            }
            catch (Exception error)
            {
                Method.Error($"对话数据错误 不要修改游戏文件鸭~\n以下给开发人员看的：{error}");
                Environment.Exit(1);
            }
            foreach (var i in branch_btns)
                i.Dispose();//关闭选项
            branch_btns.Clear();
            DisplayOne(CurrentObj, Program.UI);
        }

        public static void DisplayOne(JObject obj, MainUI ui)//传入一个对话对象 每个对话对象的根级必为obj 而深处必只有prop
        {
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
            foreach (JProperty key in obj.Properties())//遍历对象下所有一级子节点键值对
            {
                switch (key.Name)
                {
                    case "chr":
                        ui.spk.Text = ChrMap[(int)key.Value];
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
                                    if (ActMap.ContainsKey(acts.Value.ToString()))
                                        ActMap[acts.Value.ToString()]();//对应委托
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
            if (!IsChoice && NextDialog == "" && RestOfGroupMember == 0 && CurrentMainObjID + 1 < MainObj.Count)
                CurrentObj = MainObj[++CurrentMainObjID];
            else if (RestOfGroupMember > 0)
                CurrentObj = (JObject)CurrentGroup[CurrentGroup.Count - RestOfGroupMember];
            else if (CurrentMainObjID + 1 >= MainObj.Count && NextDialog == "")
                EndDialog = true;
        }
        public static void End(MainUI ui)
        {
            ui.txt.Dispose();
            ui.spk.Dispose();
        }
        static class Method
        {
            public static void Error(string e)
            {
                MessageBox.Show(e, "o(TヘTo)", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            public static void Music(string bgm)
            {
                Program.UI.Text = "正在播放" + bgm;
            }
            public static void Animate()
            {
                Program.UI.Text = "执行到了某个动作";
            }
            public static void RecordBranch(string brc)
            {
                Program.UI.Text = "已经写入进度：" + brc;
            }
        }
        class ChoiceBtn : Button
        {
            public int Choice = 0;
        }
    }
}