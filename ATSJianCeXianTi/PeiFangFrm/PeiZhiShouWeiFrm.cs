using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSJianCeXianTi.Model;
using CommLei.JiChuLei;
using JieMianLei.FuFrom;

namespace ATSJianCeXianTi.PeiFangFrm
{
    public partial class PeiZhiShouWeiFrm : BaseFuFrom
    {
        public PeiZhiShouWeiFrm()
        {
            InitializeComponent();
            this.IsZhiXianShiX = true;
        }
        protected override void GuanBi()
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        public void SetCanShu(List<PeiFangShouWeiModel> lis)
        {
            List<string> lisname = ChangYong.MeiJuLisName(typeof(ShouWeiType));
            for (int i = 0; i < lisname.Count; i++)
            {
                string zhilingname = "";
                foreach (var item in lis)
                {
                    if (lisname[i]==item.ShouWeiType.ToString())
                    {
                        zhilingname = item.ZhiLingName;
                        break;
                    }
                }
                int index=this.jiHeDataGrid1.Rows.Add();
                this.jiHeDataGrid1.Rows[index].Cells[0].Value = lisname[i];
                this.jiHeDataGrid1.Rows[index].Cells[1].Value = zhilingname;
            }
        }

        public List<PeiFangShouWeiModel> GetCanShu()
        {
            List<PeiFangShouWeiModel> lis = new List<PeiFangShouWeiModel>();
            for (int i = 0; i < this.jiHeDataGrid1.Rows.Count; i++)
            {
                PeiFangShouWeiModel model = new PeiFangShouWeiModel();
                model.ShouWeiType = ChangYong.GetMeiJuZhi<ShouWeiType>(this.jiHeDataGrid1.Rows[i].Cells[0].Value.ToString());
                model.ZhiLingName = ChangYong.TryStr(this.jiHeDataGrid1.Rows[i].Cells[1].Value,"");
                lis.Add(model);
            }
            return lis;
        }
    }
}
