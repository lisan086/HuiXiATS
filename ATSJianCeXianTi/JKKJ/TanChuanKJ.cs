using AdvancedDataGridView;
using ATSJianCeXianTi.JKKJ.PeiZhiKJ;
using ATSJianCeXianTi.Lei;
using ATSJianCeXianTi.Model;
using ATSJianCeXianTi.PeiFangFrm;
using ATSJianMianJK;
using ATSJianMianJK.XiTong.Model;
using BaseUI.UC;
using CommLei.JiChuLei;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.AxHost;


namespace ATSJianCeXianTi.JKKJ
{
    public partial class TanChuanKJ : UserControl
    {
        private TDModel TDModel;
        /// <summary>
        /// 通道ID
        /// </summary>
        private int TdID = -1;

        private TDUIKJ TDUIKJ;
        private TangChaunKJ TangChaunKJ;
     
        public TanChuanKJ()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            SetKJ();
        }

        private void SetKJ()
        {
            {
                TDUIKJ = new TDUIKJ();
                TDUIKJ.Dock = DockStyle.Fill;
                this.panel1.Controls.Add(TDUIKJ);
            }
            {
                TangChaunKJ=new TangChaunKJ();
                TangChaunKJ.Dock = DockStyle.Fill;
                TangChaunKJ.Visible = false;
                this.panel1.Controls.Add(TangChaunKJ);
            }
        }

        public void IniData(TDModel tDModel)
        {
            TdID = tDModel.TDID;
            TDModel=tDModel;
            TDUIKJ.IniData(tDModel);
            TangChaunKJ.SetCanShu(tDModel);
        }

        public void IniData(int tdid, ZiYuanModel ziyuan)
        {
            if (tdid == TdID)
            {
                TDUIKJ.IniData(tdid, ziyuan);
                TangChaunKJ.SetCanShu(tdid, ziyuan);
            }
        }

        public void SetTanChaung(TangChuanUIModel canshu,int type,int tdid)
        {
            if (TdID==tdid)
            {
                if (type != 2)
                {
                    TDUIKJ.Visible = false;
                    TangChaunKJ.Visible = true;
                    TangChaunKJ.SetXianShi(true, canshu.Type);
                    TangChaunKJ.SetCanShu(tdid, canshu);

                }
                else 
                {
                    TangChaunKJ.SetXianShi(false,-1);
                    TDUIKJ.Visible = true;
                    TangChaunKJ.Visible = false;
                }
            }
        }
      
       

        /// <summary>
        /// 设置开关参数
        /// </summary>
        /// <param name="dModel"></param>
        /// <param name="tdid"></param>
        public void ShuXin()
        {
         
            this.tdZhuangTaiKJ1.TiaoMa = TDModel.FuWeiCanShu.MaZhi;
         
            if (TDModel.GanYingModel.IsJiTing)
            {
                this.tdZhuangTaiKJ1.MiaoSu = "急停中";

            }
            else
            {
                if (TDModel.ShouDongCanShu.IsZhanTing)
                {
                    this.tdZhuangTaiKJ1.MiaoSu = "暂停中";
                }
                else
                {
                    if (this.tdZhuangTaiKJ1.MiaoSu.Contains("暂停中"))
                    {
                        this.tdZhuangTaiKJ1.MiaoSu = this.tdZhuangTaiKJ1.MiaoSu.Replace("暂停中", "");
                    }
                }
            }
            this.tdZhuangTaiKJ1.JiTime = $"{TDModel.FuWeiCanShu.GetTestMiao().ToString("0.000")}";
            this.tdZhuangTaiKJ1.KaiShiHua();
            TDUIKJ.ShuaXin(TdID);
        }

        /// <summary>
        /// 设置测试项目
        /// </summary>
        /// <param name="tdid"></param>
        /// <param name="model"></param>
        public void SetTestXiangMu(int tdid, XiangMuModel model)
        {
            if (TdID == tdid)
            {            
                switch (model.BuZhouType)
                {
                    case BuZhouType.ZhunBeiJianCe:
                        {
                          
                            this.tdZhuangTaiKJ1.JiTime = "0";
                           
                            this.tdZhuangTaiKJ1.IsHeGe = 0;
                        }
                        break;
                    case BuZhouType.KaiShiJianCe:
                        {
                            this.tdZhuangTaiKJ1.MiaoSu = $"{model.DiLanMiaoSu}";
                            this.tdZhuangTaiKJ1.IsHeGe = 0;
                          
                        }
                        break;
                    case BuZhouType.DXiangMuJianCe:
                        {                        
                            this.tdZhuangTaiKJ1.MiaoSu = $"{model.DiLanMiaoSu}";
                         
                        }
                        break;
                    case BuZhouType.DXiangMuJieSu:
                        {
                            if (model.IsZongXiang)
                            {
                                this.tdZhuangTaiKJ1.MiaoSu = $"{model.DiLanMiaoSu}";
                                this.tdZhuangTaiKJ1.BiLi = (float)model.ZhiXingBaiFenBi;
                             
                            }
                           
                        }
                        break;
                    case BuZhouType.ZongJieSu:
                        {
                            if (tdid==1)
                            { 

                            }
                            this.tdZhuangTaiKJ1.MiaoSu = $"{model.DiLanMiaoSu}";
                        
                            if (model.ZongJieGuo == false)
                            {
                                this.tdZhuangTaiKJ1.IsHeGe = 2;
                                if (model.BuHeGeXiangMu.Count > 0)
                                {
                                    this.tdZhuangTaiKJ1.MiaoSu = $"结束检测:不合格项目:{ChangYong.FenGeDaBao<string>(model.BuHeGeXiangMu, ",")}";
                                
                                }
                            }
                            else
                            {
                                this.tdZhuangTaiKJ1.IsHeGe = 1;
                              
                            }
                           
                           
                        }
                        break;
                    
                    default:
                        break;
                }
                this.TDUIKJ.SetTestXiangMu(tdid, model, this.tdZhuangTaiKJ1.JiTime.ToString());
            }
        }

      

        public void JiaZaiPeiFang()
        {
            TDUIKJ.JiaZaiPeiFang();
          
        }

     

    }
}
