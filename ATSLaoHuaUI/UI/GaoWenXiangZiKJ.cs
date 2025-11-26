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
using System.Xml.Linq;
using ATSJianMianJK;
using ATSLaoHuaUI.PeiZhi.Frm;
using ZuZhuangUI.Lei;
using ZuZhuangUI.Model;

namespace ATSLaoHuaUI.UI
{
    public partial class GaoWenXiangZiKJ : UserControl
    {
        private SheBeiZhanModel SheBeiZhan = new SheBeiZhanModel();
        private List<MaTDModel> LisData = new List<MaTDModel>();
      
        private ZiYuanModel ZY;
        private RectangleF QiDongKuanRec = new RectangleF();
      
        private List<RectangleF> ShouDongTiaoShiQuRec = new List<RectangleF>();
        private int MoShi = 0;
        public GaoWenXiangZiKJ()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        public void SetCanShu(SheBeiZhanModel model)
        {
            LisData.Clear();
            SheBeiZhan = model;
            for (int i = 0; i < SheBeiZhan.LisMaTD.Count; i++)
            {
                LisData.Add(SheBeiZhan.LisMaTD[i]);
            }
          
        }
        public void SetCanShu( ZiYuanModel ziYuanModel)
        {
            ZY = ziYuanModel;
        }

        public void SetMoShi(int moshi)
        {
            MoShi = moshi;
        }
        private void GaoWenXiangZiKJ_Load(object sender, EventArgs e)
        {
            this.Paint += GaoWenXiangZiKJ_Paint;
            this.MouseClick += GaoWenXiangZiKJ_MouseClick;
           
        }

        private void GaoWenXiangZiKJ_MouseClick(object sender, MouseEventArgs e)
        {
            if (QiDongKuanRec.Contains(new Point(e.X, e.Y)))
            {
                if (SheBeiZhan.TestState == 0 || SheBeiZhan.TestState == 1)
                {
                    List<string> canshu = new List<string>();
                    for (int i = 0; i < LisData.Count; i++)
                    {
                        canshu.Add(LisData[i].BanMa);
                    }
                    MaFrm maFrm = new MaFrm();
                    maFrm.SetCanShu(SheBeiZhan.GaoWenName, SheBeiZhan.GWID, LisData.Count, canshu);
                    if (maFrm.ShowDialog(this) == DialogResult.OK)
                    {
                        List<string> saoma = maFrm.SaoMaShuJu;
                        JieMianCaoZuoModel model = new JieMianCaoZuoModel();
                        model.GWID = SheBeiZhan.GWID;
                        model.CanShu = saoma;
                        JieMianCaoZuoLei.CerateDanLi().JieMianCuoZuo(DoType.SaoMaQueRen, model);
                    }
                }
            }
            else 
            {
                if (MoShi == 1)
                {
                    if (SheBeiZhan.TestState != 2)
                    {
                        for (int i = 0; i < ShouDongTiaoShiQuRec.Count; i++)
                        {
                            if (ShouDongTiaoShiQuRec[i].Contains(new Point(e.X, e.Y)))
                            {
                                JieMianCaoZuoModel model = new JieMianCaoZuoModel();
                                model.GWID = SheBeiZhan.GWID;
                                model.CanShu = LisData[i].MaTDID;
                                JieMianCaoZuoLei.CerateDanLi().JieMianCuoZuo(DoType.ShouDongSaoMaWeiTiaoShi, model);
                                break;
                            }
                        }
                    }
                    else
                    {
                        ZY.TiShiKuang("设备在工作");
                    }
                }
            }
        }

        private void GaoWenXiangZiKJ_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            RectangleF zongd = new RectangleF();
            zongd.X = 1;
            zongd.Y = 1;
            zongd.Width = this.Width - 2;
            zongd.Height = this.Height - 2;
            float gaodu = 85;
            {
                RectangleF rectangleF = new RectangleF();
                rectangleF.X = zongd.X;
                rectangleF.Y = zongd.Y;
                rectangleF.Width = zongd.Width;
                rectangleF.Height = gaodu;
                QiDongKuanRec = new RectangleF();
                QiDongKuanRec.X = zongd.X;
                QiDongKuanRec.Y = zongd.Y;
                QiDongKuanRec.Width = zongd.Width;
                QiDongKuanRec.Height = gaodu;
                HuaBiaoTiLan(rectangleF,e.Graphics);
            }
            {
                RectangleF rectangleF = new RectangleF();
                rectangleF.X = zongd.X;
                rectangleF.Y = zongd.Y+gaodu;
                rectangleF.Width = zongd.Width;
                rectangleF.Height = zongd.Height-gaodu;
                HuaChanPing(rectangleF, e.Graphics);
            }
        }

