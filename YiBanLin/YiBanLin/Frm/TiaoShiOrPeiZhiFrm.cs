using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JieMianLei.FuFrom;
using SSheBei.Model;
using YiBanLin.Model;

namespace YiBanLin.Frm
{
    public partial class TiaoShiOrPeiZhiFrm : BaseFuFrom
    {
        private PeiZhiLei PeiZhiLei;
        private List<TiaoShiKJ> liskj = new List<TiaoShiKJ>();
     
        public TiaoShiOrPeiZhiFrm(PeiZhiLei peiZhiLei)
        {
            InitializeComponent();
            this.IsJingYongZuiDa = false;
            PeiZhiLei = peiZhiLei;
            if (peiZhiLei.IsPeiZhi == false)
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
                List<LinModel> lis = PeiZhiLei.GetSheBei();
                for (int i = 0; i < lis.Count; i++)
                {
                    SheBeiKJ kj = new SheBeiKJ();
                    kj.SetCanShu(lis[i]);
                    this.flowLayoutPanel1.Controls.Add(kj);
                }
                
                this.tabPage3.Parent = null;
            }
            else
            {
                this.tabPage1.Parent = null;
                {
                    List<LinModel>  cun= PeiZhiLei.DataMoXing.LisSheBei;
                    foreach (var item in cun)
                    {
                        TiaoShiKJ kjs = new TiaoShiKJ();
                        kjs.SetCanShu(item, PeiZhiLei);
                        liskj.Add(kjs);

                    }
                    this.flowLayoutPanel2.Controls.AddRange(liskj.ToArray());
                }

                this.timer1.Enabled = true;

            }
        }

        private void TiaoShiOrPeiZhiFrm_Load(object sender, EventArgs e)
        {
            SetCanShu();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SheBeiKJ kj = new SheBeiKJ();
            kj.SetCanShu(new LinModel());
            this.flowLayoutPanel1.Controls.Add(kj);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<LinModel> lis = new List<LinModel>();
            for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
            {
                if (this.flowLayoutPanel1.Controls[i] is SheBeiKJ)
                {
                    SheBeiKJ kj = this.flowLayoutPanel1.Controls[i] as SheBeiKJ;
                    lis.Add(kj.GetLinModel());
                }
            }
            PeiZhiLei.BaoCun(lis);

            this.QiDongTiShiKuang("保存成功");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (var item in liskj)
            {
                item.ShuaXin();
            }
        }

      
    }
}
