using CommLei.DataChuLi;
using CommLei.JiChuLei;
using JieMianLei.FuFrom;
using SSheBei.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XiangTongChuanKouSheBei.Frm;
using ZhongChuanAVLei.Frm;
using ZhongWangSheBei.Model;


namespace ZhongWangSheBei.Frm
{
    public partial class TiaoShiOrPeiZhiFrm : BaseFuFrom
    {
        private List<ShuJuKJ> liskj = new List<ShuJuKJ>();
        private PeiZhiLei PeiZhiLei;
        public TiaoShiOrPeiZhiFrm(PeiZhiLei peiZhiLei)
        {
            InitializeComponent();
            PeiZhiLei = peiZhiLei;
            if (peiZhiLei.IsPeiZhi==false)
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
                List<ZSModel> lis = PeiZhiLei.GetSheBei();
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
                    List<ZSModel> lis = PeiZhiLei.DataMoXing.LisSheBei;
                    for (int i = 0; i < lis.Count; i++)
                    {
                        ShuJuKJ kjs = new ShuJuKJ();
                        List<CunModel> lismodel = PeiZhiLei.DataMoXing.GetSheBeiJiCunQi(lis[i].SheBeiID);
                        kjs.SetCanShu(lismodel, PeiZhiLei);
                        liskj.Add(kjs);

                    }

                    int count = liskj.Count;
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
                        liskj[i].Dock = DockStyle.Fill;
                        this.tableLayoutPanel1.Controls.Add(liskj[i], i, 0);
                    }
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
            kj.SetCanShu(new ZSModel());
            this.flowLayoutPanel1.Controls.Add(kj);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<ZSModel> lis = new List<ZSModel>();
            for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
            {
                if (this.flowLayoutPanel1.Controls[i] is SheBeiKJ)
                {
                    SheBeiKJ kj = this.flowLayoutPanel1.Controls[i] as SheBeiKJ;
                    lis.Add(kj.GetCanShu());
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

        private void TiaoShiOrPeiZhiFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.timer1.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int shebeid = ChangYong.TryInt(this.textBox2.Text,-1);
            ZSModel model = null;
            for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
            {
                if (this.flowLayoutPanel1.Controls[i] is SheBeiKJ)
                {
                    SheBeiKJ kj = this.flowLayoutPanel1.Controls[i] as SheBeiKJ;
                    ZSModel zj = kj.GetCanShu();
                    if (zj.SheBeiID== shebeid)
                    {
                        model = ChangYong.FuZhiShiTi(zj);
                        break;
                    }
                  
                }
            }
            if (model!=null)
            {
                SheBeiKJ kj = new SheBeiKJ();
                kj.SetCanShu(model);
                this.flowLayoutPanel1.Controls.Add(kj);
            }
        }
    }
}
