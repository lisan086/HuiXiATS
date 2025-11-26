using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ATSJianMianJK.GongNengLei;
using ATSJianMianJK.Log;
using ATSJianMianJK.QuanXian;
using ATSLaoHuaUI.Model;
using CommLei.DataChuLi;
using CommLei.JiChuLei;
using SSheBei.Model;
using ZuZhuangUI.Model;
using ZuZhuangUI.PeiZhi.Frm;

namespace ZuZhuangUI.Lei
{
    public  class DataJiHe : JiHeData
    {
        /// <summary>
        /// true是开启刷新数据
        /// </summary>
        private bool IsKaiQi = false;
        /// <summary>
        /// 配方的名称
        /// </summary>
        public string PeiFangName { get; set; } = "";
     
        /// <summary>
        /// 每个工位的数据
        /// </summary>
        private Dictionary<string, List<ShuJuLisModel>> MeiGeGongWei = new Dictionary<string, List<ShuJuLisModel>>();

        /// <summary>
        /// 系统参数
        /// </summary>
        public XiTongModel XiTongModel { get; set; } = new XiTongModel();

        public List<SheBeiZhanModel> LisSheBeiBianHao = new List<SheBeiZhanModel>();

        #region 单利
        private readonly static object _DuiXiang = new object();

        private static DataJiHe _LogTxt = null;



        private DataJiHe()
        {
            FenPeiData();
        }
        /// <summary>
        /// 单例类，必须KaiqiRiZhi设置为True才能写日志
        /// </summary>
        /// <returns>返回NewXieRiZhiLog</returns>
        public static DataJiHe Cerate()
        {
            if (_LogTxt == null)
            {
                lock (_DuiXiang)
                {
                    if (_LogTxt == null)
                    {
                        _LogTxt = new DataJiHe();
                    }
                }
            }
            return _LogTxt;
        }
        #endregion
        /// <summary>
        /// 子分配
        /// </summary>
        protected override void ZiFenPeiData()
        {         
            MeiGeGongWei.Clear();
            LisSheBeiBianHao.Clear();
            PeiFangChuLi peiFangChuLi = new PeiFangChuLi();
            string WenJianLuJing = HCDanGeDataLei<PeiFangXuanZeModel>.Ceratei().LisWuLiao.LuJin;
            peiFangChuLi.JiaZaiPeiFang(WenJianLuJing);
            PeiFangName = ChangYong.GetWenJianName(WenJianLuJing);      
            for (int i = 0; i < peiFangChuLi.BTKLineModel.Count; i++)
            {
                SheBeiZhanModel shebianhao = ChangYong.FuZhiShiTi(peiFangChuLi.BTKLineModel[i]);
                shebianhao.IsKeYiShouDongTest = 0;
                shebianhao.IsGuZhang = false;
                LisSheBeiBianHao.Add(shebianhao);
                {
                    List<YeWuDataModel> shuju = shebianhao.LisQingQiu;
                    if (shuju != null && shuju.Count > 0)
                    {
                        for (int c = 0; c < shuju.Count; c++)
                        {
                                             
                            if (string.IsNullOrEmpty(shuju[c].Value.JCQStr) == false && shuju[c].Value.SheBeiID >= 0)
                            {
                                string keys = $"{shuju[c].Value.SheBeiID}:{shuju[c].Value.JCQStr}";
                                if (MeiGeGongWei.ContainsKey(keys) == false)
                                {
                                    MeiGeGongWei.Add(keys, new List<ShuJuLisModel>());
                                }
                                MeiGeGongWei[keys].Add(shuju[c].Value);

                            }

                        }
                     
                        
                    }
                }
                {
                    List<MaTDModel> shuju = shebianhao.LisMaTD;
                    if (shuju != null && shuju.Count > 0)
                    {
                        for (int c = 0; c < shuju.Count; c++)
                        {
                            List<YeWuDataModel> shujus = shuju[c].LisData;
                            foreach (var item in shujus)
                            {
                                if (string.IsNullOrEmpty(item.Value.JCQStr) == false && item.Value.SheBeiID >= 0)
                                {
                                    string keys = $"{item.Value.SheBeiID}:{item.Value.JCQStr}";
                                    if (MeiGeGongWei.ContainsKey(keys) == false)
                                    {
                                        MeiGeGongWei.Add(keys, new List<ShuJuLisModel>());
                                    }
                                    MeiGeGongWei[keys].Add(item.Value);

                                }
                            }
                          

                        }

                    }
                }
            }
            IsKaiQi = true;
        }

