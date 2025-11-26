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
                                         
                        if (lis[i].ToLower().Contains("xie"))
                        {
                            JiCunQiModel model = new JiCunQiModel();
                            model.SheBeiID = SheBeiID;
                            model.WeiYiBiaoShi = $"{shebei.Name}:{lis[i]}";
                            model.MiaoSu = "写这个是获取ping值，开始ping是会Ping所有";
                            model.DuXie = 2;
                            LisXie.Add(model);
                            LisDuXie.Add(model);
                            if (JiLu.ContainsKey(model.WeiYiBiaoShi) == false)
                            {
                                CunModel cunModel = new CunModel();
                                cunModel.ZongSheBeiId = shebei.SheBeiID;
                                cunModel.IsDu = ChangYong.GetMeiJuZhi<CunType>(lis[i]);
                                cunModel.JiCunQi = model;
                                cunModel.Time = shebei.Time;
                                
                                JiLu.Add(model.WeiYiBiaoShi, cunModel);
                            }
                        }                                      
                    }
                   
                }
            }
            {
                JiCunQiModel model = new JiCunQiModel();
                model.SheBeiID = SheBeiID;
                model.WeiYiBiaoShi = $"{CunType.KaiShiPing}";
                model.MiaoSu = "开始ping是会Ping所有";
                model.DuXie = 2;
                LisXie.Add(model);
                LisDuXie.Add(model);
                if (JiLu.ContainsKey(model.WeiYiBiaoShi) == false)
                {
                    CunModel cunModel = new CunModel();
                    cunModel.ZongSheBeiId = -1;
                    cunModel.IsDu = CunType.KaiShiPing;
                    cunModel.JiCunQi = model;
                    cunModel.Time = 1000;

                    JiLu.Add(model.WeiYiBiaoShi, cunModel);
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

        public void SetJiCunQiValue(string weiyibiaoshi, string shuju)
        {
            if (JiLu.ContainsKey(weiyibiaoshi))
            {
                CunModel cunModel = JiLu[weiyibiaoshi];
                cunModel.JiCunQi.IsKeKao = true;
                cunModel.JiCunQi.Value = shuju;           
            }
        
        }
        public void SetZhengZaiValue(string weiyibiaoshi,int sate)
        {
            if (JiLu.ContainsKey(weiyibiaoshi))
            {
                CunModel cunModel = JiLu[weiyibiaoshi];
                cunModel.JiCunQi.IsKeKao = true;
                cunModel.IsZhengZaiCe = sate;
                if (cunModel.IsDu==CunType.KaiShiPing)
                {
                    if (sate==0)
                    {
                        for (int i = 0; i < KeyS.Count; i++)
                        {
                            
                            JiLu[KeyS[i]].IsZhengZaiCe = 0;
                        }
                    }
                }
            }

        }

        /// <summary>
        /// 1是成功 0是未测完 3 不存在 其他表示超时
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public CunModel IsChengGong(int zongid, CunType cunType)
        {
            if (cunType == CunType.KaiShiPing)
            {
                foreach (var item in JiLu.Keys)
                {
                    CunModel cunModel = JiLu[item];
                    if (cunModel.IsDu == cunType)
                    {
                        return cunModel;
                    }

                }
            }
            else
            {
                foreach (var item in JiLu.Keys)
                {
                    CunModel cunModel = JiLu[item];
                    if (cunModel.IsDu == cunType && cunModel.ZongSheBeiId == zongid)
                    {
                        return cunModel;
                    }

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

     


        public SaoMaModel GetSheBeiModel(CunModel model)
        {
            if (model.IsDu != CunType.KaiShiPing)
            {
                for (int i = 0; i < LisSheBei.Count; i++)
                {
                    if (LisSheBei[i].SheBeiID == model.ZongSheBeiId)
                    {
                        return LisSheBei[i];
                    }
                }
            }
            return null;
        }


        public SaoMaModel GetSheBeiModel(int shebeid)
        {
            for (int i = 0; i < LisSheBei.Count; i++)
            {
                if (LisSheBei[i].SheBeiID == shebeid)
                {
                    return LisSheBei[i];
                }
            }
            return null;
        }
    }
}
