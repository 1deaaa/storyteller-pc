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
            this.view = new System.Windows.Forms.TreeView();
            this.search_list = new System.Windows.Forms.ListBox();
            this.search = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.act = new System.Windows.Forms.Button();
            this.opt = new System.Windows.Forms.Button();
            this.txt = new System.Windows.Forms.Label();
            this.chr = new System.Windows.Forms.Label();
            this.id = new System.Windows.Forms.Label();
            this.chr_edit = new System.Windows.Forms.TextBox();
            this.txt_edit = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.opt_edit = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // view
            // 
            this.view.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.view.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.view.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.view.Location = new System.Drawing.Point(0, 0);
            this.view.Margin = new System.Windows.Forms.Padding(2);
            this.view.Name = "view";
            this.view.Size = new System.Drawing.Size(740, 762);
            this.view.TabIndex = 0;
            this.view.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.view_AfterSelect);
            // 
            // search_list
            // 
            this.search_list.BackColor = System.Drawing.Color.LightSteelBlue;
            this.search_list.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.search_list.Font = new System.Drawing.Font("Microsoft YaHei UI Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.search_list.ForeColor = System.Drawing.Color.Black;
            this.search_list.FormattingEnabled = true;
            this.search_list.ItemHeight = 27;
            this.search_list.Location = new System.Drawing.Point(1001, 114);
            this.search_list.Margin = new System.Windows.Forms.Padding(2);
            this.search_list.Name = "search_list";
            this.search_list.Size = new System.Drawing.Size(410, 648);
            this.search_list.TabIndex = 2;
            this.search_list.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged_1);
            // 
            // search
            // 
            this.search.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.search.Font = new System.Drawing.Font("微软雅黑 Light", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.search.Location = new System.Drawing.Point(1001, 3);
            this.search.Margin = new System.Windows.Forms.Padding(2);
            this.search.Multiline = true;
            this.search.Name = "search";
            this.search.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.search.Size = new System.Drawing.Size(410, 100);
            this.search.TabIndex = 1;
            this.search.TextChanged += new System.EventHandler(this.textBox1_TextChanged_1);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightSkyBlue;
            this.panel1.Controls.Add(this.act);
            this.panel1.Controls.Add(this.opt);
            this.panel1.Controls.Add(this.txt);
            this.panel1.Controls.Add(this.chr);
            this.panel1.Controls.Add(this.id);
            this.panel1.Controls.Add(this.chr_edit);
            this.panel1.Controls.Add(this.txt_edit);
            this.panel1.Font = new System.Drawing.Font("微软雅黑 Light", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel1.Location = new System.Drawing.Point(745, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(251, 478);
            this.panel1.TabIndex = 3;
            // 
            // act
            // 
            this.act.BackColor = System.Drawing.Color.Orange;
            this.act.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.act.Font = new System.Drawing.Font("微软雅黑 Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.act.Location = new System.Drawing.Point(7, 423);
            this.act.Name = "act";
            this.act.Size = new System.Drawing.Size(237, 48);
            this.act.TabIndex = 11;
            this.act.Text = "创建行为节点";
            this.act.UseVisualStyleBackColor = false;
            this.act.Click += new System.EventHandler(this.act_Click);
            // 
            // opt
            // 
            this.opt.BackColor = System.Drawing.Color.LimeGreen;
            this.opt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opt.Font = new System.Drawing.Font("微软雅黑 Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.opt.Location = new System.Drawing.Point(7, 369);
            this.opt.Name = "opt";
            this.opt.Size = new System.Drawing.Size(237, 48);
            this.opt.TabIndex = 10;
            this.opt.Text = "创建选项节点";
            this.opt.UseVisualStyleBackColor = false;
            this.opt.Click += new System.EventHandler(this.opt_Click);
            // 
            // txt
            // 
            this.txt.Font = new System.Drawing.Font("微软雅黑 Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt.Location = new System.Drawing.Point(0, 88);
            this.txt.Name = "txt";
            this.txt.Size = new System.Drawing.Size(251, 24);
            this.txt.TabIndex = 8;
            this.txt.Text = "文本";
            this.txt.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // chr
            // 
            this.chr.Font = new System.Drawing.Font("微软雅黑 Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chr.Location = new System.Drawing.Point(0, 24);
            this.chr.Name = "chr";
            this.chr.Size = new System.Drawing.Size(251, 24);
            this.chr.TabIndex = 7;
            this.chr.Text = "角色";
            this.chr.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // id
            // 
            this.id.Dock = System.Windows.Forms.DockStyle.Top;
            this.id.Font = new System.Drawing.Font("微软雅黑 Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.id.Location = new System.Drawing.Point(0, 0);
            this.id.Name = "id";
            this.id.Size = new System.Drawing.Size(251, 24);
            this.id.TabIndex = 6;
            this.id.Text = "ID";
            this.id.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // chr_edit
            // 
            this.chr_edit.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.chr_edit.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chr_edit.ForeColor = System.Drawing.Color.Salmon;
            this.chr_edit.Location = new System.Drawing.Point(7, 58);
            this.chr_edit.Margin = new System.Windows.Forms.Padding(10);
            this.chr_edit.Name = "chr_edit";
            this.chr_edit.Size = new System.Drawing.Size(237, 27);
            this.chr_edit.TabIndex = 5;
            this.chr_edit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chr_edit.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // txt_edit
            // 
            this.txt_edit.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txt_edit.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_edit.ForeColor = System.Drawing.Color.BlueViolet;
            this.txt_edit.Location = new System.Drawing.Point(2, 123);
            this.txt_edit.Margin = new System.Windows.Forms.Padding(2);
            this.txt_edit.Multiline = true;
            this.txt_edit.Name = "txt_edit";
            this.txt_edit.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_edit.Size = new System.Drawing.Size(247, 241);
            this.txt_edit.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Aquamarine;
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.opt_edit);
            this.panel2.Location = new System.Drawing.Point(745, 504);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(251, 258);
            this.panel2.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("微软雅黑 Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(-5, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(251, 24);
            this.label1.TabIndex = 12;
            this.label1.Text = "选项名字";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("微软雅黑 Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(5, 86);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(239, 48);
            this.button1.TabIndex = 11;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // opt_edit
            // 
            this.opt_edit.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.opt_edit.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.opt_edit.Location = new System.Drawing.Point(7, 46);
            this.opt_edit.Margin = new System.Windows.Forms.Padding(10);
            this.opt_edit.Name = "opt_edit";
            this.opt_edit.Size = new System.Drawing.Size(235, 27);
            this.opt_edit.TabIndex = 9;
            this.opt_edit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.ClientSize = new System.Drawing.Size(1422, 763);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.search_list);
            this.Controls.Add(this.search);
            this.Controls.Add(this.view);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load_1);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public TreeView view;
        private ListBox search_list;
        private TextBox search;
        private Panel panel1;
        private TextBox txt_edit;
        private TextBox chr_edit;
        private Label id;
        private Label chr;
        private Panel panel2;
        private TextBox opt_edit;
        private Button opt;
        private Button button1;
        private Button act;
        private Label txt;
        private Label label1;
    }
}
