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
using System.Xml.Linq;
using static System.Windows.Forms.AxHost;

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

                List<string> lismeiju = ChangYong.MeiJuLisName(typeof(YingYongType));
             
                for (int i = 0; i < lismeiju.Count; i++)
                {

                    if (lismeiju[i].ToLower().StartsWith("du"))
                    {
                        DataCunModel sdmodel = new DataCunModel();
                        sdmodel.SheBeiID = shebei.SheBeiID;
                        JiCunQiModel jicunmodel = new JiCunQiModel();
                        jicunmodel.DuXie = 1;
                        jicunmodel.SheBeiID = SheBeiID;
                        jicunmodel.Value = 0;
                        jicunmodel.WeiYiBiaoShi = $"{shebei.SheBeiName}-{lismeiju[i]}";
                        sdmodel.JiCunQiModel = jicunmodel;
                        sdmodel.Name = jicunmodel.WeiYiBiaoShi;
                        sdmodel.YingYongType = ChangYong.GetMeiJuZhi<YingYongType>(lismeiju[i]);
                        LisDuXie.Add(jicunmodel);
                        LisDu.Add(jicunmodel);
                        if (JiLu.ContainsKey(jicunmodel.WeiYiBiaoShi)==false)
                        {
                            JiLu.Add(jicunmodel.WeiYiBiaoShi, sdmodel);
                        }
                    }
                    else
                    {
                        DataCunModel sdmodel = new DataCunModel();
                        sdmodel.SheBeiID = shebei.SheBeiID;
                        JiCunQiModel jicunmodel = new JiCunQiModel();
                        jicunmodel.DuXie = 2;
                        jicunmodel.Value = "";
                        jicunmodel.SheBeiID = SheBeiID;
                        jicunmodel.WeiYiBiaoShi = $"{shebei.SheBeiName}-{lismeiju[i]}";
                        sdmodel.JiCunQiModel = jicunmodel;
                        sdmodel.Name = jicunmodel.WeiYiBiaoShi;
                        sdmodel.YingYongType = ChangYong.GetMeiJuZhi<YingYongType>(lismeiju[i]);
                        LisDuXie.Add(jicunmodel);
                        LisXie.Add(jicunmodel);
                        if (JiLu.ContainsKey(jicunmodel.WeiYiBiaoShi) == false)
                        {
                            JiLu.Add(jicunmodel.WeiYiBiaoShi, sdmodel);
                        }
                    }
                   

                }
           
            }
            KeyS= JiLu.Keys.ToList();
        }

        public void SetTx(int zongid,bool tx)
        {
            for (int i = 0; i < LisSheBei.Count; i++)
            {
                if (LisSheBei[i].SheBeiID == zongid)
                {
                    SheBeiModel cunModel = LisSheBei[i];
                    cunModel.Tx = tx;
                    for (int c = 0; c < KeyS.Count; c++)
                    {
                        if (JiLu[KeyS[c]].SheBeiID== zongid)
                        {
                            JiLu[KeyS[c]].JiCunQiModel.IsKeKao = tx;
                        }
                        
                    }
                    break;
                }
            }

        
        }



        public void SetZhuangTai(DataCunModel cunModel, int state)
        {
            if (JiLu.ContainsKey(cunModel.JiCunQiModel.WeiYiBiaoShi))
            {
                DataCunModel dataCunModel = JiLu[cunModel.JiCunQiModel.WeiYiBiaoShi];
                if (state == 0)
                {
                    for (int i = 0; i < KeyS.Count; i++)
                    {
                        if (JiLu[KeyS[i]].SheBeiID== dataCunModel.SheBeiID)
                        {
                            JiLu[KeyS[i]].JiCunQiModel.Value = "";
                        }
                    }
                    dataCunModel.JiCunQiModel.Value = "";
                }
            }

        }

        public void SetZhi(DataCunModel cunModel,object zhi)
        {
            if (JiLu.ContainsKey(cunModel.JiCunQiModel.WeiYiBiaoShi))
            {
                DataCunModel dataCunModel = JiLu[cunModel.JiCunQiModel.WeiYiBiaoShi];
                dataCunModel.JiCunQiModel.Value = zhi;
            }
        }
        public void SetZhi(DataCunModel cunModel, byte[] zhi)
        {
            if (JiLu.ContainsKey(cunModel.JiCunQiModel.WeiYiBiaoShi))
            {
                DataCunModel dataCunModel = JiLu[cunModel.JiCunQiModel.WeiYiBiaoShi];
                if (dataCunModel.YingYongType == YingYongType.DuQingQiuASCII)
                {
                    dataCunModel.JiCunQiModel.Value = Encoding.ASCII.GetString(zhi);
                }
            }
        }
     


        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isfuzhi"></param>
        /// <returns></returns>
        public DataCunModel GetModel(JiCunQiModel name,bool isfuzhi)
        {
            if (JiLu.ContainsKey(name.WeiYiBiaoShi))
            {
                DataCunModel dataCunModel = JiLu[name.WeiYiBiaoShi];
                if (isfuzhi)
                {
                    DataCunModel xinmodel = dataCunModel.FuZhi();
                    xinmodel.JiCunQiModel = name;
                    return xinmodel;
                }
                else
                {
                    return dataCunModel;
                }
            }
         
            return null;
            
        }


        public List<DataCunModel> GetCunModels(int shebeiid)
        {
            List<DataCunModel> lis = new List<DataCunModel>();
            for (int i = 0; i < KeyS.Count; i++)
            {
                if (JiLu[KeyS[i]].SheBeiID== shebeiid)
                {
                    lis.Add(JiLu[KeyS[i]]);
                }
            }
            return lis;
        }
    }
}
