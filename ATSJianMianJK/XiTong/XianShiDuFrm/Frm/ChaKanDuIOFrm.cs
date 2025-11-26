using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSJianMianJK.XiTong.Model;
using ATSJianMianJK.XiTong.XianShiDuFrm.KJ;
using CommLei.JiChuLei;
using JieMianLei.FuFrom;

namespace ATSJianMianJK.XiTong.XianShiDuFrm.Frm
{
    public partial class ChaKanDuIOFrm : BaseFuFrom
    {
        private List<DuIOCanShuModel> iOCanShuModels=new List<DuIOCanShuModel>();
        public ChaKanDuIOFrm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }
        public  void IniData(List<DuIOCanShuModel> kamodel)
        {
            iOCanShuModels = kamodel;
            List<int> tdids = new List<int>();
            for (int i = 0; i < kamodel.Count; i++)
            {
                if (tdids.IndexOf(kamodel[i].TDID)<0)
                {
                    tdids.Add(kamodel[i].TDID);
                }
            }
            this.commBoxE1.Items.Clear();
            for (int i = 0; i < tdids.Count; i++)
            {
                this.commBoxE1.Items.Add(tdids[i]);
            }
            if (this.commBoxE1.Items.Count>0)
            {
                this.commBoxE1.SelectedIndex = 0;
            }
            
           
        }
        protected void ShuXinShuJu()
        {
            for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
            {
                if (this.flowLayoutPanel1.Controls[i] is DuKuaiIOKJ)
                {
                    DuKuaiIOKJ kj = this.flowLayoutPanel1.Controls[i] as DuKuaiIOKJ;
                    for (int c = 0; c < iOCanShuModels.Count; c++)
                    {
                        kj.ShuXinData(iOCanShuModels[c]);
                    }
                  
                }
            }
        }
        protected override void GuanBi()
        {
            this.timer1.Enabled = false;
            base.GuanBi();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ShuXinShuJu();
        }

      
        private void commBoxE1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
          
            Thread.Sleep(150);
            this.flowLayoutPanel1.Controls.Clear();
            int id = ChangYong.TryInt(this.commBoxE1.Text,-1);
            List<DuKuaiIOKJ> kjDu = new List<DuKuaiIOKJ>();
          
            for (int i = 0; i < iOCanShuModels.Count; i++)
            {
                if (id== iOCanShuModels[i].TDID)
                {
                    DuKuaiIOKJ kj = new DuKuaiIOKJ();
                    kj.SetCanShu(iOCanShuModels[i]);
                    kjDu.Add(kj);
                }
            }

            this.flowLayoutPanel1.Controls.AddRange(kjDu.ToArray());
            this.timer1.Enabled = true;
        }
    }
}
