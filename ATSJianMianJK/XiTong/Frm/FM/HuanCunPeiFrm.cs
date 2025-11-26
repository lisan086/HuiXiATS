using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSJianMianJK.GongNengLei;
using ATSJianMianJK.XiTong.Frm.KJ;
using ATSJianMianJK.XiTong.Model;
using CommLei.DataChuLi;
using JieMianLei.FuFrom;

namespace ATSJianMianJK.XiTong.Frm.FM
{
    public partial class HuanCunPeiFrm : BaseFuFrom
    {
        public HuanCunPeiFrm()
        {
            InitializeComponent();
            SetCanShu();
        }
        private void SetCanShu()
        {
            List<HuanCunKJ> kjs = new List<HuanCunKJ>();
            List<HuanCunModel> lismodel = HCLisDataLei<HuanCunModel>.Ceratei().LisWuLiao;
            for (int i = 0; i < lismodel.Count; i++)
            {
                HuanCunKJ kj = new HuanCunKJ();
                kj.SetCanShu(lismodel[i]);
                kjs.Add(kj);
            }
            this.flowLayoutPanel1.Controls.AddRange(kjs.ToArray());
        }
        private void button1_Click(object sender, EventArgs e)
        {
            HuanCunKJ kj = new HuanCunKJ();
            kj.SetCanShu(new HuanCunModel());
            this.flowLayoutPanel1.Controls.Add(kj);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<HuanCunModel> lismodel = new List<HuanCunModel>();
            for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
            {
                if (this.flowLayoutPanel1.Controls[i] is HuanCunKJ)
                {
                    HuanCunKJ kj = this.flowLayoutPanel1.Controls[i] as HuanCunKJ;
                    HuanCunModel model = kj.GetModel();
                    if (string.IsNullOrEmpty(model.HuanCunName) == false)
                    {
                        lismodel.Add(model);
                    }
                }

            }
            HCLisDataLei<HuanCunModel>.Ceratei().LisWuLiao = lismodel;
            HCLisDataLei<HuanCunModel>.Ceratei().BaoCun();
            HuanCunLei.Cerate().ShuaXinHuanCun();
        }
    }
}
