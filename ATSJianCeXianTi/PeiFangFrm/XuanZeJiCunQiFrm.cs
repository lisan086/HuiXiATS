using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommLei.JiChuLei;
using JieMianLei.FuFrom;
using SSheBei.Model;

namespace ATSJianCeXianTi.PeiFangFrm
{
    public partial class XuanZeJiCunQiFrm : BaseFuFrom
    {
        public string WeiYiBiaoShi { get; set; } = "";
        public string MiaoSu { get; set; } = "";
        public bool IsDu { get; set; } = false;
        public XuanZeJiCunQiFrm()
        {
            InitializeComponent();
        }

        public void SetCanShu(List<JiCunQiModel> xiejicunqis,string xuanzejicunqi,bool isduxie)
        {
            for (int i = 0; i < xiejicunqis.Count; i++)
            {
                int index = this.jiHeDataGrid1.Rows.Add();
                this.jiHeDataGrid1.Rows[index].Cells[0].Value = xiejicunqis[i].WeiYiBiaoShi;
                this.jiHeDataGrid1.Rows[index].Cells[1].Value = isduxie ? "读" : "写";
                this.jiHeDataGrid1.Rows[index].Cells[2].Value = xiejicunqis[i].MiaoSu;
                this.jiHeDataGrid1.Rows[index].Cells[3].Value = "选择";
                //  if (isduxie)
                {
                    if (xiejicunqis[i].WeiYiBiaoShi.Equals(xuanzejicunqi))
                    {
                        this.jiHeDataGrid1.Rows[index].DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
                this.jiHeDataGrid1.Rows[index].Height = 32;
            }
    
        }

        private void jiHeDataGrid1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex>=0)
            {
                if (e.ColumnIndex==3)
                {
                    WeiYiBiaoShi = this.jiHeDataGrid1.Rows[e.RowIndex].Cells[0].Value.ToString();
                    IsDu= this.jiHeDataGrid1.Rows[e.RowIndex].Cells[1].Value.ToString().Contains("读");
                    MiaoSu=ChangYong.TryStr( this.jiHeDataGrid1.Rows[e.RowIndex].Cells[2].Value,"");
                    this.DialogResult=DialogResult.OK;
                    this.Close();
                }
            }
        }
    }
}
