using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using CommLei.DataChuLi;
using CommLei.JiChuLei;
using LeiSaiDMC.Frm.KJ;
using LeiSaiDMC.Model;
using SSheBei.CRCJiaoYan;
using SSheBei.Model;

namespace LeiSaiDMC.Frm
{
    public  class DataMoXing
    {
        ///// <summary>
        ///// 设备id
        ///// </summary>
        //public int SheBeiID { get; set; }

        ///// <summary>
        ///// 设备名称
        ///// </summary>
        //public string SheBeiName { get; set; } = "";
        ///// <summary>
        ///// 读寄存器
        ///// </summary>
        //public List<JiCunQiModel> LisDu = new List<JiCunQiModel>();
        ///// <summary>
        ///// 写寄存器
        ///// </summary>
        //public List<JiCunQiModel> LisXie = new List<JiCunQiModel>();

        ///// <summary>
        ///// 设备
        ///// </summary>
        //public List<LSModel> LisSheBei { get; set; } = new List<LSModel>();

        //public List<XinJiCunModel> LisJiCunQi { get; set; } = new List<XinJiCunModel>();

        ///// <summary>
        ///// 用于初始化
        ///// </summary>
        //public void IniData(string lujing)
        //{
        //    LisDu.Clear();
        //    LisXie.Clear();
        //    LisSheBei.Clear();
        //    LisJiCunQi.Clear();
        //    JosnOrSModel sosnOrSModel = new JosnOrSModel(lujing);

