using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DialogEditor
{
    public partial class ActEditor : Form
    {
        public static string fun;
        public static string args;
        public ActEditor(bool edit_mode = false, string _args = null)
        {
            fun = null;
            args = null;
            InitializeComponent();
            if (edit_mode)
            {
                textBox1.Enabled = false;
                if (!string.IsNullOrEmpty(_args))
                {
                    var items = _args.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in items)
                    {
                        listBox1.Items.Add(item);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count == 0)
                return;
            listBox1.Items.Remove(listBox1.SelectedItems[0]);
        }

        private void ActEditor_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
                return;
            listBox1.Items.Add(textBox2.Text);
            textBox2.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0)
                args = " ";
            else
            {
                fun = textBox1.Text;
                StringBuilder sb = new StringBuilder();
                foreach (var item in listBox1.Items)
                    sb.Append(item.ToString()).Append(" ");
                // 移除最后一个多余的空格
                if (sb.Length > 0)
                {
                    sb.Length--;
                }
                args = sb.ToString();
            }
            Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
