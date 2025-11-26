using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YiBanSaoMaQi.Model;

namespace YiBanSaoMaQi.Frm
{
    public partial class TiaoShiKJ : UserControl
    {
        private IPFuWuPeiModel SaoMaModel;
        private PeiZhiLei PeiZhiLei;
        public TiaoShiKJ()
        {
            InitializeComponent();
        }
        public void SetCanShu(IPFuWuPeiModel maModel,List<CunModel> shuju,PeiZhiLei peiZhiLei)
        {
            SaoMaModel=maModel;
            PeiZhiLei=peiZhiLei;
            this.label4.Text=maModel.ZhanDianName;
            this.label4.BackColor = maModel.Tx ? Color.Green : Color.Red;
            for (int i = 0; i < shuju.Count; i++)
            {
                FuZhi(shuju[i]);
            }
           
        }

        public void ShuaXin()
        {
            if (SaoMaModel.Tx)
            {
                if (this.label4.BackColor != Color.Green)
                {
                    this.label4.BackColor = Color.Green;
                }
            }
            else
            {
                if (this.label4.BackColor != Color.Red)
                {
                    this.label4.BackColor = Color.Red;
                }
            }

            for (int i = 0; i < this.dataGridView2.Rows.Count; i++)
            {
                if (this.dataGridView2.Rows[i].Tag is CunModel)
                {
                    CunModel model = this.dataGridView2.Rows[i].Tag as CunModel;
                    if (model.IsZhengZaiCe == 0)
                    {
                        this.dataGridView2.Rows[i].Cells[3].Value = "执行中";
                    }
                    else
                    {
                        this.dataGridView2.Rows[i].Cells[3].Value = model.JiCunQi.Value;
                    }
                }
            }
        }

        public void FuZhi(CunModel model)
        {
            int index = this.dataGridView2.Rows.Add();
            this.dataGridView2.Rows[index].Cells[0].Value = model.JiCunQi.WeiYiBiaoShi;
            this.dataGridView2.Rows[index].Cells[1].Value = model.JiCunQi.MiaoSu;
            this.dataGridView2.Rows[index].Cells[2].Value = "";
            this.dataGridView2.Rows[index].Cells[3].Value = "";
            this.dataGridView2.Rows[index].Cells[4].Value = "执行";
            this.dataGridView2.Rows[index].Tag = model;
            this.dataGridView2.Rows[index].Height = 32;
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 4)
                {
                    if (this.dataGridView2.Rows[e.RowIndex].Tag is CunModel)
                    {
                        CunModel model = this.dataGridView2.Rows[e.RowIndex].Tag as CunModel;
                        CunModel xinmodel = model.FuZhi();
                        xinmodel.JiCunQi.Value = this.dataGridView2.Rows[e.RowIndex].Cells[2].Value;
                        PeiZhiLei.XieJiDianQi(xinmodel.JiCunQi);
                    }
                }
            }
        }
    }
}
