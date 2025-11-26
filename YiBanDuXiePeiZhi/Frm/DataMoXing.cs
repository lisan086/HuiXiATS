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
using System.Security.AccessControl;


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
            foreach (SaoMaModel item in LisSheBei)
            {
                List<CunModel> jicunqimodels = item.PeiZhiJiCunQi;
                for (int c = 0; c < jicunqimodels.Count; c++)
                {
                    CunModel cunmodel = jicunqimodels[c];
                    JiCunQiModel model = new JiCunQiModel();
                    model.SheBeiID = SheBeiID;

                    if (cunmodel.IsDu.ToString().ToLower().StartsWith("du"))
                    {
                        model.WeiYiBiaoShi = cunmodel.Name;
                        model.MiaoSu = cunmodel.MiaoSu;
                        model.DuXie = 1;
                        LisDu.Add(model);
                    }
                    else
                    {
                        model.WeiYiBiaoShi = cunmodel.Name;
                        model.MiaoSu = cunmodel.MiaoSu;
                        model.DuXie = 2;
                        LisXie.Add(model);
                    }
                    LisDuXie.Add(model);
                    if (JiLu.ContainsKey(model.WeiYiBiaoShi) == false)
                    {
                        cunmodel.ZongSheBeiId = item.SheBeiID;
                        cunmodel.JiCunQi = model;
                        JiLu.Add(model.WeiYiBiaoShi, cunmodel);
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
                    foreach (var item in LisSheBei[i].PeiZhiJiCunQi)
                    {
                        item.JiCunQi.IsKeKao = zhuangtai;
                    }
                    break;
                }
            }
        }

        public void SetJiCunQiValue(string weiyibiaoshi, string shuju)
        {
            if (JiLu.ContainsKey(weiyibiaoshi))
            {
                CunModel cunModel = JiLu[weiyibiaoshi];            
                cunModel.JiCunQi.Value = shuju;           
            }
        
        }
        public void SetJiCunQiValue(CunModel model, byte[] shuju)
        {
            if (JiLu.ContainsKey(model.JiCunQi.WeiYiBiaoShi))
            {
                CunModel cunModel = JiLu[model.JiCunQi.WeiYiBiaoShi];
              
                if (model.IsDu == CunType.Xie16FanHuiAscii)
                {
                    cunModel.JiCunQi.Value =Encoding.ASCII.GetString(shuju);
                }
                else if (model.IsDu == CunType.XieAsciiFanHuiAscii)
                {
                    cunModel.JiCunQi.Value = Encoding.ASCII.GetString(shuju);
                }
                else if (model.IsDu == CunType.DuAsciiAnd16Xie)
                {
                    cunModel.JiCunQi.IsKeKao = true;
                    cunModel.IsZhengZaiCe = 1;
                    cunModel.JiCunQi.Value = Encoding.ASCII.GetString(shuju);
                }
                else if (model.IsDu == CunType.DuAsciiXie)
                {
                    cunModel.JiCunQi.IsKeKao = true;
                    cunModel.IsZhengZaiCe = 1;
                    cunModel.JiCunQi.Value = Encoding.ASCII.GetString(shuju);
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
                cunModel.IsZhengZaiCe = sate;
                if (cunModel.IsZhengZaiCe==0)
                {
                    cunModel.JiCunQi.Value = "";
                }
            }

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

        public void GetTxCount(CunModel model, bool istx)
        {
            SaoMaModel models=GetSheBeiModel(model);
            if (models!=null)
            {
                if (istx)
                {
                    models.TxCount = 0;
                }
                else
                {
                    models.TxCount++;
                }
                if (models.TxCount > 5)
                {
                    SetHeGe(models.SheBeiID, false);
                }
                else
                {
                    SetHeGe(models.SheBeiID, true);
                }
            }
          
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


        public byte[] GetZhiLing(CunModel model,string zhiling,out bool fanhui)
        {
            fanhui = false;
            switch (model.IsDu)
            {
                case CunType.XieAsciiFanHuiAscii:
                    {
                        byte[] data = Encoding.ASCII.GetBytes(zhiling);
                        fanhui = true;
                        return data;
                    }

                case CunType.Xie16FanHuiAscii:
                    {
                        byte[] data = ChangYong.HexStringToByte(zhiling);
                        fanhui=true;
                        return data;
                    }

                case CunType.Xie16WuFanHui:
                    {
                        byte[] data = ChangYong.HexStringToByte(zhiling);

                        return data;
                    }
                case CunType.XieAsciiWuFanHui:
                    {
                        byte[] data = Encoding.ASCII.GetBytes(zhiling);
                        return data;
                    }
                case CunType.DuAsciiAnd16Xie:
                    {
                        byte[] data = ChangYong.HexStringToByte(model.ZhiLing);
                        fanhui = true;
                        return data;
                    }
                    
                case CunType.DuAsciiXie:
                    {
                        byte[] data = Encoding.ASCII.GetBytes(model.ZhiLing);
                        fanhui = true;
                        return data;
                    }
                  
           
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
