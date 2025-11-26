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
                int changdu = shebei.ChangDu * 2;
                shebei.ZhiLing.Clear();
                shebei.Tx.Clear();
                shebei.TxCiShu.Clear();
                List<int> shebeidizhis = shebei.DiZhi;
                for (int f = 0; f < shebeidizhis.Count; f++)
                {
                    int dizhi = shebeidizhis[f];
                    List<string> lisxie = ChangYong.MeiJuLisName(typeof(CunType));
                    for (int i = 0; i < lisxie.Count; i++)
                    {
                        if (lisxie[i].ToLower().StartsWith("xie"))
                        {
                            CunType cunType = ChangYong.GetMeiJuZhi<CunType>(lisxie[i]);
                            JiCunQiModel model = new JiCunQiModel();
                            model.WeiYiBiaoShi = $"{shebei.Name}-{dizhi}-{lisxie[i]}";
                            model.SheBeiID = SheBeiID;
                            model.MiaoSu = ChangYong.GetEnumDescription(cunType);
                            model.DuXie = 2;
                            if (JiLu.ContainsKey(model.WeiYiBiaoShi) == false)
                            {
                                List<int> lisints = GetJiCunDiZhi(cunType, shebei);
                                CunModel cunModel = new CunModel();
                                cunModel.ZongSheBeiId = shebei.SheBeiID;
                                cunModel.JiCunDiZhi = lisints[0];
                                cunModel.PianYiLiang = lisints[1];
                                cunModel.ChangDu = lisints[2];
                                cunModel.IsDu = cunType;
                                cunModel.JiCunQi = model;
                                cunModel.ZDiZhi = dizhi;
                                cunModel.DianLiuChengShu = shebei.DianLiuChengShu;
                                cunModel.DianYaChengShu = shebei.DianYaChengShu;
                                cunModel.GongLuChengShu = shebei.GongLuChengShu;
                                cunModel.QiShiDiZhi = shebei.QiShiDiZhi;

                                JiLu.Add(model.WeiYiBiaoShi, cunModel);
                            }
                            LisXie.Add(model);
                            LisDuXie.Add(model);
                        }
                        else
                        {
                            CunType cunType = ChangYong.GetMeiJuZhi<CunType>(lisxie[i]);
                            JiCunQiModel model = new JiCunQiModel();
                            model.WeiYiBiaoShi = $"{shebei.Name}-{dizhi}-{lisxie[i]}";
                            model.SheBeiID = SheBeiID;
                            model.MiaoSu = ChangYong.GetEnumDescription(cunType);
                            model.DuXie = 1;
                            if (JiLu.ContainsKey(model.WeiYiBiaoShi) == false)
                            {
                                List<int> lisints = GetJiCunDiZhi(cunType, shebei);
                                CunModel cunModel = new CunModel();
                                cunModel.ZongSheBeiId = shebei.SheBeiID;
                                cunModel.JiCunDiZhi = lisints[0];
                                cunModel.PianYiLiang = lisints[1];
                                cunModel.ChangDu = lisints[2];
                                cunModel.IsDu = cunType;
                                cunModel.JiCunQi = model;
                                cunModel.ZDiZhi =dizhi;
                                cunModel.QiShiDiZhi = shebei.QiShiDiZhi;
                                cunModel.DianLiuChengShu = shebei.DianLiuChengShu;
                                cunModel.DianYaChengShu = shebei.DianYaChengShu;
                                cunModel.GongLuChengShu = shebei.GongLuChengShu;
                                JiLu.Add(model.WeiYiBiaoShi, cunModel);
                            }
                            LisDu.Add(model);
                            LisDuXie.Add(model);
                        }
                    }
                    {
                        List<byte> fenzhuang = new List<byte>();
                        fenzhuang.Add((byte)dizhi);
                        fenzhuang.Add(0x03);
                        List<byte> qishiwei = GetShiJianZhi(shebei.QiShiDiZhi, true);
                        fenzhuang.AddRange(qishiwei.ToArray());
                        fenzhuang.Add(0x00);
                        fenzhuang.Add((byte)(changdu/2));
                        byte[] shu = CRC.ToModbus(fenzhuang, false);
                        fenzhuang.AddRange(shu);
                      
                        shebei.ChangDu = changdu;
                        if (shebei.ZhiLing.ContainsKey(dizhi)==false)
                        {
                            shebei.ZhiLing.Add(dizhi, fenzhuang);
                            shebei.Tx.Add(dizhi,false);
                            shebei.TxCiShu.Add(dizhi,5);
                        }
                    }
                }
              
            }
            KeyS = JiLu.Keys.ToList();
        }

        public void SetTX(int zongid,int dizhi,bool zhuangtai)
        {
            for (int i = 0; i < LisSheBei.Count; i++)
            {
                if (LisSheBei[i].SheBeiID == zongid)
                {
                    if (dizhi < 0)
                    {
                        List<int> dizhis = LisSheBei[i].ZhiLing.Keys.ToList();
                        foreach (var item in dizhis)
                        {
                            LisSheBei[i].Tx[item] = zhuangtai;
                        }
                    }
                    else
                    {
                        if (LisSheBei[i].Tx.ContainsKey(dizhi))
                        {
                            LisSheBei[i].Tx[dizhi] = zhuangtai;
                        }
                    }
                    break;
                }
            }
            for (int i = 0; i < KeyS.Count; i++)
            {
                CunModel cunModel = JiLu[KeyS[i]];
                if (cunModel.ZongSheBeiId == zongid)
                {
                    if (dizhi < 0)
                    {
                        cunModel.JiCunQi.IsKeKao = zhuangtai;
                    }
                    else
                    {
                        if (cunModel.ZDiZhi==dizhi)
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

        public void SetJiCunQiDuValue(int zongid,int dizhi,List<byte> shuju)
        {
            for (int i = 0; i < KeyS.Count; i++)
            {
                CunModel cunModel = JiLu[KeyS[i]];

                if (cunModel.ZongSheBeiId == zongid&& cunModel.ZDiZhi== dizhi)
                {
                    cunModel.JiCunQi.IsKeKao = true;
                    List<byte> shijidu = new List<byte>();
                    for (int c = cunModel.PianYiLiang; c < cunModel.PianYiLiang + cunModel.ChangDu; c++)
                    {
                        if (c < shuju.Count)
                        {
                            shijidu.Add(shuju[c]);
                        }
                    }
                    if (cunModel.IsDu == CunType.DuDianYuanState)
                    {
                        int zhi = 0;
                        for (int j = 0; j < shijidu.Count; j++)
                        {
                            if (shijidu[j] == 0x01)
                            {
                                zhi = 1;
                                break;
                            }
                        }
                        cunModel.JiCunQi.Value = zhi;
                    }
                    else if (cunModel.IsDu== CunType.DuSetDianLiu)
                    {                   
                        double jieguozhi = GetValue(shijidu);
                        cunModel.JiCunQi.Value = jieguozhi*cunModel.DianLiuChengShu;
                    }
                    else if (cunModel.IsDu == CunType.DuSetDianYa)
                    {
                        double jieguozhi = GetValue(shijidu);
                        cunModel.JiCunQi.Value = jieguozhi * cunModel.DianYaChengShu;
                    }
                    else if (cunModel.IsDu == CunType.DuShiShiDianLiu)
                    {
                        double jieguozhi = GetValue(shijidu);
                        cunModel.JiCunQi.Value = jieguozhi * cunModel.DianLiuChengShu;
                    }
                    else if (cunModel.IsDu == CunType.DuShiShiDianYa)
                    {
                        double jieguozhi = GetValue(shijidu);
                        cunModel.JiCunQi.Value = jieguozhi * cunModel.DianYaChengShu;
                    }
                    else if (cunModel.IsDu == CunType.DuShiShiGongLu)
                    {
                        double jieguozhi = GetValue(shijidu);
                        cunModel.JiCunQi.Value = jieguozhi * cunModel.GongLuChengShu;
                    }
                }
              
            }
            for (int i = 0; i < LisSheBei.Count; i++)
            {
                if (LisSheBei[i].SheBeiID== zongid)
                {
                    if (LisSheBei[i].Tx.ContainsKey(dizhi))
                    {
                        LisSheBei[i].Tx[dizhi] = true;
                    }
                  
                    break;
                }
            }
        }

     
        private double GetValue(List<byte> canshu)
        {
            if (canshu.Count == 2)
            {
                double sdvlaue = BitConverter.ToInt16(JiaoHuanByte(canshu.ToList()).ToArray(), 0);
              
                return sdvlaue;
            }
            else
            {
                double sdvlaue = BitConverter.ToInt32(JiaoHuanByte(canshu.ToList()).ToArray(), 0);
              
                return sdvlaue;
            }
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

        public ZSModel GetSheBeiModel(CunModel model)
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

        public byte[] GetBtyez(int value)
        {


            int hValue = (value >> 8) & 0xFF;

            int lValue = value & 0xFF;

            byte[] arr = new byte[] { (byte)hValue, (byte)lValue };
            return arr;
        }
        public List<List< byte>> GetCMD(CunModel model)
        {

            if (model.IsDu == CunType.XieDianYuanOFF)
            {
                List<byte> xieshuju = new List<byte>();
                xieshuju.Add((byte)model.ZDiZhi);
                xieshuju.Add(0x06);
                int dizhi = model.JiCunDiZhi;
                byte[] dishi = GetBtyez(dizhi);
                xieshuju.AddRange(dishi);
                int xinzhi = 0;
                byte[] dishsi = GetShiJianZhi(xinzhi, true).ToArray();
                xieshuju.AddRange(dishsi);
                byte[] shu = CRC.ToModbus(xieshuju, false);
                xieshuju.AddRange(shu);
                return new List<List<byte>>() { xieshuju };
            }
            else if (model.IsDu == CunType.XieDianYuanON)
            {
                List<byte> xieshuju = new List<byte>();
                xieshuju.Add((byte)model.ZDiZhi);
                xieshuju.Add(0x06);
                int dizhi = model.JiCunDiZhi;
                byte[] dishi = GetBtyez(dizhi);
                xieshuju.AddRange(dishi);
                int xinzhi = 1;
                byte[] dishsi = GetShiJianZhi(xinzhi, true).ToArray();
                xieshuju.AddRange(dishsi);
                byte[] shu = CRC.ToModbus(xieshuju, false);
                xieshuju.AddRange(shu);
                return new List<List<byte>>() { xieshuju };
            }
            else if (model.IsDu == CunType.XieSetDianLiu)
            {
                List<byte> xieshuju = new List<byte>();
                xieshuju.Add((byte)model.ZDiZhi);
                xieshuju.Add(0x06);
                int dizhi = model.JiCunDiZhi;
                byte[] dishi = GetBtyez(dizhi);
                xieshuju.AddRange(dishi);
                double changdu = ChangYong.TryFloat(model.JiCunQi.Value,0);
                double dianliuchengxu = model.DianLiuChengShu;
                if (dianliuchengxu == 0)
                {
                    dianliuchengxu = 1;
                }
                changdu = changdu / dianliuchengxu;
                int xinzhi = (int)changdu;
                byte[] dishsi = GetShiJianZhi(xinzhi, true).ToArray();
                xieshuju.AddRange(dishsi);
                byte[] shu = CRC.ToModbus(xieshuju, false);
                xieshuju.AddRange(shu);
                return new List<List<byte>>() { xieshuju };
            }
            else if (model.IsDu == CunType.XieSetDianYa)
            {
                List<byte> xieshuju = new List<byte>();
                xieshuju.Add((byte)model.ZDiZhi);
                xieshuju.Add(0x06);
                int dizhi = model.JiCunDiZhi;
                byte[] dishi = GetBtyez(dizhi);
                xieshuju.AddRange(dishi);
                double changdu = ChangYong.TryFloat(model.JiCunQi.Value, 0);
                double dianliuchengxu = model.DianYaChengShu;
                if (dianliuchengxu == 0)
                {
                    dianliuchengxu = 1;
                }
                changdu = changdu / dianliuchengxu;
                int xinzhi = (int)changdu;
                byte[] dishsi = GetShiJianZhi(xinzhi, true).ToArray();
                xieshuju.AddRange(dishsi);
                byte[] shu = CRC.ToModbus(xieshuju, false);
                xieshuju.AddRange(shu);
                return new List<List<byte>>() { xieshuju };
            }
            else if (model.IsDu == CunType.XieSetDianYuanON)
            {
                ZSModel mods = GetSheBeiModel(model);
                if (mods != null)
                {
                    List<List<byte>> lis = new List<List<byte>>();
                    foreach (var item in JiLu.Keys)
                    {
                        CunModel sid= JiLu[item];
                        if (sid.IsDu== CunType.XieSetDianLiu&&sid.ZongSheBeiId==mods.SheBeiID)
                        {
                            List<byte> xieshuju = new List<byte>();
                            xieshuju.Add((byte)model.ZDiZhi);
                            xieshuju.Add(0x06);
                            int dizhi = sid.JiCunDiZhi;
                            byte[] dishi = GetBtyez(dizhi);
                            xieshuju.AddRange(dishi);
                            double changdu = ChangYong.TryFloat(mods.SetDianLiu, 0);
                            double dianliuchengxu = model.DianLiuChengShu;
                            if (dianliuchengxu == 0)
                            {
                                dianliuchengxu = 1;
                            }
                            changdu = changdu / dianliuchengxu;
                            int xinzhi = (int)changdu;
                            byte[] dishsi = GetShiJianZhi(xinzhi, true).ToArray();
                            xieshuju.AddRange(dishsi);
                            byte[] shu = CRC.ToModbus(xieshuju, false);
                            xieshuju.AddRange(shu);
                            lis.Add(xieshuju);
                        }
                        if (sid.IsDu == CunType.XieSetDianYa && sid.ZongSheBeiId == mods.SheBeiID)
                        {
                            List<byte> xieshuju = new List<byte>();
                            xieshuju.Add((byte)model.ZDiZhi);
                            xieshuju.Add(0x06);
                            int dizhi = sid.JiCunDiZhi;
                            byte[] dishi = GetBtyez(dizhi);
                            xieshuju.AddRange(dishi);
                            double changdu = ChangYong.TryFloat(mods.SetDianYa, 0);
                            double dianliuchengxu = model.DianYaChengShu;
                            if (dianliuchengxu == 0)
                            {
                                dianliuchengxu = 1;
                            }
                            changdu = changdu / dianliuchengxu;
                            int xinzhi = (int)changdu;
                            byte[] dishsi = GetShiJianZhi(xinzhi, true).ToArray();
                            xieshuju.AddRange(dishsi);
                            byte[] shu = CRC.ToModbus(xieshuju, false);
                            xieshuju.AddRange(shu);
                            lis.Add(xieshuju);
                        }
                    }
                 
                    {
                        List<byte> xieshuju = new List<byte>();
                        xieshuju.Add((byte)model.ZDiZhi);
                        xieshuju.Add(0x06);
                        int dizhi = model.JiCunDiZhi;
                        byte[] dishi = GetBtyez(dizhi);
                        xieshuju.AddRange(dishi);
                        int xinzhi = 1;
                        byte[] dishsi = GetShiJianZhi(xinzhi, true).ToArray();
                        xieshuju.AddRange(dishsi);
                        byte[] shu = CRC.ToModbus(xieshuju, false);
                        xieshuju.AddRange(shu);
                        lis.Add(xieshuju);
                    }
                    return lis;
                }
            }
            return new List<List<byte>>();
        }

        private List<int> GetJiCunDiZhi(CunType model,ZSModel zsmodel)
        {
            for (int i = 0; i < zsmodel.DuiYingDiZhi.Count; i++)
            {
                string dizhi = zsmodel.DuiYingDiZhi[i];
                if (dizhi.StartsWith(model.ToString()))
                {
                    string[] sankuai = dizhi.Split(':');
                    if (sankuai.Length>=3)
                    {
                        return new List<int>() { ChangYong.TryInt(sankuai[1], 0), ChangYong.TryInt(sankuai[2], 0), ChangYong.TryInt(sankuai[3], 0) };
                    }
                    break;
                }
            }
            return new List<int>() { 0,0,0};
        }
    }

    
}
