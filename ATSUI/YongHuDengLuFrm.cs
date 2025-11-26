using ATSJianMianJK.QuanXian;
using Common.JieMianLei;
using JieMianLei.FuFrom;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATSUI
{
    public partial class YongHuDengLuFrm : BaseFuFrom
    {
      
        public YongHuDengLuFrm()
        {
            InitializeComponent();
            this.IsZhiXianShiX = true;
            AnEnterJinRuXia anEnterJinRuXia = new AnEnterJinRuXia();
            anEnterJinRuXia.BangDingAnXiaToKongJian(this.textBox1, this.textBox2);
            anEnterJinRuXia.BangDingAnXiaToKongJian(this.textBox2, this.button1);
        }

        public void SetCanShu()
        {
            this.labFbiaoTi.Text = "切换用户";
        }
        protected override void GuanBi()
        {
            this.DialogResult = DialogResult.Cancel;
            base.GuanBi();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string dengluming = this.textBox1.Text;
            string mima = this.textBox2.Text;
            if (string.IsNullOrEmpty(dengluming))
            {
                this.QiDongTiShiKuang("登录名没有填写");
                return;
            }
            bool dengl = QuanXianLei.CerateDanLi().DengLu(dengluming, mima);
            if (dengl)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                this.QiDongTiShiKuang("登录名或者与密码不对");
            }
        }

        private void YongHuDengLuFrm_Load(object sender, EventArgs e)
        {
            this.textBox1.Focus();
            this.textBox1.Select();
            this.textBox1.Text = "admin";
            this.textBox2.Text = "admin123";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //List<int> jishu = new List<int>() { 1,2,3,4,5};
            //Parallel.ForEach(jishu, (x) =>{
            //    for (int i = 0; i < x*10; i++)
            //    {
            //        Thread.Sleep(100);
            //    }
            //});
        }
    }
}
