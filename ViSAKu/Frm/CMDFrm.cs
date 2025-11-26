using CommLei.JiChuLei;
using JieMianLei.FuFrom;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ViSAKu.Model;

namespace ViSAKu.Frm
{
    public partial class CMDFrm : BaseFuFrom
    {
        public CMDFrm()
        {
            InitializeComponent();
        }
        public void SetCanShu(List<DataLieModel> lis)
        {
            List<Control> kjs = new List<Control>();
            for (int i = 0; i < lis.Count; i++)
            {
                CMDKJ kj = new CMDKJ();
                kj.SetCanShu(lis[i]);
                kjs.Add(kj);
               
            }
            this.flowLayoutPanel1.Controls.AddRange(kjs.ToArray());
        }

        public List<DataLieModel> GetModel()
        {
            List<DataLieModel> lis = new List<DataLieModel>();
            for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
            {
                if (this.flowLayoutPanel1.Controls[i] is CMDKJ)
                {
                    CMDKJ kj = this.flowLayoutPanel1.Controls[i] as CMDKJ;
                    lis.Add(ChangYong.FuZhiShiTi(kj.GetModel()));
                }
            }
            return lis;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CMDKJ kj = new CMDKJ();
            kj.SetCanShu(new DataLieModel());
            this.flowLayoutPanel1.Controls.Add(kj);
        }
    }
}
