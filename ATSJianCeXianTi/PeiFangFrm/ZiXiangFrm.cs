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
using CommLei.JiChuLei;
using JieMianLei.FuFrom;

namespace ATSJianCeXianTi.PeiFangFrm
{
    public partial class ZiXiangFrm : BaseFuFrom
    {
        private PeiFangLei PeiFangLei;
        public ZiXiangFrm()
        {
            InitializeComponent();
        }
        public void SetCanShu(List<TestModel> lis,PeiFangLei peiFangLei)
        {
            PeiFangLei = peiFangLei;
            for (int i = 0; i < lis.Count; i++)
            {
                FuZhi(lis[i],null);
            }
        }
        protected override void GuanBi()
        {
            this.DialogResult = DialogResult.OK;
            base.GuanBi();
        }
        public List<TestModel> GetModels()
        {
            List<TestModel> kais = new List<TestModel>();
            for (int i = 0; i < this.jiHeDataGrid1.Rows.Count; i++)
            {
                if (this.jiHeDataGrid1.Rows[i].Tag is TestModel)
                {
                    TestModel yemodel = this.jiHeDataGrid1.Rows[i].Tag as TestModel;
                    yemodel.ZiXiangShunXu = ChangYong.TryInt(this.jiHeDataGrid1.Rows[i].Cells[9].Value.ToString(), 1);
                    kais.Add(yemodel);
                }
            }
            return kais;
        }

        private void FuZhi(TestModel model,DataGridViewRow row)
        {
            if (row == null)
            {
                int index = this.jiHeDataGrid1.Rows.Add();
                row = this.jiHeDataGrid1.Rows[index];
            }
            row.Cells[0].Value = model.ItemName;
            row.Cells[1].Value = model.GongNengType.ToString();
            row.Cells[2].Value = string.Format("{0}:{1}", model.SheBeiID, model.SheBeiName);
            row.Cells[3].Value = model.CMDSend;
            row.Cells[4].Value = model.CMDCanShu;
            row.Cells[5].Value = model.LowStr;
            row.Cells[6].Value = model.UpStr;
            row.Cells[7].Value = model.DanWei;
            row.Cells[8].Value = model.BiJiaoType.ToString();
            row.Cells[9].Value = model.ZiXiangShunXu;          
            row.Tag = model;
            row.Height = 32;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.jiHeDataGrid1.SelectedCells.Count > 0)
            {
                int index= this.jiHeDataGrid1.SelectedCells[0].RowIndex;
                this.jiHeDataGrid1.Rows.RemoveAt(index);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FuZhi(new TestModel(),null);
        }

        private void jiHeDataGrid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 1)
                {
                    string gongnengma = this.jiHeDataGrid1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

                    XuanZseFrm fem = new XuanZseFrm();
                    fem.SetCanShu(PeiFangLei.JianCeDui.GetGongNeng(), gongnengma);
                    if (fem.ShowDialog(this) == DialogResult.OK)
                    {
                        this.jiHeDataGrid1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = fem.JieGuo;
                    }
                }
                else if (e.ColumnIndex == 2)
                {
                    string gongnengma = this.jiHeDataGrid1.Rows[e.RowIndex].Cells[1].Value.ToString();
                    if (string.IsNullOrEmpty(gongnengma))
                    {
                        this.QiDongTiShiKuang("没有选择功能");
                        return;
                    }
                    DataGridViewRow row = this.jiHeDataGrid1.Rows[e.RowIndex];
                    string shebei = row.Cells[e.ColumnIndex].Value.ToString();
                    ZhongJianModel model = new ZhongJianModel();
                    model.TestModel = ChangYong.FuZhiShiTi(row.Tag as TestModel);
                    XuanZeJiCunPeiZhiFrm fem = new XuanZeJiCunPeiZhiFrm(PeiFangLei);
                    fem.SetCanShu(model, "");
                    if (fem.ShowDialog(this) == DialogResult.OK)
                    {
                        model = ChangYong.FuZhiShiTi(fem.ZhongJianModel);
                        FuZhi(model.TestModel, row);
                    }
                }

            }
        }
    }
}
