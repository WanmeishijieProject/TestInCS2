﻿using FFmpegDemo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FFMPEG拉流测试
{
    public partial class Form1 : Form
    {
        tstRtmp rtmp = new tstRtmp();
        Thread thPlayer;

        public Form1()
        {
            InitializeComponent();
        }
      
        private void BtnPlay_Click(object sender, EventArgs e)
        {
            BtnPlay.Enabled = false;
            if (thPlayer != null)
            {
                rtmp.Stop();

                thPlayer = null;
            }
            else
            {
                thPlayer = new Thread(DeCoding);
                thPlayer.IsBackground = true;
                thPlayer.Start();
                BtnPlay.Text = "停止播放";
                BtnPlay.Enabled = true;
            }
        }

        /// <summary>
        /// 播放线程执行方法
        /// </summary>
        private unsafe void DeCoding()
        {
            try
            {
                Console.WriteLine("DeCoding run...");
                Bitmap oldBmp = null;


                // 更新图片显示
                tstRtmp.ShowBitmap show = (bmp) =>
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        this.pic.Image = bmp;
                        if (oldBmp != null)
                        {
                            oldBmp.Dispose();
                        }
                        oldBmp = bmp;
                    }));
                };
                rtmp.Start(show, txtUrl.Text.Trim());

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.WriteLine("DeCoding exit");
                this.Invoke(new MethodInvoker(() =>
                {
                    BtnPlay.Text = "开始播放";
                    BtnPlay.Enabled = true;
                }));
            }
        }
    }
}
