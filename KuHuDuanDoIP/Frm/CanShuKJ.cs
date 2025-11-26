using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommLei.JiChuLei;
using KuHuDuanDoIP.Model;
using SSheBei.ABSSheBei;

namespace KuHuDuanDoIP.Frm
{
    public partial class CanShuKJ : UserControl, KJPeiZhiJK
    {
     
        private List<JiLuZhiLingModel> LisZhiLing = new List<JiLuZhiLingModel>();
        private int XuanZe = 0;
        public CanShuKJ()
        {
            InitializeComponent();
        }
        public void SetData(List<string> ips, List<JiLuZhiLingModel> lis)
        {
         
            XuanZe = 2;
            LisZhiLing = lis;
          
            this.tianCanShuKJ3.Visible = true;
            this.tianCanShuKJ1.Visible = true;
            this.tianCanShuKJ2.Visible = true;
            this.tianCanShuKJ1.BiaoTi = "Ta地址";
            this.tianCanShuKJ2.BiaoTi = "负载类型";
            this.tianCanShuKJ3.BiaoTi = "参数";
            this.commBoxE1.Items.Clear();
            this.commBoxE2.Items.Clear();
            for (int i = 0; i < lis.Count; i++)
            {
                this.commBoxE1.Items.Add(lis[i].ZhiLingName);
            }
            if (this.commBoxE1.Items.Count > 0)
            {
                this.commBoxE1.SelectedIndex = 0;
            }
            this.commBoxE1.SelectedIndexChanged += CommBoxE1_SelectedIndexChanged;
          
            this.commBoxE2.Items.Clear();
            for (int i = 0; i < ips.Count; i++)
            {
                this.commBoxE2.Items.Add(ips[i]);
            }
            if (this.commBoxE2.Items.Count > 0)
            {
                this.commBoxE2.SelectedIndex = 0;
            }
            XuanZe = 0;
        }

        private void CommBoxE1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (XuanZe == 0)
            {
                string text = this.commBoxE1.Text;
                for (int i = 0; i < LisZhiLing.Count; i++)
                {
                    if (LisZhiLing[i].ZhiLingName.Equals(text))
                    {
                        this.tianCanShuKJ1.SetCanShu(LisZhiLing[i].TaDiZhi);
                        this.tianCanShuKJ2.SetCanShu(LisZhiLing[i].FuZaiLeiXing);
                        this.tianCanShuKJ3.SetCanShu(LisZhiLing[i].ZhiLingShuJu);
                        break;
                    }
                }
            }
        }

     

        public string GetCanShu()
        {
            return $"{this.commBoxE1.Text}#{this.tianCanShuKJ2.GetCanShu()}#{this.tianCanShuKJ1.GetCanShu()}#{this.tianCanShuKJ3.GetCanShu()}#{this.commBoxE2.Text}";
        }

        public Control GetPeiZhiKJ(string jicunqibiaoshi)
        {
            return this;
        }

        public void SetCanShu(string canshu)
        {
            List<string> lis = ChangYong.JieGeStr(canshu, '#');
            if (lis.Count >= 5)
            {
                XuanZe = 9;
                this.commBoxE1.Text = lis[0];
                this.commBoxE2.Text = lis[4];
                this.tianCanShuKJ1.SetCanShu(lis[2]);
                this.tianCanShuKJ2.SetCanShu(lis[1]);
                this.tianCanShuKJ3.SetCanShu(lis[3]);
                XuanZe = 0;
            }
        }
    }
}
