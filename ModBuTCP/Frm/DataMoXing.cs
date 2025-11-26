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
        public Dictionary<int, List<JiLuModel>> JiLu = new Dictionary<int, List<JiLuModel>>();

        public Dictionary<int, List<DataCunModel>> LisTeShuBoolDu = new Dictionary<int, List<DataCunModel>>();

        private List<int> KeyS = new List<int>();

        /// <summary>
        /// 用于初始化
        /// </summary>
        public void IniData(string lujing)
        {
            LisDu.Clear();
            LisXie.Clear();
            JiLu.Clear();
            LisDuXie.Clear();
            LisTeShuBoolDu.Clear();
            JosnOrSModel JosnOrSModel = new JosnOrSModel(lujing);
            LisSheBei = JosnOrSModel.GetLisTModel<SheBeiModel>();
            if (LisSheBei == null)
            {
                LisSheBei = new List<SheBeiModel>();
            }
            for (int c = 0; c < LisSheBei.Count; c++)
            {
                SheBeiModel shebei = LisSheBei[c];
                SheBeiModel xinshebei = ChangYong.FuZhiShiTi(shebei);                          
                FromSortZX(xinshebei.DataCunModels, true);
                int shuliang = 80;
                int index = 0;
                Dictionary<int, List<DataCunModel>> paixu = new Dictionary<int, List<DataCunModel>>();
                for (int i = 0; i < xinshebei.DataCunModels.Count; i++)
                {
                    if (xinshebei.DataCunModels[i].YingYongType == YingYongType.XiePuTong)
                    {
                        JiCunQiModel jicunmodel = ShengChengModel(xinshebei.DataCunModels[i]);
                        jicunmodel.DuXie = 2;
                        DataCunModel sdmodel = GetModel(xinshebei.DataCunModels[i].Name, false);
                        sdmodel.JiCunQiModel = jicunmodel;
                        LisXie.Add(jicunmodel);
                        LisDuXie.Add(jicunmodel);
                        continue;
                    }
                    if (xinshebei.DataCunModels[i].DataType==DataType.TeShuBool)
                    {
                        JiCunQiModel jicunmodel = ShengChengModel(xinshebei.DataCunModels[i]);
                        jicunmodel.DuXie = 2;
                        if (xinshebei.DataCunModels[i].YingYongType==YingYongType.DuXieYiQi)
                        {
                            jicunmodel.DuXie = 3;
                        }                    
                        DataCunModel sdmodel = GetModel(xinshebei.DataCunModels[i].Name, false);
                        sdmodel.JiCunQiModel = jicunmodel;
                        LisDu.Add(jicunmodel);
                        LisDuXie.Add(jicunmodel);
                        if (LisTeShuBoolDu.ContainsKey(shebei.SheBeiID)==false)
                        {
                            LisTeShuBoolDu.Add(shebei.SheBeiID,new List<DataCunModel>());
                        }
                        LisTeShuBoolDu[shebei.SheBeiID].Add(sdmodel);
                        continue;
                    } 
                    int pianyi = xinshebei.DataCunModels[i].JiCunDiZhi+ xinshebei.DataCunModels[i].Count;
                    int zuixiaopianyi = xinshebei.DataCunModels[index].JiCunDiZhi;
                    if (pianyi - zuixiaopianyi <= shuliang)
                    {
                        if (paixu.ContainsKey(zuixiaopianyi) == false)
                        {
                            paixu.Add(zuixiaopianyi, new List<DataCunModel>());

                        }
                        paixu[zuixiaopianyi].Add(xinshebei.DataCunModels[i]);

                    }
                    else
                    {
                        index = i;
                        zuixiaopianyi = xinshebei.DataCunModels[index].JiCunDiZhi;
                        if (paixu.ContainsKey(zuixiaopianyi) == false)
                        {
                            paixu.Add(zuixiaopianyi, new List<DataCunModel>());

                        }
                        paixu[zuixiaopianyi].Add(xinshebei.DataCunModels[i]);

                    }

                }
                foreach (var item in paixu.Keys)
                {
                    JiLuModel models = new JiLuModel();
                    models.JiCunQiZuiXiaoPianYi = item;
                    models.SheBeiID = shebei.SheBeiID;

                    int zongcount = paixu[item].Count;
                    int count = paixu[item][zongcount - 1].JiCunDiZhi + paixu[item][zongcount - 1].Count - paixu[item][0].JiCunDiZhi;
                    models.ShuJu = GetDuBaoWen(item, count);
                    models.Count = count;
                    for (int i = 0; i < paixu[item].Count; i++)
                    {
                        DataCunModel sdmodel = GetModel(paixu[item][i].Name, false);
                        JiCunQiModel jicunmodel = ShengChengModel(sdmodel);
                        jicunmodel.DuXie = 1;
                        sdmodel.JiCunQiModel = jicunmodel;
                      
                        if (sdmodel.YingYongType==YingYongType.DuXieYiQi)
                        {
                            jicunmodel.DuXie = 3;
                            LisXie.Add(jicunmodel);
                        }
                        LisDu.Add(jicunmodel);
                        LisDuXie.Add(jicunmodel);
                        models.Lis.Add(sdmodel);
                    }
                    if (JiLu.ContainsKey(shebei.SheBeiID)==false)
                    {
                        JiLu.Add(shebei.SheBeiID,new List<JiLuModel>());
                    }
                    JiLu[shebei.SheBeiID].Add(models);
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

        public void SetJiCunQiValue(int zongid, List<byte> shuju,JiLuModel jiLuModel)
        {
            for (int c = 0; c < jiLuModel.Lis.Count; c++)
            {
                DataCunModel cunmodel = jiLuModel.Lis[c];
                int zhengshu = cunmodel.JiCunDiZhi;
                int yushu = (zhengshu - jiLuModel.JiCunQiZuiXiaoPianYi) * 2;
                int zhencount = cunmodel.Count * 2;
                List<byte> geshu = new List<byte>();
                if (cunmodel.IsXieWan != 1)
                {
                    cunmodel.IsXieWan = 1;
                }
                for (int i = yushu; i < yushu + zhencount; i++)
                {
                    geshu.Add(shuju[3 + i]);
                }
                cunmodel.JiCunQiModel.IsKeKao = true;
                
                switch (cunmodel.DataType)
                {
                    case DataType.Int:
                        {
                            List<byte> bianhuan = CRC.JiaoHuanByte(geshu);
                            if (cunmodel.XiaoShuWei <= 0)
                            {
                                cunmodel.XiaoShuWei = 0;
                            }
                            if (cunmodel.XiaoShuWei>=15)
                            {
                                cunmodel.XiaoShuWei = 14;
                            }
                            if (cunmodel.BeiChuShu == 0)
                            {
                                cunmodel.BeiChuShu = 1;
                            }
                            double zhi = CRC.GetInt(bianhuan, 0);
                            cunmodel.JiCunQiModel.Value = Math.Round((zhi / cunmodel.BeiChuShu)+cunmodel.BZhi, cunmodel.XiaoShuWei);
                        }
                        break;
                    case DataType.Float:
                        {
                            List<byte> bianhuan = CRC.JiaoHuanByte(geshu);
                            if (cunmodel.XiaoShuWei <= 0)
                            {
                                cunmodel.XiaoShuWei = 0;
                            }
                            if (cunmodel.XiaoShuWei >= 15)
                            {
                                cunmodel.XiaoShuWei = 14;
                            }
                            if (cunmodel.BeiChuShu == 0)
                            {
                                cunmodel.BeiChuShu = 1;
                            }
                            double zhi = BitConverter.ToSingle(bianhuan.ToArray(), 0);
                            cunmodel.JiCunQiModel.Value = Math.Round((zhi / cunmodel.BeiChuShu)+cunmodel.BZhi, cunmodel.XiaoShuWei);
                        }
                        break;
                    case DataType.String:
                        {
                            string dshuju = "";
                            if (geshu.Count > 2)
                            {
                                int count = geshu[1];
                                try
                                {
                                    byte[] shujdu = CRC.JiaoHuanByte(geshu).ToArray();
                                    dshuju = Encoding.ASCII.GetString(shujdu, 2, count);
                                }
                                catch
                                {


                                }
                            }
                            cunmodel.JiCunQiModel.Value = dshuju.Replace("\0", "").Trim();
                         
                        }
                        break;
                    case DataType.String16OrACII:
                        {
                            
                              string zhuanhuan = Encoding.ASCII.GetString(CRC.JiaoHuanByte(geshu).ToArray());
                            if (string.IsNullOrEmpty(cunmodel.FenGeFu)==false)
                            {
                                zhuanhuan = zhuanhuan.Split(new string[] { cunmodel.FenGeFu },StringSplitOptions.None)[0];
                                if (string.IsNullOrEmpty(zhuanhuan))
                                {
                                    zhuanhuan = "";
                                }
                            }
                            // string zhuanhuan = Encoding.Unicode.GetString((geshu).ToArray());
                            cunmodel.JiCunQiModel.Value = zhuanhuan.Replace("\0", "").Trim();
                        }
                        break;
                    case DataType.Bool:
                        {
                            if (cunmodel.ZiDiZhi>=0)
                            {
                                List<int> zhuans = ChangYong.Get10Or2(BitConverter.ToInt16(CRC.JiaoHuanByte(geshu).ToArray(), 0), 16);
                                if (cunmodel.ZiDiZhi < zhuans.Count)
                                {
                                    cunmodel.JiCunQiModel.Value = zhuans[cunmodel.ZiDiZhi];
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
               
                
            }
           
        }

        public void SetJiCunQiValue(DataCunModel model, List<byte> shuju)
        {
            if (model.DataType == DataType.TeShuBool)
            {
                if (shuju.Count >= model.Count + 3)
                {
                    if (model.IsXieWan!=1)
                    {
                        model.IsXieWan = 1;
                    }
                    model.JiCunQiModel.IsKeKao = true;
                    if (shuju[3] == 0x01)
                    {
                        model.JiCunQiModel.Value = 1;
                    }
                    else
                    {
                        model.JiCunQiModel.Value = 0;
                    }
                }
            }
          

        }

        public void SetZhengTaiState(int zongid,string name,int state)
        {
            if (JiLu.ContainsKey(zongid))
            {
                for (int c = 0; c < JiLu[zongid].Count; c++)
                {
                    JiLuModel jiLuModel = JiLu[zongid][c];
                    for (int i = 0; i < jiLuModel.Lis.Count; i++)
                    {
                        if (jiLuModel.Lis[i].JiCunQiModel.WeiYiBiaoShi.Equals(name))
                        {
                            jiLuModel.Lis[i].IsXieWan = state;
                            if (state==0)
                            {
                                jiLuModel.Lis[i].JiCunQiModel.Value = "";
                            }
                            return;
                        }
                    }
                 
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

        public List<JiLuModel> GetJiLuSheBei(int zongshebeiid)
        {
            if (JiLu.ContainsKey(zongshebeiid))
            {
                return JiLu[zongshebeiid];
            }
            return new List<JiLuModel>();
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
        /// 获取斑纹
        /// </summary>
        /// <param name="dizhi"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private List<byte> GetDuBaoWen(int zuixiaodizhi, int count)
        {
            List<byte> baowen = new List<byte>();
            baowen.Add(0x00);
            baowen.Add(0x00);
            baowen.Add(0x00);
            baowen.Add(0x00);
            baowen.Add(0x00);
            baowen.Add(0x06);
            baowen.Add((byte)1);
            baowen.Add(0x03);
            List<byte> sju =CRC.ShiOrByte2(zuixiaodizhi, true).ToList();
            baowen.Add(sju[0]);
            baowen.Add(sju[1]);
            baowen.Add(0x00);
            baowen.Add((byte)count);
            return baowen;
        }


        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isfuzhi"></param>
        /// <returns></returns>
        public DataCunModel GetModel(string name, bool isfuzhi)
        {
            for (int i = 0; i < LisSheBei.Count; i++)
            {
                for (int c = 0; c < LisSheBei[i].DataCunModels.Count; c++)
                {
                    if (LisSheBei[i].DataCunModels[c].Name.Equals(name))
                    {
                        if (isfuzhi)
                        {
                            DataCunModel models = ChangYong.FuZhiShiTi(LisSheBei[i].DataCunModels[c]);
                            models.SheBeiID = LisSheBei[i].SheBeiID;
                            return models;
                        }
                        else
                        {
                            LisSheBei[i].DataCunModels[c].SheBeiID= LisSheBei[i].SheBeiID;
                            return LisSheBei[i].DataCunModels[c];
                        }
                    }
                }
             
            }
            return null;
        }

        public List<DataCunModel> GetTeShuBoolDu(int shebeiid)
        {
            if (LisTeShuBoolDu.ContainsKey(shebeiid))
            {
                return LisTeShuBoolDu[shebeiid];
            }
            else
            {
                return new List<DataCunModel>();
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
