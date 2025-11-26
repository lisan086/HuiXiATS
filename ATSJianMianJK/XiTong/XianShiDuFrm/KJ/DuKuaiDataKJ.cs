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
using CommLei.JiChuLei;

namespace ATSJianMianJK.XiTong.XianShiDuFrm.KJ
{
    public partial class DuKuaiDataKJ : UserControl
    {
        public DuKuaiDataKJ()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }
        public void SetCanShu(DuShuJuModel model)
        {
            this.label3.Text = model.Name.ToString();
            for (int i = 0; i < model.LisJiCunQi.Count; i++)
            {
                int index = this.jiHeDataGrid1.Rows.Add();
                this.jiHeDataGrid1.Rows[index].Cells[0].Value = string.Format("{0}:{1}",model.LisJiCunQi[i].SheBeiID, model.LisJiCunQi[i].JiCunQiName);
                this.jiHeDataGrid1.Rows[index].Cells[1].Value = string.Format("{0}", model.LisJiCunQi[i].Type);
                this.jiHeDataGrid1.Rows[index].Cells[2].Value = "";
                this.jiHeDataGrid1.Rows[index].Height = 32;
            }
        }

        public void ShuXinData(DuShuJuModel shujus)
        {
            if (shujus.Name == this.label3.Text)
            {
                List<DuModel> models = shujus.LisJiCunQi;
                for (int i = 0; i < models.Count; i++)
                {
                    for (int c = 0; c < this.jiHeDataGrid1.Rows.Count; c++)
                    {
                        string biaoshi = ChangYong.TryStr(this.jiHeDataGrid1.Rows[c].Cells[0].Value.ToString(), "");
                        string type = ChangYong.TryStr(this.jiHeDataGrid1.Rows[c].Cells[1].Value.ToString(), "");

                        if (biaoshi.Equals($"{models[i].SheBeiID}:{models[i].JiCunQiName}")&& type.Equals(models[i].Type))
                        {
                            this.jiHeDataGrid1.Rows[c].Cells[2].Value = models[i].Value;
                            break;
                        }
                    }
                }


              
            }

        }
    }
}
