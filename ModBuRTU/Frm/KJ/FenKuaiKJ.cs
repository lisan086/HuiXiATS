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
using SundyChengZhong.Frm.KJ;
using SundyChengZhong.Model;

namespace ModBusRTU.Frm.KJ
{
    public partial class FenKuaiKJ : UserControl
    {
        private List<CZJiCunQiModel> CZJiCunQiModels;
        public FenKuaiKJ()
        {
            InitializeComponent();
        }

        public void SetCanShu(string kuaming,List<CZJiCunQiModel> lisshuju)
        {
            this.label4.Text = kuaming;
            CZJiCunQiModels = ChangYong.FuZhiShiTi(lisshuju);
        }


        public List<CZJiCunQiModel> GetShuJu()
        {
            return CZJiCunQiModels;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            JiCunQiFrm kj = new JiCunQiFrm();
            kj.SetCanShu(CZJiCunQiModels);
            if (kj.ShowDialog(this) == DialogResult.OK)
            {
                CZJiCunQiModels = ChangYong.FuZhiShiTi(kj.GetModel());
            }
        }
    }
}
