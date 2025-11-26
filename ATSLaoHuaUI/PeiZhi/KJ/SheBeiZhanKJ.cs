using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSLaoHuaUI.PeiZhi.Frm;
using CommLei.JiChuLei;
using SSheBei.Model;
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
            this.commBoxE2.Items.Clear();
            this.commBoxE1.SetCanShu<SheBeiType>();
            this.commBoxE2.SetCanShu<SheBeiZuType>();
            SheBeiZhanModel = ChangYong.FuZhiShiTi(model);

            this.textBox1.Text = model.XianTi;
            this.textBox2.Text = model.GaoWenName;      
            this.textBox4.Text = model.GWID.ToString();   
            this.commBoxE1.Text = model.SheBeiType.ToString();
            this.commBoxE2.Text = model.SheBeiZu.ToString();
            this.checkBox2.Checked = model.IsShangMes==1;
            this.textBox7.Text=model.SetTestTime.ToString();
            this.textBox5.Text = model.WenDuLow.ToString();
            this.textBox6.Text = model.WenDuUp.ToString();
            this.textBox9.Text = model.ShengWenTime.ToString();
            this.textBox8.Text = model.MeiGeCaiJiTime.ToString();
            this.textBox11.Text = model.MeiCiCANJianGeTime.ToString();
            this.textBox10.Text = model.JinGeCanSendTime.ToString();
            this.textBox3.Text = model.QiTa.ToString();
            this.textBox12.Text = model.ZuiDiWenDuLow.ToString();
        }

        public SheBeiZhanModel GetCanShu()
        {
            SheBeiZhanModel datas = ChangYong.FuZhiShiTi(SheBeiZhanModel);
            datas.XianTi = this.textBox1.Text;
            datas.GaoWenName = this.textBox2.Text;
            datas.GWID = ChangYong.TryInt(this.textBox4.Text, -1);
            datas.SheBeiType = this.commBoxE1.GetCanShu<SheBeiType>();           
            datas.IsShangMes = this.checkBox2.Checked?1:0;
            datas.SetTestTime = ChangYong.TryDouble(this.textBox7.Text, 60);
            datas.WenDuLow = ChangYong.TryDouble(this.textBox5.Text,60);
            datas.WenDuUp = ChangYong.TryDouble(this.textBox6.Text, 150);
            datas.MeiCiCANJianGeTime = ChangYong.TryInt(this.textBox11.Text, 5000);
            datas.SheBeiZu = this.commBoxE2.Text;
            datas.ShengWenTime= ChangYong.TryDouble(this.textBox9.Text, 60);
            datas.MeiGeCaiJiTime = ChangYong.TryFloat(this.textBox8.Text, 2);
            datas.JinGeCanSendTime = ChangYong.TryInt(this.textBox10.Text, 100);
            datas.QiTa= this.textBox3.Text;
            datas.ZuiDiWenDuLow = ChangYong.TryDouble(this.textBox12.Text, 30);
            for (int i = 0; i < datas.LisQingQiu.Count; i++)
            {
                datas.LisQingQiu[i].GWID = datas.GWID;
                datas.LisQingQiu[i].MaTD = -1;
            }
            for (int i = 0; i < datas.LisMaTD.Count; i++)
            {
                datas.LisMaTD[i].GWID = datas.GWID;
                foreach (var item in datas.LisMaTD[i].LisData)
                {
                    item.GWID= datas.GWID;
                    item.MaTD = datas.LisMaTD[i].MaTDID;
                }
                foreach (var item in datas.LisMaTD[i].LisKongZhi)
                {
                    item.GWID = datas.GWID;
                    item.MaTD = datas.LisMaTD[i].MaTDID;
                }
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
            TDMaFrm zhanFrm = new TDMaFrm();
            zhanFrm.SetCanShu(SheBeiZhanModel.LisMaTD);
            zhanFrm.ShowDialog(this);
            SheBeiZhanModel.LisMaTD = ChangYong.FuZhiShiTi(zhanFrm.GetModel());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            YeWuShuJuFrm zhanFrm = new YeWuShuJuFrm();
            zhanFrm.SetCanShu(SheBeiZhanModel.LisQingQiu, 1);
            zhanFrm.ShowDialog(this);
            SheBeiZhanModel.LisQingQiu = ChangYong.FuZhiShiTi(zhanFrm.GetModel());
        }
    }
}
