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
using JieMianLei.FuFrom;
using LeiSaiDMC.Frm.KJ;
using LeiSaiDMC.Model;

namespace LeiSaiDMC.Frm
{
    public partial class ZhouOrIOFrm : BaseFuFrom
    {
        public ZhouOrIOFrm()
        {
            InitializeComponent();
        }

        public void SetCanShu(List<KaCanShuModel> kamodes,List<ZhouPeiZhiModel> zhoupeizhimodels)
        {
            List<ZhouPeiZhiKJ> kjs = new List<ZhouPeiZhiKJ>();
            for (int i = 0; i < kamodes.Count; i++)
            {
                if (kamodes[i].IsZhou)
                {
                    ZhouPeiZhiKJ kj = new ZhouPeiZhiKJ();
                    ZhouPeiZhiModel peis = new ZhouPeiZhiModel();
                    for (int c = 0; c < zhoupeizhimodels.Count; c++)
                    {
                        if (zhoupeizhimodels[c].ZhouPeiZhiID== kamodes[i].ZhouPeiZhiID)
                        {
                            peis = zhoupeizhimodels[c];
                            break;
                        }
                    }
                    kj.SetCanShu(kamodes[i], peis);
                    kjs.Add(kj);
                }
                else
                {
                    ZhouPeiZhiKJ kj = new ZhouPeiZhiKJ();
                    kj.SetCanShu(kamodes[i],new ZhouPeiZhiModel());
                    kjs.Add(kj);
                }
            }
            this.flowLayoutPanel1.Controls.AddRange(kjs.ToArray());
        }

        public List<KaCanShuModel> GetModel(out List<ZhouPeiZhiModel> PeiZhou)
        {
            List<KaCanShuModel> kas = new List<KaCanShuModel>();
            PeiZhou = new List<ZhouPeiZhiModel>();
            for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
            {
                if (this.flowLayoutPanel1.Controls[i] is ZhouPeiZhiKJ)
                {
                    ZhouPeiZhiKJ kj = this.flowLayoutPanel1.Controls[i] as ZhouPeiZhiKJ;
                    KaCanShuModel ka = kj.GetKaModel();
                    if (ka.IsZhou)
                    {
                        PeiZhou.Add(ChangYong.FuZhiShiTi(kj.GetZhouPeiZhiModel()));
                    }
                    kas.Add(ka);
                }
            }
            return kas;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ZhouPeiZhiKJ kj = new ZhouPeiZhiKJ();
            kj.SetCanShu(new KaCanShuModel(), new ZhouPeiZhiModel());
            this.flowLayoutPanel1.Controls.Add(kj);
        }
    }
}
