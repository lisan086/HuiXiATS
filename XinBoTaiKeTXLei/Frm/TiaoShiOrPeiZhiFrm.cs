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
using JieMianLei.FuFrom;

namespace BoTaiKeTXLei.Frm
{
    public partial class TiaoShiOrPeiZhiFrm : BaseFuFrom
    {
        private PeiZhiLei PeiZhiLei;
        public TiaoShiOrPeiZhiFrm(PeiZhiLei peiZhiLei)
        {
            InitializeComponent();
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
                List<YiQiModel> lis = PeiZhiLei.GetSheBei();
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
                    TiaoShiKJ kj = new TiaoShiKJ();
                    kj.SetCanShu(PeiZhiLei);
                    kj.Dock = DockStyle.Fill;
                    this.panel2.Controls.Add(kj);
                }

                this.timer1.Enabled = true;

            }
        }

        private void TiaoShiOrPeiZhiFrm_Load(object sender, EventArgs e)
        {
            SetCanShu();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.panel2.Controls.Count>0)
            {
                if (this.panel2.Controls[0] is TiaoShiKJ)
                {
                    (this.panel2.Controls[0] as TiaoShiKJ).ShuaXin();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SheBeiKJ kj = new SheBeiKJ();
            kj.SetCanShu(new YiQiModel());
            this.flowLayoutPanel1.Controls.Add(kj);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<YiQiModel> lis = new List<YiQiModel>();
            for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
            {
                if (this.flowLayoutPanel1.Controls[i] is SheBeiKJ)
                {
                    SheBeiKJ kj = this.flowLayoutPanel1.Controls[i] as SheBeiKJ;
                    lis.Add(kj.GetSaoMaModel());
                }
            }
            PeiZhiLei.BaoCun(lis);

            this.QiDongTiShiKuang("保存成功");
        }
    }
}
