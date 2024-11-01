using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using static DialogSystem.Map;

namespace DialogSystem
{
    public partial class Editor : Form
    {
        RichNode CurrentNode;//当前选中节点
        RichNode _last_slc;//上一个选中节点
        public static string DataFilePath = @"..\..\..\对话.json";
        public static JToken JsonSource = JToken.Parse(File.ReadAllText(DataFilePath));
        public static string CurrentScene = "";
        public static int CurrentId;
        int _option_id;//记录选项所属父级id
        #region 搜索和UI相关
        public Editor()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            LoadTree();
        }

        private RichNode FindNodeByText(TreeNodeCollection nodes, string searchText)
        {
            foreach (RichNode node in nodes)
            {
                if (node.Text.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return node;
                }
                RichNode foundNode = FindNodeByText(node.Nodes, searchText);
                if (foundNode != null)
                {
                    return foundNode;
                }
            }
            return null;
        }

        private List<string> MatchNode(string searchText)
        {
            List<string> matchingOptions = new List<string>();
            SearchNode(view.Nodes, searchText, matchingOptions);
            return matchingOptions;
        }

        private void SearchNode(TreeNodeCollection nodes, string searchText, List<string> matchingOptions)
        {
            foreach (RichNode node in nodes)
            {
                if (node.Text.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    matchingOptions.Add(node.Text);
                }
                SearchNode(node.Nodes, searchText, matchingOptions);
            }
        }

