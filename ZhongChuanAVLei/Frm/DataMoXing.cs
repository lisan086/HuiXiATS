using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseUI.UC;
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
                        List<byte> qishiwei = GetShiJianZhi(zishebei.QiShiDiZhi,true);
                        fenzhuang.AddRange(qishiwei.ToArray());                     
                        fenzhuang.Add(0x00);
                        fenzhuang.Add((byte)zishebei.JiLu);
                        byte[] shu = CRC.ToModbus(fenzhuang, false);
                        fenzhuang.AddRange(shu);
                        zishebei.ZhiLing = fenzhuang;
                        zishebei.ChangDu = zishebei.JiLu * 2;
                    }
                    {//xie
                     
                    }
                    {
                        //du
                        for (int i = 0; i < zishebei.JiLu; i++)
                        {
                            //读
                            {
                                JiCunQiModel model = new JiCunQiModel();
                                model.WeiYiBiaoShi = $"{zishebei.ZiName}读第{i + 1}路";
                                model.SheBeiID = SheBeiID;
                                model.MiaoSu = ChangYong.GetEnumDescription(CunType.DuJiCunQi);
                                model.DuXie = 1;
                                LisDu.Add(model);
                                if (JiLu.ContainsKey(model.WeiYiBiaoShi) == false)
                                {
                                    CunModel cunModel = new CunModel();
                                    cunModel.ZiSheBeiID = zishebei.ZSID;
                                    cunModel.ZongSheBeiId = shebei.SheBeiID;
                                    cunModel.JiCunDiZhi =zishebei.QiShiDiZhi+ i;
                                    cunModel.IsDu = CunType.DuJiCunQi;
                                    cunModel.JiCunQi = model;
                                    cunModel.ZDiZhi = zishebei.DiZhi;
                                    cunModel.JiLu = zishebei.JiLu;
                                    cunModel.QiShiDiZhi = zishebei.QiShiDiZhi;
                                    cunModel.ChengShu = zishebei.ChengShu;
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
                        int yushu = (zhengshu - cunModel.QiShiDiZhi) * 2;
                        if (yushu < shuju.Count - 1 && yushu >= 0)
                        {
                            cunModel.JiCunQi.Value = GetValue(new byte[] { shuju[yushu], shuju[yushu + 1] },cunModel.ChengShu);

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

     

      

     
     
        private object GetValue(byte[] canshu,double chengshu)
        {
            double sdvlaue = BitConverter.ToInt16(JiaoHuanByte(canshu.ToList()).ToArray(), 0);
            double zhenzhsi = sdvlaue*chengshu;
            return zhenzhsi;
        }
        private List<byte> JiaoHuanByte(List<byte> geshu)
        {
            List<byte> shuju = new List<byte>();

            for (int i = 0; i < geshu.Count; i += 2)
            {
                if (geshu.Count > i + 1)
                {
                    shuju.Add(geshu[i + 1]);
                    shuju.Add(geshu[i]);
                }
                else
                {
                    if (geshu.Count > i)
                    {
                        shuju.Add(geshu[i]);
                    }
                }
            }
            return shuju;
        }

     
        public List<byte> GetShiJianZhi(int len, bool isgaoweizaiqian)
        {
            List<byte> shuju = new List<byte>();
            byte[] intBuff = BitConverter.GetBytes(len);
            if (isgaoweizaiqian)
            {
                if (intBuff.Length >= 2)
                {
                    shuju.Add(intBuff[1]);
                    shuju.Add(intBuff[0]);
                }
            }
            else
            {
                if (intBuff.Length >= 2)
                {
                    shuju.Add(intBuff[0]);
                    shuju.Add(intBuff[1]);
                }
            }
            return shuju;
        }
        public List<CunModel> GetSheBeiJiCunQi(int shebeiid)
        {
            List<CunModel> lis = new List<CunModel>();
            for (int i = 0; i < KeyS.Count; i++)
            {
                CunModel cun = JiLu[KeyS[i]];
                if (cun.ZongSheBeiId == shebeiid)
                {
                    lis.Add(cun);
                }
            }
            return lis;
        }
    }

    
}
