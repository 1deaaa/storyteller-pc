using Newtonsoft.Json.Linq;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Threading;

namespace DialogSystem
{
    public partial class Form1 : Form
    {
        TreeNode selected;
        TreeNode _last_slc;
        string filePath = @"E:\0\开发\C#\DialogTest\bin\Debug\对话.json";
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls=false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        void LoadTree()
        {

        }

        private void treeView1_AfterSelect_1(object sender, TreeViewEventArgs e)
        {

        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 获取用户在ListBox中选中的项
            string selectedOption = listBox1.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedOption))
            {
                Thread td= new Thread(() =>
                {
                    // 根据选中的项在树状图中查找对应的节点
                    selected = FindNodeByText(treeView1.Nodes, selectedOption);
                    if (selected != null)
                    {
                        // 自动选中树状图中的节点
                        if (_last_slc != null)
                            _last_slc.BackColor = Color.White;
                        treeView1.SelectedNode = selected;
                        selected.BackColor = Color.Red;
                        _last_slc = selected;
                        selected.EnsureVisible();
                    }
                });
                td.Start();
            }
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // 当文本框内容发生变化时，执行模糊搜索并更新ListBox中的项
            string searchText = textBox1.Text.Trim();

            if (!string.IsNullOrEmpty(searchText))
            {
                // 执行模糊搜索
                List<string> matchingOptions = SearchOptions(searchText);

                // 更新ListBox中的项
                UpdateListBox(matchingOptions);
            }
            else
            {
                // 清空ListBox
                listBox1.Items.Clear();
            }
        }

        private List<string> SearchOptions(string searchText)
        {
            // 在树状图节点中执行模糊搜索
            List<string> matchingOptions = new List<string>();
            SearchNodes(treeView1.Nodes, searchText, matchingOptions);
            return matchingOptions;
        }

        private void SearchNodes(TreeNodeCollection nodes, string searchText, List<string> matchingOptions)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Text.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    matchingOptions.Add(node.Text);
                }

                SearchNodes(node.Nodes, searchText, matchingOptions);
            }
        }

        private void UpdateListBox(List<string> options)
        {
            // 更新ListBox中的项
            listBox1.Items.Clear();
            foreach (string option in options)
            {
                listBox1.Items.Add(option);
            }
        }
    }
}
