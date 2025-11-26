using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSJianCeXianTi.Lei;
using ATSJianCeXianTi.Model;
using ATSJianCeXianTi.PeiFangFrm;
using ATSJianMianJK.QuanXian;

namespace ATSJianCeXianTi.JKKJ.XianShi
{
    public partial class TDLanKJ : UserControl
    {

        public bool IsPingBi { get; set; } = false;

        public string PeiFangName { get; set; } = "";
        public string DiBuMiaoSu { get; set; } = "";
        public string ZhongBuMiaoSu { get; set; } = "";
    
        private RectangleF JiXingRec = new RectangleF();
        private RectangleF HuanBanRec = new RectangleF();
        private RectangleF GengHuanTangZhengRec = new RectangleF();
        private QuanXianLei QuanXianLei;    
       

        private int TDID = -1;
        private string TDName = "";

        public TDLanKJ()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }
     
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="tdid"></param>
        /// <param name="tDTJModel"></param>
        public void IniData(int tdid,string tdname)
        {
            TDID = tdid;
            TDName=tdname;
            this.Paint += JiShuKJ_Paint;
            this.MouseDoubleClick += label2_MouseClick;
        }

        public void SetQuanXian(int tdid, QuanXianLei quanXian)
        {
            if (TDID==tdid)
            {
                QuanXianLei = quanXian;
            }
        }

        public void ShuaXin()
        {
            this.Refresh();
        }

        private void label2_MouseClick(object sender, MouseEventArgs e)
        {
            Point dian=new Point(e.X,e.Y);
            if (JiXingRec.Contains(dian))
            {
                PeiFangXuanZeFrm peiFangXuanZeFrm = new PeiFangXuanZeFrm();
                peiFangXuanZeFrm.SetCanShu("");
                if (peiFangXuanZeFrm.ShowDialog(this) == DialogResult.OK)
                {
                    PeiFangName = peiFangXuanZeFrm.PeiFangNames;
                    JieMianModel model = new JieMianModel();
                    model.TDID = TDID;
                    model.CaoZuo = true;
                    model.PeiFangName = PeiFangName;
                    ATSJianCeXianTi.Lei.JieMianLei.Cerate().CaoZuo(DoType.JiaZaiPeiFang, model);
                }
            }
            else if (HuanBanRec.Contains(dian))
            {
                JiShuGuanLiLei.Cerate().SetHuanBan(TDID);
            }
            else if (GengHuanTangZhengRec.Contains(dian))
            {
                GengHuanTanZhenFrm gengHuanTanZhenFrm = new GengHuanTanZhenFrm(TDID);
                gengHuanTanZhenFrm.ShowDialog(this);
            }
        }

        private void JiShuKJ_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            RectangleF zongd = new RectangleF();
            zongd.X = 1;
            zongd.Y = 1;
            zongd.Width = this.Width - 2;
            zongd.Height = this.Height - 2;
            RectangleF zuobian = new RectangleF();
            RectangleF youbian = new RectangleF();
            if (IsPingBi == false)
            {

                youbian.X = zongd.X +  zongd.Width /2f;
                youbian.Y = zongd.Y;
                youbian.Width =  zongd.Width / 2f;
                youbian.Height = zongd.Height;

                zuobian.X = zongd.X;
                zuobian.Y = zongd.Y;
                zuobian.Width = zongd.Width / 2;
                zuobian.Height = zongd.Height;
                e.Graphics.DrawLine(Pens.Black, zuobian.X + zuobian.Width, zuobian.Y, zuobian.X + zuobian.Width, zuobian.Y + zuobian.Height);
            }
            else
            {
                youbian.X = zongd.X;
                youbian.Y = zongd.Y;
                youbian.Width = zongd.Width;
                youbian.Height = zongd.Height;
            }
            SolidBrush beijingshua = new SolidBrush(Color.White);
            SolidBrush zitishua = new SolidBrush(Color.Black);
            StringFormat stringFormat = new StringFormat();
            stringFormat.LineAlignment = StringAlignment.Center;
            stringFormat.Alignment = StringAlignment.Near;

            if (IsPingBi==false)
            {
                HuaZouBian(zuobian, beijingshua, zitishua, stringFormat,e.Graphics);
            }

            HuaYouBian(youbian, beijingshua, zitishua, stringFormat, e.Graphics);

