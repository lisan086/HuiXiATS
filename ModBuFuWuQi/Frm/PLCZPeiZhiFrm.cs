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
using CommLei.DataChuLi;
using CommLei.JiChuLei;
using JieMianLei.FuFrom;
using ModBuTCP.Model;
using SSheBei.Model;

namespace ModBuTCP.Frm
{
    public partial class PLCZPeiZhiFrm : BaseFuFrom
    {
        public PLCZPeiZhiFrm()
        {
            InitializeComponent();
        }
        protected override void GuanBi()
        {
            this.DialogResult = DialogResult.Cancel;
            base.GuanBi();
        }
        public void SetShuJu(List<DataCunModel> JiCunQis)
        {
            List<JiCunQiKj> kjs = new List<JiCunQiKj>();
            for (int i = 0; i < JiCunQis.Count; i++)
            {
                JiCunQiKj kj = new JiCunQiKj();
                kj.SetCanShu(JiCunQis[i]);
                kjs.Add(kj);
            }
            this.flowLayoutPanel1.Controls.AddRange(kjs.ToArray());
        }

        public List<DataCunModel> GetShuJu()
        {
            List<DataCunModel> models = new List<DataCunModel>();
            for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
            {
                if (this.flowLayoutPanel1.Controls[i] is JiCunQiKj)
                {
                    JiCunQiKj ks = this.flowLayoutPanel1.Controls[i] as JiCunQiKj;
                    models.Add(ChangYong.FuZhiShiTi(ks.GetCanShu()));
                }
            }
            return models;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            JiCunQiKj kj = new JiCunQiKj();
            kj.SetCanShu(new DataCunModel());
            this.flowLayoutPanel1.Controls.Add(kj);
        }

      

     
    }
}
