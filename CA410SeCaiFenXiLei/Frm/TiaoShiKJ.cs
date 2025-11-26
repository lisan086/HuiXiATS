using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YiBanSaoMaQi.Frm;
using YiBanSaoMaQi.Model;

namespace YiBanDuXiePeiZhi.Frm
{
    public partial class TiaoShiKJ : UserControl
    {
        private SaoMaModel SaoMaModel; 
        private PeiZhiLei PeiZhiLei;
        public TiaoShiKJ()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        public void SetCanShu(SaoMaModel maModel,PeiZhiLei peiZhiLei)
        {
            PeiZhiLei=peiZhiLei;
            this.label1.Text = maModel.Name;
            this.dataGridView1.Rows.Clear();
            this.dataGridView2.Rows.Clear();
            SaoMaModel = maModel;
            Dictionary<string, CunModel> dumodels = peiZhiLei.DataMoXing.JiLu;
            foreach (var item in dumodels.Keys)
            {
                CunModel msdodel = dumodels[item];
                if (msdodel.IsDu.ToString().ToLower().StartsWith("du"))
                {
                    FuZhi(msdodel, true);
                }
                else
                {
                    FuZhi(msdodel, false);
                }
            }
          
        }

        public void ShuaXin()
        {
            if (SaoMaModel.TX)
            {
                if (this.label1.BackColor != Color.Green)
                {
                    this.label1.BackColor = Color.Green;
                }
            }
            else
            {
                if (this.label1.BackColor != Color.Red)
                {
                    this.label1.BackColor = Color.Red;
                }
            }
            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            {
                if (this.dataGridView1.Rows[i].Tag is CunModel )
                {
                    CunModel model = this.dataGridView1.Rows[i].Tag as CunModel;
                    this.dataGridView1.Rows[i].Cells[2].Value = model.JiCunQi.Value;
                }
            }
            for (int i = 0; i < this.dataGridView2.Rows.Count; i++)
            {
                if (this.dataGridView2.Rows[i].Tag is CunModel)
                {
                    CunModel model = this.dataGridView2.Rows[i].Tag as CunModel;
                    if (model.IsZhengZaiCe == 0)
                    {
                        this.dataGridView2.Rows[i].Cells[3].Value ="执行中";
                    }
                    else
                    {
                        this.dataGridView2.Rows[i].Cells[3].Value = model.JiCunQi.Value;
                    }
                }
            }
        }

        private void FuZhi(CunModel model,bool isdu)
        {
            if (isdu)
            {
                int index = this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[index].Cells[0].Value = model.JiCunQi.WeiYiBiaoShi;
                this.dataGridView1.Rows[index].Cells[1].Value = model.IsDu.ToString();              
                this.dataGridView1.Rows[index].Cells[2].Value = "";
                this.dataGridView1.Rows[index].Tag = model;
                this.dataGridView1.Rows[index].Height = 32;
            }
            else
            {
                int index = this.dataGridView2.Rows.Add();
                this.dataGridView2.Rows[index].Cells[0].Value = model.JiCunQi.WeiYiBiaoShi;
                this.dataGridView2.Rows[index].Cells[1].Value = model.IsDu.ToString();
             
                this.dataGridView2.Rows[index].Cells[2].Value = "";
                this.dataGridView2.Rows[index].Cells[3].Value = "";
                this.dataGridView2.Rows[index].Cells[4].Value = "执行";
                this.dataGridView2.Rows[index].Tag = model;
                this.dataGridView2.Rows[index].Height = 32;
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex>=0)
            {
                if (e.ColumnIndex==4)
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
