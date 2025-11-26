using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            model.MaName= ChangYong.TryStr(row.Cells[0].Value, "");
            model.ShouBuBianShi1 = ChangYong.TryStr(row.Cells[1].Value,"");
            model.ShouBuBianShi2 = ChangYong.TryStr(row.Cells[2].Value, "");
            model.ShouBuBianShi3 = ChangYong.TryStr(row.Cells[3].Value, "");
            model.ShouBuBianShi4 = ChangYong.TryStr(row.Cells[4].Value, "");
            model.MuQianWeiZhi = ChangYong.TryInt(row.Cells[6].Value,1);
            model.MouChangDu = ChangYong.TryInt(row.Cells[5].Value, 6);
            model.YiGongYong = ChangYong.TryInt(row.Cells[7].Value, 4);
            model.DanQianShiYong= ChangYong.TryStr(row.Cells[8].Value, "").ToLower().Contains("u");
            return model;
        }
        private void FuZhi(MaGuanLiModel model)
        {
            int index = this.dataGridView1.Rows.Add();
            DataGridViewRow row = this.dataGridView1.Rows[index];
            row.Cells[0].Value = model.MaName;
            row.Cells[1].Value = model.ShouBuBianShi1;
            row.Cells[2].Value = model.ShouBuBianShi2;
            row.Cells[3].Value = model.ShouBuBianShi3;
            row.Cells[4].Value = model.ShouBuBianShi4;
            row.Cells[5].Value = model.MouChangDu;
            row.Cells[6].Value = model.MuQianWeiZhi;
            row.Cells[7].Value = model.YiGongYong;
            row.Cells[8].Value = model.DanQianShiYong;
            row.Cells[9].Value = "保存";
            row.Height = 32;

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridView1.Rows.Count > 0)
            {
                if (e.RowIndex >= 0)
                {
                    if (e.ColumnIndex == 9)
                    {
                        MaGuanLiModel model = GetCanShu(this.dataGridView1.Rows[e.RowIndex]);
                        if (string.IsNullOrEmpty(model.MaName) == false)
                        {
                            MaGuanLi.CerateDanLi().SetDuiXiang(model);
                            this.QiDongTiShiKuang("保存成功");
                            SetCanShu();
                        }
                        else
                        {
                            this.QiDongTiShiKuang("名称未填写");
                        }
                    }
                    else if (e.ColumnIndex>0&&e.ColumnIndex<=4)
                    {
                        XuanZeFrm xuanZeFrm = new XuanZeFrm();
                        xuanZeFrm.SetCanShu(ChangYong.TryStr(this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value, ""));
                        if (xuanZeFrm.ShowDialog(this)==DialogResult.OK)
                        {
                            this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = xuanZeFrm.JieGuo;
                        }
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FuZhi(new MaGuanLiModel() { YiGongYong=4, ShouBuBianShi1="BC", MuQianWeiZhi=1, MouChangDu=6});
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ShuChuMaShuJuModel model = MaGuanLi.CerateDanLi().GetMaShuJu(false);
            if (model == null)
            {
                this.QiDongTiShiKuang("没有选择当前码");
            }
            else
            {
                this.QiDongTiShiKuang(model.GetLog());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            JieMianCaoZuoModel model = new JieMianCaoZuoModel();
            model.CanShu = "";
            model.GWID = -99;
            JieMianCaoZuoLei.CerateDanLi().JieMianCuoZuo(DoType.ShuChuMa, model);
        }
    }
}