            beijingshua.Dispose();
            zitishua.Dispose();
            stringFormat.Dispose();
        }
        private void HuaZouBian(RectangleF zuobian, SolidBrush beijingshua, SolidBrush zitishua, StringFormat stringFormat,Graphics e)
        {
            float zhongbu = 45f;
            {
                RectangleF ss = new RectangleF();
                ss.X = zuobian.X;
                ss.Y = zuobian.Y;
                ss.Width = zuobian.Width;
                ss.Height = zuobian.Height / 2f;
                {
                    RectangleF tongdao = new RectangleF();
                    tongdao.X = ss.X;
                    tongdao.Y = ss.Y;
                    tongdao.Width = ss.Width/4f;
                    tongdao.Height = ss.Height;
                    HuaStr(TDName,e, tongdao, beijingshua,zitishua,stringFormat,35f);
                }
                {
                    RectangleF tongdao = new RectangleF();
                    tongdao.X = ss.X+ ss.Width / 4f;
                    tongdao.Y = ss.Y;
                    tongdao.Width = 3*ss.Width / 4f;
                    tongdao.Height = ss.Height;
                    JiXingRec = new RectangleF();
                    JiXingRec.X = ss.X + ss.Width / 4f;
                    JiXingRec.Y = ss.Y;
                    JiXingRec.Width = 3 * ss.Width / 4f;
                    JiXingRec.Height = ss.Height;
                    HuaStr($"产品机型:{PeiFangName}", e, tongdao, beijingshua, zitishua, stringFormat, 48f);
                }

            }
            {
                RectangleF ss = new RectangleF();
                ss.X = zuobian.X;
                ss.Y = zuobian.Y+ zuobian.Height / 2f;
                ss.Width = zuobian.Width;
                ss.Height = zuobian.Height / 4f;
                HuaStr($"{ZhongBuMiaoSu}", e, ss, beijingshua, zitishua, stringFormat, zhongbu);
            }
            {
                RectangleF ss = new RectangleF();
                ss.X = zuobian.X;
                ss.Y = zuobian.Y + 3*zuobian.Height / 4f;
                ss.Width = zuobian.Width;
                ss.Height = zuobian.Height / 4f;
                HuaStr($"{DiBuMiaoSu}", e, ss, beijingshua, zitishua, stringFormat, zhongbu);
            }
        }

        private void HuaYouBian(RectangleF youbian, SolidBrush beijingshua, SolidBrush zitishua, StringFormat stringFormat, Graphics e)
        {
            float zitigao = 40f;
            {
            
                RectangleF ss = new RectangleF();
                ss.X = youbian.X;
                ss.Y = youbian.Y;
                ss.Width = youbian.Width;
                ss.Height = youbian.Height / 4f;
                TDJiShuModel modesl= JiShuGuanLiLei.Cerate().GetTDCount(TDID);
                if (modesl != null)
                {
                    for (int i = 0; i < 4; i++)
                    {

                        RectangleF sss = new RectangleF();
                        sss.X = ss.X + (i) * ss.Width / 4f;
                        sss.Y = ss.Y;
                        sss.Width = ss.Width / 4f;
                        sss.Height = ss.Height;
                        string miaosu = "";
                        if (i == 0)
                        {
                            miaosu = "通道总数";
                        }
                        else if (i == 1)
                        {
                            int hegeshu = modesl.ZHeGeCount;
                            miaosu = $"PASS:{hegeshu}";
                        }
                        else if (i == 2)
                        {
                            int hegeshu = modesl.ZNGCount;
                            miaosu = $"NG:{hegeshu}";
                        }
                        else if (i == 3)
                        {
                            float hegeshu = modesl.ZHeGeCount;
                            float buhegeshu = modesl.ZNGCount;
                            float zongshu = hegeshu + buhegeshu;
                            if (zongshu == 0)
                            {
                                zongshu = 1;
                            }
                            float bili = (hegeshu / (zongshu)) * 100f;
                            miaosu = $"Yeid(%):{bili.ToString("0.00")}";
                        }
                        HuaStr($"{miaosu}", e, sss, beijingshua, zitishua, stringFormat, zitigao);
                    }
                }
               
            }
            {

                RectangleF ss = new RectangleF();
                ss.X = youbian.X;
                ss.Y = youbian.Y+ youbian.Height / 4f;
                ss.Width = youbian.Width;
                ss.Height = youbian.Height / 4f;
                HuanBanRec = new RectangleF();
                HuanBanRec.X = ss.X;
                HuanBanRec.Y = ss.Y;
                HuanBanRec.Width = ss.Width;
                HuanBanRec.Height =ss.Height;
                TDJiShuModel modesl = JiShuGuanLiLei.Cerate().GetTDCount(TDID);
                if (modesl != null)
                {
                    for (int i = 0; i < 4; i++)
                    {

                        RectangleF sss = new RectangleF();
                        sss.X = ss.X + (i) * ss.Width / 4f;
                        sss.Y = ss.Y;
                        sss.Width = ss.Width / 4f;
                        sss.Height = ss.Height;

                        string miaosu = "";
                        if (i == 0)
                        {
                            miaosu = "换班计数";
                        }
                        else if (i == 1)
                        {
                            int hegeshu = modesl.HBHeGeCount;
                            miaosu = $"PASS:{hegeshu}";
                        }
                        else if (i == 2)
                        {
                            int hegeshu = modesl.HBNGCount;
                            miaosu = $"NG:{hegeshu}";
                        }
                        else if (i == 3)
                        {
                            float hegeshu = modesl.HBHeGeCount;
                            float buhegeshu = modesl.HBNGCount;
                            float zongshu = hegeshu + buhegeshu;
                            if (zongshu == 0)
                            {
                                zongshu = 1;
                            }
                            float bili = (hegeshu / (zongshu)) * 100f;
                            miaosu = $"Yeid(%):{bili.ToString("0.00")}";
                        }
                        HuaStr($"{miaosu}", e, sss, beijingshua, zitishua, stringFormat, zitigao);
                    }
                }

            }
            {
                RectangleF ss = new RectangleF();
                ss.X = youbian.X;
                ss.Y = youbian.Y+ youbian.Height / 2f;
                ss.Width = youbian.Width;
                ss.Height = youbian.Height / 4f;
                Model.PeiFangJiShuModel modesl = JiShuGuanLiLei.Cerate().GetPeiFangCount(TDID);
                if (modesl != null)
                {
                    for (int i = 0; i < 4; i++)
                    {

                        RectangleF sss = new RectangleF();
                        sss.X = ss.X + (i) * ss.Width / 4f;
                        sss.Y = ss.Y;
                        sss.Width = ss.Width / 4f;
                        sss.Height = ss.Height;
                        string miaosu = "";
                        if (i == 0)
                        {
                            miaosu = "配方计数";
                        }
                        else if (i == 1)
                        {
                            int hegeshu = modesl.ZHeGeCount;
                            miaosu = $"PASS:{hegeshu}";
                        }
                        else if (i == 2)
                        {
                            int hegeshu = modesl.ZNGCount;
                            miaosu = $"NG:{hegeshu}";
                        }
                        else if (i == 3)
                        {
                            float hegeshu = modesl.ZHeGeCount;
                            float buhegeshu = modesl.ZNGCount;
                            float zongshu = hegeshu + buhegeshu;
                            if (zongshu == 0)
                            {
                                zongshu = 1;
                            }
                            float bili = (hegeshu / (zongshu)) * 100f;
                            miaosu = $"Yeid(%):{bili.ToString("0.00")}";
                        }
                        HuaStr($"{miaosu}", e, sss, beijingshua, zitishua, stringFormat, zitigao);
                    }
                }
            }
            {
                RectangleF ss = new RectangleF();
                ss.X = youbian.X;
                ss.Y = youbian.Y + 3f*youbian.Height / 4f;
                ss.Width = youbian.Width;
                ss.Height = youbian.Height / 4f;
                GengHuanTangZhengRec = new RectangleF();
                GengHuanTangZhengRec.X = youbian.X;
                GengHuanTangZhengRec.Y = youbian.Y + 3f * youbian.Height / 4f;
                GengHuanTangZhengRec.Width = youbian.Width;
                GengHuanTangZhengRec.Height = youbian.Height / 4f;
                List<ZhenModel> modesl = JiShuGuanLiLei.Cerate().GetTangZhengCount(TDID);
                if (modesl != null)
                {
                    for (int i = 0; i < 3; i++)
                    {

                        RectangleF sss = new RectangleF();
                        sss.X = ss.X + (i) * ss.Width / 3f;
                        sss.Y = ss.Y;
                        sss.Width = ss.Width / 3f;
                        sss.Height = ss.Height;
                        string miaosu = "";
                        if (i == 0)
                        {
                            miaosu = "探针计数";
                        }
                        else
                        {
                            int count = i - 1;
                            if (count < modesl.Count)
                            {
                                int yushu = modesl[count].XianZhiCount - modesl[count].ShiYongCount;
                                string yss = modesl[count].IsGaoPin == 1 ? "高频余数:" : "低频余数:";
                                miaosu = $"{yss}{yushu}";
                            }

                        }

                        HuaStr($"{miaosu}", e, sss, beijingshua, zitishua, stringFormat, zitigao);
                    }
                }
            }
        }
        private void HuaStr(string mingcheng, Graphics e, RectangleF rec, SolidBrush beijinghuam,SolidBrush zitihuashua, StringFormat stringFormat,float jichugao)
        {
            float daxiao = GetWenZiDaXiao(rec.Height, jichugao);
            Font ziti = new Font("微软雅黑", daxiao);
            e.FillRectangle(beijinghuam, rec);
            e.DrawString($"{mingcheng}", ziti, zitihuashua, rec, stringFormat);
            e.DrawRectangle(Pens.YellowGreen, rec.X,rec.Y,rec.Width,rec.Height);
            ziti.Dispose();
        }


        private float GetWenZiDaXiao(float gaodu, float bilidaxiao)
        {
            float daxiao = (gaodu * 12f) / bilidaxiao;
            return daxiao;
        }
    }
}
