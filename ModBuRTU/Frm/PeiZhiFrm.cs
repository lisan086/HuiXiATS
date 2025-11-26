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
using SundyChengZhong.Frm.KJ;
using SundyChengZhong.Model;

namespace SundyChengZhong.Frm
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
                    CZKJ kj = new CZKJ();
                    kj.SetCanShu(lis[i]);
                    this.flowLayoutPanel1.Controls.Add(kj);
                }
                this.tabPage3.Parent = null;
            }
            else
            {
                this.tabPage1.Parent = null;
                {
                    List<SheBeiModel> lis = PeiZhiLei.DataMoXing.LisSheBei;
                    for (int i = 0; i < lis.Count; i++)
                    {
                        SheBeiTiaoShiKJ kjs = new SheBeiTiaoShiKJ();
                        kjs.SetCanShu(lis[i], PeiZhiLei);
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
        private void button1_Click(object sender, EventArgs e)
        {
            if (PeiZhiLei.IsPeiZhi)
            {
                List<SheBeiModel> lis = new List<SheBeiModel>();
                for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
                {
                    if (this.flowLayoutPanel1.Controls[i] is CZKJ)
                    {
                        CZKJ cZKJ = this.flowLayoutPanel1.Controls[i] as CZKJ;
                        lis.Add(cZKJ.GetModel());
                    }
                }
                PeiZhiLei.BaoCun(lis);
            }
            else
            {
                this.QiDongTiShiKuang("不是在配置界面上");
            }
        }

        private void PeiZhiFrm_Load(object sender, EventArgs e)
        {
            SetCanShu();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            PeiZhiLei.ChuFaShiJian(new SSheBei.Model.JiCunQiModel() {WeiYiBiaoShi= "置零",Value="1"});
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            CZKJ kj = new CZKJ();
            kj.SetCanShu(new SheBeiModel());
            this.flowLayoutPanel1.Controls.Add(kj);
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
