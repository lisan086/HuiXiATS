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
using XiangTongChuanKouSheBei.ChuLiLei.SheBeiLei;
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

        private Dictionary<int, ABSZiSheBeiLei> LisZiSheBei = new Dictionary<int, ABSZiSheBeiLei>();

        private List<string> KeyS = new List<string>();
      
        /// <summary>
        /// 用于初始化
        /// </summary>
        public void IniData(string lujing)
        {
            LisZiSheBei.Clear();
            LisDu.Clear();
            LisXie.Clear();
            JiLu.Clear();
            LisDuXie.Clear();
            JosnOrSModel JosnOrSModel = new JosnOrSModel(lujing);
            LisSheBei = JosnOrSModel.GetLisTModel<ZSModel>();
            if (LisSheBei == null)
            {
                LisSheBei = new List<ZSModel>();
            }
            for (int c = 0; c < LisSheBei.Count; c++)
            {
                ZSModel shebei = LisSheBei[c];
                for (int f = 0; f < shebei.LisSheBei.Count; f++)
                {
                    ZiSheBeiModel lis= shebei.LisSheBei[f];
                    lis.SheBeiID=shebei.SheBeiID;
                    for (int d = 0; d < lis.LisJiCunQi.Count; d++)
                    {
                        CunModel cunmodel = lis.LisJiCunQi[d];
                        cunmodel.SheBeiID = lis.SheBeiID;
                        cunmodel.ZiID = lis.ZiID;
                        JiCunQiModel model = new JiCunQiModel();
                        model.WeiYiBiaoShi = $"{lis.Name}-{cunmodel.Name}";
                        model.SheBeiID = SheBeiID;
                        model.MiaoSu = cunmodel.MiaoSu;
                        if (cunmodel.IsDu==CunType.DuShuJu)
                        {                        
                            model.DuXie = 1;
                            LisDu.Add(model);
                            LisDuXie.Add(model);
                        }
                        else if (cunmodel.IsDu == CunType.XieShuJu)
                        {
                            model.DuXie = 2;
                            LisXie.Add(model);
                            LisDuXie.Add(model);
                        }
                        else if (cunmodel.IsDu == CunType.DuXieYiQi)
                        {
                            model.DuXie = 3;
                            LisXie.Add(model);
                            LisDu.Add(model);
                            LisDuXie.Add(model);
                        }

                        if (JiLu.ContainsKey(model.WeiYiBiaoShi) == false)
                        {
                            cunmodel.JiCunQi = model;
                            JiLu.Add(model.WeiYiBiaoShi, cunmodel);
                        }
                    }
                    for (int d = 0; d < lis.ZhiLingS.Count; d++)
                    {
                        lis.ZhiLingS[d].SendZhiLing = ChangYong.HexStringToByte(lis.ZhiLingS[d].DuZhiLing);
                    }
                    if (LisZiSheBei.ContainsKey(lis.ZiID)==false)
                    {
                        if (lis.SheBeiType==SheBeiType.JianDanModBusRTU)
                        {
                            ABSZiSheBeiLei lei = new JianDanModBusLei();
                            lei.SetCanShu(lis);
                            LisZiSheBei.Add(lis.ZiID, lei);
                        }
                    }
                }
               
            }
            KeyS = JiLu.Keys.ToList();
        }

        public void SetTX(int shebeiid,int zishebeiid,bool zhuangtai)
        {
            if (zishebeiid<0)
            {
                for (int i = 0; i < LisSheBei.Count; i++)
                {
                    if (LisSheBei[i].SheBeiID == shebeiid)
                    {
                        List<ZiSheBeiModel> dizhis = LisSheBei[i].LisSheBei;
                        foreach (var item in dizhis)
                        {
                            item.Tx = zhuangtai;
                        }
                        break;
                    }
                }
                for (int i = 0; i < KeyS.Count; i++)
                {
                    CunModel cunModel = JiLu[KeyS[i]];
                    if (cunModel.SheBeiID == shebeiid)
                    {
                        cunModel.JiCunQi.IsKeKao = zhuangtai;
                    }
                }
            }
            else
            {
                for (int i = 0; i < LisSheBei.Count; i++)
                {
                    if (LisSheBei[i].SheBeiID == shebeiid)
                    {
                        List<ZiSheBeiModel> dizhis = LisSheBei[i].LisSheBei;
                        for (int c = 0; c < dizhis.Count; c++)
                        {
                            ZiSheBeiModel modes = dizhis[c];
                            if (modes.ZiID == zishebeiid)
                            {
                                modes.Tx = zhuangtai;
                                break;
                            }
                        }
                        break;
                    }
                }
                for (int i = 0; i < KeyS.Count; i++)
                {
                    CunModel cunModel = JiLu[KeyS[i]];
                    if (cunModel.SheBeiID == shebeiid)
                    {
                        if (cunModel.ZiID == zishebeiid)
                        {
                            cunModel.JiCunQi.IsKeKao = zhuangtai;
                        }
                    }
                }
            }
           
        }


        public void SetSate(CunModel model,int state)
        {
            if (JiLu.ContainsKey(model.JiCunQi.WeiYiBiaoShi))
            {
                CunModel xinmodel = JiLu[model.JiCunQi.WeiYiBiaoShi];
                xinmodel.XieState = state;
                if (state==0)
                {
                    xinmodel.JiCunQi.Value = "";
                }
            }
        }



        public void SetXieJiCunQiZhi(CunModel model, object shuju)
        {
            if (JiLu.ContainsKey(model.JiCunQi.WeiYiBiaoShi))
            {
                CunModel xinmodel = JiLu[model.JiCunQi.WeiYiBiaoShi];
                xinmodel.JiCunQi.Value = shuju;
            }
        }

    

     
     
    
        public CunModel GetCunModel(JiCunQiModel model,bool isfuzhi)
        {
            if (JiLu.ContainsKey(model.WeiYiBiaoShi))
            {
                CunModel xinmodel = JiLu[model.WeiYiBiaoShi];
                if (isfuzhi)
                {
                    return xinmodel.FuZhi();
                }
                else
                {
                    return xinmodel;
                }
            }
            return null;
        }

   

        public List<CunModel> GetSheBeiJiCunQi(int shebeiid)
        {
            List<CunModel> lis = new List<CunModel>();
            for (int i = 0; i < KeyS.Count; i++)
            {
                CunModel cun = JiLu[KeyS[i]];
                if (cun.SheBeiID == shebeiid)
                {
                    lis.Add(cun);
                }
            }
            return lis;
        }

       
        public List<List<byte>> GetSendCMD(CunModel model,out int xierutime)
        {
            xierutime = 20;
            List<List<byte>> cmd = new List<List<byte>>();
            if (LisZiSheBei.ContainsKey(model.ZiID))
            {
                FanHuiModel mos = LisZiSheBei[model.ZiID].GetSendCMD(model);
                cmd = mos.SendData;
                xierutime = mos.XieTime;
            }
            return cmd;
        }

        public void JieShouShuJu(byte[] shuju,int datachangdu, int zishebeiid)
        {
            if (LisZiSheBei.ContainsKey(zishebeiid))
            {
                LisZiSheBei[zishebeiid].JieShouShuJu(shuju, datachangdu);
            }
        }
        public int JiaoYanShuJu(int zishebeiid,DuZhiLingModel model)
        {
            if (LisZiSheBei.ContainsKey(zishebeiid))
            {
               return   LisZiSheBei[zishebeiid].JiaYanShuJu(model);
            }
            return 1;
        }
        public void ClearData(int zishebeiid)
        {
            if (LisZiSheBei.ContainsKey(zishebeiid))
            {
                LisZiSheBei[zishebeiid].ClearData();              
            }
        }
    }

    
}
