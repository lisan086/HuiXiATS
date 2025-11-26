using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSJianCeXianTi.GongNengLei.Model;
using CommLei.JiChuLei;
using SSheBei.ABSSheBei;
using SSheBei.ZongKongZhi;

namespace ATSJianCeXianTi.GongNengLei.GNKJ
{
    public partial class DengDaiTuiChuKJ : UserControl, KJPeiZhiJK
    {
        public DengDaiTuiChuKJ()
        {
            InitializeComponent();
        }
        public void SetCanShu(int shebeiid, string weiyibiaoshi)
        {
            KJPeiZhiJK kj= ZongSheBeiKongZhi.Cerate().GetPeiZhiJieKou(shebeiid, weiyibiaoshi);
            Control peizhi = kj.GetPeiZhiKJ(weiyibiaoshi);
            peizhi.Dock = DockStyle.Fill;
            this.panel2.Controls.Add(peizhi);
        }
        public string GetCanShu()
        {
            DengDaiModel dengDaiModel = new DengDaiModel();
            if (this.panel2.Controls.Count > 0)
            {
                if (this.panel2.Controls[0] is KJPeiZhiJK)
                {
                    KJPeiZhiJK kjs = this.panel2.Controls[0] as KJPeiZhiJK;
                    dengDaiModel.CanShu = kjs.GetCanShu();
                }
            }
            dengDaiModel.GNCanShu = this.textBox1.Text;
            return ChangYong.HuoQuJsonStr(dengDaiModel);
        }

        public Control GetPeiZhiKJ(string jicunqibiaoshi)
        {
            return this;
        }

        public void SetCanShu(string canshu)
        {
            DengDaiModel dengDaiModel = ChangYong.HuoQuJsonToShiTi<DengDaiModel>(canshu);
            if (dengDaiModel != null)
            {
                if (this.panel2.Controls.Count > 0)
                {
                    if (this.panel2.Controls[0] is KJPeiZhiJK)
                    {
                        KJPeiZhiJK kjs = this.panel2.Controls[0] as KJPeiZhiJK;
                        kjs.SetCanShu(dengDaiModel.CanShu);
                    }
                }
                this.textBox1.Text = dengDaiModel.GNCanShu;
            }
           

        }
    }
}
