using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ATSFoZhaoZuZhuangUI.Model;
using ATSJianMianJK.GongNengLei;
using ATSJianMianJK.Log;
using CommLei.DataChuLi;
using CommLei.JiChuLei;
using Common.DataChuLi;
using SSheBei.Model;
using SSheBei.ZongKongZhi;
using ZuZhuangUI.Lei;
using ZuZhuangUI.Model;

namespace ATSFoZhaoZuZhuangUI.Lei
{
    public  class HuanXingLei
    {
        /// <summary>
        /// 工作开关
        /// </summary>
        private bool GongZuoWork = false;
        private bool ZongKaiGuan = true;
        public string MuQianXingHao { get; set; } = "";
        #region 单例
        private static HuanXingLei _LogTxt = null;
        private readonly static object _DuiXiang = new object();

     

        private HuanXingLei()
        {
            if (DataJiHe.Cerate().LisSheBeiBianHao.Count > 0)
            {
                MuQianXingHao = DataJiHe.Cerate().LisSheBeiBianHao[0].BangDingMa;
            }
            Thread thread = new Thread(Work);
            thread.IsBackground = true;
            thread.DisableComObjectEagerCleanup();
            thread.Start();
        }

       

        /// <summary>
        /// 单例类，必须KaiqiRiZhi设置为True才能写日志
        /// </summary>
        /// <returns>返回NewXieRiZhiLog</returns>
        public static HuanXingLei Ceratei()
        {
            if (_LogTxt == null)
            {
                lock (_DuiXiang)//线程锁
                {
                    if (_LogTxt == null)
                    {
                        _LogTxt = new HuanXingLei();
                    }
                }
            }
            return _LogTxt;
        }
        #endregion

        private void Work()
        {
            XiTongModel xitongmodel = DataJiHe.Cerate().XiTongModel;
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
                    if (xitongmodel.QiDongXinHaoName.JiCunValue.ToString().Equals(xitongmodel.PiPeiZhi))
                    {
                        if ((DateTime.Now - JiShiTime).TotalMilliseconds >= 200)
                        {
                            if (xitongmodel.IsQieHuan)
                            {
                                string mas = xitongmodel.BangDingHuanXing.JiCunValue.ToString();
                                PeiFangChuLi peiFangChuLi = new PeiFangChuLi();
                                List<string> peifangming = peiFangChuLi.GetPeiFangNames().Keys.ToList();
                                bool iszai = false;
                                switch (xitongmodel.PanDuanMoShi)
                                {
                                    case PanDuanMoShi.QianBaoHan:
                                        {
                                            if (mas.StartsWith(MuQianXingHao))
                                            {
                                                iszai = true;
                                            }
                                        }
                                        break;
                                    case PanDuanMoShi.HouBaoHan:
                                        if (mas.EndsWith(MuQianXingHao))
                                        {
                                            iszai = true;
                                        }
                                        break;
                                    case PanDuanMoShi.DengYu:
                                        {
                                            if (mas.Equals(MuQianXingHao))
                                            {
                                                iszai = true;
                                            }
                                        }
                                        break;
                                    case PanDuanMoShi.BaoHan:
                                        if (mas.Contains(MuQianXingHao))
                                        {
                                            iszai = true;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                                if (iszai == false)
                                {
                                    bool isxiele = false;
                                    for (int i = 0; i < peifangming.Count; i++)
                                    {
                                        peiFangChuLi.JiaZaiPeiFang(peifangming[i]);
                                        List<SheBeiZhanModel> shuju = peiFangChuLi.BTKLineModel;
                                        if (shuju.Count > 0)
                                        {
                                            string qiehuanma = shuju[0].BangDingMa;
                                            bool isbaohan = false;
                                            switch (xitongmodel.PanDuanMoShi)
                                            {
                                                case PanDuanMoShi.QianBaoHan:
                                                    {
                                                        if (mas.StartsWith(qiehuanma))
                                                        {
                                                            isbaohan = true;
                                                        }
                                                    }
                                                    break;
                                                case PanDuanMoShi.HouBaoHan:
                                                    if (mas.EndsWith(qiehuanma))
                                                    {
                                                        isbaohan = true;
                                                    }
                                                    break;
                                                case PanDuanMoShi.DengYu:
                                                    if (mas.Equals(qiehuanma))
                                                    {
                                                        isbaohan = true;
                                                    }
                                                    break;
                                                case PanDuanMoShi.BaoHan:
                                                    if (mas.Contains(qiehuanma))
                                                    {
                                                        isbaohan = true;
                                                    }
                                                    break;
                                                default:
                                                    break;
                                            }
                                            if (isbaohan)
                                            {
                                                JieMianCaoZuoModel ssssd = new JieMianCaoZuoModel();
                                                ssssd.CanShu = peifangming[i];
                                                JieMianCaoZuoLei.CerateDanLi().JieMianCuoZuo(DoType.HuanPeiFang, ssssd);
                                                Thread.Sleep(50);
                                                MuQianXingHao = qiehuanma;
                                                SheBeiJiHe.Cerate().XieRuJiaoYanData(xitongmodel.XieShuJu, xitongmodel.PassZhi, 1);
                                                isxiele = true;
                                                break;
                                            }
                                        }
                                    }
                                    if (isxiele==false)
                                    {
                                        SheBeiJiHe.Cerate().XieRuJiaoYanData(xitongmodel.XieShuJu, xitongmodel.NGZhi, 1);
                                    }
                                }
                                else
                                {
                                    SheBeiJiHe.Cerate().XieRuJiaoYanData(xitongmodel.XieShuJu, xitongmodel.PassZhi, 1);
                                }
                            }
                            else
                            {
                                SheBeiJiHe.Cerate().XieRuJiaoYanData(xitongmodel.XieShuJu,xitongmodel.PassZhi,1);
                            }
                            JiShiTime = DateTime.Now;

                        }
                    }
                    else
                    {
                        JiShiTime = DateTime.Now;
                    }

                }
                catch (Exception ex)
                {
                    RiJiLog.Cerate().Add(RiJiEnum.TDXieJiLu, $"切换配方:发生错误{ex}", -1);
                }
                Thread.Sleep(5);
            }
        }

        public void Open()
        {
          
            GongZuoWork = true;

        }
        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            ZongKaiGuan = false;
           
        }
    }
}
