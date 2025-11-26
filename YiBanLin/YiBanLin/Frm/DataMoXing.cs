using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommLei.JiChuLei;
using SSheBei.CRCJiaoYan;
using SSheBei.Model;
using YiBanLin.Model;
using CommLei.DataChuLi;
using System.Windows.Forms;


namespace YiBanLin.Frm
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
        public List<LinModel> LisSheBei = new List<LinModel>();

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
            LisSheBei = JosnOrSModel.GetLisTModel<LinModel>();
            if (LisSheBei == null)
            {
                LisSheBei = new List<LinModel>();
            }
          
            for (int c = 0; c < LisSheBei.Count; c++)
            {
                LinModel shebei = LisSheBei[c];
                {
                    List<string> lis = ChangYong.MeiJuLisName(typeof(CunType));
                    for (int i = 0; i < lis.Count; i++)
                    {
                        CunType cunType = ChangYong.GetMeiJuZhi<CunType>(lis[i]);
                        JiCunQiModel model = new JiCunQiModel();
                        model.SheBeiID = SheBeiID;
                     
                        if (lis[i].ToLower().Contains("du"))
                        {
                          
                            model.WeiYiBiaoShi = $"{shebei.Name}:{lis[i]}";
                         
                            model.MiaoSu = ChangYong.GetEnumDescription(cunType);
                            model.DuXie = 1;
                            LisDu.Add(model);
                        }
                        else
                        {                        
                            model.WeiYiBiaoShi =$"{shebei.Name}:{lis[i]}";                          
                            model.MiaoSu = ChangYong.GetEnumDescription(cunType);
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
                           
                            JiLu.Add(model.WeiYiBiaoShi, cunModel);
                        }
                        
                    }
                   
                }
            }
            KeyS = JiLu.Keys.ToList();
        }


        public void SetTX(int zongid,bool tx)
        {
            for (int i = 0; i < LisSheBei.Count; i++)
            {
                if (LisSheBei[i].SheBeiID == zongid)
                {
                    LisSheBei[i].Tx=tx;
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
        public void SetZhengZaiValue(CunModel model,int sate)
        {
            if (JiLu.ContainsKey(model.JiCunQi.WeiYiBiaoShi))
            {
                JiLu[model.JiCunQi.WeiYiBiaoShi].IsZhengZaiCe = sate;
               
            }

        }

       

        public CunModel GetModel(JiCunQiModel model)
        {
            if (JiLu.ContainsKey(model.WeiYiBiaoShi))
            {
                CunModel cunModel = JiLu[model.WeiYiBiaoShi].FuZhi();
                cunModel.JiCunQi.Value = model.Value;
                return ChangYong.FuZhiShiTi(cunModel);
            }
            return null;
        }

        public CunModel GetModel(int zongid,CunType cunType)
        {
            foreach (var item in JiLu.Keys)
            {
                CunModel cunModel = JiLu[item];
                if (cunModel.IsDu == cunType && cunModel.ZongSheBeiId == zongid)
                {
                    return cunModel.FuZhi();
                }

            }

            return null;
        }


        public LinModel GetSheBeiModel(CunModel model)
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

        public LinModel GetSheBeiModel(int shebeiid)
        {
            for (int i = 0; i < LisSheBei.Count; i++)
            {
                if (LisSheBei[i].SheBeiID == shebeiid)
                {
                    return LisSheBei[i];
                }
            }
            return null;
        }
      
    }
}
