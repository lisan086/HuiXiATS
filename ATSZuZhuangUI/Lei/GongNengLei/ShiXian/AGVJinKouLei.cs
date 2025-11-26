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
    public  class AGVJinKouLei : ABSGongNengLei
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
            DateTime mantime = DateTime.Now;
            while (ZongKaiGuan)
            {
                //处理进站
                {
                    try
                    {
                        bool jinzhanzhi = DataJiHe.Cerate().GetGWPiPei(SheBeiID, CaoZuoType.AGVJinJinZhan请求_单);
                        if (jinzhanzhi)
                        {
                            if ((DateTime.Now - jinzhantime).TotalMilliseconds >= 500)
                            {
                                WriteLog(RiJiEnum.TDLiuCheng, $"收到PLC进站请求,准备校验");
                                XieShuJu(CaoZuoType.AGVJinJinZhan请求_单, "", 1);
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

                        bool jinzhanzhi = DataJiHe.Cerate().GetGWPiPei(SheBeiID, CaoZuoType.AGVJinChuZhan请求_单);
                        if (jinzhanzhi)
                        {
                            if ((DateTime.Now - chuzhantime).TotalMilliseconds >= 500)
                            {
                                WriteLog(RiJiEnum.TDLiuCheng, $"收到PLC出站请求,准备上传数据");
                                XieShuJu(CaoZuoType.AGVJinChuZhan请求_单, "", 1);
                               
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
                //满了
                {
                    try
                    {

                        bool jinzhanzhi = DataJiHe.Cerate().GetGWPiPei(SheBeiID, CaoZuoType.AGVJinKongPan请求_单);
                        if (jinzhanzhi)
                        {
                            if ((DateTime.Now - mantime).TotalMilliseconds >= 500)
                            {
                                WriteLog(RiJiEnum.TDLiuCheng, $"进入口已经没有托盘，发送AGV运送过来");
                                XieShuJu(CaoZuoType.AGVJinKongPan请求_单, "", 1);
                                XieShuJu(CaoZuoType.AGVJinYunLai_单, "", 2);
                              
                                Thread.Sleep(500);
                                WriteLog(RiJiEnum.TDLiuCheng, $"下达AGV运来命令");
                              
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
            int weizhi = DataJiHe.Cerate().GetGWZhiInt(SheBeiID, CaoZuoType.AGVJinJinZhan标志_单, 1);
            string mesma = DataJiHe.Cerate().GetGWZhiStr(SheBeiID, CaoZuoType.AGVJinJinZhan码_单, "");
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
            ChuFaEvent(EventType.AGVJinZhanKaiShi, modess);
            if (ishege)
            {
                if (weizhi==1)
                {
                    mesma = $"{mesma},{1}";
                    WriteLog(RiJiEnum.TDLiuCheng, $"AGV进来的产品:{mesma}");
                }
                bool jieguo = JinZhanJiaoYanMes(mesma, "", false);
                if (jieguo == false)
                {
                    ishege = false;
                }
            }
            if (ishege)
            {
                XieShuJu(CaoZuoType.AGVJinJinZhan结果_多, "", 2);
                WriteLog(RiJiEnum.TDLiuCheng, $"mes校验合格，写合格结果，成功入站:{mesma}");
            }
            else
            {
               
                XieShuJu(CaoZuoType.AGVJinJinZhan结果_多, "", 3);
                WriteLog(RiJiEnum.TDLiuCheng, $"mes校验合格，写NG结果，NG入站:{mesma}");
            }
            modess.JinChuZhanJieGuo = ishege;
            ChuFaEvent(EventType.AGVJinZhanJieGuo, modess);
        }

        private void ChuZhanJiaoYan()
        {
          
            string miaosuweizhi = 1 == 1 ? "正常" : "小车";
            string mesma = DataJiHe.Cerate().GetGWZhiStr(SheBeiID, CaoZuoType.AGVJinChuZhan码_单, "");
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
            JieMianCaoZuoLei.CerateDanLi().ChuFaJieMianEvent(EventType.AGVChuZhanKaiShi, modess);
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
              
                bool shangchuan = ChuZhanMes(mesma, ishege, "");
                if (shangchuan == false)
                {
                    ishege = false;
                }
                modess.JinChuZhanJieGuo = ishege;
                ChuFaEvent(EventType.AGVChuZhanJieGuo, modess);
            }
            if (ishege)
            {
                XieShuJu(CaoZuoType.AGVJinChuZhan结果_多, "", 2);
                WriteLog(RiJiEnum.TDLiuCheng, $"{SheBeiName}，写合格结果，成功{miaosuweizhi}出站:{mesma}");
            }
            else
            {
                XieShuJu(CaoZuoType.AGVJinChuZhan结果_多, "", 3);
                WriteLog(RiJiEnum.TDLiuCheng, $"{SheBeiName}:，写不合格结果，失败{miaosuweizhi}出站:{mesma}");
            }



        }
    }
}
