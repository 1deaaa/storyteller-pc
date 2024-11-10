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
using static DialogSystem.Map;

namespace DialogSystem
{
    public partial class Editor : Form
    {
        RichNode CurrentNode;//当前选中节点
        RichNode _last_slc;//上一个选中节点
        public static string CurrentScene = "";
        public static int NewId=-1;
        public static int CurrentId = -1;
        int crt_chr = 0;
        int _option_id;//记录选项所属父级id
        #region 搜索和UI相关
        public Editor()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;//禁用多线程报错
            LoadDialogueToTreeView(view,Manager.JsonSource);
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
        int prev_obj_index = 0;//在编辑数组成员时 记录上一个对象的索引
        #region 弃用
        //public static void EditSingleKey(JObject obj, string scene, string dialogID, string keyType, JToken newValue, bool _is_root = true)
        //{
        //    if (obj.TryGetValue(dialogID, out JToken token))
        //    {
        //        if (newValue.ToString() == "" && keyType == "txt")//处理删除操作
        //        {
        //            if (Method.Warn("这将删除所有子节点 请务必谨慎操作！！！"))
        //            {
        //                obj.Remove(dialogID);
        //                return;
        //            }
        //        }
        //        obj[dialogID][keyType] = newValue;
        //        return;
        //    }
        //    if (_is_root)
        //    {
        //        _is_root = false;//最外层就有选项
        //        foreach (var prop in ((JObject)obj[scene]).Properties())//第一层也就是主线对话 需要直接处理
        //        {
        //            if (prop.Name == dialogID)
        //            {
        //                JObject _j = (JObject)prop.Value;
        //                _j[keyType] = newValue;
        //                prop.Value = _j;
        //                return;
        //            }
        //            if (prop.Value.Type == JTokenType.Object)
        //            {
        //                EditSingleKey((JObject)prop.Value, scene, dialogID, keyType, newValue, _is_root);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        foreach (var prop in obj.Properties())//已经进入内层
        //        {
        //            if (prop.Value.Type == JTokenType.Object)
        //            {
        //                EditSingleKey((JObject)prop.Value, scene, dialogID, keyType, newValue, _is_root);
        //            }
        //        }
        //    }
        //}


