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
        /// 收集值
        /// </summary>
        private List<double> JunYuanDu = new List<double>();
      
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
            foreach (SaoMaModel item in LisSheBei)
            {
                List<string> jicunqimodels = ChangYong.MeiJuLisName(typeof(CunType));
                for (int c = 0; c < jicunqimodels.Count; c++)
                {
                    CunType cuntype= ChangYong.GetMeiJuZhi<CunType>(jicunqimodels[c]);
                    if (cuntype == CunType.XieTestShuJu)
                    {
                        List<string> datatpes = ChangYong.MeiJuLisName(typeof(DataType));
                        for (int d = 0; d < datatpes.Count; d++)
                        {
                            DataType datatype = ChangYong.GetMeiJuZhi<DataType>(datatpes[d]);
                            if (datatype==DataType.Wu|| datatype == DataType.JunYunDu)
                            {
                                continue;
                            }
                            {
                                for (int i = 1; i < 4; i++)
                                {
                                    CunModel cunmodel = new CunModel();
                                    cunmodel.ZongSheBeiId = item.SheBeiID;
                                    cunmodel.IsDu = CunType.DuShuJu;
                                    cunmodel.SheMeYanSe = datatype;
                                    cunmodel.DuType =i;
                                    cunmodel.IsZhengZaiCe = 1;
                                    JiCunQiModel model = new JiCunQiModel();
                                    model.SheBeiID = SheBeiID;
                                    string mingcheng = i == 1 ? "亮度" : i == 2 ? "X坐标" : "Y坐标";
                                    model.WeiYiBiaoShi = $"{item.Name}-{datatype}{mingcheng}";
                                    model.MiaoSu = ChangYong.GetEnumDescription(cunmodel.IsDu);
                                    model.DuXie = 1;
                                    LisDu.Add(model);
                                    LisDuXie.Add(model);
                                    if (JiLu.ContainsKey(model.WeiYiBiaoShi) == false)
                                    {
                                        cunmodel.ZongSheBeiId = item.SheBeiID;
                                        cunmodel.JiCunQi = model;
                                        JiLu.Add(model.WeiYiBiaoShi, cunmodel);
                                    }
                                }
                            }
                            {
                                CunModel cunmodel = new CunModel();
                                cunmodel.ZongSheBeiId = item.SheBeiID;
                                cunmodel.IsDu = cuntype;
                                cunmodel.SheMeYanSe = datatype;
                                cunmodel.DuType = -1;
                                JiCunQiModel model = new JiCunQiModel();
                                model.SheBeiID = SheBeiID;
                                model.WeiYiBiaoShi = $"{item.Name}-{datatype}{jicunqimodels[c]}";
                                model.MiaoSu = ChangYong.GetEnumDescription(cunmodel.IsDu);
                                model.DuXie = 2;
                                LisXie.Add(model);
                                LisDuXie.Add(model);
                                if (JiLu.ContainsKey(model.WeiYiBiaoShi) == false)
                                {
                                    cunmodel.ZongSheBeiId = item.SheBeiID;
                                    cunmodel.JiCunQi = model;
                                    JiLu.Add(model.WeiYiBiaoShi, cunmodel);
                                }
                            }
                        }

                      
                    }
                    else if (cuntype==CunType.DuShuJu)
                    {
                        continue;
                    }
                    else  if (cuntype == CunType.XieJunYunDu)
                    {
                        List<string> datatpes = ChangYong.MeiJuLisName(typeof(DataType));
                        for (int d = 0; d < datatpes.Count; d++)
                        {
                            DataType datatype = ChangYong.GetMeiJuZhi<DataType>(datatpes[d]);
                            if ( datatype == DataType.JunYunDu)
                            {
                                for (int i = 1; i < 4; i++)
                                {
                                    CunModel cunmodel = new CunModel();
                                    cunmodel.ZongSheBeiId = item.SheBeiID;
                                    cunmodel.IsDu = CunType.DuShuJu;
                                    cunmodel.SheMeYanSe = datatype;
                                    cunmodel.DuType = i;
                                    cunmodel.IsZhengZaiCe = 1;
                                    JiCunQiModel model = new JiCunQiModel();
                                    model.SheBeiID = SheBeiID;
                                    string mingcheng = i == 1 ? "亮度" : i == 2 ? "X坐标" : "Y坐标";
                                    model.WeiYiBiaoShi = $"{item.Name}-{datatype}{mingcheng}";
                                    model.MiaoSu = ChangYong.GetEnumDescription(cunmodel.IsDu);
                                    model.DuXie = 1;
                                    LisDu.Add(model);
                                    LisDuXie.Add(model);
                                    if (JiLu.ContainsKey(model.WeiYiBiaoShi) == false)
                                    {
                                        cunmodel.ZongSheBeiId = item.SheBeiID;
                                        cunmodel.JiCunQi = model;
                                        JiLu.Add(model.WeiYiBiaoShi, cunmodel);
                                    }
                                }
                                {
                                    CunModel cunmodel = new CunModel();
                                    cunmodel.ZongSheBeiId = item.SheBeiID;
                                    cunmodel.IsDu = cuntype;
                                    cunmodel.SheMeYanSe = datatype;
                                    cunmodel.DuType = -1;
                                    JiCunQiModel model = new JiCunQiModel();
                                    model.SheBeiID = SheBeiID;
                                    model.WeiYiBiaoShi = $"{item.Name}-{cuntype}";
                                    model.MiaoSu = ChangYong.GetEnumDescription(cunmodel.IsDu);
                                    model.DuXie = 2;
                                    LisXie.Add(model);
                                    LisDuXie.Add(model);
                                    if (JiLu.ContainsKey(model.WeiYiBiaoShi) == false)
                                    {
                                        cunmodel.ZongSheBeiId = item.SheBeiID;
                                        cunmodel.JiCunQi = model;
                                        JiLu.Add(model.WeiYiBiaoShi, cunmodel);
                                    }
                                }
                            }
                           
                        }
                    }
                    else
                    {
                        CunModel cunmodel = new CunModel();
                        cunmodel.ZongSheBeiId = item.SheBeiID;
                        cunmodel.IsDu = cuntype;
                        cunmodel.SheMeYanSe = DataType.Wu;
                        cunmodel.DuType = -1;
                        JiCunQiModel model = new JiCunQiModel();
                        model.SheBeiID = SheBeiID;
                        model.WeiYiBiaoShi = $"{item.Name}-{jicunqimodels[c]}";
                        model.MiaoSu = ChangYong.GetEnumDescription(cunmodel.IsDu);
                        model.DuXie = 2;
                        LisXie.Add(model);
                        LisDuXie.Add(model);
                        if (JiLu.ContainsKey(model.WeiYiBiaoShi) == false)
                        {
                            cunmodel.ZongSheBeiId = item.SheBeiID;
                            cunmodel.JiCunQi = model;
                            JiLu.Add(model.WeiYiBiaoShi, cunmodel);
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
                    for (int c = 0; c < KeyS.Count; c++)
                    {
                        if (JiLu[KeyS[c]].ZongSheBeiId== zongid)
                        {
                            JiLu[KeyS[c]].JiCunQi.IsKeKao = zhuangtai;
                        }
                    }
                    break;
                }
            }
        }

        public void SetJiCunQiValue(string weiyibiaoshi, string shuju)
        {
            if (JiLu.ContainsKey(weiyibiaoshi))
            {
                CunModel cunModel = JiLu[weiyibiaoshi];            
                cunModel.JiCunQi.Value = shuju;           
            }
        
        }
        public bool SetJiCunQiValue(CunModel model, byte[] shuju)
        {
            if (JiLu.ContainsKey(model.JiCunQi.WeiYiBiaoShi))
            {
                CunModel cunModel = JiLu[model.JiCunQi.WeiYiBiaoShi];
                string jieguo = Encoding.ASCII.GetString(shuju);
                cunModel.JiCunQi.Value = jieguo;
                if (jieguo.ToLower().Contains("ok"))
                {

                    if (cunModel.IsDu == CunType.XieTestShuJu)
                    {
                        string[] fenge = jieguo.Split(',');
                        for (int c = 0; c < KeyS.Count; c++)
                        {
                            if (JiLu[KeyS[c]].ZongSheBeiId == cunModel.ZongSheBeiId)
                            {
                                if (JiLu[KeyS[c]].IsDu == CunType.DuShuJu)
                                {
                                    if (JiLu[KeyS[c]].SheMeYanSe == cunModel.SheMeYanSe)
                                    {
                                        if (JiLu[KeyS[c]].DuType == 1)
                                        {
                                            if (fenge.Length > 5)
                                            {
                                                JiLu[KeyS[c]].JiCunQi.Value = fenge[5];
                                            }
                                        }
                                        if (JiLu[KeyS[c]].DuType == 2)
                                        {
                                            if (fenge.Length > 3)
                                            {
                                                JiLu[KeyS[c]].JiCunQi.Value = fenge[3];
                                            }
                                        }
                                        if (JiLu[KeyS[c]].DuType == 3)
                                        {
                                            if (fenge.Length > 4)
                                            {
                                                JiLu[KeyS[c]].JiCunQi.Value = fenge[4];
                                            }
                                        }
                                    }

                                }

                            }
                        }
                    }
                    else if (cunModel.IsDu == CunType.XieJunYunDu)
                    {
                        string[] fenge = jieguo.Split(',');
                        if (fenge.Length > 5)
                        {
                            JunYuanDu.Add(ChangYong.TryDouble(fenge[5],0));
                            for (int c = 0; c < KeyS.Count; c++)
                            {
                                if (JiLu[KeyS[c]].ZongSheBeiId == cunModel.ZongSheBeiId)
                                {
                                    if (JiLu[KeyS[c]].IsDu == CunType.DuShuJu)
                                    {
                                        if (JiLu[KeyS[c]].SheMeYanSe == cunModel.SheMeYanSe)
                                        {
                                            if (JiLu[KeyS[c]].DuType == 1)
                                            {
                                                if (fenge.Length > 5)
                                                {
                                                    JiLu[KeyS[c]].JiCunQi.Value = fenge[5];
                                                }
                                            }
                                            if (JiLu[KeyS[c]].DuType == 2)
                                            {
                                                if (fenge.Length > 3)
                                                {
                                                    JiLu[KeyS[c]].JiCunQi.Value = fenge[3];
                                                }
                                            }
                                            if (JiLu[KeyS[c]].DuType == 3)
                                            {
                                                if (fenge.Length > 4)
                                                {
                                                    JiLu[KeyS[c]].JiCunQi.Value = fenge[4];
                                                }
                                            }
                                        }

                                    }

                                }
                            }
                        }

                    }
                    return true;
                }
            }
            return false;

        }
        public void SetZhengZaiValue(string weiyibiaoshi,int sate)
        {
            if (JiLu.ContainsKey(weiyibiaoshi))
            {
                CunModel cunModel = JiLu[weiyibiaoshi];              
                cunModel.IsZhengZaiCe = sate;
                if (cunModel.IsZhengZaiCe == 0)
                {
                    cunModel.JiCunQi.Value = "";

                    if (cunModel.IsDu == CunType.XieTestShuJu)
                    {
                    
                        for (int c = 0; c < KeyS.Count; c++)
                        {
                            if (JiLu[KeyS[c]].ZongSheBeiId == cunModel.ZongSheBeiId)
                            {
                                if (JiLu[KeyS[c]].IsDu == CunType.DuShuJu)
                                {
                                    if (JiLu[KeyS[c]].SheMeYanSe == cunModel.SheMeYanSe)
                                    {
                                        if (JiLu[KeyS[c]].DuType == 1)
                                        {
                                            JiLu[KeyS[c]].JiCunQi.Value = "";
                                        }
                                        if (JiLu[KeyS[c]].DuType == 2)
                                        {
                                            JiLu[KeyS[c]].JiCunQi.Value = "";
                                        }
                                        if (JiLu[KeyS[c]].DuType == 3)
                                        {
                                            JiLu[KeyS[c]].JiCunQi.Value = "";
                                        }
                                    }

                                }

                            }
                        }
                    }

                    if (cunModel.IsDu == CunType.XieJunYunDu)
                    {

                        for (int c = 0; c < KeyS.Count; c++)
                        {
                            if (JiLu[KeyS[c]].ZongSheBeiId == cunModel.ZongSheBeiId)
                            {
                                if (JiLu[KeyS[c]].IsDu == CunType.DuShuJu)
                                {
                                    if (JiLu[KeyS[c]].SheMeYanSe == cunModel.SheMeYanSe)
                                    {
                                        if (JiLu[KeyS[c]].DuType == 1)
                                        {
                                            JiLu[KeyS[c]].JiCunQi.Value = "";
                                        }
                                        if (JiLu[KeyS[c]].DuType == 2)
                                        {
                                            JiLu[KeyS[c]].JiCunQi.Value = "";
                                        }
                                        if (JiLu[KeyS[c]].DuType == 3)
                                        {
                                            JiLu[KeyS[c]].JiCunQi.Value = "";
                                        }
                                    }

                                }

                            }
                        }
                    }
                }           
            }

        }

        public void QingLing(int zongid)
        {
            for (int c = 0; c < KeyS.Count; c++)
            {
                if (JiLu[KeyS[c]].ZongSheBeiId == zongid)
                {
                    if (JiLu[KeyS[c]].IsDu == CunType.DuShuJu)
                    {
                        JiLu[KeyS[c]].JiCunQi.Value = "";
                    }
          
                }
            }
            JunYuanDu.Clear();
        }
        public bool QiuSeYu(CunModel zongid, out double shuju)
        {
            //Gamut= ALCD/A基准*100%
            // ALCD值 = (Rx * Gy + Ry * Bx + Gx * By – Rx* By – Gx* Ry – Bx* Gy)/ 2
            shuju = 0;
            SaoMaModel shebeimodel = GetSheBeiModel(zongid);
            if (shebeimodel != null)
            {

                bool r = true;
                double rx = 0;
                double ry = 0;
                bool g = true;
                double gx = 0;
                double gy = 0;
                bool b = true;
                double bx = 0;
                double by = 0;
                for (int c = 0; c < KeyS.Count; c++)
                {
                    if (JiLu[KeyS[c]].ZongSheBeiId == zongid.ZongSheBeiId)
                    {
                        if (JiLu[KeyS[c]].IsDu == CunType.DuShuJu)
                        {
                            if (JiLu[KeyS[c]].SheMeYanSe == DataType.Red)
                            {
                                if (JiLu[KeyS[c]].DuType == 2)
                                {
                                    rx = ChangYong.TryDouble(JiLu[KeyS[c]].JiCunQi.Value, 0);
                                    if (r)
                                    {
                                        r = string.IsNullOrEmpty(JiLu[KeyS[c]].JiCunQi.Value.ToString()) == false;
                                    }
                                }
                                if (JiLu[KeyS[c]].DuType == 3)
                                {
                                    ry = ChangYong.TryDouble(JiLu[KeyS[c]].JiCunQi.Value, 0);
                                    if (r)
                                    {
                                        r = string.IsNullOrEmpty(JiLu[KeyS[c]].JiCunQi.Value.ToString()) == false;
                                    }
                                }

                            }
                            else if (JiLu[KeyS[c]].SheMeYanSe == DataType.Green)
                            {
                                if (JiLu[KeyS[c]].DuType == 2)
                                {
                                    gx = ChangYong.TryDouble(JiLu[KeyS[c]].JiCunQi.Value, 0);
                                    if (g)
                                    {
                                        g = string.IsNullOrEmpty(JiLu[KeyS[c]].JiCunQi.Value.ToString()) == false;
                                    }
                                }
                                if (JiLu[KeyS[c]].DuType == 3)
                                {
                                    gy = ChangYong.TryDouble(JiLu[KeyS[c]].JiCunQi.Value, 0);
                                    if (g)
                                    {
                                        g = string.IsNullOrEmpty(JiLu[KeyS[c]].JiCunQi.Value.ToString()) == false;
                                    }
                                }
                            }
                            else if (JiLu[KeyS[c]].SheMeYanSe == DataType.Blue)
                            {
                                if (JiLu[KeyS[c]].DuType == 2)
                                {
                                    bx = ChangYong.TryDouble(JiLu[KeyS[c]].JiCunQi.Value, 0);
                                    if (b)
                                    {
                                        b = string.IsNullOrEmpty(JiLu[KeyS[c]].JiCunQi.Value.ToString()) == false;
                                    }
                                }
                                if (JiLu[KeyS[c]].DuType == 3)
                                {
                                    by = ChangYong.TryDouble(JiLu[KeyS[c]].JiCunQi.Value, 0);
                                    if (b)
                                    {
                                        b = string.IsNullOrEmpty(JiLu[KeyS[c]].JiCunQi.Value.ToString()) == false;
                                    }
                                }
                            }

                        }
                     
                    }
                }
                if (r && g && b)
                {
                    // ALCD值 = (Rx * Gy + Ry * Bx + Gx * By – Rx* By – Gx* Ry – Bx* Gy)/ 2
                    double Ajizhun = shebeimodel.JiZhunACD;

                    double Acd = (rx - bx) * (gy - by) - 0.5 * (rx - gx) * (gy - ry) - 0.5 * (gx - bx) * (gy - by) - 0.5 * (rx - bx) * (ry - by);

                    double Gamut = (Acd / Ajizhun) * 100;
                    shuju = Gamut;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool QiuSeDuiBiDui(CunModel zongid, out double shuju)
        {
            //Gamut= ALCD/A基准*100%
            // ALCD值 = (Rx * Gy + Ry * Bx + Gx * By – Rx* By – Gx* Ry – Bx* Gy)/ 2
            shuju = 0;
            SaoMaModel shebeimodel = GetSheBeiModel(zongid);
            if (shebeimodel != null)
            {

                bool baise = true;
                double bailiang = 0;
              
                bool heise = true;
                double heiliang = 0;
           
                for (int c = 0; c < KeyS.Count; c++)
                {
                    if (JiLu[KeyS[c]].ZongSheBeiId == zongid.ZongSheBeiId)
                    {
                        if (JiLu[KeyS[c]].IsDu == CunType.DuShuJu)
                        {
                            if (JiLu[KeyS[c]].SheMeYanSe == DataType.White)
                            {
                                if (JiLu[KeyS[c]].DuType == 1)
                                {
                                    bailiang = ChangYong.TryDouble(JiLu[KeyS[c]].JiCunQi.Value, 0);
                                    if (baise)
                                    {
                                        baise = string.IsNullOrEmpty(JiLu[KeyS[c]].JiCunQi.Value.ToString()) == false;
                                    }
                                }


                            }
                            else if (JiLu[KeyS[c]].SheMeYanSe == DataType.Black)
                            {
                                if (JiLu[KeyS[c]].DuType == 1)
                                {
                                    heiliang = ChangYong.TryDouble(JiLu[KeyS[c]].JiCunQi.Value, 0);
                                    if (heise)
                                    {
                                        heise = string.IsNullOrEmpty(JiLu[KeyS[c]].JiCunQi.Value.ToString()) == false;
                                    }
                                }

                            }
                        }

                    }
                }
                if (baise && heise)
                {
                    if (heiliang==0)
                    {
                        heiliang = 0.00001;
                    }
                    //对比度=（亮区亮度值）/ 亮区亮度值，值越大，明暗分界越明显
                    double Acd = (bailiang) / heiliang;
                    shuju = Acd;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool QiuJunYunDu(CunModel zongid, out double shuju)
        {
            //Gamut= ALCD/A基准*100%
            // ALCD值 = (Rx * Gy + Ry * Bx + Gx * By – Rx* By – Gx* Ry – Bx* Gy)/ 2
            shuju = 0;
            SaoMaModel shebeimodel = GetSheBeiModel(zongid);
            if (shebeimodel != null)
            {
                if (JunYuanDu.Count > 1)
                {
                    double max = JunYuanDu.Max();
                    double min = JunYuanDu.Min();
                    if (max == 0)
                    {

                        return false;
                    }
                    else
                    {
                        double Acd = min / max;
                        shuju = Acd;
                        return true;
                    }
                   
                }
                else
                {
                    return false;
                }
        
            }
            else
            {
                return false;
            }
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

        public SaoMaModel GetSheBeiModel(int shebeiid)
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
