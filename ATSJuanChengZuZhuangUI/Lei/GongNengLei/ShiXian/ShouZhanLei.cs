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
    public class ShouZhanLei : ABSGongNengLei
    {
        private bool ZongKaiGuan = true;
        private SheBeiZhanModel SheBeiZhanModel;
        private bool SaoWanXinHao = false;
      
      
        public override void IniData(SheBeiZhanModel model)
        {
            ZongKaiGuan = true;
            SheBeiZhanModel = model;
            JiHeSheBei.Cerate().GaiBianEvent += GongWeiLei_GaiBianEvent;
            Thread thread = new Thread(Work);
            thread.IsBackground = true;
            thread.DisableComObjectEagerCleanup();
            thread.Start();
        }

        private void GongWeiLei_GaiBianEvent(int arg1, IOType type, string arg3, bool arg4, string arg5)
        {
            if (ZongKaiGuan)
            {
                if (type == IOType.IOChuFaEvent)
                {
                    if (arg1 == SheBeiZhanModel.GWID)
                    {
                        if (arg3.Contains("触发码"))
                        {
                            if (arg4)
                            {
                                ShuChuMa();
                            }
                        }

                    }
                }
            }
        }

       
        private void Work()
        {
            if (SheBeiID < 0)
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

                        bool jinzhanzhi = DataJiHe.Cerate().GetGWPiPei(SheBeiID, CaoZuoType.ShouZhanBangMaQingQiu);
                        if (jinzhanzhi)
                        {
                            if ((DateTime.Now - jinzhantime).TotalMilliseconds >= 500)
                            {
                                WriteLog(RiJiEnum.TDLiuCheng, $"收到PLC扫码确认请求");
                                Thread.Sleep(500);
                                QiTaJinZhan();
                                WriteLog(RiJiEnum.TDLiuCheng, $"请求结束");
                              
                                XieShuJu(CaoZuoType.ShouZhanBangMaQingQiu,"",1);
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

                if (SaoWanXinHao)
                {
                    SaoWanXinHao = false;
                    WriteLog(RiJiEnum.TDLiuCheng, $"扫码校验确认");
                    ChuZhanJiaoYanN();
                }
                Thread.Sleep(50);
            }
        }

        private void QiTaJinZhan()
        {
            SaoWanXinHao = true;
            JieMianShiJianModel modess = new JieMianShiJianModel();
            modess.GWID = SheBeiID;
            modess.JieGuo = true;
            modess.Value = "绑定二维码";
            JieMianCaoZuoLei.CerateDanLi().ChuFaJieMianEvent(EventType.JinZhanErWeiMa, modess);
        }

    


        private void ChuZhanJiaoYanN()
        {
            bool ishege = true;
            string mesma = DataJiHe.Cerate().GetGWZhiStr(SheBeiID, CaoZuoType.ShouZhanGuoChengMa, "");
            if (string.IsNullOrEmpty(mesma)==false)
            {
                List<YeWuDataModel> shouzhanbangmas = DataJiHe.Cerate().GetDataModel(SheBeiID,CaoZuoType.ShouZhanFuBanMa1,true);

                List<string> shujus = new List<string>();
                for (int i = 0; i < shouzhanbangmas.Count; i++)
                {
                    shujus.Add(shouzhanbangmas[i].Value.JiCunValue);
                }
                bool jinzhans = JinZhanJiaoYanMes(mesma,ChangYong.FenGeDaBao(shujus,","),true);
                if (jinzhans)
                {
                    ishege = true;
                }
                else
                {
                    ishege = false;
                }
            
            }
            else
            {
                ishege = false;
                WriteLog(RiJiEnum.TDLiuCheng, $"扫的主码:{mesma} 为空");
            }
            if (ishege)
            {
                XieShuJu(CaoZuoType.ShouZhanXieJieGuo,"",2);
                
                WriteLog(RiJiEnum.TDLiuCheng, $"校验合格:{mesma}");
            }
            else
            {
                XieShuJu(CaoZuoType.ShouZhanXieJieGuo, "", 3);
                WriteLog(RiJiEnum.TDLiuCheng, $"校验不合格:{mesma}");

            }
            JieMianShiJianModel modess = new JieMianShiJianModel();
            modess.GWID = SheBeiID;
            modess.JieGuo = ishege;
            modess.Value = mesma;
            JieMianCaoZuoLei.CerateDanLi().ChuFaJieMianEvent(EventType.TestZongJieGuo, modess);
        }
        public override void Close()
        {
            ZongKaiGuan = false;
            Thread.Sleep(15);
            JiHeSheBei.Cerate().GaiBianEvent -= GongWeiLei_GaiBianEvent;
        }

        public override void CaoZuo(DoType doType, JieMianCaoZuoModel model)
        {
            if (doType == DoType.SaoMaQueRen)
            {
                //Ma = model.CanShu.ToString();
                ////ma.RemoveAt(0);
                ////CiMa = ma;
                //SaoWanXinHao = true;
            }
            else if (doType==DoType.ShuChuMa)
            {
                ShuChuMa();
            }
        }

        private void ShuChuMa()
        {
            string lujing = MaGuanLi.CerateDanLi().GetMaWenJianPath();
            ShuChuMaShuJuModel models = MaGuanLi.CerateDanLi().GetMaShuJu();
            if (models.IsQuanBuWeiKong())
            {
                DaYingLeiXin<ShuChuMaShuJuModel, ShuChuMaShuJuModel> daYingLeiXin = new DaYingLeiXin<ShuChuMaShuJuModel, ShuChuMaShuJuModel>(lujing);

                daYingLeiXin.DaYingYuLan(models, new List<ShuChuMaShuJuModel>(), 1, "", true);
                JieMianShiJianModel modess = new JieMianShiJianModel();

                modess.GWID = SheBeiID;
                modess.JieGuo = true;
                modess.Value = models.GetLisMa();
                JieMianCaoZuoLei.CerateDanLi().ChuFaJieMianEvent(EventType.ShouZhanChuMa, modess);
                RiJiLog.Cerate().Add(RiJiEnum.TDLiuCheng, $"输出码:{models.GetLog()}", SheBeiZhanModel.GWID);
            }
            else
            {
                RiJiLog.Cerate().Add(RiJiEnum.TDLiuChengRed, "码已经用完", SheBeiZhanModel.GWID);
            }
        }
    }
}
