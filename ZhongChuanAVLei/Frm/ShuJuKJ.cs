using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZhongWangSheBei.Model;

namespace ZhongChuanAVLei.Frm
{
    public partial class ShuJuKJ : UserControl
    {
        public ShuJuKJ()
        {
            InitializeComponent();
        }
        public void SetCanShu(List<CunModel> lis)
        {
            this.dataGridView1.Rows.Clear();
            for (int i = 0; i < lis.Count; i++)
            {
                int index = this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[index].Cells[0].Value = lis[i].JiCunQi.WeiYiBiaoShi;
                this.dataGridView1.Rows[index].Cells[1].Value = $"{lis[i].ZDiZhi}:{lis[i].JiCunDiZhi}";
                this.dataGridView1.Rows[index].Cells[2].Value = lis[i].JiCunQi.Value;
                this.dataGridView1.Rows[index].Height = 32;
                this.dataGridView1.Rows[index].Tag = lis[i];
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
    }
}
