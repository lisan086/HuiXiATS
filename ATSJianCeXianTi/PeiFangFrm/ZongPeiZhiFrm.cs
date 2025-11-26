using ATSJianCeXianTi.JKKJ.PeiZhiKJ;
using ATSJianCeXianTi.Model;
using CommLei.DataChuLi;
using CommLei.JiChuLei;
using JieMianLei.FuFrom;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATSJianCeXianTi.PeiFangFrm
{
    public partial class ZongPeiZhiFrm : BaseFuFrom
    {
        public ZongPeiZhiFrm()
        {
            InitializeComponent();
            SetCanShu();
        }
        private void SetCanShu()
        {
            List<TDPeiZhiKJ> kjs = new List<TDPeiZhiKJ>();
            List<TDModel> lismodel = HCLisDataLei<TDModel>.Ceratei().LisWuLiao;
            for (int i = 0; i < lismodel.Count; i++)
            {
                TDPeiZhiKJ kj = new TDPeiZhiKJ();
                kj.SetCanShu(lismodel[i]);
                kjs.Add(kj);
            }
            this.flowLayoutPanel1.Controls.AddRange(kjs.ToArray());
        }
        private void button1_Click(object sender, EventArgs e)
        {
            TDPeiZhiKJ kj = new TDPeiZhiKJ();
            kj.SetCanShu(new TDModel());
            this.flowLayoutPanel1.Controls.Add(kj);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<TDModel> lismodel = new List<TDModel>();
            for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
            {
                if (this.flowLayoutPanel1.Controls[i] is TDPeiZhiKJ)
                {
                    TDPeiZhiKJ kj = this.flowLayoutPanel1.Controls[i] as TDPeiZhiKJ;
                    TDModel model = kj.GetCanShu();
                    if (model.TDID > 0)
                    {
                        lismodel.Add(model);
                    }
                }
               
            }
            HCLisDataLei<TDModel>.Ceratei().LisWuLiao = lismodel;
            HCLisDataLei<TDModel>.Ceratei().BaoCun();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int tdid = ChangYong.TryInt(this.textBox1.Text,-1);
            if (tdid > 0)
            {
                for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
                {
                    if (this.flowLayoutPanel1.Controls[i] is TDPeiZhiKJ)
                    {
                        TDPeiZhiKJ kj = this.flowLayoutPanel1.Controls[i] as TDPeiZhiKJ;
                        TDModel model = kj.GetCanShu();
                        if (model.TDID == tdid)
                        {
                            TDModel xinmodel = ChangYong.FuZhiShiTi(model);
                            xinmodel.TDID = -1;
                            TDPeiZhiKJ kjs = new TDPeiZhiKJ();
                            kjs.SetCanShu(xinmodel);
                            this.flowLayoutPanel1.Controls.Add(kjs);
                            break;
                        }
                    }

                }
            }
            else
            {
                this.QiDongTiShiKuang("未选择复制那个通道");
            }
        }
    }
}
