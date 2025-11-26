using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SSheBei.Model;
using ViSAKu.Model;

namespace ViSAKu.Frm
{
    public partial class TiaoShiKJ : UserControl
    {
        private PeiZhiLei PeiZhiLei;
        private SheBeiVisaModel SheBeiModel;
        public TiaoShiKJ()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }
        public void SetCanShu(SheBeiVisaModel shebeimodel, PeiZhiLei peiZhi)
        {
            PeiZhiLei = peiZhi;
            SheBeiModel = shebeimodel;
            this.label1.Text = SheBeiModel.Name;
            for (int i = 0; i < SheBeiModel.LisData.Count; i++)
            {
                CunType cunModel = SheBeiModel.LisData[i].IsDu;
                if (cunModel == CunType.DuShuJu)
                {
                    ZengJiaShuJu(SheBeiModel.LisData[i], 1);
                }
                else
                {
                    ZengJiaShuJu(SheBeiModel.LisData[i], 2);
                }
                
            }
        }


        public void ShuaXin()
        {
            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            {
                if (this.dataGridView1.Rows[i].Tag is DataLieModel)
                {
                    DataLieModel model = this.dataGridView1.Rows[i].Tag as DataLieModel;
                    this.dataGridView1.Rows[i].Cells[2].Value = model.JiCunQiModel.Value;
                }
            }
            if (SheBeiModel.IsConnect)
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

        private void ZengJiaShuJu(DataLieModel cunModel, int leixing)
        {
            if (leixing == 1)
            {
                int index = this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[index].Cells[0].Value = cunModel.Name;
                this.dataGridView1.Rows[index].Cells[1].Value = cunModel.CMD;
                this.dataGridView1.Rows[index].Cells[2].Value = "";
                this.dataGridView1.Rows[index].Height = 32;
                this.dataGridView1.Rows[index].Tag = cunModel;
            }
            else if (leixing == 2)
            {
                int index = this.dataGridView2.Rows.Add();
                this.dataGridView2.Rows[index].Cells[0].Value = cunModel.Name;
                this.dataGridView2.Rows[index].Cells[1].Value = cunModel.CMD;
                this.dataGridView2.Rows[index].Cells[2].Value = "";
                this.dataGridView2.Rows[index].Cells[3].Value = "写入";
                this.dataGridView2.Rows[index].Cells[4].Value = "";
                this.dataGridView2.Rows[index].Height = 32;
                this.dataGridView2.Rows[index].Tag = cunModel;
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 3)
                {
                    if (this.dataGridView2.Rows[e.RowIndex].Tag is DataLieModel)
                    {
                        DataLieModel model = this.dataGridView2.Rows[e.RowIndex].Tag as DataLieModel;
                        JiCunQiModel jicunmodel = new JiCunQiModel();
                        jicunmodel.WeiYiBiaoShi = model.JiCunQiModel.WeiYiBiaoShi;
                        jicunmodel.SheBeiID = model.JiCunQiModel.SheBeiID;
                        jicunmodel.Value = this.dataGridView2.Rows[e.RowIndex].Cells[2].Value;
                        this.dataGridView2.Rows[e.RowIndex].Cells[4].Value = "正在执行";
                        Task.Factory.StartNew(() => {
                            DateTime dateTime = DateTime.Now;
                            string jieguo = PeiZhiLei.XieJiDianQi(jicunmodel);
                            string miaosu = $"{(DateTime.Now - dateTime).TotalMilliseconds.ToString("0.00")}ms";
                            this.label1.Invoke(new Action(() => {
                                this.dataGridView2.Rows[e.RowIndex].Cells[4].Value = $"{miaosu}  {jieguo}";
                            }));
                        });
                      
                       
                    }
                }
            }
        }
    }
}
