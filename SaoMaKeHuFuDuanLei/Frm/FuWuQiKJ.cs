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
using YiBanSaoMaQi.Frm;
using YiBanSaoMaQi.Model;

namespace SaoMaKeHuFuDuanLei.Frm
{
    public partial class FuWuQiKJ : UserControl
    {
        private IPFuWuPeiModel SaoMaModel;
        private PeiZhiLei PeiZhiLei;
        public FuWuQiKJ()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        public void SetCanShu(IPFuWuPeiModel maModel, PeiZhiLei peiZhiLei)
        {
            SaoMaModel = maModel;
            PeiZhiLei = peiZhiLei;
            this.label4.Text = maModel.ZhanDianName;
            this.label4.BackColor = maModel.Tx ? Color.Green : Color.Red;
           

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
            List<DataGridViewRow> lis = new List<DataGridViewRow>();
            for (int i = 0; i < this.dataGridView2.Rows.Count; i++)
            {
                if (this.dataGridView2.Rows[i].Tag is SaoMaMoXingModel)
                {
                    SaoMaMoXingModel dsss = this.dataGridView2.Rows[i].Tag as SaoMaMoXingModel;
                    if (dsss.JieSu!=0)
                    {
                        lis.Add(this.dataGridView2.Rows[i]);
                    }
                }
            }
            for (int i = 0; i < lis.Count; i++)
            {
                this.dataGridView2.Rows.Remove(lis[i]);
            }
            List<SaoMaMoXingModel> lisvv= PeiZhiLei.DataMoXing.GetShuJu();
            for (int c = 0; c < lisvv.Count; c++)
            {
                bool zhen = false;
                for (int i = 0; i < this.dataGridView2.Rows.Count; i++)
                {
                    if (this.dataGridView2.Rows[i].Tag is SaoMaMoXingModel)
                    {
                        SaoMaMoXingModel dsss = this.dataGridView2.Rows[i].Tag as SaoMaMoXingModel;
                        if (dsss.BiaoShi== lisvv[c].BiaoShi)
                        {
                            this.dataGridView2.Rows[i].Cells[1].Value = lisvv[c].PaiXu;
                            zhen = true;
                            break;
                        }
                    }
                }
                if (zhen==false)
                {
                    FuZhi(lisvv[c]);
                }
            }
        }

        public void FuZhi(SaoMaMoXingModel model)
        {
            int index = this.dataGridView2.Rows.Add();
            this.dataGridView2.Rows[index].Cells[0].Value = ChangYong.HuoQuJsonStr(model);
            this.dataGridView2.Rows[index].Cells[1].Value = model.PaiXu;
           
            this.dataGridView2.Rows[index].Tag = model;
            this.dataGridView2.Rows[index].Height = 32;
        }
    }
}
