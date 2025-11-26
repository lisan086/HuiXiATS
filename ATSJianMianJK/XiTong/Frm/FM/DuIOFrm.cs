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
using BaseUI.UC;
using CommLei.DataChuLi;
using CommLei.JiChuLei;
using JieMianLei.FuFrom.KJ;

namespace ATSJianMianJK.XiTong.Frm.FM
{
    public partial class DuIOFrm : JiChuWeiHuFrom
    {
        private List<string> JiCunQiLeiXing = new List<string>();
        private List<DuIOCanShuModel> ShuJu = new List<DuIOCanShuModel>();
        public DuIOFrm()
        {
            InitializeComponent();
            BuXianShiKJ(new List<string>() { "查找" });
            this.IsZhiXianShiX = true;
            this.commBoxE1.Items.Clear();
            this.comboBox2.Items.Clear();
            {
                List<string> zhi = ChangYong.MeiJuLisName(typeof(PanDuanType));
                for (int i = 0; i < zhi.Count; i++)
                {
                    this.commBoxE1.Items.Add(zhi[i]);
                }
            }
            {
                List<string> zhi = ChangYong.MeiJuLisName(typeof(IOType));
                for (int i = 0; i < zhi.Count; i++)
                {
                    this.comboBox2.Items.Add(zhi[i]);
                }
            }
            SetLie();
        }
        protected override void GuanBi()
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        protected override void BaoCun()
        {
            this.DialogResult = DialogResult.OK;
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
                model.BangDingName = "PanDuanFangShi";
                model.DiLie = 1;
                model.FullChang = 80;
                model.IsKeJian = true;
                model.IsShanChuHang = false;
                model.IsZhiDu = true;
                model.IsZhiXianShiBangDingName = false;
                model.LeiXing = 1;
                model.LieName = "判断";
                lies.Add(model);
            }
            {
                LieModel model = new LieModel();
                model.BangDingName = "MiaShu";
                model.DiLie = 1;
                model.FullChang = 80;
                model.IsKeJian = true;
                model.IsShanChuHang = false;
                model.IsZhiDu = true;
                model.IsZhiXianShiBangDingName = false;
                model.LeiXing = 1;
                model.LieName = "延时(s)";
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
        public void SetCanShu(List<DuIOCanShuModel> iOModels)
        {
            List<string> ioleixing = ChangYong.MeiJuLisName(typeof(IOType));
            this.comboBox2.Items.Clear();
            for (int i = 0; i < ioleixing.Count; i++)
            {
                this.comboBox2.Items.Add(ioleixing[i]);
            }
            ShuJu = iOModels;
            this.FuZhi(iOModels);
        }
       
        public List<DuIOCanShuModel> GetCanShu()
        {
            return ChangYong.FuZhiShiTi(ShuJu);
        }
        protected override void GengGai(object sender, DataGridViewRow row, int lie, bool isshanchu)
        {
            if (isshanchu == false)
            {
                if (lie == 5)
                {
                    this.XianShiPanl(true);
                    if (row.Tag is DuIOCanShuModel)
                    {
                        DuIOCanShuModel model = row.Tag as DuIOCanShuModel;
                        this.textBox3.Text = model.Name;
                        this.comboBox2.Text = model.Type.ToString();
                        this.commBoxE1.Text = model.PanDuanFangShi.ToString();
                        this.textBox1.Text=model.MiaShu.ToString();
                    }
                }
                else if (lie == 6)
                {
                    if (row.Tag is DuIOCanShuModel)
                    {
                        DuIOCanShuModel model = row.Tag as DuIOCanShuModel;
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
                else if (lie == 4)
                {
                    if (row.Tag is DuIOCanShuModel)
                    {
                        DuIOCanShuModel model = row.Tag as DuIOCanShuModel;
                        string id = model.Name;
                        for (int i = 0; i < ShuJu.Count; i++)
                        {
                            if (ShuJu[i].Name == id)
                            {
                                DuJiCunQiFrm frm = new DuJiCunQiFrm();
                                frm.SetCanShu( ShuJu[i].LisJiCunQi,2);
                                if (frm.ShowDialog(this) == DialogResult.OK)
                                {

                                    ShuJu[i].LisJiCunQi =ChangYong.FuZhiShiTi( frm.GetCanShu());
                                }
                                break;
                            }
                        }

                    }

                }
            }
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

                    ShuJu[i].PanDuanFangShi = ChangYong.GetMeiJuZhi<PanDuanType>(this.commBoxE1.Text);
                    ShuJu[i].Type =ChangYong.GetMeiJuZhi<IOType>( this.comboBox2.Text);
                    ShuJu[i].Name = this.textBox3.Text;
                    ShuJu[i].MiaShu = ChangYong.TryDouble(this.textBox1.Text,0);
                    break;
                }
            }
            if (iscunzai == false)
            {
                DuIOCanShuModel dumodel = new DuIOCanShuModel();


                dumodel.PanDuanFangShi = ChangYong.GetMeiJuZhi<PanDuanType>(this.commBoxE1.Text);
                dumodel.Type = ChangYong.GetMeiJuZhi<IOType>(this.comboBox2.Text);
                dumodel.Name = this.textBox3.Text;
                dumodel.MiaShu = ChangYong.TryDouble(this.textBox1.Text, 0);
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
                    List<DuIOCanShuModel> lis = new List<DuIOCanShuModel>();
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
                            DuIOCanShuModel xinmodel = new DuIOCanShuModel();
                            xinmodel.Name = name;
                            xinmodel.PanDuanFangShi = ChangYong.GetMeiJuZhi<PanDuanType>(ChangYong.TryStr(dt.Rows[i]["PanDuanFangShi"], ""));
                            xinmodel.Type=ChangYong.GetMeiJuZhi<IOType>( ChangYong.TryStr(dt.Rows[i]["Type"], "IOData"));
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
            jieguo.Add("PanDuanFangShi", new List<object>());
            jieguo.Add("WeiYiBiaoShi", new List<object>());
            jieguo.Add("IOType", new List<object>());
            jieguo.Add("PiPeiType", new List<object>());
            jieguo.Add("PiPeiValue", new List<object>());
            jieguo.Add("SheBeiID", new List<object>());
            List<DuIOCanShuModel> xieSates = GetCanShu();
            for (int c = 0; c < xieSates.Count; c++)
            {
                for (int i = 0; i < xieSates[c].LisJiCunQi.Count; i++)
                {
                    jieguo["Name"].Add(xieSates[c].Name);
                    jieguo["Type"].Add(xieSates[c].Type);
                    jieguo["PanDuanFangShi"].Add(xieSates[c].PanDuanFangShi);
                    DuModel models = xieSates[c].LisJiCunQi[i];
                    jieguo["WeiYiBiaoShi"].Add(models.JiCunQiName);
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
            model.JiCunQiName = ChangYong.TryStr(row["WeiYiBiaoShi"], "");
            model.Type = ChangYong.TryStr(row["IOType"], "");
            model.PiPeiType = ChangYong.GetMeiJuZhi<PiPeiType>(ChangYong.TryStr(row["PiPeiType"], ""));
            model.PiPeiValue = ChangYong.TryStr(row["PiPeiValue"], "");
            model.SheBeiID = ChangYong.TryInt(ChangYong.TryStr(row["SheBeiID"], ""),0);
            return model;
        }
    }
}
