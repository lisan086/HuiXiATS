using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ATSJianMianJK.GongNengLei;
using ATSJianMianJK.Log;
using CommLei.JiChuLei;
using ZuZhuangUI.Mes;
using ZuZhuangUI.Model;

namespace ZuZhuangUI.Lei.GongNengLei.ShiXian
{
    public class GongWeiLei : ABSGongNengLei
    {
        private bool ZongKaiGuan = true;
        private SheBeiZhanModel SheBeiZhanModel;
        private bool IsShouZhan = false;
        public override void IniData(SheBeiZhanModel model)
        {
            ZongKaiGuan = true;
            SheBeiZhanModel = model;
            IsShouZhan = model.IsShouZhan == 1;
            Thread thread = new Thread(Work);
            thread.IsBackground = true;
            thread.DisableComObjectEagerCleanup();
            thread.Start();
        }

        private void Work()
        {
            if (SheBeiID<0)
            {
                return;
            }
          
          
            DateTime jinzhantime = DateTime.Now;
            DateTime chuzhantime = DateTime.Now;
       
            while (ZongKaiGuan)
            {
              
                //处理进站

                {
                    try
                    {

                        bool jinzhanzhi = DataJiHe.Cerate().GetGWPiPei(SheBeiID, CaoZuoType.JinZhanQingQiu);
                        if (jinzhanzhi)
                        {
                            if ((DateTime.Now - jinzhantime).TotalMilliseconds >= 500)
                            {
                                WriteLog(RiJiEnum.TDLiuCheng, $"收到PLC进站请求,准备校验");
                                Thread.Sleep(500);
                                QiTaJinZhan();
                                WriteLog(RiJiEnum.TDLiuCheng, $"进站结束");
                                SheBeiJiHe.Cerate().XieRuDanData(CaoZuoType.JinZhanQingQiu,SheBeiID,1);
                                jinzhantime = DateTime.Now;
                            }

                        }
                        else
                        {
                            jinzhantime = DateTime.Now;
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLog(RiJiEnum.TDLiuChengRed, $"进站{ex}");
                    }
                }

                //处理出站
                {
                    try
                    {

                        bool jinzhanzhi = DataJiHe.Cerate().GetGWPiPei(SheBeiID, CaoZuoType.ChuZhanQingQiu);
                        if (jinzhanzhi)
                        {
                            if ((DateTime.Now - chuzhantime).TotalMilliseconds >= 500)
                            {
                                WriteLog(RiJiEnum.TDLiuCheng, $"收到PLC出站请求,准备上传数据");
                                Thread.Sleep(500);
                                ChuZhanJiaoYan();
                                WriteLog(RiJiEnum.TDLiuCheng, $"出站结束");
                                SheBeiJiHe.Cerate().XieRuDanData(CaoZuoType.ChuZhanQingQiu, SheBeiID, 1);
                             
                                chuzhantime = DateTime.Now;
                            }

                        }
                        else
                        {
                            chuzhantime = DateTime.Now;
                        }
                    }
                    catch (Exception ex)
                    {

                        WriteLog(RiJiEnum.TDLiuChengRed, $"出站{ex}");
                    }
                }
                Thread.Sleep(50);
            }
        }

      
        public override void Close()
        {
            ZongKaiGuan = false;
            Thread.Sleep(15);
        }

        private void QiTaJinZhan()
        {
            string mesma = DataJiHe.Cerate().GetGWZhiStr(SheBeiID, CaoZuoType.JinZhanErWeiMa, "");
            bool ishege = true;
            if (string.IsNullOrEmpty(mesma))
            {
                WriteLog(RiJiEnum.TDLiuChengRed, $"读到的码为空，进站失败");
                JieMianShiJianModel modess = new JieMianShiJianModel();
                modess.GWID = SheBeiID;
                modess.JieGuo = false;
                modess.Value = $"{mesma}:读到的码为空，进站失败";
                JieMianCaoZuoLei.CerateDanLi().ChuFaJieMianEvent(EventType.JinZhanErWeiMa, modess);
                ishege = false;
            }
            else
            {
                JieMianShiJianModel modess = new JieMianShiJianModel();
                modess.GWID = SheBeiID;
                modess.JieGuo = true;
                modess.Value= mesma;
                JieMianCaoZuoLei.CerateDanLi().ChuFaJieMianEvent(EventType.JinZhanErWeiMa, modess);
                WriteLog(RiJiEnum.TDLiuCheng, $"读到进站二维码:{mesma}");
            }
            if (ishege)
            {
                bool jieguo=  ShangMes(1, mesma, true);
                if (jieguo==false)
                {
                    ishege = false;
                }
            }
            if (ishege)
            {
                SheBeiJiHe.Cerate().XieRuDanData(CaoZuoType.JinZhanDuJieGuo, SheBeiID, 2);
                SheBeiJiHe.Cerate().XieRuDanData(CaoZuoType.JinZhanWanCheng, SheBeiID, 2);           
                WriteLog(RiJiEnum.TDLiuCheng, $"mes校验合格，写合格结果，成功入站:{mesma}");
            }
            else
            {
                SheBeiJiHe.Cerate().XieRuDanData(CaoZuoType.JinZhanDuJieGuo, SheBeiID, 3);
                SheBeiJiHe.Cerate().XieRuDanData(CaoZuoType.JinZhanWanCheng, SheBeiID, 3);
                WriteLog(RiJiEnum.TDLiuCheng, $"mes校验合格，写NG结果，NG入站:{mesma}");
            }

        }

        private void ChuZhanJiaoYan()
        {
            string mesma = DataJiHe.Cerate().GetGWZhiStr(SheBeiID, CaoZuoType.ChuZhanErWeiMa, "");
            bool ishege = true;
            if (string.IsNullOrEmpty(mesma))
            {
                WriteLog(RiJiEnum.TDLiuChengRed, $"读到的码为空，出站失败");
                JieMianShiJianModel modess = new JieMianShiJianModel();
                modess.GWID = SheBeiID;
                modess.JieGuo = false;
                modess.Value = $"{mesma}:读到的码为空，出站失败";
                JieMianCaoZuoLei.CerateDanLi().ChuFaJieMianEvent(EventType.ChuZhanErWeiMa, modess);
                ishege = false;
            }
            else
            {
                JieMianShiJianModel modess = new JieMianShiJianModel();
                modess.GWID = SheBeiID;
                modess.JieGuo = true;
                modess.Value = mesma;
                JieMianCaoZuoLei.CerateDanLi().ChuFaJieMianEvent(EventType.ChuZhanErWeiMa, modess);
                WriteLog(RiJiEnum.TDLiuCheng, $"读到进站二维码:{mesma}");
            }
            if (ishege)
            {
                WriteLog(RiJiEnum.TDLiuCheng, $"出站开始上传数据");
                List<YeWuDataModel> lise = DataJiHe.Cerate().GetDataModel(SheBeiID,CaoZuoType.DataShangChuan,true);
                for (int i = 0; i < lise.Count; i++)
                {
                    if (lise[i].IsShangChuan==1)
                    {
                        lise[i].IsShangChuanHeGe = true;
                        lise[i].IsShuJuHeGe = JiaoYanHeGe(lise[i]);
                        bool she = ShangMes(2, mesma,true, lise[i]);
                        if (SheBeiZhanModel.IsQuanBuShangChuan!=1)
                        {
                            if (she == false || lise[i].IsShuJuHeGe)
                            {
                                ishege = false;
                                break;
                            }
                        }
                        
                    }
                }
                bool ischuzhan = ShangMes(3, mesma, ishege);
                if (ischuzhan == false)
                {
                    ishege = ischuzhan;
                }
                JieMianShiJianModel models = new JieMianShiJianModel();
                models.GWID = SheBeiID;
                models.JieGuo = ishege;
                models.Value = mesma;
                JieMianCaoZuoLei.CerateDanLi().ChuFaJieMianEvent(EventType.TestZongJieGuo, models);
            }
            if (ishege)
            {

                SheBeiJiHe.Cerate().XieRuDanData(CaoZuoType.ChuZhanXieJieGuo, SheBeiID, 2);
                SheBeiJiHe.Cerate().XieRuDanData(CaoZuoType.ChuZhanWanCheng, SheBeiID, 2);
                WriteLog(RiJiEnum.TDLiuCheng, $"mes校验合格，写合格结果，成功出站:{mesma}");
            }
            else
            {
                SheBeiJiHe.Cerate().XieRuDanData(CaoZuoType.ChuZhanXieJieGuo, SheBeiID, 3);
                SheBeiJiHe.Cerate().XieRuDanData(CaoZuoType.ChuZhanWanCheng, SheBeiID, 3);
                WriteLog(RiJiEnum.TDLiuCheng, $"mes校验失败，写失败结果，出站失败:{mesma}");
               
            }
        }
        private bool JiaoYanHeGe(YeWuDataModel testmodel)
        {
            if (testmodel.IsYiZhuangTaiWeiZhun == 1)
            {
                if (testmodel.State.JiCunValue == "1")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if(testmodel.IsYiZhuangTaiWeiZhun==2)
            {
                bool zhen1 = true;
                bool zhen2 = true;
                {
                    double shangxian = ChangYong.TryDouble(testmodel.Up.JiCunValue, 0);
                    double xiaxian = ChangYong.TryDouble(testmodel.Low.JiCunValue, 0);
                    double zhi = ChangYong.TryDouble(testmodel.Value.JiCunValue, -1);
                    if (zhi >= xiaxian && zhi <= shangxian)
                    {
                        zhen1 = true;
                    }
                    else
                    {
                        zhen1 = false;
                    }
                }
                {
                    if (testmodel.State.JiCunValue == "1")
                    {
                        zhen2 = true;
                    }
                    else
                    {
                        zhen2 = false;
                    }
                }
                if (zhen1&&zhen2)
                {
                    return true;
                }
                return false;
            }
            else if (testmodel.IsYiZhuangTaiWeiZhun == 3)
            {
                double shangxian = ChangYong.TryDouble(testmodel.Up.JiCunValue,0);
                double xiaxian = ChangYong.TryDouble(testmodel.Low.JiCunValue, 0);
                double zhi = ChangYong.TryDouble(testmodel.Value.JiCunValue, -1);
                if (zhi>= xiaxian&& zhi<= shangxian)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
        private bool ShangMes(int type, string mazhi,bool isjieguo, YeWuDataModel testmodel = null)
        {
            if (type == 1)
            {
                if (SheBeiZhanModel.IsMes)
                {
                    LianWanModel lianWan = ShangChuanLei.Cerate().ABSShangChuanLei.KaiShiTiJiaoMa(SheBeiZhanModel, mazhi, IsShouZhan);
                    if (lianWan.FanHuiJieGuo == JinZhanJieGuoType.NG)
                    {
                        JieMianShiJianModel models = new JieMianShiJianModel();
                        models.GWID = SheBeiID;
                        models.JieGuo = false;
                        models.Value = $"{mazhi}:入站失败";
                        JieMianCaoZuoLei.CerateDanLi().ChuFaJieMianEvent(EventType.JinZhan, models);
                        WriteLog(RiJiEnum.MesData, $"{mazhi}:过站失败:{lianWan.NeiRong}");
                        return false;
                    }
                    else
                    {
                        JieMianShiJianModel models = new JieMianShiJianModel();
                        models.GWID = SheBeiID;
                        models.JieGuo = true;
                        models.Value = $"{mazhi}:入站成功";
                        JieMianCaoZuoLei.CerateDanLi().ChuFaJieMianEvent(EventType.JinZhan, models);
                        WriteLog(RiJiEnum.MesData, $"{mazhi}:MES进站成功:{lianWan.NeiRong}");
                        return true;
                    }
                }
                else
                {
                    JieMianShiJianModel models = new JieMianShiJianModel();
                    models.GWID = SheBeiID;
                    models.JieGuo = true;
                    models.Value = $"{mazhi}:不启用Mes入站成功";
                    JieMianCaoZuoLei.CerateDanLi().ChuFaJieMianEvent(EventType.JinZhan, models);
                    WriteLog(RiJiEnum.MesData, $"{mazhi}: 不启用MES进站成功");
                    return true;
                }
            }
            else if (type == 2)
            {
                if (SheBeiZhanModel.IsMes)
                {
                    LianWanModel lianWan = ShangChuanLei.Cerate().ABSShangChuanLei.BuZhouShangChuan(SheBeiZhanModel, mazhi, testmodel);
                    if (lianWan.FanHuiJieGuo == JinZhanJieGuoType.NG)
                    {
                        testmodel.IsShangChuanHeGe = false;
                        JieMianShiJianModel models = new JieMianShiJianModel();
                        models.GWID = SheBeiID;
                        models.JieGuo = false;
                        models.Value = testmodel;
                        JieMianCaoZuoLei.CerateDanLi().ChuFaJieMianEvent(EventType.ChuZhanData, models);
                        WriteLog(RiJiEnum.MesData, $"{mazhi}:上传数据失败:{lianWan.NeiRong}");
                        return false;

                    }
                    else
                    {
                        testmodel.IsShangChuanHeGe = true;
                        JieMianShiJianModel models = new JieMianShiJianModel();
                        models.GWID = SheBeiID;
                        models.JieGuo = true;
                        models.Value = testmodel;
                        JieMianCaoZuoLei.CerateDanLi().ChuFaJieMianEvent(EventType.ChuZhanData, models);
                        WriteLog(RiJiEnum.MesData, $"{mazhi}:上传数据成功:{lianWan.NeiRong}");
                        return true;
                    }
                }
                else
                {
                    JieMianShiJianModel models = new JieMianShiJianModel();
                    models.GWID = SheBeiID;
                    models.JieGuo = true;
                    models.Value = testmodel;
                    JieMianCaoZuoLei.CerateDanLi().ChuFaJieMianEvent(EventType.ChuZhanData, models);
                    WriteLog(RiJiEnum.MesData, $"{mazhi}: 不启用Mes显示数据");
                    return true;
                }
            }
            else if (type == 3)
            {
                if (SheBeiZhanModel.IsMes)
                {
                    LianWanModel lianWan = ShangChuanLei.Cerate().ABSShangChuanLei.JieSu(SheBeiZhanModel, mazhi, isjieguo);

                    if (lianWan.FanHuiJieGuo == JinZhanJieGuoType.NG)
                    {
                        JieMianShiJianModel models = new JieMianShiJianModel();
                        models.GWID = SheBeiID;
                        models.JieGuo = false;
                        models.Value = $"{mazhi}:出站失败"; ;
                        JieMianCaoZuoLei.CerateDanLi().ChuFaJieMianEvent(EventType.ChuZhan, models);
                        WriteLog(RiJiEnum.MesData, $"{mazhi}:出站失败:{lianWan.NeiRong}");
                        return false;
                    }
                    else
                    {
                        JieMianShiJianModel models = new JieMianShiJianModel();
                        models.GWID = SheBeiID;
                        models.JieGuo = false;
                        models.Value = $"{mazhi}:出站成功"; ;
                        JieMianCaoZuoLei.CerateDanLi().ChuFaJieMianEvent(EventType.ChuZhan, models);
                        WriteLog(RiJiEnum.MesData, $"{mazhi}:出站成功:{lianWan.NeiRong}");
                        return false;
                    }
                }
                else
                {
                    JieMianShiJianModel models = new JieMianShiJianModel();
                    models.GWID = SheBeiID;
                    models.JieGuo = true;
                    models.Value = $"{mazhi}: 不启用Mes出站成功";
                    JieMianCaoZuoLei.CerateDanLi().ChuFaJieMianEvent(EventType.ChuZhan, models);
                    WriteLog(RiJiEnum.MesData, $"{mazhi}: 不启用Mes出站成功");
                    return true;
                }
            }
            return true;
        }
    }
}
