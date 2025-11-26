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
using ZhongWangSheBei.Model;

namespace XiangTongChuanKouSheBei.Frm
{
    public partial class JiCunQiKJ : UserControl
    {
        public JiCunQiKJ()
        {
            InitializeComponent();
        }
        public void SetCanShu(CunModel model)
        {
            this.comboBox2.Items.Clear();
            List<string> meiju = ChangYong.MeiJuLisName(typeof(CunType));
            for (int i = 0; i < meiju.Count; i++)
            {
                this.comboBox2.Items.Add(meiju[i]);
            }
            if (this.comboBox2.Items.Count>0)
            { 
                this.comboBox2.SelectedIndex = 0;
            }
            this.textBox5.Text = model.Name;
            this.textBox6.Text = model.JiCunDiZhi.ToString();
            this.textBox2.Text = model.ChangDu.ToString();
            this.textBox3.Text = model.ChengShu.ToString();
            this.textBox1.Text = model.MiaoSu;
            this.comboBox2.Text = model.IsDu.ToString();
            this.textBox4.Text = model.ZhiLingID.ToString();
            this.textBox7.Text = model.QiTaCanShu;
        }

        public CunModel GetCanShu()
        {
            CunModel model = new CunModel();
            model.Name = this.textBox5.Text;
            model.JiCunDiZhi = ChangYong.TryInt(this.textBox6.Text, 1);
            model.MiaoSu = this.textBox1.Text;
            model.ChangDu = ChangYong.TryInt(this.textBox2.Text, 1);
            model.ChengShu = ChangYong.TryDouble(this.textBox3.Text, 1);
            model.IsDu = ChangYong.GetMeiJuZhi<CunType>(this.comboBox2.Text);
            model.QiTaCanShu = this.textBox7.Text;
            model.ZhiLingID = ChangYong.TryInt(this.textBox4.Text, -1);
            return model;
        }
    }
}
