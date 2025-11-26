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
using ModBuTCP.Model;

namespace ModBuTCP.Frm
{
    public partial class SheBeiKJ : UserControl
    {
        private SheBeiModel LisData = new SheBeiModel();
        public SheBeiKJ()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }
        public void SetCanShu(SheBeiModel shuju)
        {

            LisData = ChangYong.FuZhiShiTi(shuju);
            this.textBox4.Text = shuju.IpOrCom.ToString();
            this.textBox1.Text = shuju.Port.ToString();
            this.textBox2.Text = shuju.SheBeiID.ToString();
            this.textBox3.Text = shuju.SheBeiName.ToString();
            this.textBox5.Text = shuju.XieYanShi.ToString();
            this.textBox6.Text = shuju.DuYanShi.ToString();
            this.textBox7.Text = shuju.DiZhi.ToString();
        }

        //获取寄存器的数据
        public SheBeiModel GetCanShu()
        {
            SheBeiModel model = ChangYong.FuZhiShiTi(LisData);
            model.SheBeiID = ChangYong.TryInt(this.textBox2.Text, -1);
            model.Port = ChangYong.TryInt(this.textBox1.Text, -1);
            model.IpOrCom = this.textBox4.Text;
            model.SheBeiName = this.textBox3.Text;
            model.XieYanShi= ChangYong.TryInt(this.textBox5.Text, -1);
            model.DuYanShi = ChangYong.TryInt(this.textBox6.Text, -1);
            model.DiZhi = ChangYong.TryInt(this.textBox7.Text, -1);
            return model;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (this.Parent != null)
            {
                this.Parent.Controls.Remove(this);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //PLCZPeiZhiFrm zhanFrm = new PLCZPeiZhiFrm();
            //zhanFrm.SetShuJu(LisData.DataCunModels);
            //if (zhanFrm.ShowDialog(this) == DialogResult.OK)
            //{
            //    LisData.DataCunModels = ChangYong.FuZhiShiTi(zhanFrm.GetShuJu());
            //}
        }
    }
}
