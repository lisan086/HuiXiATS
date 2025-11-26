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
using SSheBei.PeiZhi;
using ZhongWangSheBei.Frm;
using ZhongWangSheBei.Model;

namespace ZhongChuanAVLei.Frm
{
    public partial class ShuJuKJ : UserControl
    {
        private PeiZhiLei PeiZhiLei;
        public ShuJuKJ()
        {
            InitializeComponent();
        }
        public void SetCanShu(List<CunModel> lis,PeiZhiLei peiZhiLei)
        {
            this.dataGridView1.Rows.Clear();
            this.dataGridView2.Rows.Clear();
            PeiZhiLei = peiZhiLei;
            for (int i = 0; i < lis.Count; i++)
            {
                if (lis[i].IsDu.ToString().ToLower().Contains("du"))
                {
                    int index = this.dataGridView1.Rows.Add();
                    this.dataGridView1.Rows[index].Cells[0].Value = lis[i].JiCunQi.WeiYiBiaoShi;
                    this.dataGridView1.Rows[index].Cells[1].Value = $"{lis[i].ZiID}:{lis[i].JiCunDiZhi}";
                    this.dataGridView1.Rows[index].Cells[2].Value = lis[i].JiCunQi.Value;
                    this.dataGridView1.Rows[index].Height = 32;
                    this.dataGridView1.Rows[index].Tag = lis[i];
                }
                else
                {
                    int index = this.dataGridView2.Rows.Add();
                    this.dataGridView2.Rows[index].Cells[0].Value = lis[i].JiCunQi.WeiYiBiaoShi;
                    this.dataGridView2.Rows[index].Cells[1].Value = $"{lis[i].ZiID}:{lis[i].JiCunDiZhi}";
                    this.dataGridView2.Rows[index].Cells[2].Value ="";
                    this.dataGridView2.Rows[index].Cells[3].Value = "执行";
                    this.dataGridView2.Rows[index].Height = 32;
                    this.dataGridView2.Rows[index].Tag = lis[i];
                }
            }
        }

        public void ShuaXin()
        {
            int count = this.dataGridView1.Rows.Count;
          
            for (int i = 0; i < count; i++)
            {
                if (this.dataGridView1.Rows[i].Tag is CunModel)
                {
                    CunModel model = this.dataGridView1.Rows[i].Tag as CunModel;
                    this.dataGridView1.Rows[i].Cells[2].Value = model.JiCunQi.Value;
                    if (model.JiCunQi.IsKeKao)
                    {
                        this.dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                    }
                    else
                    {
                        this.dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
              
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex>=0)
            {
                if (e.ColumnIndex==3)
                {
                    if (this.dataGridView2.Rows[e.RowIndex].Tag is CunModel)
                    {
                        CunModel model = this.dataGridView2.Rows[e.RowIndex].Tag as CunModel;
                        CunModel xinmodel = model.FuZhi();
                        xinmodel.JiCunQi.Value = ChangYong.TryStr(this.dataGridView2.Rows[e.RowIndex].Cells[2].Value,"0");
                        PeiZhiLei.XieJiDianQi(xinmodel);
                    }

                }
            }
        }
    }
}
