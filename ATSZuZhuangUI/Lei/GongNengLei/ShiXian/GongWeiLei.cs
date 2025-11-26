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
                        bool jinzhanzhi = DataJiHe.Cerate().GetGWPiPei(SheBeiID, CaoZuoType.Zhan进站请求_单);
                        if (jinzhanzhi)
                        {
                            if ((DateTime.Now - jinzhantime).TotalMilliseconds >= 500)
                            {
                                WriteLog(RiJiEnum.TDLiuCheng, $"收到PLC进站请求,准备校验");                            
                              
                                XieShuJu(CaoZuoType.Zhan进站请求_单, "", 1);
                                JinZhan();
                                Thread.Sleep(500);
                                WriteLog(RiJiEnum.TDLiuCheng, $"进站结束");
                              
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

                        bool jinzhanzhi = DataJiHe.Cerate().GetGWPiPei(SheBeiID, CaoZuoType.Zhan出站请求_单);
                        if (jinzhanzhi)
                        {
                            if ((DateTime.Now - chuzhantime).TotalMilliseconds >= 500)
                            {
                                WriteLog(RiJiEnum.TDLiuCheng, $"收到PLC出站请求,准备上传数据");
                              
                                XieShuJu(CaoZuoType.Zhan出站请求_单, "", 1);

                                ChuZhanJiaoYan();

                                Thread.Sleep(500);
                                WriteLog(RiJiEnum.TDLiuCheng, $"出站结束");
                              
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
                Thread.Sleep(10);
            }
        }

      
        public override void Close()
        {
            ZongKaiGuan = false;
            Thread.Sleep(15);
           
        }

        private void JinZhan()
        {
            string mesma = DataJiHe.Cerate().GetGWZhiStr(SheBeiID, CaoZuoType.Zhan进站过程码_单, "");
            bool ishege = true;
            JieMianShiJianModel modess = new JieMianShiJianModel();
            modess.ErWeiMa = mesma;
            if (string.IsNullOrEmpty(mesma))
            {                      
                modess.MiaoSu = $"{SheBeiName}:读到的码为空，进站失败:{mesma}";
                WriteLog(RiJiEnum.TDLiuChengRed, modess.MiaoSu);
                ishege = false;
            }
            else
            {
                modess.MiaoSu = $"{SheBeiName}:读到的码,开始进站:{mesma}";
                WriteLog(RiJiEnum.TDLiuCheng, modess.MiaoSu);
                ishege = true;
            }
            ChuFaEvent(EventType.ZhanKaiShiJinZhan, modess);
            if (ishege)
            {
                bool jieguo = JinZhanJiaoYanMes(mesma, "", false);
                if (jieguo==false)
                {
                    ishege = false;
                }
            }
            if (ishege)
            {
                XieShuJu(CaoZuoType.Zhan进站写结果_多,"",2);                    
                WriteLog(RiJiEnum.TDLiuCheng, $"mes校验合格，写合格结果，成功入站:{mesma}");
            }
            else
            {
                XieShuJu(CaoZuoType.Zhan进站写结果_多, "", 3);
                WriteLog(RiJiEnum.TDLiuCheng, $"mes校验合格，写NG结果，NG入站:{mesma}");
            }          
            modess.JinChuZhanJieGuo = ishege;
            ChuFaEvent(EventType.ZhanJinZhanJieGuo, modess);
        }

        private void ChuZhanJiaoYan()
        {
            JieMianShiJianModel modess = new JieMianShiJianModel();
            modess.GWID = SheBeiID;
            string mesma = DataJiHe.Cerate().GetGWZhiStr(SheBeiID, CaoZuoType.Zhan出站过程码_单, "");
            bool ishege = true;
            modess.ErWeiMa = mesma;
            if (string.IsNullOrEmpty(mesma))
            {         
                modess.MiaoSu = $"{SheBeiName}:读到的码为空，出站失败:{mesma}";
                WriteLog(RiJiEnum.TDLiuChengRed, modess.MiaoSu);              
                ishege = false;
            }
            else
            {
                modess.MiaoSu = $"{SheBeiName}:读到出站二维码:{mesma}";        
                WriteLog(RiJiEnum.TDLiuCheng, modess.MiaoSu);
            }
            JieMianCaoZuoLei.CerateDanLi().ChuFaJieMianEvent(EventType.ZhanKaiShiChuZhan, modess);
            if (ishege)
            {
                WriteLog(RiJiEnum.TDLiuCheng,$"{ SheBeiName}:出站开始上传数据");
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
                int ceshigongwei = DataJiHe.Cerate().GetGWZhiInt(SheBeiID,CaoZuoType.Zhan测试工位_单,-1);
                for (int i = 0; i < lise.Count; i++)
                {
                    if (ceshigongwei>=0)
                    {
                        if (lise[i].YongDeMaMingCheng.EndsWith(ceshigongwei.ToString())==false)
                        {
                            continue;
                        }
                    }
                    if (lise[i].IsShangChuan==1)
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
                            datamodel.ErWeiMa = mesma;
                            datamodel.ZhanChuZhanDataModel.YeWuDataModel = lise[i];
                            ChuFaEvent(EventType.ZhanChuZhanData, datamodel);
                        }
                        if (SheBeiZhanModel.IsQuanBuShangChuan != 1)
                        {
                            if (ishege==false)
                            {
                               
                                break;
                            }
                        }                    
                    }
                }
               
                List<YeWuDataModel> bangmas = DataJiHe.Cerate().GetDataModel(SheBeiID, CaoZuoType.Zhan绑码_多, true);
                if (bangmas.Count > 0)
                {
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
                }
                bool shangchuan=ChuZhanMes(mesma, ishege,"");
                if (shangchuan == false)
                {
                    ishege = false;
                }
                modess.JinChuZhanJieGuo = ishege;
                ChuFaEvent(EventType.ZhanChuZhanJieGuo, modess);
            }
            if (ishege)
            {
                XieShuJu(CaoZuoType.Zhan出站写结果_多,"",2);            
                WriteLog(RiJiEnum.TDLiuCheng, $"{SheBeiName}，写合格结果，成功出站:{mesma}");
            }
            else
            {
                XieShuJu(CaoZuoType.Zhan出站写结果_多, "", 3);
                WriteLog(RiJiEnum.TDLiuCheng, $"{SheBeiName}:，写不合格结果，失败出站:{mesma}");
            }
        }
      

        public override void CaoZuo(DoType doType, JieMianCaoZuoModel model)
        {
           
        }
    }
}
