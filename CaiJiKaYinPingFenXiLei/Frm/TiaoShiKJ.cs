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
        private bool DianKaiShiHua = false;
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

            if (PeiZhiLei.DataMoXing.YuanShiDian.Count > 0 && PeiZhiLei.DataMoXing.LvBoDianDian.Count>0 && PeiZhiLei.DataMoXing.PingLvDian.Count>0)
            {
                if (DianKaiShiHua == false)
                {
                    DianKaiShiHua = true;
                    float shijian =(float)(SaoMaModel.CaiJiMiaoSu/SaoMaModel.CaiJiShuLiang);
                    this.quXian21KJ1.ClearData();
                    this.quXian21KJ2.ClearData();
                    this.quXian21KJ3.ClearData();
                    float shijiand = 0;
                    for (int i = 0; i < PeiZhiLei.DataMoXing.YuanShiDian.Count; i++)
                    {
                        PointF dian = new PointF(shijiand, (float)PeiZhiLei.DataMoXing.YuanShiDian[i]);
                        this.quXian21KJ1.AddDian(dian);
                        shijiand++;
                    }
                    shijiand = 0;
                    for (int i = 0; i < PeiZhiLei.DataMoXing.LvBoDianDian.Count; i++)
                    {
                        PointF dian = new PointF(shijiand, (float)PeiZhiLei.DataMoXing.LvBoDianDian[i]);
                        this.quXian21KJ2.AddDian(dian);
                        shijiand++;
                    }
                    for (int i = 0; i < PeiZhiLei.DataMoXing.PingLvDian.Count; i++)
                    {
                        PointF dian = new PointF(PeiZhiLei.DataMoXing.PingLvDian[i].X, PeiZhiLei.DataMoXing.PingLvDian[i].Y);
                        this.quXian21KJ3.AddDian(dian);

                    }
                    this.quXian21KJ2.KaiShiHua();
                    this.quXian21KJ1.KaiShiHua();
                    this.quXian21KJ3.KaiShiHua();
                }
            }
            else
            {
                if (DianKaiShiHua)
                {
                    this.quXian21KJ1.ClearData();
                    this.quXian21KJ2.ClearData();
                    this.quXian21KJ3.ClearData();
                    this.quXian21KJ2.KaiShiHua();
                    this.quXian21KJ1.KaiShiHua();
                    this.quXian21KJ3.KaiShiHua();
                }
                DianKaiShiHua = false;
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
