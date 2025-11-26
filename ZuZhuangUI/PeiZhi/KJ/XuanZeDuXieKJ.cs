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
            this.textBox1.Text = model.JiCunValue;
        }
        public ShuJuLisModel GetCanShu()
        {
            ShuJuLisModel model = new ShuJuLisModel();
            model.JCQStr = this.textBox2.Text;
            model.JiCunValue = this.textBox1.Text;
            model.SheBeiID= this.SheBeiID ;
            return model;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int shebeiid = SheBeiID;
            XuanZeJiCunQiFrm xuanZeJiCunQiFrm = new XuanZeJiCunQiFrm();
            xuanZeJiCunQiFrm.SetCanShu(this.textBox2.Text, shebeiid, 3);
            if (xuanZeJiCunQiFrm.ShowDialog(this) == DialogResult.OK)
            {
                this.textBox2.Text = xuanZeJiCunQiFrm.JiCunQiWeiYiBiaoShi;
                SheBeiID = xuanZeJiCunQiFrm.SheBeiID;
             
            }
        }
    }
}
