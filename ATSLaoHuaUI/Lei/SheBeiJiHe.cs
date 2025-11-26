using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ATSJianMianJK.GongNengLei;
using ATSJianMianJK.Log;
using ATSJianMianJK.XiTong.Model;
using CommLei.DataChuLi;
using CommLei.JiChuLei;
using SSheBei.Model;
using ZuZhuangUI.Model;

namespace ZuZhuangUI.Lei
{
    public class SheBeiJiHe
    {
        /// <summary>
        /// 线程总开关
        /// </summary>
        private bool ZongKaiGuan = true;

        /// <summary>
        /// 工作开关
        /// </summary>
        private bool GongZuoWork = false;
        #region 单利
        private readonly static object _DuiXiang = new object();

        private static SheBeiJiHe _LogTxt = null;



        private SheBeiJiHe()
        {
            IniData();

        }
        /// <summary>
        /// 单例类，必须KaiqiRiZhi设置为True才能写日志
        /// </summary>
        /// <returns>返回NewXieRiZhiLog</returns>
        public static SheBeiJiHe Cerate()
        {
            if (_LogTxt == null)
            {
                lock (_DuiXiang)
                {
                    if (_LogTxt == null)
                    {
                        _LogTxt = new SheBeiJiHe();
                    }
                }
            }
            return _LogTxt;
        }
        #endregion

        private void IniData()
        {
            JiHeSheBei.Cerate().JiHeData = DataJiHe.Cerate();
            JiHeSheBei.Cerate().XieBuZhou = new XieBuZhou();
          
            Thread thread = new Thread(Work);
            thread.IsBackground = true;
            thread.DisableComObjectEagerCleanup();
            thread.Start();
        }

      
        /// <summary>
        /// 打开
        /// </summary>
        public void Open()
        {
            JiHeSheBei.Cerate().Open();
            GongZuoWork = true;

        }
        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            ZongKaiGuan = false;
            Thread.Sleep(100);
            JiHeSheBei.Cerate().Close();
        }

        private void Work()
        {
            DateTime JiShiTime = DateTime.Now;
            while (ZongKaiGuan)
            {
                if (GongZuoWork == false)
                {
                    Thread.Sleep(50);
                    continue;
                }
                try
                {
                    if ((DateTime.Now - JiShiTime).TotalMilliseconds >= 1500)
                    {
                        List<SheBeiZhanModel> shebeis = DataJiHe.Cerate().LisSheBeiBianHao;
                        if (shebeis.Count > 0)
                        {
                            for (int i = 0; i < shebeis.Count; i++)
                            {
                                List<YeWuDataModel> shujus = DataJiHe.Cerate().GetDataModel(shebeis[i].GWID, CaoZuoType.ZongQingXieXinTiao, true);
                                if (shujus.Count > 0)
                                {
                                    for (int c = 0; c < shujus.Count; c++)
                                    {
                                        XiePLCShuJu(shujus[c]);
                                    }
                                }
                            }
                        }
                      
                        JiShiTime = DateTime.Now;

                    }
                }
                catch (Exception ex)
                {
                    RiJiLog.Cerate().Add(RiJiEnum.TDXieJiLu, $"设备集合:发生错误{ex}", -1);
                }
                Thread.Sleep(5);
            }
        }



        /// <summary>
        /// 写入单个数据 true 表示校验成功，false表示不是
        /// </summary>
        /// <param name="id"></param>
        /// <param name="shebeiid"></param>
        public void XieRuDanData(CaoZuoType dataType, object zhi, int gwid)
        {
            List<XieRuMolde> jicunqis = GetXieModel(dataType, gwid, zhi);
            RiJiLog.Cerate().Add(RiJiEnum.TDXieJiLu, ChangYong.HuoQuJsonStr(jicunqis), gwid);
            if (jicunqis.Count > 0)
            {
                int cishu = 6;
                for (int i = 0; i < jicunqis.Count; i++)
                {
                    for (int c = 0; c < cishu; c++)
                    {
                        JiHeSheBei.Cerate().XieShuJu(gwid, jicunqis[i]);
                        DateTime yanzhong = DateTime.Now;
                        int iskeyi = 0;
                        for (; ZongKaiGuan;)
                        {
                            JiaoYanJieGuoModel chenggong = JiHeSheBei.Cerate().GetXieJieGuo(jicunqis[i]);
                            if (chenggong.IsZuiZhongJieGuo != JieGuoType.JingXingZhong)
                            {
                                if (chenggong.IsZuiZhongJieGuo == JieGuoType.ChengGongJiGuo)
                                {
                                    iskeyi = 1;
                                }
                                else
                                {
                                    iskeyi = 2;
                                }
                            }
                            if (iskeyi > 0)
                            {
                                break;
                            }
                            if ((DateTime.Now - yanzhong).TotalMilliseconds >= 800)
                            {
                                break;
                            }
                            Thread.Sleep(1);
                        }
                        if (iskeyi == 1)
                        {
                            break;
                        }
                    }

                }



            }
        }

