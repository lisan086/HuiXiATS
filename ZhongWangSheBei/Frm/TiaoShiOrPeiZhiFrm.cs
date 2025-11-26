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
using ZhongWangSheBei.Model;


namespace ZhongWangSheBei.Frm
{
    public partial class TiaoShiOrPeiZhiFrm : BaseFuFrom
    {
        private List<IOKJ> liskj = new List<IOKJ>();
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
                    PeiKJ kj = new PeiKJ();
                    kj.SetCanShu(lis[i]);
                    this.flowLayoutPanel1.Controls.Add(kj);
                }
                this.tabPage2.Parent = null;
                this.tabPage3.Parent = null;
            }
            else 
            {
                this.tabPage1.Parent = null;
                {
                    List<ZSModel> lis = PeiZhiLei.DataMoXing.LisSheBei;
                    for (int i = 0; i < lis.Count; i++)
                    {
                        for (int c = 0; c < lis[i].LisJiLu.Count; c++)
                        {
                            IOKJ kjs = new IOKJ(PeiZhiLei);
                            kjs.SetCanShu(lis[i].LisJiLu[c], lis[i].SheBeiID);
                            liskj.Add(kjs);
                        }

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
                {
                    List<JiCunQiModel> jicun = PeiZhiLei.DataMoXing.LisXie;
                    foreach (var item in jicun)
                    {
                        int index = this.dataGridView1.Rows.Add();
                        this.dataGridView1.Rows[index].Cells[0].Value = item.WeiYiBiaoShi;
                        this.dataGridView1.Rows[index].Cells[1].Value = "";
                        this.dataGridView1.Rows[index].Cells[2].Value = 0;
                        this.dataGridView1.Rows[index].Cells[3].Value = "执行";
                        this.dataGridView1.Rows[index].Tag = item;
                        this.dataGridView1.Rows[index].Height = 32;
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
            PeiKJ kj = new PeiKJ();
            kj.SetCanShu(new ZSModel());
            this.flowLayoutPanel1.Controls.Add(kj);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<ZSModel> lis = new List<ZSModel>();
            for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
            {
                if (this.flowLayoutPanel1.Controls[i] is PeiKJ)
                {
                    PeiKJ kj = this.flowLayoutPanel1.Controls[i] as PeiKJ;
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex>=0)
            {
                if (e.ColumnIndex==3)
                {
                    if (this.dataGridView1.Rows[e.RowIndex].Tag is JiCunQiModel)
                    {
                        JiCunQiModel model = this.dataGridView1.Rows[e.RowIndex].Tag as JiCunQiModel;
                        JiCunQiModel xinmodel = model.FuZhi();
                        xinmodel.SheBeiID = model.SheBeiID;
                        xinmodel.Value = this.dataGridView1.Rows[e.RowIndex].Cells[1].Value;
                        this.dataGridView1.Rows[e.RowIndex].Cells[2].Value = 0;
                        double haomiao = 0;
                        bool ischaoshi = false;
                        this.Waiting(() => {
                            DateTime timeshijina = DateTime.Now;
                            int chaoshitime = 1500;
                            PeiZhiLei.XieJiDianQi(xinmodel);
                            for (; ; )
                            {                              
                                int modess = PeiZhiLei.DataMoXing.IsChengGong(xinmodel);
                                if (modess==1)
                                {
                                    haomiao = (DateTime.Now - timeshijina).TotalMilliseconds;
                                    break;
                                }
                                if ((DateTime.Now- timeshijina).TotalMilliseconds>= chaoshitime)
                                {
                                    ischaoshi = true;
                                    haomiao = chaoshitime;
                                    break;
                                }
                            }
                        },"正在执行，请稍后...",this);
                        if (ischaoshi)
                        {
                            this.dataGridView1.Rows[e.RowIndex].Cells[2].Value = $"超时:{haomiao}ms";
                        }
                        else
                        {
                            this.dataGridView1.Rows[e.RowIndex].Cells[2].Value = $"{haomiao}ms";
                        }

                    }
                }
            }
        }
    }
}
