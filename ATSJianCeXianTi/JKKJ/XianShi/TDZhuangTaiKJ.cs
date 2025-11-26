using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATSJianCeXianTi.JKKJ.PeiZhiKJ
{
    public partial class TDZhuangTaiKJ : UserControl
    {
        public TDZhuangTaiKJ()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        public float BiLi { get; set; } = 0;
        public string TiaoMa { get; set; } = "";

        public string MiaoSu { get; set; } = "";

        public string JiTime { get; set; } = "";

        /// <summary>
        /// 0表示未检测 1表示合格 2表示不合格
        /// </summary>
        public int IsHeGe { get; set; } = 0;
        private void TDZhuangTaiKJ_Load(object sender, EventArgs e)
        {
            this.Paint += TDZhuangTaiKJ_Paint;
        }
        public void KaiShiHua()
        {
            this.Refresh();
        }
        private void TDZhuangTaiKJ_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            RectangleF zong = new RectangleF();
            zong.X = 1;
            zong.Y = 1;
            zong.Width = this.Width - 2;
            zong.Height = this.Height - 2;
            SolidBrush solidBrush = new SolidBrush(Color.Black);
            Font ziti = new Font("微软姚黑",12);
            StringFormat stringFormat = new StringFormat();
           
            {
                stringFormat.LineAlignment = StringAlignment.Center;
                stringFormat.Alignment = StringAlignment.Near;
                RectangleF rectangleF = new RectangleF();
                rectangleF.X = zong.X;
                rectangleF.Y = zong.Y;
                rectangleF.Width = zong.Width / 3f;
                rectangleF.Height = zong.Height;
                solidBrush.Color = Color.LightGreen;
                e.Graphics.FillRectangle(solidBrush, rectangleF);
                solidBrush.Color = Color.Black;
                e.Graphics.DrawString($"扫码:{TiaoMa}", ziti, solidBrush, rectangleF, stringFormat);
            }
            {
                stringFormat.LineAlignment = StringAlignment.Center;
                stringFormat.Alignment = StringAlignment.Center;
                RectangleF rectangleF = new RectangleF();
                rectangleF.X = zong.X + zong.Width / 3f;
                rectangleF.Y = zong.Y;
                rectangleF.Width = zong.Width / 2f;
                rectangleF.Height = zong.Height;
            
                if (IsHeGe == 1)
                {
                    solidBrush.Color = Color.Green;
                    BiLi = 1;
                    e.Graphics.FillRectangle(solidBrush, rectangleF);
                }
                else if (IsHeGe == 2)
                {
                    solidBrush.Color = Color.Red;
                    BiLi = 1;
                    e.Graphics.FillRectangle(solidBrush, rectangleF);
                }
                else
                {
                    solidBrush.Color = Color.White;
                    e.Graphics.FillRectangle(solidBrush, rectangleF);
                    {
                        RectangleF bilirec = new RectangleF();
                        bilirec.X = rectangleF.X;
                        bilirec.Y = rectangleF.Y;
                        bilirec.Width = rectangleF.Width*BiLi;
                        bilirec.Height = rectangleF.Height;
                        solidBrush.Color = Color.LightBlue;
                        e.Graphics.FillRectangle(solidBrush, bilirec);
                    }
                  
                }
             
                solidBrush.Color = Color.Black;
                e.Graphics.DrawString($"{MiaoSu} 完成{BiLi*100}%", ziti, solidBrush, rectangleF, stringFormat);
            }
            {
                stringFormat.LineAlignment = StringAlignment.Center;
                stringFormat.Alignment = StringAlignment.Far;
                RectangleF rectangleF = new RectangleF();
                rectangleF.X = zong.X + zong.Width / 3f+ zong.Width / 2f;
                rectangleF.Y = zong.Y;
                rectangleF.Width = zong.Width / 6f;
                rectangleF.Height = zong.Height;
                solidBrush.Color = Color.LightYellow;         
                e.Graphics.FillRectangle(solidBrush, rectangleF);
                solidBrush.Color = Color.Black;
                e.Graphics.DrawString($"计时(s):{JiTime}", ziti, solidBrush, rectangleF, stringFormat);
            }
            solidBrush.Dispose();
            ziti.Dispose();
            stringFormat.Dispose();
        }
    }
}
