using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSJianMianJK.XiTong.Frm.FM;
using ATSJianMianJK.XiTong.Model;
using CommLei.JiChuLei;

namespace ATSJianMianJK.XiTong.Frm.KJ
{
    public partial class XieKJ : UserControl
    {
       
        private XieSateModel XieSateModel;
        public XieKJ()
        {
            InitializeComponent();
        }

        public void SetCanShu(XieSateModel sateModel)
        {
            this.commBoxE1.Items.Clear();
            List<string> xie = ChangYong.MeiJuLisName(typeof(XieSateType));
            XieSateModel = ChangYong.FuZhiShiTi(sateModel);
            for (int i = 0; i < xie.Count; i++)
            {
                this.commBoxE1.Items.Add(xie[i]);
            }
            this.textBox1.Text = XieSateModel.Name;
            this.commBoxE1.Text = XieSateModel.Type.ToString();
            this.textBox2.Text = XieSateModel.Miao.ToString();
           
        }


        public XieSateModel GetModel()
        {
            XieSateModel model = ChangYong.FuZhiShiTi(XieSateModel);
            model.Name = this.textBox1.Text;
            model.Miao = ChangYong.TryDouble(this.textBox2.Text,0);
            model.Type=ChangYong.GetMeiJuZhi<XieSateType>( this.commBoxE1.Text);
            return model;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.Parent != null)
            {
                this.Parent.Controls.Remove(this);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            XieJiCunQiFrm jiCunQiFrm = new XieJiCunQiFrm();
            jiCunQiFrm.SetCanShu(XieSateModel.LisXies);
            if (jiCunQiFrm.ShowDialog(this) == DialogResult.OK)
            {
                XieSateModel.LisXies = ChangYong.FuZhiShiTi(jiCunQiFrm.GetCanShu());
            }
        }
    }
}
