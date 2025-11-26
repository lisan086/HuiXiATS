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
using ModBuTCP.Model;
using SSheBei.Model;

namespace ModBuTCP.Frm
{
    public partial class PeiZhiFrm : BaseFuFrom
    {
        private List<SheBeiTiaoShiKJ> shebeikj = new List<SheBeiTiaoShiKJ>();
        private PeiZhiLei PeiZhiLei;
        public PeiZhiFrm(PeiZhiLei peiZhiLei)
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
                List<SheBeiModel> lis = PeiZhiLei.GetSheBei();
                for (int i = 0; i < lis.Count; i++)
                {
                    SheBeiKJ kj = new SheBeiKJ();
                    kj.SetCanShu(lis[i]);
                    this.flowLayoutPanel1.Controls.Add(kj);
                }
                this.tabPage2.Parent = null;
             
            }
            else
            {
                this.tabPage1.Parent = null;
                {
                    List<SheBeiModel> lis = PeiZhiLei.DataMoXing.LisSheBei;
                    for (int i = 0; i < lis.Count; i++)
                    {
                        SheBeiTiaoShiKJ kjs = new SheBeiTiaoShiKJ();
                        kjs.SetCanShu(lis[i],PeiZhiLei.DataMoXing.GetCunModels(lis[i].SheBeiID), PeiZhiLei);
                        shebeikj.Add(kjs);

                    }

                    int count = shebeikj.Count;
                    this.tableLayoutPanel1.Controls.Clear();
                    this.tableLayoutPanel1.ColumnStyles.Clear();
                    this.tableLayoutPanel1.RowStyles.Clear();
                    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                    float ssd = count;
                    if (ssd <= 0)
                    {
                        ssd = 1;
                    }
                    float baifenbi = (1f / ssd) * 100f;
                    for (int i = 0; i < count; i++)
                    {
                        this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, baifenbi));
                    }
                    for (int i = 0; i < count; i++)
                    {
                        shebeikj[i].Dock = DockStyle.Fill;
                        this.tableLayoutPanel1.Controls.Add(shebeikj[i], i, 0);
                    }
                }             
                this.timer1.Enabled = true;

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SheBeiKJ kj = new SheBeiKJ();
            kj.SetCanShu(new SheBeiModel());
            this.flowLayoutPanel1.Controls.Add(kj);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<SheBeiModel> models = new List<SheBeiModel>();
            for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
            {
                if (this.flowLayoutPanel1.Controls[i] is SheBeiKJ)
                {
                    SheBeiKJ aKJ = this.flowLayoutPanel1.Controls[i] as SheBeiKJ;
                    models.Add(aKJ.GetCanShu());
                }
            }
            PeiZhiLei.BaoCun(models);
            this.QiDongTiShiKuang("保存成功");
        }

        private void PeiZhiFrm_Load(object sender, EventArgs e)
        {
            SetCanShu();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < shebeikj.Count; i++)
            {
                shebeikj[i].ShuaXin();
            }
        }
    }
}
