using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSJianMianJK.GongNengLei;
using ATSJianMianJK.XiTong.Frm.KJ;
using ATSJianMianJK.XiTong.Model;
using CommLei.JiChuLei;
using JieMianLei.FuFrom;

namespace ATSJianMianJK.XiTong.XianShiDuFrm.Frm
{
    public partial class ZhiXingXieFrm : BaseFuFrom
    {
        private int TDID = -1;
        public ZhiXingXieFrm()
        {
            InitializeComponent();
        }
        public void SetCanShu(List<XieModel> lis, string biaoti, int tdid)
        {
            TDID = tdid;
            this.labFbiaoTi.Text = biaoti;
            for (int i = 0; i < lis.Count; i++)
            {
                {
                    int index = this.jiHeDataGrid1.Rows.Add();
                    this.jiHeDataGrid1.Rows[index].Cells[0].Value = $"{lis[i].ShunXu}:{lis[i].GenJuType}";
                    this.jiHeDataGrid1.Rows[index].Cells[1].Value = $"开始";
                    this.jiHeDataGrid1.Rows[index].Height = 32;
                    this.jiHeDataGrid1.Rows[index].Tag = lis[i];
                }
                {
                    foreach (var item in lis[i].TiaoJianJiChu)
                    {
                        int index = this.jiHeDataGrid1.Rows.Add();
                        this.jiHeDataGrid1.Rows[index].Cells[0].Value = $"{lis[i].ShunXu}:{item.ShunXu}:{3} {item.ToString()}";
                        this.jiHeDataGrid1.Rows[index].Cells[1].Value = $"";
                        this.jiHeDataGrid1.Rows[index].Height = 32;
                        this.jiHeDataGrid1.Rows[index].Tag = item;
                    }
                    foreach (var item in lis[i].FuZhiJiChu)
                    {
                        int index = this.jiHeDataGrid1.Rows.Add();
                        this.jiHeDataGrid1.Rows[index].Cells[0].Value = $"{lis[i].ShunXu}:{item.ShunXu}:{1} {item.ToString()}";
                        this.jiHeDataGrid1.Rows[index].Cells[1].Value = $"";
                        this.jiHeDataGrid1.Rows[index].Height = 32;
                        this.jiHeDataGrid1.Rows[index].Tag = item;
                    }
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<XieModel> xies = new List<XieModel>();
            XieSateModel xinmodel = new XieSateModel();
            for (int i = 0; i < this.jiHeDataGrid1.Rows.Count; i++)
            {
                if (this.jiHeDataGrid1.Rows[i].Tag is XieModel)
                {
                    XieModel model = this.jiHeDataGrid1.Rows[i].Tag as XieModel;
                    XieModel xie = ChangYong.FuZhiShiTi(model);                  
                     xies.Add(xie);
                }
                if (this.jiHeDataGrid1.Rows[i].Tag is JiChuXieDYModel)
                {
                    this.jiHeDataGrid1.Rows[i].Cells[1].Value = "";
                }
            }
            if (xies.Count > 0)
            {
                xies.Sort((x, y) =>
                {
                    if (x.ShunXu > y.ShunXu)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                });
            }
            xinmodel.LisXies.AddRange(xies);
            Task.Factory.StartNew(() => {
                JiHeSheBei.Cerate().XieChengGong(TDID, xinmodel.LisXies, ZhiXingShunXu);
            });
           
        }

        private void ZhiXingShunXu(int zongshuxun,int fenshuxun,int jieguo,bool ifjieguo)
        {
            this.Controlinkove.FanXingGaiBing(() => {
                string key = "";
                if (jieguo <= 2)
                {
                    key = $"{zongshuxun}:{fenshuxun}:{1}";
                }
                else
                {
                    key = $"{zongshuxun}:{fenshuxun}:{3}";
                }
                for (int i = 0; i < this.jiHeDataGrid1.Rows.Count; i++)
                {
                    string zhi = ChangYong.TryStr(this.jiHeDataGrid1.Rows[i].Cells[0].Value, "");
                    if (zhi.StartsWith(key))
                    {
                        if (jieguo == 1)
                        {
                            this.jiHeDataGrid1.Rows[i].Cells[1].Value = "←";
                            this.jiHeDataGrid1.Rows[i].Cells[1].Style.ForeColor = Color.Yellow;
                        }
                        else if (jieguo == 2)
                        {
                            this.jiHeDataGrid1.Rows[i].Cells[1].Value = "←";
                            this.jiHeDataGrid1.Rows[i].Cells[1].Style.ForeColor = Color.Green;
                        }
                        else if (jieguo == 3)
                        {
                            this.jiHeDataGrid1.Rows[i].Cells[1].Value = "←";
                            this.jiHeDataGrid1.Rows[i].Cells[1].Style.ForeColor = Color.Yellow;
                        }
                        else if (jieguo == 4)
                        {
                            this.jiHeDataGrid1.Rows[i].Cells[1].Value = "←";
                            this.jiHeDataGrid1.Rows[i].Cells[1].Style.ForeColor = ifjieguo? Color.Green:Color.Red;
                        }
                        break;
                    }
                }
            });
          
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            //int id = ChangYong.TryInt(this.commBoxE1.Text, -1);
            //for (int i = 0; i < XieSateModels.Count; i++)
            //{
            //    foreach (var item in XieSateModels[i].LisXies)
            //    {
            //        if (item.IsShiXiTongDu == 1)
            //        {
            //            bool xiezhi = JiHeSheBei.Cerate().JiHeData.GetIOBool(id, item.DuJiCunQiName, false);
            //            FuZhi($"{item.DuSheBeiID}:{item.DuJiCunQiName}:{item.DuPeiZhiValue}", xiezhi);
            //        }
            //        else
            //        {
            //            string xiezhi = JiHeSheBei.Cerate().JiHeData.GetXieZhi(item.DuJiCunQiName, item.DuSheBeiID, "");
            //            FuZhi($"{item.DuSheBeiID}:{item.DuJiCunQiName}:{item.DuPeiZhiValue}", xiezhi);


            //        }
            //        {
            //            string xiezhi = JiHeSheBei.Cerate().JiHeData.GetXieZhi(item.XieJiCunQiName, item.XieSheBeiID, "");
            //            FuZhi($"{item.XieSheBeiID}:{item.XieJiCunQiName}", xiezhi);
            //        }
            //    }
            //}

        }
    }
}
