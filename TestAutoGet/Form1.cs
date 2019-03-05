using IniFileOperator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestAutoGet
{
    public partial class Form1 : Form
    {
        const string FILE_INI = @"C:\Users\Administrator\AppData\Local\Temp\AutoGet\FIBER.ini";
        IniFile iniFile = new IniFile(FILE_INI);
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            iniFile.IniWriteValue("System", "Action", "1");
            textBox1.Text = iniFile.IniReadValue("System", "LastErrorCode");
        }  
    }
}
