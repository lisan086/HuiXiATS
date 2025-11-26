using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ModBuTCP.Model;
using SSheBei.Model;

namespace ModBuTCP.Frm
{
    public partial class SheBeiTiaoShiKJ : UserControl
    {
        private PeiZhiLei PeiZhiLei;
        private SheBeiModel SheBeiModel;
        public SheBeiTiaoShiKJ()
        {
            InitializeComponent();
        }

        public void SetCanShu(SheBeiModel shebeimodel, PeiZhiLei peiZhi)
        {
            PeiZhiLei = peiZhi;
            SheBeiModel = shebeimodel;
            for (int i = 0; i < SheBeiModel.DataCunModels.Count; i++)
            {
                DataCunModel cunModel= SheBeiModel.DataCunModels[i];
                ZengJiaShuJu(cunModel, 1);
                ZengJiaShuJu(cunModel, 2);
               
               
            }
        }


        public void ShuaXin()
        {
            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            {
                if (this.dataGridView1.Rows[i].Tag is DataCunModel)
                {
                    DataCunModel model=  this.dataGridView1.Rows[i].Tag as DataCunModel;
                    this.dataGridView1.Rows[i].Cells[2].Value = model.JiCunQiModel.Value;
                }
            }
            if (SheBeiModel.Tx)
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
        }

        private void ZengJiaShuJu(DataCunModel cunModel,int leixing)
        {
            if (leixing == 1)
            {
                int index = this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[index].Cells[0].Value = cunModel.Name;
                this.dataGridView1.Rows[index].Cells[1].Value = cunModel.JiCunDiZhi;
                this.dataGridView1.Rows[index].Cells[2].Value = "";
                this.dataGridView1.Rows[index].Height = 32;
                this.dataGridView1.Rows[index].Tag = cunModel;
            }
            else if (leixing == 2)
            {
                int index = this.dataGridView2.Rows.Add();
                this.dataGridView2.Rows[index].Cells[0].Value = cunModel.Name;
                this.dataGridView2.Rows[index].Cells[1].Value = cunModel.JiCunDiZhi;
                this.dataGridView2.Rows[index].Cells[2].Value ="";
                this.dataGridView2.Rows[index].Cells[3].Value ="写入";
                this.dataGridView2.Rows[index].Height = 32;
                this.dataGridView2.Rows[index].Tag = cunModel;
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex>=0)
            {
                if (e.ColumnIndex==3)
                {
                    if (this.dataGridView2.Rows[e.RowIndex].Tag is DataCunModel)
                    {
                        DataCunModel model = this.dataGridView2.Rows[e.RowIndex].Tag as DataCunModel;
                        JiCunQiModel jicunmodel = new JiCunQiModel();
                        jicunmodel.WeiYiBiaoShi = model.JiCunQiModel.WeiYiBiaoShi;
                        jicunmodel.SheBeiID = model.JiCunQiModel.SheBeiID;
                        jicunmodel.Value = this.dataGridView2.Rows[e.RowIndex].Cells[2].Value;
                        PeiZhiLei.XieJiDianQi(jicunmodel);
                    }
                }
            }
        }
    }
}
