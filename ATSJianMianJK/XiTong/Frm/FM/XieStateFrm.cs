using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSJianMianJK.XiTong.Frm.KJ;
using ATSJianMianJK.XiTong.Model;
using CommLei.DataChuLi;
using CommLei.JiChuLei;
using Common.DataChuLi;
using JieMianLei.FuFrom;


namespace ATSJianMianJK.XiTong.Frm.FM
{
    public partial class XieStateFrm : BaseFuFrom
    {
      
        public XieStateFrm()
        {
            InitializeComponent();
        }
        protected override void GuanBi()
        {
            this.DialogResult = DialogResult.Cancel;
            base.GuanBi();
        }
        public void SetCanShu(List<XieSateModel> lis)
        {
          
            this.flowLayoutPanel1.Controls.Clear();
            List<XieKJ> kjs = new List<XieKJ>();
            for (int i = 0; i < lis.Count; i++)
            {
                XieKJ kJ = new XieKJ();
                kJ.SetCanShu( lis[i]);
                kjs.Add(kJ);
            }
            this.flowLayoutPanel1.Controls.AddRange(kjs.ToArray());
        }

        public List<XieSateModel> GetCanShu()
        {
            List<XieSateModel> lis = new List<XieSateModel>();
            for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
            {
                if (this.flowLayoutPanel1.Controls[i] is XieKJ)
                {
                    XieKJ kJ = this.flowLayoutPanel1.Controls[i] as XieKJ;
                    lis.Add(kJ.GetModel());
                }
            }
            return lis;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            XieKJ kJ = new XieKJ();
            kJ.SetCanShu(new XieSateModel());
            this.flowLayoutPanel1.Controls.Add(kJ);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Dictionary<string, List<object>> jieguo = new Dictionary<string, List<object>>();
            jieguo.Add("Name", new List<object>());
            jieguo.Add("JiCunQiName", new List<object>());
            jieguo.Add("PeiZhiValue", new List<object>());
            jieguo.Add("IsYongPeiZhi", new List<object>());
            jieguo.Add("ShunXu", new List<object>());
            jieguo.Add("GenJuType", new List<object>());
            jieguo.Add("IsPingBi", new List<object>());
            jieguo.Add("XieJiCunType", new List<object>());
            jieguo.Add("KongZhiJiGe", new List<object>());
            jieguo.Add("SheBeiID", new List<object>());
            List<XieSateModel> xieSates = GetCanShu();
            for (int c = 0; c < xieSates.Count; c++)
            {              
                for (int i = 0; i < xieSates[c].LisXies.Count; i++)
                {
                    jieguo["Name"].Add(xieSates[c].Name);
                    XieModel models = xieSates[c].LisXies[i];
                    //jieguo["JiCunQiName"].Add(models.JiCunQiName);
                    //jieguo["SheBeiID"].Add(models.SheBeiID);
                    //jieguo["PeiZhiValue"].Add(models.PeiZhiValue);
                    //jieguo["IsYongPeiZhi"].Add(models.IsYongPeiZhi);
                    jieguo["ShunXu"].Add(models.ShunXu);
                    jieguo["GenJuType"].Add(models.GenJuType);
                  
                   
               
                  
                }
            }
            DuRuExclWenDan daoChuTxlxs = new DuRuExclWenDan();
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "*.xlsx|*.xlsx";
            if (saveFile.ShowDialog(this) == DialogResult.OK)
            {
                string  wenjinnane = saveFile.FileName;

                if (string.IsNullOrEmpty(wenjinnane) == false)
                {
                    daoChuTxlxs.DaoChuExc(wenjinnane, jieguo);
                }
            }
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DuRuExclWenDan daoChuTxlxs = new DuRuExclWenDan();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "*.xlsx|*.xlsx";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string wenjinnane = openFileDialog.FileName;
                if (string.IsNullOrEmpty(wenjinnane) == false)
                {
                    DataTable dt = daoChuTxlxs.DuQuWenJian(wenjinnane);
                    if (dt!=null&&dt.Rows.Count>0)
                    {
                        List<XieSateModel> lis = new List<XieSateModel>();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string name =ChangYong.TryStr( dt.Rows[i]["Name"],"");
                            bool iscunzai = false;
                            for (int c = 0; c < lis.Count; c++)
                            {
                                if (lis[c].Name.Equals(name))
                                {
                                  
                                    iscunzai = true;
                                    lis[c].LisXies.Add(GetModel(dt.Rows[i]));
                                    break;
                                }
                            }

                            if (iscunzai==false)
                            {
                                XieSateModel xinmodel = new XieSateModel();
                                xinmodel.Name = name;                             
                                xinmodel.LisXies.Add(GetModel(dt.Rows[i]));
                                lis.Add(xinmodel);
                            }
                        }
                        SetCanShu(lis);
                    }
                }//EXCEL模板里的数据添加到界面gridview成功！              
            }
        }

        private XieModel GetModel(DataRow row)
        {
            XieModel model = new XieModel();
            model.GenJuType =ChangYong.GetMeiJuZhi<GenJuType>( ChangYong.TryStr(row["GenJuType"], "FuZhi"));
            //model.IsPingBi = ChangYong.TryInt(ChangYong.TryStr(row["IsPingBi"], "1"), 1);
            //model.IsYongPeiZhi = ChangYong.TryInt(ChangYong.TryStr(row["IsYongPeiZhi"], "1"), 1);
            //model.JiCunQiName = ChangYong.TryStr(row["JiCunQiName"], "");
            //model.SheBeiID = ChangYong.TryInt(ChangYong.TryStr(row["SheBeiID"], "1"), -1);
            //model.KongZhiJiGe = ChangYong.TryInt(ChangYong.TryStr(row["KongZhiJiGe"], "1"), 1);
            //model.PeiZhiValue = ChangYong.TryStr(row["PeiZhiValue"], "");
            model.ShunXu = ChangYong.TryInt(ChangYong.TryStr(row["ShunXu"], "1"), 1);
        
            return model;
        }
    }
}
