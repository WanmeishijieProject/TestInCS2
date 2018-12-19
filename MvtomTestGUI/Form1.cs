using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LinearRoomMvtemLib;

namespace MvtomTestGUI
{
    public partial class Form1 : Form
    {
        MvtemZoom MvtomLen;
        private delegate void SetText(string Text);
        public Form1()
        {
            InitializeComponent();
            MvtomLen = new MvtemZoom(14, 9600);
            MvtomLen.OnPuseChanged += OnPuseErrorChanged;
            MvtomLen.OnStateChanged += OnStateChanged;
        }

        private void BtnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                MvtomLen.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnHome_Click(object sender, EventArgs e)
        {
            MvtomLen.Home();
        }

        private void BtnDesZoomRate_Click(object sender, EventArgs e)
        {
            if(double.TryParse(textBox_ZoomRate.Text, out double Rate))
                MvtomLen.ZoomRandom(Rate);
        }

        private void BtnReadRate_Click(object sender, EventArgs e)
        {
            try
            {
                var Rate = MvtomLen.ReadCurZoomRate();
                MvtomLen.ReadCurMotionState();
                textBox1.Text = Rate.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void OnPuseErrorChanged(object sender, PuseChangedArgs e)
        {
            switch (e.PuseType)
            {
                case EnumPuseChangedType.HomePuseError:
                    BeginInvoke(new SetText(
                        str => { textBox_HomePuseError.Text = str; }
                        ),e.CurrentValue.ToString());
                    break;
                case EnumPuseChangedType.ToDesPuseError:
                    if (textBox_CurRatePuseError.InvokeRequired)
                    {
                        BeginInvoke(new SetText(str => {
                            textBox_CurRatePuseError.Text = str;
                        }),e.CurrentValue.ToString());
                    }
                    else
                    {
                        textBox_CurRatePuseError.Text = e.CurrentValue.ToString();
                    }
                    break ;
                default:
                    break;
            }
        }

        private void OnStateChanged(object sender, MotionStateChangedArgs e)
        {
            switch (e.StateType)
            {
                case EnumState.CurrentState:
                    Console.WriteLine($"{e.StateType.ToString()}:{e.State.ToString()}");
                    break;
                case EnumState.MotionState:
                    Console.WriteLine($"{e.StateType.ToString()}:{e.State.ToString()}");
                    break;
                case EnumState.TemState:
                    Console.WriteLine($"{e.StateType.ToString()}:{e.State.ToString()}");
                    break;
                default:
                    break;
            }
        }
    }
}
