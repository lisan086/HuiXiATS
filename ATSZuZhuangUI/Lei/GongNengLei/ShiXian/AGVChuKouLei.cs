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
    public class AGVChuKouLei : ABSGongNengLei
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
                        bool jinzhanzhi = DataJiHe.Cerate().GetGWPiPei(SheBeiID, CaoZuoType.AGVChuJinZhan请求_单);
                        if (jinzhanzhi)
                        {
                            if ((DateTime.Now - jinzhantime).TotalMilliseconds >= 500)
                            {
                                WriteLog(RiJiEnum.TDLiuCheng, $"收到PLC进站请求,准备校验");
                                XieShuJu(CaoZuoType.FoZhanSaoMa请求_单, "", 1);
                                JinZhan();
                                WriteLog(RiJiEnum.TDLiuCheng, $"进站结束");
                                Thread.Sleep(500);
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

                        bool jinzhanzhi = DataJiHe.Cerate().GetGWPiPei(SheBeiID, CaoZuoType.AGVChuChuZhan请求_单);
                        if (jinzhanzhi)
                        {
                            if ((DateTime.Now - chuzhantime).TotalMilliseconds >= 500)
                            {
                                WriteLog(RiJiEnum.TDLiuCheng, $"收到PLC出站请求,准备上传数据");
                                XieShuJu(CaoZuoType.AGVChuChuZhan请求_单, "", 1);
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

                        bool jinzhanzhi = DataJiHe.Cerate().GetGWPiPei(SheBeiID, CaoZuoType.AGVChuChuZhanManPan_单);
                        if (jinzhanzhi)
                        {
                            if ((DateTime.Now - mantime).TotalMilliseconds >= 500)
                            {
                                WriteLog(RiJiEnum.TDLiuCheng, $"去老化的盘已经满了，准备调AGV进来，或者人工取走");
                                XieShuJu(CaoZuoType.AGVChuChuZhanManPan_单, "", 1);
                                WriteLog(RiJiEnum.TDLiuCheng, $"下达AGV取走命令");
                                XieShuJu(CaoZuoType.AGVChuQuZouHuoWu_单, "", 2);
                                Thread.Sleep(500);
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

                        WriteLog(RiJiEnum.TDLiuChengRed, $"满盘{ex}");
                    }
                }
                Thread.Sleep(10);
            }
        }

        private void JinZhan()
        {
            string mesma = DataJiHe.Cerate().GetGWZhiStr(SheBeiID, CaoZuoType.AGVChuJinZhanMa_单, "");
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
                bool jieguo = JinZhanJiaoYanMes(mesma, "", false);
                if (jieguo == false)
                {
                    ishege = false;
                }
            }
            if (ishege)
            {
                string qiuxinxi = DataJiHe.Cerate().GetGWZhiStr(SheBeiID,CaoZuoType.AGVChuQiuQuWeiZhi_单,"");
                LianWanModel shuju = GetMesXinXi(mesma, qiuxinxi);
                if (shuju.FanHuiJieGuo == JinZhanJieGuoType.Pass)
                {
                    int fangxiang = ChangYong.TryInt(shuju.FanHuiCanShu,1);              
                    XieShuJu(CaoZuoType.AGVChuQuWeiZhi_单, fangxiang, 0);
                    XieShuJu(CaoZuoType.AGVChuJinZhanJieGuo_多, "", 2);
                    WriteLog(RiJiEnum.TDLiuCheng, $"mes校验合格，写合格结果，成功入站:{mesma}");
                }
                else
                {
                    XieShuJu(CaoZuoType.AGVChuQuWeiZhi_单, "", 3);
                    XieShuJu(CaoZuoType.AGVChuJinZhanJieGuo_多, "", 3);
                    WriteLog(RiJiEnum.TDLiuCheng, $"获取信息失败:{mesma}");
                }
            }
            else
            {
                XieShuJu(CaoZuoType.AGVChuQuWeiZhi_单, "", 3);
                XieShuJu(CaoZuoType.AGVChuJinZhanJieGuo_多, "", 3);
                WriteLog(RiJiEnum.TDLiuCheng, $"mes校验合格，写NG结果，NG入站:{mesma}");
            }
            modess.JinChuZhanJieGuo = ishege;
            ChuFaEvent(EventType.AGVJinZhanJieGuo, modess);
        }

        private void ChuZhanJiaoYan()
        {
            int weizhi = DataJiHe.Cerate().GetGWZhiInt(SheBeiID, CaoZuoType.AGVChuZhanWeiZhi_单,1);
            string miaosuweizhi = weizhi == 2 ? "正常" : "小车";
            string mesma = DataJiHe.Cerate().GetGWZhiStr(SheBeiID, CaoZuoType.AGVChuChuZhanMa_单, "");
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
                string canshu = "";
                if (weizhi==1)
                {

                    int ceng = DataJiHe.Cerate().GetGWZhiInt(SheBeiID, CaoZuoType.AGVChuChuZhanCeng_单, 1);

                    int lie = DataJiHe.Cerate().GetGWZhiInt(SheBeiID, CaoZuoType.AGVChuChuZhanLie_单, 1);
                    int pai = DataJiHe.Cerate().GetGWZhiInt(SheBeiID, CaoZuoType.AGVChuChuZhanPai_单, 1);
                    canshu = $"AGVChu,{ceng},{lie},{pai}";
                    WriteLog(RiJiEnum.TDLiuCheng, $"AGV出去的产品:{mesma}");
                }
                bool shangchuan = ChuZhanMes(mesma, ishege, canshu);
                if (shangchuan == false)
                {
                    ishege = false;
                }
                modess.JinChuZhanJieGuo = ishege;
                ChuFaEvent(EventType.AGVChuZhanJieGuo, modess);
            }
            if (ishege)
            {
                XieShuJu(CaoZuoType.AGVChuChuZhanJieGuo_多, "", 2);
                WriteLog(RiJiEnum.TDLiuCheng, $"{SheBeiName}，写合格结果，成功{miaosuweizhi}出站:{mesma}");
            }
            else
            {
                XieShuJu(CaoZuoType.AGVChuChuZhanJieGuo_多, "", 3);
                WriteLog(RiJiEnum.TDLiuCheng, $"{SheBeiName}:，写不合格结果，失败{miaosuweizhi}出站:{mesma}");
            }



        }
    }
}
