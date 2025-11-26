using CommLei.JiChuLei;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ViSAKu.Model;

namespace ViSAKu.Frm
{
    public partial class CMDKJ : UserControl
    {
        private DataLieModel DataLieModel;
        public CMDKJ()
        {
            InitializeComponent();
        }
        public void SetCanShu(DataLieModel model)
        {
            List<string> meijus = ChangYong.MeiJuLisName(typeof(CunType));
            for (int i = 0; i < meijus.Count; i++)
            {
                this.comboBox2.Items.Add(meijus[i]);
            }
            if (this.comboBox2.Items.Count>0)
            {
                this.comboBox2.SelectedIndex = 0;
            }
            List<string> meijus1 = ChangYong.MeiJuLisName(typeof(ShuJuType));
            for (int i = 0; i < meijus1.Count; i++)
            {
                this.commBoxE1.Items.Add(meijus1[i]);
            }
            if (this.commBoxE1.Items.Count > 0)
            {
                this.commBoxE1.SelectedIndex = 0;
            }

            DataLieModel = ChangYong.FuZhiShiTi(model);
            this.txbMoShi.Text = DataLieModel.Name;
            this.textBox1.Text = DataLieModel.CMD.ToString();
            this.commBoxE1.Text = DataLieModel.ShuJuType.ToString();
          
            this.comboBox2.Text = DataLieModel.IsDu.ToString();
            this.textBox4.Text = DataLieModel.MiaoSu;
        }

        public DataLieModel GetModel()
        {
            DataLieModel model = ChangYong.FuZhiShiTi(DataLieModel);
            model.Name = this.txbMoShi.Text;
            model.CMD = this.textBox1.Text;
            model.ShuJuType =ChangYong.GetMeiJuZhi<ShuJuType>( this.commBoxE1.Text);
        
            model.MiaoSu = this.textBox4.Text;
            model.IsDu = ChangYong.GetMeiJuZhi<CunType>(this.comboBox2.Text);
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
