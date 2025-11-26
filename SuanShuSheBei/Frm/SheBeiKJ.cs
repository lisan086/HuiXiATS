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
using SuanShuSheBei.Frm;
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
           
            this.textBox4.Text = model.SheBeiID.ToString();
            this.textBox5.Text = model.Name.ToString();
            this.textBox6.Text = model.Time.ToString();
           
        }
        public SaoMaModel GetSaoMaModel() 
        {
            SaoMaModel model = ChangYong.FuZhiShiTi(SaoMaModel);
              
            model.SheBeiID = ChangYong.TryInt(this.textBox4.Text, 0);
            model.Name=this.textBox5.Text;
            model.Time = ChangYong.TryInt(this.textBox6.Text, 0);
        
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
            JiaoBenFrm jiaoBenFrm = new JiaoBenFrm();
            jiaoBenFrm.SetCanShu(SaoMaModel.LisJiaoBen);
            DialogResult jieguo=  jiaoBenFrm.ShowDialog(this);
            if (jieguo== DialogResult.OK)
            {
                SaoMaModel.LisJiaoBen = jiaoBenFrm.GetModel();
            }
        }
    }
}
