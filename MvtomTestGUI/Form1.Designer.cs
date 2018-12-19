namespace MvtomTestGUI
{
    partial class Form1
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
            this.BtnOpen = new System.Windows.Forms.Button();
            this.BtnHome = new System.Windows.Forms.Button();
            this.textBox_ZoomRate = new System.Windows.Forms.TextBox();
            this.BtnDesZoomRate = new System.Windows.Forms.Button();
            this.BtnReadRate = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox_HomePuseError = new System.Windows.Forms.TextBox();
            this.textBox_CurRatePuseError = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // BtnOpen
            // 
            this.BtnOpen.Location = new System.Drawing.Point(36, 27);
            this.BtnOpen.Name = "BtnOpen";
            this.BtnOpen.Size = new System.Drawing.Size(134, 48);
            this.BtnOpen.TabIndex = 0;
            this.BtnOpen.Text = "Open";
            this.BtnOpen.UseVisualStyleBackColor = true;
            this.BtnOpen.Click += new System.EventHandler(this.BtnOpen_Click);
            // 
            // BtnHome
            // 
            this.BtnHome.Location = new System.Drawing.Point(188, 30);
            this.BtnHome.Name = "BtnHome";
            this.BtnHome.Size = new System.Drawing.Size(134, 48);
            this.BtnHome.TabIndex = 1;
            this.BtnHome.Text = "Home";
            this.BtnHome.UseVisualStyleBackColor = true;
            this.BtnHome.Click += new System.EventHandler(this.BtnHome_Click);
            // 
            // textBox_ZoomRate
            // 
            this.textBox_ZoomRate.Location = new System.Drawing.Point(36, 133);
            this.textBox_ZoomRate.Multiline = true;
            this.textBox_ZoomRate.Name = "textBox_ZoomRate";
            this.textBox_ZoomRate.Size = new System.Drawing.Size(133, 50);
            this.textBox_ZoomRate.TabIndex = 2;
            // 
            // BtnDesZoomRate
            // 
            this.BtnDesZoomRate.Location = new System.Drawing.Point(196, 132);
            this.BtnDesZoomRate.Name = "BtnDesZoomRate";
            this.BtnDesZoomRate.Size = new System.Drawing.Size(125, 50);
            this.BtnDesZoomRate.TabIndex = 3;
            this.BtnDesZoomRate.Text = "指定倍率";
            this.BtnDesZoomRate.UseVisualStyleBackColor = true;
            this.BtnDesZoomRate.Click += new System.EventHandler(this.BtnDesZoomRate_Click);
            // 
            // BtnReadRate
            // 
            this.BtnReadRate.Location = new System.Drawing.Point(41, 242);
            this.BtnReadRate.Name = "BtnReadRate";
            this.BtnReadRate.Size = new System.Drawing.Size(127, 59);
            this.BtnReadRate.TabIndex = 4;
            this.BtnReadRate.Text = "读取倍率";
            this.BtnReadRate.UseVisualStyleBackColor = true;
            this.BtnReadRate.Click += new System.EventHandler(this.BtnReadRate_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(196, 244);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(125, 56);
            this.textBox1.TabIndex = 5;
            // 
            // textBox_HomePuseError
            // 
            this.textBox_HomePuseError.Location = new System.Drawing.Point(382, 32);
            this.textBox_HomePuseError.Multiline = true;
            this.textBox_HomePuseError.Name = "textBox_HomePuseError";
            this.textBox_HomePuseError.Size = new System.Drawing.Size(207, 45);
            this.textBox_HomePuseError.TabIndex = 6;
            // 
            // textBox_CurRatePuseError
            // 
            this.textBox_CurRatePuseError.Location = new System.Drawing.Point(382, 132);
            this.textBox_CurRatePuseError.Multiline = true;
            this.textBox_CurRatePuseError.Name = "textBox_CurRatePuseError";
            this.textBox_CurRatePuseError.Size = new System.Drawing.Size(207, 45);
            this.textBox_CurRatePuseError.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.textBox_CurRatePuseError);
            this.Controls.Add(this.textBox_HomePuseError);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.BtnReadRate);
            this.Controls.Add(this.BtnDesZoomRate);
            this.Controls.Add(this.textBox_ZoomRate);
            this.Controls.Add(this.BtnHome);
            this.Controls.Add(this.BtnOpen);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnOpen;
        private System.Windows.Forms.Button BtnHome;
        private System.Windows.Forms.TextBox textBox_ZoomRate;
        private System.Windows.Forms.Button BtnDesZoomRate;
        private System.Windows.Forms.Button BtnReadRate;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox_HomePuseError;
        private System.Windows.Forms.TextBox textBox_CurRatePuseError;
    }
}

