namespace DialogSystem
{
    partial class MainUI
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txt = new System.Windows.Forms.Label();
            this.spk = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cap = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txt
            // 
            this.txt.BackColor = System.Drawing.Color.PaleTurquoise;
            this.txt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.txt.Font = new System.Drawing.Font("微软雅黑 Light", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txt.Location = new System.Drawing.Point(-3, 495);
            this.txt.Name = "txt";
            this.txt.Size = new System.Drawing.Size(1085, 125);
            this.txt.TabIndex = 0;
            this.txt.Text = "对话框";
            this.txt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txt.Click += new System.EventHandler(this.txt_Click);
            // 
            // spk
            // 
            this.spk.BackColor = System.Drawing.Color.PaleTurquoise;
            this.spk.Font = new System.Drawing.Font("微软雅黑 Light", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.spk.ForeColor = System.Drawing.Color.SeaGreen;
            this.spk.Location = new System.Drawing.Point(470, 495);
            this.spk.Name = "spk";
            this.spk.Size = new System.Drawing.Size(138, 38);
            this.spk.TabIndex = 1;
            this.spk.Text = "角色";
            this.spk.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("微软雅黑 Light", 20F);
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(11, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(240, 63);
            this.label1.TabIndex = 2;
            this.label1.Text = "↑调试信息";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cap
            // 
            this.cap.BackColor = System.Drawing.Color.PaleTurquoise;
            this.cap.Font = new System.Drawing.Font("微软雅黑 Light", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cap.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.cap.Location = new System.Drawing.Point(396, 457);
            this.cap.Name = "cap";
            this.cap.Size = new System.Drawing.Size(279, 38);
            this.cap.TabIndex = 3;
            this.cap.Text = "任务提示";
            this.cap.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cap.Click += new System.EventHandler(this.label2_Click);
            // 
            // MainUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1075, 619);
            this.Controls.Add(this.cap);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.spk);
            this.Controls.Add(this.txt);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MainUI";
            this.Text = "MainUI";
            this.Load += new System.EventHandler(this.MainUI_Load);
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.Label txt;
        public System.Windows.Forms.Label spk;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label cap;
    }
}

