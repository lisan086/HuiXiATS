using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSZuZhuangUI.PeiZhi.KJ;
using CommLei.JiChuLei;
using JieMianLei.FuFrom;
using ZuZhuangUI.Model;

namespace ATSJuanChengZuZhuangUI.PeiZhi.Frm
{
    public partial class XuanZeFrm : BaseFuFrom
    {
      
        public XuanZeFrm()
        {
            InitializeComponent();
        }

        public void SetCanShu(List<MaGuiZeModel> lis)
        {
         
            List<MaKJ> meijus =new List<MaKJ>();
            for (int i = 0; i < lis.Count; i++)
            {
                MaKJ frm = new MaKJ();
                frm.SetShuJu(lis[i]);
                meijus.Add(frm);
            }
            this.flowLayoutPanel1.Controls.AddRange(meijus.ToArray());
        }

        public List<MaGuiZeModel> GetShuJu()
        {
            List<MaGuiZeModel> shujus = new List<MaGuiZeModel>();
            for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
            {
                if (this.flowLayoutPanel1.Controls[i] is MaKJ)
                {
                    MaKJ kj = this.flowLayoutPanel1.Controls[i] as MaKJ;
                    shujus.Add(kj.GetModel());
                }
            }
            return shujus;
        }
     
        private void button5_Click_1(object sender, EventArgs e)
        {
            MaKJ frm = new MaKJ();
            frm.SetShuJu(new MaGuiZeModel());
            this.flowLayoutPanel1.Controls.Add(frm);
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
