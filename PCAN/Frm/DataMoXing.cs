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
                        JiCunQiModel model = new JiCunQiModel();
                        model.SheBeiID = SheBeiID;
                        
                        if (lis[i].ToLower().StartsWith("du"))
                        {                         
                            model.WeiYiBiaoShi = $"{shebei.Name}{lis[i]}";
                            model.MiaoSu = ChangYong.GetEnumDescription(ChangYong.GetMeiJuZhi<CunType>(lis[i]));
                            model.DuXie = 1;
                            LisDu.Add(model);
                        }
                        else
                        {                        
                            model.WeiYiBiaoShi =$"{shebei.Name}{lis[i]}";                     
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
                            cunModel.ChaoTime = shebei.ChaoShiTime;
                            JiLu.Add(model.WeiYiBiaoShi, cunModel);
                        }
                    }
                   
                }
            }
            KeyS = JiLu.Keys.ToList();
        }

   
        public void SetHeGe(int zongid,bool ishege)
        {
        
            for (int i = 0; i < LisSheBei.Count; i++)
            {
                if (LisSheBei[i].SheBeiID == zongid)
                {
                    LisSheBei[i].TX = ishege;
                    foreach (var item in KeyS)
                    {
                        CunModel cunModel = JiLu[item];
                        if (cunModel.ZongSheBeiId == zongid)
                        {
                            cunModel.JiCunQi.IsKeKao = ishege;
                        }

                    }
                    break;
                }
            }
        }

        public void SetJiCunQiValue(string weiyibiaoshi,string shuju)
        {
            if (JiLu.ContainsKey(weiyibiaoshi))
            {
                CunModel cunModel = JiLu[weiyibiaoshi];
                cunModel.JiCunQi.IsKeKao = true;
                JiLu[weiyibiaoshi].JiCunQi.Value = shuju;
               
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
            }

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

        /// <summary>
        /// 1是成功 0是未测完 3 不存在 其他表示超时
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public CunModel IsChengGong(int zongid,CunType cunType)
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
                CunModel xinmodel = cunModel.FuZhi();
                xinmodel.JiCunQi = model;
                return xinmodel;
            }
            return null;
        }

        public JiCunQiModel GetTiaoShiModel(int zongid,CunType cunType)
        {
            foreach (var item in JiLu.Keys)
            {
                CunModel cunModel = JiLu[item];
                if (cunModel.IsDu == cunType && cunModel.ZongSheBeiId == zongid)
                {
                    return cunModel.JiCunQi.FuZhi();
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

    }
}
