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
using JieMianLei.FuFrom;
using LeiSaiDMC.Frm.KJ;
using LeiSaiDMC.Model;

namespace LeiSaiDMC.Frm
{
    public partial class ZhouOrIOFrm : BaseFuFrom
    {
        private int Type = 1;
        public ZhouOrIOFrm()
        {
            InitializeComponent();
        }

        public void SetCanShu(List<ZhouModel> kamodes)
        {
            Type = 1;
            List<ZhouPeiZhiKJ> kjs = new List<ZhouPeiZhiKJ>();
            for (int i = 0; i < kamodes.Count; i++)
            {
                ZhouPeiZhiKJ kj = new ZhouPeiZhiKJ();               
                kj.SetCanShu(kamodes[i]);
                kjs.Add(kj);
            }
            this.flowLayoutPanel1.Controls.AddRange(kjs.ToArray());
        }
        public void SetCanShu(List<IOModel> kamodes)
        {
            Type = 2;
            List<IOKJ> kjs = new List<IOKJ>();
            for (int i = 0; i < kamodes.Count; i++)
            {
                IOKJ kj = new IOKJ();
                kj.SetCanShu(kamodes[i]);
                kjs.Add(kj);
            }
            this.flowLayoutPanel1.Controls.AddRange(kjs.ToArray());
        }

        public List<ZhouModel> GetZhouModel()
        {
            List<ZhouModel> kas = new List<ZhouModel>();
           
            for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
            {
                if (this.flowLayoutPanel1.Controls[i] is ZhouPeiZhiKJ)
                {
                    ZhouPeiZhiKJ kj = this.flowLayoutPanel1.Controls[i] as ZhouPeiZhiKJ;
                    ZhouModel ka = kj.GetKaModel();
                    
                    kas.Add(ka);
                }
            }
            return kas;
        }
        public List<IOModel> GetIOModel()
        {
            List<IOModel> kas = new List<IOModel>();

            for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
            {
                if (this.flowLayoutPanel1.Controls[i] is IOKJ)
                {
                    IOKJ kj = this.flowLayoutPanel1.Controls[i] as IOKJ;
                    IOModel ka = kj.GetKaModel();

                    kas.Add(ka);
                }
            }
            return kas;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Type == 1)
            {
                ZhouPeiZhiKJ kj = new ZhouPeiZhiKJ();
                kj.SetCanShu(new ZhouModel());
                this.flowLayoutPanel1.Controls.Add(kj);
            }
            else if (Type == 2)
            {
                IOKJ kj = new IOKJ();
                kj.SetCanShu(new IOModel());
                this.flowLayoutPanel1.Controls.Add(kj);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
