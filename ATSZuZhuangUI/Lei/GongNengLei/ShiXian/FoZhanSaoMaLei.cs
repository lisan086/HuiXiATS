using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ATSJianMianJK.Log;
using CommLei.DataChuLi;
using CommLei.GongYeJieHe;
using CommLei.JiChuLei;
using Common.DataChuLi;
using ZuZhuangUI.Lei;
using ZuZhuangUI.Lei.GongNengLei;
using ZuZhuangUI.Model;

namespace ATSZuZhuangUI.Lei.GongNengLei.ShiXian
{
    public class FoZhanSaoMaLei : ABSGongNengLei
    {

        private bool ZongKaiGuan = true;
        private SheBeiZhanModel SheBeiZhanModel;
        public override void CaoZuo(DoType doType, JieMianCaoZuoModel model)
        {

        }

        public override void Close()
        {
            ZongKaiGuan = false;
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
            while (ZongKaiGuan)
            {

                {
                    try
                    {
                        bool jinzhanzhi = DataJiHe.Cerate().GetGWPiPei(SheBeiID, CaoZuoType.FoZhanSaoMa请求_单);
                        if (jinzhanzhi)
                        {
                            if ((DateTime.Now - jinzhantime).TotalMilliseconds >= 150)
                            {
                                WriteLog(RiJiEnum.TDLiuCheng, $"收到PLC扫码请求,准备校验");
                                XieShuJu(CaoZuoType.FoZhanSaoMa请求_单, "", 1);
                                KaiShiSaoMa();
                                Thread.Sleep(200);
                                WriteLog(RiJiEnum.TDLiuCheng, $"请求结束");

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

                Thread.Sleep(10);
            }
        }

        private void KaiShiSaoMa()
        {
           
            string jieguoshuju = "";
            bool zongjieguo = false;
            WriteLog(RiJiEnum.TDLiuCheng, $"开始读码,请稍后....");
            string jieguo = "";
            string erweima = "";
            bool ishege = SheBeiJiHe.Cerate().XieRuZhiXieData(CaoZuoType.FoZhanSaoMa启动扫码_多, "", SheBeiID, out jieguo);
            {
                JieMianShiJianModel modess = new JieMianShiJianModel();
                modess.GWID = SheBeiID;
                modess.MiaoSu = "开始读码,请稍后....";
                modess.ZhanChuZhanDataModel = new ZhanChuZhanDataModel();
                JieMianCaoZuoLei.CerateDanLi().ChuFaJieMianEvent(EventType.FoZhanSaoMaKaiShi, modess);
            }
            if (ishege)
            {
                WriteLog(RiJiEnum.TDLiuCheng, $"启动扫码成功，准备读数.....");
                Thread.Sleep(200);
                string qianma = DataJiHe.Cerate().GetGWZhiStr(SheBeiID, CaoZuoType.FoZhanSaoMa读码上_单, "");
                string houma = DataJiHe.Cerate().GetGWZhiStr(SheBeiID, CaoZuoType.FoZhanSaoMa读码下_单, "");
                string zuzhuangjieguo = $"{qianma}*{houma}*;*,";
                WriteLog(RiJiEnum.TDLiuCheng, $"读取的数据上:{qianma} 后码:{houma},组装:{zuzhuangjieguo}");
                {
                    JieMianShiJianModel modess = new JieMianShiJianModel();
                    modess.GWID = SheBeiID;
                    modess.MiaoSu = $"已经读取到码:{zuzhuangjieguo}";
                    modess.ErWeiMa = zuzhuangjieguo;
                    modess.ZhanChuZhanDataModel = new ZhanChuZhanDataModel();
                    JieMianCaoZuoLei.CerateDanLi().ChuFaJieMianEvent(EventType.FoZhanDuQuMa, modess);
                }
                erweima = zuzhuangjieguo;
                bool jiesuanjieguo = SheBeiJiHe.Cerate().XieRuZhiXieData(CaoZuoType.FoZhanSaoMa计算扫码_单, zuzhuangjieguo, SheBeiID, out jieguo);
                if (jiesuanjieguo)
                {
                    WriteLog(RiJiEnum.TDLiuCheng, $"计算成功:{jieguo}");
                    string fanhuijieguo = "";
                    bool ischeng = SheBeiJiHe.Cerate().XieRuZhiXieData(CaoZuoType.FoZhanSaoMa写码结果_单, jieguo, SheBeiID, out fanhuijieguo);
                    if (ischeng)
                    {
                        jieguoshuju = $"写入成功:{jieguo}";
                        zongjieguo = true;
                        WriteLog(RiJiEnum.TDLiuCheng, $"写入成功:{jieguo}");
                    }
                    else
                    {
                        jieguoshuju = $"写入失败:{jieguo}";

                        WriteLog(RiJiEnum.TDLiuCheng, $"写入失败:{jieguo}");
                    }
                }
                else
                {
                    jieguoshuju = $"计算失败:{jieguo}";

                    WriteLog(RiJiEnum.TDLiuCheng, $"计算失败:{jieguo}");
                }
            }
            else
            {
                WriteLog(RiJiEnum.TDLiuCheng, $"启动扫码失败，写不合格数据");
              
                jieguoshuju = "启动扫码失败，写不合格数据";
            }
            if (zongjieguo)
            {
                XieShuJu(CaoZuoType.FoZhanSaoMa写PLC结果_多, "", 2);
            }
            else
            {
                XieShuJu(CaoZuoType.FoZhanSaoMa写PLC结果_多, "", 3);
            }
            {
                JieMianShiJianModel modess = new JieMianShiJianModel();
                modess.GWID = SheBeiID;
                modess.MiaoSu = jieguoshuju;
                modess.ErWeiMa = erweima;
                modess.JinChuZhanJieGuo = zongjieguo;
                modess.ZhanChuZhanDataModel = new ZhanChuZhanDataModel();
                JieMianCaoZuoLei.CerateDanLi().ChuFaJieMianEvent(EventType.FoZhanJieSu, modess);
            }
        }
    }
}
