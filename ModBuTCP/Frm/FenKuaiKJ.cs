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
using ModBuTCP.Model;

namespace ModBuTCP.Frm
{
    public partial class FenKuaiKJ : UserControl
    {
        private List<DataCunModel> CZJiCunQiModels;
        public FenKuaiKJ()
        {
            InitializeComponent();
        }
        public void SetCanShu(string kuaming, List<DataCunModel> lisshuju)
        {
            this.label4.Text = kuaming;
            CZJiCunQiModels = ChangYong.FuZhiShiTi(lisshuju);
        }


        public List<DataCunModel> GetShuJu()
        {
            return CZJiCunQiModels;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PLCZPeiZhiFrm kj = new PLCZPeiZhiFrm();
            kj.SetShuJu(CZJiCunQiModels);
            if (kj.ShowDialog(this) == DialogResult.OK)
            {
                CZJiCunQiModels = ChangYong.FuZhiShiTi(kj.GetShuJu());
            }
        }
    }
}
