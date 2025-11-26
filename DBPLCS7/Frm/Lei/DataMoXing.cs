using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommLei.JiChuLei;
using DBPLCS7.Model;
using SSheBei.Model;
using CommLei.DataChuLi;
using S7.Net;
using BaseUI.UC;
using System.Reflection;

namespace DBPLCS7.Frm.Lei
{
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

        public List<JiCunQiModel> LisDuXie = new List<JiCunQiModel>();

        /// <summary>
        /// 设备
        /// </summary>
        public List<PLCShBeiModel> LisSheBei = new List<PLCShBeiModel>();

        /// <summary>
        /// 记录DB快的最小寄存器
        /// </summary>
        public Dictionary<int, List<JiLvDaXiaoModel>> JiLuDBKuaiZuiXiao = new Dictionary<int, List<JiLvDaXiaoModel>>();

        /// <summary>
        /// 用于初始化
        /// </summary>
        public void IniData(string lujing)
        {
            LisDu.Clear();
            LisXie.Clear();
            JiLuDBKuaiZuiXiao.Clear();
            JosnOrSModel JosnOrSModel = new JosnOrSModel(lujing);
            LisSheBei = JosnOrSModel.GetLisTModel<PLCShBeiModel>();
            if (LisSheBei == null)
            {
                LisSheBei = new List<PLCShBeiModel>();

            }
        
            for (int c = 0; c < LisSheBei.Count; c++)
            {
                PLCShBeiModel shebei = LisSheBei[c];
                for (int i = 0; i < shebei.JiCunQi.Count; i++)
                {
                    PLCJiCunQiModel plcjicunqi = shebei.JiCunQi[i];
                    JiCunQiModel jiCunQiModel = new JiCunQiModel();
                    jiCunQiModel.MiaoSu = ChangYong.GetEnumDescription(plcjicunqi.PLCDataType);
                    plcjicunqi.SheBeiID = LisSheBei[c].Port;
                    plcjicunqi.JiCunQiModel = new JiCunQiModel();
                    plcjicunqi.JiCunQiModel.WeiYiBiaoShi = LisSheBei[c].JiCunQi[i].Name;
                    plcjicunqi.JiCunQiModel.SheBeiID = SheBeiID;
                    int biaozhi = 3;
                    if (plcjicunqi.GongNengType.ToString().Contains("Du"))
                    {
                        biaozhi = 1;
                       
                     
                    }
                    if (plcjicunqi.GongNengType.ToString().Contains("Xie"))
                    {
                        biaozhi = 2;
                                             
                    }
                    if (plcjicunqi.GongNengType.ToString().Contains("Du")&& plcjicunqi.GongNengType.ToString().Contains("Xie"))
                    {
                        biaozhi = 3;
                    }
                    plcjicunqi.JiCunQiModel.DuXie = biaozhi;
                    if (biaozhi == 1)
                    {
                        plcjicunqi.IsXieWan = 1;
                        LisDu.Add(plcjicunqi.JiCunQiModel);
                    }
                    else if (biaozhi == 2)
                    {
                        LisXie.Add(plcjicunqi.JiCunQiModel);
                    }
                    else
                    {
                        plcjicunqi.IsXieWan = 1;
                        LisDu.Add(plcjicunqi.JiCunQiModel);
                        LisXie.Add(plcjicunqi.JiCunQiModel);
                    }
                    LisDuXie.Add(plcjicunqi.JiCunQiModel);
                }
                List<JiLvDaXiaoModel> jilu= FenPeiShuJu(shebei.JiCunQi);
                if (JiLuDBKuaiZuiXiao.ContainsKey(shebei.Port)==false)
                {
                    JiLuDBKuaiZuiXiao.Add(shebei.Port, jilu);
                }
            }
         
        }

        /// <summary>
        /// 分配读的数据
        /// </summary>
        /// <param name="lis"></param>
        private List<JiLvDaXiaoModel> FenPeiShuJu(List<PLCJiCunQiModel> xinlis)
        {
            List<JiLvDaXiaoModel> jilus = new List<JiLvDaXiaoModel>();
            List<PLCJiCunQiModel> lis =ChangYong.FuZhiShiTi( xinlis);          
           
            Dictionary<int, List<PLCJiCunQiModel>> cunchu = new Dictionary<int, List<PLCJiCunQiModel>>();
            for (int i = 0; i < lis.Count; i++)
            {
                if (lis[i].GongNengType.ToString().Contains("Du"))
                {

                    if (lis[i].XinHaoType == XinHaoType.Memory)
                    {
                        lis[i].DBKuan = -4;
                    }
                    else if (lis[i].XinHaoType == XinHaoType.OutPut)
                    {
                        lis[i].DBKuan = -3;
                    }
                    else if (lis[i].XinHaoType == XinHaoType.InPut)
                    {
                        lis[i].DBKuan = -2;
                    }
                    if (cunchu.ContainsKey(lis[i].DBKuan) == false)
                    {
                        cunchu.Add(lis[i].DBKuan, new List<PLCJiCunQiModel>());
                    }
                    cunchu[lis[i].DBKuan].Add(lis[i]);
                }
            }
            foreach (var item in cunchu.Keys)
            {
                FromSortZX(cunchu[item], true);
                List<PLCJiCunQiModel> pLCs = cunchu[item];
                if (pLCs.Count > 0)
                {
                  
                    int count = pLCs.Count - 1;
                    int zongcount = (pLCs[count].PianYiLiang + pLCs[count].GetCount()) - (pLCs[0].PianYiLiang);
                    JiLvDaXiaoModel model = new JiLvDaXiaoModel();
                    model.JiCunQiZuiXiaoPianYi = pLCs[0].PianYiLiang;
                    model.JiCunQiDuShuLiang = zongcount;
                    model.DBKuai = item;
                   
                    model.DataType = item == -2 ? DataType.Input : item == -3 ? DataType.Output : item == -4 ? DataType.Memory : DataType.DataBlock;
                    for (int i = 0; i < pLCs.Count; i++)
                    {
                        PLCJiCunQiModel mosd = GetXinLis(pLCs[i].Name, xinlis);
                        if (mosd != null)
                        {
                            model.BangDianJiCunQi.Add(mosd);
                        }
                    }

                    jilus.Add(model);
                }
            }
            return jilus;

        }

