using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSJianMianJK.XiTong.Frm.KJ;
using ATSJianMianJK.XiTong.Model;
using CommLei.JiChuLei;
using JieMianLei.FuFrom;

namespace ATSJianMianJK.XiTong.Frm.FM
{
    public partial class DuJiCunQiFrm : BaseFuFrom
    {
        private int LeiXing = 1;
        public DuJiCunQiFrm()
        {
            InitializeComponent();
        }
        protected override void GuanBi()
        {
            this.DialogResult = DialogResult.Cancel;
            base.GuanBi();
        }
        public void SetCanShu(List<DuModel> lis,int leixing)
        {
            LeiXing = leixing;
            List<string> xinlis = ChangYong.MeiJuLisName(typeof(PiPeiType));
            List<string> peizhis = new List<string>();
            for (int i = 0; i < xinlis.Count; i++)
            {
                if (leixing == 1)
                {
                    if (xinlis[i].StartsWith(PiPeiType.Zhi.ToString()))
                    {
                        peizhis.Add(xinlis[i]);
                    }
                }
                else
                {
                    if (xinlis[i].StartsWith(PiPeiType.Zhi.ToString())==false)
                    {
                        peizhis.Add(xinlis[i]);
                    }
                }
            }
           
            this.flowLayoutPanel1.Controls.Clear();
            List<DuJiCunQiKJ> kjs = new List<DuJiCunQiKJ>();
            for (int i = 0; i < lis.Count; i++)
            {
                DuJiCunQiKJ kJ = new DuJiCunQiKJ();
                kJ.SetCanShu(lis[i], peizhis);
                kjs.Add(kJ);
            }
            this.flowLayoutPanel1.Controls.AddRange(kjs.ToArray());
        }

        public List<DuModel> GetCanShu()
        {
            List<DuModel> lian = new List<DuModel>();
            for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
            {
                if (this.flowLayoutPanel1.Controls[i] is DuJiCunQiKJ)
                {
                    DuJiCunQiKJ kj = this.flowLayoutPanel1.Controls[i] as DuJiCunQiKJ;
                    lian.Add(ChangYong.FuZhiShiTi(  kj.GetModel()));
                }
            }
            return lian;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            List<string> xinlis = ChangYong.MeiJuLisName(typeof(PiPeiType));
            List<string> peizhis = new List<string>();
            for (int i = 0; i < xinlis.Count; i++)
            {
                if (LeiXing == 1)
                {
                    if (xinlis[i].StartsWith(PiPeiType.Zhi.ToString()))
                    {
                        peizhis.Add(xinlis[i]);
                    }
                }
                else
                {
                    if (xinlis[i].StartsWith(PiPeiType.Zhi.ToString()) == false)
                    {
                        peizhis.Add(xinlis[i]);
                    }
                }
            }
            DuJiCunQiKJ kJ = new DuJiCunQiKJ();
            kJ.SetCanShu(new DuModel(), peizhis);
            this.flowLayoutPanel1.Controls.Add(kJ);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