        protected override void ZiClose()
        {
            IsKaiQi = false;
            MeiGeGongWei.Clear();

            LisSheBeiBianHao.Clear();
        }
        protected override void ZiShuaXinData(List<JiCunQiModel> lismodel)
        {
            if (IsKaiQi)
            {
                foreach (var item in lismodel)
                {
                    string keys = $"{item.SheBeiID}:{item.WeiYiBiaoShi}";
                    if (MeiGeGongWei.ContainsKey(keys))
                    {
                        List<ShuJuLisModel> xinshuju = MeiGeGongWei[keys];
                        for (int c = 0; c < xinshuju.Count; c++)
                        {
                            xinshuju[c].JiCunValue =ChangYong.TryStr(item.Value,"");
                            xinshuju[c].IsKeKao = item.IsKeKao;
                        }
                    }
                }
                for (int i = 0; i < LisSheBeiBianHao.Count; i++)
                {
                    LisSheBeiBianHao[i].ShiShiWenDu = GetGWZhiFloat(LisSheBeiBianHao[i].GWID,CaoZuoType.ZongDuWenDu, 0);
                    LisSheBeiBianHao[i].IsGuZhang = GetGWPiPei(LisSheBeiBianHao[i].GWID, CaoZuoType.ZongGuZhan);
                    LisSheBeiBianHao[i].IsYunXing = GetGWPiPei(LisSheBeiBianHao[i].GWID, CaoZuoType.ZongYunXingState)?1:0;
                }
            }
          
        }

        /// <summary>
        /// 获取工位的值
        /// </summary>
        /// <param name="gwid"></param>
        /// <param name="caoZuo"></param>
        /// <param name="shibaizhi"></param>
        /// <returns></returns>
        public int GetGWZhiInt(int gwid, CaoZuoType caoZuo, int shibaizhi)
        {
            for (int i = 0; i < LisSheBeiBianHao.Count; i++)
            {
                if (LisSheBeiBianHao[i].GWID == gwid)
                {
                    {
                        List<YeWuDataModel> lis = LisSheBeiBianHao[i].LisQingQiu;
                        foreach (var item in lis)
                        {
                            if (item.CaoZuoType == caoZuo)
                            {
                                return ChangYong.TryInt(item.Value.JiCunValue, shibaizhi);
                            }
                        }
                    }
                    {
                        List<MaTDModel> lis = LisSheBeiBianHao[i].LisMaTD;
                        for (int c = 0; c < lis.Count; c++)
                        {
                            {
                                List<YeWuDataModel> ss = lis[c].LisKongZhi;
                                foreach (var item in ss)
                                {
                                    if (item.CaoZuoType == caoZuo)
                                    {
                                        return ChangYong.TryInt(item.Value.JiCunValue, shibaizhi);
                                    }
                                }
                            }
                            {
                                List<YeWuDataModel> ss = lis[c].LisData;
                                foreach (var item in ss)
                                {
                                    if (item.CaoZuoType == caoZuo)
                                    {
                                        return ChangYong.TryInt(item.Value.JiCunValue, shibaizhi);
                                    }
                                }
                            }
                        }
                       
                    }
                    break;
                }
            }
          
            return shibaizhi;
        }

