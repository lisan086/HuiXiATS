using CommLei.JiChuLei;
using JieMianLei.FuFrom;
using KuHuDuanDoIP.Model;
using SSheBei.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KuHuDuanDoIP.Frm
{
    public partial class PeiZhiFrm : BaseFuFrom
    {
        PeiZhiLei PeiZhiLei;
        private TiaoShiKJ TiaoShiKJ = null;
        public PeiZhiFrm(PeiZhiLei peiZhiLei)
        {
            InitializeComponent();
            PeiZhiLei = peiZhiLei;
            if (peiZhiLei.IsPeiZhi)
            {

            }
            else
            {
                this.QuXiaoBiaoTi();
             
            }
           
        }

        protected override void GuanBi()
        {
            this.timer1.Enabled = false;
            base.GuanBi();
        }

        private void SetCanShu()
        {
            if (PeiZhiLei.IsPeiZhi)
            {
                List<SheBeiModel> lis = PeiZhiLei.GetSheBei();
                for (int i = 0; i < lis.Count; i++)
                {
                    DoipKJ kj = new DoipKJ();
                    kj.SetCanShu(lis[i]);
                    this.flowLayoutPanel1.Controls.Add(kj);
                }
                this.tabPage2.Parent = null;
               
            }
            else
            {
                this.tabPage1.Parent = null;
                {
                    TiaoShiKJ = new TiaoShiKJ();
                    TiaoShiKJ.Dock = DockStyle.Fill;
                    TiaoShiKJ.SetCanShu(PeiZhiLei.MoXing.IPS,PeiZhiLei);
                    this.tabPage2.Controls.Add(TiaoShiKJ);
                }
                this.timer1.Enabled = true;

            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            List<SheBeiModel> lis = new List<SheBeiModel>();
            for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
            {
                if (this.flowLayoutPanel1.Controls[i] is DoipKJ)
                {
                    DoipKJ doipKJ = this.flowLayoutPanel1.Controls[i] as DoipKJ;
                    lis.Add(doipKJ.GetModel());
                }
            }
            PeiZhiLei.BaoCun(lis);
        }

        private void PeiZhiFrm_Load(object sender, EventArgs e)
        {
            SetCanShu();
        }

      

      

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (TiaoShiKJ!=null)
            {
                TiaoShiKJ.ShuaXin();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DoipKJ kj = new DoipKJ();
            kj.SetCanShu(new SheBeiModel());
            this.flowLayoutPanel1.Controls.Add(kj);
        }
    }
}
