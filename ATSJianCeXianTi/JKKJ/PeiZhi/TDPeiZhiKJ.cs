using ATSJianCeXianTi.Model;
using ATSJianCeXianTi.PeiFangFrm;
using CommLei.JiChuLei;
using SSheBei.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATSJianCeXianTi.JKKJ.PeiZhiKJ
{
    public partial class TDPeiZhiKJ : UserControl
    {
        private TDModel TDPeiZhiModel = new TDModel();
        public TDPeiZhiKJ()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }
        public void SetCanShu(TDModel model)
        {
            this.commBoxE2.Items.Clear();
            List<string> lisshebeizu = ChangYong.MeiJuLisName(typeof(SheBeiZuType));
            for (int i = 0; i <lisshebeizu.Count; i++)
            {
                this.commBoxE2.Items.Add(lisshebeizu[i]);
            }
            TDPeiZhiModel = ChangYong.FuZhiShiTi(model);
            this.textBox1.Text = model.TDID.ToString();
            this.textBox7.Text = model.TDName.ToString();
            this.commBoxE2.Text = model.PeiZhiCanShu.SheBeiZu;
            this.checkBox3.Checked = model.PeiZhiCanShu.IsSaoMa;
            this.xuanZeDuXieKJ1.SetCanShu ( model.PeiZhiCanShu.QiDongName);
            this.xuanZeDuXieKJ2.SetCanShu(model.PeiZhiCanShu.SaoMaName);
            this.xuanZeDuXieKJ3.SetCanShu(model.PeiZhiCanShu.DuSaoMaZhi);
            this.xuanZeDuXieKJ4.SetCanShu(model.PeiZhiCanShu.JiTingName);
            this.xuanZeDuXieKJ5.SetCanShu(model.PeiZhiCanShu.TingZhiName);
            this.xuanZeDuXieKJ6.SetCanShu(model.PeiZhiCanShu.WuPingStateName);
            this.xuanZeDuXieKJ7.SetCanShu(model.PeiZhiCanShu.QieHuanXingHaoName);
            this.checkBox1.Checked = model.PeiZhiCanShu.IsXianShiZheDang;
            this.checkBox2.Checked = model.PeiZhiCanShu.IsZiDongQiePeiFang==1;
            this.checkBox4.Checked = model.PeiZhiCanShu.QieHuanType == 1;
            this.checkBox5.Checked = model.PeiZhiCanShu.BuChuanGuoZhan != 1;
            this.xuanZeDuXieKJ8.SetCanShu(model.PeiZhiCanShu.MesDuState);
            this.xuanZeDuXieKJ9.SetCanShu(model.PeiZhiCanShu.MesXieState);
            this.xuanZeDuXieKJ10.SetCanShu(model.PeiZhiCanShu.DuDianJianMoShi);
        }

        public TDModel GetCanShu()
        {
            TDModel model = new TDModel();
            model.PeiZhiCanShu = ChangYong.FuZhiShiTi(TDPeiZhiModel.PeiZhiCanShu);         
            model.TDID = ChangYong.TryInt(this.textBox1.Text,-1);
            model.TDName = this.textBox7.Text;
            model.PeiZhiCanShu.SheBeiZu = this.commBoxE2.Text;
            model.PeiZhiCanShu.IsSaoMa= this.checkBox3.Checked;
            model.PeiZhiCanShu.SaoMaName = this.xuanZeDuXieKJ2.GetCanShu();
            model.PeiZhiCanShu.QiDongName= this.xuanZeDuXieKJ1.GetCanShu();
            model.PeiZhiCanShu.JiTingName = this.xuanZeDuXieKJ4.GetCanShu();
            model.PeiZhiCanShu.TingZhiName = this.xuanZeDuXieKJ5.GetCanShu();
            model.PeiZhiCanShu.DuSaoMaZhi = this.xuanZeDuXieKJ3.GetCanShu();
            model.PeiZhiCanShu.WuPingStateName= this.xuanZeDuXieKJ6.GetCanShu();
            model.PeiZhiCanShu.QieHuanXingHaoName = this.xuanZeDuXieKJ7.GetCanShu();
            model.PeiZhiCanShu.IsXianShiZheDang=this.checkBox1.Checked;
            model.PeiZhiCanShu.IsZiDongQiePeiFang = this.checkBox2.Checked ? 1 : 0;
            model.PeiZhiCanShu.QieHuanType = this.checkBox4.Checked ? 1 : 2;
            model.PeiZhiCanShu.BuChuanGuoZhan = checkBox5.Checked ? 2 : 1;
            model.PeiZhiCanShu.MesDuState = this.xuanZeDuXieKJ8.GetCanShu();
            model.PeiZhiCanShu.MesXieState = this.xuanZeDuXieKJ9.GetCanShu();
            model.PeiZhiCanShu.DuDianJianMoShi= this.xuanZeDuXieKJ10.GetCanShu();
            return model;
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
