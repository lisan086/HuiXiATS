using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModBuTCP.Model;
using SSheBei.Model;
using CommLei.DataChuLi;
using CommLei.JiChuLei;
using SSheBei.LianJieQi;
using SSheBei.CRCJiaoYan;

namespace ModBuTCP.Frm
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
        /// 读写寄存器
        /// </summary>
        public List<JiCunQiModel> LisDuXie = new List<JiCunQiModel>();
        /// <summary>
        /// 设备
        /// </summary>
        public List<SheBeiModel> LisSheBei = new List<SheBeiModel>();


        /// <summary>
        /// 写标识的对应 key表示设备id
        /// </summary>
        public Dictionary<string, DataCunModel> JiLu = new Dictionary<string, DataCunModel>();

      

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
            LisSheBei = JosnOrSModel.GetLisTModel<SheBeiModel>();
            if (LisSheBei == null)
            {
                LisSheBei = new List<SheBeiModel>();
            }
           
            for (int c = 0; c < LisSheBei.Count; c++)
            {
                SheBeiModel shebei = LisSheBei[c];
                                
                FromSortZX(shebei.DataCunModels, true);             
                for (int i = 0; i < shebei.DataCunModels.Count; i++)
                {
                    JiCunQiModel jicunmodel = ShengChengModel(shebei.DataCunModels[i]);
                    jicunmodel.DuXie = 3;
                    jicunmodel.Value = 0;
                    DataCunModel sdmodel = shebei.DataCunModels[i];
                    sdmodel.JiCunQiModel = jicunmodel;
                    LisXie.Add(jicunmodel);
                    LisDuXie.Add(jicunmodel);
                    LisDu.Add(jicunmodel);
                    if (JiLu.ContainsKey(jicunmodel.WeiYiBiaoShi) == false)
                    {
                        JiLu.Add(jicunmodel.WeiYiBiaoShi, sdmodel);
                    }
                }
           
            }
            KeyS = JiLu.Keys.ToList();
        }

        public void SetTx(int zongid,bool tx)
        {
            for (int i = 0; i < LisSheBei.Count; i++)
            {
                if (LisSheBei[i].SheBeiID == zongid)
                {
                    SheBeiModel cunModel = LisSheBei[i];
                    cunModel.Tx = tx;
                    for (int c = 0; c < cunModel.DataCunModels.Count; c++)
                    {
                        cunModel.DataCunModels[c].JiCunQiModel.IsKeKao = tx;
                    }
                    break;
                }
            }

        
        }

   

        public DataCunModel IsChengGong(JiCunQiModel model)
        {
          
            if (LisSheBei.Count>0)
            {
                for (int i = 0; i < LisSheBei.Count; i++)
                {
                    List<DataCunModel> items= LisSheBei[i].DataCunModels;
                    for (int ic = 0; ic < items.Count; ic++)
                    {
                        if (items[ic].JiCunQiModel.WeiYiBiaoShi == model.WeiYiBiaoShi)
                        {
                            return items[ic];
                        }
                    }
                    
                }
            }
       

            return null;
        }

   

        /// <summary>
        ///  从小到大排序
        /// </summary>
        /// <param name="lisObj">集合</param>
        /// <param name="IsSort">为true表示从小到大，为false则是从大到小</param>
        private void FromSortZX(List<DataCunModel> lisObj, bool IsSort)
        {
            if (lisObj.Count > 0)
            {
                try
                {
                    DataCunModel obj = null;
                    for (int i = 0; i < lisObj.Count; i++)
                    {
                        for (int j = i + 1; j < lisObj.Count; j++)
                        {
                            if (IsSort)
                            {
                                if (lisObj[i].JiCunDiZhi > lisObj[j].JiCunDiZhi)
                                {
                                    obj = lisObj[i];
                                    lisObj[i] = lisObj[j];
                                    lisObj[j] = obj;

                                }
                            }
                            else
                            {
                                if (lisObj[i].JiCunDiZhi < lisObj[j].JiCunDiZhi)
                                {
                                    obj = lisObj[i];
                                    lisObj[i] = lisObj[j];
                                    lisObj[j] = obj;

                                }
                            }
                        }
                    }
                }
                catch
                {


                }

            }
        }



        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isfuzhi"></param>
        /// <returns></returns>
        public void GetModel(JiCunQiModel name)
        {
            for (int i = 0; i < LisSheBei.Count; i++)
            {
                for (int c = 0; c < LisSheBei[i].DataCunModels.Count; c++)
                {
                    if (LisSheBei[i].DataCunModels[c].JiCunQiModel.WeiYiBiaoShi.Equals(name.WeiYiBiaoShi))
                    {
                        LisSheBei[i].DataCunModels[c].JiCunQiModel.Value = name.Value;
                    }
                }
             
            }
            
        }

    
        private JiCunQiModel ShengChengModel(DataCunModel model)
        {
            JiCunQiModel qiModel = new JiCunQiModel();
            qiModel.WeiYiBiaoShi = model.Name;
            qiModel.SheBeiID = SheBeiID;
            qiModel.MiaoSu = model.YingYongType == YingYongType.DuPuTong ? "读寄存器" : model.YingYongType == YingYongType.XiePuTong ? "写寄存器" : "读写寄存器";
            return qiModel;
        }
    }
}
