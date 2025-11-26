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
using JieMianLei.FuFrom;

namespace ATSJianCeXianTi.PeiFangFrm
{
    public partial class DuiYingMaFrm : BaseFuFrom
    {
        public List<string> MaS { get; set; } = new List<string>();
        public DuiYingMaFrm()
        {
            InitializeComponent();
            this.IsZhiXianShiX = true;
        }
        public void SetCanShu(List<string> mas)
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
                string ma =ChangYong.TryStr( this.jiHeDataGrid1.Rows[i].Cells[0].Value,"");
                if (string.IsNullOrEmpty(ma)==false&& MaS.IndexOf(ma)<0)
                {
                    MaS.Add(ma);
                }
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FuZhi("");
        }

        private void FuZhi(string tiaoma)
        {
            int index = this.jiHeDataGrid1.Rows.Add();
            this.jiHeDataGrid1.Rows[index].Cells[0].Value = tiaoma;
            this.jiHeDataGrid1.Rows[index].Cells[1].Value = "删除";
            this.jiHeDataGrid1.Rows[index].Height = 32;
        }

        private void jiHeDataGrid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.jiHeDataGrid1.Rows.Count > 0)
            {
                if (e.RowIndex >= 0)
                {
                    if (e.ColumnIndex == 1)
                    {
                        this.jiHeDataGrid1.Rows.RemoveAt(e.RowIndex);
                    }
                }
            }
        }
    }
}
