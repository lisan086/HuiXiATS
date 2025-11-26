using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BaseUI.UC;
using JieMianLei.FuFrom;
using LeiSaiDMC.Frm.KJ;
using LeiSaiDMC.Model;
using SSheBei.Model;

namespace LeiSaiDMC.Frm
{
    public partial class PeiZhiJieMian : BaseFuFrom
    {
        private List<ZhouKJ> KJZhou = new List<ZhouKJ>();
        private  PeiZhiLei PeiZhiLei;
        public PeiZhiJieMian(PeiZhiLei peiZhiLei)
        {
            InitializeComponent();
            PeiZhiLei = peiZhiLei;
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
                this.tabPage2.Parent = null;
                this.tabPage3.Parent = null;
                List<LSModel> lis = PeiZhiLei.GetSheBei();
                for (int i = 0; i < lis.Count; i++)
                {
                    LSKJ kj = new LSKJ();
                    kj.SetCanShu(lis[i]);
                    this.flowLayoutPanel1.Controls.Add(kj);
                }
            }
            else
            {
                this.QuXiaoBiaoTi();
                this.tabPage1.Parent = null;
                {
                    KJZhou.Clear();
                    List<LSModel> cashu = PeiZhiLei.DataMoXing.LisSheBei;
                    for (int i = 0; i < cashu.Count; i++)
                    {
                        LSModel shebei = cashu[i];
                        foreach (var item in shebei.LisZhouModel)
                        {
                            ZhouKJ kjsd = new ZhouKJ();
                            kjsd.SetCanShu(shebei, item,PeiZhiLei.DataMoXing.GetSheBeiJiCunQi(shebei.SheBeiID, item.ZhouNO) ,PeiZhiLei);
                            this.flowLayoutPanel2.Controls.Add(kjsd);
                            KJZhou.Add(kjsd);
                        }
                    }
                    this.flowLayoutPanel2.Controls.AddRange(KJZhou.ToArray());
                }
              

                this.timer1.Enabled = true;
            }
        }

     
        private void PeiZhiJieMian_Load(object sender, EventArgs e)
        {
            SetCanShu();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LSKJ kj = new LSKJ();
            kj.SetCanShu(new LSModel());
            this.flowLayoutPanel1.Controls.Add(kj);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<LSModel> lis = new List<LSModel>();
            for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
            {
                if (this.flowLayoutPanel1.Controls[i] is LSKJ)
                {
                    LSKJ kj = this.flowLayoutPanel1.Controls[i] as LSKJ;
                    lis.Add(kj.GetModel());
                }
            }
            PeiZhiLei.BaoCun(lis);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
         
            for (int i = 0; i < KJZhou.Count; i++)
            {
                KJZhou[i].ShuaXinData();
            }
        }
    }
}
