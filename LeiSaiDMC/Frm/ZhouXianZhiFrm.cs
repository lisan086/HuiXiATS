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
using LeiSaiDMC.Model;

namespace LeiSaiDMC.Frm
{
    public partial class ZhouXianZhiFrm : BaseFuFrom
    {
        public ZhouXianZhiFrm()
        {
            InitializeComponent();
            this.IsZhiXianShiX = true;
        }

        public void SetCanShu(ZhouPeiZhiModel peiZhiModel,bool istiaoshi)
        {
            if (istiaoshi)
            {
                txbMoShi.Enabled = false;
              
            }
            else
            {
            }
            txbMoShi.Text = peiZhiModel.ZhouPeiZhiID.ToString();
            textBox1.Text = peiZhiModel.Low_Vel.ToString();
            textBox2.Text = peiZhiModel.High_VelZ.ToString();
            textBox3.Text = peiZhiModel.Tacc.ToString();
            textBox4.Text = peiZhiModel.Tdec.ToString();
            textBox5.Text= peiZhiModel.StopTime.ToString();
            textBox6.Text = peiZhiModel.Stop_Vel.ToString();
            textBox7.Text= peiZhiModel.Spara.ToString();
            textBox9.Text = peiZhiModel.JuLiZuiDi.ToString();
            textBox8.Text = peiZhiModel.JuLiZuiGao.ToString();
            textBox11.Text = peiZhiModel.WuChaS.ToString();
            textBox10.Text = peiZhiModel.WuChaX.ToString();
            textBox13.Text = peiZhiModel.HomeMothod.ToString();
            textBox12.Text = peiZhiModel.HomeHigh_Vel.ToString();
            textBox15.Text = peiZhiModel.HomeLow_Vel.ToString();
            textBox14.Text = peiZhiModel.HomeTacc.ToString();
            textBox17.Text = peiZhiModel.HomeTdec.ToString();
            textBox16.Text = peiZhiModel.HomeOffsetpos.ToString();
        }

        public ZhouPeiZhiModel GetModel()
        {
            ZhouPeiZhiModel peiZhiModel = new ZhouPeiZhiModel();
            peiZhiModel.ZhouPeiZhiID = ChangYong.TryInt(txbMoShi.Text,-1);
            peiZhiModel.Low_Vel = ChangYong.TryDouble(this.textBox1.Text,0d);
            peiZhiModel.High_VelZ= ChangYong.TryDouble(this.textBox2.Text, 0d);
            peiZhiModel.Tacc = ChangYong.TryDouble(this.textBox3.Text, 0d);
            peiZhiModel.Tdec = ChangYong.TryDouble(this.textBox4.Text, 0d);
            peiZhiModel.StopTime = ChangYong.TryDouble(this.textBox5.Text, 0d);
            peiZhiModel.Stop_Vel = ChangYong.TryDouble(this.textBox6.Text, 0d);
            peiZhiModel.Spara = ChangYong.TryDouble(this.textBox7.Text, 0d);
            peiZhiModel.JuLiZuiDi = ChangYong.TryDouble(this.textBox9.Text, 0d);
            peiZhiModel.JuLiZuiGao = ChangYong.TryDouble(this.textBox8.Text, 0d);
            peiZhiModel.WuChaS = ChangYong.TryDouble(this.textBox11.Text, 0d);
            peiZhiModel.WuChaX = ChangYong.TryDouble(this.textBox10.Text, 0d);
            peiZhiModel.HomeMothod = (ushort)ChangYong.TryInt(this.textBox13.Text, 0);
            peiZhiModel.HomeHigh_Vel = ChangYong.TryDouble(this.textBox12.Text, 0d);
            peiZhiModel.HomeLow_Vel = ChangYong.TryDouble(this.textBox15.Text, 0d);
            peiZhiModel.HomeTacc = ChangYong.TryDouble(this.textBox14.Text, 0d);
            peiZhiModel.HomeTdec = ChangYong.TryDouble(this.textBox17.Text, 0d);
            peiZhiModel.HomeOffsetpos = ChangYong.TryDouble(this.textBox16.Text, 0d);
            return peiZhiModel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

      
    }
}
