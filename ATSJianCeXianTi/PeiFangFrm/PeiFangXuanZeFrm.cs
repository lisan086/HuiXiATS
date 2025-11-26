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
using CommLei.JieMianLei;
using JieMianLei.FuFrom;

namespace ATSJianCeXianTi.PeiFangFrm
{
    public partial class PeiFangXuanZeFrm : BaseFuFrom
    {
        private PFPeiZhiFrm PFPeiZhiFrm;
        public string PeiFangNames { get; set; } = "";
        public PeiFangXuanZeFrm()
        {
            InitializeComponent();
            IniData();
            this.ZuiDaHua();
        }
        private void IniData()
        {
         
            {                           
                jiHeDataGrid1.Dock = DockStyle.Fill;
              
            }
            {
                PFPeiZhiFrm = new PFPeiZhiFrm();
                PFPeiZhiFrm.QuXiaoBiaoTi();
                PFPeiZhiFrm.TopLevel = false;
                PFPeiZhiFrm.FormBorderStyle = FormBorderStyle.None;
                PFPeiZhiFrm.Dock = DockStyle.Fill;
                PFPeiZhiFrm.Parent = this.panel2;
                PFPeiZhiFrm.Hide();
            }
        }
        public  void SetCanShu(string danqianpefang)
        {
            PeiFangNames = danqianpefang;
            this.jiHeDataGrid1.Rows.Clear();
            PeiFangLei peiFangLei = new PeiFangLei();
            List<string> peifang = peiFangLei.GetPeiFangNames();
            for (int i = 0; i < peifang.Count; i++)
            {
                int index = this.jiHeDataGrid1.Rows.Add();
                this.jiHeDataGrid1.Rows[index].Cells[0].Value = peifang[i];
                this.jiHeDataGrid1.Rows[index].Cells[1].Value ="编辑";
                this.jiHeDataGrid1.Rows[index].Cells[2].Value ="选择";
                if (peifang[i].Equals(danqianpefang))
                {
                    this.jiHeDataGrid1.Rows[index].DefaultCellStyle.ForeColor = Color.Red;
                }
                else
                {
                    this.jiHeDataGrid1.Rows[index].DefaultCellStyle.ForeColor = Color.Black;
                }
                this.jiHeDataGrid1.Rows[index].Height = 32;
            }
        }

        private void jiHeDataGrid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex>=0)
            {
                if (e.ColumnIndex == 2)
                {
                    PeiFangNames = this.jiHeDataGrid1.Rows[e.RowIndex].Cells[0].Value.ToString();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                if (e.ColumnIndex == 1)
                {
                    this.button1.Text = "返回";
                    this.jiHeDataGrid1.Visible = false;
                    string peifangming = this.jiHeDataGrid1.Rows[e.RowIndex].Cells[0].Value.ToString();
                    PFPeiZhiFrm.Visible = false;
                    PFPeiZhiFrm.SetCanShu(peifangming);
                    PFPeiZhiFrm.Show();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.button1.Text.Contains("增"))
            {
                int index = this.jiHeDataGrid1.Rows.Add();
                this.jiHeDataGrid1.Rows[index].Cells[0].Value = "";
                this.jiHeDataGrid1.Rows[index].Cells[1].Value = "编辑";
                this.jiHeDataGrid1.Rows[index].Cells[2].Value = "选择";
            }
            else
            {
                this.button1.Text = "增加";
                this.jiHeDataGrid1.Visible = true;
                PFPeiZhiFrm.Visible = false;
                SetCanShu(PeiFangNames);
            }
           
        }

      
    }
}
