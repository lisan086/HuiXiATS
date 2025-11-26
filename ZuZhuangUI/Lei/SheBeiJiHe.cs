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
            JiHeSheBei.Cerate().GaiBianEvent += SheBeiJiHe_GaiBianEvent;
            Thread thread = new Thread(Work);
            thread.IsBackground = true;
            thread.DisableComObjectEagerCleanup();
            thread.Start();
        }

        private void SheBeiJiHe_GaiBianEvent(int arg1, IOType arg2, string arg3, bool arg4)
        {
            //TanZhengJiShuModel model = new TanZhengJiShuModel();
            //model.DuName = arg3;
            //model.IOType = arg2;
            //model.TDID = arg1;
            //model.ZhuangTai = arg4;
            //JieMianLei.Cerate().ChuFaEvent(EventType.TanZhengJiShu, model, true);
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
                        //JieMianLei.Cerate().ChuFaEvent(EventType.TDSate, DataJiHe.Cerate().TDLisState.Values.ToList(), true);
                        List<YeWuDataModel> shujus = DataJiHe.Cerate().GetDataModel(CaoZuoType.XieXinTiao,true);
                        for (int i = 0; i < shujus.Count; i++)
                        {
                            XiePLCShuJu(shujus[i]);
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
            XieRuMolde jicunqi = GetXieModel(dataType,zhi,gwid);
            if (jicunqi != null)
            {
                JiHeSheBei.Cerate().XieShuJu(gwid,jicunqi);
                int cishu = 6;
                DateTime yanzhong = DateTime.Now;
                for (; ZongKaiGuan;)
                {
                    JiaoYanJieGuoModel chenggong = JiHeSheBei.Cerate().GetXieJieGuo(jicunqi);
                    if (chenggong.IsZuiZhongJieGuo!= JieGuoType.JingXingZhong)
                    {
                        if (chenggong.Value.ToString()== zhi.ToString())
                        {
                            break;
                        }
                    }
                    if (cishu <= 0)
                    {
                        break;
                    }
                    if ((DateTime.Now - yanzhong).TotalMilliseconds >= 1500)
                    {
                        JiHeSheBei.Cerate().XieShuJu(gwid, jicunqi);
                        yanzhong = DateTime.Now;
                        cishu--;
                    }
                    Thread.Sleep(1);
                }
            }

        }

        /// <summary>
        /// 写入单个数据 true 表示校验成功，false表示不是 1-清零值 2-是合格值 3-不合格值
        /// </summary>
        /// <param name="id"></param>
        /// <param name="shebeiid"></param>
        public void XieRuDanData(CaoZuoType dataType, int gwid,int leixing)
        {
            XieRuMolde jicunqi = GetXieModel(dataType, "", gwid,leixing);
            if (jicunqi != null)
            {
                JiHeSheBei.Cerate().XieShuJu(gwid, jicunqi);
                int cishu = 6;
                DateTime yanzhong = DateTime.Now;
                for (; ZongKaiGuan;)
                {
                    JiaoYanJieGuoModel chenggong = JiHeSheBei.Cerate().GetXieJieGuo(jicunqi);
                    if (chenggong.IsZuiZhongJieGuo != JieGuoType.JingXingZhong)
                    {
                        if (chenggong.Value.ToString() == jicunqi.Zhi.ToString())
                        {
                            break;
                        }
                    }
                    if (cishu <= 0)
                    {
                        break;
                    }
                    if ((DateTime.Now - yanzhong).TotalMilliseconds >= 1500)
                    {
                        JiHeSheBei.Cerate().XieShuJu(gwid, jicunqi);
                        yanzhong = DateTime.Now;
                        cishu--;
                    }
                    Thread.Sleep(1);
                }
            }

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
                    JiHeSheBei.Cerate().XieShuJu(model.GWID, xiemodel);
                }
               
            }
        }

        private XieRuMolde GetXieModel(CaoZuoType dataType, object zhi, int gwid,int  isyongpipei=0)
        {
            YeWuDataModel xie = DataJiHe.Cerate().GetModel(gwid, dataType,true);
            if (xie != null)
            {
                if (isyongpipei == 1)
                {
                    XieRuMolde ruMolde = GetXieModel(xie.Value, xie.QingLingZhi);
                    return ruMolde;
                }
                else if (isyongpipei == 2)
                {
                    XieRuMolde ruMolde = GetXieModel(xie.Value, xie.PassZhi);
                    return ruMolde;
                }
                else if (isyongpipei == 3)
                {
                    XieRuMolde ruMolde = GetXieModel(xie.Value, xie.NGZhi);
                    return ruMolde;
                }
                else
                {
                    XieRuMolde ruMolde = GetXieModel(xie.Value, zhi);
                    return ruMolde;
                }
               
            }
            return null;
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
                return model;
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
