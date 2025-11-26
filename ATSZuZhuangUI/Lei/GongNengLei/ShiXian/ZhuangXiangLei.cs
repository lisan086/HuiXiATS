using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ATSJianMianJK.Log;
using ATSJianMianJK.Mes;
using CommLei.JiChuLei;
using ZuZhuangUI.Lei;
using ZuZhuangUI.Lei.GongNengLei;
using ZuZhuangUI.Model;

namespace ATSFoZhaoZuZhuangUI.Lei.GongNengLei.ShiXian
{
    public  class ZhuangXiangLei : ABSGongNengLei
    {
        private bool ZongKaiGuan = true;
        private SheBeiZhanModel SheBeiZhanModel;
        public override void CaoZuo(DoType doType, JieMianCaoZuoModel model)
        {

        }

        public override void Close()
        {
            ZongKaiGuan = false;
            Thread.Sleep(10);
        }
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
            if (SheBeiID < 0)
            {
                return;
            }
            DateTime jinzhantime = DateTime.Now;
            DateTime chuzhantime = DateTime.Now;
            DateTime qidongtime = DateTime.Now;
            DateTime mantime = DateTime.Now;
            while (ZongKaiGuan)
            {
                //处理进站
                {
                    try
                    {
                        bool jinzhanzhi = DataJiHe.Cerate().GetGWPiPei(SheBeiID, CaoZuoType.ZXJinZhan请求_单);
                        if (jinzhanzhi)
                        {
                            if ((DateTime.Now - jinzhantime).TotalMilliseconds >= 500)
                            {
                                WriteLog(RiJiEnum.TDLiuCheng, $"收到PLC进站请求,准备校验");
                              
                                XieShuJu(CaoZuoType.ZXJinZhan请求_单, "", 1);
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

                        bool jinzhanzhi = DataJiHe.Cerate().GetGWPiPei(SheBeiID, CaoZuoType.ZXChuZhan请求_单);
                        if (jinzhanzhi)
                        {
                            if ((DateTime.Now - chuzhantime).TotalMilliseconds >= 500)
                            {
                                WriteLog(RiJiEnum.TDLiuCheng, $"收到PLC出站请求,准备上传数据");
                              
                                XieShuJu(CaoZuoType.ZXChuZhan请求_单, "", 1);

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

                //装箱启动流程
                {
                    try
                    {

                        bool jinzhanzhi = DataJiHe.Cerate().GetGWPiPei(SheBeiID, CaoZuoType.ZXQiDong_单);
                        if (jinzhanzhi)
                        {
                            if ((DateTime.Now - qidongtime).TotalMilliseconds >= 500)
                            {
                                WriteLog(RiJiEnum.TDLiuCheng, $"收到PLC装箱启动信号");
                              
                                XieShuJu(CaoZuoType.ZXQiDong_单, "", 1);

                                ZhuangXiangQiDong();
                                Thread.Sleep(500);
                                WriteLog(RiJiEnum.TDLiuCheng, $"装箱结束");

                                qidongtime = DateTime.Now;
                            }

                        }
                        else
                        {
                            qidongtime = DateTime.Now;
                        }
                    }
                    catch (Exception ex)
                    {

                        WriteLog(RiJiEnum.TDLiuChengRed, $"出站{ex}");
                    }
                }
                //满了
                {
                    try
                    {

                        bool jinzhanzhi = DataJiHe.Cerate().GetGWPiPei(SheBeiID, CaoZuoType.ZXJieSu_单);
                        if (jinzhanzhi)
                        {
                            if ((DateTime.Now - mantime).TotalMilliseconds >= 500)
                            {
                                WriteLog(RiJiEnum.TDLiuCheng, $"已经装箱满了，请出箱子码");
                                XieShuJu(CaoZuoType.ZXJieSu_单, "", 1);
                               
                                ZhuangXiangMan();
                                Thread.Sleep(500);
                                WriteLog(RiJiEnum.TDLiuCheng, $"出码完成");
                                mantime = DateTime.Now;
                            }

                        }
                        else
                        {
                            mantime = DateTime.Now;
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

        private void JinZhan()
        {
            string mesma = DataJiHe.Cerate().GetGWZhiStr(SheBeiID, CaoZuoType.ZXJinZhanMa_单, "");
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
            ChuFaEvent(EventType.ZXJinZhanKaiShi, modess);
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
              
                XieShuJu(CaoZuoType.ZXJinZhan结果_单, "", 2);
                WriteLog(RiJiEnum.TDLiuCheng, $"mes校验合格，写合格结果，成功入站:{mesma}");
            }
            else
            {
             
                XieShuJu(CaoZuoType.ZXJinZhan结果_单, "", 3);
                WriteLog(RiJiEnum.TDCuoWuRedLog, $"mes校验不合格，写NG结果，NG入站:{mesma}");
            }
            modess.JinChuZhanJieGuo = ishege;
            ChuFaEvent(EventType.AGVJinZhanJieGuo, modess);
        }

        private void ChuZhanJiaoYan()
        {
            int weizhi =1;
            string miaosuweizhi = weizhi == 1 ? "正常" : "小车";
            string mesma = DataJiHe.Cerate().GetGWZhiStr(SheBeiID, CaoZuoType.ZXChuZhanMa_单, "");
            JieMianShiJianModel modess = new JieMianShiJianModel();
            modess.GWID = SheBeiID;
            bool ishege = true;
            modess.ErWeiMa = mesma;
            if (string.IsNullOrEmpty(mesma))
            {
                modess.MiaoSu = $"{SheBeiName}:读到的码为空，{miaosuweizhi}出站失败:{mesma}";
                WriteLog(RiJiEnum.TDLiuChengRed, modess.MiaoSu);
                ishege = false;
            }
            else
            {
                modess.MiaoSu = $"{SheBeiName}:{miaosuweizhi}出站读到出站二维码:{mesma}";
                WriteLog(RiJiEnum.TDLiuCheng, modess.MiaoSu);
            }
            JieMianCaoZuoLei.CerateDanLi().ChuFaJieMianEvent(EventType.ZXChuZhanKaiShi, modess);
            if (ishege)
            {
                WriteLog(RiJiEnum.TDLiuCheng, $"{SheBeiName}:{miaosuweizhi}出站开始上传数据");
                List<YeWuDataModel> lise = DataJiHe.Cerate().GetDataModel(SheBeiID, CaoZuoType.DataShangChuan, true);
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
                            datamodel.ErWeiMa = mesma;
                            datamodel.ZhanChuZhanDataModel.YeWuDataModel = lise[i];
                            ChuFaEvent(EventType.ZhanChuZhanData, datamodel);
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
                int ceng = DataJiHe.Cerate().GetGWZhiInt(SheBeiID, CaoZuoType.ZXChuZhanCeng_单, 1);
                int lie = DataJiHe.Cerate().GetGWZhiInt(SheBeiID, CaoZuoType.ZXChuZhanLie_单, 1);
                int pai = DataJiHe.Cerate().GetGWZhiInt(SheBeiID, CaoZuoType.ZXChuZhanPai_单, 1);
                string  canshu = $"ZX,{ceng},{lie},{pai}";
                bool shangchuan = ChuZhanMes(mesma, ishege, canshu);
                if (shangchuan == false)
                {
                    ishege = false;
                }
                modess.JinChuZhanJieGuo = ishege;
                ChuFaEvent(EventType.ZXChuZhanJieSu, modess);
                WriteLog(RiJiEnum.TDLiuCheng, $"{SheBeiName}，出站:{mesma} {canshu}");
            }
            if (ishege)
            {
                XieShuJu(CaoZuoType.ZXChuZhanJieGuo_多, "", 2);
                WriteLog(RiJiEnum.TDLiuCheng, $"{SheBeiName}，写合格结果，成功{miaosuweizhi}出站:{mesma}");
            }
            else
            {
                XieShuJu(CaoZuoType.ZXChuZhanJieGuo_多, "", 3);
                WriteLog(RiJiEnum.TDCuoWuRedLog, $"{SheBeiName}:，写不合格结果，失败{miaosuweizhi}出站:{mesma}");
            }



        }

        private void ZhuangXiangQiDong()
        {
            WriteLog(RiJiEnum.TDLiuCheng, $"开始请求mes 申请装箱号");
            LianWanModel shuju = GetMesXinXi("1","zxqq");
            if (shuju.FanHuiJieGuo == JinZhanJieGuoType.Pass)
            {
                WriteLog(RiJiEnum.TDLiuCheng, $"箱号申请成功{shuju.FanHuiCanShu} 写入箱号");
                XieShuJu(CaoZuoType.ZXXie箱号_单, shuju.FanHuiCanShu, 0);
                XieShuJu(CaoZuoType.ZXJieGuo_多,"",2);
            }
            else
            {
                WriteLog(RiJiEnum.TDLiuCheng, $"箱号申请失败{shuju.FanHuiCanShu}");
                XieShuJu(CaoZuoType.ZXJieGuo_多, "", 3);
            }
        }

        private void ZhuangXiangMan()
        {
            string mesma = DataJiHe.Cerate().GetGWZhiStr(SheBeiID, CaoZuoType.ZXJieGuoMa_单, "");
            List < YeWuDataModel > zhonliang = DataJiHe.Cerate().GetQiTaDataModel(SheBeiID, CaoZuoType.ZXZhongLiang_单, true);
            if (zhonliang.Count > 0)
            {
                if (string.IsNullOrEmpty(mesma) == false)
                {
                    float zhongliang = ChangYong.TryFloat(zhonliang[0].Value.JiCunValue, 0);
                    float zhongs = ChangYong.TryFloat(zhonliang[0].PassZhi, 0);
                    float zhongx = ChangYong.TryFloat(zhonliang[0].NGZhi, 0);
                    if (zhongliang >= zhongx && zhongliang <= zhongs)
                    {
                        LianWanModel shuju = GetMesXinXi(mesma, $"zxman,{zhongliang}");
                        WriteLog(RiJiEnum.TDLiuCheng, $"{mesma}箱号重量符合要求，封箱成功");
                    }
                    else
                    {
                        WriteLog(RiJiEnum.TDLiuCheng, $"{mesma}箱号重量不合格导致封箱失败,重量:{zhongliang},上限/下限{zhongs}/{zhongx}");
                    }
                }
                else
                {

                    WriteLog(RiJiEnum.TDLiuCheng, $"{mesma}箱号不存在，导致封箱失败");
                }
            }
            else
            {
                WriteLog(RiJiEnum.TDLiuCheng, $"箱号重量没有配置，导致封箱失败");
            }
        }
    }
}
