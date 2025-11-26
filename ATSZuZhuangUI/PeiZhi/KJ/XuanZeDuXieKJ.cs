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
using SSheBei.PeiZhi;
using ZuZhuangUI.Model;

namespace ZuZhuangUI.PeiZhi.KJ
{
    public partial class XuanZeDuXieKJ : UserControl
    {
        private int SheBeiID = -1;
        public XuanZeDuXieKJ()
        {
            InitializeComponent();
        }
        public string SetText
        {
            get { return this.label3.Text; }
            set
            {
                this.label3.Text = value;
            }
        }

        public void SetCanShu(ShuJuLisModel model)
        {
            this.textBox2.Text = model.JCQStr;
            SheBeiID = model.SheBeiID ;
            this.textBox3.Text = model.SheBeiID.ToString();
            this.textBox1.Text = model.JiCunValue.ToString();
            this.textBox5.Text = model.StrKaiShi.ToString();
            this.textBox4.Text = model.StrCount.ToString();
        }
        public ShuJuLisModel GetCanShu()
        {
            ShuJuLisModel model = new ShuJuLisModel();
            model.JCQStr = this.textBox2.Text;
            model.JiCunValue = this.textBox1.Text;
            model.SheBeiID=ChangYong.TryInt( this.textBox3.Text,1) ;
            model.StrKaiShi= ChangYong.TryInt(this.textBox5.Text, -1);
            model.StrCount = ChangYong.TryInt(this.textBox4.Text, -1);
            if (model.StrKaiShi >= 0 && model.StrCount > 0)
            {
                model.IsStr = true;
            }
            else
            {
                model.IsStr = false; 
            }
            model.IsStr = false;
            return model;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int shebeiid = ChangYong.TryInt(this.textBox3.Text, 1);
            XuanZeJiCunQiFrm xuanZeJiCunQiFrm = new XuanZeJiCunQiFrm();
            xuanZeJiCunQiFrm.SetCanShu(this.textBox2.Text, shebeiid, 3);
            if (xuanZeJiCunQiFrm.ShowDialog(this) == DialogResult.OK)
            {
                this.textBox2.Text = xuanZeJiCunQiFrm.JiCunQiWeiYiBiaoShi;
                SheBeiID = xuanZeJiCunQiFrm.SheBeiID;
                this.textBox3.Text=SheBeiID.ToString();
            }
        }
    }
}
