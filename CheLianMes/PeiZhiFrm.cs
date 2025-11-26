using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common.DataChuLi;
using JieMianLei.FuFrom;

namespace CheLianMes
{
    public partial class PeiZhiFrm : BaseFuFrom
    {
        public PeiZhiFrm()
        {
            InitializeComponent();
        }

        public void SetCanShu()
        {
            IniGetModelLei<PeiZhiMesWenDanModel> dushuju = new IniGetModelLei<PeiZhiMesWenDanModel>("PeiZhiMesWenDanModel");

            PeiZhiMesWenDanModel CanShuModel = dushuju.GetTModel();
            this.textBox1.Text = CanShuModel.SheBeiBianHao;
            this.textBox2.Text = CanShuModel.TongDaoName;
            this.textBox4.Text = CanShuModel.ShuJuWangZhi;
            this.textBox3.Text = CanShuModel.QianZhanWangZhi;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IniGetModelLei<PeiZhiMesWenDanModel> dushuju = new IniGetModelLei<PeiZhiMesWenDanModel>("PeiZhiMesWenDanModel");
            PeiZhiMesWenDanModel CanShuModel = new PeiZhiMesWenDanModel();
            CanShuModel.SheBeiBianHao = this.textBox1.Text;
            CanShuModel.TongDaoName=this.textBox2.Text;
            CanShuModel.ShuJuWangZhi=this.textBox4.Text;
            CanShuModel.QianZhanWangZhi= this.textBox3.Text;
            dushuju.XieTModel(CanShuModel);
        }
    }
}
