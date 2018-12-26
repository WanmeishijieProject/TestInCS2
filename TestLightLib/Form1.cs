using JKLightSourceLib;
using JKLightSourceLib.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestLightLib
{
    public partial class Form1 : Form
    {
        private JKLightSource JKController = new JKLightSource(14,9600);
        public Form1()
        {
            InitializeComponent();
            JKController.Open();
        }

        private void BtnOpenLight_Click(object sender, EventArgs e)
        {
            var value=JKController.ReadValue(EnumChannel.CH1);
            if(value!=0)
                JKController.CloseChannelLight(EnumChannel.CH1);
            else
                JKController.OpenChannelLight(EnumChannel.CH1, 60);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            JKController.WriteValue(EnumChannel.CH1,(UInt16)trackBar1.Value);
            Console.WriteLine(trackBar1.Value);

        }
    }
}
