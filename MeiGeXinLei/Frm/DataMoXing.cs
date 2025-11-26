using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommLei.JiChuLei;
using SSheBei.CRCJiaoYan;
using SSheBei.Model;
using YiBanSaoMaQi.Model;
using CommLei.DataChuLi;
using System.Xml.Linq;
using System.Net.Http.Headers;


namespace YiBanSaoMaQi.Frm
{
    /// <summary>
    /// 模型数据
    /// </summary>
    public class DataMoXing
    {
        /// <summary>
        /// 设备id
        /// </summary>
        public int SheBeiID { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string SheBeiName { get; set; } = "";
        /// <summary>
        /// 读寄存器
        /// </summary>
        public List<JiCunQiModel> LisDu = new List<JiCunQiModel>();
        /// <summary>
        /// 写寄存器
        /// </summary>
        public List<JiCunQiModel> LisXie = new List<JiCunQiModel>();

        /// <summary>
        /// 写寄存器
        /// </summary>
        public List<JiCunQiModel> LisDuXie = new List<JiCunQiModel>();

        /// <summary>
        /// 设备
        /// </summary>
        public List<SaoMaModel> LisSheBei = new List<SaoMaModel>();

        /// <summary>
        /// 写标识的对应 key表示寄存器的唯一表示
        /// </summary>
        public Dictionary<string, CunModel> JiLu = new Dictionary<string, CunModel>();

        private List<string> KeyS = new List<string>();

        /// <summary>
        /// 用于初始化
        /// </summary>
        public void IniData(string lujing)
        {
            LisDu.Clear();
            LisXie.Clear();
            JiLu.Clear();
            LisDuXie.Clear();
            JosnOrSModel JosnOrSModel = new JosnOrSModel(lujing);
            LisSheBei = JosnOrSModel.GetLisTModel<SaoMaModel>();
            if (LisSheBei == null)
            {
                LisSheBei = new List<SaoMaModel>();
            }
            for (int c = 0; c < LisSheBei.Count; c++)
            {
                SaoMaModel shebei = LisSheBei[c];
                {
                    
                    for (int i = 0; i < shebei.LisZhiLing.Count; i++)
                    {
                        ZhiLingModel zhiLingModel=shebei.LisZhiLing[i];
                        switch (zhiLingModel.ZhiLingType)
                        {
                            case ZhiLingType.BFTongDao:
                                {
                                    JiCunQiModel model = new JiCunQiModel();
                                    model.SheBeiID = SheBeiID;
                                    model.WeiYiBiaoShi = $"{shebei.SheBeiID}:{zhiLingModel.MingCheng}";
                                    model.MiaoSu = "用于播放不需要参数";
                                    model.DuXie = 2;
                                    LisXie.Add(model);
                                    LisDuXie.Add(model);
                                    if (JiLu.ContainsKey(model.WeiYiBiaoShi) == false)
                                    {
                                        CunModel cunModel = new CunModel();
                                        cunModel.ZongSheBeiId = shebei.SheBeiID;
                                        cunModel.ZhiLingId = zhiLingModel.ZhiLingID;
                                        cunModel.IsData = false;
                                        cunModel.ZhiLingType = zhiLingModel.ZhiLingType;
                                        cunModel.ShuYuTongDaoID = -1;
                                        cunModel.ZhiLing = zhiLingModel.ZhiLing;
                                        cunModel.ZhiLingJieSu = zhiLingModel.ZhiLingJieSu;
                                        cunModel.JiCunQi = model;
                                        cunModel.Time = shebei.Time;
                                        JiLu.Add(model.WeiYiBiaoShi, cunModel);
                                    }
                                }
                                break;
                            case ZhiLingType.TZBFTongDao:
                                {
                                    JiCunQiModel model = new JiCunQiModel();
                                    model.SheBeiID = SheBeiID;
                                    model.WeiYiBiaoShi = $"{shebei.SheBeiID}:{zhiLingModel.MingCheng}";
                                    model.MiaoSu = "用于停止播放不需要参数";
                                    model.DuXie = 2;
                                    LisXie.Add(model);
                                    LisDuXie.Add(model);
                                    if (JiLu.ContainsKey(model.WeiYiBiaoShi) == false)
                                    {
                                        CunModel cunModel = new CunModel();
                                        cunModel.ZongSheBeiId = shebei.SheBeiID;
                                        cunModel.ZhiLingId = zhiLingModel.ZhiLingID;
                                        cunModel.IsData = false;
                                        cunModel.ZhiLingType = zhiLingModel.ZhiLingType;
                                        cunModel.ShuYuTongDaoID = -1;
                                        cunModel.ZhiLing = zhiLingModel.ZhiLing;
                                        cunModel.ZhiLingJieSu = zhiLingModel.ZhiLingJieSu;
                                        cunModel.JiCunQi = model;
                                        cunModel.Time = shebei.Time;
                                        JiLu.Add(model.WeiYiBiaoShi, cunModel);
                                    }
                                }
                                break;
                            case ZhiLingType.KaiCai:
                                {
                                    JiCunQiModel model = new JiCunQiModel();
                                    model.SheBeiID = SheBeiID;
                                    model.WeiYiBiaoShi = $"{shebei.SheBeiID}:{zhiLingModel.MingCheng}";
                                    model.MiaoSu = "用于采集不需要参数";
                                    model.DuXie = 2;
                                    LisXie.Add(model);
                                    LisDuXie.Add(model);
                                    if (JiLu.ContainsKey(model.WeiYiBiaoShi) == false)
                                    {
                                        CunModel cunModel = new CunModel();
                                        cunModel.ZongSheBeiId = shebei.SheBeiID;
                                        cunModel.ZhiLingId = zhiLingModel.ZhiLingID;
                                        cunModel.IsData = false;
                                        cunModel.ZhiLingType = zhiLingModel.ZhiLingType;
                                        cunModel.ShuYuTongDaoID = -1;
                                        cunModel.JiCunQi = model;
                                        cunModel.ZhiLing = zhiLingModel.ZhiLing;
                                        cunModel.Time = shebei.Time;
                                        cunModel.ZhiLingJieSu = zhiLingModel.ZhiLingJieSu;
                                        JiLu.Add(model.WeiYiBiaoShi, cunModel);
                                    }
                                }
                                break;
                            case ZhiLingType.CJJieGuo:
                                {                              
                                    {
                                        JiCunQiModel model = new JiCunQiModel();
                                        model.SheBeiID = SheBeiID;
                                        model.WeiYiBiaoShi = $"{shebei.SheBeiID}:{zhiLingModel.MingCheng}";
                                        model.MiaoSu = "用于采集结果不需要参数";
                                        model.DuXie = 2;
                                        LisXie.Add(model);
                                        LisDuXie.Add(model);
                                        if (JiLu.ContainsKey(model.WeiYiBiaoShi) == false)
                                        {
                                            CunModel cunModel = new CunModel();
                                            cunModel.ZongSheBeiId = shebei.SheBeiID;
                                            cunModel.ZhiLingId = zhiLingModel.ZhiLingID;
                                            cunModel.IsData = false;
                                            cunModel.ZhiLingType = zhiLingModel.ZhiLingType;
                                            cunModel.ShuYuTongDaoID = -1;
                                            cunModel.JiCunQi = model;
                                            cunModel.ZhiLing = zhiLingModel.ZhiLing;
                                            cunModel.ZhiLingJieSu = zhiLingModel.ZhiLingJieSu;
                                            cunModel.Time = shebei.Time;
                                            JiLu.Add(model.WeiYiBiaoShi, cunModel);
                                        }
                                    }
                                    {
                                        foreach (DataSdModel item in zhiLingModel.LisData)
                                        {
                                            JiCunQiModel model = new JiCunQiModel();
                                            model.SheBeiID = SheBeiID;
                                            model.WeiYiBiaoShi = $"{shebei.SheBeiID}:{item.Name}";
                                            model.MiaoSu = "用于采集结果";
                                            model.DuXie = 1;                                     
                                            LisDu.Add(model);
                                            LisDuXie.Add(model);
                                            if (JiLu.ContainsKey(model.WeiYiBiaoShi) == false)
                                            {
                                                CunModel cunModel = new CunModel();
                                                cunModel.ZongSheBeiId = shebei.SheBeiID;
                                                cunModel.ZhiLingId = zhiLingModel.ZhiLingID;
                                                cunModel.IsData = true;
                                                cunModel.ZhiLingType = ZhiLingType.ZiJieGuo;
                                                cunModel.ShuYuTongDaoID = item.TongDao;
                                                cunModel.JiCunQi = model;
                                                cunModel.Name= item.CanShu;
                                                JiLu.Add(model.WeiYiBiaoShi, cunModel);
                                            }
                                        }
                                      
                                    }
                                }
                                break;
                         
                        }
                                              
                       
                       
                    }
                   
                }
          //      if (isyouvp)
                {
                    int ziid = -5;
                    List<string> qitashuju = ChangYong.MeiJuLisName(typeof(QiTaLeiType));
                    for (int i = 0; i < qitashuju.Count; i++)
                    {
                        JiCunQiModel model = new JiCunQiModel();
                        model.SheBeiID = SheBeiID;
                        model.WeiYiBiaoShi = $"{shebei.SheBeiID},{qitashuju[i]}";
                        model.MiaoSu = GetMiaoSu(qitashuju[i]);
                        model.DuXie = 1;
                        LisDu.Add(model);
                        LisDuXie.Add(model);
                        if (JiLu.ContainsKey(model.WeiYiBiaoShi) == false)
                        {
                            CunModel cunModel = new CunModel();
                            cunModel.ZongSheBeiId = shebei.SheBeiID;
                            cunModel.ZhiLingId = ziid;
                            cunModel.IsData = false;
                            cunModel.ZhiLingType = ZhiLingType.QiTa;
                            cunModel.ShuYuTongDaoID = -2;
                            cunModel.JiCunQi = model;
                            cunModel.Name = qitashuju[i];
                            JiLu.Add(model.WeiYiBiaoShi, cunModel);
                            ziid--;
                        }
                    }
                
                }
            }
            KeyS = JiLu.Keys.ToList();
        }

   
        public void SetState(int zongid,bool state)
        {
            for (int i = 0; i < LisSheBei.Count; i++)
            {
                if (LisSheBei[i].SheBeiID == zongid)
                {
                    LisSheBei[i].TX = state;
                    break;
                }
            }
        }

        public void SetJiCunQiValue(string weiyibiaoshi, string name, object shuju)
        {
            if (JiLu.ContainsKey(weiyibiaoshi))
            {
                CunModel cunModel = JiLu[weiyibiaoshi];
                cunModel.JiCunQi.IsKeKao = true;

                for (int c = 0; c < KeyS.Count; c++)
                {
                    if (JiLu[KeyS[c]].ZongSheBeiId == cunModel.ZongSheBeiId && JiLu[KeyS[c]].ZhiLingId == cunModel.ZhiLingId)
                    {
                        if (JiLu[KeyS[c]].Name.Equals(name))
                        {
                            JiLu[KeyS[c]].JiCunQi.Value = shuju;
                            
                            break;
                        }
                    }
                }
              
            }

        }
        public void SetJiCunQiValue(string weiyibiaoshi, object shuju)
        {
            if (JiLu.ContainsKey(weiyibiaoshi))
            {
                CunModel cunModel = JiLu[weiyibiaoshi];
                cunModel.JiCunQi.IsKeKao = true;

                cunModel.JiCunQi.Value = shuju;

            }

        }
        public void GWFuZhi(string weiyibiaoshi,string vp)
        {
         
            for (int c = 0; c < KeyS.Count; c++)
            {
                if (JiLu[KeyS[c]].JiCunQi.WeiYiBiaoShi.Equals(vp))
                {
                    if (JiLu[KeyS[c]].IsZhengZaiCe == 1)
                    {
                        string zhi = JiLu[KeyS[c]].JiCunQi.Value.ToString();
                        float cagg = ChangYong.TryFloat(zhi, 0);
                        double zss = (((cagg * cagg)) / 4f);
                        SetJiCunQiValue(weiyibiaoshi, zss.ToString());
                        SetZhengZaiValue(weiyibiaoshi, 1);
                        return;
                    }
                    break;
                }
            }
            SetJiCunQiValue(weiyibiaoshi,"超时");
            SetZhengZaiValue(weiyibiaoshi,2);
           
        }

        /// <summary>
        /// 计算指定通道的信噪比
        /// </summary>
        /// <param name="weiyibioashi"></param>
        /// <param name="vp1">返回信噪比</param>
        /// <returns></returns>
        public void GetAudioSNR(string weiyibioashi, string vp1,string vp2)
        {
            double zhi1 = 0;
            double zhi2 = 0;
            bool isyou1 = false;
            bool isyou2 = false;
            for (int c = 0; c < KeyS.Count; c++)
            {
                if (JiLu[KeyS[c]].JiCunQi.WeiYiBiaoShi.Equals(vp1))
                {
                    if (JiLu[KeyS[c]].IsZhengZaiCe == 1)
                    {
                        string zhi = JiLu[KeyS[c]].JiCunQi.Value.ToString();
                        zhi1 = ChangYong.TryFloat(zhi, 0);
                       isyou1 = true;
                    }
                   
                }
                if (JiLu[KeyS[c]].JiCunQi.WeiYiBiaoShi.Equals(vp2))
                {
                    if (JiLu[KeyS[c]].IsZhengZaiCe == 1)
                    {
                        string zhi = JiLu[KeyS[c]].JiCunQi.Value.ToString();
                        zhi2 = ChangYong.TryFloat(zhi, 0);
                        isyou2 = true;
                    }

                }
                if (isyou1&&isyou2)
                {
                    break;
                }
            }
            if (isyou1 && isyou2)
            {
              
                try
                {
                    double snr = Math.Log10(zhi1 / zhi2) * 20;
                    SetJiCunQiValue(weiyibioashi, snr);
                    SetZhengZaiValue(weiyibioashi, 1);
                }
                catch 
                {
                    SetJiCunQiValue(weiyibioashi, "0");
                    SetZhengZaiValue(weiyibioashi, 2);

                }
               
            }
            else
            {
                SetJiCunQiValue(weiyibioashi, "超时");
                SetZhengZaiValue(weiyibioashi, 2);
            }

           
        }

        public void GetAudioBalanceLeft(string weiyibioashi, string vp1, string vp2)
        {
            double zhi1 = 0;
            double zhi2 = 0;
            bool isyou1 = false;
            bool isyou2 = false;
            for (int c = 0; c < KeyS.Count; c++)
            {
                if (JiLu[KeyS[c]].JiCunQi.WeiYiBiaoShi.Equals(vp1))
                {
                    if (JiLu[KeyS[c]].IsZhengZaiCe == 1)
                    {
                        string zhi = JiLu[KeyS[c]].JiCunQi.Value.ToString();
                        zhi1 = ChangYong.TryFloat(zhi, 0);
                        isyou1 = true;
                    }

                }
                if (JiLu[KeyS[c]].JiCunQi.WeiYiBiaoShi.Equals(vp2))
                {
                    if (JiLu[KeyS[c]].IsZhengZaiCe == 1)
                    {
                        string zhi = JiLu[KeyS[c]].JiCunQi.Value.ToString();
                        zhi2 = ChangYong.TryFloat(zhi, 0);
                        isyou2 = true;
                    }

                }
                if (isyou1 && isyou2)
                {
                    break;
                }
            }
            if (isyou1 && isyou2)
            {
                if (zhi2 == 0)
                {
                    zhi2 = 1;
                }
                try
                {
                    double snr = Math.Log10(zhi1 / zhi2) * 20;
                    SetJiCunQiValue(weiyibioashi, snr);
                    SetZhengZaiValue(weiyibioashi, 1);
                }
                catch
                {
                    SetJiCunQiValue(weiyibioashi, "0");
                    SetZhengZaiValue(weiyibioashi, 2);

                }

            }
            else
            {
                SetJiCunQiValue(weiyibioashi, "超时");
                SetZhengZaiValue(weiyibioashi, 2);
            }
          
        }

        public void GetAudioBalanceRight(string weiyibioashi, string vp1, string vp2)
        {
            double zhi1 = 0;
            double zhi2 = 0;
            bool isyou1 = false;
            bool isyou2 = false;
            for (int c = 0; c < KeyS.Count; c++)
            {
                if (JiLu[KeyS[c]].JiCunQi.WeiYiBiaoShi.Equals(vp1))
                {
                    if (JiLu[KeyS[c]].IsZhengZaiCe == 1)
                    {
                        string zhi = JiLu[KeyS[c]].JiCunQi.Value.ToString();
                        zhi1 = ChangYong.TryFloat(zhi, 0);
                        isyou1 = true;
                    }

                }
                if (JiLu[KeyS[c]].JiCunQi.WeiYiBiaoShi.Equals(vp2))
                {
                    if (JiLu[KeyS[c]].IsZhengZaiCe == 1)
                    {
                        string zhi = JiLu[KeyS[c]].JiCunQi.Value.ToString();
                        zhi2 = ChangYong.TryFloat(zhi, 0);
                        isyou2 = true;
                    }

                }
                if (isyou1 && isyou2)
                {
                    break;
                }
            }
            if (isyou1 && isyou2)
            {
                if (zhi2 == 0)
                {
                    zhi2 = 1;
                }
                try
                {
                    double snr = Math.Log10(zhi1 / zhi2) * 20;
                    SetJiCunQiValue(weiyibioashi, snr);
                    SetZhengZaiValue(weiyibioashi, 1);
                }
                catch
                {
                    SetJiCunQiValue(weiyibioashi, "0");
                    SetZhengZaiValue(weiyibioashi, 2);

                }

            }
            else
            {
                SetJiCunQiValue(weiyibioashi, "超时");
                SetZhengZaiValue(weiyibioashi, 2);
            }

        }
        public void SetZhengZaiValue(string weiyibiaoshi,int sate)
        {
            if (JiLu.ContainsKey(weiyibiaoshi))
            {
                CunModel cunModel = JiLu[weiyibiaoshi];
                cunModel.IsZhengZaiCe = sate;
                if (sate==0)
                {
                    cunModel.JiCunQi.Value = "";
                }
                for (int c = 0; c < KeyS.Count; c++)
                {
                    if (JiLu[KeyS[c]].ZongSheBeiId== cunModel.ZongSheBeiId&& JiLu[KeyS[c]].ZhiLingId == cunModel.ZhiLingId)
                    {
                        JiLu[KeyS[c]].IsZhengZaiCe = sate;
                    }
                }
            }

        }


        public List<string> GetQiTaShuJu()
        {
            List<string> list = new List<string>();
            for (int c = 0; c < KeyS.Count; c++)
            {
                if (JiLu[KeyS[c]].ZhiLingType==ZhiLingType.ZiJieGuo)
                {
                    list.Add(JiLu[KeyS[c]].JiCunQi.WeiYiBiaoShi);

                }
                
            }
            return list;
        }

        /// <summary>
        /// 1是成功 0是未测完 3 不存在 其他表示超时
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public CunModel IsChengGong(string weiyibiaoshi)
        {
            if (JiLu.ContainsKey(weiyibiaoshi))
            {
                CunModel cunModel = JiLu[weiyibiaoshi];
                return cunModel;
            }
            return null;
        }

        public CunModel GetModel(JiCunQiModel model)
        {
            if (JiLu.ContainsKey(model.WeiYiBiaoShi))
            {
                CunModel cunModel = JiLu[model.WeiYiBiaoShi];
                return ChangYong.FuZhiShiTi( cunModel);
            }
            return null;
        }

     

        public SaoMaModel GetSheBeiModel(CunModel model)
        {
            for (int i = 0; i < LisSheBei.Count; i++)
            {
                if (LisSheBei[i].SheBeiID==model.ZongSheBeiId)
                {
                    return LisSheBei[i];
                }
            }
            return null;
        }


        private string GetMiaoSu(string miaosu)
        {
            if (miaosu == QiTaLeiType.GW.ToString())
            {
                return $"参数：采集的名称";
            }
            else
            {
                return $"参数：采集的名称,采集的名称";
            }
    
        }
    }
}
