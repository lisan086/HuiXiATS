using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using ATSJianMianJK.Log;
using ATSJianMianJK.XiTong.Model;
using CommLei.DataChuLi;
using CommLei.JiChuLei;
using SSheBei.Model;

namespace ATSJianMianJK.GongNengLei
{
    public class JiHeData
    {
       
        private bool IsShuaXin = false;
     
        /// <summary>
        /// 通道状态
        /// </summary>
        public Dictionary<int, TDZhuangTaiModel> TDZhaungTai = new Dictionary<int, TDZhuangTaiModel>();
        /// <summary>
        /// 通道IO数据 key表示通道
        /// </summary>
        public Dictionary<int, List<DuIOCanShuModel>> TDIOShuJu = new Dictionary<int, List<DuIOCanShuModel>>();

        /// <summary>
        /// 通道数据 key表示通道
        /// </summary>
        public Dictionary<int, List<DuShuJuModel>> TDZhiShuJu = new Dictionary<int, List<DuShuJuModel>>();

        /// <summary>
        /// 通道写数据 key表示通道
        /// </summary>
        public Dictionary<int, List<XieSateModel>> TDXieShuJu = new Dictionary<int, List<XieSateModel>>();
        
        /// <summary>
        /// 所有的数据集合
        /// </summary>
        protected Dictionary<string, List<DuModel>> Datas = new Dictionary<string, List<DuModel>>();

        public List<int> ZhuagTaiTongDaoKey = new List<int>();
        /// <summary>
        /// 无参构造函数，数据集合
        /// </summary>
        public JiHeData()
        { 

        }

