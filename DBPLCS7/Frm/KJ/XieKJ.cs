using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommLei.JiChuLei;
using DBPLCS7.Model;

namespace DBPLCS7.Frm.KJ
{
    public partial class XieKJ : UserControl
    {
        public event Func<PLCJiCunQiModel,string> ZhiXingEvent;
        private PLCShBeiModel PLCShBeiModel=new PLCShBeiModel();
        public XieKJ()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        public void SetCanShu(PLCShBeiModel pLCShBeiModel)
        {
            this.dataGridView1.Rows.Clear();
            PLCShBeiModel = pLCShBeiModel;
            this.label1.Text = PLCShBeiModel.PLCName;
            for (int i = 0; i < pLCShBeiModel.JiCunQi.Count; i++)
            {
                if (pLCShBeiModel.JiCunQi[i].GongNengType.ToString().Contains("Xie"))
                {
                    FuZhi(pLCShBeiModel.JiCunQi[i]);
                }
            }
        }
        private void FuZhi(PLCJiCunQiModel model)
        {
            int index = this.dataGridView1.Rows.Add();
            this.dataGridView1.Rows[index].Cells[0].Value = model.Name;
            this.dataGridView1.Rows[index].Cells[1].Value = $"{model.DBKuan},{model.PianYiLiang},{model.PLCDataType.ToString()}";
            this.dataGridView1.Rows[index].Cells[2].Value = model.GongNengType.ToString();
            this.dataGridView1.Rows[index].Cells[3].Value = "0";
            this.dataGridView1.Rows[index].Cells[4].Value = "0";
            this.dataGridView1.Rows[index].Cells[5].Value ="执行";
            this.dataGridView1.Rows[index].Tag = model;
            this.dataGridView1.Rows[index].Height = 32;
        }
        public void ShuXin()
        {
            if (PLCShBeiModel.Tx)
            {
                if (this.label1.ForeColor != Color.Green)
                {
                    this.label1.ForeColor = Color.Green;
                }
            }
            else
            {
                if (this.label1.ForeColor != Color.Red)
                {
                    this.label1.ForeColor = Color.Red;
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex>=0)
            {
                if (e.ColumnIndex==5)
                {
                    if (this.dataGridView1.Rows[e.RowIndex].Tag is PLCJiCunQiModel)
                    {
                        PLCJiCunQiModel modesl =ChangYong.FuZhiShiTi( this.dataGridView1.Rows[e.RowIndex].Tag as PLCJiCunQiModel);
                        modesl.Value = this.dataGridView1.Rows[e.RowIndex].Cells[3].Value;
                        if (ZhiXingEvent != null)
                        {
                            this.dataGridView1.Rows[e.RowIndex].Cells[4].Value = "";
                            string jieguo= ZhiXingEvent(modesl);
                            this.dataGridView1.Rows[e.RowIndex].Cells[4].Value = jieguo;

                        }
                    }
                }
            }
        }
    }
}
