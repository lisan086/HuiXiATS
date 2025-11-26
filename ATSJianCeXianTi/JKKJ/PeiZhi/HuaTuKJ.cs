using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATSJianCeXianTi.JKKJ.PeiZhi
{
    public partial class HuaTuKJ : UserControl
    {
        private Pen HuaBi;
        private SolidBrush HuaShua;
        private Font ZiTi;
        
        private List<ZhuXingModel> LisShuJu = new List<ZhuXingModel>();
        public HuaTuKJ()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            HuaBi = new Pen(Color.Red, 1);
            //HuaBi.StartCap = LineCap.Round;
            //HuaBi.EndCap = LineCap.Triangle;
            HuaShua = new SolidBrush(Color.Red);
            ZiTi = new Font("微软姚黑", 10);
        }
        private float _PeiLv = 1;
        public float PeiLv
        {
            get { return _PeiLv; }
            set { _PeiLv = value; }
        }
        public void SetShuJu(List<ZhuXingModel> lis)
        {
            LisShuJu = lis;
        }
        public void ShuaXin()
        {
            this.Invalidate();
        }

        public void Clear()
        {
            //ShuJu.Clear();

        }

        private void HuaTuKJ_Load(object sender, EventArgs e)
        {
            this.Paint += ZhuXingKJ_Paint;
            this.SizeChanged += ZhuXingKJ_SizeChanged;
        }
        private void ZhuXingKJ_SizeChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void ZhuXingKJ_Paint(object sender, PaintEventArgs e)
        {
            Graphics huabu = e.Graphics;
            huabu.CompositingQuality = CompositingQuality.HighQuality;
            huabu.SmoothingMode = SmoothingMode.HighQuality;
            RectangleF huaban = new RectangleF();
            huaban.X = 1;
            huaban.Y = 1;
            huaban.Width = this.Width - 2;
            huaban.Height = this.Height - 2;
            float kaunjux =2;
            float kaunjuy = huaban.Height-2;
            PointF YuanDain = new PointF(kaunjux, kaunjuy);
            {
                RectangleF huaban1 = new RectangleF();
                huaban1.X = YuanDain.X - 2;
                huaban1.Y = YuanDain.Y - 2;
                huaban1.Width = 4;
                huaban1.Height = 4;
                huabu.FillRectangle(HuaShua, huaban1);
            }

            #region 画数据
            if (LisShuJu.Count > 0)
            {
                List<PointF> lisdian = new List<PointF>();
                for (int i = 0; i < LisShuJu.Count; i++)
                {
                    PointF dian = new PointF(LisShuJu[i].XValue* _PeiLv, LisShuJu[i].YValue * _PeiLv);
                    PointF zhuanhuan = ZuoBiaoHuanSuan(dian, YuanDain);
                    if (zhuanhuan.X >= 0 && zhuanhuan.X <= this.Width)
                    {
                        if (zhuanhuan.Y >= 0 && zhuanhuan.Y <= this.Height)
                        {
                        
                            lisdian.Add(zhuanhuan);
                        }
                    }
                }
                if (lisdian.Count > 0)
                {
                    for (int i = 0; i < lisdian.Count; i++)
                    {
                        huabu.FillEllipse(HuaShua, new RectangleF(lisdian[i].X - 1f, lisdian[i].Y - 1f, 2, 2));
                    }
                 
                }
            }
         
            #endregion

        }

 
        private PointF ZuoBiaoHuanSuan(PointF pointF, PointF Yuandian)
        {
            PointF huansuanpointF = new PointF(0, 0);
            huansuanpointF.X = pointF.X + Yuandian.X;
            huansuanpointF.Y = Yuandian.Y - pointF.Y;
            return huansuanpointF;
        }
    }
    public class ZhuXingModel
    {
        public float XValue { get; set; }

        public float YValue { get; set; }
     

       public int DianWei { get; set; }
    }
}
