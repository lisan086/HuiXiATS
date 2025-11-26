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
using ZuZhuangUI.Model;

namespace ZuZhuangUI.PeiZhi.KJ
{
    public partial class DataKJ : UserControl
    {
        private YeWuDataModel YeWuDataModel; 
        public DataKJ()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        public void SetCanShu(YeWuDataModel wuDataModel)
        {
            this.commBoxE1.Items.Clear();
            this.commBoxE2.Items.Clear();
            {
                //1-PLC状态为准 2-plc和上位机判断为准 3是以上位机判断为准
                this.commBoxE2.Items.Add("1:PLC状态为准");
                this.commBoxE2.Items.Add("2:PLC和上位机判断为准");
                this.commBoxE2.Items.Add("3:上位机判断为准");
                this.commBoxE2.Items.Add("4:相机对比");
            }
            List<string> lisd = ChangYong.MeiJuLisName(typeof(CaoZuoType));
            for (int i = 0; i < lisd.Count; i++)
            {
                if (lisd[i].Contains("DataShangChuan"))
                {
                    this.commBoxE1.Items.Add(lisd[i]);
                }
            }
            if (this.commBoxE1.Items.Count>0)
            {
                this.commBoxE1.SelectedIndex = 0;
            }
            YeWuDataModel = ChangYong.FuZhiShiTi(wuDataModel);
            this.textBox2.Text = wuDataModel.ItemName;
            this.textBox14.Text = wuDataModel.PaiXu.ToString();
            this.textBox12.Text = wuDataModel.CodeOrNo.ToString();
            this.xuanZeDuXieKJ1.SetCanShu(wuDataModel.Up);
            this.xuanZeDuXieKJ2.SetCanShu(wuDataModel.Low);
            this.xuanZeDuXieKJ3.SetCanShu(wuDataModel.Value);
            this.xuanZeDuXieKJ4.SetCanShu(wuDataModel.State);
            this.checkBox2.Checked = wuDataModel.IsShangChuan==1;
            this.commBoxE1.Text= wuDataModel.CaoZuoType.ToString();
            this.textBox1.Text = wuDataModel.DanWei;
            this.textBox3.Text = wuDataModel.ZhuanTaiPiPeiZhi;
            this.commBoxE3.Text = wuDataModel.YongDeMaMingCheng;
            for (int i = 0; i < this.commBoxE2.Items.Count; i++)
            {
                if (this.commBoxE2.Items[i].ToString().Split(':')[0]==wuDataModel.IsYiZhuangTaiWeiZhun.ToString())
                {
                    this.commBoxE2.SelectedIndex = i;
                    break;
                }
            }
        }

        public YeWuDataModel GetCanShu()
        {
            YeWuDataModel model = ChangYong.FuZhiShiTi(YeWuDataModel);
            model.CaoZuoType = this.commBoxE1.GetCanShu<CaoZuoType>();
            model.CodeOrNo = this.textBox12.Text;
            model.IsShangChuan = this.checkBox2.Checked ? 1 : 0;
            model.IsYiZhuangTaiWeiZhun = ChangYong.TryInt(this.commBoxE2.Text.Split(':')[0],2);
            model.ItemName = this.textBox2.Text;
            model.Low = this.xuanZeDuXieKJ2.GetCanShu();
            model.ZhuanTaiPiPeiZhi = this.textBox3.Text;
            model.PaiXu= ChangYong.TryInt(this.textBox14.Text, 0);
            model.State = this.xuanZeDuXieKJ4.GetCanShu();       
            model.Up = this.xuanZeDuXieKJ1.GetCanShu();
            model.DanWei = this.textBox1.Text;
            model.Value = this.xuanZeDuXieKJ3.GetCanShu();
           // model.Value.JiCunValue = "";
            model.YongDeMaMingCheng = this.commBoxE3.Text;
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