        //    LisSheBei = sosnOrSModel.GetLisTModel<LSModel>();
        //    if (LisSheBei == null)
        //    {
        //        LisSheBei = new List<LSModel>();
        //    }
        //    for (int c = 0; c < LisSheBei.Count; c++)
        //    {
        //        LSModel shebei = LisSheBei[c];
        //        shebei.ZuiDaDuIO = 0;
        //        shebei.ZuiDaXieIO = 0;
        //        for (int d = 0; d < shebei.LisKaModels.Count; d++)
        //        {
        //            KaCanShuModel kamodel = shebei.LisKaModels[d];                  
        //            if (kamodel.IsZhou)
        //            {
        //                List<string> meijus = ChangYong.MeiJuLisName(typeof(CanShuType));
        //                for (int i = 0; i < meijus.Count; i++)
        //                {
        //                    CanShuType canshu = ChangYong.GetMeiJuZhi<CanShuType>(meijus[i]);
        //                    switch (canshu)
        //                    {
        //                        case CanShuType.Du位置:
        //                            {
        //                                JiCunQiModel model = new JiCunQiModel();
        //                                model.WeiYiBiaoShi = $"{kamodel.KaName}Du位置";
        //                                model.DuXie = true;
        //                                model.MiaoSu =CRC.GetMiaoSu("无参数","有最小值","有最大值","mm","比较大小","读位置数据");
        //                                model.SheBeiID = SheBeiID;
        //                                XinJiCunModel xinjimodel = new XinJiCunModel();
        //                                xinjimodel.CanShuType = canshu;
        //                                xinjimodel.SheBeiID = shebei.SheBeiID;
        //                                xinjimodel.ZhouPeiZhiID = kamodel.ZhouPeiZhiID;
        //                                xinjimodel.JiCunQiModel = model;
        //                                LisJiCunQi.Add(xinjimodel);
        //                                LisDu.Add(model);
        //                            }
        //                            break;
        //                        case CanShuType.Du速度:
        //                            {
        //                                JiCunQiModel model = new JiCunQiModel();
        //                                model.WeiYiBiaoShi = $"{kamodel.KaName}Du速度";
        //                                model.DuXie = true;
        //                                model.MiaoSu = CRC.GetMiaoSu("是double参数", "有最小值", "有最大值", "mm", "比较大小", "读速度数据");
        //                                model.SheBeiID = SheBeiID;
        //                                XinJiCunModel xinjimodel = new XinJiCunModel();
        //                                xinjimodel.CanShuType = canshu;
        //                                xinjimodel.SheBeiID = shebei.SheBeiID;
        //                                xinjimodel.ZhouPeiZhiID = kamodel.ZhouPeiZhiID;
        //                                xinjimodel.JiCunQiModel = model;
        //                                LisJiCunQi.Add(xinjimodel);
        //                                LisDu.Add(model);
        //                            }
        //                            break;
        //                        case CanShuType.Du使能:
        //                            {
        //                                JiCunQiModel model = new JiCunQiModel();
        //                                model.WeiYiBiaoShi = $"{kamodel.KaName}Du使能";
        //                                model.DuXie = true;
        //                                model.MiaoSu = CRC.GetMiaoSu("0或者1", "0到1", "0到1", "", "比较大小", "读使能");
        //                                model.SheBeiID = SheBeiID;
        //                                XinJiCunModel xinjimodel = new XinJiCunModel();
        //                                xinjimodel.CanShuType = canshu;
        //                                xinjimodel.SheBeiID = shebei.SheBeiID;
        //                                xinjimodel.ZhouPeiZhiID = kamodel.ZhouPeiZhiID;
        //                                xinjimodel.JiCunQiModel = model;
        //                                LisJiCunQi.Add(xinjimodel);
        //                                LisDu.Add(model);
        //                            }
        //                            break;
        //                        case CanShuType.Du轴状态:
        //                            {
        //                                JiCunQiModel model = new JiCunQiModel();
        //                                model.WeiYiBiaoShi = $"{kamodel.KaName}Du轴状态";
        //                                model.DuXie = true;
        //                                model.MiaoSu = CRC.GetMiaoSu("0到7", "0到7", "0到7", "", "比较大小", "0表示轴未启动 1表示启动禁止状态 等");
        //                                model.SheBeiID = SheBeiID;
        //                                XinJiCunModel xinjimodel = new XinJiCunModel();
        //                                xinjimodel.CanShuType = canshu;
        //                                xinjimodel.SheBeiID = shebei.SheBeiID;
        //                                xinjimodel.ZhouPeiZhiID = kamodel.ZhouPeiZhiID;
        //                                xinjimodel.JiCunQiModel = model;
        //                                LisJiCunQi.Add(xinjimodel);
        //                                LisDu.Add(model);
        //                            }
        //                            break;
        //                        case CanShuType.Du运动状态:
        //                            {
        //                                JiCunQiModel model = new JiCunQiModel();
        //                                model.WeiYiBiaoShi = $"{kamodel.KaName}Du运动状态";
        //                                model.DuXie = true;
        //                                model.MiaoSu = CRC.GetMiaoSu("0或者1", "0到1", "0到1", "", "比较大小", "0表示停止 1表示运动");
        //                                model.SheBeiID = SheBeiID;
        //                                XinJiCunModel xinjimodel = new XinJiCunModel();
        //                                xinjimodel.CanShuType = canshu;
        //                                xinjimodel.SheBeiID = shebei.SheBeiID;
        //                                xinjimodel.ZhouPeiZhiID = kamodel.ZhouPeiZhiID;
        //                                xinjimodel.JiCunQiModel = model;
        //                                LisJiCunQi.Add(xinjimodel);
        //                                LisDu.Add(model);
        //                            }
        //                            break;
        //                        case CanShuType.Du超限报警:
        //                            {
        //                                JiCunQiModel model = new JiCunQiModel();
        //                                model.WeiYiBiaoShi = $"{kamodel.KaName}Du运动状态";
        //                                model.DuXie = true;
        //                                model.MiaoSu = CRC.GetMiaoSu("0到1", "0到1", "0到1", "", "比较大小", "0表示未超限 1表示超限报警");
        //                                model.SheBeiID = SheBeiID;
        //                                XinJiCunModel xinjimodel = new XinJiCunModel();
        //                                xinjimodel.CanShuType = canshu;
        //                                xinjimodel.SheBeiID = shebei.SheBeiID;
        //                                xinjimodel.ZhouPeiZhiID = kamodel.ZhouPeiZhiID;
        //                                xinjimodel.JiCunQiModel = model;
        //                                LisJiCunQi.Add(xinjimodel);
        //                                LisDu.Add(model);
        //                            }
        //                            break;                              
        //                        case CanShuType.XieZhouX位置:
        //                            {
        //                                JiCunQiModel model = new JiCunQiModel();
        //                                model.WeiYiBiaoShi = $"{kamodel.KaName}XieZhouX位置";
        //                                model.DuXie = false;
        //                                model.MiaoSu = CRC.GetMiaoSu("double参数", "--", "--", "mm", "比较大小", "写轴相对位置");
        //                                model.SheBeiID = SheBeiID;
        //                                XinJiCunModel xinjimodel = new XinJiCunModel();
        //                                xinjimodel.CanShuType = canshu;
        //                                xinjimodel.SheBeiID = shebei.SheBeiID;
        //                                xinjimodel.ZhouPeiZhiID = kamodel.ZhouPeiZhiID;
        //                                xinjimodel.JiCunQiModel = model;
        //                                LisJiCunQi.Add(xinjimodel);
        //                                LisXie.Add(model);
        //                            }
        //                            break;
        //                        case CanShuType.XieZhouJ位置:
        //                            {
        //                                JiCunQiModel model = new JiCunQiModel();
        //                                model.WeiYiBiaoShi = $"{kamodel.KaName}XieZhouJ位置";
        //                                model.DuXie = false;
        //                                model.MiaoSu = CRC.GetMiaoSu("double参数", "--", "--", "mm", "比较大小", "写轴绝对位置");
        //                                model.SheBeiID = SheBeiID;
        //                                XinJiCunModel xinjimodel = new XinJiCunModel();
        //                                xinjimodel.CanShuType = canshu;
        //                                xinjimodel.SheBeiID = shebei.SheBeiID;
        //                                xinjimodel.ZhouPeiZhiID = kamodel.ZhouPeiZhiID;
        //                                xinjimodel.JiCunQiModel = model;
        //                                LisJiCunQi.Add(xinjimodel);
        //                                LisXie.Add(model);
        //                            }
        //                            break;
        //                        case CanShuType.XieZhou速度:
        //                            {
        //                                JiCunQiModel model = new JiCunQiModel();
        //                                model.WeiYiBiaoShi = $"{kamodel.KaName}XieZhou速度";
        //                                model.DuXie = false;
        //                                model.MiaoSu = CRC.GetMiaoSu("double参数", "--", "--", "mm/s", "比较大小", "写轴速度");
        //                                model.SheBeiID = SheBeiID;
        //                                XinJiCunModel xinjimodel = new XinJiCunModel();
        //                                xinjimodel.CanShuType = canshu;
        //                                xinjimodel.SheBeiID = shebei.SheBeiID;
        //                                xinjimodel.ZhouPeiZhiID = kamodel.ZhouPeiZhiID;
        //                                xinjimodel.JiCunQiModel = model;
        //                                LisJiCunQi.Add(xinjimodel);
        //                                LisXie.Add(model);
        //                            }
        //                            break;
        //                        case CanShuType.XieZhou恒速:
        //                            {
        //                                JiCunQiModel model = new JiCunQiModel();
        //                                model.WeiYiBiaoShi = $"{kamodel.KaName}XieZhou恒速";
        //                                model.DuXie = false;
        //                                model.MiaoSu = CRC.GetMiaoSu("double参数", "--", "--", "mm/s", "比较大小", "写轴恒速");
        //                                model.SheBeiID = SheBeiID;
        //                                XinJiCunModel xinjimodel = new XinJiCunModel();
        //                                xinjimodel.CanShuType = canshu;
        //                                xinjimodel.SheBeiID = shebei.SheBeiID;
        //                                xinjimodel.ZhouPeiZhiID = kamodel.ZhouPeiZhiID;
        //                                xinjimodel.JiCunQiModel = model;
        //                                LisJiCunQi.Add(xinjimodel);
        //                                LisXie.Add(model);
        //                            }
        //                            break;
        //                        case CanShuType.XieZhou使能:
        //                            {
        //                                JiCunQiModel model = new JiCunQiModel();
        //                                model.WeiYiBiaoShi = $"{kamodel.KaName}XieZhou使能";
        //                                model.DuXie = false;
        //                                model.MiaoSu = CRC.GetMiaoSu("0和1", "--", "--", "", "比较大小", "0是失能 1是使能");
        //                                model.SheBeiID = SheBeiID;
        //                                XinJiCunModel xinjimodel = new XinJiCunModel();
        //                                xinjimodel.CanShuType = canshu;
        //                                xinjimodel.SheBeiID = shebei.SheBeiID;
        //                                xinjimodel.ZhouPeiZhiID = kamodel.ZhouPeiZhiID;
        //                                xinjimodel.JiCunQiModel = model;
        //                                LisJiCunQi.Add(xinjimodel);
        //                                LisXie.Add(model);
        //                            }
        //                            break;
        //                        case CanShuType.XieZhou报警清除:
        //                            {
        //                                JiCunQiModel model = new JiCunQiModel();
        //                                model.WeiYiBiaoShi = $"{kamodel.KaName}XieZhou报警清除";
        //                                model.DuXie = false;
        //                                model.MiaoSu = CRC.GetMiaoSu("无参数", "--", "--", "", "--", "报警清除");
        //                                model.SheBeiID = SheBeiID;
        //                                XinJiCunModel xinjimodel = new XinJiCunModel();
        //                                xinjimodel.CanShuType = canshu;
        //                                xinjimodel.SheBeiID = shebei.SheBeiID;
        //                                xinjimodel.ZhouPeiZhiID = kamodel.ZhouPeiZhiID;
        //                                xinjimodel.JiCunQiModel = model;
        //                                LisJiCunQi.Add(xinjimodel);
        //                                LisXie.Add(model);
        //                            }
        //                            break;
        //                        case CanShuType.XieZhou位置清零:
        //                            {
        //                                JiCunQiModel model = new JiCunQiModel();
        //                                model.WeiYiBiaoShi = $"{kamodel.KaName}XieZhou位置清零";
        //                                model.DuXie = false;
        //                                model.MiaoSu = CRC.GetMiaoSu("无参数", "--", "--", "", "--", "报警清除");
        //                                model.SheBeiID = SheBeiID;
        //                                XinJiCunModel xinjimodel = new XinJiCunModel();
        //                                xinjimodel.CanShuType = canshu;
        //                                xinjimodel.SheBeiID = shebei.SheBeiID;
        //                                xinjimodel.ZhouPeiZhiID = kamodel.ZhouPeiZhiID;
        //                                xinjimodel.JiCunQiModel = model;
        //                                LisJiCunQi.Add(xinjimodel);
        //                                LisXie.Add(model);
        //                            }
        //                            break;
        //                        case CanShuType.XieZhou停止:
        //                            {
        //                                JiCunQiModel model = new JiCunQiModel();
        //                                model.WeiYiBiaoShi = $"{kamodel.KaName}XieZhou停止";
        //                                model.DuXie = false;
        //                                model.MiaoSu = CRC.GetMiaoSu("无参数", "--", "--", "", "--", "报警清除");
        //                                model.SheBeiID = SheBeiID;
        //                                XinJiCunModel xinjimodel = new XinJiCunModel();
        //                                xinjimodel.CanShuType = canshu;
        //                                xinjimodel.SheBeiID = shebei.SheBeiID;
        //                                xinjimodel.ZhouPeiZhiID = kamodel.ZhouPeiZhiID;
        //                                xinjimodel.JiCunQiModel = model;
        //                                LisJiCunQi.Add(xinjimodel);
        //                                LisXie.Add(model);
        //                            }
        //                            break;
        //                        case CanShuType.XieZhou回零:
        //                            {
        //                                JiCunQiModel model = new JiCunQiModel();
        //                                model.WeiYiBiaoShi = $"{kamodel.KaName}XieZhou回零";
        //                                model.DuXie = false;
        //                                model.MiaoSu = CRC.GetMiaoSu("无参数", "--", "--", "", "--", "报警清除");
        //                                model.SheBeiID = SheBeiID;
        //                                XinJiCunModel xinjimodel = new XinJiCunModel();
        //                                xinjimodel.CanShuType = canshu;
        //                                xinjimodel.SheBeiID = shebei.SheBeiID;
        //                                xinjimodel.ZhouPeiZhiID = kamodel.ZhouPeiZhiID;
        //                                xinjimodel.JiCunQiModel = model;
        //                                LisJiCunQi.Add(xinjimodel);
        //                                LisXie.Add(model);
        //                            }
        //                            break;                                                         
        //                        default:
        //                            break;
        //                    }
        //                }
                      
