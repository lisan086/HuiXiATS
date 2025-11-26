using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSJianMianJK.XiTong.Model;
using CommLei.DataChuLi;
using CommLei.JiChuLei;
using JieMianLei.FuFrom.KJ;

namespace ATSJianMianJK.XiTong.Frm.FM
{
    public partial class DuCanShuFrm : JiChuWeiHuFrom
    {
       
        private List<DuShuJuModel> ShuJu = new List<DuShuJuModel>();
        public DuCanShuFrm()
        {
            InitializeComponent();
            BuXianShiKJ(new List<string>() { "查找" });
            this.IsZhiXianShiX = true;
         
            SetLie();
        }
        protected override void GuanBi()
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void SetLie()
        {
            List<LieModel> lies = new List<LieModel>();

            {
                LieModel model = new LieModel();
                model.BangDingName = "Name";
                model.DiLie = 1;
                model.FullChang = 80;
                model.IsKeJian = true;
                model.IsShanChuHang = false;
                model.IsZhiDu = true;
                model.IsZhiXianShiBangDingName = false;
                model.LeiXing = 1;
                model.LieName = "写名称";
                lies.Add(model);
            }
            {
                LieModel model = new LieModel();
                model.BangDingName = "Type";
                model.DiLie = 1;
                model.FullChang = 80;
                model.IsKeJian = true;
                model.IsShanChuHang = false;
                model.IsZhiDu = true;
                model.IsZhiXianShiBangDingName = false;
                model.LeiXing = 1;
                model.LieName = "类型";
                lies.Add(model);
            }           
            {
                LieModel model = new LieModel();
                model.BangDingName = "明细";
                model.DiLie = 9;
                model.FullChang = 30;
                model.IsKeJian = true;
                model.IsShanChuHang = false;
                model.IsZhiDu = true;
                model.IsZhiXianShiBangDingName = true;
                model.LeiXing = 4;
                model.LieName = "明细";
                lies.Add(model);
            }
            {
                LieModel model = new LieModel();
                model.BangDingName = "修改";
                model.DiLie = 9;
                model.FullChang = 30;
                model.IsKeJian = true;
                model.IsShanChuHang = false;
                model.IsZhiDu = true;
                model.IsZhiXianShiBangDingName = true;
                model.LeiXing = 4;
                model.LieName = "修改";
                lies.Add(model);
            }
            {
                LieModel model = new LieModel();
                model.BangDingName = "删除";
                model.DiLie = 9;
                model.FullChang = 30;
                model.IsKeJian = true;
                model.IsShanChuHang = false;
                model.IsZhiDu = true;
                model.IsZhiXianShiBangDingName = true;
                model.LeiXing = 4;
                model.LieName = "删除";
                lies.Add(model);
            }

            this.AddLie(lies);
        }

        /// <summary>
        /// 用于设置参数
        /// </summary>
        public void SetCanShu(List<DuShuJuModel> iOModels, List<string> shujutype)
        {
            
            this.comboBox2.Items.Clear();
            for (int i = 0; i < shujutype.Count; i++)
            {
                this.comboBox2.Items.Add(shujutype[i]);
            }
            ShuJu = iOModels;
            this.FuZhi(iOModels);
        }
        public void SetCanShu(List<DuShuJuModel> iOModels)
        {

            ShuJu = iOModels;
            this.FuZhi(iOModels);
        }
        public List<DuShuJuModel> GetCanShu()
        {
            return ChangYong.FuZhiShiTi(ShuJu);
        }
        protected override void GengGai(object sender, DataGridViewRow row, int lie, bool isshanchu)
        {
            if (isshanchu == false)
            {
                if (lie == 3)
                {
                    this.XianShiPanl(true);
                    if (row.Tag is DuShuJuModel)
                    {
                        DuShuJuModel model = row.Tag as DuShuJuModel;
                        this.textBox3.Text = model.Name;
                        this.comboBox2.Text = model.Type.ToString();
                      
                    }
                }
                else if (lie == 4)
                {
                    if (row.Tag is DuShuJuModel)
                    {
                        DuShuJuModel model = row.Tag as DuShuJuModel;
                        string id = model.Name;
                        for (int i = 0; i < ShuJu.Count; i++)
                        {
                            if (ShuJu[i].Name == id)
                            {
                                ShuJu.RemoveAt(i);
                                break;
                            }
                        }
                        SetCanShu(ShuJu);
                    }

                }
                else if (lie == 2)
                {
                    if (row.Tag is DuShuJuModel)
                    {
                        DuShuJuModel model = row.Tag as DuShuJuModel;
                        string id = model.Name;
                        for (int i = 0; i < ShuJu.Count; i++)
                        {
                            if (ShuJu[i].Name == id)
                            {
                                DuJiCunQiFrm frm = new DuJiCunQiFrm();
                                frm.SetCanShu( ShuJu[i].LisJiCunQi,1);
                                if (frm.ShowDialog(this) == DialogResult.OK)
                                {

                                    ShuJu[i].LisJiCunQi = ChangYong.FuZhiShiTi(frm.GetCanShu());
                                }
                                break;
                            }
                        }

                    }

                }
            }
        }
        protected override void BaoCun()
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        protected override void XingZeng()
        {
            base.XingZeng();

        }
        protected override void QueDing()
        {
            string id = ChangYong.TryStr(this.textBox3.Text, "");
            bool iscunzai = false;
            for (int i = 0; i < ShuJu.Count; i++)
            {
                if (ShuJu[i].Name == id)
                {
                    iscunzai = true;          
                    ShuJu[i].Type = this.comboBox2.Text;
                    ShuJu[i].Name = this.textBox3.Text;
                    break;
                }
            }
            if (iscunzai == false)
            {
                DuShuJuModel dumodel = new DuShuJuModel();           
                dumodel.Type = this.comboBox2.Text;
                dumodel.Name = this.textBox3.Text;
                ShuJu.Add(dumodel);
            }
            SetCanShu(ShuJu);
        }

