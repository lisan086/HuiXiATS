using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSJianMianJK.GongNengLei;
using ATSJianMianJK.XiTong.Model;
using ATSJianMianJK.XiTong.XianShiDuFrm.KJ;
using CommLei.JiChuLei;
using JieMianLei.FuFrom;

namespace ATSJianMianJK.XiTong.XianShiDuFrm.Frm
{
    public partial class ChaKanHuanCunFrm : BaseFuFrom
    {
        private List<Huan_> LisHuan = new List<Huan_>();
        public ChaKanHuanCunFrm()
        {
            InitializeComponent();
        }
        private  void SetCanShu(int tdid)
        {
            this.flowLayoutPanel1.Controls.Clear();
            LisHuan.Clear();
            List<HuanCunModel> lis = HuanCunLei.Cerate().GetHunCunTD(tdid);
            for (int i = 0; i < lis.Count; i++)
            {
                Huan_ huan_ = new Huan_();
                huan_.SetCanShu(lis[i]);
                LisHuan.Add(huan_);
                this.flowLayoutPanel1.Controls.Add(huan_);
            }
        }
        private void commBoxE1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetCanShu(ChangYong.TryInt(this.commBoxE1.Text,-1));
        }

        private void ChaKanHuanCunFrm_Load(object sender, EventArgs e)
        {
            this.commBoxE1.Items.Clear();
            List<int> tds = HuanCunLei.Cerate().GetTDs();
            for (int i = 0; i < tds.Count; i++)
            {
                this.commBoxE1.Items.Add(tds[i]);
            }
            this.commBoxE1.SelectedIndex = 0;
            
        }
        protected override void GuanBi()
        {
            this.timer1.Enabled = false;
            base.GuanBi();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < LisHuan.Count; i++)
            {
                LisHuan[i].ShuaXin();
            }
        }
    }
}
