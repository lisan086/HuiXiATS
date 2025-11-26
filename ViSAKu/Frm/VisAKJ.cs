using CommLei.JiChuLei;
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
    public partial class VisAKJ : UserControl
    {
        
        private SheBeiVisaModel SheBeiVisaModel = new SheBeiVisaModel();
        public VisAKJ()
        {
            InitializeComponent();
        }


        public void SetCanShu(SheBeiVisaModel model,List<string> lianjienass) 
        {
            for (int i = 0; i < lianjienass.Count; i++)
            {
                this.comboBox2.Items.Add(lianjienass[i]);
            }
            SheBeiVisaModel = ChangYong.FuZhiShiTi(model);
            this.txbMoShi.Text = SheBeiVisaModel.Name;
            this.textBox1.Text = SheBeiVisaModel.SheBeiID.ToString();
            this.comboBox2.Text = SheBeiVisaModel.LianJieName;
            this.textBox2.Text = SheBeiVisaModel.XieYanShi.ToString();
        }
        public void SetTX(bool istx)
        {
            if (istx)
            {
                if (this.label5.BackColor != Color.Green)
                {
                    this.label5.BackColor = Color.Green;
                }
            }
            else
            {
                if (this.label5.BackColor != Color.Red)
                {
                    this.label5.BackColor = Color.Red;
                }
            }
        }
        public SheBeiVisaModel GetModel()
        {
            SheBeiVisaModel model = ChangYong.FuZhiShiTi(SheBeiVisaModel);
            model.Name = this.txbMoShi.Text;
            model.SheBeiID = ChangYong.TryInt(this.textBox1.Text,1);
            model.LianJieName = this.comboBox2.Text;
            SheBeiVisaModel.XieYanShi = ChangYong.TryInt(this.textBox2.Text, 10);
            return model;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            CMDFrm cMDFrm = new CMDFrm();
            cMDFrm.SetCanShu(SheBeiVisaModel.LisData);
            cMDFrm.ShowDialog(this);
            SheBeiVisaModel.LisData = ChangYong.FuZhiShiTi(cMDFrm.GetModel());
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
