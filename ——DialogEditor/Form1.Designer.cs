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
            TreeNode treeNode1 = new TreeNode("节点1");
            TreeNode treeNode2 = new TreeNode("节点3");
            TreeNode treeNode3 = new TreeNode("节点6");
            TreeNode treeNode4 = new TreeNode("节点4", new TreeNode[] { treeNode3 });
            TreeNode treeNode5 = new TreeNode("节点2", new TreeNode[] { treeNode2, treeNode4 });
            TreeNode treeNode6 = new TreeNode("节点0", new TreeNode[] { treeNode1, treeNode5 });
            TreeNode treeNode7 = new TreeNode("节点7");
            TreeNode treeNode8 = new TreeNode("节点5", new TreeNode[] { treeNode7 });
            TreeNode treeNode9 = new TreeNode("节点8");
            TreeNode treeNode10 = new TreeNode("节点9");
            TreeNode treeNode11 = new TreeNode("节点10");
            TreeNode treeNode12 = new TreeNode("节点11");
            TreeNode treeNode13 = new TreeNode("节点12");
            TreeNode treeNode14 = new TreeNode("节点13");
            TreeNode treeNode15 = new TreeNode("节点14");
            TreeNode treeNode16 = new TreeNode("节点15");
            TreeNode treeNode17 = new TreeNode("节点16");
            TreeNode treeNode18 = new TreeNode("节点17");
            TreeNode treeNode19 = new TreeNode("节点18");
            TreeNode treeNode20 = new TreeNode("节点19");
            TreeNode treeNode21 = new TreeNode("节点20");
            TreeNode treeNode22 = new TreeNode("节点21");
            treeView1 = new TreeView();
            listBox1 = new ListBox();
            textBox1 = new TextBox();
            SuspendLayout();
            // 
            // treeView1
            // 
            treeView1.Location = new Point(-1, 1);
            treeView1.Name = "treeView1";
            treeNode1.Name = "节点1";
            treeNode1.Text = "节点1";
            treeNode2.Name = "节点3";
            treeNode2.Text = "节点3";
            treeNode3.Name = "节点6";
            treeNode3.Text = "节点6";
            treeNode4.Name = "节点4";
            treeNode4.Text = "节点4";
            treeNode5.Name = "节点2";
            treeNode5.Text = "节点2";
            treeNode6.BackColor = Color.White;
            treeNode6.Name = "节点0";
            treeNode6.Text = "节点0";
            treeNode7.Name = "节点7";
            treeNode7.Text = "节点7";
            treeNode8.Name = "节点5";
            treeNode8.Text = "节点5";
            treeNode9.Name = "节点8";
            treeNode9.Text = "节点8";
            treeNode10.BackColor = Color.White;
            treeNode10.ForeColor = Color.Black;
            treeNode10.Name = "节点9";
            treeNode10.Text = "节点9";
            treeNode11.Name = "节点10";
            treeNode11.Text = "节点10";
            treeNode12.Name = "节点11";
            treeNode12.Text = "节点11";
            treeNode13.Name = "节点12";
            treeNode13.Text = "节点12";
            treeNode14.Name = "节点13";
            treeNode14.Text = "节点13";
            treeNode15.Name = "节点14";
            treeNode15.Text = "节点14";
            treeNode16.Name = "节点15";
            treeNode16.Text = "节点15";
            treeNode17.Name = "节点16";
            treeNode17.Text = "节点16";
            treeNode18.Name = "节点17";
            treeNode18.Text = "节点17";
            treeNode19.Name = "节点18";
            treeNode19.Text = "节点18";
            treeNode20.Name = "节点19";
            treeNode20.Text = "节点19";
            treeNode21.Name = "节点20";
            treeNode21.Text = "节点20";
            treeNode22.Name = "a";
            treeNode22.Text = "节点21";
            treeView1.Nodes.AddRange(new TreeNode[] { treeNode6, treeNode8, treeNode9, treeNode10, treeNode11, treeNode12, treeNode13, treeNode14, treeNode15, treeNode16, treeNode17, treeNode18, treeNode19, treeNode20, treeNode21, treeNode22 });
            treeView1.Size = new Size(974, 1014);
            treeView1.TabIndex = 0;
            treeView1.AfterSelect += treeView1_AfterSelect_1;
            // 
            // listBox1
            // 
            listBox1.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 31;
            listBox1.Location = new Point(1331, 50);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(339, 965);
            listBox1.TabIndex = 2;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 134);
            textBox1.Location = new Point(1331, 1);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(339, 43);
            textBox1.TabIndex = 1;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1673, 1017);
            Controls.Add(listBox1);
            Controls.Add(textBox1);
            Controls.Add(treeView1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        public TreeView treeView1;
        private ListBox listBox1;
        private TextBox textBox1;
    }
}
