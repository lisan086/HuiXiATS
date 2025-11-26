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
using BaseUI.UC;

namespace CaiJiKaYinPingFenXiLei.Frm
{
    public partial class QuXian21KJ : UserControl
    {
        public QuXian21KJ()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        
            HuaShua = new SolidBrush(Color.Red);
            HuaBi = new Pen(Color.Red);
        }
        private SolidBrush HuaShua;
        private Pen HuaBi;

        private List<PointF> Dians = new List<PointF>();

       
        private RectangleF rectangleF = new RectangleF(0, 0, 50, 50);
        private float ZuiDaZhiX = 0;

        private float ZuiDaZhiY = 0;

        private float _ZuiDaX = 6f;
        private float _ZuiDaY = 5f;

        private bool _XianShiKeDu = true;

        private Color _ZuoBiaoYanSe = Color.Black;

        private Color _StrYanSe = Color.Black;

        private string _YZhouStr = "采样值";
        private string _XZhouStr = "真实值";
        private float _DuanShuX = 10;
        private float _DuanShuY = 10;

        private float _ZiTiDaXiao = 9;
        private bool _XianShiWanGe = false;
        private bool IsHuiZhi = false;
        private Point ZuoBiao = new Point();
        public string YZhouStr
        {
            get
            {
                return _YZhouStr;
            }

            set
            {
                _YZhouStr = value;
                this.Invalidate();
            }

        }

        public string XZhouStr
        {
            get
            {
                return _XZhouStr;
            }

            set
            {
                _XZhouStr = value;
                this.Invalidate();
            }
        }

        public float ZuiDaX
        {
            get
            {
                return _ZuiDaX;
            }
        }
        public float ZuiDaY
        {
            get
            {
                return _ZuiDaY;
            }


        }
        public bool XianShiKeDu
        {
            get
            {
                return _XianShiKeDu;
            }

            set
            {
                _XianShiKeDu = value;
                this.Invalidate();
            }
        }



        public Color ZuoBiaoYanSe
        {
            get
            {
                return _ZuoBiaoYanSe;
            }

            set
            {
                _ZuoBiaoYanSe = value;
                this.Invalidate();
            }
        }

        public float ZiTiDaXiao
        {
            get
            {
                return _ZiTiDaXiao;
            }

            set
            {
                _ZiTiDaXiao = value;
                this.Invalidate();
            }
        }

        public Color StrYanSe
        {
            get
            {
                return _StrYanSe;
            }

            set
            {
                _StrYanSe = value;
                this.Invalidate();
            }
        }

        public float DuanShuX
        {
            get
            {
                return _DuanShuX;
            }

            set
            {
                _DuanShuX = value;
                this.Invalidate();
            }
        }

        public float DuanShuY
        {
            get
            {
                return _DuanShuY;
            }

            set
            {
                _DuanShuY = value;
                this.Invalidate();
            }
        }

        public bool XianShiWanGe
        {
            get
            {
                return _XianShiWanGe;
            }

            set
            {
                _XianShiWanGe = value;
                this.Invalidate();
            }
        }

        private void QuXian21KJ_Load(object sender, EventArgs e)
        {
            this.Paint += UCXinQuXian_Paint;
            this.MouseMove += UCXinQuXian_MouseMove;
            this.MouseClick += UCXinQuXian_MouseClick;
            this.MouseLeave += UCXinQuXian_MouseLeave;
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            this.Invalidate();
            base.OnSizeChanged(e);
        }
        private void UCXinQuXian_MouseLeave(object sender, EventArgs e)
        {
            IsHuiZhi = false;
        }

        private void UCXinQuXian_MouseMove(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Cross;
            ZuoBiao = new Point(e.X, e.Y);

            this.Refresh();

        }

