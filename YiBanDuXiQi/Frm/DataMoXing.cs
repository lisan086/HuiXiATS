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
using System.IO;


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
                    List<string> lis = ChangYong.MeiJuLisName(typeof(CunType));
                    for (int i = 0; i < lis.Count; i++)
                    {
                        JiCunQiModel model = new JiCunQiModel();
                        model.SheBeiID = SheBeiID;
                        
                        if (lis[i].ToLower().Contains("du"))
                        {                       
                            model.WeiYiBiaoShi = $"{shebei.Name}:{lis[i]}";                      
                            model.MiaoSu = ChangYong.GetEnumDescription(ChangYong.GetMeiJuZhi<CunType>(lis[i]));
                            model.DuXie = 1;
                            LisDu.Add(model);
                        }
                        else
                        {
                            model.WeiYiBiaoShi = $"{shebei.Name}:{lis[i]}";                           
                            model.MiaoSu = ChangYong.GetEnumDescription(ChangYong.GetMeiJuZhi<CunType>(lis[i]));
                            model.DuXie = 2;
                            LisXie.Add(model);
                        }
                        LisDuXie.Add(model);
                        if (JiLu.ContainsKey(model.WeiYiBiaoShi) == false)
                        {
                            CunModel cunModel = new CunModel();
                            cunModel.ZongSheBeiId = shebei.SheBeiID;
                            cunModel.IsDu = ChangYong.GetMeiJuZhi<CunType>(lis[i]);
                            cunModel.JiCunQi = model;
                            cunModel.Time = shebei.Time;
                            cunModel.JieShouCount = shebei.JieShouCount;
                            JiLu.Add(model.WeiYiBiaoShi, cunModel);
                        }
                    }
                   
                }
            }
            KeyS = JiLu.Keys.ToList();
        }

   
        public void SetHeGe(int zongid,bool zhuangtai)
        {
        

            for (int i = 0; i < LisSheBei.Count; i++)
            {
                if (LisSheBei[i].SheBeiID == zongid)
                {
                    LisSheBei[i].TX = zhuangtai;
                    break;
                }
            }
        }

        public void SetJiCunQiValue(string weiyibiaoshi, List<byte> shuju)
        {
            if (JiLu.ContainsKey(weiyibiaoshi))
            {
                CunModel cunModel = JiLu[weiyibiaoshi];
                cunModel.JiCunQi.IsKeKao = true;
                cunModel.JiCunQi.Value = Encoding.ASCII.GetString(shuju.ToArray());           
            }
        
        }
        public void SetJiCunQiValue(CunModel model, byte[] shuju)
        {
            if (JiLu.ContainsKey(model.JiCunQi.WeiYiBiaoShi))
            {
                CunModel cunModel = JiLu[model.JiCunQi.WeiYiBiaoShi];
                cunModel.JiCunQi.IsKeKao = true;
                if (model.IsDu == CunType.XieModBusRTUDan1FanHui)
                {
                    List<string> jies = ChangYong.JieGeStr(model.JiCunQi.Value.ToString(), ',');
                    int index = ChangYong.TryInt(jies[jies.Count - 1],0);
                    List<int> jiexie = ChangYong.Get10Or2(shuju[3],8);
                    if (index < jiexie.Count)
                    {
                        cunModel.JiCunQi.Value = jiexie[index];
                    }
                    else
                    {
                        cunModel.JiCunQi.Value = "";
                    }
                }
                else if (model.IsDu == CunType.XieModBusRTUDan2FanHui)
                {
                    List<string> jies = ChangYong.JieGeStr(model.JiCunQi.Value.ToString(), ',');
                    int zhi = CRC.GetInt(new List<byte>() { shuju[3], shuju[4] }, jies[jies.Count - 1] == "1", 0);
                    cunModel.JiCunQi.Value = zhi;
                }
                else if (model.IsDu == CunType.XieModBusRTUDan4FanHui)
                {
                    List<string> jies = ChangYong.JieGeStr(model.JiCunQi.Value.ToString(), ',');
                    int zhi = CRC.GetInt(new List<byte>() { shuju[3], shuju[4], shuju[5], shuju[6] }, jies[jies.Count - 1] == "1", 0);
                    cunModel.JiCunQi.Value = zhi;
                }
                else
                {
                    cunModel.JiCunQi.Value = "";
                }
            }

        }
        public void SetZhengZaiValue(string weiyibiaoshi,int sate)
        {
            if (JiLu.ContainsKey(weiyibiaoshi))
            {
                CunModel cunModel = JiLu[weiyibiaoshi];
                cunModel.JiCunQi.IsKeKao = true;
                cunModel.IsZhengZaiCe = sate;
              
            }

        }

        /// <summary>
        /// 1是成功 0是未测完 3 不存在 其他表示超时
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public CunModel IsChengGong(int zongid, CunType cunType)
        {
            foreach (var item in JiLu.Keys)
            {
                CunModel cunModel = JiLu[item];
                if (cunModel.IsDu == cunType && cunModel.ZongSheBeiId == zongid)
                {
                    return cunModel;
                }

            }

            return null;
        }

        public CunModel GetModel(JiCunQiModel model)
        {
            if (JiLu.ContainsKey(model.WeiYiBiaoShi))
            {
                CunModel cunModel = JiLu[model.WeiYiBiaoShi];
                return cunModel;
            }
            return null;
        }

        public JiCunQiModel GetModel(int zongid,CunType cunType)
        {
            foreach (var item in JiLu.Keys)
            {
                CunModel cunModel = JiLu[item];
                if (cunModel.IsDu == cunType && cunModel.ZongSheBeiId == zongid)
                {
                    return cunModel.JiCunQi;
                }

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


        public byte[] GetZhiLing(CunModel model,out bool fanhui)
        {
            fanhui = false;
            switch (model.IsDu)
            {
                case CunType.XieAsciiFanHuiAscii:
                    {
                        byte[] data = Encoding.ASCII.GetBytes(model.JiCunQi.Value.ToString());
                        fanhui = true;
                        return data;
                    }

                case CunType.Xie16FanHuiAscii:
                    {
                        byte[] data = ChangYong.HexStringToByte(model.JiCunQi.Value.ToString());
                        fanhui=true;
                        return data;
                    }

                case CunType.Xie16WuFanHui:
                    {
                        byte[] data = ChangYong.HexStringToByte(model.JiCunQi.Value.ToString());

                        return data;
                    }
                case CunType.XieAsciiWuFanHui:
                    {
                        byte[] data = Encoding.ASCII.GetBytes(model.JiCunQi.Value.ToString());
                        return data;
                    }
                case CunType.XieZhiLing1FanHuiAscii:
                    {
                        SaoMaModel saomamaodel =GetSheBeiModel(model);
                        if (saomamaodel != null)
                        {
                            string zhi = string.Format(saomamaodel.ZhiLing1, model.JiCunQi.Value);
                            byte[] data = Encoding.ASCII.GetBytes(zhi);
                            fanhui = true;
                            return data;
                        }
                      
                    }
                    break;
                case CunType.XieZhiLing2FanHuiAscii:
                    {
                        {
                            SaoMaModel saomamaodel = GetSheBeiModel(model);
                            if (saomamaodel != null)
                            {
                                string zhi = string.Format(saomamaodel.ZhiLing2, model.JiCunQi.Value);
                                byte[] data = Encoding.ASCII.GetBytes(zhi);
                                fanhui = true;
                                return data;
                            }
                       
                        }
                    }
                    break;
                case CunType.XieZhiLing1WuFanHui:
                    {
                        SaoMaModel saomamaodel = GetSheBeiModel(model);
                        if (saomamaodel != null)
                        {
                            string zhi = string.Format(saomamaodel.ZhiLing1, model.JiCunQi.Value);
                            byte[] data = Encoding.ASCII.GetBytes(zhi);
                            return data;
                        }
                    
                    }
                    break;
                case CunType.XieZhiLing2WuFanHui:
                    {
                        SaoMaModel saomamaodel = GetSheBeiModel(model);
                        if (saomamaodel != null)
                        {
                            string zhi = string.Format(saomamaodel.ZhiLing2, model.JiCunQi.Value);
                            byte[] data = Encoding.ASCII.GetBytes(zhi);
                            return data;
                        }
                       
                       
                    }
                    break;
                case CunType.XieModBusRTUDan1FanHui:
                    {
                        List<string> lis = ChangYong.JieGeStr(model.JiCunQi.Value.ToString(),',');
                        if (lis.Count >= 3)
                        {
                            List<byte> fenzhuang = new List<byte>();
                            fenzhuang.Add(ChangYong.TryByte(lis[0],0x01));
                            fenzhuang.Add(ChangYong.TryByte(lis[2],0x03));
                            byte[] shuju = GetBtyez(ChangYong.TryInt(lis[1],0));
                            fenzhuang.AddRange(shuju);
                            fanhui = true;
                            int shuliang = 1;
                            byte[] countbyte = GetBtyez(shuliang);
                            fenzhuang.AddRange(countbyte);
                            byte[] shu = CRC.ToModbus(fenzhuang, false);
                            fenzhuang.AddRange(shu);
                            return fenzhuang.ToArray();
                        }
                    }
                    break;
                case CunType.XieModBusRTUDan2FanHui:
                    {
                        List<string> lis = ChangYong.JieGeStr(model.JiCunQi.Value.ToString(), ',');
                        if (lis.Count >= 3)
                        {
                            List<byte> fenzhuang = new List<byte>();
                            fenzhuang.Add(ChangYong.TryByte(lis[0], 0x01));
                            fenzhuang.Add(ChangYong.TryByte(lis[2], 0x03));
                            byte[] shuju = GetBtyez(ChangYong.TryInt(lis[1], 0));
                            fenzhuang.AddRange(shuju);
                            fanhui = true;
                            int shuliang = 1;
                            byte[] countbyte = GetBtyez(shuliang);
                            fenzhuang.AddRange(countbyte);
                            byte[] shu = CRC.ToModbus(fenzhuang, false);
                            fenzhuang.AddRange(shu);
                            return fenzhuang.ToArray();
                        }
                    }
                    break;
                case CunType.XieModBusRTUDan4FanHui:
                    {
                        List<string> lis = ChangYong.JieGeStr(model.JiCunQi.Value.ToString(), ',');
                        if (lis.Count >= 3)
                        {
                            List<byte> fenzhuang = new List<byte>();
                            fenzhuang.Add(ChangYong.TryByte(lis[0], 0x01));
                            fenzhuang.Add(ChangYong.TryByte(lis[2], 0x03));
                            byte[] shuju = GetBtyez(ChangYong.TryInt(lis[1], 0));
                            fenzhuang.AddRange(shuju);
                            fanhui = true;
                            int shuliang = 2;
                            byte[] countbyte = GetBtyez(shuliang);
                            fenzhuang.AddRange(countbyte);
                            byte[] shu = CRC.ToModbus(fenzhuang, false);
                            fenzhuang.AddRange(shu);
                            return fenzhuang.ToArray();
                        }
                    }
                    break;
                default:
                    break;
            }

            return null;
        }

        public byte[] GetBtyez(int value)
        {


            int hValue = (value >> 8) & 0xFF;

            int lValue = value & 0xFF;

            byte[] arr = new byte[] { (byte)hValue, (byte)lValue };
            return arr;
        }
    }
}
