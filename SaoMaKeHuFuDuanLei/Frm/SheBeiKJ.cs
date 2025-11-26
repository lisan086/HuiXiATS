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
        private IPFuWuPeiModel IPFuWuPeiModel;
        public SheBeiKJ()
        {
            InitializeComponent();
        }

        public void SetCanShu(IPFuWuPeiModel shuju)
        {
            IPFuWuPeiModel = ChangYong.FuZhiShiTi(shuju);
            this.textBox2.Text = shuju.IP;
            this.textBox3.Text = shuju.Port.ToString();
            this.textBox4.Text = shuju.PaiXu.ToString();
            this.textBox5.Text = shuju.IsTongYiGe.ToString();
            this.textBox7.Text = shuju.IsFuWuDuan.ToString();
            this.textBox1.Text = shuju.ZhanDianName;
            this.textBox6.Text = shuju.IsQingQiu.ToString();
            this.textBox8.Text= shuju.DiZhi.ToString();
            this.textBox9.Text = shuju.DataLuJing;
        }
        public IPFuWuPeiModel GetSaoMaModel() 
        {
            IPFuWuPeiModel shuju = ChangYong.FuZhiShiTi(IPFuWuPeiModel);
            shuju.IP = this.textBox2.Text;
            shuju.Port = ChangYong.TryInt(this.textBox3.Text, 9989);
            shuju.IsFuWuDuan = ChangYong.TryInt(this.textBox7.Text, 3);
            shuju.ZhanDianName = this.textBox1.Text;
            shuju.PaiXu = ChangYong.TryInt(this.textBox4.Text, 0);
            shuju.IsTongYiGe = ChangYong.TryInt(this.textBox5.Text, 0);
            shuju.IsQingQiu= ChangYong.TryInt(this.textBox6.Text, 0);
            shuju.DiZhi= this.textBox8.Text;
            shuju.DataLuJing= this.textBox9.Text;
            return shuju;
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
