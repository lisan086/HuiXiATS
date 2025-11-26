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
using DBPLCS7.Model;

namespace DBPLCS7.Frm.KJ
{
    public partial class DuKJ : UserControl
    {
        private PLCShBeiModel PLCShBeiModel = new PLCShBeiModel();
        public DuKJ()
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
                if (pLCShBeiModel.JiCunQi[i].GongNengType.ToString().Contains("Du"))
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
            this.dataGridView1.Rows[index].Cells[3].Value = model.Value;
           
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
            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            {
                if (this.dataGridView1.Rows[i].Tag is PLCJiCunQiModel)
                {

                    PLCJiCunQiModel model = this.dataGridView1.Rows[i].Tag as PLCJiCunQiModel;
                    this.dataGridView1.Rows[i].Cells[3].Value = model.Value;

                }
            }
        }

    }
}
