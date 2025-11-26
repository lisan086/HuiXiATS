using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSJuanChengZuZhuangUI.Model;
using ATSJuanChengZuZhuangUI.PeiZhi.Frm;
using CommLei.JiChuLei;
using JieMianLei.FuFrom;
using ZuZhuangUI.Lei;
using ZuZhuangUI.Model;

namespace ZuZhuangUI.PeiZhi.Frm
{
    public partial class MaGuanLiFrm : BaseFuFrom
    {
        public MaGuanLiFrm()
        {
            InitializeComponent();
            SetCanShu();
        }

        private void SetCanShu()
        {
            this.dataGridView1.Rows.Clear();
            List<MaGuanLiModel> lis= MaGuanLi.CerateDanLi().GetMaZiLiao();
            for (int i = 0; i < lis.Count; i++)
            {
                FuZhi(lis[i]);
            }
        }

        private MaGuanLiModel GetCanShu(DataGridViewRow row)
        {
            MaGuanLiModel model = new MaGuanLiModel();
            if (row.Tag is MaGuanLiModel)
            {
                model = row.Tag as MaGuanLiModel;
                model.MaName = ChangYong.TryStr(row.Cells[0].Value, "");
                model.Count = ChangYong.TryInt(row.Cells[2].Value, 5);
            }
         
            return model;
        }
        private void FuZhi(MaGuanLiModel model)
        {
            int index = this.dataGridView1.Rows.Add();
            DataGridViewRow row = this.dataGridView1.Rows[index];
            row.Cells[0].Value = model.MaName;
            row.Cells[1].Value = "标识";
            row.Cells[2].Value = model.Count;          
            row.Cells[3].Value = "保存";
            row.Tag = model;
            row.Height = 32;

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridView1.Rows.Count > 0)
            {
                if (e.RowIndex >= 0)
                {
                    if (e.ColumnIndex == 3)
                    {
                        MaGuanLiModel model = GetCanShu(this.dataGridView1.Rows[e.RowIndex]);
                        if (string.IsNullOrEmpty(model.MaName) == false)
                        {
                            MaGuanLi.CerateDanLi().ZengJiaBaoCun(model);
                            this.QiDongTiShiKuang("保存成功");
                            SetCanShu();
                        }
                        else
                        {
                            this.QiDongTiShiKuang("名称未填写");
                        }
                    }
                    else if (e.ColumnIndex==1)
                    {
                        MaGuanLiModel mosle = GetCanShu(this.dataGridView1.Rows[e.RowIndex]);
                        XuanZeFrm xuanZeFrm = new XuanZeFrm();
                        xuanZeFrm.SetCanShu(mosle.LisGuiZe);
                        if (xuanZeFrm.ShowDialog(this)==DialogResult.OK)
                        {
                            mosle.LisGuiZe = xuanZeFrm.GetShuJu();
                            this.dataGridView1.Rows[e.RowIndex].Tag = mosle;
                        }
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FuZhi(new MaGuanLiModel() {Count=5});
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<ShuChuMaModel> model = MaGuanLi.CerateDanLi().GetMaShuJu(1,false);
            if (model == null)
            {
                this.QiDongTiShiKuang("没有选择当前码");
            }
            else
            {
                this.QiDongTiShiKuang(ChangYong.HuoQuJsonStr(model),50);
            }
        }

        

      
    }
}
