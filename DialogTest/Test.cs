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
        //对话脚本结构中 之所以不用键名存储实际文本 是因为键名无法被修改
        public MainUI()
        {
            InitializeComponent();
        }
        private void txt_Click(object sender, EventArgs e)
        {
            if (!Dialog.DialogEnabled)
                return;
            Dialog.DisplayOne(Dialog.CurrentObj, this);
        }

        private void MainUI_Load(object sender, EventArgs e)
        {
            Dialog.SceneInit("开场场景");
            Dialog.DisplayOne(Dialog.CurrentObj, this);
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
    class DialogGroup
    {
        public JArray Array;
        public int NextIndex;
        public DialogGroup(JArray _array)
        {
            Array = _array;
            NextIndex = 0;//0才是有效的第一个
        }
    }
    static class Dialog
    {
        //对于vs调试显示的json数据 外层都被加了一组{} 实际上是不存在的
        public static int Choice = 0;//注意 从1开始！！！
        public static int CurrentGroupObjIndex = 0;//目前遍历到的组内对话对象
        static Stack<DialogGroup> DialogArray=new();
        public static JObject CurrentObj;//目前遍历到的对话对象
        public static List<JObject> MainObj = new();//每个场景下的主线对话对象 默认下一组就是如此
        static bool waitForChoice = false;
        public static bool EndDialog = false;//下一次点击直接关闭对话
        static string NextDialog = null;//指定next所指向的下一个对话场景 为null表示不跳转
        static List<ChoiceBtn> branch_btns = new();
        public static bool DialogEnabled = true;
        public static JToken DialogScene;
        public static JArray CrtArray
        {
            get { return DialogArray.Peek().Array; }
            set { DialogArray.Peek().Array = value; }
        }
        public static int CrtIndex
        {
            get { return DialogArray.Peek().NextIndex; }
            set { DialogArray.Peek().NextIndex = value; }
        }
        static void Debug(object t)
        {
            MessageBox.Show(t.ToString());
        }
        public static void SceneInit(string _scene)
        {
            //??=如果为null才赋值 防止重复赋值
            DialogScene = Manager.GetSceneObj(_scene);//根（场景）键值对的值为数组  Token代表任意数据节点 Prop代表键值对 Object代表{xxx}
            MainObj.Clear();
            CurrentGroupObjIndex = 0;
            NextDialog = null;
            waitForChoice = false;
            Program.UI.cap.Text = DialogScene["cap"].ToString();
            DialogArray.Clear();
            DialogArray.Push(new DialogGroup((JArray)DialogScene["dia"]));
            CurrentObj = (JObject)CrtArray[0];
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
            JObject selectedOption = (JObject)CurrentObj["opt"][Choice - 1];
            JArray diaArray = (JArray)selectedOption["dia"];
            DialogArray.Push(new DialogGroup(diaArray));//根据选项定位新的对话组
            CurrentObj = (JObject)CrtArray[0];//进入选项内部对话
            foreach (var i in branch_btns)
                i.Dispose();//关闭选项
            branch_btns.Clear();
            DisplayOne(CurrentObj, Program.UI);
        }


        public static void DisplayOne(JObject crt_obj, MainUI ui)//传入一个对话对象
        {
            #region 处理跳转和初始化
            if (EndDialog)
            {
                End(ui);
                EndDialog = false;
                return;
            }
            if (NextDialog != null)
            {
                SceneInit(NextDialog);
                DisplayOne(CurrentObj, Program.UI);
                return;
            }
            waitForChoice = false;
            DialogEnabled = true;
            #endregion

            foreach (JProperty key in crt_obj.Properties())//解析一个dia下所有参数
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
                        waitForChoice = true;
                        int i = 1;
                        foreach (JObject option in key.Value)
                        {
                            ChoiceBtn btn = new();
                            branch_btns.Add(btn);
                            btn.Text = option["optn"].ToString();
                            btn.Choice = i;
                            #region 界面相关
                            btn.Size = new Size(200, 50);
                            btn.Location = new Point(ui.Width - 200, btn.Size.Height * i);
                            btn.Click += ChoiceBtn_Click;
                            ui.Controls.Add(btn);
                            #endregion
                            i++;
                        }
                        break;
                    case "next":
                        NextDialog = key.Value.ToString();
                        break;
                }
            }

            //解析任务结束 已经显示在屏幕上 开始定位下一次解析位置 所有current皆为下次待解析对象
            if (CrtIndex < CrtArray.Count)
                CrtIndex++;
            if (waitForChoice)
                return;
            if (NextDialog != null)
            {
                SceneInit(NextDialog);
            }

            if (CrtArray.Count - CrtIndex == 0)//本层已全部解析完毕 退出本层
            {
                while (CrtArray.Count - CrtIndex == 0)
                {
                    DialogArray.Pop();
                    if (DialogArray.Count == 0)//场景所有对话结束
                    {
                        EndDialog = true;//下次点击 在事件开头直接退出
                        return;
                    }
                }
                CurrentObj = (JObject)CrtArray[CrtIndex];//切换到外层
            }
            else if (CrtArray.Count - CrtIndex > 0)
            {
                CurrentObj = (JObject)CrtArray[CrtIndex];
            }
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