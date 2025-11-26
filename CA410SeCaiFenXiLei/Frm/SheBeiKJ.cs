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
using YiBanSaoMaQi.Model;

namespace YiBanSaoMaQi.Frm
{
    public partial class SheBeiKJ : UserControl
    {
        private SaoMaModel SaoMaModel;
        public SheBeiKJ()
        {
            InitializeComponent();
        }

        public void SetCanShu(SaoMaModel model)
        {
            SaoMaModel = ChangYong.FuZhiShiTi(model);
            this.txbMoShi.Text = model.IpOrCom;
            this.textBox1.Text = model.Port.ToString();

            this.textBox4.Text = model.SheBeiID.ToString();
            this.textBox5.Text = model.Name.ToString();
            this.textBox2.Text = model.XieTime.ToString();
            this.textBox3.Text = model.DuChaoShiTime.ToString();
            this.textBox6.Text = $"{model.JiZhunACD}";
         
        }
        public SaoMaModel GetSaoMaModel() 
        {
            SaoMaModel model = ChangYong.FuZhiShiTi(SaoMaModel);
            model.IpOrCom = this.txbMoShi.Text;
            model.Port =ChangYong.TryInt( this.textBox1.Text,0);          
            model.SheBeiID = ChangYong.TryInt(this.textBox4.Text, 0);
            model.Name=this.textBox5.Text;
            model.XieTime= ChangYong.TryInt(this.textBox2.Text, 10);
            model.DuChaoShiTime= ChangYong.TryInt(this.textBox3.Text, 1000);
            model.JiZhunACD= ChangYong.TryDouble(this.textBox6.Text, 0);
         
            return model;
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
