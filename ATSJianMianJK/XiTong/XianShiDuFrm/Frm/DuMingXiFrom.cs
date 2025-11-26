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
using JieMianLei.FuFrom;

namespace ATSJianMianJK.XiTong.XianShiDuFrm.Frm
{
    public partial class DuMingXiFrom : BaseFuFrom
    {
        public DuMingXiFrom()
        {
            InitializeComponent();
            this.IsZhiXianShiX = true;
        }
        public void SetCanShu(List<DuModel> model)
        {
            for (int i = 0; i < model.Count; i++)
            {
                int index = this.jiHeDataGrid1.Rows.Add();
                this.jiHeDataGrid1.Rows[index].Cells[0].Value = string.Format("{0}:{1}",model[i].SheBeiID, model[i].JiCunQiName);
                this.jiHeDataGrid1.Rows[index].Cells[1].Value = "";
                this.jiHeDataGrid1.Rows[index].Cells[2].Value = model[i].PiPeiValue;
                this.jiHeDataGrid1.Rows[index].Cells[3].Value = model[i].Value;
                this.jiHeDataGrid1.Rows[index].Cells[4].Value = "";
                this.jiHeDataGrid1.Rows[index].Height = 32;
            }
        }

        public void ShuaXin(List<DuModel> model)
        {
            for (int i = 0; i < model.Count; i++)
            {
                for (int c = 0; c < this.jiHeDataGrid1.Rows.Count; c++)
                {
                    string biaoshi = ChangYong.TryStr(this.jiHeDataGrid1.Rows[c].Cells[0].Value.ToString(), "");

                    if (biaoshi.Equals($"{model[i].SheBeiID}:{model[i].JiCunQiName}"))
                    {
                        bool zhen = model[i].LisPiPeiValue.IndexOf(ChangYong.TryStr(model[i].Value, "")) >= 0;
                        this.jiHeDataGrid1.Rows[c].Cells[3].Value = model[i].Value;
                        this.jiHeDataGrid1.Rows[c].Cells[4].Value = zhen;
                        if (zhen)
                        {
                            this.jiHeDataGrid1.Rows[c].DefaultCellStyle.ForeColor = Color.Black;
                        }
                        else
                        {
                            this.jiHeDataGrid1.Rows[c].DefaultCellStyle.ForeColor = Color.Red;
                        }
                        break;
                    }
                }
            }
        }
    }
}
