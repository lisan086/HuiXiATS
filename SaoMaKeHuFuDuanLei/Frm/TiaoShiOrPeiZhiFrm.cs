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
using SaoMaKeHuFuDuanLei.Frm;
using SSheBei.Model;
using YiBanSaoMaQi.Model;

namespace YiBanSaoMaQi.Frm
{
    public partial class TiaoShiOrPeiZhiFrm : BaseFuFrom
    {
        private PeiZhiLei PeiZhiLei;
        private List<TiaoShiKJ> liskj = new List<TiaoShiKJ>();
        private List<FuWuQiKJ> lisfuwukj = new List<FuWuQiKJ>();
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
                List<IPFuWuPeiModel> lis = PeiZhiLei.GetSheBei();
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
                    List<IPFuWuPeiModel> lis = PeiZhiLei.DataMoXing.LisSheBei;
                    for (int i = 0; i < lis.Count; i++)
                    {
                        if (lis[i].IsFuWuDuan == 2|| lis[i].IsFuWuDuan == 4)
                        {
                            TiaoShiKJ kjs = new TiaoShiKJ();
                            kjs.SetCanShu(lis[i], PeiZhiLei.DataMoXing.GetCunModel(lis[i].IsTongYiGe), PeiZhiLei);
                            liskj.Add(kjs);
                        }
                        else if (lis[i].IsFuWuDuan == 1)
                        {
                            FuWuQiKJ kjs = new FuWuQiKJ();
                            kjs.SetCanShu(lis[i], PeiZhiLei);
                            lisfuwukj.Add(kjs);
                        }

                    }
                    this.flowLayoutPanel2.Controls.AddRange(liskj.ToArray());
                    this.flowLayoutPanel2.Controls.AddRange(lisfuwukj.ToArray());

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
            kj.SetCanShu(new IPFuWuPeiModel());
            this.flowLayoutPanel1.Controls.Add(kj);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<IPFuWuPeiModel> lis = new List<IPFuWuPeiModel>();
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (var item in liskj)
            {
                item.ShuaXin();
            }
            foreach (var item in lisfuwukj)
            {
                item.ShuaXin();
            }
        }
    }
}
