using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommLei.JiChuLei;
using KuHuDuanDoIP.Model;
using SSheBei.CRCJiaoYan;
using SSheBei.Model;
using CommLei.DataChuLi;


namespace KuHuDuanDoIP.Frm
{
    public  class DataMoXing
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
        /// 读写寄存器
        /// </summary>
        public List<JiCunQiModel> LisDuXie = new List<JiCunQiModel>();
        /// <summary>
        /// 设备
        /// </summary>
        public List<SheBeiModel> LisSheBei = new List<SheBeiModel>();

        public Dictionary<int, byte[]> SheBeiSa= new Dictionary<int, byte[]>();

        public Dictionary<int, byte[]> XiTiaoByte = new Dictionary<int, byte[]>();

        public Dictionary<int, byte[]> WoShouByte = new Dictionary<int, byte[]>();
        /// <summary>
        /// 写标识的对应 key表示寄存器的唯一表示
        /// </summary>
        public Dictionary<string, ZhiLingModel> JiLu = new Dictionary<string, ZhiLingModel>();

        private List<string> KeyS = new List<string>();

        private List<byte> HuanCunData = new List<byte>();

        public List<string> IPS = new List<string>();

        /// <summary>
        /// 用于初始化
        /// </summary>
        public void IniData(string lujing)
        {
            LisDu.Clear();
            LisXie.Clear();
            LisDuXie.Clear();
            SheBeiSa.Clear();
            XiTiaoByte.Clear();
            WoShouByte.Clear();
            JiLu.Clear();
            IPS.Clear();
            JosnOrSModel JosnOrSModel = new JosnOrSModel(lujing);
            LisSheBei = JosnOrSModel.GetLisTModel<SheBeiModel>();
            if (LisSheBei == null)
            {
                LisSheBei = new List<SheBeiModel>();
              
            }
            for (int i = 0; i < LisSheBei.Count; i++)
            {
                IPS.Add(LisSheBei[i].IP);
                if (SheBeiSa.ContainsKey(LisSheBei[i].SheBeiID)==false)
                {
                    SheBeiSa[LisSheBei[i].SheBeiID] = ChangYong.HexStringToByte(LisSheBei[i].SA);
                }
                if (string.IsNullOrEmpty(LisSheBei[i].XinTiaoZhiLing)==false)
                {
                    if (XiTiaoByte.ContainsKey(LisSheBei[i].SheBeiID))
                    {
                        XiTiaoByte.Add(LisSheBei[i].SheBeiID,ChangYong.HexStringToByte(LisSheBei[i].XinTiaoZhiLing));
                    }
                }
                if (string.IsNullOrEmpty(LisSheBei[i].WoShouZhiLing) == false)
                {
                    if (WoShouByte.ContainsKey(LisSheBei[i].SheBeiID))
                    {
                        WoShouByte.Add(LisSheBei[i].SheBeiID, ChangYong.HexStringToByte(LisSheBei[i].WoShouZhiLing));
                    }
                }
            }
            List<string> ZhiLingTypes = ChangYong.MeiJuLisName(typeof(ZhiLingType));
            for (int i = 0; i < ZhiLingTypes.Count; i++)
            {
                JiCunQiModel model = new JiCunQiModel();
                model.SheBeiID = SheBeiID;
                model.WeiYiBiaoShi =$"{SheBeiID}:{ZhiLingTypes[i]}";
                model.MiaoSu = ChangYong.GetEnumDescription(ChangYong.GetMeiJuZhi<ZhiLingType>(ZhiLingTypes[i]));
                model.DuXie = 2;
                LisXie.Add(model);
                LisDuXie.Add(model);
                if (JiLu.ContainsKey(model.WeiYiBiaoShi) == false)
                {
                    ZhiLingModel sendModel = new ZhiLingModel();
                    sendModel.JiCunQiModel = model;
                    sendModel.ZhiLingType = ChangYong.GetMeiJuZhi<ZhiLingType>(ZhiLingTypes[i]);
                    JiLu.Add(model.WeiYiBiaoShi, sendModel);
                }

            }
          
            KeyS = JiLu.Keys.ToList();
       
           
        }

        /// <summary>
        /// 设置通信状态
        /// </summary>
        /// <param name="zongid"></param>
        /// <param name="hege"></param>
        public void SetHeGe(int zongid, bool hege)
        {
            for (int i = 0; i < LisSheBei.Count; i++)
            {
                if (LisSheBei[i].SheBeiID == zongid)
                {
                    LisSheBei[i].TX = hege;
                    break;
                }
            }

        }

