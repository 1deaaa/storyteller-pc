using System.Drawing;
using System.Windows.Forms;

namespace DialogSystem
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.TreeNode treeNode199 = new System.Windows.Forms.TreeNode("节点1");
            System.Windows.Forms.TreeNode treeNode200 = new System.Windows.Forms.TreeNode("节点3");
            System.Windows.Forms.TreeNode treeNode201 = new System.Windows.Forms.TreeNode("节点6");
            System.Windows.Forms.TreeNode treeNode202 = new System.Windows.Forms.TreeNode("节点4", new System.Windows.Forms.TreeNode[] {
            treeNode201});
            System.Windows.Forms.TreeNode treeNode203 = new System.Windows.Forms.TreeNode("节点2", new System.Windows.Forms.TreeNode[] {
            treeNode200,
            treeNode202});
            System.Windows.Forms.TreeNode treeNode204 = new System.Windows.Forms.TreeNode("节点0", new System.Windows.Forms.TreeNode[] {
            treeNode199,
            treeNode203});
            System.Windows.Forms.TreeNode treeNode205 = new System.Windows.Forms.TreeNode("节点7");
            System.Windows.Forms.TreeNode treeNode206 = new System.Windows.Forms.TreeNode("节点5", new System.Windows.Forms.TreeNode[] {
            treeNode205});
            System.Windows.Forms.TreeNode treeNode207 = new System.Windows.Forms.TreeNode("节点8");
            System.Windows.Forms.TreeNode treeNode208 = new System.Windows.Forms.TreeNode("节点9");
            System.Windows.Forms.TreeNode treeNode209 = new System.Windows.Forms.TreeNode("节点10");
            System.Windows.Forms.TreeNode treeNode210 = new System.Windows.Forms.TreeNode("节点11");
            System.Windows.Forms.TreeNode treeNode211 = new System.Windows.Forms.TreeNode("节点12");
            System.Windows.Forms.TreeNode treeNode212 = new System.Windows.Forms.TreeNode("节点13");
            System.Windows.Forms.TreeNode treeNode213 = new System.Windows.Forms.TreeNode("节点14");
            System.Windows.Forms.TreeNode treeNode214 = new System.Windows.Forms.TreeNode("节点15");
            System.Windows.Forms.TreeNode treeNode215 = new System.Windows.Forms.TreeNode("节点16");
            System.Windows.Forms.TreeNode treeNode216 = new System.Windows.Forms.TreeNode("节点17");
            System.Windows.Forms.TreeNode treeNode217 = new System.Windows.Forms.TreeNode("节点18");
            System.Windows.Forms.TreeNode treeNode218 = new System.Windows.Forms.TreeNode("节点19");
            System.Windows.Forms.TreeNode treeNode219 = new System.Windows.Forms.TreeNode("节点20");
            System.Windows.Forms.TreeNode treeNode220 = new System.Windows.Forms.TreeNode("节点21");
            this.view = new System.Windows.Forms.TreeView();
            this.search_list = new System.Windows.Forms.ListBox();
            this.search = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // view
            // 
            this.view.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.view.Font = new System.Drawing.Font("微软雅黑 Light", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.view.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.view.Location = new System.Drawing.Point(0, 0);
            this.view.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.view.Name = "view";
            treeNode199.Name = "节点1";
            treeNode199.Text = "节点1";
            treeNode200.Name = "节点3";
            treeNode200.Text = "节点3";
            treeNode201.Name = "节点6";
            treeNode201.Text = "节点6";
            treeNode202.Name = "节点4";
            treeNode202.Text = "节点4";
            treeNode203.Name = "节点2";
            treeNode203.Text = "节点2";
            treeNode204.BackColor = System.Drawing.Color.White;
            treeNode204.Name = "节点0";
            treeNode204.Text = "节点0";
            treeNode205.Name = "节点7";
            treeNode205.Text = "节点7";
            treeNode206.Name = "节点5";
            treeNode206.Text = "节点5";
            treeNode207.Name = "节点8";
            treeNode207.Text = "节点8";
            treeNode208.BackColor = System.Drawing.Color.White;
            treeNode208.ForeColor = System.Drawing.Color.Black;
            treeNode208.Name = "节点9";
            treeNode208.Text = "节点9";
            treeNode209.Name = "节点10";
            treeNode209.Text = "节点10";
            treeNode210.Name = "节点11";
            treeNode210.Text = "节点11";
            treeNode211.Name = "节点12";
            treeNode211.Text = "节点12";
            treeNode212.Name = "节点13";
            treeNode212.Text = "节点13";
            treeNode213.Name = "节点14";
            treeNode213.Text = "节点14";
            treeNode214.Name = "节点15";
            treeNode214.Text = "节点15";
            treeNode215.Name = "节点16";
            treeNode215.Text = "节点16";
            treeNode216.Name = "节点17";
            treeNode216.Text = "节点17";
            treeNode217.Name = "节点18";
            treeNode217.Text = "节点18";
            treeNode218.Name = "节点19";
            treeNode218.Text = "节点19";
            treeNode219.Name = "节点20";
            treeNode219.Text = "节点20";
            treeNode220.Name = "a";
            treeNode220.Text = "节点21";
            this.view.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode204,
            treeNode206,
            treeNode207,
            treeNode208,
            treeNode209,
            treeNode210,
            treeNode211,
            treeNode212,
            treeNode213,
            treeNode214,
            treeNode215,
            treeNode216,
            treeNode217,
            treeNode218,
            treeNode219,
            treeNode220});
            this.view.Size = new System.Drawing.Size(832, 957);
            this.view.TabIndex = 0;
            // 
            // search_list
            // 
            this.search_list.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.search_list.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.search_list.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.search_list.ForeColor = System.Drawing.Color.Black;
            this.search_list.FormattingEnabled = true;
            this.search_list.ItemHeight = 31;
            this.search_list.Location = new System.Drawing.Point(1126, 135);
            this.search_list.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.search_list.Name = "search_list";
            this.search_list.Size = new System.Drawing.Size(439, 806);
            this.search_list.TabIndex = 2;
            this.search_list.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged_1);
            // 
            // search
            // 
            this.search.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.search.Font = new System.Drawing.Font("微软雅黑 Light", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.search.Location = new System.Drawing.Point(1126, 0);
            this.search.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.search.Multiline = true;
            this.search.Name = "search";
            this.search.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.search.Size = new System.Drawing.Size(439, 120);
            this.search.TabIndex = 1;
            this.search.TextChanged += new System.EventHandler(this.textBox1_TextChanged_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.ClientSize = new System.Drawing.Size(1567, 956);
            this.Controls.Add(this.search_list);
            this.Controls.Add(this.search);
            this.Controls.Add(this.view);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load_1);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public TreeView view;
        private ListBox search_list;
        private TextBox search;
    }
}
