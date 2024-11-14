using System.Drawing;
using System.Windows.Forms;

namespace DialogSystem
{
    partial class Editor
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
            this.components = new System.ComponentModel.Container();
            this.view = new System.Windows.Forms.TreeView();
            this.search_list = new System.Windows.Forms.ListBox();
            this.search = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.next_edit = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.scene_name = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.new_scene = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.new_dia = new System.Windows.Forms.Button();
            this.pgrs_slc = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.cap_edit = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.opt_edit = new System.Windows.Forms.TextBox();
            this.new_act = new System.Windows.Forms.Button();
            this.new_opt = new System.Windows.Forms.Button();
            this.txt = new System.Windows.Forms.Label();
            this.chr = new System.Windows.Forms.Label();
            this.chr_edit = new System.Windows.Forms.TextBox();
            this.txt_edit = new System.Windows.Forms.TextBox();
            this.id = new System.Windows.Forms.Label();
            this.sceneMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.删除节点ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.创建对话ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.创建场景ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dlgMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.创建对话ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.创建选项ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.创建行为ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.delMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pgrs_slc)).BeginInit();
            this.sceneMenu.SuspendLayout();
            this.dlgMenu.SuspendLayout();
            this.delMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // view
            // 
            this.view.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.view.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.view.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.view.Location = new System.Drawing.Point(0, 0);
            this.view.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.view.Name = "view";
            this.view.Size = new System.Drawing.Size(1158, 1262);
            this.view.TabIndex = 0;
            this.view.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.view_AfterSelect);
            this.view.MouseUp += new System.Windows.Forms.MouseEventHandler(this.view_MouseUp);
            // 
            // search_list
            // 
            this.search_list.BackColor = System.Drawing.Color.LightSteelBlue;
            this.search_list.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.search_list.Font = new System.Drawing.Font("Microsoft YaHei UI Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.search_list.ForeColor = System.Drawing.Color.Black;
            this.search_list.FormattingEnabled = true;
            this.search_list.ItemHeight = 31;
            this.search_list.Location = new System.Drawing.Point(1439, 128);
            this.search_list.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.search_list.Name = "search_list";
            this.search_list.Size = new System.Drawing.Size(459, 1116);
            this.search_list.TabIndex = 2;
            this.search_list.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged_1);
            // 
            // search
            // 
            this.search.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.search.Font = new System.Drawing.Font("微软雅黑 Light", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.search.Location = new System.Drawing.Point(1436, 1);
            this.search.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.search.Multiline = true;
            this.search.Name = "search";
            this.search.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.search.Size = new System.Drawing.Size(462, 120);
            this.search.TabIndex = 1;
            this.search.TextChanged += new System.EventHandler(this.textBox1_TextChanged_1);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightSkyBlue;
            this.panel1.Controls.Add(this.next_edit);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.scene_name);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.new_scene);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.new_dia);
            this.panel1.Controls.Add(this.pgrs_slc);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.cap_edit);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.opt_edit);
            this.panel1.Controls.Add(this.new_act);
            this.panel1.Controls.Add(this.new_opt);
            this.panel1.Controls.Add(this.txt);
            this.panel1.Controls.Add(this.chr);
            this.panel1.Controls.Add(this.chr_edit);
            this.panel1.Controls.Add(this.txt_edit);
            this.panel1.Controls.Add(this.id);
            this.panel1.Font = new System.Drawing.Font("微软雅黑 Light", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel1.Location = new System.Drawing.Point(1156, 1);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(282, 1261);
            this.panel1.TabIndex = 3;
            // 
            // next_edit
            // 
            this.next_edit.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.next_edit.Font = new System.Drawing.Font("微软雅黑 Light", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.next_edit.Location = new System.Drawing.Point(8, 803);
            this.next_edit.Margin = new System.Windows.Forms.Padding(12);
            this.next_edit.Name = "next_edit";
            this.next_edit.Size = new System.Drawing.Size(264, 27);
            this.next_edit.TabIndex = 23;
            this.next_edit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.next_edit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.next_edit_KeyDown);
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("微软雅黑 Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(0, 766);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(282, 29);
            this.label5.TabIndex = 22;
            this.label5.Text = "跳转";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // scene_name
            // 
            this.scene_name.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.scene_name.Font = new System.Drawing.Font("微软雅黑 Light", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.scene_name.Location = new System.Drawing.Point(8, 726);
            this.scene_name.Margin = new System.Windows.Forms.Padding(12);
            this.scene_name.Name = "scene_name";
            this.scene_name.Size = new System.Drawing.Size(264, 27);
            this.scene_name.TabIndex = 21;
            this.scene_name.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.scene_name.KeyDown += new System.Windows.Forms.KeyEventHandler(this.scene_name_KeyDown);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("微软雅黑 Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(-1, 685);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(282, 29);
            this.label4.TabIndex = 20;
            this.label4.Text = "场景名字";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // new_scene
            // 
            this.new_scene.BackColor = System.Drawing.Color.DarkTurquoise;
            this.new_scene.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.new_scene.Font = new System.Drawing.Font("微软雅黑 Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.new_scene.Location = new System.Drawing.Point(8, 925);
            this.new_scene.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.new_scene.Name = "new_scene";
            this.new_scene.Size = new System.Drawing.Size(267, 58);
            this.new_scene.TabIndex = 19;
            this.new_scene.Text = "创建新场景";
            this.new_scene.UseVisualStyleBackColor = false;
            this.new_scene.Click += new System.EventHandler(this.new_scene_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.DarkViolet;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("微软雅黑 Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(9, 1182);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(267, 65);
            this.button1.TabIndex = 18;
            this.button1.Text = "创建跳转链接";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.new_next_Click);
            // 
            // new_dia
            // 
            this.new_dia.BackColor = System.Drawing.Color.CornflowerBlue;
            this.new_dia.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.new_dia.Font = new System.Drawing.Font("微软雅黑 Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.new_dia.Location = new System.Drawing.Point(9, 988);
            this.new_dia.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.new_dia.Name = "new_dia";
            this.new_dia.Size = new System.Drawing.Size(267, 58);
            this.new_dia.TabIndex = 17;
            this.new_dia.Text = "创建新说明";
            this.new_dia.UseVisualStyleBackColor = false;
            this.new_dia.Click += new System.EventHandler(this.new_dia_Click);
            // 
            // pgrs_slc
            // 
            this.pgrs_slc.Location = new System.Drawing.Point(8, 882);
            this.pgrs_slc.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pgrs_slc.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.pgrs_slc.Name = "pgrs_slc";
            this.pgrs_slc.Size = new System.Drawing.Size(267, 36);
            this.pgrs_slc.TabIndex = 16;
            this.pgrs_slc.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.pgrs_slc.ValueChanged += new System.EventHandler(this.pgrs_changed);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("微软雅黑 Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(0, 842);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(282, 29);
            this.label3.TabIndex = 15;
            this.label3.Text = "全局进度";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // cap_edit
            // 
            this.cap_edit.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.cap_edit.Font = new System.Drawing.Font("微软雅黑 Light", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cap_edit.Location = new System.Drawing.Point(9, 646);
            this.cap_edit.Margin = new System.Windows.Forms.Padding(12);
            this.cap_edit.Name = "cap_edit";
            this.cap_edit.Size = new System.Drawing.Size(264, 27);
            this.cap_edit.TabIndex = 14;
            this.cap_edit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.cap_edit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cap_edit_KeyDown);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("微软雅黑 Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(-3, 604);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(282, 29);
            this.label2.TabIndex = 13;
            this.label2.Text = "任务栏提示";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("微软雅黑 Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(0, 524);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(282, 29);
            this.label1.TabIndex = 12;
            this.label1.Text = "选项名字";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // opt_edit
            // 
            this.opt_edit.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.opt_edit.Font = new System.Drawing.Font("微软雅黑 Light", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.opt_edit.Location = new System.Drawing.Point(9, 565);
            this.opt_edit.Margin = new System.Windows.Forms.Padding(12);
            this.opt_edit.Name = "opt_edit";
            this.opt_edit.Size = new System.Drawing.Size(264, 27);
            this.opt_edit.TabIndex = 9;
            this.opt_edit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.opt_edit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.opt_edit_KeyDown);
            // 
            // new_act
            // 
            this.new_act.BackColor = System.Drawing.Color.Orange;
            this.new_act.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.new_act.Font = new System.Drawing.Font("微软雅黑 Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.new_act.Location = new System.Drawing.Point(9, 1112);
            this.new_act.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.new_act.Name = "new_act";
            this.new_act.Size = new System.Drawing.Size(267, 65);
            this.new_act.TabIndex = 11;
            this.new_act.Text = "编辑函数映射";
            this.new_act.UseVisualStyleBackColor = false;
            this.new_act.Click += new System.EventHandler(this.act_Click);
            // 
            // new_opt
            // 
            this.new_opt.BackColor = System.Drawing.Color.LimeGreen;
            this.new_opt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.new_opt.Font = new System.Drawing.Font("微软雅黑 Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.new_opt.Location = new System.Drawing.Point(9, 1050);
            this.new_opt.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.new_opt.Name = "new_opt";
            this.new_opt.Size = new System.Drawing.Size(267, 58);
            this.new_opt.TabIndex = 10;
            this.new_opt.Text = "创建选项";
            this.new_opt.UseVisualStyleBackColor = false;
            this.new_opt.Click += new System.EventHandler(this.new_opt_Click);
            // 
            // txt
            // 
            this.txt.Font = new System.Drawing.Font("微软雅黑 Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt.Location = new System.Drawing.Point(0, 106);
            this.txt.Name = "txt";
            this.txt.Size = new System.Drawing.Size(282, 29);
            this.txt.TabIndex = 8;
            this.txt.Text = "文本";
            this.txt.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // chr
            // 
            this.chr.Font = new System.Drawing.Font("微软雅黑 Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chr.Location = new System.Drawing.Point(0, 29);
            this.chr.Name = "chr";
            this.chr.Size = new System.Drawing.Size(282, 29);
            this.chr.TabIndex = 7;
            this.chr.Text = "角色代号";
            this.chr.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // chr_edit
            // 
            this.chr_edit.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.chr_edit.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chr_edit.ForeColor = System.Drawing.Color.Salmon;
            this.chr_edit.Location = new System.Drawing.Point(8, 70);
            this.chr_edit.Margin = new System.Windows.Forms.Padding(12);
            this.chr_edit.Name = "chr_edit";
            this.chr_edit.Size = new System.Drawing.Size(267, 32);
            this.chr_edit.TabIndex = 5;
            this.chr_edit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chr_edit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.chr_edit_KeyDown);
            // 
            // txt_edit
            // 
            this.txt_edit.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txt_edit.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_edit.ForeColor = System.Drawing.Color.BlueViolet;
            this.txt_edit.Location = new System.Drawing.Point(9, 137);
            this.txt_edit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_edit.Multiline = true;
            this.txt_edit.Name = "txt_edit";
            this.txt_edit.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_edit.Size = new System.Drawing.Size(273, 366);
            this.txt_edit.TabIndex = 4;
            this.txt_edit.TextChanged += new System.EventHandler(this.txt_edit_TextChanged);
            this.txt_edit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_edit_KeyDown);
            // 
            // id
            // 
            this.id.Dock = System.Windows.Forms.DockStyle.Top;
            this.id.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.id.ForeColor = System.Drawing.Color.White;
            this.id.Location = new System.Drawing.Point(0, 0);
            this.id.Name = "id";
            this.id.Size = new System.Drawing.Size(282, 29);
            this.id.TabIndex = 6;
            this.id.Text = "所属对话ID";
            this.id.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // sceneMenu
            // 
            this.sceneMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.sceneMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.删除节点ToolStripMenuItem,
            this.创建对话ToolStripMenuItem,
            this.创建场景ToolStripMenuItem});
            this.sceneMenu.Name = "contextMenuStrip1";
            this.sceneMenu.Size = new System.Drawing.Size(153, 94);
            // 
            // 删除节点ToolStripMenuItem
            // 
            this.删除节点ToolStripMenuItem.Name = "删除节点ToolStripMenuItem";
            this.删除节点ToolStripMenuItem.Size = new System.Drawing.Size(152, 30);
            this.删除节点ToolStripMenuItem.Text = "删除节点";
            this.删除节点ToolStripMenuItem.Click += new System.EventHandler(this.删除节点ToolStripMenuItem_Click);
            // 
            // 创建对话ToolStripMenuItem
            // 
            this.创建对话ToolStripMenuItem.Name = "创建对话ToolStripMenuItem";
            this.创建对话ToolStripMenuItem.Size = new System.Drawing.Size(152, 30);
            this.创建对话ToolStripMenuItem.Text = "创建对话";
            this.创建对话ToolStripMenuItem.Click += new System.EventHandler(this.创建对话ToolStripMenuItem_Click);
            // 
            // 创建场景ToolStripMenuItem
            // 
            this.创建场景ToolStripMenuItem.Name = "创建场景ToolStripMenuItem";
            this.创建场景ToolStripMenuItem.Size = new System.Drawing.Size(152, 30);
            this.创建场景ToolStripMenuItem.Text = "创建场景";
            this.创建场景ToolStripMenuItem.Click += new System.EventHandler(this.创建场景ToolStripMenuItem_Click);
            // 
            // dlgMenu
            // 
            this.dlgMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.dlgMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.创建对话ToolStripMenuItem1,
            this.创建选项ToolStripMenuItem,
            this.创建行为ToolStripMenuItem});
            this.dlgMenu.Name = "contextMenuStrip1";
            this.dlgMenu.Size = new System.Drawing.Size(153, 124);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(152, 30);
            this.toolStripMenuItem1.Text = "删除节点";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // 创建对话ToolStripMenuItem1
            // 
            this.创建对话ToolStripMenuItem1.Name = "创建对话ToolStripMenuItem1";
            this.创建对话ToolStripMenuItem1.Size = new System.Drawing.Size(152, 30);
            this.创建对话ToolStripMenuItem1.Text = "创建对话";
            this.创建对话ToolStripMenuItem1.Click += new System.EventHandler(this.创建对话ToolStripMenuItem1_Click);
            // 
            // 创建选项ToolStripMenuItem
            // 
            this.创建选项ToolStripMenuItem.Name = "创建选项ToolStripMenuItem";
            this.创建选项ToolStripMenuItem.Size = new System.Drawing.Size(152, 30);
            this.创建选项ToolStripMenuItem.Text = "创建选项";
            this.创建选项ToolStripMenuItem.Click += new System.EventHandler(this.创建选项ToolStripMenuItem_Click);
            // 
            // 创建行为ToolStripMenuItem
            // 
            this.创建行为ToolStripMenuItem.Name = "创建行为ToolStripMenuItem";
            this.创建行为ToolStripMenuItem.Size = new System.Drawing.Size(152, 30);
            this.创建行为ToolStripMenuItem.Text = "创建行为";
            // 
            // delMenu
            // 
            this.delMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.delMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2});
            this.delMenu.Name = "contextMenuStrip1";
            this.delMenu.Size = new System.Drawing.Size(153, 34);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(152, 30);
            this.toolStripMenuItem2.Text = "删除节点";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.ClientSize = new System.Drawing.Size(1901, 1261);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.search_list);
            this.Controls.Add(this.search);
            this.Controls.Add(this.view);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Editor";
            this.Text = "哈基Miku";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Editor_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load_1);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Editor_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pgrs_slc)).EndInit();
            this.sceneMenu.ResumeLayout(false);
            this.dlgMenu.ResumeLayout(false);
            this.delMenu.ResumeLayout(false);
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
        private TextBox opt_edit;
        private Button new_opt;
        private Button new_act;
        private Label txt;
        private Label label1;
        private Label label3;
        private TextBox cap_edit;
        private Label label2;
        private NumericUpDown pgrs_slc;
        private Button new_dia;
        private ContextMenuStrip sceneMenu;
        private ToolStripMenuItem 删除节点ToolStripMenuItem;
        private Button button1;
        private Button new_scene;
        private TextBox scene_name;
        private Label label4;
        private TextBox next_edit;
        private Label label5;
        private ContextMenuStrip dlgMenu;
        private ToolStripMenuItem toolStripMenuItem1;
        private ContextMenuStrip delMenu;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripMenuItem 创建对话ToolStripMenuItem;
        private ToolStripMenuItem 创建场景ToolStripMenuItem;
        private ToolStripMenuItem 创建对话ToolStripMenuItem1;
        private ToolStripMenuItem 创建选项ToolStripMenuItem;
        private ToolStripMenuItem 创建行为ToolStripMenuItem;
    }
}
