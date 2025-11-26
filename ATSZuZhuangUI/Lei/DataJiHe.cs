using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ATSFoZhaoZuZhuangUI.Model;
using ATSJianMianJK.GongNengLei;
using ATSJianMianJK.Log;
using ATSJianMianJK.QuanXian;
using CommLei.DataChuLi;
using CommLei.JiChuLei;
using SSheBei.Model;
using ZuZhuangUI.Model;
using ZuZhuangUI.PeiZhi.Frm;

namespace ZuZhuangUI.Lei
{
    public  class DataJiHe : JiHeData
    {
        private bool IsKaiQi = false;
        public string PeiFangName { get; set; } = "";
     
     

        /// <summary>
        /// 每个工位的数据
        /// </summary>
        private Dictionary<string, List<ShuJuLisModel>> MeiGeGongWei = new Dictionary<string, List<ShuJuLisModel>>();

        /// <summary>
        /// 每个工位的数据
        /// </summary>
        private Dictionary<string, List<ShuJuLisModel>> XiTongData = new Dictionary<string, List<ShuJuLisModel>>();


        public List<SheBeiZhanModel> LisSheBeiBianHao = new List<SheBeiZhanModel>();

        public XiTongModel XiTongModel { get; set; } = new XiTongModel();

        #region 单利
        private readonly static object _DuiXiang = new object();

        private static DataJiHe _LogTxt = null;



