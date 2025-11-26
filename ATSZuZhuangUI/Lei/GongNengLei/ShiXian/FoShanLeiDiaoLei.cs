using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSJianMianJK.Log;
using ATSJianMianJK.Mes;
using ATSJuanChengZuZhuangUI.Model;
using CommLei.GongYeJieHe;
using CommLei.JiChuLei;
using ZuZhuangUI.Lei;
using ZuZhuangUI.Lei.GongNengLei;
using ZuZhuangUI.Model;

namespace ATSFoZhaoZuZhuangUI.Lei.GongNengLei.ShiXian
{
    public class FoShanLeiDiaoLei : ABSGongNengLei
    {

        private bool ShouDongLeiDiao = false;
        private bool ZongKaiGuan = true;
        private SheBeiZhanModel SheBeiZhanModel;
        private FanXingJiHeLei<ShuChuMaModel> GetBiaoJiao = new FanXingJiHeLei<ShuChuMaModel>();
        public override void CaoZuo(DoType doType, JieMianCaoZuoModel model)
        {
            if (doType==DoType.ShouDongLeiDiao)
            {
                ShouDongLeiDiao = true;
            }
        }

        public override void Close()
        {
            ZongKaiGuan = false;
        }

        public override void IniData(SheBeiZhanModel model)
        {
            SheBeiZhanModel = model;
            {
                Thread thread = new Thread(LeiDiaoQingQiu);
                thread.IsBackground = true;
                thread.DisableComObjectEagerCleanup();
                thread.Start();

                Thread thread2 = new Thread(LeiDiaoChuZhan);
                thread2.IsBackground = true;
                thread2.DisableComObjectEagerCleanup();
                thread2.Start();
            }
            {
                Thread thread1 = new Thread(PCBJinZhanOrChuZhan);
                thread1.IsBackground = true;
                thread1.DisableComObjectEagerCleanup();
                thread1.Start();
            }
        }

