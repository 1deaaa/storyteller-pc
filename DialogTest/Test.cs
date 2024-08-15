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
        public static JObject JsonSource = JObject.Parse(System.IO.File.ReadAllText(@"..\..\..\对话.json"));
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
    }
    public static class Dialog
    {
        //对于vs调试显示的json数据 外层都被加了一组{} 实际上是不存在的
        public static int ChoiceCureent;
        public static int CurrentMainObjID = 0;//目前遍历到的主线对话对象
        static JObject CurrentGroup;
        public static JObject CurrentObj;//目前遍历到的对话对象
        public static List<JObject> MainObj = new();//每个场景下的主线对话对象
        static bool IsOpt=false;
        public static bool EndDialog=false;//下一次点击直接关闭对话
        static List<int> RestOfGroupMember;//对象内的剩余成员数 用于确定是否要跳出内层 序号为层级（深度） 成员为具体数 #目前假定分支末端只会跳转到主线/使用next跳转至场景
        static int DepthCurrent = 0;//当前遍历深度 从零开始
        static string NextDialog = "";//指定next所指向的下一个对话场景 为空表示不跳转
        static List<ChoiceBtn> branch_btns = new();
        public static bool DialogEnabled = true;
        public static JObject Scene;
        #region Debug
        static void Debug(object t)
        {
            MessageBox.Show(t.ToString());
        }

        //public Dialog(string _scene)
        //{
        //    DialogScene = MainUI.JsonSource[_scene];//根（场景）键值对的值为数组  Token代表任意数据节点 Prop代表键值对 Object代表{xxx}
        //    foreach (JObject obj in DialogScene)
        //    {
        //        MainObj.Add(obj);
        //    }
        //    CurrentObj = MainObj[0];
        //    act ??= new Dictionary<string, ActionHandler>
        //    {
        //            #region 行为映射表
        //            { "BGM", Method.Music },//绑定所有指令和函数
        //            {"动作",Method.Animate }
        //            #endregion 行为映射表
        //    };
        //    chr ??= new Dictionary<int, string>()
        //    {
        //            #region 角色映射表
        //            {0,"我" },
        //            {1,"..." },
        //            { 2,"可可酱"}
        //            #endregion 角色映射表
        //    };
        //}
        #endregion
        public static void SceneInit(string _scene)
        {
            //??=如果为null才赋值 防止重复赋值
            Scene = (JObject)MainUI.JsonSource[_scene];//根（场景）键值对的值为数组  Token代表任意数据节点 Prop代表键值对 Object代表{xxx}
            //查找名为_scene的键值
            MainObj.Clear();
            CurrentMainObjID = 0;
            NextDialog = "";
            IsOpt = false;
            RestOfGroupMember.Clear();
            RestOfGroupMember.Add(Scene.Count);
            DepthCurrent= 0;
            CurrentObj = (JObject)Scene.Properties().First().Value;//获取场景下的第一个对话对象
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
            ChoiceCureent = clicked_btn.Choice;
            //
            Program.UI.Text = "曼波" + ChoiceCureent.ToString();
            //
            CurrentGroup = (JObject)CurrentObj[clicked_btn.Text];//根据选项定位
            RestOfGroupMember[DepthCurrent] = CurrentGroup.Count - 1;//目前已经处理过第一个
            try
            {
                CurrentObj = (JObject)CurrentGroup.Properties().First().Value;
            }
            catch(Exception error)
            {
                Method.Error($"对话数据错误 不要修改游戏文件鸭~\n以下给开发人员看的：{error}");
                Environment.Exit(1);
            }
            foreach (var i in branch_btns)
                i.Dispose();//关闭选项
            branch_btns.Clear();
            DisplayOne(CurrentObj, Program.UI);
        }

        public static void DisplayOne(JObject obj,MainUI ui)//传入一个对话对象
        {
            if (EndDialog)
            {
                End(ui);
                EndDialog = false;
                return;
            }
            if (NextDialog!="")
            {
                SceneInit(NextDialog);
                DisplayOne(CurrentObj, Program.UI);
                return;
            }
            IsOpt=false;
            DialogEnabled = true;
            foreach (JProperty key in obj.Properties())//遍历对象下所有一级子节点键值对
            {
                switch(key.Name)
                {
                    case "chr":
                        ui.spk.Text = Map.ChrMap[(int)key.Value];
                        break;
                    case "txt":
                        ui.txt.Text = key.Value.ToString();
                        break;
                    case "act":
                        foreach(JProperty acts in key.Value)
                        {
                            switch(acts.Name)
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
                        IsOpt=true;
                        RestOfGroupMember = 0;
                        CurrentObj = (JObject)CurrentObj["opt"];
                        int i = 0;
                        foreach (JProperty options in key.Value)
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
            if (!IsOpt && NextDialog == "" && DepthCurrent == 0 && RestOfGroupMember[0] > 0)//跳回主线
                CurrentObj = MainObj[++CurrentMainObjID];
            else if (RestOfGroupMember[DepthCurrent] == 0&&DepthCurrent>0)
                DepthCurrent -= 1;
            else if (RestOfGroupMember[DepthCurrent] > 0)//还在组内
                CurrentObj = (JObject)CurrentGroup.Properties().ElementAt(RestOfGroupMember[DepthCurrent]).Value;
            else if (CurrentMainObjID + 1 >= MainObj.Count && NextDialog == "")//对话结束且无结束后跳转
                EndDialog = true;
        }
        public static void End (MainUI ui)
        {
            ui.txt.Dispose();
            ui.spk.Dispose();       
         }
        class ChoiceBtn : Button
        {
            public int Choice = 0;
        }
        #region 弃用
        /*
        public void ShowD(Form f)//实质上为解析一个对话对象
        {
            if (id[0][item] >= json.Count() - 1)//先检测0级对话（最外层被汉语标记的）是否进行完 防止数组溢出
            {
                return;
                //这里可以写关闭对话框
            }
            Label txt = f.Controls["txt"] as Label;
            Label spk = f.Controls["spk"] as Label;
            try//无视找不到元素的报错
            {
                spk.Text = chr[json[id]["chr"].Value<int>()];
                txt.Text = json[id]["txt"].Value<string>();
            }
            catch { }
            //这是所有特殊选项
            #region 选项opt处理
            //遇到选项则展开下级分支
            if (json[id[level][item]].SelectToken("opt") != null)
            {
                level++;
                DialogEnabled = false;
                branch = new List<Btn>();
                int num = 0;
                foreach (JProperty i in json[id[level][item]])
                {
                    num++;
                    branch_num++;//计算选项数量
                    Btn btn = new();
                    branch.Add(btn);
                    btn.Text = i.Name;
                    btn.item = num;
                    btn.Size = new Size(200, 50);
                    //
                    btn.Location = new Point(f.Width - 200, 350 + btn.Size.Height * num);
                    btn.Click += Btn_Click;
                    f.Controls.Add(btn);
                    //
                }
                //next_id = id + branch_num;
            }
            #endregion 选项
            #region 行为act处理
            if (json[id].SelectToken("act") != null)//行为执行
            {
                foreach (var action in json[id]["act"])
                {
                    string actionString = action.ToString();
                    if (act.ContainsKey(actionString))
                    {
                        act[actionString]();//对应委托
                    }
                }
            }
            #endregion 行为
            #region 无义选项opt_fake处理
            if (json[id].SelectToken("opt_fake") != null)
            {
                DialogEnabled = false;
                branch = new List<Btn>();
                int num = 0;
                foreach (JProperty i in json[id]["opt_fake"])
                {
                    num++;
                    branch_num++;//计算选项数量
                    Btn btn = new();
                    branch.Add(btn);
                    btn.Text = i.Name;
                    btn.item = num;
                    btn.Size = new Size(200, 50);
                    btn.Location = new Point(f.Width - 200, 350 + btn.Size.Height * num);
                    btn.Click += FakeBtnClick;
                    f.Controls.Add(btn);
                }
            }
            #endregion 无义选项
        }
        */
        #endregion
    }
}