        private void HuaBiaoTiLan(RectangleF bitikuang,Graphics e)
        {
            StringFormat stringFormat = new StringFormat();
            stringFormat.LineAlignment = StringAlignment.Center;
            stringFormat.Alignment = StringAlignment.Near;
            SolidBrush huashua = new SolidBrush(Color.AliceBlue);
            Font ziti = new Font("微软姚黑",15);
            Font ziti1 = new Font("微软姚黑", 11);
          
            {
                Color yanse = Color.BlueViolet;
                huashua.Color = yanse;
                e.FillRectangle(huashua, bitikuang);
            }
            float fenshu = 3;
            float kuangdu = bitikuang.Width / fenshu;
            float gaodu = bitikuang.Height / 2f;
            {
             
                RectangleF kuan = new RectangleF();
                kuan.X = bitikuang.X ;
                kuan.Y = bitikuang.Y;
                kuan.Width = kuangdu;
                kuan.Height = bitikuang.Height;
                huashua.Color = Color.White;
                string yunxing = SheBeiZhan.IsYunXing == 1 ? "设备运行" : "设备未运行";
                string name = $"{yunxing}:{SheBeiZhan.MiaoSu}";
                e.DrawString(name, ziti, huashua, kuan, stringFormat);
            }
            {
                {
                    string name = $"设定温度(℃):{SheBeiZhan.WenDuLow}-{SheBeiZhan.WenDuUp}";
                    RectangleF kuan = new RectangleF();
                    kuan.X = bitikuang.X + 1 * kuangdu;
                    kuan.Y = bitikuang.Y;
                    kuan.Width = kuangdu;
                    kuan.Height = gaodu;
                    Color yanse = Color.White;
                    e.DrawString(name, ziti1, huashua, kuan, stringFormat);
                }
                {
                    string  name = $"       温度(℃):{SheBeiZhan.ShiShiWenDu.ToString("0.00")}";
                    RectangleF kuan = new RectangleF();
                    kuan.X = bitikuang.X + 1 * kuangdu;
                    kuan.Y = bitikuang.Y+gaodu;
                    kuan.Width = kuangdu;
                    kuan.Height = gaodu;
                    Color yanse = Color.White;
                    e.DrawString(name, ziti1, huashua, kuan, stringFormat);
                }
                {
                    string name = $"总时长(min):{SheBeiZhan.SetTestTime}";
                    RectangleF kuan = new RectangleF();
                    kuan.X = bitikuang.X + 2 * kuangdu;
                    kuan.Y = bitikuang.Y;
                    kuan.Width = kuangdu;
                    kuan.Height = gaodu;
                    Color yanse = Color.White;
                    e.DrawString(name, ziti1, huashua, kuan, stringFormat);
                }
                {
                    string name = $"    剩余(min):{(SheBeiZhan.SetTestTime - SheBeiZhan.TestTime).ToString("0.00")}";
                    RectangleF kuan = new RectangleF();
                    kuan.X = bitikuang.X + 2 * kuangdu;
                    kuan.Y = bitikuang.Y+gaodu;
                    kuan.Width = kuangdu;
                    kuan.Height = gaodu;
                    Color yanse = Color.White;
                    e.DrawString(name, ziti1, huashua, kuan, stringFormat);
                }
             
            }
           
            huashua.Dispose();
            stringFormat.Dispose();
            ziti.Dispose();
            ziti1.Dispose();
        }