        //public static void EditOptObject(JObject obj, string scene, string dialogID, string keyType, string keyName, JToken newKeyName, bool _is_root = true)
        //{
        //    if (obj.TryGetValue(dialogID, out JToken token))
        //    {
        //        if (newKeyName.ToString() == "" && keyType == "txt")//处理删除操作
        //        {
        //            if (Method.Warn("这将删除所有子节点 请务必谨慎操作！！！"))
        //            {
        //                obj.Remove(dialogID);
        //                return;
        //            }
        //        }
        //        obj[dialogID][keyType] = newKeyName;
        //        return;
        //    }
        //    if (_is_root)//最外层就有选项
        //    {
        //        _is_root = false;//标记下次进入内层遍历
        //        foreach (var prop in ((JObject)obj[scene]).Properties())//第一层也就是主线对话 需要直接处理
        //        {
        //            if (prop.Name == dialogID)//选项改名
        //            {
        //                JObject _j = (JObject)prop.Value[keyType];
        //                JObject _t = (JObject)_j[keyName];
        //                _j.Remove(keyName);
        //                _j.Add(newKeyName.ToString(), _t);
        //                prop.Value[keyType] = _j;
        //                return;
        //            }
        //            if (prop.Value.Type == JTokenType.Object)
        //            {
        //                EditOptObject((JObject)prop.Value, scene, dialogID, keyType, keyName, newKeyName, _is_root);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        foreach (var prop in obj.Properties())//已经进入场景层
        //        {
        //            if (prop.Value.Type == JTokenType.Object)
        //            {
        //                EditOptObject((JObject)prop.Value, scene, dialogID, keyType, keyName, newKeyName, _is_root);
        //            }
        //        }
        //    }
        //}
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
            if(CurrentNode.chr>-1)
                crt_chr = CurrentNode.chr;
            id.Text = "ID：" + CurrentNode.id.ToString();
            CurrentId= CurrentNode.id;
            chr_edit.Text = CurrentNode.chr.ToString();
            txt_edit.Text = CurrentNode.txt;
            opt_edit.Text = CurrentNode.opt;
            cap_edit.Text = CurrentNode.scene_cap;
            pgrs_slc.Value = Convert.ToInt32(CurrentNode.scene_pgrs);
            CurrentScene = CurrentNode.scene;
        }
        #region AI生成实验
        public void LoadDialogueToTreeView(TreeView treeView,JArray src)
        {
            foreach (var scene_obj in src)
            {
                RichNode sceneNode = new RichNode(scene_obj["scene"].ToString());
                CurrentScene= scene_obj["scene"].ToString();
                sceneNode.scene = CurrentScene;
                sceneNode.scene_cap = scene_obj["cap"]?.ToString();
                sceneNode.NodeType = NodeType.Scene;
                treeView.Nodes.Add(sceneNode);
                JObject dialogueObject = scene_obj as JObject;
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

        private void AddDialogueNode(JObject dlg_obj, RichNode parentNode)
        {
            if (dlg_obj == null)
                return;
            string txt = dlg_obj["txt"]?.ToString();
            int chr = int.Parse(dlg_obj["chr"]?.ToString());
            int id = int.Parse(dlg_obj["id"].ToString());
            if (id > NewId)
            {
                NewId = id;
                NewId++;
            }
            RichNode dlgNode = null;
            if (!string.IsNullOrEmpty(txt))
            {
                dlgNode = new RichNode(txt);
                if (dlg_obj.ContainsKey("opt"))//带opt的对话节点
                {
                    dlgNode.ForeColor = ThemeColor.Option;
                    dlgNode.NodeType = NodeType.DlgWithOpt;
                }
                dlgNode.chr = chr;
                dlgNode.txt = txt;
                dlgNode.id = id;
                dlgNode.scene = CurrentScene;
                dlgNode.Text = Map.ChrMap[chr] + "：" + dlgNode.txt;
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
                        bgmNode.id = id;
                        bgmNode.scene = CurrentScene;
                        Method.Music(bgm);
                        bgmNode.BackColor = ThemeColor.Action;
                        (dlgNode ?? parentNode).Nodes.Add(bgmNode);
                    }

                    if (actObject.ContainsKey("fun"))
                    {
                        string fun = actObject["fun"].ToString();
                        RichNode funNode = new RichNode("⚡" + fun);
                        funNode.id = id;
                        funNode.scene = CurrentScene;
                        funNode.BackColor = ThemeColor.Action;
                        (dlgNode ?? parentNode).Nodes.Add(funNode);
                    }
                }
            }
            if (dlg_obj.ContainsKey("opt"))
            {
                JArray optArray = dlg_obj["opt"] as JArray;
                if (optArray != null)
                {
                    foreach (JObject optObject in optArray)
                    {
                        string optName = optObject["optn"]?.ToString();
                        RichNode optNode = new RichNode(optName);
                        optNode.opt = optName;
                        optNode.BackColor = ThemeColor.Option;
                        optNode.id = id; // 设置选项节点ID
                        optNode.scene = CurrentScene;
                        (dlgNode ?? parentNode).Nodes.Add(optNode); // 为空返回parentnode
                        JArray optDlgArray = optObject["dia"] as JArray;
                        if (optDlgArray != null)
                        {
                            foreach (var opt_dlg in optDlgArray)
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
            if (CurrentNode.txt == null)
                return;
            txt_edit.Text = txt_edit.Text.Replace("\n", "");
            CurrentNode.txt = txt_edit.Text;
            CurrentNode.Text = Map.ChrMap[CurrentNode.chr] + "：" + CurrentNode.txt;
            EditDlgTxt(CurrentScene, CurrentId, CurrentNode.txt);
        }

        private void txt_edit_KeyDown(object sender, KeyEventArgs e)
        {

        }

        #region AI
        private JToken FindDialogue(string scene, int id)
        {
            JObject tar_obj=Manager.GetSceneObj(scene);
            return _FindDialogueById(tar_obj["dia"], id);
        }

        private JToken _FindDialogueById(JToken dialogues, int id)
        {
            foreach (var dialogue in dialogues)
            {
                if (dialogue["id"].Value<int>() == id)
                {
                    return dialogue;
                }
                if (dialogue["opt"] != null)
                {
                    foreach (var option in dialogue["opt"])
                    {
                        var result = _FindDialogueById(option["dia"], id);
                        if (result != null)
                        {
                            return result;
                        }
                    }
                }
            }
            return null;
        }

        private void EditDlgTxt(string scene, int id, string newText)
        {
            var dialogue = FindDialogue(scene, id);
            if (dialogue != null)
            {
                dialogue["txt"] = newText;
            }
        }
        private void EditDlgChr(string scene, int id, int newCharacter)
        {
            var dialogue = FindDialogue(scene, id);
            if (dialogue != null)
            {
                dialogue["chr"] = newCharacter;
            }
        }

        private void EditOptName(string scene, int id, string oldOptionName, string newOptionName)
        {
            var dialogue = FindDialogue(scene, id);
            if (dialogue != null && dialogue["opt"] != null)
            {
                var options = dialogue["opt"] as JArray;
                if (options != null)
                {
                    foreach (var option in options)
                    {
                        if (option["optn"]?.ToString() == oldOptionName)
                        {
                            option["optn"] = newOptionName;
                            break;
                        }
                    }
                }
            }
        }




        private void DeleteDialogue(string scene, int id)
        {
            var sceneData = Manager.GetSceneObj(scene);
            if (sceneData != null)
            {
                _DeleteDialogueById(sceneData["dia"], id);
            }
        }

        private bool _DeleteDialogueById(JToken dialogues, int id)
        {
            for (int i = 0; i < dialogues.Count(); i++)
            {
                var dialogue = dialogues[i];
                if (dialogue["id"].Value<int>() == id)
                {
                    dialogue.Remove();
                    return true;
                }
                if (dialogue["opt"] != null)
                {
                    foreach (var option in dialogue["opt"])
                    {
                        if (_DeleteDialogueById(option["dia"], id))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }


        private void DeleteOption(string scene, int id, string optionName)
        {
            var dialogue = FindDialogue(scene, id);
            if (dialogue != null && dialogue["opt"] != null)
            {
                var options = dialogue["opt"] as JArray;
                if (options != null)
                {
                    var optionToRemove = options.FirstOrDefault(opt => opt["optn"]?.ToString() == optionName);
                    if (optionToRemove != null)
                    {
                        options.Remove(optionToRemove);
                    }
                }
            }
        }


        private void AddDialogue(string scene,int prevId, string newText, int newCharacter)
        {
            var prevDialogue = FindDialogue(scene, prevId);
            if (prevDialogue != null)
            {
                var dialogues = prevDialogue.Parent as JArray;
                if (dialogues != null)
                {
                    JObject newDialogue = new JObject
                    {
                        ["id"] = ++NewId,
                        ["chr"] = newCharacter,
                        ["txt"] = newText
                    };
                    prev_obj_index = dialogues.IndexOf(prevDialogue);
                    dialogues.Insert(prev_obj_index + 1, newDialogue);
                }
            }
        }


        private void AddOption(string scene, int id, string optionName, JArray newOptions)
        {
            var dialogue = FindDialogue(scene, id);
            if (dialogue != null)
            {
                if (dialogue["opt"] == null)
                {
                    dialogue["opt"] = new JArray();
                }
                var options = dialogue["opt"] as JArray;
                var newOption = new JObject
                {
                    ["optn"] = optionName,
                    ["dia"] = newOptions
                };
                options.Add(newOption);
            }
        }


        private void EditScene(string scene,string newName)
        {
            
        }
        private void SaveJson()
        {
            File.WriteAllText(Manager.DataFilePath, Manager.JsonSource.ToString());
        }

        #endregion

        private void chr_edit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (CurrentNode.chr < 0)
                    return;
                if (chr_edit.Text == "")
                {
                    Method.Error("角色节点禁止为空！！！");
                    chr_edit.Text = "0";
                }
                chr_edit.Text = chr_edit.Text.Replace("\n", "");
                CurrentNode.chr = int.Parse(chr_edit.Text);
                try
                {
                    CurrentNode.Text = Map.ChrMap[CurrentNode.chr] + "：" + CurrentNode.txt;
                }
                catch {
                    Method.Error("角色ID不存在！！！");
                    chr_edit.Text = "0";
                    CurrentNode.chr = 0;
                }
                EditDlgChr(CurrentScene, CurrentId, CurrentNode.chr);
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
                EditOptName(CurrentScene, CurrentId, CurrentNode.opt, opt_edit.Text);
                CurrentNode.opt= opt_edit.Text;
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
            Manager.JsonSource[CurrentScene]["pgrs"]= pgrs_slc.Value;
        }

        private void opt_edit_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            Method.Inf(Manager.JsonSource.ToString());
        }

        private void dia_Click(object sender, EventArgs e)
        {
            if (CurrentNode.txt == null)
                return;
            AddDialogue(CurrentScene, CurrentId, "", 0);
            RichNode rn= new RichNode("");
            rn.id = NewId;
            rn.chr = crt_chr;
            rn.txt = "";
            rn.scene = CurrentScene;
            CurrentNode.Parent.Nodes.Insert(prev_obj_index+1,rn);
            view.SelectedNode = rn;
            txt_edit.Focus();
        }

        private void cap_edit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (CurrentNode.scene==null)
                    return;
                if (cap_edit.Text == "")
                {
                    Method.Error("角色节点禁止为空！！！");
                    cap_edit.Text = "场景";
                }
                CurrentNode.scene_cap = cap_edit.Text;
                EditDlgChr(CurrentScene, CurrentId, CurrentNode.chr);
            }
        }

        private void chr_edit_TextChanged(object sender, EventArgs e)
        {

        }

        private void 删除节点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(CurrentNode.txt!=null)
            {
                if (CurrentNode.NodeType == NodeType.DlgWithOpt)//有子节点则进行二次确认
                {
                    if (Method.Warn("这将删除所有节点下所有内容 务必谨慎操作！！！"))
                    {
                        DeleteDialogue(CurrentScene, CurrentId);
                        CurrentNode.Remove();
                    }
                }
                else
                {
                    DeleteDialogue(CurrentScene, CurrentId);
                    CurrentNode.Remove();
                }
            }
        }

        private void view_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // 获取鼠标位置下的节点
                TreeNode node = view.GetNodeAt(e.X, e.Y);
                if (node != null)
                {
                    // 选中该节点
                    contextMenuStrip1.Show(view, e.Location);
                    view.SelectedNode = node;
                    // 显示右键菜单
                }
            }
        }

        private void Editor_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Manager.History.Pop();
            view.Nodes.Clear();
            LoadDialogueToTreeView(view, Manager.History.Peek());
        }
    }
}
