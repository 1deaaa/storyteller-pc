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

namespace DialogSystem
{
    public partial class Editor : Form
    {
        RichNode CurrentNode;//当前选中节点
        RichNode _last_slc;//上一个选中节点
        public static JToken JsonSource = JToken.Parse(File.ReadAllText(@"..\..\..\对话.json"));
        public static string CurrentScene = "";
        public static int CurrentId;
        int _option_id;//记录选项所属父级id
        public Editor()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            LoadTree();
        }

        void LoadTree()
        {
            AddSceneToTreeView(view, (JObject)JsonSource);
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

        private void AddSceneToTreeView(TreeView treeView, JObject jsonObject)
        {
            foreach (var scene in jsonObject.Properties())
            {
                RichNode sceneNode = new RichNode();
                sceneNode.Text = scene.Name;
                CurrentScene = scene.Name;
                treeView.Nodes.Add(sceneNode);
                AddNodeToParent(sceneNode, (JObject)scene.Value);
            }
        }

        private void AddNodeToParent(RichNode parentNode, JObject jsonObject)
        {
            foreach (var key in jsonObject.Properties())
            {
                RichNode node = new RichNode();
                node.Text = "⏬";//默认文本 当连续选项出现时 没有txt节点设置文本
                node.scene = CurrentScene;
                if (key.Value is JObject)//处理分支节点
                {
                    //对话ID节点
                    int _id;
                    if (int.TryParse(key.Name, out _id) && _id > 100)//所以选项名字严禁纯数字！！！
                    {
                        CurrentId = _id;
                        node.id = CurrentId;
                        //处理txt时会给节点设置Text
                    }
                    //
                    else
                    {
                        node.id=CurrentId;
                        switch (key.Name)
                        {
                            case "opt":
                                node.opt = (JObject)key.Value;
                                node.Text = "✨选项";
                                node.ForeColor = ThemeColor.Option;
                                break;
                            case "act":
                                node.act = (JObject)key.Value;
                                node.Text = "⚡行为";
                                node.ForeColor = ThemeColor.Action;
                                break;
                            case "brc":
                                node.Text = "🌿分支";
                                break;
                            default:
                                    node.Text = key.Name;//只有可能是选项名字了
                                    node.BackColor = ThemeColor.Option;
                                break;
                        }
                    }
                       parentNode.Nodes.Add(node);
                       AddNodeToParent(node, (JObject)key.Value);
                }
                else//处理单节点 也就是给父节点贴上属性或者子节点
                { 
                    node.id = CurrentId;
                    switch(key.Name)
                    {
                        case "chr":
                            parentNode.chr=key.Value.ToString();
                            break;
                        case "txt":
                            parentNode.txt=key.Value.ToString();
                            parentNode.Text=Map.ChrMap[int.Parse(parentNode.chr)]+"："+key.Value.ToString();
                            break;
                        case "bgm":
                            node.Text= "🎵" + key.Value.ToString();
                            node.BackColor = ThemeColor.Action;
                            parentNode.Nodes.Add(node);
                            break;
                        case "rcd"://记录当前选项
                            node.Text= "🖊️"+ key.Value.ToString();
                            node.BackColor = ThemeColor.Action;
                            parentNode.Nodes.Add(node);
                            break ;
                        case "fun":
                            node.Text = "⚡" + key.Value.ToString();
                            node.BackColor = ThemeColor.Action;
                            parentNode.Nodes.Add(node);
                            break;               
                    }
                }
            }
        }

        private void view_AfterSelect(object sender, TreeViewEventArgs e)
        {
            CurrentNode=e.Node as RichNode;
            id.Text = "ID：" + CurrentNode.id.ToString();
            chr_edit.Text= CurrentNode.chr;
            txt_edit.Text = CurrentNode.txt;
            CurrentScene= CurrentNode.scene;
            switch(CurrentNode.Text)
            {
                case "选项":
                    break;

            }
        }

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
                CurrentNode.txt = txt_edit.Text;
                CurrentNode.Text = CurrentNode.txt;
                JProperty _j = FindProperty((JObject)JsonSource[CurrentScene],CurrentNode.id.ToString());
                JObject obj = (JObject)_j.Value;
                Method.Error(obj);

            }
        }

        public static JProperty FindProperty(JObject obj, string propertyName)
        {
            // 如果在最外层找到则返回
            if (obj.TryGetValue(propertyName, out JToken token))
            {
                return new JProperty(propertyName, token);
            }
            // 遍历所有子对象进行递归查找
            foreach (var prop in obj.Properties())
            {
                if (prop.Value.Type == JTokenType.Object)
                {
                    var found = FindProperty((JObject)prop.Value, propertyName);
                    if (found != null)
                    {
                        return found;
                    }
                }
            }

            // 如果没有找到任何匹配项，则返回null
            return null;
        }
    }
}
