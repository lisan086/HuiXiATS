using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSJianMianJK.XiTong.Frm.FM;
using ATSJianMianJK.XiTong.Model;
using CommLei.JiChuLei;

namespace ATSJianMianJK.XiTong.Frm.KJ.XieQKJ
{
    public partial class XieJiHeKJ : UserControl
    {
        private bool IsFuZhi = false;
        public XieJiHeKJ()
        {
            InitializeComponent();
        }

        public void SetCanShu( List<JiChuXieDYModel> xieDYModel, bool isfuzhi)
        {
            this.jiHeDataGrid1.Rows.Clear();
            IsFuZhi = isfuzhi;
            for (int i = 0; i < xieDYModel.Count; i++)
            {
                FuZHi(xieDYModel[i]);
            }
        }
        /// <summary>
        /// 移动数据
        /// </summary>
        /// <param name="isxiangxia"></param>
        /// <param name="node"></param>
        private void YiDong(bool isxiangxia, DataGridViewRow node)
        {
            int sdhu = this.jiHeDataGrid1.Rows.Count;
            int index = -1;

            for (int i = 0; i < sdhu; i++)
            {
                if (this.jiHeDataGrid1.Rows[i].Equals(node))
                {
                    index = i;
                    if (isxiangxia == false)
                    {
                        if (index == 0)
                        {
                            index = -1;
                        }
                        else
                        {
                            this.jiHeDataGrid1.Rows.Remove(node);
                        }
                    }
                    else
                    {
                        if (index == sdhu - 1)
                        {
                            index += 1;
                        }
                        else
                        {

                            this.jiHeDataGrid1.Rows.Remove(node);

                        }
                    }

                    break;
                }
            }
            if (isxiangxia == false)
            {
                index -= 1;
                if (index >= 0)
                {

                    this.jiHeDataGrid1.Rows.Insert(index, node);
                    node.Selected = true;
                }

            }
            else
            {
                index += 1;
                if (index < sdhu)
                {
                    this.jiHeDataGrid1.Rows.Insert(index, node);
                    node.Selected = true;
                }
            }
        }

        private void FuZHi(JiChuXieDYModel model)
        {
            int index = this.jiHeDataGrid1.Rows.Add();
            this.jiHeDataGrid1.Rows[index].Cells[0].Value = model.ToString();
            this.jiHeDataGrid1.Rows[index].Cells[1].Value = "删除";
            this.jiHeDataGrid1.Rows[index].Height = 32;
            this.jiHeDataGrid1.Rows[index].Tag = model;
        }

        private void jiHeDataGrid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex>=0)
            {
                if (e.ColumnIndex == 1)
                {
                    this.jiHeDataGrid1.Rows.RemoveAt(e.RowIndex);
                }
                else if (e.ColumnIndex == 0)
                {
                    XieMoKuaiJiCunQiFrm moKuaiJiCunQiFrm = new XieMoKuaiJiCunQiFrm();
                    moKuaiJiCunQiFrm.SetCanShu(this.jiHeDataGrid1.Rows[e.RowIndex].Tag as JiChuXieDYModel,IsFuZhi);
                    moKuaiJiCunQiFrm.ShowDialog(this);
                    this.jiHeDataGrid1.Rows[e.RowIndex].Cells[0].Value = moKuaiJiCunQiFrm.GetCanShu().ToString();
                    this.jiHeDataGrid1.Rows[e.RowIndex].Tag = ChangYong.FuZhiShiTi(moKuaiJiCunQiFrm.GetCanShu());
                }
            }
        }
        public List<JiChuXieDYModel> GetCanShu()
        {
            List<JiChuXieDYModel> lis = new List<JiChuXieDYModel>();
            for (int i = 0; i < this.jiHeDataGrid1.Rows.Count; i++)
            {
                if (this.jiHeDataGrid1.Rows[i].Tag is JiChuXieDYModel)
                {
                    JiChuXieDYModel model = this.jiHeDataGrid1.Rows[i].Tag as JiChuXieDYModel;
                    model.ShunXu = i;
                    lis.Add(ChangYong.FuZhiShiTi(model));
                }
            }
            return lis; 
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (this.jiHeDataGrid1.SelectedCells.Count > 0)
            {

                for (int i = 0; i < this.jiHeDataGrid1.SelectedCells.Count; i++)
                {
                    DataGridViewCell node = this.jiHeDataGrid1.SelectedCells[i];
                   
                    YiDong(false, this.jiHeDataGrid1.Rows[node.RowIndex]);
                }


            }
           
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (this.jiHeDataGrid1.SelectedCells.Count > 0)
            {

                for (int i = 0; i < this.jiHeDataGrid1.SelectedCells.Count; i++)
                {
                    DataGridViewCell node = this.jiHeDataGrid1.SelectedCells[i];

                    YiDong(true, this.jiHeDataGrid1.Rows[node.RowIndex]);
                }


            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FuZHi(new JiChuXieDYModel());
        }
    }
}
