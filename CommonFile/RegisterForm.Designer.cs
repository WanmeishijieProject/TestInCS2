namespace CommonFile
{
    partial class RegisterForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textbox_MachineKey = new System.Windows.Forms.TextBox();
            this.textBox_RegisterKey = new System.Windows.Forms.TextBox();
            this.button_Register = new System.Windows.Forms.Button();
            this.button_Cancle = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(29, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "机器码:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(29, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "注册码:";
            // 
            // textbox_MachineKey
            // 
            this.textbox_MachineKey.Location = new System.Drawing.Point(99, 42);
            this.textbox_MachineKey.Name = "textbox_MachineKey";
            this.textbox_MachineKey.ReadOnly = true;
            this.textbox_MachineKey.Size = new System.Drawing.Size(673, 21);
            this.textbox_MachineKey.TabIndex = 1;
            // 
            // textBox_RegisterKey
            // 
            this.textBox_RegisterKey.Location = new System.Drawing.Point(99, 88);
            this.textBox_RegisterKey.Name = "textBox_RegisterKey";
            this.textBox_RegisterKey.Size = new System.Drawing.Size(673, 21);
            this.textBox_RegisterKey.TabIndex = 1;
            // 
            // button_Register
            // 
            this.button_Register.Location = new System.Drawing.Point(569, 134);
            this.button_Register.Name = "button_Register";
            this.button_Register.Size = new System.Drawing.Size(90, 37);
            this.button_Register.TabIndex = 2;
            this.button_Register.Text = "注册";
            this.button_Register.UseVisualStyleBackColor = true;
            this.button_Register.Click += new System.EventHandler(this.button_Register_Click);
            // 
            // button_Cancle
            // 
            this.button_Cancle.Location = new System.Drawing.Point(679, 134);
            this.button_Cancle.Name = "button_Cancle";
            this.button_Cancle.Size = new System.Drawing.Size(90, 37);
            this.button_Cancle.TabIndex = 2;
            this.button_Cancle.Text = "取消";
            this.button_Cancle.UseVisualStyleBackColor = true;
            this.button_Cancle.Click += new System.EventHandler(this.button_Cancle_Click);
            // 
            // RegisterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(789, 194);
            this.Controls.Add(this.button_Cancle);
            this.Controls.Add(this.button_Register);
            this.Controls.Add(this.textBox_RegisterKey);
            this.Controls.Add(this.textbox_MachineKey);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "RegisterForm";
            this.Text = "注册";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textbox_MachineKey;
        private System.Windows.Forms.TextBox textBox_RegisterKey;
        private System.Windows.Forms.Button button_Register;
        private System.Windows.Forms.Button button_Cancle;
    }
}