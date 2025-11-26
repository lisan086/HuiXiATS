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
            this.commBoxE1.SetCanShu<Define>();
            SaoMaModel = ChangYong.FuZhiShiTi(model);
            this.commBoxE1.Text = model.DeviceType.ToString();
            this.textBox1.Text= model.DeviceIndex.ToString();
            this.textBox2.Text = model.ChaoShiTime.ToString();
            this.textBox3.Text=model.IndexCount.ToString();
            this.textBox4.Text = model.SheBeiID.ToString();
            this.textBox5.Text = model.Name.ToString();
            this.textBox6.Text = model.YiZhiFaDeZhiLing.ToString();
            this.checkBox4.Checked = model.IsFD == 1;
        }
        public SaoMaModel GetSaoMaModel() 
        {
            SaoMaModel model = ChangYong.FuZhiShiTi(SaoMaModel);
            model.DeviceType =this.commBoxE1.GetCanShu<Define>();
            model.DeviceIndex =ChangYong.TryUInt( this.textBox1.Text,0);      
            model.IndexCount= ChangYong.TryInt(this.textBox3.Text, 1);
            model.SheBeiID = ChangYong.TryInt(this.textBox4.Text, 0);
            model.Name=this.textBox5.Text;
            model.YiZhiFaDeZhiLing = this.textBox6.Text;
            model.ChaoShiTime = ChangYong.TryInt(this.textBox2.Text,1000);
            model.IsFD = this.checkBox4.Checked ? 1 : 0;
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
