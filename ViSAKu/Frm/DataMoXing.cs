using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommLei.JiChuLei;
using SSheBei.CRCJiaoYan;
using System.Windows.Forms;
using SSheBei.Model;
using ViSAKu.Model;
using CommLei.DataChuLi;

namespace ViSAKu.Frm
{
    /// <summary>
    /// 数据模型
    /// </summary>
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
        public List<SheBeiVisaModel> LisSheBei { get; set; } = new List<SheBeiVisaModel>();

        /// <summary>
        /// 写标识的对应 key表示寄存器的唯一表示
        /// </summary>
        public Dictionary<string, DataLieModel> JiLu = new Dictionary<string, DataLieModel>();

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
            LisSheBei = JosnOrSModel.GetLisTModel<SheBeiVisaModel>();
            if (LisSheBei == null)
            {
                LisSheBei = new List<SheBeiVisaModel>();
            }
            for (int c = 0; c < LisSheBei.Count; c++)
            {
                SheBeiVisaModel shebei = LisSheBei[c];
                for (int d = 0; d < shebei.LisData.Count; d++)
                {
                    DataLieModel zishebei = shebei.LisData[d];
                    zishebei.SheBeiID= LisSheBei[c].SheBeiID;
                    zishebei.XieYanShi = LisSheBei[c].XieYanShi;
                    if (zishebei.IsDu.ToString().ToLower().Contains("du"))
                    {
                        JiCunQiModel jicunqimodel = new JiCunQiModel();
                        jicunqimodel.SheBeiID = SheBeiID;
                        jicunqimodel.WeiYiBiaoShi = zishebei.Name;
                        jicunqimodel.DuXie = 1;
                        jicunqimodel.MiaoSu = zishebei.MiaoSu;
                        jicunqimodel.Value = "";
                        LisDu.Add(jicunqimodel);
                        LisDuXie.Add(jicunqimodel);
                        zishebei.JiCunQiModel = jicunqimodel;
                        if (JiLu.ContainsKey(jicunqimodel.WeiYiBiaoShi) == false)
                        {
                            JiLu.Add(jicunqimodel.WeiYiBiaoShi, zishebei);
                        }
                    }
                    else
                    {
                        JiCunQiModel jicunqimodel = new JiCunQiModel();
                        jicunqimodel.SheBeiID = SheBeiID;
                        jicunqimodel.WeiYiBiaoShi = zishebei.Name;
                        jicunqimodel.DuXie = 2;
                        jicunqimodel.MiaoSu = zishebei.MiaoSu;
                        jicunqimodel.Value = "";
                        LisXie.Add(jicunqimodel);
                        LisDuXie.Add(jicunqimodel);
                        zishebei.JiCunQiModel = jicunqimodel;
                        if (JiLu.ContainsKey(jicunqimodel.WeiYiBiaoShi) == false)
                        {
                            JiLu.Add(jicunqimodel.WeiYiBiaoShi, zishebei);
                        }
                    }


                }
              
            }

            KeyS = JiLu.Keys.ToList();
        }

        public void SetTxState(string lianjiename,bool state)
        {
            for (int i = 0; i < LisSheBei.Count; i++)
            {
                if (LisSheBei[i].LianJieName == lianjiename)
                {
                    LisSheBei[i].IsConnect = state;
                    List<DataLieModel> lis = LisSheBei[i].LisData;
                    for (int c = 0; c < lis.Count; c++)
                    {
                        lis[c].JiCunQiModel.IsKeKao = state;
                    }
                    break;
                }
            }
        }

        public void SetXiWan(string weiyiname, int shuju)
        {
            if (JiLu.ContainsKey(weiyiname))
            {
                DataLieModel cunModel = JiLu[weiyiname];
                
                cunModel.IsXieWan = shuju;
            }
            
        }
        public void SetJiCunQiValue(string weiyiname, object shuju)
        {
            int shebeiid = -1;
            for (int i = 0; i < KeyS.Count; i++)
            {
                DataLieModel cunModel = JiLu[KeyS[i]];
                if (cunModel.JiCunQiModel.WeiYiBiaoShi == weiyiname )
                {                  
                    cunModel.JiCunQiModel.IsKeKao = true;
                    cunModel.GetValue(ChangYong.TryStr(shuju,""));
                    shebeiid = cunModel.SheBeiID;
                
                    break;
                }
            }
            for (int i = 0; i < KeyS.Count; i++)
            {
                DataLieModel cunModel = JiLu[KeyS[i]];
                if (cunModel.SheBeiID == shebeiid)
                {
                    cunModel.JiCunQiModel.IsKeKao = true;
                  
                }
            }
            for (int i = 0; i < LisSheBei.Count; i++)
            {
                if (LisSheBei[i].SheBeiID == shebeiid)
                {
                    LisSheBei[i].IsConnect = true;
                    break;
                }
            }
        }

        public DataLieModel GetModel(JiCunQiModel model)
        {
            if (JiLu.ContainsKey(model.WeiYiBiaoShi))
            {
                DataLieModel cunModel = JiLu[model.WeiYiBiaoShi];
                return cunModel;

            }
            return null;
        }
    }
}
