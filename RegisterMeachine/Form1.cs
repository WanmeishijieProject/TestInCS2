using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CommonFile.CommonSoftware;

namespace RegisterMeachine
{
    public partial class Form1 : Form
    {
        CommonFile.CommonSoftware Tool = new CommonFile.CommonSoftware();
        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add(EnumTimeOut.TwoDay.ToString());
            comboBox1.Items.Add(EnumTimeOut.OneWeek.ToString());
            comboBox1.Items.Add(EnumTimeOut.HalfMonth.ToString());
            comboBox1.Items.Add(EnumTimeOut.OneMonth.ToString());
            comboBox1.Items.Add(EnumTimeOut.Free.ToString());
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Enum.TryParse(comboBox1.Text, out EnumTimeOut TimeOut);
            var Pswd=Tool.GenPswd(textBox1.Text, TimeOut);
            textBox2.Text = Pswd;
        }
    }
}
