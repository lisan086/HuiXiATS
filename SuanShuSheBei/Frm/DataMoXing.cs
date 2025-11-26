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
using SuanShuSheBei.Frm.JiaoBenKuanJia;


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

        private XinJiChengJiaoBen XinJiChengJiaoBen;
        /// <summary>
        /// 用于初始化
        /// </summary>
        public void IniData(string lujing)
        {
            XinJiChengJiaoBen = new XinJiChengJiaoBen();
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
                    List<CunModel> ShuJu = shebei.LisJiaoBen;
                    for (int i = 0; i < ShuJu.Count; i++)
                    {
                        JiCunQiModel model = new JiCunQiModel();
                        model.SheBeiID = SheBeiID;
                        model.WeiYiBiaoShi = $"{shebei.Name}:{ShuJu[i].JiaoBenName}";
                        model.MiaoSu = ShuJu[i].MiaoSu;
                        model.DuXie = 2;
                        LisXie.Add(model);
                        LisDuXie.Add(model);
                        if (JiLu.ContainsKey(model.WeiYiBiaoShi) == false)
                        {
                            CunModel cunModel = ShuJu[i];
                            cunModel.ZongSheBeiId = shebei.SheBeiID;
                            cunModel.IsDu = CunType.XieJiaoBen;
                            cunModel.JiCunQi = model;
                            cunModel.Time = shebei.Time;
                            JiLu.Add(model.WeiYiBiaoShi, cunModel);
                            Task.Factory.StartNew(() => {
                                XinJiChengJiaoBen.SetDuiXiang(model.WeiYiBiaoShi, cunModel.JiaoBenNeiRong);
                            });
                        }
                    }
                }
                {
                    List<string> lis = ChangYong.MeiJuLisName(typeof(CunType));
                    for (int i = 0; i < lis.Count; i++)
                    {
                        if (lis[i].Contains("XieJiaoBen"))
                        {
                            continue;
                        }
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

                            JiLu.Add(model.WeiYiBiaoShi, cunModel);
                            shebei.LisJiaoBen.Add(cunModel);
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

      
        public void SetJiCunQiValue(CunModel model, object shuju)
        {
            if (JiLu.ContainsKey(model.JiCunQi.WeiYiBiaoShi))
            {
                CunModel cunModel = JiLu[model.JiCunQi.WeiYiBiaoShi];
                cunModel.JiCunQi.IsKeKao = true;
                cunModel.JiCunQi.Value = shuju;
            }

        }
        public void SetZhengZaiValue(CunModel weiyibiaoshi,int sate)
        {
            if (JiLu.ContainsKey(weiyibiaoshi.JiCunQi.WeiYiBiaoShi))
            {
                CunModel cunModel = JiLu[weiyibiaoshi.JiCunQi.WeiYiBiaoShi];
                cunModel.JiCunQi.IsKeKao = true;
                cunModel.IsZhengZaiCe = sate;
                if (cunModel.IsZhengZaiCe==0)
                {
                    if (cunModel.IsDu==CunType.MoLiTuPian)
                    {
                        for (int i = 0; i < KeyS.Count; i++)
                        {
                            if (JiLu[KeyS[i]].IsDu==CunType.DuShuJu&& JiLu[KeyS[i]].ZongSheBeiId==cunModel.ZongSheBeiId)
                            {
                                JiLu[KeyS[i]].JiCunQi.Value = 0;
                                break;
                            }
                        }
                    }
                }
            }

        }

        public void SetJiCunQiValue(int shebeiid,int zhuangtai)
        {
            for (int i = 0; i < KeyS.Count; i++)
            {
                if (JiLu[KeyS[i]].IsDu == CunType.DuShuJu && JiLu[KeyS[i]].ZongSheBeiId == shebeiid)
                {
                    JiLu[KeyS[i]].JiCunQi.Value = zhuangtai;
                    break;
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


        public object ZhiXingJiaoBen(CunModel model, string canshu, out bool ishege)
        {
            object jieguo = XinJiChengJiaoBen.ShuChuYuJuJieGuo(model.JiCunQi.WeiYiBiaoShi, canshu,out ishege);
            return jieguo;
        }
      
    }
}
