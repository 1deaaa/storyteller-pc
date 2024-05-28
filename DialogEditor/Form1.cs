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
        TreeNode selected;
        TreeNode _last_slc;
        public static JToken JsonSource = JToken.Parse(System.IO.File.ReadAllText("对话.json"));
        TreeNode currentNode;
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            LoadTree();
        }

        void LoadTree()
        {
            AddNodesToTreeView(view, (JObject)JsonSource);
        }

        private void treeView1_AfterSelect_1(object sender, TreeViewEventArgs e)
        {

        }

        private TreeNode FindNodeByText(TreeNodeCollection nodes, string searchText)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Text.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return node;
                }
                TreeNode foundNode = FindNodeByText(node.Nodes, searchText);
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
            foreach (TreeNode node in nodes)
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
                    selected = FindNodeByText(view.Nodes, selectedOption);
                    if (selected != null)
                    {
                        // 自动选中树状图中的节点
                        if (_last_slc != null)
                            _last_slc.BackColor = Color.White;
                        view.SelectedNode = selected;
                        selected.BackColor = Color.Red;
                        _last_slc = selected;
                        selected.EnsureVisible();
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

        private void AddNodesToTreeView(TreeView treeView, JObject jsonObject)
        {
            foreach (var property in jsonObject.Properties())
            {
                TreeNode node = new TreeNode(property.Name);
                treeView.Nodes.Add(node);
                AddChildrenToNode(node, (JObject)property.Value);
            }
        }

        private void AddChildrenToNode(TreeNode parentNode, JObject jsonObject)
        {
            foreach (var key in jsonObject.Properties())
            {
                 
            }
        }
    }
}
