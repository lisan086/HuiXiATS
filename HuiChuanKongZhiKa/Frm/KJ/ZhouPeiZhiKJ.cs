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
        private ZhouModel KaCanShuModel;

       
        public ZhouPeiZhiKJ()
        {
            InitializeComponent();
        }
        public void SetCanShu(ZhouModel kamodel)
        {
            KaCanShuModel = ChangYong.FuZhiShiTi(kamodel);         
            this.txbMoShi.Text = kamodel.ZhouName;
            this.textBox1.Text = kamodel.ZhouNO.ToString();
            this.textBox2.Text = kamodel.WuChaS.ToString();
            this.textBox3.Text = kamodel.WuChaX.ToString();
            this.textBox4.Text = kamodel.JuLiZuiDi.ToString();
            this.textBox5.Text = kamodel.JuLiZuiGao.ToString();
            this.textBox6.Text = kamodel.TongYiType.ToString();
            this.textBox7.Text = kamodel.SuDu.ToString();
            this.textBox8.Text = kamodel.JiaSuDu.ToString();
            this.textBox9.Text = kamodel.HomeJiaSuDu.ToString();
            this.textBox10.Text = kamodel.HomeSuDu.ToString();
            this.textBox11.Text = kamodel.HuiLingMoShi.ToString();

        }

        public ZhouModel GetKaModel()
        {
            ZhouModel kamodel = ChangYong.FuZhiShiTi(KaCanShuModel);
            kamodel.ZhouName = this.txbMoShi.Text;
            kamodel.ZhouNO = (short)ChangYong.TryUShort(this.textBox1.Text, 0);
            kamodel.WuChaS = ChangYong.TryDouble(this.textBox2.Text,0);
            kamodel.WuChaX = ChangYong.TryDouble(this.textBox3.Text, 0);
            kamodel.JuLiZuiDi = ChangYong.TryDouble(this.textBox4.Text, 0);
            kamodel.JuLiZuiGao = ChangYong.TryDouble(this.textBox5.Text, 0);
            kamodel.TongYiType= ChangYong.TryInt(this.textBox6.Text, 0);
            kamodel.SuDu= ChangYong.TryDouble(this.textBox7.Text, 0);
            kamodel.JiaSuDu = ChangYong.TryDouble(this.textBox8.Text, 0);
            kamodel.HomeJiaSuDu = ChangYong.TryDouble(this.textBox9.Text, 0);
            kamodel.HomeSuDu = ChangYong.TryDouble(this.textBox10.Text, 0);
            kamodel.HuiLingMoShi= ChangYong.TryInt(this.textBox11.Text, 27);
            return kamodel;

        }
       
      
        private void button2_Click(object sender, EventArgs e)
        {
            //ZhouPeiZhiModel model = ZhouPeiZhiModel;
            //ZhouXianZhiFrm frm = new ZhouXianZhiFrm();
            //if (model != null)
            //{
            //    frm.SetCanShu(model, false);
            //}
            //else
            //{
            //    frm.SetCanShu(new ZhouPeiZhiModel(), false);
            //}
            //if (frm.ShowDialog(this) == DialogResult.OK)
            //{
            //    ZhouPeiZhiModel = ChangYong.FuZhiShiTi(frm.GetModel());
              

            //}
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
