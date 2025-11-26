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
using static System.Windows.Forms.AxHost;


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

        public Dictionary<int, string> ShangYiCiMa = new Dictionary<int, string>();

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
                            LisDuXie.Add(model);
                        }
                        else
                        {
                        
                            model.WeiYiBiaoShi =$"{shebei.Name}:{lis[i]}";
                           
                            model.MiaoSu = ChangYong.GetEnumDescription(ChangYong.GetMeiJuZhi<CunType>(lis[i]));
                            model.DuXie = 2;
                            LisXie.Add(model);
                            LisDuXie.Add(model);
                        }
                        if (JiLu.ContainsKey(model.WeiYiBiaoShi) == false)
                        {
                            CunModel cunModel = new CunModel();
                            cunModel.ZongSheBeiId = shebei.SheBeiID;
                            cunModel.IsDu = ChangYong.GetMeiJuZhi<CunType>(lis[i]);
                            cunModel.JiCunQi = model;
                            cunModel.JieXiGeShi = shebei.JieXiType;
                            
                            JiLu.Add(model.WeiYiBiaoShi, cunModel);
                        }
                    }
                   
                }
                if (ShangYiCiMa.ContainsKey(shebei.SheBeiID)==false)
                {
                    ShangYiCiMa.Add(shebei.SheBeiID, "");
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
                    foreach (var item in KeyS)
                    {
                        if (JiLu[item].ZongSheBeiId==zongid)
                        {
                            JiLu[item].JiCunQi.IsKeKao = state;
                        }
                    }
                    break;
                }
            }
        }

        public void SetJiCunQiValue(CunModel xianmodel, string shuju)
        {
            if (JiLu.ContainsKey(xianmodel.JiCunQi.WeiYiBiaoShi))
            {
                CunModel model = JiLu[xianmodel.JiCunQi.WeiYiBiaoShi];
                model.JiCunQi.IsKeKao = true;
                if (model.IsDu == CunType.Xie开启扫码)
                {
                    for (int i = 0; i < KeyS.Count; i++)
                    {
                        CunModel cunModel = JiLu[KeyS[i]];
                        if (cunModel.ZongSheBeiId == model.ZongSheBeiId)
                        {
                            cunModel.JiCunQi.IsKeKao = true;
                            if (cunModel.IsDu == CunType.Du读数据)
                            {
                                cunModel.JiCunQi.Value = shuju;                             
                                break;
                            }
                        }

                    }
                }
                else
                {
                    model.JiCunQi.Value = shuju;
                }

             
            }

        }
        public void SetZhuangTaiZhi(CunModel models,int state)
        {
            if (JiLu.ContainsKey(models.JiCunQi.WeiYiBiaoShi))
            {
                CunModel model = JiLu[models.JiCunQi.WeiYiBiaoShi];           
                model.IsZhengZaiCe = state;
                if (model.IsDu == CunType.Xie开启扫码)
                {
                    if (state == 0)
                    {
                        model.JiCunQi.Value = "";
                    }
                    foreach (var item in KeyS)
                    {
                        if (JiLu[item].ZongSheBeiId == model.ZongSheBeiId && JiLu[item].IsDu == CunType.Du读数据)
                        {
                            JiLu[item].IsZhengZaiCe = state;
                            if (state == 0)
                            {
                                JiLu[item].JiCunQi.Value = "";
                            }
                            break;
                        }
                    }
                }
                else if (model.IsDu == CunType.Xie关闭扫码)
                {
                    if (state == 0)
                    {
                        model.JiCunQi.Value = "";
                        foreach (var item in KeyS)
                        {
                            if (JiLu[item].ZongSheBeiId == model.ZongSheBeiId && JiLu[item].IsDu == CunType.Du读数据)
                            {
                                JiLu[item].IsZhengZaiCe = state;
                                if (state == 0)
                                {
                                    JiLu[item].JiCunQi.Value = "";
                                }
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (state == 0)
                    {
                        model.JiCunQi.Value = "";
                    }
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
                CunModel model = JiLu[weiyibiaoshi];

                if (model.IsDu == CunType.Xie开启扫码)
                {
                    foreach (var item in KeyS)
                    {
                        if (JiLu[item].ZongSheBeiId == model.ZongSheBeiId && JiLu[item].IsDu == CunType.Du读数据)
                        {
                            return JiLu[item];

                        }
                    }
                }

                return model;
            }


            return null;
        }

        public CunModel GetModel(JiCunQiModel model)
        {
            if (JiLu.ContainsKey(model.WeiYiBiaoShi))
            {
                CunModel cunModel = JiLu[model.WeiYiBiaoShi];
                if (cunModel.IsDu != CunType.Du读数据)                   
                {
                    return cunModel.FuZhi();
                }

            }
            return null;
        }
        public CunModel GetCunModel(int zongid, CunType cunType)
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

        public JiCunQiModel GetModel(int zongid,CunType cunType)
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
