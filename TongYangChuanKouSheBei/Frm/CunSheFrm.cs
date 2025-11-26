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
using JieMianLei.FuFrom;
using ZhongWangSheBei.Model;

namespace XiangTongChuanKouSheBei.Frm
{
    public partial class CunSheFrm : BaseFuFrom
    {
     
        public CunSheFrm()
        {
            InitializeComponent();
            this.IsZhiXianShiX = true;
        }
        public void SetCanShu(DuZhiLingModel model)
        {
           
            this.textBox5.Text=model.ZhiLingID.ToString();
            this.textBox6.Text = model.QiShiDiZhi.ToString();
            this.textBox2.Text = model.DuZhiLing.ToString();       
            this.textBox1.Text=model.ShuJuChangDu.ToString();
        }

        public DuZhiLingModel GetCanShu()
        {
            DuZhiLingModel model = new DuZhiLingModel();
            model.ZhiLingID = ChangYong.TryInt(this.textBox5.Text, 1);
            model.QiShiDiZhi = ChangYong.TryInt(this.textBox6.Text, 1);
            model.DuZhiLing = this.textBox2.Text;
            model.ShuJuChangDu= ChangYong.TryInt(this.textBox1.Text, 1);
            return model;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