        /// <summary>
        /// 写入单个数据 true 表示校验成功，false表示不是 1-清零值 2-是合格值 3-不合格值
        /// </summary>
        /// <param name="id"></param>
        /// <param name="shebeiid"></param>
        public void XieRuPeiZhiData(CaoZuoType dataType, int gwid,int leixing)
        {
            List<XieRuMolde> jicunqis = GetXiePeiZhiModel(dataType, gwid,leixing);
            RiJiLog.Cerate().Add(RiJiEnum.TDXieJiLu, ChangYong.HuoQuJsonStr(jicunqis), gwid);
            if (jicunqis.Count>0)
            {
                int cishu = 6;
                for (int i = 0; i < jicunqis.Count; i++)
                {
                    for (int c = 0; c < cishu; c++)
                    {
                        JiHeSheBei.Cerate().XieShuJu(gwid, jicunqis[i]);
                        DateTime yanzhong = DateTime.Now;
                        int iskeyi = 0;
                        for (; ZongKaiGuan;)
                        {
                            JiaoYanJieGuoModel chenggong = JiHeSheBei.Cerate().GetXieJieGuo(jicunqis[i]);
                            if (chenggong.IsZuiZhongJieGuo != JieGuoType.JingXingZhong)
                            {
                                if (chenggong.IsZuiZhongJieGuo == JieGuoType.ChengGongJiGuo)
                                {
                                    iskeyi = 1;
                                }
                                else
                                {
                                    iskeyi = 2;
                                }
                            }
                            if (iskeyi>0)
                            {
                                break;
                            }
                            if ((DateTime.Now - yanzhong).TotalMilliseconds >= 800)
                            {
                                break;
                            }
                            Thread.Sleep(1);
                        }
                        if (iskeyi==1)
                        {
                            break;
                        }
                    }
                  
                }
             
                
               
            }

        }

        /// <summary>
        /// 写入单个数据 true 表示校验成功，false表示不是 1-清零值 2-是合格值 3-不合格值
        /// </summary>
        /// <param name="id"></param>
        /// <param name="shebeiid"></param>
        public void XieRuPeiZhiData(YeWuDataModel model,int gwid, int leixing)
        {
            if (model==null)
            {
                return;
            }
            XieRuMolde xinmodel = new XieRuMolde();
            xinmodel.JiCunQiWeiYiBiaoShi = model.Value.JCQStr;
            xinmodel.SheBeiID=model.Value.SheBeiID;
            xinmodel.Zhi = model.PassZhi;
            if (leixing == 1)
            {
                xinmodel.Zhi = model.QingLingZhi;
            }
            else if (leixing == 3)
            {
                xinmodel.Zhi = model.NGZhi;
            }
            if (string.IsNullOrEmpty(xinmodel.JiCunQiWeiYiBiaoShi) == false && string.IsNullOrEmpty(xinmodel.Zhi.ToString()) == false)
            {
                List<XieRuMolde> jicunqis = new List<XieRuMolde>() { xinmodel };
              
                if (jicunqis.Count > 0)
                {

                    int cishu = 6;
                    for (int i = 0; i < jicunqis.Count; i++)
                    {
                        for (int c = 0; c < cishu; c++)
                        {
                            JiHeSheBei.Cerate().XieShuJu(gwid, jicunqis[i]);
                            DateTime yanzhong = DateTime.Now;
                            int iskeyi = 0;
                            for (; ZongKaiGuan;)
                            {
                                JiaoYanJieGuoModel chenggong = JiHeSheBei.Cerate().GetXieJieGuo(jicunqis[i]);
                                if (chenggong.IsZuiZhongJieGuo != JieGuoType.JingXingZhong)
                                {
                                    if (chenggong.IsZuiZhongJieGuo == JieGuoType.ChengGongJiGuo)
                                    {
                                        iskeyi = 1;
                                    }
                                    else
                                    {
                                        iskeyi = 2;
                                    }
                                }
                                if (iskeyi > 0)
                                {
                                    break;
                                }
                                if ((DateTime.Now - yanzhong).TotalMilliseconds >= 800)
                                {
                                    break;
                                }
                                Thread.Sleep(1);
                            }
                            if (iskeyi == 1)
                            {
                                break;
                            }
                        }

                    }


                }
            }
            RiJiLog.Cerate().Add(RiJiEnum.TDXieJiLu, ChangYong.HuoQuJsonStr(xinmodel), model.GWID);

        }

