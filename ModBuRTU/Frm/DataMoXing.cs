using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommLei.JiChuLei;
using SSheBei.CRCJiaoYan;
using SSheBei.Model;
using SundyChengZhong.Model;
using CommLei.DataChuLi;
using System.Reflection;
using System.Xml.Linq;

namespace SundyChengZhong.Frm
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

        /// <summary>
        /// 写寄存器
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
                FromSortZX(xinshebei.LisJiCunQis, true);
                Dictionary<int,Dictionary<int, List<CZJiCunQiModel>>> jixuxieru = new Dictionary<int, Dictionary<int, List<CZJiCunQiModel>>>();
                for (int i = 0; i < xinshebei.LisJiCunQis.Count; i++)
                {
                    int shebeidizhi = xinshebei.LisJiCunQis[i].SheBeiDiZhi;
                    int dugnm = xinshebei.LisJiCunQis[i].DuGNM;
                    if (jixuxieru.ContainsKey(shebeidizhi) ==false)
                    {
                        jixuxieru.Add(shebeidizhi, new Dictionary<int, List<CZJiCunQiModel>>());
                    }
                    if (jixuxieru[shebeidizhi].ContainsKey(dugnm) ==false)
                    {
                        jixuxieru[shebeidizhi].Add(dugnm, new List<CZJiCunQiModel>());
                    }
                    jixuxieru[shebeidizhi][dugnm].Add(xinshebei.LisJiCunQis[i]);
                }
                foreach (var item1 in jixuxieru.Keys)
                {                  
                    Dictionary<int, List<CZJiCunQiModel>> shuju = jixuxieru[item1];
                    foreach (var item in shuju.Keys)
                    {
                        int shuliang = 80;
                        int index = 0;
                        List<CZJiCunQiModel> sjicunqis = shuju[item];
                        Dictionary<int, List<CZJiCunQiModel>> paixu = new Dictionary<int, List<CZJiCunQiModel>>();
                        for (int i = 0; i < sjicunqis.Count; i++)
                        {
                            if (sjicunqis[i].DataSType == DataSType.XiePuTong)
                            {
                                JiCunQiModel jicunmodel = ShengChengModel(sjicunqis[i]);
                                jicunmodel.DuXie = 2;
                                CZJiCunQiModel czmodel = GetModel(sjicunqis[i].Name,false);
                                czmodel.JiCunQiModel = jicunmodel;
                                LisXie.Add(jicunmodel);
                                LisDuXie.Add(jicunmodel);
                                continue;
                            }
                            int pianyi = sjicunqis[i].JiCunDiZhi + sjicunqis[i].Count;
                            int zuixiaopianyi = sjicunqis[index].JiCunDiZhi;
                            if (pianyi - zuixiaopianyi <= shuliang)
                            {
                                if (paixu.ContainsKey(zuixiaopianyi) == false)
                                {
                                    paixu.Add(zuixiaopianyi, new List<CZJiCunQiModel>());

                                }
                                paixu[zuixiaopianyi].Add(sjicunqis[i]);

                            }
                            else
                            {
                                index = i;
                                zuixiaopianyi = sjicunqis[index].JiCunDiZhi;
                                if (paixu.ContainsKey(zuixiaopianyi) == false)
                                {
                                    paixu.Add(zuixiaopianyi, new List<CZJiCunQiModel>());

                                }
                                paixu[zuixiaopianyi].Add(sjicunqis[i]);

                            }

                        }
                        foreach (var item2 in paixu.Keys)
                        {
                            JiLuModel models = new JiLuModel();
                            models.SheBeiDiZhi = item1;
                            models.JiCunQiZuiXiaoPianYi = item2;
                            models.SheBeiID = shebei.SheBeiID;
                            models.IsXianQuan = item == 1 || item == 2 ? true : false;
                            int zongcount = paixu[item].Count;
                            int count = paixu[item][zongcount - 1].JiCunDiZhi + paixu[item][zongcount - 1].Count - paixu[item][0].JiCunDiZhi;
                            models.ShuJu = GetDuBaoWen(item2, count, (byte)item, (byte)item1);
                            models.Count = count;
                            for (int i = 0; i < paixu[item].Count; i++)
                            {
                                CZJiCunQiModel sdmodel = GetModel(paixu[item][i].Name, false);
                                JiCunQiModel jicunmodel = ShengChengModel(sdmodel);
                                jicunmodel.DuXie = 1;
                                sdmodel.JiCunQiModel = jicunmodel;
                             
                                if (sdmodel.DataSType ==DataSType.DuXieYiQi)
                                {
                                    jicunmodel.DuXie = 3;
                                    LisXie.Add(jicunmodel);
                                }
                                LisDu.Add(jicunmodel);
                                models.Lis.Add(sdmodel);
                                LisDuXie.Add(jicunmodel);
                            }
                            if (JiLu.ContainsKey(shebei.SheBeiID) == false)
                            {
                                JiLu.Add(shebei.SheBeiID, new List<JiLuModel>());
                            }
                            JiLu[shebei.SheBeiID].Add(models);
                        }

                    }
                }
               
              
             
            }
            KeyS = JiLu.Keys.ToList();
        }

        public void SetTx(int zongid, bool tx)
        {
            for (int i = 0; i < LisSheBei.Count; i++)
            {
                if (LisSheBei[i].SheBeiID == zongid)
                {
                    SheBeiModel cunModel = LisSheBei[i];
                    cunModel.Tx = tx;
                    for (int c = 0; c < cunModel.LisJiCunQis.Count; c++)
                    {

                        cunModel.LisJiCunQis[c].JiCunQiModel.IsKeKao = tx;
                    }
                    break;
                }
            }


        }

        public void SetJiCunQiValue(int zongid, List<byte> shuju, JiLuModel jiLuModel)
        {
            if (jiLuModel.IsXianQuan)
            {
                List<int> ziduan = new List<int>();
                for (int i = 3; i < shuju.Count - 2; i++)
                {
                    ziduan.AddRange(ChangYong.Get10Or2(shuju[i], 8));
                }
                for (int c = 0; c < jiLuModel.Lis.Count; c++)
                {
                    CZJiCunQiModel cunmodel = jiLuModel.Lis[c];

                    int zhengshu = cunmodel.JiCunDiZhi;
                    if (cunmodel.IsXieWan!=1)
                    {
                        cunmodel.IsXieWan = 1;
                    }
                    cunmodel.JiCunQiModel.IsKeKao = true;
                    if (zhengshu < ziduan.Count)
                    {
                        cunmodel.JiCunQiModel.Value = ziduan[zhengshu];
                    }


                }
            }
            else
            {
                List<byte> shujus = new List<byte>();
                for (int i = 3; i < shuju.Count - 2; i++)
                {
                    shujus.Add(shuju[i]);
                }
                for (int i = 0; i < jiLuModel.Lis.Count; i++)
                {
                    int weizhi = jiLuModel.Lis[i].JiCunDiZhi - jiLuModel.JiCunQiZuiXiaoPianYi;
                    int weizhisd = weizhi * 2;
                    List<byte> shujusd = new List<byte>();
                    for (int c = weizhisd; c < weizhisd + jiLuModel.Lis[i].Count * 2; c++)
                    {
                        shujusd.Add(shujus[c]);
                    }
                    float jieguozhi = CRC.GetInt(shujusd, 0);
                    if (jiLuModel.Lis[i].XiaoShuWei<=0)
                    {
                        jiLuModel.Lis[i].XiaoShuWei = 0;
                    }
                    if (jiLuModel.Lis[i].XiaoShuWei >14)
                    {
                        jiLuModel.Lis[i].XiaoShuWei = 14;
                    }
                    if (jiLuModel.Lis[i].BeiChuShu==0)
                    {
                        jiLuModel.Lis[i].BeiChuShu = 1;
                    }
                    if (jiLuModel.Lis[i].IsXieWan != 1)
                    {
                        jiLuModel.Lis[i].IsXieWan = 1;
                    }
                    double jieguozhDFi = (Math.Round((jieguozhi / jiLuModel.Lis[i].BeiChuShu) + jiLuModel.Lis[i].BZhi, jiLuModel.Lis[i].XiaoShuWei));
                    jiLuModel.Lis[i].JiCunQiModel.Value = jieguozhDFi;
                    jiLuModel.Lis[i].JiCunQiModel.IsKeKao = true;
                }
            }
            for (int i = 0; i < LisSheBei.Count; i++)
            {
                if (LisSheBei[i].SheBeiID == zongid)
                {
                    SheBeiModel cunModel = LisSheBei[i];
                    cunModel.LisJiCunQis.ForEach((x) => { x.JiCunQiModel.IsKeKao = true; });
                    cunModel.Tx = true;
                    break;
                }
            }
        }

        public void SetZhengTaiState(int zongid, string name, int state)
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
                            if (state == 0)
                            {
                                jiLuModel.Lis[i].JiCunQiModel.Value = "";
                            }
                            return;
                        }
                    }

                }
            }
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
        private void FromSortZX(List<CZJiCunQiModel> lisObj, bool IsSort)
        {
            if (lisObj.Count > 0)
            {
                try
                {
                    CZJiCunQiModel obj = null;
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
        private List<byte> GetDuBaoWen(int zuixiaodizhi, int count,byte gonengma,byte dizhi)
        {
            List<byte> fenzhuang = new List<byte>();
            fenzhuang.Add(dizhi);
            fenzhuang.Add(gonengma);
            byte[] shuju = GetBtyez(zuixiaodizhi);
            fenzhuang.AddRange(shuju);

            int shuliang = count;
            byte[] countbyte = GetBtyez(shuliang);
            fenzhuang.AddRange(countbyte);
            byte[] shu = CRC.ToModbus(fenzhuang, false);
            fenzhuang.AddRange(shu);
            return fenzhuang;
        }

        public byte[] GetBtyez(int value)
        {


            int hValue = (value >> 8) & 0xFF;

            int lValue = value & 0xFF;

            byte[] arr = new byte[] { (byte)hValue, (byte)lValue };
            return arr;
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isfuzhi"></param>
        /// <returns></returns>
        public CZJiCunQiModel GetModel(string name, bool isfuzhi)
        {
            for (int i = 0; i < LisSheBei.Count; i++)
            {
                for (int c = 0; c < LisSheBei[i].LisJiCunQis.Count; c++)
                {
                    if (LisSheBei[i].LisJiCunQis[c].JiCunQiModel.WeiYiBiaoShi.Equals(name))
                    {
                        if (isfuzhi)
                        {
                            CZJiCunQiModel models = ChangYong.FuZhiShiTi(LisSheBei[i].LisJiCunQis[c]);
                            models.SheBeiID = LisSheBei[i].SheBeiID;
                            return models;
                        }
                        else
                        {
                            LisSheBei[i].LisJiCunQis[c].SheBeiID = LisSheBei[i].SheBeiID;
                            return LisSheBei[i].LisJiCunQis[c];
                        }
                    }
                }

            }
            return null;
        }


        private JiCunQiModel ShengChengModel(CZJiCunQiModel model)
        {
            JiCunQiModel qiModel = new JiCunQiModel();
            qiModel.WeiYiBiaoShi = model.Name;
            qiModel.SheBeiID = SheBeiID;
            qiModel.MiaoSu = model.MiaoSu;
            return qiModel;
        }
    }
}