        private DataJiHe()
        {
            XiTongModel = ChangYong.FuZhiShiTi(HCDanGeDataLei<XiTongModel>.Ceratei().LisWuLiao);
            {
                string keys = $"{XiTongModel.QiDongXinHaoName.SheBeiID}:{XiTongModel.QiDongXinHaoName.JCQStr}";
                if (XiTongData.ContainsKey(keys) == false)
                {
                    XiTongData.Add(keys, new List<ShuJuLisModel>());
                }
                XiTongData[keys].Add(XiTongModel.QiDongXinHaoName);
            }
            {
                string keys = $"{XiTongModel.BangDingHuanXing.SheBeiID}:{XiTongModel.BangDingHuanXing.JCQStr}";
                if (XiTongData.ContainsKey(keys) == false)
                {
                    XiTongData.Add(keys, new List<ShuJuLisModel>());
                }
                XiTongData[keys].Add(XiTongModel.BangDingHuanXing);
            }
            {
                string keys = $"{XiTongModel.XieShuJu.SheBeiID}:{XiTongModel.XieShuJu.JCQStr}";
                if (XiTongData.ContainsKey(keys) == false)
                {
                    XiTongData.Add(keys, new List<ShuJuLisModel>());
                }
                XiTongData[keys].Add(XiTongModel.XieShuJu);
            }
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
                LisSheBeiBianHao.Add(shebianhao);
                {
                    List<YeWuDataModel> shuju = shebianhao.LisData;
                    if (shuju != null && shuju.Count > 0)
                    {
                        for (int c = 0; c < shuju.Count; c++)
                        {
                            if (string.IsNullOrEmpty(shuju[c].Up.JCQStr)==false&& shuju[c].Up.SheBeiID>=0)
                            {
                                string keys = $"{shuju[c].Up.SheBeiID}:{shuju[c].Up.JCQStr}";
                                if (MeiGeGongWei.ContainsKey(keys) == false)
                                {
                                    MeiGeGongWei.Add(keys, new List<ShuJuLisModel>());
                                }
                                MeiGeGongWei[keys].Add(shuju[c].Up);
                            }
                            if (string.IsNullOrEmpty(shuju[c].Low.JCQStr) == false && shuju[c].Low.SheBeiID >= 0)
                            {
                                string keys = $"{shuju[c].Low.SheBeiID}:{shuju[c].Low.JCQStr}";
                                if (MeiGeGongWei.ContainsKey(keys) == false)
                                {
                                    MeiGeGongWei.Add(keys, new List<ShuJuLisModel>());
                                }
                                MeiGeGongWei[keys].Add(shuju[c].Low);

                            }
                            if (string.IsNullOrEmpty(shuju[c].State.JCQStr) == false && shuju[c].State.SheBeiID >= 0)
                            {
                                string keys = $"{shuju[c].State.SheBeiID}:{shuju[c].State.JCQStr}";
                                if (MeiGeGongWei.ContainsKey(keys) == false)
                                {
                                    MeiGeGongWei.Add(keys, new List<ShuJuLisModel>());
                                }
                                MeiGeGongWei[keys].Add(shuju[c].State);

                            }
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
                            if (xinshuju[c].IsStr)
                            {
                                string wenben = ChangYong.TryStr(item.Value,"");
                                int qishiweizhi = xinshuju[c].StrKaiShi;
                                int count = xinshuju[c].StrCount;
                                int zongshuling = wenben.Length;
                                int hansheng = zongshuling - qishiweizhi;
                                if (hansheng >= count)
                                {
                                    xinshuju[c].JiCunValue = wenben.Substring(qishiweizhi, count);
                                }
                                else
                                {
                                    xinshuju[c].JiCunValue = wenben.Substring(qishiweizhi, hansheng);
                                }
                            }
                            else
                            {
                                xinshuju[c].JiCunValue = item.Value;
                            }
                        }
                    }

                    if (XiTongData.ContainsKey(keys))
                    {
                        List<ShuJuLisModel> xinshuju = XiTongData[keys];
                        for (int c = 0; c < xinshuju.Count; c++)
                        {
                            if (xinshuju[c].IsStr)
                            {
                                string wenben = ChangYong.TryStr(item.Value, "");
                                int qishiweizhi = xinshuju[c].StrKaiShi;
                                int count = xinshuju[c].StrCount;
                                int zongshuling = wenben.Length;
                                int hansheng = zongshuling - qishiweizhi;
                                if (hansheng >= count)
                                {
                                    xinshuju[c].JiCunValue = wenben.Substring(qishiweizhi, count);
                                }
                                else
                                {
                                    xinshuju[c].JiCunValue = wenben.Substring(qishiweizhi, hansheng);
                                }
                            }
                            else
                            {
                                xinshuju[c].JiCunValue = item.Value;
                            }
                        }
                    }
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
                    List<YeWuDataModel> lis = LisSheBeiBianHao[i].LisQingQiu;
                    foreach (var item in lis)
                    {
                        if (item.CaoZuoType== caoZuo)
                        {
                            return ChangYong.TryInt(item.Value.JiCunValue, shibaizhi);
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
                    List<YeWuDataModel> lis = LisSheBeiBianHao[i].LisQingQiu;
                    foreach (var item in lis)
                    {
                        if (item.CaoZuoType == caoZuo)
                        {
                            return ChangYong.TryFloat(item.Value.JiCunValue, shibaizhi);
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
                    List<YeWuDataModel> lis = LisSheBeiBianHao[i].LisQingQiu;
                    foreach (var item in lis)
                    {
                        if (item.CaoZuoType == caoZuo)
                        {
                            return ChangYong.TryStr(item.Value.JiCunValue, shibaizhi);
                        }
                    }
                    break;
                }
            }

            return shibaizhi;
        }

        public bool GetGWPiPei(int gwid, CaoZuoType caoZuo)
        {
            YeWuDataModel model = GetShouGeModel(gwid,caoZuo,false);
            if (model != null)
            {
                List<string> lis = ChangYong.JieGeStr(model.QingQiuPiPei, ',');
                if (string.IsNullOrEmpty(model.Value.JiCunValue.ToString()))
                {
                    return false;
                }
                else
                {
                    if (lis.IndexOf(model.Value.JiCunValue.ToString()) >= 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }

        public bool GetGWPiPei(YeWuDataModel model)
        {
           
            if (model != null)
            {
                List<string> lis = ChangYong.JieGeStr(model.QingQiuPiPei, ',');
                if (string.IsNullOrEmpty(model.Value.JiCunValue.ToString()))
                {
                    return false;
                }
                else
                {
                    if (lis.IndexOf(model.Value.JiCunValue.ToString()) >= 0)
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
        public YeWuDataModel GetShouGeModel(int gwid, CaoZuoType caoZuo,bool isfuzhi)
        {
            for (int i = 0; i < LisSheBeiBianHao.Count; i++)
            {
                if (LisSheBeiBianHao[i].GWID == gwid)
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
                    List<YeWuDataModel> lis = LisSheBeiBianHao[i].LisData;
                    foreach (var item in lis)
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
                    break;
                }
            }

            return shuju;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="gwid"></param>
        /// <param name="caoZuo"></param>
        /// <returns></returns>
        public List<YeWuDataModel> GetDataModel( CaoZuoType caoZuo, bool isfuzhi)
        {
            List<YeWuDataModel> shuju = new List<YeWuDataModel>();
            for (int i = 0; i < LisSheBeiBianHao.Count; i++)
            {
                List<YeWuDataModel> lis = LisSheBeiBianHao[i].LisData;
                foreach (var item in lis)
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

            return shuju;
        }


        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="gwid"></param>
        /// <param name="caoZuo"></param>
        /// <returns></returns>
        public List<YeWuDataModel> GetQiTaDataModel(int gwid, CaoZuoType caoZuo, bool isfuzhi)
        {
            List<YeWuDataModel> shuju = new List<YeWuDataModel>();
            for (int i = 0; i < LisSheBeiBianHao.Count; i++)
            {
                if (LisSheBeiBianHao[i].GWID == gwid)
                {
                    List<YeWuDataModel> lis = LisSheBeiBianHao[i].LisQingQiu;
                    foreach (var item in lis)
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
                    break;
                }
            }

            return shuju;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="gwid"></param>
        /// <param name="caoZuo"></param>
        /// <returns></returns>
        public List<YeWuDataModel> GetQiTaDataModel(CaoZuoType caoZuo, bool isfuzhi)
        {
            List<YeWuDataModel> shuju = new List<YeWuDataModel>();
            for (int i = 0; i < LisSheBeiBianHao.Count; i++)
            {
                List<YeWuDataModel> lis = LisSheBeiBianHao[i].LisQingQiu;
                foreach (var item in lis)
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

            return shuju;
        }
        public List<string> GetPeiFangNames()
        {
            PeiFangChuLi peiFangChuLi = new PeiFangChuLi();
            return peiFangChuLi.GetPeiFangNames().Keys.ToList();
        }


        public void QiHuanPeiFang(string lujing)
        {
            IsKaiQi = false;
            Thread.Sleep(100);
            PeiFangChuLi peiFangChuLi = new PeiFangChuLi();
            peiFangChuLi.SetDanQianBaoPeiFang(lujing);        
            ZiFenPeiData();
            RiJiLog.Cerate().Add(RiJiEnum.TDXieJiLu, $"切换配方:{lujing}", -1);
        }
    }
}
