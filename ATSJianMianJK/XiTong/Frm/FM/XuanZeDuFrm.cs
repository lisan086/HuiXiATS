using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSJianMianJK.XiTong.Model;
using CommLei.DataChuLi;
using JieMianLei.FuFrom;

namespace ATSJianMianJK.XiTong.Frm.FM
{
    public partial class XuanZeDuFrm : BaseFuFrom
    {
        public string MingZi { get; set; } = "";
        public XuanZeDuFrm()
        {
            InitializeComponent();
        }

        public void SetCanShu(string dukuainame,int morendu=1)
        {
            if (morendu == 1)
            {
                List<DuIOCanShuModel> lismodel = HCLisDataLei<DuIOCanShuModel>.Ceratei().LisWuLiao;

                List<RadioButton> kjs = new List<RadioButton>();
                for (int i = 0; i < lismodel.Count; i++)
                {
                    kjs.Add(ShengChengKJ(lismodel[i].Name, lismodel[i].Name.Equals(dukuainame)));
                }
                this.flowLayoutPanel1.Controls.AddRange(kjs.ToArray());
            }
            else if(morendu == 2)
            {
                List<XieSateModel> lismodel = HCLisDataLei<XieSateModel>.Ceratei().LisWuLiao;

                List<RadioButton> kjs = new List<RadioButton>();
                for (int i = 0; i < lismodel.Count; i++)
                {
                    kjs.Add(ShengChengKJ(lismodel[i].Name, lismodel[i].Name.Equals(dukuainame)));
                }
                this.flowLayoutPanel1.Controls.AddRange(kjs.ToArray());
            }
            else if (morendu == 3)
            {
                List<DuShuJuModel> lismodel = HCLisDataLei<DuShuJuModel>.Ceratei().LisWuLiao;
                List<RadioButton> kjs = new List<RadioButton>();
                for (int i = 0; i < lismodel.Count; i++)
                {
                    kjs.Add(ShengChengKJ(lismodel[i].Name, lismodel[i].Name.Equals(dukuainame)));
                }
                this.flowLayoutPanel1.Controls.AddRange(kjs.ToArray());
            }
            else if (morendu == 4)
            {
                List<HuanCunModel> lismodel = HCLisDataLei<HuanCunModel>.Ceratei().LisWuLiao;

                List<RadioButton> kjs = new List<RadioButton>();
                for (int i = 0; i < lismodel.Count; i++)
                {
                    kjs.Add(ShengChengKJ(lismodel[i].HuanCunName, lismodel[i].HuanCunName.Equals(dukuainame)));
                }
                this.flowLayoutPanel1.Controls.AddRange(kjs.ToArray());
            }
        }

     
        private RadioButton ShengChengKJ(string mingzi, bool isxuanze)
        {
            RadioButton radioButton = new RadioButton();
            radioButton.AutoSize = true;
            radioButton.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

            radioButton.Text = mingzi;
            radioButton.Checked = isxuanze;
            return radioButton;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MingZi = "";

            foreach (var item in this.flowLayoutPanel1.Controls)
            {
                if (item is RadioButton)
                {
                    RadioButton rm = item as RadioButton;
                    if (rm.Checked)
                    {
                        MingZi = rm.Text;
                        break;
                    }
                }
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