        //            }
        //            else
        //            {
        //                JiCunQiModel model = new JiCunQiModel();
        //                model.WeiYiBiaoShi = kamodel.KaName;
        //                model.SheBeiID = SheBeiID;

        //                if (kamodel.IsXieIO == false)
        //                {
        //                    model.DuXie = true;
        //                    model.MiaoSu = "该寄存器是读:0:1:无单位:采用比较关系";
        //                    XinJiCunModel xinjimodel = new XinJiCunModel();
        //                    xinjimodel.CanShuType = CanShuType.DuIO;
        //                    xinjimodel.SheBeiID = shebei.SheBeiID;
        //                    xinjimodel.ZhouPeiZhiID = kamodel.ZhouPeiZhiID;
        //                    xinjimodel.JiCunQiModel = model;
        //                    LisJiCunQi.Add(xinjimodel);
        //                    LisDu.Add(model);
        //                    if (shebei.ZuiDaDuIO < kamodel.BitNoOrZhouHao)
        //                    {
        //                        shebei.ZuiDaDuIO = kamodel.BitNoOrZhouHao;
        //                    }
        //                }
        //                else
        //                {
        //                    model.MiaoSu = "该寄存器是读写:0:1:无单位:采用比较关系";
        //                    model.DuXie = false;
                           