        /// <summary>
        /// 获取工位的值
        /// </summary>
        /// <param name="gwid"></param>
        /// <param name="caoZuo"></param>
        /// <param name="shibaizhi"></param>
        /// <returns></returns>
        public float GetGWZhiFloat(int gwid, CaoZuoType caoZuo, float shibaizhi)
        {
            for (int i = 0; i < LisSheBeiBianHao.Count; i++)
            {
                if (LisSheBeiBianHao[i].GWID == gwid)
                {
                    {
                        List<YeWuDataModel> lis = LisSheBeiBianHao[i].LisQingQiu;
                        foreach (var item in lis)
                        {
                            if (item.CaoZuoType == caoZuo)
                            {
                                return ChangYong.TryFloat(item.Value.JiCunValue, shibaizhi);
                            }
                        }
                    }
                    {
                        List<MaTDModel> lis = LisSheBeiBianHao[i].LisMaTD;
                        for (int c = 0; c < lis.Count; c++)
                        {
                            {
                                List<YeWuDataModel> ss = lis[c].LisKongZhi;
                                foreach (var item in ss)
                                {
                                    if (item.CaoZuoType == caoZuo)
                                    {
                                        return ChangYong.TryFloat(item.Value.JiCunValue, shibaizhi);
                                    }
                                }
                            }
                            {
                                List<YeWuDataModel> ss = lis[c].LisData;
                                foreach (var item in ss)
                                {
                                    if (item.CaoZuoType == caoZuo)
                                    {
                                        return ChangYong.TryFloat(item.Value.JiCunValue, shibaizhi);
                                    }
                                }
                            }
                        }

                    }
                    break;
                }
            }

            return shibaizhi;
        }
        /// <summary>
        /// 获取工位的值
        /// </summary>
        /// <param name="gwid"></param>
        /// <param name="caoZuo"></param>
        /// <param name="shibaizhi"></param>
        /// <returns></returns>
        public string GetGWZhiStr(int gwid, CaoZuoType caoZuo, string shibaizhi)
        {
            for (int i = 0; i < LisSheBeiBianHao.Count; i++)
            {
                if (LisSheBeiBianHao[i].GWID == gwid)
                {
                    {
                        List<YeWuDataModel> lis = LisSheBeiBianHao[i].LisQingQiu;
                        foreach (var item in lis)
                        {
                            if (item.CaoZuoType == caoZuo)
                            {
                                return ChangYong.TryStr(item.Value.JiCunValue, shibaizhi);
                            }
                        }
                    }
                    {
                        {
                            List<MaTDModel> lis = LisSheBeiBianHao[i].LisMaTD;
                            for (int c = 0; c < lis.Count; c++)
                            {
                                {
                                    List<YeWuDataModel> ss = lis[c].LisKongZhi;
                                    foreach (var item in ss)
                                    {
                                        if (item.CaoZuoType == caoZuo)
                                        {
                                            return ChangYong.TryStr(item.Value.JiCunValue, shibaizhi);
                                        }
                                    }
                                }
                                {
                                    List<YeWuDataModel> ss = lis[c].LisData;
                                    foreach (var item in ss)
                                    {
                                        if (item.CaoZuoType == caoZuo)
                                        {
                                            return ChangYong.TryStr(item.Value.JiCunValue, shibaizhi);
                                        }
                                    }
                                }
                            }

                        }
                    }
                    break;
                }
            }

            return shibaizhi;
        }

