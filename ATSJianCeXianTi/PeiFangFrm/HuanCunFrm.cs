using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSJianCeXianTi.Lei;
using ATSJianCeXianTi.Model;
using ATSJianMianJK.GongNengLei;
using ATSJianMianJK.XiTong.Model;
using CommLei.DataChuLi;
using CommLei.JiChuLei;
using JieMianLei.FuFrom;

namespace ATSJianCeXianTi.PeiFangFrm
{
    public partial class HuanCunFrm : BaseFuFrom
    {
        public CaoZuoHuanCunModel CaoZuoHuanCunModel { get; set; }
        private PeiFangLei PeiFangLei;
       
        public HuanCunFrm()
        {
            InitializeComponent();
            this.IsZhiXianShiX = true;
        }

        public void SetCanShu(CaoZuoHuanCunModel caoZuoHuanCun, PeiFangLei peiFang)
        {
          
            PeiFangLei = peiFang;
            List<string> meijus = ChangYong.MeiJuLisName(typeof(HuanCunCaoZuoType));
            for (int i = 0; i < meijus.Count; i++)
            {
                this.comboBox1.Items.Add(meijus[i]);
            }
            List < HuanCunModel > lismodel = HCLisDataLei<HuanCunModel>.Ceratei().LisWuLiao;
            for (int i = 0; i < lismodel.Count; i++)
            {
                this.comboBox2.Items.Add(lismodel[i].HuanCunName);
            }
            this.comboBox1.Text = caoZuoHuanCun.HuanCunCaoZuoType.ToString();
            this.textBox4.Text = caoZuoHuanCun.BiaoHao.ToString();
            this.comboBox2.Text = caoZuoHuanCun.HuanCunName;
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CaoZuoHuanCunModel = new CaoZuoHuanCunModel();
            CaoZuoHuanCunModel.HuanCunName= this.comboBox2.Text;
            CaoZuoHuanCunModel.HuanCunCaoZuoType = ChangYong.GetMeiJuZhi<HuanCunCaoZuoType>(this.comboBox1.Text);
            CaoZuoHuanCunModel.BiaoHao =ChangYong.TryInt( this.textBox4.Text,0);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }


    }
}