        public void SetJiCunQiValue(ZhiLingModel model, object shuju)
        {
            if (JiLu.ContainsKey(model.JiCunQiModel.WeiYiBiaoShi))
            {
                ZhiLingModel cunModel = JiLu[model.JiCunQiModel.WeiYiBiaoShi];
                cunModel.JiCunQiModel.IsKeKao = true;
                cunModel.JiCunQiModel.Value = shuju;
            }

        }
        public void SetJiCunQiValue(ZhiLingModel model, byte[] shusju)
        {           
            if (JiLu.ContainsKey(model.JiCunQiModel.WeiYiBiaoShi))
            {
                HuanCunData.AddRange(shusju);
                ZhiLingModel cunModel = JiLu[model.JiCunQiModel.WeiYiBiaoShi];
                cunModel.JiCunQiModel.IsKeKao = true;
                cunModel.JiCunQiModel.Value = ChangYong.ByteOrString(shusju," ");
            }


        }
        public void SetZhengZaiValue(ZhiLingModel model, int sate)
        {
            if (JiLu.ContainsKey(model.JiCunQiModel.WeiYiBiaoShi))
            {
                ZhiLingModel cunModel = JiLu[model.JiCunQiModel.WeiYiBiaoShi];
                cunModel.IsXieWan = sate;
                if (sate == 0)
                {
                    cunModel.JiCunQiModel.Value = "";
                    if (cunModel.ZhiLingType!= ZhiLingType.XieDoipGetZiData)
                    {
                        HuanCunData.Clear();
                    }
                }

            }

        }


        /// <summary>
        /// 1是成功 0是未测完 3 不存在 其他表示超时
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ZhiLingModel IsChengGong(JiCunQiModel model)
        {
            if (JiLu.ContainsKey(model.WeiYiBiaoShi))
            {
                ZhiLingModel cunModel = JiLu[model.WeiYiBiaoShi];
                return cunModel;
            }
            return null;
        }

        public ZhiLingModel GetModel(JiCunQiModel model)
        {
            if (JiLu.ContainsKey(model.WeiYiBiaoShi))
            {
                ZhiLingModel cunModel = JiLu[model.WeiYiBiaoShi];
                return cunModel.FuZhi();
            }
            return null;
        }
        public ZhiLingModel GetModel(ZhiLingType cunType, bool isfuzhi = true)
        {
            for (int i = 0; i < KeyS.Count; i++)
            {
                ZhiLingModel cunModel = JiLu[KeyS[i]];
                if (cunModel.ZhiLingType == cunType)
                {
                    if (isfuzhi)
                    {
                        ZhiLingModel xinmodel = cunModel.FuZhi();

                        return xinmodel;
                    }
                    else
                    {
                        return cunModel;
                    }
                }
            }

            return null;
        }

        public SheBeiModel GetSheBeiModel(ZhiLingModel model)
        {
            for (int i = 0; i < LisSheBei.Count; i++)
            {
                if (LisSheBei[i].IP == model.IP)
                {
                    return LisSheBei[i];
                }
            }
            return null;
        }
        public SheBeiModel GetSheBeiModel(string ip)
        {
            for (int i = 0; i < LisSheBei.Count; i++)
            {
                if (LisSheBei[i].IP == ip)
                {
                    return LisSheBei[i];
                }
            }
            return null;
        }
        public void BaoCunJiLu(string lujing,ZhiLingModel model)
        {
            JosnOrSModel JosnOrSModel = new JosnOrSModel(lujing);
            List<JiLuZhiLingModel> lis= JosnOrSModel.GetLisTModel<JiLuZhiLingModel>();
            if (lis == null)
            {
                lis = new List<JiLuZhiLingModel>();
            }
            bool zhen = false;
            for (int i = 0; i < lis.Count; i++)
            {
                if (lis[i].ZhiLingName.Equals(model.ZhiLingName))
                {
                    lis[i].FuZhi(model);
                    zhen = true;
                    break;
                }
            }
            if (zhen==false)
            {
                JiLuZhiLingModel models = new JiLuZhiLingModel();
                models.FuZhi(model);
                lis.Add(models);
            }
            JosnOrSModel.XieTModel(lis);
        }

        public List<JiLuZhiLingModel> GetJiLuData(string lujing)
        {
            JosnOrSModel JosnOrSModel = new JosnOrSModel(lujing);
            List<JiLuZhiLingModel> lis = JosnOrSModel.GetLisTModel<JiLuZhiLingModel>();
            if (lis == null)
            {
                lis = new List<JiLuZhiLingModel>();
            }
            return lis;
        }
    }

   

}
