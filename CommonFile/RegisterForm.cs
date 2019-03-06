using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonFile
{
    public partial class RegisterForm : Form
    {
        static CommonSoftware software = new CommonSoftware();
        static bool IsCheckSuccess;
        public RegisterForm()
        {
            IsCheckSuccess = false;
            InitializeComponent();
            textbox_MachineKey.Text = software.GetMachineKey();
        }

        private void button_Register_Click(object sender, EventArgs e)
        {
            try
            {
                IsCheckSuccess = software.CheckRegisterKey(textbox_MachineKey.Text, textBox_RegisterKey.Text);
                if (IsCheckSuccess)
                {
                    MessageBox.Show("注册成功");
                    Close();
                }
                else
                {
                    MessageBox.Show("注册失败");
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show($"注册失败{ex.Message}");
                IsCheckSuccess = false;
            }
        }


        

        /// <summary>
        /// 检查是否过期
        /// </summary>
        /// <param name="DaysLeft"></param>
        /// <returns></returns>
        public static bool CheckIsTimeout(out double DaysLeft)
        {
            var Ret = software.CheckFile(out DaysLeft);
            //MessageBox.Show($"{Ret}");
            if (!Ret)
            {
                var dlg = new RegisterForm();
                dlg.ShowDialog();

                //MessageBox.Show("1");

                if (IsCheckSuccess)
                {
                    //MessageBox.Show("2");
                    Ret = true;
                    DaysLeft = software.DaysLeft;
                    dlg.Close();
                }
                else
                {
                    //MessageBox.Show("3");
                    Ret = false;
                    DaysLeft = 0;

                }

            }
            return Ret;
        }
  

        private void button_Cancle_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
