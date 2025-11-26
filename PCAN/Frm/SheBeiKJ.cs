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
            this.txbMoShi.Text = model.PHandle.ToString();
            this.textBox1.Text= model.m_Baudrate.ToString();
            this.textBox2.Text = model.ChaoShiTime.ToString();
            this.textBox4.Text = model.SheBeiID.ToString();
            this.textBox5.Text = model.Name.ToString();
          
       
        }
        public SaoMaModel GetSaoMaModel() 
        {
            SaoMaModel model = ChangYong.FuZhiShiTi(SaoMaModel);
            model.PHandle =(ushort)ChangYong.TryUShort(this.txbMoShi.Text,(ushort)0);
            model.m_Baudrate =ChangYong.TryStr( this.textBox1.Text,"");          
            model.SheBeiID = ChangYong.TryInt(this.textBox4.Text, 0);
            model.Name=this.textBox5.Text;
            model.ChaoShiTime = ChangYong.TryInt(this.textBox2.Text,1000);
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
