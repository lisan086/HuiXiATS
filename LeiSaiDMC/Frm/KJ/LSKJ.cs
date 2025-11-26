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
            this.textBox2.Text = LSModel.CardType.ToString();
        }

        public LSModel GetModel()
        {
            LSModel lsmodel = ChangYong.FuZhiShiTi(LSModel);
            lsmodel.Name = this.txbMoShi.Text;
            lsmodel.SheBeiID = ChangYong.TryInt(this.textBox1.Text,1);
            lsmodel.CardType= ChangYong.TryInt(this.textBox2.Text, 1);
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
            ferm.SetCanShu(LSModel.LisKaModels,LSModel.LisZhouPeiZhiModels);
            ferm.ShowDialog(this);
            List<ZhouPeiZhiModel> zhous = new List<ZhouPeiZhiModel>();
            List<KaCanShuModel> liska = ferm.GetModel(out zhous);
            LSModel.LisKaModels = ChangYong.FuZhiShiTi(liska);
            LSModel.LisZhouPeiZhiModels = ChangYong.FuZhiShiTi(zhous);
        }
    }
}
