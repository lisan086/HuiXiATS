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

        public void SetCanShu(SaoMaModel model,List<string> wangkas)
        {
            this.comboBox2.Items.Clear();
            for (int i = 0; i < wangkas.Count; i++)
            {
                this.comboBox2.Items.Add(wangkas[i]);
            }
            SaoMaModel = ChangYong.FuZhiShiTi(model);
            this.txbMoShi.Text = model.Ip;
            this.textBox1.Text = ChangYong.FenGeDaBao(model.GuanLianSBID,",");
        
            this.textBox4.Text = model.SheBeiID.ToString();
            this.textBox5.Text = model.Name.ToString();
            this.textBox6.Text = model.Time.ToString();
            this.comboBox2.Text = model.WangKaName;
        }
        public SaoMaModel GetSaoMaModel() 
        {
            SaoMaModel model = ChangYong.FuZhiShiTi(SaoMaModel);
            model.Ip= this.txbMoShi.Text;
            model.GuanLianSBID =ChangYong.JieGeInt( this.textBox1.Text,',');          
            model.SheBeiID = ChangYong.TryInt(this.textBox4.Text, 0);
            model.Name=this.textBox5.Text;
            model.Time = ChangYong.TryInt(this.textBox6.Text, 0);
            model.WangKaName=this.comboBox2.Text;
          
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
