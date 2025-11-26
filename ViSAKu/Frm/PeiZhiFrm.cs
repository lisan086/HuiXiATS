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
using CommLei.JiChuLei;
using JieMianLei.FuFrom;
using Microsoft.Win32;
using SSheBei.Model;
using ViSAKu.Model;

namespace ViSAKu.Frm
{
    public partial class PeiZhiFrm : BaseFuFrom
    {

        private Dictionary<string, VisAKJ> kjs = new Dictionary<string, VisAKJ>();

        private List<TiaoShiKJ> Liskjs = new List<TiaoShiKJ>();
        private PeiZhiLei PeiZhiLei;
        public PeiZhiFrm(PeiZhiLei peiZhiLei)
        {
            InitializeComponent();
            PeiZhiLei = peiZhiLei;
            if (peiZhiLei.IsPeiZhi==false)
            {
                this.QuXiaoBiaoTi();
            }
           
            SetCanShu();
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
                List<Control> KJs = new List<Control>();
                List<SheBeiVisaModel> models = PeiZhiLei.GetSheBei();
                for (int i = 0; i < models.Count; i++)
                {
                    VisAKJ vis = new VisAKJ();
                    vis.SetCanShu(models[i], PeiZhiLei.LianJieNames);
                    KJs.Add(vis);
                    if (PeiZhiLei.IsPeiZhi == false)
                    {
                        if (kjs.ContainsKey(models[i].LianJieName) == false)
                        {
                            kjs.Add(models[i].LianJieName, vis);
                        }
                    }
                }
                this.flowLayoutPanel1.Controls.AddRange(KJs.ToArray());
                this.tabPage2.Parent = null;
            }
            else
            {
                this.tabPage1.Parent = null;
                this.button2.Enabled = false;

                this.tabPage1.Parent = null;
                {
                    List<SheBeiVisaModel> models = PeiZhiLei.DataMoXing.LisSheBei;
                    for (int i = 0; i < models.Count; i++)
                    {
                        TiaoShiKJ kjs = new TiaoShiKJ();
                        kjs.SetCanShu(models[i], PeiZhiLei);
                        Liskjs.Add(kjs);

                    }

                    int count = Liskjs.Count;
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
                        Liskjs[i].Dock = DockStyle.Fill;
                        this.tableLayoutPanel1.Controls.Add(Liskjs[i], i, 0);
                    }
                }
                this.timer1.Enabled = true;




            }



        }

   

        private void button1_Click(object sender, EventArgs e)
        {
            List<SheBeiVisaModel> models = new List<SheBeiVisaModel>();
            for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
            {
                if (this.flowLayoutPanel1.Controls[i] is VisAKJ)
                {
                    VisAKJ aKJ = this.flowLayoutPanel1.Controls[i] as VisAKJ;
                    models.Add(aKJ.GetModel());
                }
            }
            PeiZhiLei.BaoCun(models);
            this.QiDongTiShiKuang("保存成功");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            VisAKJ vis = new VisAKJ();
            vis.SetCanShu(new SheBeiVisaModel(), PeiZhiLei.LianJieNames);
         
            this.flowLayoutPanel1.Controls.Add(vis);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            List<string> keys = kjs.Keys.ToList();
            for (int i = 0; i < keys.Count; i++)
            {
                try
                {
                    kjs[keys[i]].SetTX(PeiZhiLei.VisaGuanXin.IsTX(keys[i]));
                }
                catch
                {

                   
                }
                
            }
            for (int i = 0; i < Liskjs.Count; i++)
            {
                Liskjs[i].ShuaXin();
            }
        }

        private void PeiZhiFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.timer1.Enabled = false;
        }
    }
}