        public void XiePLCShuJu(YeWuDataModel model)
        {
            if (model!=null)
            {
                ShuJuLisModel xie = model.Value;
                if (xie != null)
                {
                    XieRuMolde xiemodel = new XieRuMolde();
                    xiemodel.SheBeiID = xie.SheBeiID;
                    xiemodel.JiCunQiWeiYiBiaoShi = xie.JCQStr;
                    xiemodel.Zhi = model.PassZhi;
                    if (string.IsNullOrEmpty(xiemodel.JiCunQiWeiYiBiaoShi) == false)
                    {
                        JiHeSheBei.Cerate().XieShuJu(model.GWID, xiemodel);
                    }
                    RiJiLog.Cerate().Add(RiJiEnum.TDXieJiLu,ChangYong.HuoQuJsonStr(xiemodel), model.GWID);
                }
               
            }
        }

        private List< XieRuMolde> GetXiePeiZhiModel(CaoZuoType dataType,int gwid,int  isyongpipei)
        {
            List<XieRuMolde> lisxie = new List<XieRuMolde>();
            List<YeWuDataModel> xies = DataJiHe.Cerate().GetDataModel(gwid, dataType,true);
            for (int i = 0; i < xies.Count; i++)
            {
                if (isyongpipei == 1)
                {
                    if (string.IsNullOrEmpty(xies[i].QingLingZhi) == false)
                    {
                        XieRuMolde ruMolde = GetXieModel(xies[i].Value, xies[i].QingLingZhi);
                        if (ruMolde != null)
                        {
                            lisxie.Add(ruMolde);
                        }
                    }
                }
                else if (isyongpipei == 2)
                {
                    if (string.IsNullOrEmpty(xies[i].PassZhi) == false)
                    {
                        XieRuMolde ruMolde = GetXieModel(xies[i].Value, xies[i].PassZhi);
                        if (ruMolde != null)
                        {
                            lisxie.Add(ruMolde);
                        }
                    }
                }
                else if (isyongpipei == 3)
                {
                    if (string.IsNullOrEmpty(xies[i].NGZhi) == false)
                    {
                        XieRuMolde ruMolde = GetXieModel(xies[i].Value, xies[i].NGZhi);
                        if (ruMolde != null)
                        {
                            lisxie.Add(ruMolde);
                        }
                    }
                }
               
            }
           
            return lisxie;
        }

        private List<XieRuMolde> GetXieModel(CaoZuoType dataType, int gwid, object zhi)
        {
            List<XieRuMolde> lisxie = new List<XieRuMolde>();
            List<YeWuDataModel> xies = DataJiHe.Cerate().GetDataModel(gwid, dataType, true);
            for (int i = 0; i < xies.Count; i++)
            {
                XieRuMolde ruMolde = GetXieModel(xies[i].Value, zhi);
                lisxie.Add(ruMolde);

            }

            return lisxie;
        }

        private XieRuMolde GetXieModel(ShuJuLisModel shujumodel,object zhi)
        {
            ShuJuLisModel xie = shujumodel;
            if (xie != null)
            {
                XieRuMolde model = new XieRuMolde();
                model.SheBeiID=xie.SheBeiID;
                model.JiCunQiWeiYiBiaoShi = xie.JCQStr;
                model.Zhi = zhi;
                if (string.IsNullOrEmpty(model.JiCunQiWeiYiBiaoShi) == false)
                {
                    return model;
                }
            }
            return null;
        }

        public int GetSheBeiID(string sfname, string shebeizu)
        {
            List<JiaZaiSheBeiModel> lis = HCLisDataLei<JiaZaiSheBeiModel>.Ceratei().LisWuLiao;
            for (int i = 0; i < lis.Count; i++)
            {
                if (lis[i].SheBeiName == sfname)
                {
                    if (lis[i].SheBeiZu.Contains("无"))
                    {
                        return lis[i].SheBeiID;
                    }
                    else
                    {
                        if (lis[i].SheBeiZu == shebeizu)
                        {
                            return lis[i].SheBeiID;
                        }
                    }
                }
            }
            return -1;
        }
    }
}
