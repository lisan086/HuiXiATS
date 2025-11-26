using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSJianMianJK;
using ATSJianMianJK.Log;
using ATSZuZhuangUI.Lei.GongNengLei.KongJian;
using CommLei.JiChuLei;
using ZuZhuangUI.Model;

namespace ATSZuZhuangUI.UI
{
    public partial class FoZhaoSaoMaKJ : UserControl, IFTongYongKJ
    {
        int JiShuID = 1;
        Image OK = null;
        Image NG = null;
        private int TDID = -1;
        private ZiYuanModel ZiYuan;
        private SheBeiZhanModel SheBeiZhanModel;
        public FoZhaoSaoMaKJ()
        {
            InitializeComponent();
        }

        public void SetModel(SheBeiZhanModel zhanModel)
        {
            SheBeiZhanModel = zhanModel;
            TDID = SheBeiZhanModel.GWID;
            this.label2.Visible = true;
            this.label2.Text = zhanModel.LineCode;
            try
            {
                string wenjian = Application.StartupPath + @"\TuPian\OK.jpg";
                if (File.Exists(wenjian))
                {
                    Image sok = Image.FromFile(wenjian);
                    OK = (Image)sok.Clone();
                    sok.Dispose();
                }
            }
            catch
            {


            }
            try
            {
                string wenjian = Application.StartupPath + @"\TuPian\NG.jpg";
                if (File.Exists(wenjian))
                {
                    Image sok = Image.FromFile(wenjian);
                    NG = (Image)sok.Clone();
                    sok.Dispose();
                }
            }
            catch
            {
            }
        }

        public void SetCanShu(EventType arg1, JieMianShiJianModel model)
        {
            if (TDID != model.GWID)
            {
                return;
            }
            switch (arg1)
            {
                case EventType.FoZhanSaoMaKaiShi:
                    {
                        this.pictureBox1.Visible = false;
                        this.label1.Text = model.MiaoSu;                   
                    }
                    break;
                case EventType.FoZhanDuQuMa:
                    {
                        this.label1.Text = model.MiaoSu;                      
                    }
                    break;
                case EventType.FoZhanJieSu:
                    {
                        this.label1.Text = model.MiaoSu;
                        if (model.JinChuZhanJieGuo)
                        {
                            this.ucJiLvContor2.LogAppend(Color.Green, $"{model.ErWeiMa} {JiShuID}");
                        }
                        else
                        {
                            this.ucJiLvContor2.LogAppend(Color.Red, $"{model.ErWeiMa} {JiShuID}");
                        }
                        JiShuID++;
                        FuZhiTuPian(model.JinChuZhanJieGuo);
                    }
                    break;
              
                default:
                    break;
            }
        }
        public void SetTanChuan(int tdid, string msg)
        {
            if (tdid == TDID)
            {
                ZiYuan.TiShiKuang(msg);
            }
        }

        private void FuZhiTuPian(bool jinzhanstate)
        {
            this.pictureBox1.BringToFront();
            if (jinzhanstate)
            {

                if (pictureBox1.Image != null)
                {
                    pictureBox1.Image.Dispose();
                }
                pictureBox1.Image = OK == null ? null : (Image)OK.Clone();
            }
            else
            {
                if (pictureBox1.Image != null)
                {
                    pictureBox1.Image.Dispose();
                }
                pictureBox1.Image = NG == null ? null : (Image)NG.Clone();
            }

            this.pictureBox1.Visible = true;
        }

        public void SetLog(int tdtd, RiJiModel riji)
        {
            if (TDID == tdtd)
            {
                this.ucJiLvContor1.LogAppend(riji.IsRed ? Color.Red : Color.Black, riji.Msg);
            }
        }

        public void ShuXin()
        {

        }

        public void SetCanShu(ZiYuanModel ziYuanModel)
        {
            ZiYuan = ziYuanModel;
        }

        public void Close()
        {

        }
    }
}
