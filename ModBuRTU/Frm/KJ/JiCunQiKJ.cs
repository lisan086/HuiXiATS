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
using SSheBei.Model;
using SundyChengZhong.Model;

namespace SundyChengZhong.Frm.KJ
{
    public partial class JiCunQiKJ : UserControl
    {
        private CZJiCunQiModel cZJiCunQiModel;
        public JiCunQiKJ()
        {
            InitializeComponent();
        }
        public void SetCanShu(CZJiCunQiModel lsmodel)
        {
            List<string> meijus = ChangYong.MeiJuLisName(typeof(DataSType));
            for (int i = 0; i < meijus.Count; i++)
            {
                this.commBoxE1.Items.Add(meijus[i]);
            }
            if (this.commBoxE1.Items.Count>0)
            {
                this.commBoxE1.SelectedIndex = 0;
            }
            cZJiCunQiModel = ChangYong.FuZhiShiTi(lsmodel);
            this.txbMoShi.Text = cZJiCunQiModel.Name;
            this.textBox1.Text = cZJiCunQiModel.JiCunDiZhi.ToString();
            this.textBox2.Text = cZJiCunQiModel.Count.ToString();
            this.textBox3.Text = cZJiCunQiModel.XiaoShuWei.ToString();
            this.textBox8.Text = cZJiCunQiModel.BeiChuShu.ToString();
            this.textBox4.Text = cZJiCunQiModel.SheBeiDiZhi.ToString();
            this.textBox5.Text = cZJiCunQiModel.DuGNM.ToString();
            this.textBox6.Text = cZJiCunQiModel.XieGNM.ToString();
            this.textBox7.Text = cZJiCunQiModel.MiaoSu;
            this.textBox9.Text=cZJiCunQiModel.BZhi.ToString();
            this.commBoxE1.Text = cZJiCunQiModel.DataSType.ToString();
        }

        public CZJiCunQiModel GetModel()
        {
            CZJiCunQiModel lsmodel = ChangYong.FuZhiShiTi(cZJiCunQiModel);
            lsmodel.Name = this.txbMoShi.Text;
            lsmodel.JiCunDiZhi = ChangYong.TryInt(this.textBox1.Text, 0);
            lsmodel.Count = ChangYong.TryInt(this.textBox2.Text, 0);
            lsmodel.SheBeiDiZhi = ChangYong.TryInt(this.textBox4.Text, 1);
            lsmodel.XiaoShuWei = ChangYong.TryInt(this.textBox3.Text, -1);
            lsmodel.BeiChuShu = ChangYong.TryDouble(this.textBox8.Text, 0);
            lsmodel.DuGNM = ChangYong.TryInt(this.textBox5.Text,3);
            lsmodel.XieGNM = ChangYong.TryInt(this.textBox6.Text, 3);
            lsmodel.MiaoSu = this.textBox7.Text;
            lsmodel.DataSType = ChangYong.GetMeiJuZhi<DataSType>(this.commBoxE1.Text);
            lsmodel.BZhi = ChangYong.TryDouble(this.textBox9.Text,0);
            return lsmodel;
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
