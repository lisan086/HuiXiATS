using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSJianMianJK.XiTong.Model;
using ATSJianMianJK.XiTong.XianShiDuFrm.Frm;
using CommLei.JiChuLei;
using JieMianLei.UI;
using SSheBei.Model;

namespace ATSJianMianJK.XiTong.XianShiDuFrm.KJ
{
    public partial class XieKuaiKJ : UserControl
    {
        private XieSateModel XieSateModel;
      
     
        public XieKuaiKJ()
        {
            InitializeComponent();
        }
        public void SetCanShu(XieSateModel model)
        {
            XieSateModel = model;
            this.commBoxE1.Items.Clear();
            List<string> xie = ChangYong.MeiJuLisName(typeof(XieSateType));

            for (int i = 0; i < xie.Count; i++)
            {
                this.commBoxE1.Items.Add(xie[i]);
            }
            this.textBox1.Text = XieSateModel.Name;
            this.textBox1.Enabled = false;
            this.commBoxE1.Text = XieSateModel.Type.ToString();
            this.commBoxE1.Enabled = false;


        }

    

        protected void QiDongTiShiKuang(string msg, int shijian = 5)
        {
            MsgBoxFrom msgBoxFrom = new MsgBoxFrom();
            msgBoxFrom.AddMsg(msg);
            msgBoxFrom.SetCanShu(IsQiDongZiDongGuanBi: true, "确定", "", shijian);
            msgBoxFrom.TopMost = true;
            msgBoxFrom.BringToFront();
            msgBoxFrom.ShowDialog();
        }



        private void button2_Click(object sender, EventArgs e)
        {
            ZhiXingXieFrm zhiXingXieFrm = new ZhiXingXieFrm();
            zhiXingXieFrm.SetCanShu(XieSateModel.LisXies,XieSateModel.Name, XieSateModel.TDID);
            zhiXingXieFrm.Show(this);
        }
    }
}
