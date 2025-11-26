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
using ZuZhuangUI.Model;
using ZuZhuangUI.PeiZhi.Frm;

namespace ZuZhuangUI.PeiZhi.KJ
{
    public partial class SheBeiZhanKJ : UserControl
    {
        private SheBeiZhanModel SheBeiZhanModel;
        public SheBeiZhanKJ()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }
        public void SetCanShu(SheBeiZhanModel model)
        {
            this.commBoxE1.Items.Clear();
            this.commBoxE1.SetCanShu<SheBeiType>();
            SheBeiZhanModel = ChangYong.FuZhiShiTi(model);
            this.textBox1.Text = model.LineCode;
            this.textBox2.Text = model.XingHao;       
            this.textBox4.Text = model.GWID.ToString();
            this.textBox5.Text = model.MaName.ToString();
            this.commBoxE1.Text = model.IsZhengZhanDian.ToString();
            this.checkBox2.Checked = model.IsMes;
            this.checkBox3.Checked = model.IsQuanBuShangChuan==1;
        }

        public SheBeiZhanModel GetCanShu()
        {
            SheBeiZhanModel datas = ChangYong.FuZhiShiTi(SheBeiZhanModel);
            datas.LineCode = this.textBox1.Text;
            datas.XingHao = this.textBox2.Text;
            datas.GWID = ChangYong.TryInt(this.textBox4.Text, -1);
            datas.IsZhengZhanDian = this.commBoxE1.GetCanShu<SheBeiType>();         
            datas.IsMes = this.checkBox2.Checked;
            datas.IsQuanBuShangChuan=this.checkBox3 .Checked?1:0;
            datas.MaName= this.textBox5.Text;
            for (int i = 0; i < datas.LisQingQiu.Count; i++)
            {
                datas.LisQingQiu[i].GWID = datas.GWID;
            }
            for (int i = 0; i < datas.LisData.Count; i++)
            {
                datas.LisData[i].GWID = datas.GWID;
            }
            return datas;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.Parent != null)
            {
                this.Parent.Controls.Remove(this);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            YeWuShuJuFrm zhanFrm = new YeWuShuJuFrm();
            zhanFrm.SetCanShu(SheBeiZhanModel.LisData, true,"");
            zhanFrm.ShowDialog(this);
            SheBeiZhanModel.LisData = ChangYong.FuZhiShiTi(zhanFrm.GetModel());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            YeWuShuJuFrm zhanFrm = new YeWuShuJuFrm();
            zhanFrm.SetCanShu(SheBeiZhanModel.LisQingQiu, false,ChangYong.GetEnumDescription(SheBeiZhanModel.IsZhengZhanDian));
            zhanFrm.ShowDialog(this);
            SheBeiZhanModel.LisQingQiu = ChangYong.FuZhiShiTi(zhanFrm.GetModel());
        }
    }
}