        private void UCXinQuXian_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                IsHuiZhi = true;
            }
            else
            {

                IsHuiZhi = false;
            }
        }
        private void UCXinQuXian_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                Graphics huabu = e.Graphics;
                huabu.CompositingQuality = CompositingQuality.HighQuality;
                huabu.SmoothingMode = SmoothingMode.HighQuality;
                float jianju = 40;
                rectangleF.X = jianju;
                rectangleF.Y = jianju;
                rectangleF.Width = this.Width - 2 * jianju;
                rectangleF.Height = this.Height - 2 * jianju;

                PointF YuanDian = new PointF(rectangleF.X, rectangleF.Y + 2f*rectangleF.Height / 4f);
                #region 画坐标
                HuaZuoBiao(rectangleF, huabu, YuanDian);
                #endregion
                #region 画移动值
                HuaYiDong(rectangleF, huabu, YuanDian);
                #endregion
                #region 画折线
                HuaZheXian(rectangleF, huabu, YuanDian);
                #endregion
            }
            catch
            {


            }

        }
        private void HuaZuoBiao(RectangleF rectangleF, Graphics e, PointF yuandian)
        {
            HuaShua.Color = _ZuoBiaoYanSe;
            HuaBi.Color = _ZuoBiaoYanSe;
            HuaBi.Width = 1f;

            int chaochu1 = 2;
            float xiaogao = 4;
            Font ZiTi = new Font("微软姚黑", _ZiTiDaXiao);
            List<float> jianju = QiuBiLi(rectangleF, yuandian);
            float bialiy1 = jianju[1];
            float bialix = jianju[0];
            StringFormat stringFormat = new StringFormat();
            stringFormat.LineAlignment = StringAlignment.Center;
            stringFormat.Alignment = StringAlignment.Center;
            #region 画0
            HuaBi.Color = _ZuoBiaoYanSe;
            HuaShua.Color = _ZuoBiaoYanSe;
            e.DrawString("0", ZiTi, HuaShua, new PointF(yuandian.X - 5, yuandian.Y + 1));
            e.DrawLine(HuaBi, yuandian.X, yuandian.Y, yuandian.X, rectangleF.Y);
            e.DrawLine(HuaBi, yuandian.X, yuandian.Y, rectangleF.X + rectangleF.Width, yuandian.Y);
            List<PointF> LisDian = new List<PointF>();
            LisDian.Add(new PointF(yuandian.X, rectangleF.Y));
            LisDian.Add(new PointF(yuandian.X - xiaogao, rectangleF.Y + 2 * xiaogao));
            LisDian.Add(new PointF(yuandian.X + xiaogao, rectangleF.Y + 2 * xiaogao));

            List<PointF> LisDian1 = new List<PointF>();
            LisDian1.Add(new PointF(rectangleF.X + rectangleF.Width, yuandian.Y));
            LisDian1.Add(new PointF(rectangleF.X + rectangleF.Width - 2 * xiaogao, yuandian.Y - xiaogao));
            LisDian1.Add(new PointF(rectangleF.X + rectangleF.Width - 2 * xiaogao, yuandian.Y + xiaogao));
            e.FillPolygon(HuaShua, LisDian.ToArray());
            e.FillPolygon(HuaShua, LisDian1.ToArray());
            #endregion
            #region 画Y轴刻度
            HuaBi.Color = _ZuoBiaoYanSe;
            HuaShua.Color = _ZuoBiaoYanSe;

            HuaBi.Width = 1f;
            int xjilu = 46;
            int zigaodu = ZiTi.Height + 6;
            float y = (_ZuiDaY / _DuanShuY);
            for (int i = 0; i < _DuanShuY; i++)
            {
                PointF pointF1 = new PointF(-chaochu1, ((y * (i + 1))) * bialiy1);
                PointF pointF2 = new PointF(0, ((y * (i + 1)) * bialiy1));
                PointF pointF3 = new PointF(rectangleF.Width + chaochu1, ((y * (i + 1)) * bialiy1));
                PointF zhuanhuan1 = ZuoBiaoHuanSuan(pointF1, yuandian);
                PointF zhuanhuan2 = ZuoBiaoHuanSuan(pointF2, yuandian);
                PointF zhuanhuan3 = ZuoBiaoHuanSuan(pointF3, yuandian);
                if (_XianShiKeDu)
                {
                    e.DrawLine(HuaBi, zhuanhuan1, zhuanhuan2);
                    RectangleF rectangle = new RectangleF(zhuanhuan1.X - xjilu, zhuanhuan1.Y - zigaodu / 2, xjilu, zigaodu);
                    e.DrawString((y * (i + 1)).ToString("0"), ZiTi, HuaShua, rectangle, stringFormat);
                }
                if (_XianShiWanGe)
                {
                    e.DrawLine(HuaBi, zhuanhuan1, zhuanhuan3);
                }
            }
            {
                HuaShua.Color = _StrYanSe;

                RectangleF s = new RectangleF();
                s.X = rectangleF.X - 30;
                s.Y = rectangleF.Y - 40;
                s.Width = (_YZhouStr.Length + 5) * ZiTi.Size + 3;
                s.Height = 30;
                e.DrawString(string.Format("{0}", _YZhouStr), ZiTi, HuaShua, s, stringFormat);

            }

            #endregion

            #region 画X轴
            HuaBi.Color = _ZuoBiaoYanSe;
            HuaShua.Color = _ZuoBiaoYanSe;
            float x = (_ZuiDaX / _DuanShuX);
            int XLi = 20;
            for (int i = 0; i < _DuanShuX; i++)
            {
                PointF pointF1 = new PointF((x * (i + 1)) * bialix, -3);
                PointF pointF2 = new PointF((x * (i + 1)) * bialix, 0);
                PointF pointF3 = new PointF((x * (i + 1)) * bialix, ZuiDaY * bialiy1);
                PointF zhuanhuan1 = ZuoBiaoHuanSuan(pointF1, yuandian);
                PointF zhuanhuan2 = ZuoBiaoHuanSuan(pointF2, yuandian);
                PointF zhuanhuan3 = ZuoBiaoHuanSuan(pointF3, yuandian);
                if (_XianShiKeDu)
                {
                    e.DrawLine(HuaBi, zhuanhuan1, zhuanhuan2);

                    RectangleF rectangle = new RectangleF(zhuanhuan1.X - XLi, zhuanhuan1.Y, 2 * XLi, 20);

                    e.DrawString((x * (i + 1)).ToString("0"), ZiTi, HuaShua, rectangle, stringFormat);
                }
                if (_XianShiWanGe)
                {
                    e.DrawLine(HuaBi, zhuanhuan1, zhuanhuan3);
                }
            }
            {
                HuaShua.Color = _StrYanSe;

                RectangleF s = new RectangleF();
                s.X = rectangleF.X + rectangleF.Width - 80;
                s.Y = rectangleF.Y + rectangleF.Height + ZiTi.Height + 5;
                s.Width = 150;
                s.Height = ZiTi.Height + 5;

                e.DrawString(string.Format("{0}", _XZhouStr), ZiTi, HuaShua, s, stringFormat);
            }
            #endregion
            stringFormat.Dispose();
            ZiTi.Dispose();
        }

        private void HuaYiDong(RectangleF rectangleF, Graphics e, PointF yuandian)
        {
            if (IsHuiZhi)
            {
                HuaBi.Width = 2;
                PointF dian1 = ZuoBiaoHuanSuanN(ZuoBiao, yuandian);
                List<float> jianju = QiuBiLi(rectangleF,yuandian);
                float bialiy = jianju[1];
                float bialix = jianju[0];


                PointF xiandian = new PointF(dian1.X / bialix, dian1.Y / bialiy);
                string zhi = string.Format("({0} , {1})", xiandian.X.ToString("0.00"), xiandian.Y.ToString("0.00"));
                Font ZiTi = new Font("微软姚黑", _ZiTiDaXiao);
                e.DrawString(zhi, ZiTi, HuaShua, new PointF(ZuoBiao.X, ZuoBiao.Y));
                ZiTi.Dispose();
            }

        }
        private void HuaZheXian(RectangleF rectangleF, Graphics e, PointF yuandian)
        {
            HuaBi.Width = 1.5f;
            List<float> jianju = QiuBiLi(rectangleF, yuandian);
            float bialiy = jianju[1];
            float bialix = jianju[0];
            List<PointF> pointFs = new List<PointF>();
            List<PointF> huadian = Dians;
            for (int i = 0; i < huadian.Count; i++)
            {
                PointF dian = new PointF(huadian[i].X * bialix, huadian[i].Y * bialiy);
                pointFs.Add(ZuoBiaoHuanSuan(dian, yuandian));
            }
            HuaBi.Color = Color.Red;
            HuaShua.Color = HuaBi.Color;
            if (pointFs.Count == 1)
            {
                e.FillEllipse(HuaShua, new RectangleF(pointFs[0].X - 4f, pointFs[0].Y - 4f, 8, 8));
            }
            else if (pointFs.Count > 1)
            {
                e.DrawLines(HuaBi, pointFs.ToArray());
            }
        }
        private PointF ZuoBiaoHuanSuan(PointF pointF, PointF yuandian)
        {

            PointF huansuanpointF = new PointF(0, 0);
            huansuanpointF.X = pointF.X + yuandian.X;
            huansuanpointF.Y = yuandian.Y - pointF.Y;
            return huansuanpointF;
        }

        private PointF ZuoBiaoHuanSuanN(PointF pointF, PointF yuandian)
        {

            PointF huansuanpointF = new PointF(0, 0);
            huansuanpointF.X = pointF.X - yuandian.X;
            huansuanpointF.Y = yuandian.Y - pointF.Y;
            return huansuanpointF;
        }
        private List<float> QiuBiLi(RectangleF rectangleF,PointF yuandian)
        {
            List<float> bili = new List<float>();
            bili.Add(0f);//x
            bili.Add(0f);//y1

            {
                float xKeDu = _ZuiDaX / _DuanShuX;
                float XJianJu = (rectangleF.X+(float)rectangleF.Width- yuandian.X) / _DuanShuX;
                float bialix = XJianJu / xKeDu;
                bili[0] = bialix;
            }
            {
                float xKeDu = _ZuiDaY / _DuanShuY;
                float XJianJu = (yuandian.Y-rectangleF.Y) / _DuanShuY;
                float bialix = XJianJu / xKeDu;
                bili[1] = bialix;
            }

            return bili;
        }



        public void AddDian(PointF dian)
        {
            float wucah = 0.3f;

            PointF dian1 = new PointF();
            dian1.X = dian.X;
            dian1.Y = dian.Y;
            Dians.Add(dian1);
            float shangyiuxi = ZuiDaZhiY;
            if (dian.Y >= ZuiDaZhiY + wucah)
            {
                ZuiDaZhiY = dian.Y + 1;
                float dians = dian.Y + 1;
                int zhi = (int)dians + 1;
                int daishu = (int)_DuanShuX;
                int yushu = zhi % daishu;
                int quzheng = zhi / daishu;

                ZuiDaZhiY = quzheng * daishu + daishu;
                if (quzheng % 2 == 0)
                {
                    ZuiDaZhiY = (quzheng + 2) * daishu;
                }
                else
                {
                    ZuiDaZhiY = quzheng * daishu + daishu;
                }

            }
            if (shangyiuxi != ZuiDaZhiY)
            {
                _ZuiDaY =ZuiDaZhiY;
            }
            float shangyiuxi1 = ZuiDaZhiX;
            if (dian.X >= ZuiDaZhiX + wucah)
            {

                float dians = dian.X + 1;
                int zhi = (int)dians + 1;
                int daishu = (int)_DuanShuY;
                int yushu = zhi % daishu;
                int quzheng = zhi / daishu;

                ZuiDaZhiX = quzheng * daishu + daishu;
                if (quzheng % 2 == 0 && false)
                {
                    ZuiDaZhiX = (quzheng + 2) * daishu;
                }
                else
                {
                    ZuiDaZhiX = quzheng * daishu + daishu;
                }

            }
            if (shangyiuxi1 != ZuiDaZhiX)
            {
                _ZuiDaX = ZuiDaZhiX;
            }

        }


        public void KaiShiHua()
        {
            this.Refresh();
        }
        public void ClearData()
        {
            Dians.Clear();
            ZuiDaZhiX = 0;
            ZuiDaZhiY = 0;
        }



        public List<PointF> GetDian()
        {
            List<PointF> dians = new List<PointF>();
            foreach (var item in Dians)
            {
                PointF dian1 = new PointF();
                dian1.X = item.X;
                dian1.Y = item.Y;
                dians.Add(dian1);
            }
            return dians;
        }
    }
}