        private void PCBJinZhanOrChuZhan()
        {
            if (SheBeiID < 0)
            {
                return;
            }
            DateTime jinzhantime = DateTime.Now;
           
            while (ZongKaiGuan)
            {

                {
                    try
                    {
                        bool jinzhanzhi = DataJiHe.Cerate().GetGWPiPei(SheBeiID, CaoZuoType.LeiDiaoPCB进站请求_单);
                        if (jinzhanzhi)
                        {
                            if ((DateTime.Now - jinzhantime).TotalMilliseconds >= 150)
                            {
                                WriteLog(RiJiEnum.TDLiuCheng, $"收到PLC的PCB板进站请求,准备PCB码校验");
                                XieShuJu(CaoZuoType.LeiDiaoPCB进站请求_单, "", 1);
                                JiaoYanPCB();
                                Thread.Sleep(500);
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

        private void JiaoYanPCB()
        {
            bool ishege = true;
            string pcbma = DataJiHe.Cerate().GetGWZhiStr(SheBeiID, CaoZuoType.LeiDiaoPLB进站码_单, "");
            if (string.IsNullOrEmpty(pcbma))
            {
                ishege = false;
                WriteLog(RiJiEnum.TDLiuCheng, $"PCB校验为空:{pcbma} ");
            }
            ShangChuanDataModel model = new ShangChuanDataModel();
            model.HuoQuXinXiModel.CanShu = pcbma;
            model.ShangChuanType = ShangChuanType.HuoQuXinXi;
            model.TDID = SheBeiID;
            model.TDName = SheBeiName;         
            LianWanModel jieguo = ShangChuanMesLei.Cerate().ShangMes(model);
            if (jieguo.FanHuiJieGuo== JinZhanJieGuoType.NG)
            {
                ishege = false;
            }
            WriteLog(RiJiEnum.TDLiuCheng, $"PCB校验结果:{jieguo.NeiRong} ");
            if (ishege)
            {
                XieShuJu(CaoZuoType.LeiDiaoPCB进站结果_多, "", 2);
                WriteLog(RiJiEnum.TDLiuCheng, $"{SheBeiName}，校验PCB成功，写成功结果:{pcbma}");
            }
            else
            {
                XieShuJu(CaoZuoType.LeiDiaoPCB进站结果_多, "", 3);
                WriteLog(RiJiEnum.TDLiuCheng, $"{SheBeiName}:，校验PCB不成功，写失败结果:{pcbma}");
            }
        }

        private void LeiDiaoQingQiu()
        {
            if (SheBeiID < 0)
            {
                return;
            }
            DateTime jinzhantime = DateTime.Now;
          
            while (ZongKaiGuan)
            {
                {
                    try
                    {
                        bool jinzhanzhi = DataJiHe.Cerate().GetGWPiPei(SheBeiID, CaoZuoType.LeiDiaoKe进站请求_单);
                        if (jinzhanzhi||ShouDongLeiDiao)
                        {
                            if ((DateTime.Now - jinzhantime).TotalMilliseconds >= 150)
                            {
                          
                                if (ShouDongLeiDiao)
                                {
                                    WriteLog(RiJiEnum.TDLiuCheng, $"收到PLC扫码镭雕手动请求,准备校验");
                                }
                                else
                                {
                                    WriteLog(RiJiEnum.TDLiuCheng, $"收到PLC扫码镭雕请求,准备校验");
                                }
                                ShouDongLeiDiao = false;
                                XieShuJu(CaoZuoType.LeiDiaoKe进站请求_单, "", 1);
                                LeiDiaoJiaoYan();
                            
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
                        WriteLog(RiJiEnum.TDLiuChengRed, $"镭雕进站{ex}");
                    }
                }               
                Thread.Sleep(10);
            }
        }

        private void LeiDiaoChuZhan()
        {
            if (SheBeiID < 0)
            {
                return;
            }
            
            DateTime chuzhantime = DateTime.Now;
            while (ZongKaiGuan)
            {
               
                {
                    try
                    {
                        bool jinzhanzhi = DataJiHe.Cerate().GetGWPiPei(SheBeiID, CaoZuoType.LeiDiaoKe出站请求_单);
                        if (jinzhanzhi)
                        {
                            if ((DateTime.Now - chuzhantime).TotalMilliseconds >= 150)
                            {
                                WriteLog(RiJiEnum.TDLiuCheng, $"收到PLC扫码镭雕出站请求,准备校验");
                                XieShuJu(CaoZuoType.LeiDiaoKe出站请求_单, "", 1);
                                ChuZhanJiaoYan();
                              
                                Thread.Sleep(250);
                                WriteLog(RiJiEnum.TDLiuCheng, $"请求结束");
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
                        WriteLog(RiJiEnum.TDLiuChengRed, $"镭雕进站{ex}");
                    }
                }
                Thread.Sleep(10);
            }
        }

        private void ChuZhanJiaoYan()
        {
            bool ishege = true;
            string pcbma = DataJiHe.Cerate().GetGWZhiStr(SheBeiID,CaoZuoType.LeiDiaoKe出站PCB码_单, "");
            string ketima = DataJiHe.Cerate().GetGWZhiStr(SheBeiID, CaoZuoType.LeiDiaoKe出站码_单, "");
            if (string.IsNullOrEmpty(pcbma)||string.IsNullOrEmpty(ketima))
            {
                ishege = false;
                WriteLog(RiJiEnum.TDLiuCheng, $"pcb码不存在或者外壳码不存在,出站失败:{pcbma} {ketima}");
            }
            bool isshangxun = true;
            if (ishege)
            {
                if (SheBeiZhanModel.QiTaCanShu == "ASY1")
                {
                    ShuChuMaModel shujusd = GetShuChuKeTiMa(ketima);
                    if (shujusd != null)
                    {
                        WriteLog(RiJiEnum.TDLiuCheng, $"{SheBeiName}:出站开始上传数据");
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

                        WriteLog(RiJiEnum.TDLiuCheng, $"{SheBeiName}，打码数据:{ChangYong.HuoQuJsonStr(shujusd)}");
                        for (int i = 0; i < lise.Count; i++)
                        {
                            if (lise[i].IsShangChuan == 1)
                            {
                                JiaoYanMa(lise[i], shujusd);
                                lise[i].IsShangChuanHeGe = BuZhouShuJuMes(lise[i], ketima);
                                if (lise[i].IsShuJuHeGe == false || lise[i].IsShangChuanHeGe == false)
                                {
                                    ishege = false;

                                }
                                {
                                    JieMianShiJianModel datamodel = new JieMianShiJianModel();
                                    datamodel.GWID = SheBeiID;
                                    datamodel.ErWeiMa = ketima;
                                    datamodel.ZhanChuZhanDataModel.YeWuDataModel = lise[i];
                                    ChuFaEvent(EventType.LeiDiaoShuJu, datamodel);
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
                        WriteLog(RiJiEnum.TDLiuCheng, $"开始绑码:{pcbma} {ketima}");
                    }
                    else
                    {
                        WriteLog(RiJiEnum.TDLiuCheng, $"{SheBeiName}:没有查询到相应的码:{ketima}");
                        ishege = false;
                        isshangxun = false;
                    }
                }
            }
            if (ishege)
            {
                bool bangmahege = BangMaJiaoYanMes(ketima, new List<string>() { pcbma });
                if (bangmahege == false)
                {
                    ishege = false;
                }
            }

            if (isshangxun)
            {
                bool shangchuan = ChuZhanMes(ketima, ishege,"");
                if (shangchuan == false)
                {
                    ishege = false;
                }
            }
            if (ishege)
            {
                XieShuJu(CaoZuoType.LeiDiaoKeXieKeTiMa_单,ketima,0);
                XieShuJu(CaoZuoType.LeiDiaoKe出站结果_多, "", 2);
                WriteLog(RiJiEnum.TDLiuCheng, $"{SheBeiName}，写合格结果，成功出站:{ketima}");
            }
            else
            {
                XieShuJu(CaoZuoType.LeiDiaoKe出站结果_多, "", 3);
                WriteLog(RiJiEnum.TDLiuCheng, $"{SheBeiName}:，写不合格结果，失败出站:{ketima}");
            }
            {
                JieMianShiJianModel modess = new JieMianShiJianModel();
                modess.GWID = SheBeiID;
                modess.MiaoSu = string.Format("{1}出站:{0}", ishege ? "成功" : "失败", ketima);
                modess.ErWeiMa = ketima;
                modess.JinChuZhanJieGuo = ishege;
                modess.ZhanChuZhanDataModel = new ZhanChuZhanDataModel();
                JieMianCaoZuoLei.CerateDanLi().ChuFaJieMianEvent(EventType.LeiDiaoChuZhan, modess);
            }
        }

        private void JiaoYanMa(YeWuDataModel testmodel,ShuChuMaModel mamodel)
        {
            if (testmodel.IsYiZhuangTaiWeiZhun == 4)
            {
                if (mamodel==null)
                {
                    testmodel.IsShuJuHeGe = false;
                    return;
                }
                string bijiaoma = ChangYong.TryStr(testmodel.Value.JiCunValue,"");
                string jieguo = mamodel.GetMa(testmodel.YongDeMaMingCheng);
                if (string.IsNullOrEmpty(jieguo) == false && string.IsNullOrEmpty(bijiaoma) == false)
                {
                    if (jieguo == bijiaoma)
                    {
                        testmodel.IsShuJuHeGe = true;
                    }
                    else
                    {
                        testmodel.IsShuJuHeGe = false;
                    }
                }
                else
                {
                    testmodel.IsShuJuHeGe = false;
                }
              
            }
            else
            {
                testmodel.IsShuJuHeGe = JiaoYanHeGe(testmodel);
            }
        }
        private void LeiDiaoJiaoYan()
        {
            bool ishege = true;

            List<ShuChuMaModel> shuju = MaGuanLi.CerateDanLi().GetMaShuJu(SheBeiID);
            if (shuju.Count==0)
            {
                ishege = false;
                WriteLog(RiJiEnum.TDLiuCheng, $"没有镭雕数据");
            }
          
            if (ishege)
            {
                WriteLog(RiJiEnum.TDLiuCheng, $"开始进站请求");
               
                bool zhen= JinZhanJiaoYanMes(shuju[0].Ma,"",false);
                if (zhen==false)
                {
                    ishege = false;
                }
                WriteLog(RiJiEnum.TDLiuCheng, string.Format("进站:{0}", zhen?"成功":"失败"));
            }
            if (ishege)
            {
                WriteLog(RiJiEnum.TDLiuCheng, $"准备开始镭雕:{shuju[0].Ma}");
                List<YeWuDataModel> sss = DataJiHe.Cerate().GetQiTaDataModel(SheBeiID, CaoZuoType.LeiDiaoKeMa进站数据_多, false);
                if (sss.Count >= 0)
                {
                    XieShuJu(CaoZuoType.LeiDiaoKeXie进站激光触发_单, "", 2);
                    int duoshaoge = 0;
                    List<YeWuDataModel> shujd = new List<YeWuDataModel>();
                    for (int i = 0; i < sss.Count; i++)
                    {
                        if (string.IsNullOrEmpty(sss[i].QingQiuPiPei)==false)
                        {
                            shujd.Add(sss[i]);
                            duoshaoge +=1;
                        }
                    }
                    int shichang = DataJiHe.Cerate().GetGWZhiInt(SheBeiID, CaoZuoType.LeiDiaoShiChang_单, 30);
                    DateTime shijian = DateTime.Now;
                    for (; ZongKaiGuan; )
                    {
                        for (int i = 0; i < shujd.Count; i++)
                        {
                            bool ischenggong = DataJiHe.Cerate().GetGWPiPei(shujd[i]);
                            if (ischenggong)
                            {
                                string shsuju = shuju[0].GetMa(shujd[i].YongDeMaMingCheng);
                                XieShuJu(CaoZuoType.LeiDiaoKe进站写数据_单, shsuju, 0,false);
                                shujd[i].Value.JiCunValue = "";
                                Thread.Sleep(20);
                              
                                WriteLog(RiJiEnum.TDLiuCheng, $"镭雕数据:{shsuju},{shujd[i].ItemName} 请求数据:{shujd[i].QingQiuPiPei}");
                                shujd.RemoveAt(i);
                                break;
                            }
                        }
                        if (shujd.Count==0)
                        {
                            WriteLog(RiJiEnum.TDLiuCheng, string.Format("镭雕参数数据完成"));
                            Thread.Sleep(1000);
                            break;
                        }
                        if ((DateTime.Now- shijian).TotalSeconds>= shichang)
                        {
                            WriteLog(RiJiEnum.TDLiuCheng, string.Format("镭雕参数没有镭雕完失败，超时"));
                            ishege = false;
                            break;
                        }
                        Thread.Sleep(1);
                    }
                }
            }
            if (ishege)
            {

                XieShuJu(CaoZuoType.LeiDiaoKe进站结果_多, "", 2);
                GetBiaoJiao.Add(shuju[0]);
                WriteLog(RiJiEnum.TDLiuCheng, $"{SheBeiName}，写合格结果，成功进站:{shuju[0].Ma}");
            }
            else
            {
                XieShuJu(CaoZuoType.LeiDiaoKe进站结果_多, "", 3);
                if (shuju.Count > 0)
                {
                    WriteLog(RiJiEnum.TDLiuCheng, $"{SheBeiName}，写不合格结果，失败进站:{shuju[0].Ma}");
                }
                else
                {
                    WriteLog(RiJiEnum.TDLiuCheng, $"{SheBeiName}，写不合格结果，失败进站:");
                }
            }

            {
                JieMianShiJianModel modess = new JieMianShiJianModel();
                modess.GWID = SheBeiID;
                modess.MiaoSu = string.Format("生成镭雕数据:{0}", ishege ? "成功" : "失败");
                modess.ErWeiMa = ChangYong.HuoQuJsonStr(shuju);
                modess.JinChuZhanJieGuo = ishege;
                modess.ZhanChuZhanDataModel = new ZhanChuZhanDataModel();
                JieMianCaoZuoLei.CerateDanLi().ChuFaJieMianEvent(EventType.LeiDiaoJinZhan, modess);
            }
        }

        private ShuChuMaModel GetShuChuKeTiMa(string shijuema)
        {
            int count = GetBiaoJiao.GetCount();
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    ShuChuMaModel shuju = GetBiaoJiao.GetModel_iHead(i);
                    if (shuju.Ma.Equals(shijuema))
                    {
                        GetBiaoJiao.Romve_Zhiding(i);
                        return shuju;
                    }
                }
            }
            return null;
        }
    }
}
