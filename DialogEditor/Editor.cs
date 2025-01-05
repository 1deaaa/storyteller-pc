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
using static DialogSystem.Manager;
using System.Reflection.Emit;
using System.Security.Cryptography;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using DialogEditor;

namespace DialogSystem
{
    public partial class Editor : Form
    {
        RichNode CurrentNode;//当前选中节点
        RichNode _last_slc;//上一个选中节点
        public static string CurrentScene = "";
        public static int NewId = -1;
        public static int CurrentId = -1;
        int crt_chr = 0;
        int _option_id;//记录选项所属父级id
        bool _is_loading = true;//用于防止使用text_changed事件时在初始化阶段触发加入历史记录
        string empty_default = "未填写文本";//json解析时会忽略空字符串
        #region 搜索和UI相关
        public Editor()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;//禁用多线程报错
            string path = null;
            if (File.Exists("cfg"))
                path = File.ReadAllText("cfg");
            if (!string.IsNullOrEmpty(path))
            {
                JsonSource = JArray.Parse(File.ReadAllText(path));
                LoadDialogueToTreeView(view, Manager.JsonSource);
            }
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
            this.KeyPreview = true;
            if (JsonSource != null)
                History.Push((JArray)JsonSource.DeepClone());
            if (_is_loading)
                _is_loading = false;
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
            view.Size = new Size(Width - panel1.Width - panel2.Width - 6, Height - 40);
            search.Height = panel2.Height - search.Height - search.Top;
            panel1.Left = view.Width;
            panel2.Left = panel1.Width + view.Width;
            panel1.Height = view.Height;
            panel1.Height = panel2.Height;
        }
        #endregion
        private void view_AfterSelect(object sender, TreeViewEventArgs e)
        {
            CurrentNode = e.Node as RichNode;
            CurrentScene = CurrentNode.scene;
            if (CurrentNode.chr > -1)
                crt_chr = CurrentNode.chr;
            id.Text = "ID：" + CurrentNode.id.ToString();
            CurrentId = CurrentNode.id;
            chr_edit.Text = CurrentNode.chr.ToString();
            txt_edit.Text = CurrentNode.txt;
            opt_edit.Text = CurrentNode.opt;
            cap_edit.Text = CurrentNode.scene_cap;
            if(CurrentNode.NodeType==NodeType.Scene)
                pgrs_slc.Value = Convert.ToDecimal(CurrentNode.scene_pgrs);
            next_edit.Text = CurrentNode.next;
            if (CurrentNode.scene_cap != null)
                scene_name.Text = CurrentScene;
            else
                scene_name.Text = "";
        }
        #region 加载树状图
        public void LoadDialogueToTreeView(TreeView treeView, JArray src)
        {
            treeView.Nodes.Clear();
            foreach (var scene_obj in src)
            {
                RichNode sceneNode = new RichNode(scene_obj["scene"].ToString());
                CurrentScene = scene_obj["scene"].ToString();
                sceneNode.scene = CurrentScene;
                sceneNode.scene_cap = scene_obj["cap"]?.ToString();
                sceneNode.scene_pgrs = scene_obj["pgrs"]?.ToString();
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
                    dlgNode.NodeType = NodeType.DlgWithOpt;
                else//普通对话节点
                    dlgNode.NodeType = NodeType.Dialog;
                dlgNode.chr = chr;
                dlgNode.txt = txt;
                dlgNode.id = id;
                dlgNode.scene = CurrentScene;
                dlgNode.Text = Map.ChrMap[chr] + "：" + dlgNode.txt;
                parentNode.Nodes.Add(dlgNode);
            }
            if (dlg_obj.ContainsKey("opt"))//带选项对话
            {
                JArray optArray = dlg_obj["opt"] as JArray;
                if (optArray != null)
                {
                    foreach (JObject optObject in optArray)
                    {
                        string optName = optObject["optn"]?.ToString();
                        RichNode optItem = new RichNode("🚩" + optName);
                        optItem.opt = optName;
                        optItem.NodeType = NodeType.OptItem;
                        optItem.id = id; // 设置选项节点ID
                        optItem.scene = CurrentScene;
                        (dlgNode ?? parentNode).Nodes.Add(optItem); // 为空返回parentnode
                        JArray optDlgArray = optObject["dia"] as JArray;
                        if (optDlgArray != null)
                        {
                            foreach (var opt_dlg in optDlgArray)
                            {
                                AddDialogueNode(opt_dlg as JObject, optItem);
                            }
                        }
                    }
                }
            }
            if (dlg_obj.ContainsKey("act"))
            {
                JObject actObject = dlg_obj["act"] as JObject;
                if (actObject != null)
                {
                    foreach (var prop in actObject.Properties())
                    {
                        RichNode richNode = new RichNode("⚡" + prop.Name);
                        richNode.id = id;
                        richNode.NodeType = NodeType.ActItem;
                        richNode.scene = CurrentScene;
                        richNode.act_fun = prop.Name;
                        richNode._act_args = prop.Value.ToString();
                        (dlgNode ?? parentNode).Nodes.Add(richNode);
                    }
                }
            }
        }




