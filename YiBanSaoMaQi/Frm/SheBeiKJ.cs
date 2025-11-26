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
            List<string> meijus = ChangYong.MeiJuLisName(typeof(JieXieDataType));
            for (int i = 0; i < meijus.Count; i++)
            {
               this.comboBox3.Items.Add(meijus[i]);
            }
            SaoMaModel = ChangYong.FuZhiShiTi(model);
            this.txbMoShi.Text = model.IpOrCom;
            this.textBox1.Text= model.Port.ToString();
            this.textBox2.Text = model.KaiShiSaoMaZhiLing.ToString();
            this.textBox3.Text = model.JieGuoSaoMaZhiLing.ToString();
            this.textBox4.Text = model.SheBeiID.ToString();
            this.textBox5.Text = model.Name.ToString();
            this.textBox6.Text = model.Time.ToString();
            this.textBox7.Text = model.ChangDu.ToString();
            this.comboBox3.Text= model.JieXieDataType.ToString();
            for (int i = 0; i < this.comboBox1.Items.Count; i++)
            {
                string zhi = this.comboBox1.Items[i].ToString().Split(':')[0];
                if (zhi==model.FaGeShi.ToString())
                {
                    this.comboBox1.SelectedIndex = i;
                    break;
                }
            }

            for (int i = 0; i < this.comboBox2.Items.Count; i++)
            {
                string zhi = this.comboBox2.Items[i].ToString().Split(':')[0];
                if (zhi == model.JieXiType.ToString())
                {
                    this.comboBox2.SelectedIndex = i;
                    break;
                }
            }
        }
        public SaoMaModel GetSaoMaModel() 
        {
            SaoMaModel model = ChangYong.FuZhiShiTi(SaoMaModel);
            model.IpOrCom = this.txbMoShi.Text;
            model.Port =ChangYong.TryInt( this.textBox1.Text,0);
            model.KaiShiSaoMaZhiLing = this.textBox2.Text;
            model.JieGuoSaoMaZhiLing=this.textBox3.Text;
            model.SheBeiID = ChangYong.TryInt(this.textBox4.Text, 0);
            model.Name=this.textBox5.Text;
            model.Time = ChangYong.TryInt(this.textBox6.Text, 0);
            model.JieXiType = ChangYong.TryInt(this.comboBox2.Text.Split(':')[0], 0);
            model.FaGeShi = ChangYong.TryInt(this.comboBox1.Text.Split(':')[0], 0);
            model.ChangDu = this.textBox7.Text;
            model.JieXieDataType = ChangYong.GetMeiJuZhi<JieXieDataType>(this.comboBox3.Text);
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