        //                    model.MiaoSu = "该寄存器是读:0:1:无单位:采用比较关系";
        //                    XinJiCunModel xinjimodel = new XinJiCunModel();
        //                    xinjimodel.CanShuType = CanShuType.DuIO;
        //                    xinjimodel.SheBeiID = shebei.SheBeiID;
        //                    xinjimodel.ZhouPeiZhiID = kamodel.ZhouPeiZhiID;
        //                    xinjimodel.JiCunQiModel = model;
        //                    LisJiCunQi.Add(xinjimodel);
        //                    LisXie.Add(model);
        //                    if (shebei.ZuiDaXieIO < kamodel.BitNoOrZhouHao)
        //                    {
        //                        shebei.ZuiDaXieIO = kamodel.BitNoOrZhouHao;
        //                    }
                          
        //                }
                      

        //            }

        //        }

        //    }
       
        //    //全局寄存器
        //    {
        //        {
        //            JiCunQiModel mjicunqimode = new JiCunQiModel();
        //            mjicunqimode.WeiYiBiaoShi = $"读{DuCanShuType.总线状态}";
        //            mjicunqimode.SheBeiID = SheBeiID;
        //            mjicunqimode.MiaoSu = $"该寄存器是读写:0:1:无单位:采用比较关系 1表示总线良好";
        //            mjicunqimode.DuXie = true;
        //            QiTaJiCunQiDu.Add(mjicunqimode);
        //            if (JiCunQiDuType.ContainsKey(mjicunqimode.WeiYiBiaoShi) == false)
        //            {
        //                JiCunQiDuType.Add(mjicunqimode.WeiYiBiaoShi, DuCanShuType.总线状态);
        //            }
        //        }
        //    }
        //    //全局写寄存器
        //    {