        public PLCJiCunQiModel GetPLCDian(JiCunQiModel mdoel,int shebid=0,int type=0)
        {
            if (type == 0)
            {
                for (int c = 0; c < LisSheBei.Count; c++)
                {
                    foreach (var item in LisSheBei[c].JiCunQi)
                    {
                        if (item.Name.Equals(mdoel.WeiYiBiaoShi))
                        {
                            return item.FuZhi();
                        }
                    }

                }
            }
            else if (type==1)
            {
                
                for (int c = 0; c < LisSheBei.Count; c++)
                {
                    if (LisSheBei[c].Port== shebid)
                    {
                        foreach (var item in LisSheBei[c].JiCunQi)
                        {
                            if (item.GongNengType== GongNengType.DuXinTiao)
                            {
                                return item.FuZhi();
                            }
                        }
                        break;
                    }

                }
            }
            else if (type == 2)
            {

                for (int c = 0; c < LisSheBei.Count; c++)
                {
                    if (LisSheBei[c].Port == shebid)
                    {
                        foreach (var item in LisSheBei[c].JiCunQi)
                        {
                            if (item.GongNengType == GongNengType.XieXinTiao)
                            {
                                return item.FuZhi();
                            }
                        }
                        break;
                    }

                }
            }
          
            return null;
        }


        public void SetZhuangTaiZhi(JiCunQiModel mdoel,int zhi)
        {
            for (int c = 0; c < LisSheBei.Count; c++)
            {
                foreach (var item in LisSheBei[c].JiCunQi)
                {
                    if (item.JiCunQiModel.WeiYiBiaoShi.Equals(mdoel.WeiYiBiaoShi))
                    {
                        item.IsXieWan = zhi;
                        if (zhi==0)
                        {
                            item.JiCunQiModel.Value = "";
                        }
                        return;
                    }
                }

            }
        }

        public void SetTx(int shebeiid,bool tx)
        {
            for (int i = 0; i < LisSheBei.Count; i++)
            {
                if (LisSheBei[i].Port==shebeiid)
                {
                    LisSheBei[i].Tx = tx;
                    break;
                }
            }
            if (JiLuDBKuaiZuiXiao.ContainsKey(shebeiid))
            {
                List<JiLvDaXiaoModel> jis = JiLuDBKuaiZuiXiao[shebeiid];

                for (int i = 0; i < jis.Count; i++)
                {
                    jis[i].BangDianJiCunQi.ForEach(item => { item.JiCunQiModel.IsKeKao = tx; });
                }
            }
        }
        /// <summary>
        ///  从小到大排序
        /// </summary>
        /// <param name="lisObj">集合</param>
        /// <param name="IsSort">为true表示从小到大，为false则是从大到小</param>
        private void FromSortZX(List<PLCJiCunQiModel> lisObj, bool IsSort)
        {
            if (lisObj.Count > 0)
            {
                try
                {
                    PLCJiCunQiModel obj = null;
                    for (int i = 0; i < lisObj.Count; i++)
                    {
                        for (int j = i + 1; j < lisObj.Count; j++)
                        {
                            if (IsSort)
                            {
                                if (lisObj[i].PianYiLiang > lisObj[j].PianYiLiang)
                                {
                                    obj = lisObj[i];
                                    lisObj[i] = lisObj[j];
                                    lisObj[j] = obj;

                                }
                            }
                            else
                            {
                                if (lisObj[i].PianYiLiang < lisObj[j].PianYiLiang)
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

        private PLCJiCunQiModel GetXinLis(string name,List<PLCJiCunQiModel> lis)
        {
            for (int i = 0; i < lis.Count; i++)
            {
                if (lis[i].Name.Equals(name))
                {
                    return lis[i];
                }
            }
            return null;
        }
    }
}