        #endregion
        #region 编辑节点
        private JObject FindDialogue(string scene, int id)
        {
            JObject tar_obj = Manager.GetSceneObj(scene);
            return (JObject)_FindDialogueById(tar_obj["dia"], id);
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

            History.Push((JArray)JsonSource.DeepClone());
        }
        private void EditDlgChr(string scene, int id, int newCharacter)
        {
            var dialogue = FindDialogue(scene, id);
            if (dialogue != null)
            {
                dialogue["chr"] = newCharacter;
            }

            History.Push((JArray)JsonSource.DeepClone());
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

            History.Push((JArray)JsonSource.DeepClone());
        }




        private void DeleteDialogue(string scene, int id)
        {
            var sceneData = Manager.GetSceneObj(scene);
            if (sceneData != null)
            {
                _DeleteDialogueById(sceneData["dia"], id);
            }

            History.Push((JArray)JsonSource.DeepClone());
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

            History.Push((JArray)JsonSource.DeepClone());
        }


        private void AddDialogue(string scene, int prevId, string newText, int newCharacter)
        {
            if (prevId < 1)//场景节点
            {
                JArray _j = (JArray)GetSceneObj(scene)["dia"];
                _j.Add(new JObject
                {
                    ["id"] = ++NewId,
                    ["chr"] = newCharacter,
                    ["txt"] = newText
                });
                History.Push((JArray)JsonSource.DeepClone());
                return;
            }
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
                    dialogues.Insert(dialogues.IndexOf(prevDialogue) + 1, newDialogue);
                }
            }
            History.Push((JArray)JsonSource.DeepClone());
        }


        private void AddOption(string scene, int id, string optionName)
        {
            var dialogue = FindDialogue(scene, id);
            if (dialogue != null)
            {
                if (dialogue["opt"] == null)
                {
                    dialogue["opt"] = new JArray();
                }
                var options = dialogue["opt"] as JArray;
                var dia = new JArray();
                var diaObj = new JObject
                {
                    ["id"] = ++NewId,
                    ["chr"] = 0,
                    ["txt"] = empty_default
                };
                dia.Add(diaObj);
                var newOption = new JObject
                {
                    ["optn"] = optionName,
                    ["dia"] = dia
                };
                options.Add(newOption);
            }

            History.Push((JArray)JsonSource.DeepClone());
        }


        private void EditScene(string scene, string newName)
        {
            var sceneData = GetSceneObj(scene);
            sceneData["scene"] = newName;

            History.Push((JArray)JsonSource.DeepClone());
        }
        private void RefreshTree(JArray src)
        {
            int _id = CurrentNode.id;
            view.Nodes.Clear();
            LoadDialogueToTreeView(view, src);
            RichNode richNode = GetDlgNode(view.Nodes, _id);
            if (richNode == null)
                CurrentNode = view.Nodes[0] as RichNode;
            else
                CurrentNode = richNode;
            CurrentNode.EnsureVisible();
            CurrentNode.ExpandAll();
        }
        public static RichNode GetDlgNode(TreeNodeCollection nodes, int id)
        {
            foreach (TreeNode node in nodes)
            {
                if (node is RichNode richNode)
                {
                    if (richNode.id == id)
                        return richNode;

                    if (richNode.Nodes.Count > 0)
                    {
                        RichNode foundNode = GetDlgNode(richNode.Nodes, id);
                        if (foundNode != null)
                            return foundNode;
                    }
                }
            }
            return null;
        }
        private void NewOrEditNext(string scene, int id, string direct = null)
        {
            var obj = FindDialogue(scene, id);
            if (direct == null)
                obj["next"] = CurrentScene;
            else
                obj["next"] = direct;

            History.Push((JArray)JsonSource.DeepClone());
        }
        private void AddAct(string scene, int id, string fun, string args)
        {
            var obj = FindDialogue(scene, id);
            if (!obj.ContainsKey("act"))
                obj["act"] = new JObject();
            JObject s = (JObject)obj["act"];
            s.Add(new JProperty(fun, args));
            History.Push((JArray)JsonSource.DeepClone());
        }
        private void EditAct(string scene, int id, string fun, string args)
        {
            var obj = FindDialogue(scene, id);
            if (!obj.ContainsKey("act"))
                return;
            JObject s = (JObject)obj["act"];
            s[fun] = args;
            History.Push((JArray)JsonSource.DeepClone());
        }
        void DeleteNode()
        {
            if (CurrentNode.txt != null)//对话节点
            {
                if (CurrentNode.NodeType == NodeType.DlgWithOpt)//有子节点则进行二次确认
                {
                    if (Method.Warn("这将删除所对话节点下所有内容 务必谨慎操作！！！"))
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
            else if (CurrentNode.NodeType == NodeType.OptItem)//选项节点
            {
                if (Method.Warn("这将删除选项下所有对话 务必谨慎操作！！！"))
                {
                    DeleteOption(CurrentScene, CurrentId, CurrentNode.opt);
                    if (CurrentNode.Parent.Nodes.Count == 1)//全部移除 还原父节点
                    {
                        RichNode rn = (RichNode)CurrentNode.Parent;
                        rn.NodeType = NodeType.Dialog;
                    }
                    CurrentNode.Remove();
                }
            }
            else if (CurrentNode.NodeType == NodeType.Scene)//场景节点
            {
                if(view.Nodes.Count==1)
                {
                    Method.Inf("至少要有一个场景节点");
                    return;
                }
                if (Method.Warn("这将删除场景下所有对话 务必谨慎操作！！！"))
                {
                    JsonSource.Remove(GetSceneObj(CurrentScene));

                    History.Push((JArray)JsonSource.DeepClone());
                    CurrentNode.Remove();
                }
            }
            else if (CurrentNode.NodeType == NodeType.ActItem)
            {
                JObject _j = (JObject)FindDialogue(CurrentScene, CurrentId)["act"];
                _j.Remove(CurrentNode.act_fun);

                History.Push((JArray)JsonSource.DeepClone());
                CurrentNode.Remove();
            }
            else if (CurrentNode.next != null)
            {
                JObject _j = (JObject)FindDialogue(CurrentScene, CurrentId)["next"];
                _j.Remove();

                History.Push((JArray)JsonSource.DeepClone());
                CurrentNode.Remove();
            }
        }
        private void SaveJson()
        {
            if (JsonSource != null)
                File.WriteAllText(Manager.DataFilePath, Manager.JsonSource.ToString());
        }

        #endregion

        private void new_opt_Click(object sender, EventArgs e)
        {
            if (CurrentNode.txt != null)//只在dlg下创建选项
            {
                AddOption(CurrentScene, CurrentId, empty_default);
                CurrentNode.NodeType = NodeType.DlgWithOpt;
                RichNode richNode = new RichNode("🚩" + "新选项");
                richNode.id = CurrentId;
                richNode.scene = CurrentScene;
                richNode.opt = "新选项";
                richNode.NodeType = NodeType.OptItem;
                RichNode _dlg = new RichNode(empty_default);
                _dlg.NodeType = NodeType.Dialog;
                _dlg.id = NewId;
                _dlg.scene = CurrentScene;
                _dlg.txt = empty_default;
                _dlg.chr = crt_chr;
                CurrentNode.Nodes.Add(richNode);
                richNode.Nodes.Add(_dlg);
                view.SelectedNode = _dlg;
                _dlg.EnsureVisible();
            }
        }

        private void act_Click(object sender, EventArgs e)
        {
            if (CurrentNode.NodeType == NodeType.Dialog)
            {
                new ActEditor().ShowDialog();
                if (ActEditor.args == null || ActEditor.fun == null)
                {
                    Method.Error("用户取消编辑");
                    return;
                }
                AddAct(CurrentScene, CurrentId, ActEditor.fun, ActEditor.args);
                RichNode rn = new RichNode("⚡" + ActEditor.fun);
                rn.NodeType = NodeType.ActItem;
                rn.id = CurrentId;
                rn.scene = CurrentScene;
                rn.act_fun = ActEditor.fun;
                rn._act_args = ActEditor.args;
                CurrentNode.Nodes.Add(rn);
            }
            else if (CurrentNode.NodeType == NodeType.ActItem)
            {
                new ActEditor(true, CurrentNode._act_args).ShowDialog();
                if (ActEditor.args == null)
                {
                    Method.Error("用户取消编辑");
                    return;
                }
                EditAct(CurrentScene, CurrentId, CurrentNode.act_fun, ActEditor.args);
                CurrentNode._act_args = ActEditor.args;
            }

        }

        private void txt_edit_TextChanged(object sender, EventArgs e)
        {
            txt_edit.Text = txt_edit.Text.Replace("\n", "");
        }

        private void txt_edit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;
            if (CurrentNode.txt == null)
            {
                txt_edit.Text = "";
                return;
            }
            CurrentNode.txt = txt_edit.Text.Replace("\n", "");
            CurrentNode.Text = Map.ChrMap[CurrentNode.chr] + "：" + CurrentNode.txt;
            EditDlgTxt(CurrentScene, CurrentId, CurrentNode.txt);
        }



        private void chr_edit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;
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
            catch
            {
                Method.Error("角色ID不存在！！！");
                chr_edit.Text = "0";
                CurrentNode.chr = 0;
            }
            EditDlgChr(CurrentScene, CurrentId, CurrentNode.chr);
        }

        private void opt_edit_KeyDown(object sender, KeyEventArgs e)
        {
            if (CurrentNode.opt == null)
                return;
            if (e.KeyCode != Keys.Enter)
                return;
            if (opt_edit.Text == "")
            {
                Method.Error("选项节点禁止为空！！！");
                return;
            }
            CurrentNode.Text = "🚩" + opt_edit.Text;
            EditOptName(CurrentScene, CurrentId, CurrentNode.opt, opt_edit.Text);
            CurrentNode.opt = opt_edit.Text;
        }


        private void pgrs_changed(object sender, EventArgs e)
        {
            if (CurrentNode == null)
                return;
            else if (CurrentNode.scene_pgrs == null)
            {
                if (pgrs_slc.Value != 0)
                    Method.Error("只能在场景节点设置进度！");
                return;
            }
            CurrentNode.scene_pgrs = pgrs_slc.Value.ToString();
            GetSceneObj(CurrentScene)["pgrs"] = pgrs_slc.Value;

            History.Push((JArray)JsonSource.DeepClone());
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Method.Inf(JsonSource.ToString());
        }

        private void new_dia_Click(object sender, EventArgs e)
        {
            if (CurrentNode.NodeType == NodeType.ActItem && CurrentNode.NodeType == NodeType.Next)
                return;
            RichNode rn = new RichNode(empty_default);
            rn.chr = crt_chr;
            rn.txt = empty_default;
            rn.scene = CurrentScene;
            rn.NodeType = NodeType.Dialog;
            switch (CurrentNode.NodeType)
            {
                case NodeType.Dialog:
                    AddDialogue(CurrentScene, CurrentId, empty_default, 0);
                    rn.id = NewId;//Add dlg会改变newid 所以必须在其后设置
                    CurrentNode.Parent.Nodes.Insert(CurrentNode.Index + 1, rn);
                    break;
                case NodeType.OptItem:
                case NodeType.Scene:
                    RichNode richNode;
                    if (CurrentNode.Nodes.Count - 1 > -1)
                    {
                        richNode = (RichNode)CurrentNode.Nodes[CurrentNode.Nodes.Count - 1];
                        AddDialogue(CurrentScene, richNode.id, empty_default, 0);
                        rn.id = NewId;
                        CurrentNode.Nodes.Add(rn);
                    }
                    else
                    {
                        AddDialogue(CurrentScene, 0, empty_default, 0);
                        rn.id = NewId;
                        CurrentNode.Nodes.Add(rn);
                    }
                    break;
            }
            view.SelectedNode = rn;
            txt_edit.Focus();
        }

        private void cap_edit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;
            if (CurrentNode.scene == null)
                return;
            if (cap_edit.Text == "")
            {
                Method.Error("角色节点禁止为空！！！");
                cap_edit.Text = "场景";
            }
            CurrentNode.scene_cap = cap_edit.Text;
            var scene = GetSceneObj(CurrentScene);
            scene["cap"] = cap_edit.Text;
        }

        private void 删除节点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteNode();
        }

        private void view_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // 获取鼠标位置下的节点
                TreeNode node = view.GetNodeAt(e.X, e.Y);
                view.SelectedNode = node;
                if (node != null)
                {
                    if (CurrentNode.NodeType == NodeType.Scene)
                        sceneMenu.Show(view, e.Location);
                    else if (CurrentNode.txt != null)
                        dlgMenu.Show(view, e.Location);
                    else if (true)
                        delMenu.Show(view, e.Location);
                    // 显示右键菜单
                }
            }
        }

        private void Editor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Z)
            {
                // 执行撤销操作
                if (Manager.History.Count > 1)
                    Manager.History.Pop();
                else
                    return;
                RefreshTree(Manager.History.Peek());
                e.Handled = true; // 防止事件继续传播
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Method.Inf(JsonSource.ToString());
        }

        private void scene_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;
            if (CurrentNode.scene_cap == null)
                return;
            var scene = GetSceneObj(CurrentScene);
            scene["scene"] = scene_name.Text;
            CurrentNode.Text = scene_name.Text;
            History.Push((JArray)JsonSource.DeepClone());
        }

        private void new_scene_Click(object sender, EventArgs e)
        {
            var scene = new JObject
            {
                ["scene"] = "新场景",
                ["cap"] = "新任务指引",
                ["pgrs"] = 0,
                ["dia"] = new JArray()
            };
            RichNode richNode = new RichNode("新场景");
            richNode.scene = "新场景";
            richNode.scene_cap = "新任务指引";
            richNode.scene_pgrs = "0";
            richNode.NodeType = NodeType.Scene;
            richNode.id = -1;
            if (CurrentNode==null)
            {
                NewId = 10000;
                JsonSource.Add(scene);
                view.Nodes.Add(richNode);
            }
           else if (CurrentNode.NodeType != NodeType.Scene)
                return;
            else
            {
                JsonSource.Insert(JsonSource.IndexOf(GetSceneObj(CurrentScene)), scene);
                view.Nodes.Insert(view.Nodes.IndexOf(CurrentNode) + 1, richNode);
            }
            view.SelectedNode = richNode;
            richNode.EnsureVisible();
        }

        private void new_next_Click(object sender, EventArgs e)
        {
            if (CurrentNode.txt == null || CurrentNode.next != null)
                return;
            NewOrEditNext(CurrentScene, CurrentId);
            RichNode richNode = new RichNode("🚀" + CurrentNode.scene) { NodeType = NodeType.Next };
            richNode.next = CurrentNode.scene;
            richNode.scene = CurrentScene;
            richNode.id = CurrentId;
            CurrentNode.Nodes.Add(richNode);
            richNode.EnsureVisible();
        }

        private void next_edit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;
            if (CurrentNode.next == null)
                return;
            List<string> scenes = new List<string>();
            foreach (var scene in JsonSource)
            {
                scenes.Add(scene["scene"].ToString());
            }
            if (!scenes.Contains(next_edit.Text))
            {
                Method.Error("不存在的场景");
                next_edit.ForeColor = Color.Red;
                return;
            }
            NewOrEditNext(CurrentScene, CurrentId, next_edit.Text);
            CurrentNode.next = next_edit.Text;
            CurrentNode.Text = "🚀" + CurrentNode.next;
            next_edit.ForeColor = Color.Black;
        }

        private void 创建对话ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new_dia_Click(sender, e);
        }

        private void 创建场景ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new_scene_Click(sender, e);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DeleteNode();
        }

        private void 创建对话ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new_dia_Click(sender, e);
        }

        private void 创建选项ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new_opt_Click(sender, e);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            DeleteNode();
        }


        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("是否在新建前保存当前对话？", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                保存ToolStripMenuItem_Click(sender, e);
            }
            else if (result == DialogResult.Cancel)
            {
                return; // 用户取消操作，直接返回
            }
            ReLoad();
        }
        private void ReLoad()
        {
            DataFilePath = null;
            // 清空当前对话树
            CurrentNode = null;
            _last_slc = null;
            CurrentScene = null;
            NewId = 10000;
            CurrentId = -1;
            crt_chr = 0;
            _option_id = -1;
            _is_loading = false;
            // 清空TreeView
            view.Nodes.Clear();
            // 清空对话数据
            JsonSource = new JArray();
            History.Clear();
            new_scene_Click(null, null);
        }
        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "JSON数据 (*.json)|*.json|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    DataFilePath = openFileDialog.FileName;
                    JsonSource = JArray.Parse(File.ReadAllText(DataFilePath));
                    LoadDialogueToTreeView(view, JsonSource);
                }
            }
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(DataFilePath))
            {
                另存为ToolStripMenuItem_Click(sender, e);
            }
            else
            {
                SaveJson();
            }
        }

        private void 另存为ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "JSON数据 (*.json)|*.json|All files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    DataFilePath = saveFileDialog.FileName;
                    SaveJson();
                }
            }
        }

        private void 创建行为ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            act_Click(sender, e);
        }

        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            contextMenuStrip1.Show(button2, e.Location);
        }

        private void Editor_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            if (!File.Exists(DataFilePath))//空文件
            {
                switch (Method.Ask("新文档尚未保存 是否保存"))
                {
                    case DialogResult.Yes:
                        另存为ToolStripMenuItem_Click(sender, e);
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true; // 取消关闭操作
                        return;
                }
                return;
            }

            if (!JToken.DeepEquals(JsonSource, JArray.Parse(File.ReadAllText(DataFilePath))))//未保存
            {
                switch (Method.Ask("数据已更改 是否保存"))
                {
                    case DialogResult.Yes:
                        保存ToolStripMenuItem_Click(sender, e);
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true; // 取消关闭操作
                        return;
                }
            }
            File.WriteAllText("cfg", DataFilePath);
        }
    }
}