        //        {
        //            JiCunQiModel mjicunqimode = new JiCunQiModel();
        //            mjicunqimode.WeiYiBiaoShi = "写热复位";
        //            mjicunqimode.SheBeiID = SheBeiID;
        //            mjicunqimode.MiaoSu = $"该寄存器是写:0:1:无单位:";
        //            mjicunqimode.DuXie = false;
        //            QiTaJiCunQiXie.Add(mjicunqimode);
        //            if (JiCunQiType.ContainsKey(mjicunqimode.WeiYiBiaoShi) == false)
        //            {
        //                JiCunQiType.Add(mjicunqimode.WeiYiBiaoShi, XieCaoZuoType.热复位);
        //            }
        //        }
        //        {
        //            JiCunQiModel mjicunqimode = new JiCunQiModel();
        //            mjicunqimode.WeiYiBiaoShi = "写所有轴停止";
        //            mjicunqimode.SheBeiID = SheBeiID;
        //            mjicunqimode.MiaoSu = $"该寄存器是写:0:1:无单位:";
        //            mjicunqimode.DuXie = false;
        //            QiTaJiCunQiXie.Add(mjicunqimode);
        //            if (JiCunQiType.ContainsKey(mjicunqimode.WeiYiBiaoShi) == false)
        //            {
        //                JiCunQiType.Add(mjicunqimode.WeiYiBiaoShi, XieCaoZuoType.所有轴停止);
        //            }
        //        }
        //        {
        //            JiCunQiModel mjicunqimode = new JiCunQiModel();
        //            mjicunqimode.WeiYiBiaoShi = "写所有轴回零";
        //            mjicunqimode.SheBeiID = SheBeiID;
        //            mjicunqimode.MiaoSu = $"该寄存器是写:0:1:无单位:所有轴回零";
        //            mjicunqimode.DuXie = false;
        //            QiTaJiCunQiXie.Add(mjicunqimode);
        //            if (JiCunQiType.ContainsKey(mjicunqimode.WeiYiBiaoShi) == false)
        //            {
        //                JiCunQiType.Add(mjicunqimode.WeiYiBiaoShi, XieCaoZuoType.所有轴回零);
        //            }
        //        }
        //    }
        //}

        //public void SetTxBuHeGe(int zongid)
        //{
        //    for (int i = 0; i < LisSheBei.Count; i++)
        //    {
        //        if (LisSheBei[i].SheBeiID == zongid)
        //        {
        //            SheBeiModel cunModel = LisSheBei[i];
        //            cunModel.Tx = false;
        //            for (int c = 0; c < cunModel.DataCunModels.Count; c++)
        //            {
        //                cunModel.DataCunModels[c].JiCunQiModel.IsKeKao = false;
        //            }
        //            break;
        //        }
        //    }


        //}

 
    }
}