        private void UpdateListBox(List<string> options)
        {
            // 更新ListBox中的项
            search_list.Items.Clear();
            foreach (string option in options)
            {
                search_list.Items.Add(option);
            }
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            Form1_SizeChanged(sender, e);
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            // 当文本框内容发生变化时，执行模糊搜索并更新ListBox中的项
            string searchText = search.Text.Trim();

            if (!string.IsNullOrEmpty(searchText))
            {
                // 执行模糊搜索
                List<string> matchingOptions = MatchNode(searchText);
                // 更新List中的项
                UpdateListBox(matchingOptions);
            }
            else
            {
                // 清空ListBox
                search_list.Items.Clear();
            }
        }

        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            // 获取用户在ListBox中选中的项
            string selectedOption = search_list.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(selectedOption))
            {
                Thread td = new Thread(() =>
                {
                    // 根据选中的项在树状图中查找对应的节点
                    CurrentNode = FindNodeByText(view.Nodes, selectedOption);
                    if (CurrentNode != null)
                    {
                        // 自动选中树状图中的节点
                        if (_last_slc != null)
                            _last_slc.BackColor = Color.White;
                        view.SelectedNode = CurrentNode;
                        CurrentNode.BackColor = Color.Red;
                        _last_slc = CurrentNode;
                        CurrentNode.EnsureVisible();
                    }
                });
                td.Start();
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            view.Size = new Size(Width / 2, Height - 40);
            search.Location = new Point(Width - search.Width, 0);
            search_list.Location = new Point(search.Left, search.Height + 4);
            search_list.Size = new Size(search.Width, Height - 40 - 4);
        }
        #endregion

        void LoadTree()
        {
            //AddSceneToTreeView(view, (JObject)JsonSource);
            LoadDialogueToTreeView(view);
        }
        #region 弃用
        //private void AddSceneToTreeView(TreeView treeView, JObject jsonObject)
        //{
        //    foreach (var scene in jsonObject.Properties())
        //    {
        //        RichNode sceneNode = new RichNode();
        //        sceneNode.NodeType = NodeType.Scene;
        //        sceneNode.Text = scene.Name;
        //        CurrentScene = scene.Name;
        //        sceneNode.scene_cap=scene.Value["cap"]?.ToString();
        //        cap_edit.Text = sceneNode.scene_cap;
        //        sceneNode.scene_pgrs= scene.Value["pgrs"]?.ToString();
        //        pgrs_slc.Value = Convert.ToInt32(sceneNode.scene_pgrs);
        //        treeView.Nodes.Add(sceneNode);
        //        AddNodeToParent(sceneNode, (JArray)scene["dia"]);
        //    }
        //}

        //private void AddNodeToParent(RichNode parentNode, JArray dlg_arry)
        //{
        //    foreach (JObject obj in dlg_arry)
        //    {
        //        RichNode node = new RichNode();
        //        node.Text = "⏬";//默认文本 当连续选项出现时 没有txt节点设置文本
        //        node.scene = CurrentScene;
        //        if (obj is JObject)//处理分支节点
        //        {
        //            //对话ID节点
        //            int _id;
        //            if (int.TryParse(obj["id"].ToString(), out _id) && _id > 100)//所以选项名字严禁纯数字！！！
        //            {
        //                CurrentId = _id;
        //                node.id = CurrentId;
        //                //处理txt时会给节点设置Text

        //            }
        //            //
        //            else
        //            {
        //                node.id=CurrentId;
        //                switch (obj.Value)
        //                {
        //                    case "opt":
        //                        //node.opt = (JObject)key.Value;
        //                        node.Text = "✨选项";
        //                        node.NodeType=NodeType.OptionRoot;
        //                        node.ForeColor = ThemeColor.Option;
        //                        break;
        //                    case "act":
        //                        //node.act = (JObject)key.Value;
        //                        node.Text = "⚡行为";
        //                        node.NodeType=NodeType.ActionRoot;
        //                        node.ForeColor = ThemeColor.Action;
        //                        break;
        //                    case "brc":
        //                        node.Text = "🌿分支";
        //                        break;
        //                    default:
        //                        node.Text = obj.Name;//只有可能是选项名字了
        //                        node.opt = obj.Name;
        //                        node.NodeType=NodeType.Option;
        //                        node.BackColor = ThemeColor.Option;
        //                        break;
        //                }
        //            }
        //               parentNode.Nodes.Add(node);
        //               AddNodeToParent(node, (JObject)obj.Value);
        //        }
        //        else//处理单节点 也就是给父节点贴上属性或者子节点
        //        { 
        //            node.id = CurrentId;
        //            switch(obj.Name)
        //            {
        //                case "chr":
        //                    parentNode.chr=obj.Value.ToString();
        //                    break;
        //                case "txt":
        //                    parentNode.txt=obj.Value.ToString();
        //                    parentNode.Text=Map.ChrMap[int.Parse(parentNode.chr)]+"："+obj.Value.ToString();
        //                    break;
        //                case "bgm":
        //                    node.Text= "🎵" + obj.Value.ToString();
        //                    node.BackColor = ThemeColor.Action;
        //                    parentNode.Nodes.Add(node);
        //                    break;
        //                case "rcd"://记录当前选项
        //                    node.Text= "🖊️"+ obj.Value.ToString();
        //                    node.BackColor = ThemeColor.Action;
        //                    parentNode.Nodes.Add(node);
        //                    break ;
        //                case "fun":
        //                    node.Text = "⚡" + obj.Value.ToString();
        //                    node.BackColor = ThemeColor.Action;
        //                    parentNode.Nodes.Add(node);
        //                    break;               
        //            }
        //        }
        //    }
        //}
        #endregion
        private void view_AfterSelect(object sender, TreeViewEventArgs e)
        {
            CurrentNode = e.Node as RichNode;
            id.Text = "ID：" + CurrentNode.id.ToString();
            chr_edit.Text = CurrentNode.chr;
            txt_edit.Text = CurrentNode.txt;
            opt_edit.Text = CurrentNode.opt;
            cap_edit.Text = CurrentNode.scene_cap;
            pgrs_slc.Value = Convert.ToInt32(CurrentNode.scene_pgrs);
            CurrentScene = CurrentNode.scene;
        }
        #region AI生成实验
        public static void LoadDialogueToTreeView(TreeView treeView)
        {
            JObject src = JObject.Parse(File.ReadAllText(DataFilePath));
            foreach (var scene_ppt in src.Properties())
            {
                RichNode sceneNode = new RichNode(scene_ppt.Name);
                sceneNode.scene = scene_ppt.Name;
                treeView.Nodes.Add(sceneNode);
                JObject dialogueObject = scene_ppt.Value as JObject;
                if (dialogueObject != null)
                {
                    if (dialogueObject.ContainsKey("dia"))
                    {
                        JArray diaArray = dialogueObject["dia"] as JArray;
                        if (diaArray != null)
                        {
                            foreach (var diaItem in diaArray)
                            {
                                AddDialogueNode(diaItem as JObject, sceneNode);
                            }
                        }
                    }
                }
            }
        }

        private static void AddDialogueNode(JObject dlg_obj, RichNode parentNode)
        {
            if (dlg_obj == null)
                return;
            string txt = dlg_obj["txt"]?.ToString();
            string chr = dlg_obj["chr"]?.ToString();
            int id = int.Parse(dlg_obj["id"].ToString());
            RichNode dlgNode = null;
            if (!string.IsNullOrEmpty(txt))
            {
                dlgNode = new RichNode(txt);
                if (dlg_obj.ContainsKey("opt"))
                    dlgNode.ForeColor = ThemeColor.Option;
                dlgNode.chr = chr;
                dlgNode.txt = txt;
                dlgNode.id = id;
                parentNode.Nodes.Add(dlgNode);
            }
            if (dlg_obj.ContainsKey("act"))
            {
                JObject actObject = dlg_obj["act"] as JObject;
                if (actObject != null)
                {
                    if (actObject.ContainsKey("bgm"))
                    {
                        string bgm = actObject["bgm"].ToString();
                        RichNode bgmNode = new RichNode("🎵" + bgm);
                        Method.Music(bgm);
                        bgmNode.BackColor = ThemeColor.Action;
                        (dlgNode ?? parentNode).Nodes.Add(bgmNode);
                    }

                    if (actObject.ContainsKey("fun"))
                    {
                        string fun = actObject["fun"].ToString();
                        RichNode funNode = new RichNode("⚡" + fun);
                        funNode.BackColor = ThemeColor.Action;
                        (dlgNode ?? parentNode).Nodes.Add(funNode);
                    }
                }
            }
            if (dlg_obj.ContainsKey("opt"))
            {
                JObject optObject = dlg_obj["opt"] as JObject;
                if (optObject != null)
                {
                    foreach (var optProperty in optObject.Properties())
                    {
                        RichNode optNode = new RichNode(optProperty.Name);
                        optNode.BackColor = ThemeColor.Option;
                        optNode.id = id;
                        (dlgNode ?? parentNode).Nodes.Add(optNode);//为空返回parentnode
                        JArray optArray = optProperty.Value as JArray;
                        if (optArray != null)
                        {
                            foreach (var opt_dlg in optArray)
                            {
                                AddDialogueNode(opt_dlg as JObject, optNode);
                            }
                        }
                    }
                }
            }
        }


        #endregion



        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void opt_Click(object sender, EventArgs e)
        {

        }

        private void act_Click(object sender, EventArgs e)
        {

        }

        private void txt_edit_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void txt_edit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (CurrentNode.txt == null)
                    return;
                txt_edit.Text = txt_edit.Text.Replace("\n","");
                CurrentNode.txt = txt_edit.Text;
                CurrentNode.Text = Map.ChrMap[int.Parse(CurrentNode.chr)] + "：" + CurrentNode.txt;
                EditSingleKey((JObject)JsonSource,CurrentScene,CurrentNode.id.ToString(),"txt",txt_edit.Text);
                File.WriteAllText(DataFilePath,JsonSource.ToString());
            }
        }

        public static void EditSingleKey(JObject obj, string scene,string dialogID, string keyType,JToken newValue,bool _is_root=true)
        {
            if (obj.TryGetValue(dialogID, out JToken token))
            {
                if(newValue.ToString()==""&&keyType=="txt")//处理删除操作
                {
                    if(Method.Warn("这将删除所有子节点 请务必谨慎操作！！！"))
                    {
                        obj.Remove(dialogID);
                        return;
                    }
                }
                obj[dialogID][keyType] = newValue;
                return;
            }
            if (_is_root)
            {
                _is_root = false;//最外层就有选项
                foreach (var prop in ((JObject)obj[scene]).Properties())//第一层也就是主线对话 需要直接处理
                {
                    if(prop.Name==dialogID)
                    {
                        JObject _j=(JObject)prop.Value;   
                        _j[keyType] = newValue;
                        prop.Value = _j;
                        return;
                    }
                    if (prop.Value.Type == JTokenType.Object)
                    {
                        EditSingleKey((JObject)prop.Value, scene,dialogID, keyType,newValue,_is_root);
                    }
                }
            }
            else
            {
                foreach (var prop in obj.Properties())//已经进入内层
                {
                    if (prop.Value.Type == JTokenType.Object)
                    {
                        EditSingleKey((JObject)prop.Value, scene, dialogID, keyType, newValue, _is_root);
                    }
                }
            }
        }
        public static void EditOptObject(JObject obj, string scene, string dialogID, string keyType, string keyName, JToken newKeyName, bool _is_root = true)
        {
            if (obj.TryGetValue(dialogID, out JToken token))
            {
                if (newKeyName.ToString() == "" && keyType == "txt")//处理删除操作
                {
                    if (Method.Warn("这将删除所有子节点 请务必谨慎操作！！！"))
                    {
                        obj.Remove(dialogID);
                        return;
                    }
                }
                obj[dialogID][keyType] = newKeyName;
                return;
            }
            if (_is_root)//最外层就有选项
            {
                _is_root = false;//标记下次进入内层遍历
                foreach (var prop in ((JObject)obj[scene]).Properties())//第一层也就是主线对话 需要直接处理
                {
                    if (prop.Name == dialogID)//选项改名
                    {
                        JObject _j = (JObject)prop.Value[keyType];
                        JObject _t= (JObject)_j[keyName];
                        _j.Remove(keyName);
                        _j.Add(newKeyName.ToString(), _t);
                        prop.Value[keyType] = _j;
                        return;
                    }
                    if (prop.Value.Type == JTokenType.Object)
                    {
                        EditOptObject((JObject)prop.Value, scene, dialogID, keyType, keyName,newKeyName, _is_root);
                    }
                }
            }
            else
            {
                foreach (var prop in obj.Properties())//已经进入场景层
                {
                    if (prop.Value.Type == JTokenType.Object)
                    {
                        EditOptObject((JObject)prop.Value, scene, dialogID, keyType, keyName, newKeyName, _is_root);
                    }
                }
            }
        }

        private void chr_edit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (CurrentNode.chr == null)
                    return;
                if(chr_edit.Text=="")
                {
                    Method.Error("角色节点禁止为空！！！");
                    chr_edit.Text = "0";
                }
                chr_edit.Text = chr_edit.Text.Replace("\n", "");
                CurrentNode.chr = chr_edit.Text;
                CurrentNode.Text = Map.ChrMap[int.Parse(CurrentNode.chr)] + "：" + CurrentNode.txt;
                EditSingleKey((JObject)JsonSource, CurrentScene, CurrentNode.id.ToString(), "chr", chr_edit.Text);
                File.WriteAllText(DataFilePath, JsonSource.ToString());
            }
        }

        private void opt_edit_KeyDown(object sender, KeyEventArgs e)
        {
            if(CurrentNode.opt==null)
                return;
            if (e.KeyCode == Keys.Enter)
            {
                if (opt_edit.Text == "")
                {
                    Method.Error("选项节点禁止为空！！！");
                    return;
                }
                CurrentNode.Text = opt_edit.Text;
                EditOptObject((JObject)JsonSource, CurrentScene, CurrentNode.id.ToString(), "opt",CurrentNode.opt,opt_edit.Text);
                CurrentNode.opt= opt_edit.Text;
                File.WriteAllText(DataFilePath, JsonSource.ToString());
            }
        }

        private void id_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (CurrentNode == null)
                return;
            else if (CurrentNode.scene_pgrs == null)
            {
                if(pgrs_slc.Value!=0)
                    Method.Error("只能在场景节点设置进度！");
                return;
            }
            CurrentNode.scene_pgrs=pgrs_slc.Value.ToString();
        }

        private void opt_edit_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