        public void HuaChanPing(RectangleF bitikuang, Graphics e)
        {
            StringFormat stringFormat = new StringFormat();
            stringFormat.LineAlignment = StringAlignment.Center;
            stringFormat.Alignment = StringAlignment.Near;
            SolidBrush huashua = new SolidBrush(Color.AliceBlue);
            Font ziti = new Font("微软姚黑", 12);
         
            int lieshu = 4;
            int count = LisData.Count;
            int hengcount = count / lieshu;
            int yushu = count % lieshu;
            if (yushu>0)
            {
                hengcount += 1;
            }
            float kuan= bitikuang.Width/ (float)lieshu;
            float gao = bitikuang.Height / (float)hengcount;
            int jishu = 0;
            for (int i = 0; i < hengcount; i++)
            {
                for (int j = 0; j < lieshu; j++)
                {
                    RectangleF rectangleF = new RectangleF();
                    rectangleF.X = bitikuang.X+kuan*j;
                    rectangleF.Y = bitikuang.Y + gao*i;
                    rectangleF.Width = kuan;
                    rectangleF.Height = gao;
                    {
                        Color yanse = Color.White;
                        huashua.Color = yanse;
                        e.FillRectangle(huashua, rectangleF);
                    }
                    {
                        float rows = 4;
                        float xingao= rectangleF.Height/rows;
                        //产品标题
                        {
                            stringFormat.LineAlignment = StringAlignment.Center;
                            stringFormat.Alignment = StringAlignment.Center;
                            RectangleF shourec = new RectangleF();
                            shourec.X = rectangleF.X;
                            shourec.Y = rectangleF.Y;
                            shourec.Width = rectangleF.Width;
                            shourec.Height = xingao;
                            Color yanse = Color.Gray;
                            huashua.Color = yanse;
                            e.FillRectangle(huashua, shourec);
                            string name = GetMa(jishu);
                            huashua.Color = Color.Black;
                            e.DrawString(name, ziti, huashua, shourec, stringFormat);
                            ShouDongTiaoShiQuRec.Add(shourec);
                        }
                        {
                          
                            stringFormat.LineAlignment = StringAlignment.Center;
                            stringFormat.Alignment = StringAlignment.Center;                       
                            List<string> name = GetShuJu(jishu);
                            if (name.Count>0)
                            {
                                if (name.Count == 1)
                                {
                                    RectangleF shourec = new RectangleF();
                                    shourec.X = rectangleF.X;
                                    shourec.Y = rectangleF.Y + xingao ;
                                    shourec.Width = rectangleF.Width;
                                    shourec.Height = xingao * 2;
                                    huashua.Color = Color.Black;
                                    e.DrawString(name[0], ziti, huashua, shourec, stringFormat);
                                }
                                else
                                {
                                    for (int d = 0; d < 2; d++)
                                    {
                                        RectangleF shourec = new RectangleF();
                                        shourec.X = rectangleF.X;
                                        shourec.Y = rectangleF.Y + xingao * (d+1);
                                        shourec.Width = rectangleF.Width;
                                        shourec.Height = xingao ;
                                        huashua.Color = Color.Black;
                                        e.DrawString(name[d], ziti, huashua, shourec, stringFormat);
                                    }
                                }
                               
                            }
                          
                        }
                        //底部
                        {

                            stringFormat.LineAlignment = StringAlignment.Center;
                            stringFormat.Alignment = StringAlignment.Center;
                            float kuangbi = 0;
                            bool isshibai = false;
                            string name = GetWeiBu(jishu,out kuangbi,out isshibai);


                            RectangleF shourec = new RectangleF();
                            shourec.X = rectangleF.X-1;
                            shourec.Y = rectangleF.Y + xingao * 3;
                            shourec.Width = rectangleF.Width-1;
                            shourec.Height = xingao-1;
                            huashua.Color = Color.Yellow;
                            e.FillRectangle(huashua, shourec);
                            {
                                RectangleF luse = new RectangleF();
                                luse.X = shourec.X;
                                luse.Y = shourec.Y;
                                luse.Width = kuangbi* shourec.Width;
                                luse.Height = xingao;
                                if (isshibai)
                                {
                                    huashua.Color = Color.Red;
                                    e.FillRectangle(huashua, shourec);
                                }
                                else
                                {
                                    huashua.Color = Color.Green;
                                    e.FillRectangle(huashua, luse);
                                }
                            
                            }

                            huashua.Color = Color.Black;
                            e.DrawString(name, ziti, huashua, shourec, stringFormat);

                        }
                    }
                    e.DrawRectangle(Pens.Black, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
                    jishu++;
                }
            }
        
            huashua.Dispose();
            stringFormat.Dispose();
            ziti.Dispose();
          
        }


        public string GetMa(int count)
        {
            if (count< LisData.Count)
            {
                if (string.IsNullOrEmpty(LisData[count].BanMa) == false)
                {
                    return $"{LisData[count].TDName}:{LisData[count].BanMa}";
                }
                else
                {
                    return $"{LisData[count].TDName}";
                }
            }
            return $"";
        }

        public List<string> GetShuJu(int count)
        {
          
            if (count < LisData.Count)
            {
                List<string> shuju = new List<string>();
                List<YeWuDataModel> lis = LisData[count].LisData;
                if (lis.Count > 0)
                {
                    for (int i = 0; i < lis.Count; i++)
                    {
                        if (lis[i].IsXianShi)
                        {
                            shuju.Add($"{lis[i].ItemName}:{lis[i].Value.JiCunValue}");
                        }
                    }
                }
                return shuju;
            }
            return new List<string>();
        }

        public string GetWeiBu(int count,out float bilikuang,out bool isshibai)
        {
            bilikuang = 0;
            isshibai = false;
            if (count < LisData.Count)
            {
                if (LisData[count].IsShuJuHeGe == 0)
                {
                    bilikuang = 0;
                }
                else
                {
                    if (LisData[count].IsShuJuHeGe == 3)
                    {
                        isshibai = LisData[count].IsShangChuanHeGe?false:true;
                    }
                    else
                    {
                        isshibai = LisData[count].IsShuJuHeGe == 4;
                    }
                    float zongtine = (float)SheBeiZhan.SetTestTime;
                    float xianzaitiem = (float)SheBeiZhan.TestTime;
                    if (zongtine == 0)
                    {
                        zongtine = 1;
                    }
                    bilikuang = (float)(xianzaitiem / zongtine);
                }
                return LisData[count].DiBuMiaoSu;
            }
            return "";
        }


        public void ShuaXin()
        {
            this.Invalidate();
        }
       
    }
}
