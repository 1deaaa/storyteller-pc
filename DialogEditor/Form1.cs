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
    public partial class Form1 : Form
    {
        RichNode currentNode;//当前选中节点
        RichNode _last_slc;//上一个选中节点
        public static JToken jsonSource = JToken.Parse(File.ReadAllText(@"..\..\..\对话.json"));
        public static string currentScene = "";
        public static int currentId;
        int _option_id;//记录选项所属父级id
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            LoadTree();
        }

        void LoadTree()
        {
            AddRootsToTreeView(view, (JObject)jsonSource);
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
                    currentNode = FindNodeByText(view.Nodes, selectedOption);
                    if (currentNode != null)
                    {
                        // 自动选中树状图中的节点
                        if (_last_slc != null)
                            _last_slc.BackColor = Color.White;
                        view.SelectedNode = currentNode;
                        currentNode.BackColor = Color.Red;
                        _last_slc = currentNode;
                        currentNode.EnsureVisible();
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

        private void AddRootsToTreeView(TreeView treeView, JObject jsonObject)
        {
            foreach (var scene in jsonObject.Properties())
            {
                RichNode rootNode = new RichNode();
                rootNode.Text = scene.Name;
                currentScene = scene.Name;
                treeView.Nodes.Add(rootNode);
                AddToParent(rootNode, (JObject)scene.Value);
            }
        }

        private void AddToParent(RichNode parentNode, JObject jsonObject)
        {
            foreach (var key in jsonObject.Properties())
            {
                RichNode node = new RichNode();
                node.scene = currentScene;
                if (key.Value is JObject)//处理需要分支的节点
                {
                    int id;
                    if (int.TryParse(key.Name, out id) && id > 100)//所以选项名字严禁纯数字！！！
                    {
                        currentId = id;
                        node.Text =Map.ChrMap[Convert.ToInt32(key.Value["chr"])] +"："+ key.Value["txt"].ToString();
                        node.id = currentId;
                    }
                    else
                    {
                        node.id=currentId;
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
                    AddToParent(node, (JObject)key.Value);
                }
                else//遍历到底部节点
                { 
                    switch(key.Name)
                    {
                        case "chr":
                            parentNode.chr=key.Value.ToString();
                            break;
                        case "txt":
                            parentNode.txt=key.Value.ToString();
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
            currentNode=e.Node as RichNode;
            id.Text = "ID：" + currentNode.id.ToString();
            chr_edit.Text= currentNode.chr;
            txt_edit.Text = currentNode.txt;
            switch(currentNode.Text)
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
                currentNode.txt = txt_edit.Text;
                currentNode.Text = currentNode.txt;
                //保存到json


            }
        }
    }
}
