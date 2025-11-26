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
using CommLei.JiChuLei;
using JieMianLei.FuFrom;
using ZuZhuangUI.Lei;
using ZuZhuangUI.Model;
using ZuZhuangUI.PeiZhi.KJ;

namespace ZuZhuangUI.PeiZhi.Frm
{
    public partial class TDShuJuFrm : BaseFuFrom
    {
        public TDShuJuFrm()
        {
            InitializeComponent();
            IniData();
        }
        private void IniData()
        {
            this.commBoxE1.Items.Clear();
            List<SheBeiZhanModel> lis = JieMianCaoZuoLei.CerateDanLi().DataJiHe.LisSheBeiBianHao;
            if (lis.Count>0)
            {
                for (int i = 0; i < lis.Count; i++)
                {
                    this.commBoxE1.Items.Add($"{lis[i].GWID}:{lis[i].LineCode}");
                }
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            this.timer1.Enabled= false;
            Thread.Sleep(200);
            bool iszhi = this.checkBox1.Checked;
            int id = ChangYong.TryInt(this.commBoxE1.Text.Split(':')[0],-1);
            this.flowLayoutPanel1.Controls.Clear();
            if (id>=0)
            {
                List<SheBeiZhanModel> lis = JieMianCaoZuoLei.CerateDanLi().DataJiHe.LisSheBeiBianHao;
                List<XianShiShuJuKJ> liskj = new List<XianShiShuJuKJ>();
                if (lis.Count > 0)
                {
                    for (int i = 0; i < lis.Count; i++)
                    {
                        if (id == lis[i].GWID)
                        {
                            if (iszhi)
                            {
                                List<YeWuDataModel> zhilis = lis[i].LisQingQiu;
                                foreach (var item in zhilis)
                                {
                                    XianShiShuJuKJ xianShiShuJuKJ = new XianShiShuJuKJ();
                                    xianShiShuJuKJ.SetCanShu(item, iszhi);
                                    liskj.Add(xianShiShuJuKJ);
                                }

                            }
                            else
                            {
                                List<YeWuDataModel> zhilis = lis[i].LisData;
                                foreach (var item in zhilis)
                                {
                                    XianShiShuJuKJ xianShiShuJuKJ = new XianShiShuJuKJ();
                                    xianShiShuJuKJ.SetCanShu(item, iszhi);
                                    liskj.Add(xianShiShuJuKJ);
                                }
                            }
                            break;
                        }
                    }
                }
                this.flowLayoutPanel1.Controls.AddRange(liskj.ToArray());
            }

            this.timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.flowLayoutPanel1.Controls.Count>0)
            {
                for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
                {
                    if (this.flowLayoutPanel1.Controls[i] is XianShiShuJuKJ)
                    {
                        (this.flowLayoutPanel1.Controls[i] as XianShiShuJuKJ).ShuaXin();
                    }
                }
            }
        }
    }
}