        /// <summary>
        /// 分配数据
        /// </summary>
        public void FenPeiData(bool ischufazishuju=true)
        {
            IsShuaXin = true;
          
            TDIOShuJu.Clear();
            TDZhiShuJu.Clear();
            TDXieShuJu.Clear();
            TDZhaungTai.Clear();
            Datas.Clear();
          
            {
                List<DuIOCanShuModel> lismodel = HCLisDataLei<DuIOCanShuModel>.Ceratei().LisWuLiao;
                for (int i = 0; i < lismodel.Count; i++)
                {
                    DuIOCanShuModel model = ChangYong.FuZhiShiTi(lismodel[i]);
                    int tdid = model.TDID;
                    if (TDIOShuJu.ContainsKey(tdid) == false)
                    {
                        TDIOShuJu.Add(tdid, new List<DuIOCanShuModel>());
                        TDZhaungTai.Add(tdid, new TDZhuangTaiModel());
                    }
                    TDIOShuJu[tdid].Add(model);
                    for (int c = 0; c < model.LisJiCunQi.Count; c++)
                    {
                        DuModel weiyibiaoshimodel = model.LisJiCunQi[c];
                        weiyibiaoshimodel.IsXie = false;
                        string key = $"{weiyibiaoshimodel.SheBeiID}:{weiyibiaoshimodel.JiCunQiName}";
                        if (Datas.ContainsKey(key) == false)
                        {
                            Datas.Add(key, new List<DuModel>());
                        }
                        Datas[key].Add(weiyibiaoshimodel);
                    }
                }
            }
            {
                List<DuShuJuModel> lismodel = HCLisDataLei<DuShuJuModel>.Ceratei().LisWuLiao;
                for (int i = 0; i < lismodel.Count; i++)
                {
                    DuShuJuModel model = ChangYong.FuZhiShiTi(lismodel[i]);
                    int tdid = model.TDID;
                    if (TDZhiShuJu.ContainsKey(tdid) == false)
                    {
                        TDZhiShuJu.Add(tdid, new List<DuShuJuModel>());
                    }
                    TDZhiShuJu[tdid].Add(model);
                    for (int c = 0; c < model.LisJiCunQi.Count; c++)
                    {
                        DuModel weiyibiaoshimodel = model.LisJiCunQi[c];
                        weiyibiaoshimodel.IsXie = false;
                        string key = $"{weiyibiaoshimodel.SheBeiID}:{weiyibiaoshimodel.JiCunQiName}";
                        if (Datas.ContainsKey(key) == false)
                        {
                            Datas.Add(key, new List<DuModel>());
                        }
                        Datas[key].Add(weiyibiaoshimodel);
                    }
                }
            }
            {
                List<XieSateModel> lismodel = HCLisDataLei<XieSateModel>.Ceratei().LisWuLiao;
                for (int i = 0; i < lismodel.Count; i++)
                {
                    XieSateModel model = ChangYong.FuZhiShiTi(lismodel[i]);
                    int tdid = model.TDID;
                    if (TDXieShuJu.ContainsKey(tdid) == false)
                    {
                        TDXieShuJu.Add(tdid, new List<XieSateModel>());
                    }
                    if (model.Type==XieSateType.ZiDongXie)
                    {
                        if (TDZhaungTai.ContainsKey(tdid) == false)
                        {                         
                            TDZhaungTai.Add(tdid, new TDZhuangTaiModel());
                        }
                        TDZhaungTai[tdid].ZiDongXieSates.Add(model);
                    }
                    TDXieShuJu[tdid].Add(model);
                    for (int c = 0; c < model.LisXies.Count; c++)
                    {
                        XieModel weiyibiaoshimodel = model.LisXies[c];
                        {
                            for (int d = 0; d < weiyibiaoshimodel.TiaoJianJiChu.Count; d++)
                            {
                                if (weiyibiaoshimodel.TiaoJianJiChu[d].ZBLModel.ZLeiXing == ZblLeiXing.DuJC || weiyibiaoshimodel.TiaoJianJiChu[d].ZBLModel.ZLeiXing == ZblLeiXing.XieJC)
                                {
                                    string key = $"{weiyibiaoshimodel.TiaoJianJiChu[d].ZBLModel.SheBeiID}:{weiyibiaoshimodel.TiaoJianJiChu[d].ZBLModel.ZBianLiangName}";
                                    if (Datas.ContainsKey(key) == false)
                                    {
                                        Datas.Add(key, new List<DuModel>());
                                    }
                                    Datas[key].Add(new DuModel() { JiCunQiName = weiyibiaoshimodel.TiaoJianJiChu[d].ZBLModel.ZBianLiangName, SheBeiID = weiyibiaoshimodel.TiaoJianJiChu[d].ZBLModel.SheBeiID, IsXie = true });
                                }
                                if (weiyibiaoshimodel.TiaoJianJiChu[d].YBLModel.ZLeiXing == YblLeiXing.DuJC)
                                {
                                    string key = $"{weiyibiaoshimodel.TiaoJianJiChu[d].YBLModel.SheBeiID}:{weiyibiaoshimodel.TiaoJianJiChu[d].YBLModel.YouCanShu}";
                                    if (Datas.ContainsKey(key) == false)
                                    {
                                        Datas.Add(key, new List<DuModel>());
                                    }
                                    Datas[key].Add(new DuModel() { JiCunQiName = weiyibiaoshimodel.TiaoJianJiChu[d].YBLModel.YouCanShu, SheBeiID = weiyibiaoshimodel.TiaoJianJiChu[d].YBLModel.SheBeiID, IsXie = true });
                                }
                            }
                           
                        }
                        {
                            for (int d = 0; d < weiyibiaoshimodel.FuZhiJiChu.Count; d++)
                            {
                                if (weiyibiaoshimodel.FuZhiJiChu[d].ZBLModel.ZLeiXing == ZblLeiXing.DuJC || weiyibiaoshimodel.FuZhiJiChu[d].ZBLModel.ZLeiXing == ZblLeiXing.XieJC)
                                {
                                    string key = $"{weiyibiaoshimodel.FuZhiJiChu[d].ZBLModel.SheBeiID}:{weiyibiaoshimodel.FuZhiJiChu[d].ZBLModel.ZBianLiangName}";
                                    if (Datas.ContainsKey(key) == false)
                                    {
                                        Datas.Add(key, new List<DuModel>());
                                    }
                                    Datas[key].Add(new DuModel() { JiCunQiName = weiyibiaoshimodel.FuZhiJiChu[d].ZBLModel.ZBianLiangName, SheBeiID = weiyibiaoshimodel.FuZhiJiChu[d].ZBLModel.SheBeiID, IsXie = true });
                                }

                                if (weiyibiaoshimodel.FuZhiJiChu[d].YBLModel.ZLeiXing == YblLeiXing.DuJC)
                                {
                                    string key = $"{weiyibiaoshimodel.FuZhiJiChu[d].YBLModel.SheBeiID}:{weiyibiaoshimodel.FuZhiJiChu[d].YBLModel.YouCanShu}";
                                    if (Datas.ContainsKey(key) == false)
                                    {
                                        Datas.Add(key, new List<DuModel>());
                                    }
                                    Datas[key].Add(new DuModel() { JiCunQiName = weiyibiaoshimodel.FuZhiJiChu[d].YBLModel.YouCanShu, SheBeiID = weiyibiaoshimodel.FuZhiJiChu[d].YBLModel.SheBeiID, IsXie = true });
                                }
                            }
                           
                        }
                       
                    }
                }
            }
            if (ischufazishuju)
            {
                ZiFenPeiData();
            }
            ZhuagTaiTongDaoKey= TDZhaungTai.Keys.ToList();
            IsShuaXin = false;
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        public void ShuXinData(List<JiCunQiModel> lismodel)
        {
            if (IsShuaXin)
            {
                return;
            }          
            foreach (var item in lismodel)
            {
                string key = $"{item.SheBeiID}:{item.WeiYiBiaoShi}";
                if (Datas.ContainsKey(key))
                {
                    List<DuModel> shujus = Datas[key];
                    for (int i = 0; i < shujus.Count; i++)
                    {
                        shujus[i].Value = item.Value;
                        shujus[i].IsKeKao = item.IsKeKao;
                    }
                }
            }
            ZiShuaXinData(lismodel);
        }


        /// <summary>
        /// true是表示满足状态
        /// </summary>
        /// <returns></returns>
        public bool GetIOBool(int tdid,IOType iOType, bool shibaizhi)
        {
            if(TDIOShuJu.ContainsKey(tdid))
            {
              
                List<DuIOCanShuModel> liszhi = TDIOShuJu[tdid];
                for (int i = 0; i < liszhi.Count; i++)
                {
                    if (liszhi[i].Type == iOType && liszhi[i].IsCunZaiJiCunQi())
                    {
                      
                        bool zhen= liszhi[i].GetBool(shibaizhi);
                        return zhen;
                    }
                }
             
              
            }

            return shibaizhi;
        }
        /// <summary>
        /// true是表示满足状态
        /// </summary>
        /// <returns></returns>
        public bool GetIOBool(int tdid, string ioname, bool shibaizhi)
        {
            if (TDIOShuJu.ContainsKey(tdid))
            {
                List<DuIOCanShuModel> liszhi = TDIOShuJu[tdid];
                for (int i = 0; i < liszhi.Count; i++)
                {
                    if (liszhi[i].Name == ioname && liszhi[i].IsCunZaiJiCunQi())
                    {
                        return liszhi[i].GetBool(shibaizhi);
                    }
                }
            }
         
            return shibaizhi;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public string GetShouGeStr(int tdid, string duzhiname, string shibaizhi)
        {
            List<DuModel> duzhi = GetDuZhi(tdid, duzhiname);
            if (duzhi.Count>0)
            {
                return GetStr(duzhi[0], shibaizhi);
            }
            return shibaizhi;
        }

        /// <summary>
        /// 获取读数据的值
        /// </summary>
        /// <param name="tongdaoid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<DuModel> GetLeiXingDuZhi(int tdid, string type)
        {
          
            if (TDZhiShuJu.ContainsKey(tdid))
            {
                List<DuShuJuModel> lis = TDZhiShuJu[tdid];
                for (int i = 0; i < lis.Count; i++)
                {
                    if (lis[i].Type== type)
                    {                  
                        return lis[i].GetDuModel();
                    }
                }
            }
            return new List<DuModel>();
        }

        /// <summary>
        /// 获取读数据的值
        /// </summary>
        /// <param name="tongdaoid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<DuModel> GetDuZhi(int tdid, string duname)
        {
            if (TDZhiShuJu.ContainsKey(tdid))
            {
                List<DuShuJuModel> lis = TDZhiShuJu[tdid];
                for (int i = 0; i < lis.Count; i++)
                {
                    if (lis[i].Name == duname)
                    {
                        return lis[i].GetDuModel();
                    }
                }
            }
            return new List<DuModel>();
           
           
        
        }

        /// <summary>
        /// 获取读数据的值
        /// </summary>
        /// <param name="tongdaoid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetXieZhi(string jicunqiname,int shebeiid,string baibaizhi)
        {
            string key = $"{shebeiid}:{jicunqiname}";
            if (Datas.ContainsKey(key))
            {
                List<DuModel> lis = Datas[key];
                for (int i = 0; i < lis.Count; i++)
                {
                    if (lis[i].IsXie)
                    {
                        return GetStr(lis[i], baibaizhi);
                    }
                }
            }
         
            return baibaizhi;
        }

        /// <summary>
        /// 获取读的string  值
        /// </summary>
        /// <param name="zhimodel"></param>
        /// <param name="shibaizhi"></param>
        /// <returns></returns>
        public string GetStr(DuModel zhimodel,string shibaizhi)
        {
            return ChangYong.TryStr(zhimodel.Value, shibaizhi);
        }
        /// <summary>
        /// 获取读的 int  值
        /// </summary>
        /// <param name="zhimodel"></param>
        /// <param name="shibaizhi"></param>
        /// <returns></returns>
        public int GetInt(DuModel zhimodel, int shibaizhi)
        {
            return ChangYong.TryInt(zhimodel.Value, shibaizhi);
        }
        /// <summary>
        /// 获取读的 int  值
        /// </summary>
        /// <param name="zhimodel"></param>
        /// <param name="shibaizhi"></param>
        /// <returns></returns>
        public float GetFloat(DuModel zhimodel, float shibaizhi)
        {
            return ChangYong.TryFloat(zhimodel.Value,shibaizhi);
        }

        /// <summary>
        /// 获取写的数据 没有就是空
        /// </summary>
        /// <param name="xiename"></param>
        /// <returns></returns>
        public XieSateModel GetXieModel(int tdid,string xiename)
        {
            if (TDXieShuJu.ContainsKey(tdid))
            {
                List<XieSateModel> lis = TDXieShuJu[tdid];
                for (int i = 0; i < lis.Count; i++)
                {
                    if (lis[i].Name == xiename)
                    {
                        return ChangYong.FuZhiShiTi(lis[i]);
                    }
                }
            }
           
            return null;
        }

        /// <summary>
        /// 获取写的数据 没有就是空
        /// </summary>
        /// <param name="xiename"></param>
        /// <returns></returns>
        public XieSateModel GetXieModel(int tdid,XieSateType xieSateType)
        {
            if (TDXieShuJu.ContainsKey(tdid))
            {
                List<XieSateModel> lis = TDXieShuJu[tdid];
                for (int i = 0; i < lis.Count; i++)
                {
                    if (lis[i].Type == xieSateType)
                    {
                        return ChangYong.FuZhiShiTi(lis[i]);
                    }
                }
            }
            
            return null;
        }
        /// <summary>
        /// 软件关闭用的
        /// </summary>
        public void Close()
        {
            IsShuaXin = true;
            TDIOShuJu.Clear();
            TDZhiShuJu.Clear();
            TDXieShuJu.Clear();
            Datas.Clear();
            ZiClose();
        }
        /// <summary>
        /// 子关闭软件用的
        /// </summary>
        protected virtual void ZiClose()
        {

        }
    

        /// <summary>
        /// 子分配数用的
        /// </summary>
        protected virtual void ZiFenPeiData()
        { }
        /// <summary>
        /// 子刷新数据
        /// </summary>
        /// <param name="lismodel"></param>
        protected virtual void ZiShuaXinData(List<JiCunQiModel> lismodel)
        { }
    }

    /// <summary>
    /// 通道状态参数
    /// </summary>
    public class TDZhuangTaiModel
    {
        /// <summary>
        /// true通信正常
        /// </summary>
        public bool TongXin { get; set; } = false;

        /// <summary>
        /// true表示报警1类
        /// </summary>
        public bool BaoJing1 { get; set; } = false;

        /// <summary>
        /// true表示报警2类  这类报警是可以屏蔽的
        /// </summary>
        public bool BaoJing2 { get; set; } = false;

        /// <summary>
        ///  true表示报警3类   可以恢复的报警
        /// </summary>
        public bool BaoJing3 { get; set; } = false;

        /// <summary>
        ///  true表示报警4类 气缸报警 用于超时
        /// </summary>
        public bool BaoJing4 { get; set; } = false;

        /// <summary>
        /// true 表示自动
        /// </summary>
        public bool IsZiDong { get; set; } = false;

        /// <summary>
        /// true 表示急停
        /// </summary>
        public bool IsJiTing { get; set; } = false;
        /// <summary>
        /// true  表示气压不合格
        /// </summary>
        public bool QiYaBuHeGe { get; set; } = false;

        /// <summary>
        /// true表按了停止
        /// </summary>
        public bool TingZhi { get; set; } = false;

        /// <summary>
        /// true 表示自动运行与写入
        /// </summary>
        public bool IsKeYiYunXing { get; set; } = true;

        public List<XieSateModel> ZiDongXieSates { get; set; } = new List<XieSateModel>();
    }
}
