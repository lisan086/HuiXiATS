using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ATSJianMianJK.GongNengLei;
using ATSJianMianJK.Log;
using ATSJianMianJK.XiTong.Model;
using BaseUI.DaYIngMoBan.Lei;
using CommLei.JiChuLei;
using ZuZhuangUI.Model;

namespace ZuZhuangUI.Lei.GongNengLei.ShiXian
{
    public class GongWeiLei : ABSGongNengLei
    {
        private bool ZongKaiGuan = true;
        private SheBeiZhanModel SheBeiZhanModel;
     
        public override void IniData(SheBeiZhanModel model)
        {
            ZongKaiGuan = true;
            SheBeiZhanModel = model;
          
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
                                XieShuJu(CaoZuoType.JinZhanQingQiu,"",1);                            
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
                                XieShuJu(CaoZuoType.ChuZhanQingQiu, "", 1);
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
            string mesma = DataJiHe.Cerate().GetGWZhiStr(SheBeiID, CaoZuoType.JinZhanDuErWeiMa, "");
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
                bool jieguo = JinZhanJiaoYanMes(mesma, "", false);
                if (jieguo == false)
                {
                    ishege = false;
                }
            }
            if (ishege)
            {
                XieShuJu(CaoZuoType.JinZhanXieJieGuo,"",2);
                WriteLog(RiJiEnum.TDLiuCheng, $"mes校验合格，写合格结果，成功入站:{mesma}");
            }
            else
            {
                XieShuJu(CaoZuoType.JinZhanXieJieGuo, "", 3);
                WriteLog(RiJiEnum.TDLiuCheng, $"mes校验合格，写NG结果，NG入站:{mesma}");
            }

        }

        private void ChuZhanJiaoYan()
        {
            string mesma = DataJiHe.Cerate().GetGWZhiStr(SheBeiID, CaoZuoType.ChuZhanDuErWeiMa, "");
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
                if (lise.Count > 0)
                {
                    lise.Sort((x, y) =>
                    {
                        if (x.PaiXu > y.PaiXu)
                        {
                            return 1;
                        }
                        else
                        {
                            return -1;
                        }
                    });
                }

                for (int i = 0; i < lise.Count; i++)
                {
                    if (lise[i].IsShangChuan == 1)
                    {
                        lise[i].IsShuJuHeGe = JiaoYanHeGe(lise[i]);
                        lise[i].IsShangChuanHeGe = BuZhouShuJuMes(lise[i], mesma);
                        if (lise[i].IsShuJuHeGe == false || lise[i].IsShangChuanHeGe == false)
                        {
                            ishege = false;

                        }
                        {
                            JieMianShiJianModel datamodel = new JieMianShiJianModel();
                            datamodel.GWID = SheBeiID;
                            datamodel.JieGuo =true;
                            datamodel.Value = lise[i];
                            ChuFaEvent(EventType.ChuZhanData, datamodel);

                        }
                        if (SheBeiZhanModel.IsQuanBuShangChuan != 1)
                        {
                            if (ishege == false)
                            {

                                break;
                            }
                        }
                    }
                }
                List<YeWuDataModel> bangmas = DataJiHe.Cerate().GetDataModel(SheBeiID, CaoZuoType.ChuZhanBangMa, true);
                List<string> bangms = new List<string>();
                for (int i = 0; i < bangmas.Count; i++)
                {
                    bangms.Add(bangmas[i].Value.JiCunValue.ToString());
                }
                bool bangmahege = BangMaJiaoYanMes(mesma, bangms);
                if (bangmahege == false)
                {
                    ishege = false;
                }
                bool shangchuan = ChuZhanMes(mesma, ishege);
                if (shangchuan == false)
                {
                    ishege = false;
                }
               
                JieMianShiJianModel models = new JieMianShiJianModel();
                models.GWID = SheBeiID;
                models.JieGuo = ishege;
                models.Value = mesma;
                JieMianCaoZuoLei.CerateDanLi().ChuFaJieMianEvent(EventType.TestZongJieGuo, models);
            }
            if (ishege)
            {
                XieShuJu(CaoZuoType.ChuZhanXieJieGuo,"",2);
             
                WriteLog(RiJiEnum.TDLiuCheng, $"mes校验合格，写合格结果，成功出站:{mesma}");
            }
            else
            {
                XieShuJu(CaoZuoType.ChuZhanXieJieGuo, "", 3);
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
       
    }
}
