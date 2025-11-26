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
using LeiSaiDMC.Model;

namespace LeiSaiDMC.Frm.KJ
{
    public partial class IOKJ : UserControl
    {
        private IOModel KaCanShuModel;
        public IOKJ()
        {
            InitializeComponent();
        }
        public void SetCanShu(IOModel kamodel)
        {
            KaCanShuModel = ChangYong.FuZhiShiTi(kamodel);

            this.txbMoShi.Text = kamodel.IOName;
            this.textBox1.Text = kamodel.BitNo.ToString();
            this.checkBox2.Checked = kamodel.IOLeiXing == 1;
        }

        public IOModel GetKaModel()
        {
            IOModel kamodel = ChangYong.FuZhiShiTi(KaCanShuModel);
            kamodel.IOName = this.txbMoShi.Text;
            kamodel.BitNo = (short)ChangYong.TryShort(this.textBox1.Text, 0);
            kamodel.IOLeiXing = this.checkBox2.Checked ? 1 : 2;
            return kamodel;

        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.Parent != null)
            {
                this.Parent.Controls.Remove(this);
            }
        }
    }
}
