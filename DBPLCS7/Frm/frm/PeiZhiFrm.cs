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
using DBPLCS7.Frm.KJ;
using DBPLCS7.Frm.Lei;
using DBPLCS7.Model;
using JieMianLei.FuFrom;

namespace DBPLCS7.Frm.frm
{
    public partial class PeiZhiFrm : BaseFuFrom
    {
        List<XieKJ> LisXie = new List<XieKJ>();
        List<DuKJ> LisDu = new List<DuKJ>();
        List<KongZiIOKJ> KJIOS = new List<KongZiIOKJ>();
        PeiZhiLei PeiZhiLei;
        public PeiZhiFrm(PeiZhiLei peiZhiLei)
        {
            InitializeComponent();
            PeiZhiLei=peiZhiLei;
        }
        protected override void GuanBi()
        {
            this.timer1.Enabled = false;
            base.GuanBi();
        }
        private void SetCanShu()
        {
            LisXie.Clear();
            LisDu.Clear();
            if (PeiZhiLei.IsPeiZhi)
            {
                this.tabPage2.Parent = null;
                this.tabPage3.Parent = null;
                this.tabPage4.Parent = null;
                List<PLCShBeiModel> plcshebei = PeiZhiLei.GetSheBei();
                List<SheBeiKJ> lis = new List<SheBeiKJ>();
                for (int i = 0; i < plcshebei.Count; i++)
                {
                    SheBeiKJ kj = new SheBeiKJ();
                    kj.SetCanShu(plcshebei[i]);
                    lis.Add(kj);
                }
                this.flowLayoutPanel1.Controls.AddRange(lis.ToArray());
            }
            else
            {
                this.tabPage1.Parent = null;
                this.QuXiaoBiaoTi();
                this.tableLayoutPanel1.Controls.Clear();
                this.tableLayoutPanel2.Controls.Clear();
                this.tableLayoutPanel3.Controls.Clear();
                int count = PeiZhiLei.MoXing.LisSheBei.Count;
                this.tableLayoutPanel1.ColumnStyles.Clear();
                this.tableLayoutPanel1.RowStyles.Clear();
                this.tableLayoutPanel2.ColumnStyles.Clear();
                this.tableLayoutPanel2.RowStyles.Clear();
                this.tableLayoutPanel3.ColumnStyles.Clear();
                this.tableLayoutPanel3.RowStyles.Clear();
                this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                this.tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                float ssd = count;
                if (ssd <= 0)
                {
                    ssd = 1;
                }
                float baifenbi = (1f / ssd) * 100f;
                for (int i = 0; i < count; i++)
                {
                    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, baifenbi));
                    this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, baifenbi));
                    this.tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, baifenbi));
                }
                for (int i = 0; i < PeiZhiLei.MoXing.LisSheBei.Count; i++)
                {
                    {
                        XieKJ xieKJ = new XieKJ();
                        xieKJ.Dock = DockStyle.Fill;
                        xieKJ.ZhiXingEvent += XieKJ_ZhiXingEvent;
                        xieKJ.SetCanShu(PeiZhiLei.MoXing.LisSheBei[i]);
                        this.tableLayoutPanel1.Controls.Add(xieKJ);
                        LisXie.Add(xieKJ);
                    }
                    { 
                        DuKJ kj= new DuKJ();
                        kj.Dock = DockStyle.Fill;
                        kj.SetCanShu(PeiZhiLei.MoXing.LisSheBei[i]);
                        this.tableLayoutPanel2.Controls.Add(kj);
                        LisDu.Add(kj);
                    }
                    {
                        KongZiIOKJ kj = new KongZiIOKJ();
                        kj.Dock = DockStyle.Fill;
                        kj.ZhiXingEvent += XieKJ_ZhiXingEvent1;
                        kj.SetCanShu(PeiZhiLei.MoXing.LisSheBei[i]);
                        this.tableLayoutPanel3.Controls.Add(kj);
                        KJIOS.Add(kj);
                    }
                }
                this.timer1.Enabled = true;
            }
        }

        private string XieKJ_ZhiXingEvent(PLCJiCunQiModel obj)
        {
            string chaoshi = "";
            this.Waiting(() => {
                DateTime timeshijina = DateTime.Now;
                int chaoshitime = 600;
                obj.JiCunQiModel.Value = obj.Value;
                PeiZhiLei.XieJiDianQi(obj.JiCunQiModel);
                for (; ; )
                {
                    PLCJiCunQiModel modess = PeiZhiLei.MoXing.GetPLCDian(obj.JiCunQiModel);
                    if (modess !=null)
                    {
                        if (modess.Value.ToString().Equals(obj.JiCunQiModel.Value.ToString()))
                        {
                            chaoshi = $"{modess.Value},{(DateTime.Now - timeshijina).TotalSeconds}";
                            break;
                        }
                    }
                    if ((DateTime.Now - timeshijina).TotalMilliseconds >= chaoshitime)
                    {
                        chaoshi = $"超时,{(DateTime.Now - timeshijina).TotalSeconds}";
                        break;
                    }
                }
            }, "正在执行，请稍后...", this);
            return chaoshi;
        }

        private string XieKJ_ZhiXingEvent1(PLCJiCunQiModel obj)
        {
            string chaoshi = "";
            PeiZhiLei.XieJiDianQi(obj.JiCunQiModel);
         
            return chaoshi;
        }


        private void PeiZhiFrm_Load(object sender, EventArgs e)
        {
            SetCanShu();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (PeiZhiLei.IsPeiZhi)
            {
                List<PLCShBeiModel> plcshebeis = new List<PLCShBeiModel>();
                for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
                {
                    if (this.flowLayoutPanel1.Controls[i] is SheBeiKJ)
                    {
                        SheBeiKJ sheBeiKJ = this.flowLayoutPanel1.Controls[i] as SheBeiKJ;
                        plcshebeis.Add(ChangYong.FuZhiShiTi(sheBeiKJ.GetModel()));
                    }
                }
                PeiZhiLei.BaoCun(plcshebeis);
                this.QiDongTiShiKuang("保存成功");
            }
        }

        private void PeiZhiFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.timer1.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < LisXie.Count; i++)
            {
                LisXie[i].ShuXin();
            }
            for (int i = 0; i < LisDu.Count; i++)
            {
                LisDu[i].ShuXin();
            }
            for (int i = 0; i < KJIOS.Count; i++)
            {
                KJIOS[i].ShuaXin();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SheBeiKJ kj = new SheBeiKJ();
            kj.SetCanShu(new PLCShBeiModel());
         
            this.flowLayoutPanel1.Controls.Add(kj);
        }
    }
}
