using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BoTaiKeTXLei.Modle;
using CommLei.JiChuLei;
using SSheBei.ABSSheBei;

namespace BoTaiKeTXLei.Frm
{
    public partial class CanShuKJ : UserControl, KJPeiZhiJK
    {
        private CunType CunType = CunType.XieBoTaiKeFanHui8;
        private List<SendModel> SendModels = new List<SendModel>();
        private int XuanZe = 0;
        public CanShuKJ()
        {
            InitializeComponent();
        }
        public void SetData(CunType model, List<string> ips,List<SendModel> lis)
        {
            CunType=model;
            XuanZe = 3;
            if (model == CunType.XieGetZiShuJu)
            {
                this.tianCanShuKJ3.Visible = true;
                this.tianCanShuKJ1.Visible = true;
                this.tianCanShuKJ2.Visible = true;
                this.tianCanShuKJ1.BiaoTi = "参数1";
                this.tianCanShuKJ2.BiaoTi = "参数2";
                this.tianCanShuKJ3.BiaoTi = "参数3";
                this.label1.Text = "取值";
                XuanZe = 1;
                this.commBoxE2.SelectedIndexChanged += CommBoxE2_SelectedIndexChanged;
                if (this.commBoxE1.Items.Count > 0)
                {
                    this.commBoxE1.SelectedIndex = 0;
                }
                this.commBoxE2.Items.Clear();
                List<string> sso = ChangYong.MeiJuLisName(typeof(QuZhiType));
                for (int i = 0; i < sso.Count; i++)
                {
                    this.commBoxE2.Items.Add(sso[i]);
                }
                if (this.commBoxE2.Items.Count > 0)
                {
                    this.commBoxE2.SelectedIndex = 0;
                }
            }
            if (model == CunType.XieOpenBoTaiKe|| model == CunType.XieCloseBoTaiKe)
            {
                this.tianCanShuKJ3.Visible = false;
                this.tianCanShuKJ1.Visible = false;
                this.tianCanShuKJ2.Visible = false;
                this.commBoxE1.Visible = false;
                this.commBoxE2.Visible = false;
                this.label1.Text = "不需要参数";
                this.label2.Visible = false;
            }
            else
            {
                XuanZe = 0;
                SendModels = lis;
                this.tianCanShuKJ3.Visible = false;
                this.tianCanShuKJ1.Visible = true;
                this.tianCanShuKJ2.Visible = true;
                this.tianCanShuKJ1.BiaoTi = "CMD  ";
                this.tianCanShuKJ2.BiaoTi = "Param";
                this.commBoxE1.Items.Clear();
                this.commBoxE1.SelectedIndexChanged += CommBoxE1_SelectedIndexChanged;
                for (int i = 0; i < lis.Count; i++)
                {
                    this.commBoxE1.Items.Add(lis[i]);
                }
                if (this.commBoxE1.Items.Count > 0)
                {
                    this.commBoxE1.SelectedIndex = 0;
                }
              
                this.commBoxE2.Items.Clear();
                for (int i = 0; i < ips.Count; i++)
                {
                    this.commBoxE2.Items.Add(ips[i]);
                }
                if (this.commBoxE2.Items.Count > 0)
                {
                    this.commBoxE2.SelectedIndex = 0;
                }
            }

        }

        private void CommBoxE1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (XuanZe == 0)
            {
                string text = this.commBoxE1.Text;
                for (int i = 0; i < SendModels.Count; i++)
                {
                    if (SendModels[i].Name.Equals(text))
                    {
                        this.tianCanShuKJ1.SetCanShu(SendModels[i].CMD);
                        this.tianCanShuKJ2.SetCanShu(SendModels[i].Param);
                        this.commBoxE2.Text = SendModels[i].IP;
                        break;
                    }
                }
            }
        }

        private void CommBoxE2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (XuanZe == 1)
            {
                QuZhiType meiju = this.commBoxE2.GetCanShu<QuZhiType>();
                switch (meiju)
                {
                    case QuZhiType.FG:
                        {
                            this.tianCanShuKJ1.BiaoTi = "分隔符";
                            this.tianCanShuKJ2.BiaoTi = "取第几";
                            this.tianCanShuKJ3.BiaoTi = "参数3";
                        }
                        break;
                    case QuZhiType.QC:
                        {
                            this.tianCanShuKJ1.BiaoTi = "开始位置";
                            this.tianCanShuKJ2.BiaoTi = "结束位置";
                            this.tianCanShuKJ3.BiaoTi = "参数3";
                        }
                        break;
                    case QuZhiType.ZYJQ:
                        {
                            this.tianCanShuKJ1.BiaoTi = "开始字符";
                            this.tianCanShuKJ2.BiaoTi = "结束字符";
                            this.tianCanShuKJ3.BiaoTi = "参数3";
                        }
                        break;
                    case QuZhiType.LCFG:
                        {
                            this.tianCanShuKJ1.BiaoTi = "1分隔符";
                            this.tianCanShuKJ2.BiaoTi = "2分隔符";
                            this.tianCanShuKJ3.BiaoTi = "取第几";
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public string GetCanShu()
        {
            if (CunType == CunType.XieGetZiShuJu)
            {
                return $"{this.commBoxE1.Text}#{this.commBoxE2.Text}#{this.tianCanShuKJ1.GetCanShu()}*{this.tianCanShuKJ2.GetCanShu()}*{this.tianCanShuKJ3.GetCanShu()}";
            }
            else
            {
                return $"{this.commBoxE1.Text}#{this.tianCanShuKJ1.GetCanShu()}#{this.tianCanShuKJ2.GetCanShu()}#{this.commBoxE2.Text}";
            }
        }

        public Control GetPeiZhiKJ(string jicunqibiaoshi)
        {
            return this;
        }

        public void SetCanShu(string canshu)
        {
            if (CunType == CunType.XieGetZiShuJu)
            {
                List<string> lis = ChangYong.JieGeStr(canshu,'#');
                if (lis.Count>=3)
                {
                    this.commBoxE1.Text= lis[0];
                    this.commBoxE2.Text= lis[1];
                    List<string> canshus = ChangYong.JieGeStr(lis[2],'*');
                    if (canshus.Count > 0)
                    {
                        this.tianCanShuKJ1.SetCanShu(canshus[0]);
                    }
                    if (canshus.Count > 1)
                    {
                        this.tianCanShuKJ1.SetCanShu(canshus[1]);
                    }
                    if (canshus.Count > 2)
                    {
                        this.tianCanShuKJ1.SetCanShu(canshus[2]);
                    }
                }

            }
            else
            {
                List<string> lis = ChangYong.JieGeStr(canshu, '#');
                if (lis.Count >= 4)
                {
                    XuanZe = 9;
                    this.commBoxE1.Text = lis[0];
                    this.commBoxE2.Text = lis[3];
                    this.tianCanShuKJ1.SetCanShu(lis[1]);
                    this.tianCanShuKJ2.SetCanShu(lis[2]);
                    XuanZe = 0;
                }

            }

        }
    }
}
