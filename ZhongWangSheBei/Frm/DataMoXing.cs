using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommLei.DataChuLi;
using CommLei.JiChuLei;
using SSheBei.CRCJiaoYan;
using SSheBei.Model;
using ZhongWangSheBei.Model;

namespace ZhongWangSheBei.Frm
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
        /// 写寄存器
        /// </summary>
        public List<JiCunQiModel> LisDuXie = new List<JiCunQiModel>();
        /// <summary>
        /// 设备
        /// </summary>
        public List<ZSModel> LisSheBei = new List<ZSModel>();

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
          
            JosnOrSModel JosnOrSModel = new JosnOrSModel(lujing);
            LisSheBei = JosnOrSModel.GetLisTModel<ZSModel>();
            if (LisSheBei == null)
            {
                LisSheBei = new List<ZSModel>();
            }
            for (int c = 0; c < LisSheBei.Count; c++)
            {
                ZSModel shebei = LisSheBei[c];
               
                for (int d = 0; d < shebei.LisJiLu.Count; d++)
                {
                    ZiSheBeiModel zishebei = shebei.LisJiLu[d];                  
                    {
                        List<byte> fenzhuang = new List<byte>();
                        fenzhuang.Add((byte)zishebei.DiZhi);
                        fenzhuang.Add(0x03);
                        fenzhuang.Add(0x00);
                        fenzhuang.Add(0x00);
                        fenzhuang.Add(0x00);
                        fenzhuang.Add((byte)zishebei.JiLu);
                        byte[] shu = CRC.ToModbus(fenzhuang, false);
                        fenzhuang.AddRange(shu);
                        zishebei.ZhiLing = fenzhuang;
                        zishebei.ChangDu = zishebei.JiLu * 2;
                        zishebei.ChaoShiTime = shebei.ChaoShiTime;
                        zishebei.CiShu = 5;
                    }
                    {//xie
                        List<string> lisxie = ChangYong.MeiJuLisName(typeof(CunType));
                        for (int i = 0; i < lisxie.Count; i++)
                        {
                            if (lisxie[i].ToLower().StartsWith("xie"))
                            {
                                CunType cunType = ChangYong.GetMeiJuZhi<CunType>(lisxie[i]);
                                JiCunQiModel model = new JiCunQiModel();
                                model.WeiYiBiaoShi = $"{zishebei.ZiName}继电器{cunType.ToString().Replace("Xie","")}";
                                model.SheBeiID = SheBeiID;
                                model.MiaoSu = ChangYong.GetEnumDescription(cunType);                              
                                model.DuXie = 2;
                                LisXie.Add(model);
                                LisDuXie.Add(model);

                                if (JiLu.ContainsKey(model.WeiYiBiaoShi) == false)
                                {
                                    CunModel cunModel = new CunModel();
                                    cunModel.ZiSheBeiID = zishebei.ZSID;
                                    cunModel.ZongSheBeiId = shebei.SheBeiID;
                                    cunModel.JiCunDiZhi = -1;
                                    cunModel.JiCunQi = model;
                                    cunModel.IsDu = cunType;
                                    cunModel.ZDiZhi = zishebei.DiZhi;
                                    cunModel.JiLu = zishebei.JiLu;
                                    cunModel.XieRuChaoShi = shebei.XieRuChaoShi;
                                    JiLu.Add(model.WeiYiBiaoShi, cunModel);
                                }
                            }
                        }
                    }
                    {
                        //du
                        for (int i = 0; i < zishebei.JiLu; i++)
                        {
                            //读
                            {
                                JiCunQiModel model = new JiCunQiModel();
                                model.WeiYiBiaoShi = $"{zishebei.ZiName}读第{i + 1}继电器";
                                model.SheBeiID = SheBeiID;
                                model.MiaoSu = ChangYong.GetEnumDescription(CunType.DuJiCunQi);
                                model.DuXie = 1;
                                LisDu.Add(model);
                                if (JiLu.ContainsKey(model.WeiYiBiaoShi) == false)
                                {
                                    CunModel cunModel = new CunModel();
                                    cunModel.ZiSheBeiID = zishebei.ZSID;
                                    cunModel.ZongSheBeiId = shebei.SheBeiID;
                                    cunModel.JiCunDiZhi = i;
                                    cunModel.IsDu = CunType.DuJiCunQi;
                                    cunModel.JiCunQi = model;
                                    cunModel.ZDiZhi = zishebei.DiZhi;
                                    cunModel.JiLu = zishebei.JiLu;
                                    cunModel.XieRuChaoShi = shebei.XieRuChaoShi;
                                    JiLu.Add(model.WeiYiBiaoShi, cunModel);
                                }
                                LisDuXie.Add(model);

                            }

                        }
                    }

                }
              
              
            }
            KeyS = JiLu.Keys.ToList();
        }

        public void SetTX(int zongid,int zid,bool iszong,bool zhuangtai)
        {
            if (iszong)
            {
                for (int i = 0; i < LisSheBei.Count; i++)
                {
                    if (LisSheBei[i].SheBeiID == zongid)
                    {
                        for (int c = 0; c < LisSheBei[i].LisJiLu.Count; c++)
                        {
                            LisSheBei[i].LisJiLu[c].Tx = zhuangtai;
                        }
                        break;
                    }
                }
                for (int i = 0; i < KeyS.Count; i++)
                {
                    CunModel cunModel = JiLu[KeyS[i]];
                    if (cunModel.ZongSheBeiId == zongid)
                    {
                        cunModel.JiCunQi.IsKeKao = zhuangtai;
                    }
                }
            }
            else
            {
                for (int i = 0; i < LisSheBei.Count; i++)
                {
                    if (LisSheBei[i].SheBeiID == zongid)
                    {
                        for (int c = 0; c < LisSheBei[i].LisJiLu.Count; c++)
                        {
                            if (LisSheBei[i].LisJiLu[c].ZSID== zid)
                            {
                                LisSheBei[i].LisJiLu[c].Tx = zhuangtai;
                                break;
                            }
                           
                        }
                        break;
                    }
                }
                for (int i = 0; i < KeyS.Count; i++)
                {
                    CunModel cunModel = JiLu[KeyS[i]];
                    if (cunModel.ZongSheBeiId == zongid && cunModel.ZiSheBeiID == zid)
                    {
                        cunModel.JiCunQi.IsKeKao = zhuangtai;
                    }
                }
            }
           

        }

        public void SetJiCunQiValue(int zongid, int zid,List<byte> shuju)
        {
            for (int i = 0; i < KeyS.Count; i++)
            {
                CunModel cunModel = JiLu[KeyS[i]];

                if (cunModel.ZongSheBeiId == zongid && cunModel.ZiSheBeiID == zid)
                {
                    cunModel.JiCunQi.IsKeKao = true;
                    if (cunModel.IsDu == CunType.DuJiCunQi)
                    {
                        int zhengshu = cunModel.JiCunDiZhi;
                        int yushu = (zhengshu - 0) * 2;
                        if (yushu < shuju.Count - 1 && yushu >= 0)
                        {
                            cunModel.JiCunQi.Value = GetValue(new byte[] { shuju[yushu], shuju[yushu + 1] });

                        }
                    }
                }
              
            }
            for (int i = 0; i < LisSheBei.Count; i++)
            {
                if (LisSheBei[i].SheBeiID== zongid)
                {
                    for (int c = 0; c < LisSheBei[i].LisJiLu.Count; c++)
                    {
                        if (LisSheBei[i].LisJiLu[c].ZSID== zid)
                        {
                            LisSheBei[i].LisJiLu[c].Tx = true;
                        }
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// 1是成功 2是不成功 其他不可靠
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int IsChengGong(JiCunQiModel model)
        {
          
            if (JiLu.ContainsKey(model.WeiYiBiaoShi))
            {
                CunModel cunModel = JiLu[model.WeiYiBiaoShi];
                if (cunModel.IsDu==CunType.DuJiCunQi)
                {
                    for (int i = 0; i < LisDu.Count; i++)
                    {
                        if (LisDu[i].WeiYiBiaoShi.Equals(model.WeiYiBiaoShi))
                        {
                            if (LisDu[i].IsKeKao == false)
                            {
                                return 3;
                            }
                            bool  zhen = LisDu[i].Value.ToString().Equals(model.Value.ToString());
                            if (zhen)
                            {
                                return 1;
                            }
                            return 2;
                        }
                    }
                    return 3;
                }
                else
                {
                    if (cunModel.IsDu==CunType.Xie全开)
                    {
                        List<JiCunQiModel>  lis= ZhaoJiCunQi(true, cunModel,new List<int>());
                        if (lis.Count > 0)
                        {                                                   
                            for (int i = 0; i < lis.Count; i++)
                            {
                                if (lis[i].IsKeKao == false)
                                {
                                    return 3;
                                }
                                if (lis[i].Value.ToString().Equals("0"))
                                {
                                    return 2;
                                }
                            }
                            for (int i = 0; i < lis.Count; i++)
                            {
                                if (lis[i].IsKeKao==false)
                                {
                                    return 3;
                                }
                            }
                            return 1;
                        }
                        return 3;
                    }
                    else if (cunModel.IsDu == CunType.Xie全关)
                    {
                        List<JiCunQiModel> lis = ZhaoJiCunQi(true, cunModel, new List<int>());
                        if (lis.Count > 0)
                        {
                            
                            for (int i = 0; i < lis.Count; i++)
                            {
                                if (lis[i].IsKeKao == false)
                                {
                                    return 3;
                                }
                                if (lis[i].Value.ToString().Equals("1"))
                                {
                                    return 2;
                                }
                            }
                            for (int i = 0; i < lis.Count; i++)
                            {
                                if (lis[i].IsKeKao == false)
                                {
                                    return 3;
                                }
                            }
                            return 1;
                        }
                        return 3;
                    }
                    else if (cunModel.IsDu == CunType.Xie开)
                    {
                        List<int> jidianqi = ChangYong.JieGeInt(model.Value.ToString(), '|');
                        List<JiCunQiModel> lis = ZhaoJiCunQi(false, cunModel, jidianqi);
                        if (lis.Count > 0)
                        {
                           
                            for (int i = 0; i < lis.Count; i++)
                            {
                                if (lis[i].IsKeKao == false)
                                {
                                    return 3;
                                }
                                if (lis[i].Value.ToString().Equals("0"))
                                {
                                    return 2;
                                }
                            }
                            for (int i = 0; i < lis.Count; i++)
                            {
                                if (lis[i].IsKeKao == false)
                                {
                                    return 3;
                                }
                            }
                            return 1;
                        }
                        return 3;
                    }
                    else if (cunModel.IsDu == CunType.Xie关)
                    {
                        List<int> jidianqi = ChangYong.JieGeInt(model.Value.ToString(), '|');
                        List<JiCunQiModel> lis = ZhaoJiCunQi(false, cunModel, jidianqi);
                        if (lis.Count > 0)
                        {
                           
                            for (int i = 0; i < lis.Count; i++)
                            {
                                if (lis[i].IsKeKao == false)
                                {
                                    return 3;
                                }
                                if (lis[i].Value.ToString().Equals("1"))
                                {
                                    return 2;
                                }
                            }
                            for (int i = 0; i < lis.Count; i++)
                            {
                                if (lis[i].IsKeKao == false)
                                {
                                    return 3;
                                }
                            }
                            return 1;
                        }
                        return 3;
                    }
                }
            }
           
            return 3;
        }

      

        public JiCunQiModel GetXieJiCunQi(int zongid, int zid,bool iskai,bool isqunbu)
        {
            for (int i = 0; i < KeyS.Count; i++)
            {
                CunModel cunModel = JiLu[KeyS[i]];
                if ( cunModel.ZongSheBeiId == zongid && cunModel.ZiSheBeiID == zid)
                {
                    if (isqunbu == false)
                    {
                        if (iskai)
                        {
                            if (cunModel.IsDu == CunType.Xie开)
                            {
                                return ChangYong.FuZhiShiTi(cunModel.JiCunQi);
                            }

                        }
                        else
                        {
                            if (cunModel.IsDu == CunType.Xie关)
                            {
                                return ChangYong.FuZhiShiTi(cunModel.JiCunQi);
                            }
                        }
                    }
                    else
                    {
                        if (iskai)
                        {
                            if (cunModel.IsDu == CunType.Xie全开)
                            {
                                return ChangYong.FuZhiShiTi(cunModel.JiCunQi);
                            }

                        }
                        else
                        {
                            if (cunModel.IsDu == CunType.Xie全关)
                            {
                                return ChangYong.FuZhiShiTi(cunModel.JiCunQi);
                            }
                        }
                    }
                }
            }
            return null;
        }
     
        private int GetValue(byte[] canshu)
        {
            int zhi = 0;
            for (int i = 0; i < canshu.Length; i++)
            {
                if (canshu[i] == 0x01)
                {
                    zhi = 1;
                    break;
                }
            }
            return zhi;
        }

        private List<JiCunQiModel> ZhaoJiCunQi(bool isquanbu, CunModel cunmodel,List<int> dianwei)
        {
            if (isquanbu)
            {
                List<JiCunQiModel> lis = new List<JiCunQiModel>();
                for (int i = 0; i < KeyS.Count; i++)
                {
                    CunModel cun = JiLu[KeyS[i]];
                    if (cun.ZiSheBeiID == cunmodel.ZiSheBeiID && cun.ZongSheBeiId == cunmodel.ZongSheBeiId)
                    {
                        if (cun.IsDu==CunType.DuJiCunQi)
                        {
                            lis.Add(cun.JiCunQi);
                        }
                    }
                }
                return lis;
            }
            else
            {
                List<JiCunQiModel> lis = new List<JiCunQiModel>();
                for (int i = 0; i < KeyS.Count; i++)
                {
                    CunModel cun = JiLu[KeyS[i]];
                    if (cun.ZiSheBeiID == cunmodel.ZiSheBeiID && cun.ZongSheBeiId == cunmodel.ZongSheBeiId)
                    {
                     
                        if (cun.IsDu == CunType.DuJiCunQi)
                        {
                            if (dianwei.IndexOf(cun.JiCunDiZhi+1) >= 0)
                            {
                                lis.Add(cun.JiCunQi);
                            }
                        }
                    }
                }
                return lis;
            }


        }

    }

    
}