        public bool GetGWPiPei(int gwid, CaoZuoType caoZuo)
        {
            YeWuDataModel model = GetModel(gwid,caoZuo,false);
            if (model != null)
            {
                List<string> lis = ChangYong.JieGeStr(model.QingQiuPiPei, ',');
                if (string.IsNullOrEmpty(model.Value.JiCunValue))
                {
                    return false;
                }
                else
                {
                    if (lis.IndexOf(model.Value.JiCunValue) >= 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="gwid"></param>
        /// <param name="caoZuo"></param>
        /// <returns></returns>
        public YeWuDataModel GetModel(int gwid, CaoZuoType caoZuo,bool isfuzhi)
        {
            for (int i = 0; i < LisSheBeiBianHao.Count; i++)
            {
                if (LisSheBeiBianHao[i].GWID == gwid)
                {
                    {
                        List<YeWuDataModel> lis = LisSheBeiBianHao[i].LisQingQiu;
                        foreach (var item in lis)
                        {
                            if (item.CaoZuoType == caoZuo)
                            {
                                if (isfuzhi)
                                {
                                    return ChangYong.FuZhiShiTi(item);
                                }
                                else
                                {
                                    return item;
                                }
                            }
                        }
                    }
                    {
                        List<MaTDModel> lis = LisSheBeiBianHao[i].LisMaTD;
                        for (int c = 0; c < lis.Count; c++)
                        {
                            {
                                List<YeWuDataModel> ss = lis[c].LisKongZhi;
                                foreach (var item in ss)
                                {
                                    if (item.CaoZuoType == caoZuo)
                                    {
                                        if (isfuzhi)
                                        {
                                            return ChangYong.FuZhiShiTi(item);
                                        }
                                        else
                                        {
                                            return item;
                                        }
                                    }
                                }
                            }
                            {
                                List<YeWuDataModel> ss = lis[c].LisData;
                                foreach (var item in ss)
                                {
                                    if (item.CaoZuoType == caoZuo)
                                    {
                                        if (isfuzhi)
                                        {
                                            return ChangYong.FuZhiShiTi(item);
                                        }
                                        else
                                        {
                                            return item;
                                        }
                                    }
                                }
                            }
                        }

                    }
                    break;
                }
            }

         
            return null;
        }



        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="gwid"></param>
        /// <param name="caoZuo"></param>
        /// <returns></returns>
        public List<YeWuDataModel> GetDataModel(int gwid, CaoZuoType caoZuo,bool isfuzhi)
        {
            List<YeWuDataModel> shuju = new List<YeWuDataModel>();
            for (int i = 0; i < LisSheBeiBianHao.Count; i++)
            {
                if (LisSheBeiBianHao[i].GWID == gwid)
                {
                    {
                        List<YeWuDataModel> lis = LisSheBeiBianHao[i].LisQingQiu;
                        foreach (var item in lis)
                        {
                            if (item.CaoZuoType == caoZuo)
                            {
                                if (isfuzhi)
                                {
                                    shuju.Add( ChangYong.FuZhiShiTi(item));
                                }
                                else
                                {
                                    shuju.Add(item);
                                }
                            }
                        }
                    }
                    {
                        List<MaTDModel> lis = LisSheBeiBianHao[i].LisMaTD;
                        for (int c = 0; c < lis.Count; c++)
                        {
                            {
                                List<YeWuDataModel> ss = lis[c].LisKongZhi;
                                foreach (var item in ss)
                                {
                                    if (item.CaoZuoType == caoZuo)
                                    {
                                        if (isfuzhi)
                                        {
                                            shuju.Add(ChangYong.FuZhiShiTi(item));
                                        }
                                        else
                                        {
                                            shuju.Add(item);
                                        }
                                    }
                                }
                            }
                            {
                                List<YeWuDataModel> ss = lis[c].LisData;
                                foreach (var item in ss)
                                {
                                    if (item.CaoZuoType == caoZuo)
                                    {
                                        if (isfuzhi)
                                        {
                                            shuju.Add(ChangYong.FuZhiShiTi(item));
                                        }
                                        else
                                        {
                                            shuju.Add(item);
                                        }
                                    }
                                }
                            }
                        }

                    }
                    break;
                }
            }

            return shuju;
        }

    
        public List<string> GetPeiFangNames()
        {
            PeiFangChuLi peiFangChuLi = new PeiFangChuLi();
            return peiFangChuLi.GetPeiFang(true);
        }


        public void QiHuanPeiFang(string lujing)
        {
            IsKaiQi = false;
            Thread.Sleep(500);
            PeiFangChuLi peiFangChuLi = new PeiFangChuLi();
            peiFangChuLi.SetDanQianBaoPeiFang(lujing);        
            ZiFenPeiData();
            RiJiLog.Cerate().Add(RiJiEnum.TDXieJiLu, $"切换配方:{lujing}", -1);
        }
    }
}
