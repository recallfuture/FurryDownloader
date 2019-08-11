namespace FurryDownloader
{
    partial class mainFrom
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Label info_name;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label Expert;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainFrom));
            this.input_name = new System.Windows.Forms.TextBox();
            this.ButtonEnter = new System.Windows.Forms.Button();
            this.ButtonCancle = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.panel = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.InputStartPicNum = new System.Windows.Forms.TextBox();
            this.InputMaxPicNum = new System.Windows.Forms.TextBox();
            this.InputPageNum = new System.Windows.Forms.TextBox();
            this.Browse = new System.Windows.Forms.Button();
            this.FilePath = new System.Windows.Forms.TextBox();
            this.textBox = new System.Windows.Forms.TextBox();
            this.Help = new System.Windows.Forms.Button();
            this.ButtonLogin = new System.Windows.Forms.Button();
            info_name = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            Expert = new System.Windows.Forms.Label();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // info_name
            // 
            info_name.AutoSize = true;
            info_name.Font = new System.Drawing.Font("楷体", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            info_name.Location = new System.Drawing.Point(15, 33);
            info_name.Name = "info_name";
            info_name.Size = new System.Drawing.Size(235, 24);
            info_name.TabIndex = 0;
            info_name.Text = "在此处输入作者名：";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(30, 83);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(67, 15);
            label1.TabIndex = 8;
            label1.Text = "附加选项";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(3, 9);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(112, 15);
            label2.TabIndex = 6;
            label2.Text = "要下载的图集：";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(3, 38);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(112, 15);
            label3.TabIndex = 7;
            label3.Text = "图片保存目录：";
            // 
            // Expert
            // 
            Expert.AutoSize = true;
            Expert.Location = new System.Drawing.Point(33, 67);
            Expert.Name = "Expert";
            Expert.Size = new System.Drawing.Size(82, 15);
            Expert.TabIndex = 10;
            Expert.Text = "高级选项：";
            // 
            // input_name
            // 
            this.input_name.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.input_name.Location = new System.Drawing.Point(271, 28);
            this.input_name.Name = "input_name";
            this.input_name.Size = new System.Drawing.Size(227, 30);
            this.input_name.TabIndex = 1;
            // 
            // ButtonEnter
            // 
            this.ButtonEnter.Location = new System.Drawing.Point(544, 19);
            this.ButtonEnter.Name = "ButtonEnter";
            this.ButtonEnter.Size = new System.Drawing.Size(108, 47);
            this.ButtonEnter.TabIndex = 2;
            this.ButtonEnter.Text = "开始";
            this.ButtonEnter.UseVisualStyleBackColor = true;
            this.ButtonEnter.Click += new System.EventHandler(this.button1_Click);
            // 
            // ButtonCancle
            // 
            this.ButtonCancle.Enabled = false;
            this.ButtonCancle.Location = new System.Drawing.Point(544, 73);
            this.ButtonCancle.Name = "ButtonCancle";
            this.ButtonCancle.Size = new System.Drawing.Size(108, 47);
            this.ButtonCancle.TabIndex = 3;
            this.ButtonCancle.Text = "取消";
            this.ButtonCancle.UseVisualStyleBackColor = true;
            this.ButtonCancle.Click += new System.EventHandler(this.ButtonCancle_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(133, 8);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(59, 19);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "画廊";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(208, 8);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(59, 19);
            this.checkBox2.TabIndex = 5;
            this.checkBox2.Text = "手稿";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // panel
            // 
            this.panel.BackColor = System.Drawing.Color.Gainsboro;
            this.panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel.Controls.Add(this.label8);
            this.panel.Controls.Add(this.label7);
            this.panel.Controls.Add(this.label6);
            this.panel.Controls.Add(this.label5);
            this.panel.Controls.Add(this.InputStartPicNum);
            this.panel.Controls.Add(this.InputMaxPicNum);
            this.panel.Controls.Add(this.InputPageNum);
            this.panel.Controls.Add(Expert);
            this.panel.Controls.Add(this.Browse);
            this.panel.Controls.Add(this.FilePath);
            this.panel.Controls.Add(label3);
            this.panel.Controls.Add(label2);
            this.panel.Controls.Add(this.checkBox1);
            this.panel.Controls.Add(this.checkBox2);
            this.panel.Location = new System.Drawing.Point(12, 104);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(526, 126);
            this.panel.TabIndex = 6;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(453, 67);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(22, 15);
            this.label8.TabIndex = 19;
            this.label8.Text = "个";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(304, 67);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(97, 15);
            this.label7.TabIndex = 18;
            this.label7.Text = "张开始，下载";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(219, 67);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 15);
            this.label6.TabIndex = 17;
            this.label6.Text = "页第";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(130, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 15);
            this.label5.TabIndex = 16;
            this.label5.Text = "从第";
            // 
            // InputStartPicNum
            // 
            this.InputStartPicNum.Location = new System.Drawing.Point(262, 62);
            this.InputStartPicNum.Name = "InputStartPicNum";
            this.InputStartPicNum.Size = new System.Drawing.Size(36, 25);
            this.InputStartPicNum.TabIndex = 13;
            this.InputStartPicNum.Text = "1";
            // 
            // InputMaxPicNum
            // 
            this.InputMaxPicNum.Location = new System.Drawing.Point(407, 62);
            this.InputMaxPicNum.Name = "InputMaxPicNum";
            this.InputMaxPicNum.Size = new System.Drawing.Size(40, 25);
            this.InputMaxPicNum.TabIndex = 12;
            // 
            // InputPageNum
            // 
            this.InputPageNum.Location = new System.Drawing.Point(173, 62);
            this.InputPageNum.Name = "InputPageNum";
            this.InputPageNum.Size = new System.Drawing.Size(40, 25);
            this.InputPageNum.TabIndex = 11;
            this.InputPageNum.Text = "1";
            // 
            // Browse
            // 
            this.Browse.Location = new System.Drawing.Point(445, 33);
            this.Browse.Name = "Browse";
            this.Browse.Size = new System.Drawing.Size(59, 25);
            this.Browse.TabIndex = 9;
            this.Browse.Text = "浏览";
            this.Browse.UseVisualStyleBackColor = true;
            this.Browse.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // FilePath
            // 
            this.FilePath.Location = new System.Drawing.Point(133, 33);
            this.FilePath.Name = "FilePath";
            this.FilePath.ReadOnly = true;
            this.FilePath.Size = new System.Drawing.Size(306, 25);
            this.FilePath.TabIndex = 8;
            // 
            // textBox
            // 
            this.textBox.HideSelection = false;
            this.textBox.Location = new System.Drawing.Point(12, 236);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.ReadOnly = true;
            this.textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox.Size = new System.Drawing.Size(640, 305);
            this.textBox.TabIndex = 7;
            this.textBox.Text = resources.GetString("textBox.Text");
            // 
            // Help
            // 
            this.Help.Location = new System.Drawing.Point(544, 181);
            this.Help.Name = "Help";
            this.Help.Size = new System.Drawing.Size(108, 49);
            this.Help.TabIndex = 9;
            this.Help.Text = "帮助";
            this.Help.UseVisualStyleBackColor = true;
            this.Help.Click += new System.EventHandler(this.Help_Click);
            // 
            // ButtonLogin
            // 
            this.ButtonLogin.Location = new System.Drawing.Point(544, 127);
            this.ButtonLogin.Name = "ButtonLogin";
            this.ButtonLogin.Size = new System.Drawing.Size(108, 47);
            this.ButtonLogin.TabIndex = 10;
            this.ButtonLogin.Text = "登录FA";
            this.ButtonLogin.UseVisualStyleBackColor = true;
            this.ButtonLogin.Click += new System.EventHandler(this.ButtonLogin_Click);
            // 
            // mainFrom
            // 
            this.AcceptButton = this.ButtonEnter;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 553);
            this.Controls.Add(this.ButtonLogin);
            this.Controls.Add(this.Help);
            this.Controls.Add(label1);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.ButtonCancle);
            this.Controls.Add(this.ButtonEnter);
            this.Controls.Add(this.input_name);
            this.Controls.Add(info_name);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "mainFrom";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "furaffinity downloader(plus)";
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox input_name;
        private System.Windows.Forms.Button ButtonEnter;
        private System.Windows.Forms.Button ButtonCancle;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.TextBox FilePath;
        private System.Windows.Forms.Button Browse;
        private System.Windows.Forms.Button Help;
        private System.Windows.Forms.TextBox InputMaxPicNum;
        private System.Windows.Forms.TextBox InputPageNum;
        private System.Windows.Forms.TextBox InputStartPicNum;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button ButtonLogin;
    }
}