        protected override void DaoRu(string wenjiannale)
        {
            DuRuExclWenDan daoChuTxlxs = new DuRuExclWenDan();

            if (string.IsNullOrEmpty(wenjiannale) == false)
            {
                DataTable dt = daoChuTxlxs.DuQuWenJian(wenjiannale);
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<DuShuJuModel> lis = new List<DuShuJuModel>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string name = ChangYong.TryStr(dt.Rows[i]["Name"], "");
                        bool iscunzai = false;
                        for (int c = 0; c < lis.Count; c++)
                        {
                            if (lis[c].Name.Equals(name))
                            {

                                iscunzai = true;
                                lis[c].LisJiCunQi.Add(GetModel(dt.Rows[i]));
                                break;
                            }
                        }

                        if (iscunzai == false)
                        {
                            DuShuJuModel xinmodel = new DuShuJuModel();
                            xinmodel.Name = name;
                         
                            xinmodel.Type = ChangYong.TryStr(dt.Rows[i]["Type"], "");
                            xinmodel.LisJiCunQi.Add(GetModel(dt.Rows[i]));
                            lis.Add(xinmodel);
                        }
                    }
                    SetCanShu(lis);
                }
            }//EXCEL模板里的数据添加到界面gridview成功！              
        }
        protected override void DaoChu(string wenjinnane)
        {
            Dictionary<string, List<object>> jieguo = new Dictionary<string, List<object>>();
            jieguo.Add("Name", new List<object>());
            jieguo.Add("Type", new List<object>());
          
            jieguo.Add("JiCunQiName", new List<object>());
            jieguo.Add("IOType", new List<object>());
            jieguo.Add("PiPeiType", new List<object>());
            jieguo.Add("PiPeiValue", new List<object>());
            jieguo.Add("SheBeiID", new List<object>());
            List<DuShuJuModel> xieSates = GetCanShu();
            for (int c = 0; c < xieSates.Count; c++)
            {
                for (int i = 0; i < xieSates[c].LisJiCunQi.Count; i++)
                {
                    jieguo["Name"].Add(xieSates[c].Name);
                    jieguo["Type"].Add(xieSates[c].Type);
                 
                    DuModel models = xieSates[c].LisJiCunQi[i];
                    jieguo["JiCunQiName"].Add(models.JiCunQiName);
                    jieguo["IOType"].Add(models.Type);
                    jieguo["PiPeiType"].Add(models.PiPeiType);
                    jieguo["PiPeiValue"].Add(models.PiPeiValue);
                    jieguo["SheBeiID"].Add(models.SheBeiID);
                }
            }
            DuRuExclWenDan daoChuTxlxs = new DuRuExclWenDan();
            daoChuTxlxs.DaoChuExc(wenjinnane, jieguo);
        }

        private DuModel GetModel(DataRow row)
        {
            DuModel model = new DuModel();
            model.JiCunQiName = ChangYong.TryStr(row["JiCunQiName"], "");
            model.Type = ChangYong.TryStr(row["IOType"], "");
            model.PiPeiType = ChangYong.GetMeiJuZhi<PiPeiType>(ChangYong.TryStr(row["PiPeiType"], ""));
            model.PiPeiValue = ChangYong.TryStr(row["PiPeiValue"], "");
            model.SheBeiID = ChangYong.TryInt(ChangYong.TryStr(row["SheBeiID"], ""),-1);
            return model;
        }
    }
}
