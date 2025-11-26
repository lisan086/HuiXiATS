using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommLei.JiChuLei;
using JieMianLei.FuFrom;
using SSheBei.Model;
using YiBanSaoMaQi.Model;

namespace YiBanSaoMaQi.Frm
{
    public partial class TiaoShiKJ : UserControl
    {

        private BaseFuFrom BaseFuFrom;
        private PeiZhiLei PeiZhiLei;
        public TiaoShiKJ()
        {
            InitializeComponent();
        }
        public void SetCanShu(List<CunModel> cunmodels,PeiZhiLei peiZhiLei,BaseFuFrom baseFu)
        {
            BaseFuFrom=baseFu;
            PeiZhiLei =peiZhiLei;
            for (int i = 0; i < cunmodels.Count; i++)
            {
                int index = this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[index].Cells[0].Value = cunmodels[i].JiCunQi.WeiYiBiaoShi;
                this.dataGridView1.Rows[index].Cells[1].Value = cunmodels[i].Name;
                this.dataGridView1.Rows[index].Cells[2].Value = "";
                this.dataGridView1.Rows[index].Cells[3].Value = cunmodels[i].IsData?"":"执行";
                this.dataGridView1.Rows[index].Height = 32;
                this.dataGridView1.Rows[index].Tag= cunmodels[i];
            }
        }

        private  void ShuaXin(CunModel cunModel)
        {

            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            {
                if (this.dataGridView1.Rows[i].Tag is CunModel)
                {
                    CunModel model = this.dataGridView1.Rows[i].Tag as CunModel;
                    if (model.IsData&& model.ZhiLingId== cunModel.ZhiLingId)
                    {
                     
                        if (model.IsZhengZaiCe == 1)
                        {
                            this.dataGridView1.Rows[i].Cells[2].Value = model.JiCunQi.Value.ToString();

                        }
                        else if (model.IsZhengZaiCe == 0)
                        {
                            this.dataGridView1.Rows[i].Cells[2].Value = "进行中";
                        }
                        else
                        {
                            this.dataGridView1.Rows[i].Cells[2].Value = "超时或者不对";
                        }
                    }
                }
            }
            
         
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 3)
                {
                    if (this.dataGridView1.Rows[e.RowIndex].Tag is CunModel)
                    {
                        CunModel model = this.dataGridView1.Rows[e.RowIndex].Tag as CunModel;
                        JiCunQiModel xinmodel = ChangYong.FuZhiShiTi(model.JiCunQi);            
                        this.dataGridView1.Rows[e.RowIndex].Cells[2].Value = "";
                        double haomiao = 0;
                        bool ischaoshi = false;
                        BaseFuFrom.Waiting(() => {
                            DateTime timeshijina = DateTime.Now;
                            int chaoshitime = model.Time;
                            PeiZhiLei.XieJiDianQi(xinmodel);
                            Thread.Sleep(50);
                            for (; ; )
                            {
                                
                                if (model.IsZhengZaiCe == 1)
                                {
                                    haomiao = (DateTime.Now - timeshijina).TotalMilliseconds;
                                    break;
                                }
                                if ((DateTime.Now - timeshijina).TotalMilliseconds >= chaoshitime)
                                {
                                    ischaoshi = true;
                                    haomiao = chaoshitime;
                                    break;
                                }
                            }
                        }, "正在执行，请稍后...", this);
                        if (ischaoshi)
                        {
                            this.dataGridView1.Rows[e.RowIndex].Cells[2].Value = $"超时:{haomiao}ms";
                        }
                        else
                        {
                            this.dataGridView1.Rows[e.RowIndex].Cells[2].Value = $"{haomiao}ms {model.JiCunQi.Value}";
                        }
                        ShuaXin(model);
                    }
                }
            }
        }


       
    }
}
