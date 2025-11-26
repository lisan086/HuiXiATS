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
using LeiSaiDMC.Model;

namespace LeiSaiDMC.Frm.KJ
{
    public partial class LSKJ : UserControl
    {
        private LSModel LSModel;
        public LSKJ()
        {
            InitializeComponent();
        }

        public void SetCanShu(LSModel lsmodel)
        {
            LSModel = ChangYong.FuZhiShiTi(lsmodel);
            this.txbMoShi.Text = LSModel.Name;
            this.textBox1.Text = LSModel.SheBeiID.ToString();
            this.textBox2.Text = LSModel.CardNO.ToString();
            this.checkBox2.Checked = LSModel.IsDuanXianBaoChi;
            this.textBox3.Text = LSModel.DevWenJianXML;
            this.textBox4.Text = LSModel.DevWenJianXML;
        }

        public LSModel GetModel()
        {
            LSModel lsmodel = ChangYong.FuZhiShiTi(LSModel);
            lsmodel.Name = this.txbMoShi.Text;
            lsmodel.SheBeiID = ChangYong.TryInt(this.textBox1.Text,1);
            lsmodel.CardNO = (short)ChangYong.TryInt(this.textBox2.Text, 1);
            lsmodel.IsDuanXianBaoChi = this.checkBox2.Checked;
            lsmodel.DevWenJianXML = this.textBox3.Text;
            lsmodel.SysWenJianXML= this.textBox4.Text;
            return lsmodel;
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
            ZhouOrIOFrm ferm = new ZhouOrIOFrm();
            ferm.SetCanShu(LSModel.LisZhouModel);
            DialogResult jieguo= ferm.ShowDialog(this);
            if (jieguo == DialogResult.OK)
            {
                
                LSModel.LisZhouModel = ChangYong.FuZhiShiTi(ferm.GetZhouModel());
          
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ZhouOrIOFrm ferm = new ZhouOrIOFrm();
            ferm.SetCanShu(LSModel.IOS);
            DialogResult jieguo = ferm.ShowDialog(this);
            if (jieguo == DialogResult.OK)
            {

                LSModel.IOS = ChangYong.FuZhiShiTi(ferm.GetIOModel());

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog wenjian = new OpenFileDialog();
            wenjian.FileName = this.textBox3.Text;
            if (wenjian.ShowDialog(this)==DialogResult.OK)
            {
                this.textBox3.Text = wenjian.FileName;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog wenjian = new OpenFileDialog();
            wenjian.FileName = this.textBox4.Text;
            if (wenjian.ShowDialog(this) == DialogResult.OK)
            {
                this.textBox4.Text = wenjian.FileName;
            }
        }
    }
}
