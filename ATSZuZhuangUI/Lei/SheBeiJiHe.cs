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
                        List<SheBeiZhanModel> lis = DataJiHe.Cerate().LisSheBeiBianHao;
                        for (int i = 0; i < lis.Count; i++)
                        {
                            List<string> meijus= ChangYong.MeiJuLisName(typeof(CaoZuoType));
                            foreach (var item in meijus)
                            {
                                if (item.Contains("写型号"))
                                {
                                    XieRuWuJiaoYanData(ChangYong.GetMeiJuZhi<CaoZuoType>(item), "", 2, lis[i].GWID);
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
        /// 写入数据   1-清零值 2-是合格值 3-不合格值  0是需要参数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="shebeiid"></param>
        public void XieRuJiaoYanData(CaoZuoType dataType, object zhi,int leixing, int gwid)
        {
            if (dataType==CaoZuoType.DataShangChuan)
            {
                return;
            }
            List<YeWuDataModel> shujumodel = DataJiHe.Cerate().GetQiTaDataModel(gwid, dataType,true);
            if (shujumodel.Count>0)
            {
                int count = shujumodel.Count;
                for (int i = 0; i < count; i++)
                {
                    XieRuMolde xiemodel = GetXieModel(shujumodel[i].Value);
                    if (xiemodel != null)
                    {
                        if (leixing == 0)
                        {
                           
                            xiemodel.Zhi = zhi;
                        }
                        else if (leixing == 1)
                        {
                            if (string.IsNullOrEmpty(shujumodel[i].QingLingZhi))
                            {
                                continue;
                            }
                            xiemodel.Zhi = shujumodel[i].QingLingZhi;
                        }
                        else if (leixing == 2)
                        {
                            if (string.IsNullOrEmpty(shujumodel[i].PassZhi))
                            {
                                continue;
                            }
                            xiemodel.Zhi = shujumodel[i].PassZhi;
                        }
                        else if (leixing == 3)
                        {
                            if (string.IsNullOrEmpty(shujumodel[i].NGZhi))
                            {
                                continue;
                            }
                            xiemodel.Zhi = shujumodel[i].NGZhi;
                        }
                        zhi = xiemodel.Zhi.ToString();
                        JiHeSheBei.Cerate().XieShuJu(gwid, xiemodel);
                        int cishu = 3;
                        DateTime yanzhong = DateTime.Now;
                        for (; ZongKaiGuan;)
                        {
                            JiaoYanJieGuoModel chenggong = JiHeSheBei.Cerate().GetXieJieGuo(xiemodel);
                            if (chenggong.IsZuiZhongJieGuo != JieGuoType.JingXingZhong)
                            {
                                if (chenggong.Value.ToString() == zhi.ToString())
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
                                JiHeSheBei.Cerate().XieShuJu(gwid, xiemodel);
                                yanzhong = DateTime.Now;
                                cishu--;
                            }
                            Thread.Sleep(10);
                        }
                    }
                }
            }
           
        }



        /// <summary>
        /// 写入数据   1-清零值 2-是合格值 3-不合格值  0是需要参数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="shebeiid"></param>
        public void XieRuWuJiaoYanData(CaoZuoType dataType, object zhi, int leixing, int gwid)
        {
            if (dataType == CaoZuoType.DataShangChuan)
            {
                return;
            }
            List<YeWuDataModel> shujumodel = DataJiHe.Cerate().GetQiTaDataModel(gwid, dataType, true);
            if (shujumodel.Count > 0)
            {
                int count = shujumodel.Count;
                for (int i = 0; i < count; i++)
                {
                    XieRuMolde xiemodel = GetXieModel(shujumodel[i].Value);
                    if (xiemodel != null)
                    {
                        if (leixing == 0)
                        {
                            xiemodel.Zhi = zhi;
                        }
                        else if (leixing == 1)
                        {
                            xiemodel.Zhi = shujumodel[i].QingLingZhi;
                        }
                        else if (leixing == 2)
                        {
                            xiemodel.Zhi = shujumodel[i].PassZhi;
                        }
                        else if (leixing == 3)
                        {
                            xiemodel.Zhi = shujumodel[i].NGZhi;
                        }
                        JiHeSheBei.Cerate().XieShuJu(gwid, xiemodel);
                      
                    }
                }
            }

        }


        /// <summary>
        /// 用于操作设备的写
        /// </summary>
        /// <param name="id"></param>
        /// <param name="shebeiid"></param>
        public bool XieRuZhiXieData(CaoZuoType dataType,string zhi, int gwid,out string dushu)
        {
            dushu = "";
            if (dataType == CaoZuoType.DataShangChuan)
            {
                return false;
            }
            List<YeWuDataModel> shujumodel = DataJiHe.Cerate().GetQiTaDataModel(gwid, dataType, true);
            if (shujumodel.Count > 0)
            {
                if (shujumodel.Count == 1)
                {
                    XieRuMolde xiemodel = GetXieModel(shujumodel[0].Value);
                    if (xiemodel != null)
                    {
                        xiemodel.Zhi = zhi;
                        JiHeSheBei.Cerate().XieShuJu(gwid, xiemodel);
                        for (; ZongKaiGuan;)
                        {
                            Thread.Sleep(3);
                            JiaoYanJieGuoModel chenggong = JiHeSheBei.Cerate().GetXieJieGuo(xiemodel);
                            if (chenggong.IsZuiZhongJieGuo != JieGuoType.JingXingZhong)
                            {
                                if (chenggong.IsZuiZhongJieGuo == JieGuoType.ChengGongJiGuo)
                                {
                                    string jieguo = chenggong.Value.ToString();
                                    if (jieguo.Contains(shujumodel[0].QingLingZhi))
                                    {
                                        dushu = jieguo;
                                        return true;
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                            }


                        }
                    }
                }
                else
                {
                    int count = shujumodel.Count;
                    List<Task> jieherenwu = new List<Task>();
                    List<bool> ShuJu = new List<bool>();
                    StringBuilder sb = new StringBuilder(500);
                    for (int i = 0; i < count; i++)
                    {
                        ShuJu.Add(false);
                     
                      
                        jieherenwu.Add(Task.Factory.StartNew((x) =>
                        {
                            int index= ChangYong.TryInt(x,-1);
                            if (index>=0&& index< shujumodel.Count)
                            {
                                XieRuMolde xiemodel = GetXieModel(shujumodel[index].Value);
                                xiemodel.Zhi = zhi;
                                JiHeSheBei.Cerate().XieShuJu(gwid, xiemodel);
                                for (; ZongKaiGuan;)
                                {
                                    Thread.Sleep(3);
                                    JiaoYanJieGuoModel chenggong = JiHeSheBei.Cerate().GetXieJieGuo(xiemodel);
                                    if (chenggong.IsZuiZhongJieGuo != JieGuoType.JingXingZhong)
                                    {
                                        if (chenggong.IsZuiZhongJieGuo == JieGuoType.ChengGongJiGuo)
                                        {
                                            string jieguo = chenggong.Value.ToString();
                                            if (jieguo.Contains(shujumodel[index].QingLingZhi))
                                            {
                                                sb.Append(shujumodel[index].Value.ToString());
                                                ShuJu[index] = true;
                                            }
                                        }
                                        break;
                                    }


                                }
                            }
                           

                        },i));
                    }
                    Task.WaitAll(jieherenwu.ToArray());
                    dushu=sb.ToString();
                    if (ShuJu.IndexOf(false)>=0)
                    {
                        return false;
                    }
                    return true;
                }
               
            }
            return false;
        }


        /// <summary>
        /// 写入数据   1-清零值 2-是合格值 3-不合格值  0是需要参数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="shebeiid"></param>
        public void XieRuJiaoYanData(ShuJuLisModel shuju, object zhis,int gwid)
        {
            XieRuMolde xiemodel = GetXieModel(shuju);
            if (xiemodel != null)
            {
                xiemodel.Zhi = zhis;
                string zhi = xiemodel.Zhi.ToString();
                JiHeSheBei.Cerate().XieShuJu(gwid, xiemodel);
                int cishu = 3;
                DateTime yanzhong = DateTime.Now;
                for (; ZongKaiGuan;)
                {
                    JiaoYanJieGuoModel chenggong = JiHeSheBei.Cerate().GetXieJieGuo(xiemodel);
                    if (chenggong.IsZuiZhongJieGuo != JieGuoType.JingXingZhong)
                    {
                        if (chenggong.Value.ToString() == zhi.ToString())
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
                        JiHeSheBei.Cerate().XieShuJu(gwid, xiemodel);
                        yanzhong = DateTime.Now;
                        cishu--;
                    }
                    Thread.Sleep(10);
                }
            }

        }

        private XieRuMolde GetXieModel(ShuJuLisModel shujumodel)
        {
            ShuJuLisModel xie = shujumodel;
            if (xie != null)
            {
                if (xie.SheBeiID >= 0 && string.IsNullOrEmpty(xie.JCQStr) == false)
                {
                    XieRuMolde model = new XieRuMolde();
                    model.SheBeiID = xie.SheBeiID;
                    model.JiCunQiWeiYiBiaoShi = xie.JCQStr;
                    model.Zhi = "";
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
