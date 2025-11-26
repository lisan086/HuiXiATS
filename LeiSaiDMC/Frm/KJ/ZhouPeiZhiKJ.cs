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
    public partial class ZhouPeiZhiKJ : UserControl
    {
        private KaCanShuModel KaCanShuModel;

        private ZhouPeiZhiModel ZhouPeiZhiModel;
        public ZhouPeiZhiKJ()
        {
            InitializeComponent();
        }
        public void SetCanShu(KaCanShuModel kamodel, ZhouPeiZhiModel zhoumodel)
        {
            KaCanShuModel = ChangYong.FuZhiShiTi(kamodel);
            ZhouPeiZhiModel = ChangYong.FuZhiShiTi(zhoumodel);
            this.txbMoShi.Text = kamodel.KaName;
            this.checkBox1.Checked = kamodel.IsZhou;
            this.textBox1.Text = kamodel.BitNoOrZhouHao.ToString();
            this.checkBox2.Checked = kamodel.IsXieIO;
            this.textBox2.Text = kamodel.ZhouPeiZhiID.ToString();
        }

        public KaCanShuModel GetKaModel()
        {
            KaCanShuModel kamodel = ChangYong.FuZhiShiTi(KaCanShuModel);
            kamodel.KaName = this.txbMoShi.Text;
            kamodel.IsZhou = this.checkBox1.Checked;
            kamodel.BitNoOrZhouHao = (ushort)ChangYong.TryUShort(this.textBox1.Text,0);
            kamodel.IsXieIO = this.checkBox2.Checked;
            kamodel.ZhouPeiZhiID = ChangYong.TryInt(this.textBox2.Text, 0);
            return kamodel;

        }
        public ZhouPeiZhiModel GetZhouPeiZhiModel()
        {
            ZhouPeiZhiModel kamodel = ChangYong.FuZhiShiTi(ZhouPeiZhiModel);
          
            return kamodel;

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                this.button2.Visible = true;
            }
            else
            {
                this.button2.Visible = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ZhouPeiZhiModel model = ZhouPeiZhiModel;
            ZhouXianZhiFrm frm = new ZhouXianZhiFrm();
            if (model != null)
            {
                frm.SetCanShu(model, false);
            }
            else
            {
                frm.SetCanShu(new ZhouPeiZhiModel(), false);
            }
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                ZhouPeiZhiModel = ChangYong.FuZhiShiTi(frm.GetModel());
              

            }
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
