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
    public partial class QieHuanPeiFangFrm : BaseFuFrom
    {
        public List<QieHuanPeiFangModel> MaS { get; set; } = new List<QieHuanPeiFangModel>();
        public QieHuanPeiFangFrm()
        {
            InitializeComponent();
            this.IsZhiXianShiX = true;
        }
        public void SetCanShu(List<QieHuanPeiFangModel> mas)
        {

            for (int i = 0; i < mas.Count; i++)
            {
                FuZhi(mas[i]);
            }
        }

        protected override void GuanBi()
        {
            MaS.Clear();
            for (int i = 0; i < this.jiHeDataGrid1.Rows.Count; i++)
            {
                QieHuanPeiFangModel model = new QieHuanPeiFangModel();
                model.IsQieHuan = ChangYong.TryStr(this.jiHeDataGrid1.Rows[i].Cells[1].Value, "").Contains("u")?1:0;
                model.Name = ChangYong.TryStr(this.jiHeDataGrid1.Rows[i].Cells[0].Value, "");
              
                if (string.IsNullOrEmpty(model.Name) == false )
                {
                    MaS.Add(model);
                }
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FuZhi(new QieHuanPeiFangModel());
        }

        private void FuZhi(QieHuanPeiFangModel tiaoma)
        {
            int index = this.jiHeDataGrid1.Rows.Add();
            this.jiHeDataGrid1.Rows[index].Cells[0].Value = tiaoma.Name;
            this.jiHeDataGrid1.Rows[index].Cells[1].Value = tiaoma.IsQieHuan==1;
            this.jiHeDataGrid1.Rows[index].Cells[2].Value = "删除";
            this.jiHeDataGrid1.Rows[index].Height = 32;
        }

        private void jiHeDataGrid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.jiHeDataGrid1.Rows.Count > 0)
            {
                if (e.RowIndex >= 0)
                {
                    if (e.ColumnIndex == 2)
                    {
                        this.jiHeDataGrid1.Rows.RemoveAt(e.RowIndex);
                    }
                }
            }
        }
    }
}
