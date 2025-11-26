using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CommLei.DataChuLi;
using CommLei.JiChuLei;
using LeiSaiDMC.Model;
using SSheBei.Model;

namespace LeiSaiDMC.Frm
{
    public  class PeiZhiLei
    {
        public event  Action<JiCunQiModel> AnJianZhou;
        /// <summary>
        /// true 表示刷新轴配置
        /// </summary>
        public bool IsShuXinZhouPeiZhi { get; set; } = true;
      
        /// <summary>
        /// 设备ID
        /// </summary>
        public int SheBeiID { get; set; }
        /// <summary>
        /// true 表示是配置
        /// </summary>
        public bool IsPeiZhi { get; set; } = false;
        /// <summary>
        /// true 表示调试
        /// </summary>
        public bool IsTiaoShi { get; set; } = false;
        /// <summary>
        /// 文件名
        /// </summary>
        public string WenJianName { get; set; } = "";

        /// <summary>
        /// 设备
        /// </summary>
        public List<LSModel> LisSheBei { get; set; } = new List<LSModel>();

        public  List<KaCanShuModel> Keys = new List<KaCanShuModel>();

        /// <summary>
        /// 读寄存器
        /// </summary>
        public List<JiCunQiModel> QiTaJiCunQiDu = new List<JiCunQiModel>();
        /// <summary>
        /// 写寄存器
        /// </summary>
        public List<JiCunQiModel> QiTaJiCunQiXie = new List<JiCunQiModel>();

        /// <summary>
        /// 对应的寄存器
        /// </summary>
        public Dictionary<KaCanShuModel, List<JiCunQiModel>> DuiYingJiCunQi = new Dictionary<KaCanShuModel, List<JiCunQiModel>>();

        /// <summary>
        /// 每个轴对应的速度配置
        /// </summary>
        public Dictionary<KaCanShuModel, ZhouPeiZhiModel> ZhouPeiZhi = new Dictionary<KaCanShuModel, ZhouPeiZhiModel>();

        /// <summary>
        /// 写寄存起对应的类型
        /// </summary>
        private Dictionary<string, XieCaoZuoType> JiCunQiType = new Dictionary<string, XieCaoZuoType>();

        /// <summary>
        /// 读寄存器对应
        /// </summary>
        private Dictionary<string, DuCanShuType> JiCunQiDuType = new Dictionary<string, DuCanShuType>();

        /// <summary>
        /// 配置数据
        /// </summary>
        private JosnOrSModel JosnOrSModel;


        /// <summary>
        /// 用于初始化
        /// </summary>
        public void IniData()
        {
            LisSheBei.Clear();
            JiCunQiType.Clear();
            DuiYingJiCunQi.Clear();
            ZhouPeiZhi.Clear();
            QiTaJiCunQiDu.Clear();
            QiTaJiCunQiXie.Clear();
            JiCunQiDuType.Clear();
            string wenjinlujing = GetLuJing();
            JosnOrSModel = new JosnOrSModel(wenjinlujing);
            List<LSModel> zishebeis = JosnOrSModel.GetLisTModel<LSModel>();
            if (zishebeis == null)
            {
                zishebeis = new List<LSModel>();
            }
            for (int c = 0; c < zishebeis.Count; c++)
            {
                LSModel shebei = zishebeis[c];
                shebei.ZuiDaDuIO = 1;
                shebei.ZuiDaXieIO = 1;
                for (int d = 0; d < shebei.LisKaModels.Count; d++)
                {
                    KaCanShuModel kamodel = shebei.LisKaModels[d];
                    if (DuiYingJiCunQi.ContainsKey(kamodel) == false)
                    {
                        DuiYingJiCunQi.Add(kamodel, new List<JiCunQiModel>());
                    }
                    if (kamodel.IsZhou)
                    {
                        List<JiCunQiModel> lismodel = AddZhouJiCunQi(kamodel.KaName);
                        DuiYingJiCunQi[kamodel].AddRange(lismodel);                                       
                        List<ZhouPeiZhiModel> liszhou = shebei.LisZhouPeiZhiModels;
                        for (int i = 0; i < liszhou.Count; i++)
                        {
                            if (liszhou[i].ZhouPeiZhiID== kamodel.ZhouPeiZhiID)
                            {
                                if (ZhouPeiZhi.ContainsKey(kamodel)==false)
                                {
                                    ZhouPeiZhi.Add(kamodel, liszhou[i]);
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        JiCunQiModel model = new JiCunQiModel();
                        model.WeiYiBiaoShi = kamodel.KaName;
                        model.SheBeiID = SheBeiID;
                      
                        if (kamodel.IsXieIO == false)
                        {
                            model.DuXie = 1;
                            model.MiaoSu = "该寄存器是读:0:1:无单位:采用比较关系";
                            QiTaJiCunQiDu.Add(model);
                            if (shebei.ZuiDaDuIO < kamodel.BitNoOrZhouHao)
                            {
                                shebei.ZuiDaDuIO = kamodel.BitNoOrZhouHao;
                            }

                            if (JiCunQiDuType.ContainsKey(model.WeiYiBiaoShi) == false)
                            {
                                JiCunQiDuType.Add(model.WeiYiBiaoShi, DuCanShuType.DuIO);
                            }

                        }
                        else
                        {
                            model.MiaoSu = "该寄存器是读写:0:1:无单位:采用比较关系";
                            model.DuXie = 2;
                            QiTaJiCunQiXie.Add(model);
                            if (shebei.ZuiDaXieIO < kamodel.BitNoOrZhouHao)
                            {
                                shebei.ZuiDaXieIO = kamodel.BitNoOrZhouHao;
                            }
                            if (JiCunQiType.ContainsKey(model.WeiYiBiaoShi) == false)
                            {
                                JiCunQiType.Add(model.WeiYiBiaoShi, XieCaoZuoType.XieIO);
                            }
                            if (JiCunQiDuType.ContainsKey(model.WeiYiBiaoShi) == false)
                            {
                                JiCunQiDuType.Add(model.WeiYiBiaoShi, DuCanShuType.XieIO);
                            }
                        }
                        DuiYingJiCunQi[kamodel].Add(model);
                      
                    }
                  
                }
                LisSheBei.Add(shebei);
            }
            Keys = DuiYingJiCunQi.Keys.ToList();
            //全局寄存器
            {
                {
                    JiCunQiModel mjicunqimode = new JiCunQiModel();
                    mjicunqimode.WeiYiBiaoShi = $"读{DuCanShuType.总线状态}";
                    mjicunqimode.SheBeiID = SheBeiID;
                    mjicunqimode.MiaoSu = $"该寄存器是读写:0:1:无单位:采用比较关系 1表示总线良好" ;
                    mjicunqimode.DuXie = 3;
                    QiTaJiCunQiDu.Add(mjicunqimode);
                    if (JiCunQiDuType.ContainsKey(mjicunqimode.WeiYiBiaoShi) == false)
                    {
                        JiCunQiDuType.Add(mjicunqimode.WeiYiBiaoShi, DuCanShuType.总线状态);
                    }
                }
            }
            //全局写寄存器
            {
               
                {
                    JiCunQiModel mjicunqimode = new JiCunQiModel();
                    mjicunqimode.WeiYiBiaoShi = "写热复位";
                    mjicunqimode.SheBeiID = SheBeiID;
                    mjicunqimode.MiaoSu = $"该寄存器是写:0:1:无单位:";
                    mjicunqimode.DuXie = 1;
                    QiTaJiCunQiXie.Add(mjicunqimode);
                    if (JiCunQiType.ContainsKey(mjicunqimode.WeiYiBiaoShi) == false)
                    {
                        JiCunQiType.Add(mjicunqimode.WeiYiBiaoShi, XieCaoZuoType.热复位);
                    }
                }
                {
                    JiCunQiModel mjicunqimode = new JiCunQiModel();
                    mjicunqimode.WeiYiBiaoShi = "写所有轴停止";
                    mjicunqimode.SheBeiID = SheBeiID;
                    mjicunqimode.MiaoSu = $"该寄存器是写:0:1:无单位:";
                    mjicunqimode.DuXie = 1;
                    QiTaJiCunQiXie.Add(mjicunqimode);
                    if (JiCunQiType.ContainsKey(mjicunqimode.WeiYiBiaoShi) == false)
                    {
                        JiCunQiType.Add(mjicunqimode.WeiYiBiaoShi, XieCaoZuoType.所有轴停止);
                    }
                }
                {
                    JiCunQiModel mjicunqimode = new JiCunQiModel();
                    mjicunqimode.WeiYiBiaoShi = "写所有轴回零";
                    mjicunqimode.SheBeiID = SheBeiID;
                    mjicunqimode.MiaoSu = $"该寄存器是写:0:1:无单位:所有轴回零";
                    mjicunqimode.DuXie = 1;
                    QiTaJiCunQiXie.Add(mjicunqimode);
                    if (JiCunQiType.ContainsKey(mjicunqimode.WeiYiBiaoShi) == false)
                    {
                        JiCunQiType.Add(mjicunqimode.WeiYiBiaoShi, XieCaoZuoType.所有轴回零);
                    }
                }
            }
         
        }

        /// <summary>
        /// 1表示可以 2表示不可以 3表示没有
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JiCunQiModel IsChengGong(JiCunQiModel model)
        {
           
            for (int i = 0; i < QiTaJiCunQiDu.Count; i++)
            {
                if (QiTaJiCunQiDu[i].WeiYiBiaoShi.Equals(model.WeiYiBiaoShi))
                {
                    return QiTaJiCunQiDu[i];
                }
            }
         
            return null;
        }

        public void SetKaCardIO(ushort cardno, LSModel model)
        {
            for (int i = 0; i < model.LisKaModels.Count; i++)
            {
                model.LisKaModels[i].CardNO = cardno;
            }
        }

        public List<JiCunQiModel> PeiZhiDuXie()
        {
            List<JiCunQiModel> shuju = new List<JiCunQiModel>();
            shuju.AddRange(QiTaJiCunQiDu);
            shuju.AddRange(QiTaJiCunQiXie);
          
            return shuju;
        }


        public bool FuZhiJiCunQi(DuCanShuType canShu,object shuju,KaCanShuModel model)
        {

            if (canShu.ToString().Contains("IO") == false)
            {
                switch (canShu)
                {
                    case DuCanShuType.位置:
                    case DuCanShuType.速度:
                    case DuCanShuType.使能:
                    case DuCanShuType.轴状态:
                    case DuCanShuType.运动状态:
                        {
                            if (DuiYingJiCunQi.ContainsKey(model))
                            {
                                List<JiCunQiModel> jicunqis = DuiYingJiCunQi[model];
                                if (model.IsZhou)
                                {
                                    for (int i = 0; i < jicunqis.Count; i++)
                                    {
                                        if (JiCunQiDuType.ContainsKey(jicunqis[i].WeiYiBiaoShi))
                                        {
                                            if (JiCunQiDuType[jicunqis[i].WeiYiBiaoShi] == canShu)
                                            {
                                                jicunqis[i].Value = shuju;
                                                break;
                                            }
                                        }
                                    }
                                }

                            }
                          
                        }
                        break;
                    case DuCanShuType.超限报警:
                        {
                            if (DuiYingJiCunQi.ContainsKey(model))
                            {
                                List<JiCunQiModel> jicunqis = DuiYingJiCunQi[model];
                                if (model.IsZhou)
                                {
                                    ZhouPeiZhiModel mods = ZhouPeiZhi[model];

                                    double zhi = ChangYong.TryDouble(shuju, 0);
                                    int xianshi = 0;
                                    if (zhi >= mods.JuLiZuiDi && zhi <= mods.JuLiZuiGao)
                                    {
                                        xianshi = 0;
                                    }
                                    else
                                    {
                                        xianshi = 1;
                                    }
                                    for (int i = 0; i < jicunqis.Count; i++)
                                    {
                                        if (JiCunQiDuType.ContainsKey(jicunqis[i].WeiYiBiaoShi))
                                        {
                                            if (JiCunQiDuType[jicunqis[i].WeiYiBiaoShi] == canShu)
                                            {
                                                jicunqis[i].Value = xianshi;
                                                break;
                                            }
                                        }
                                    }
                                    if (xianshi==1)
                                    {
                                        return true;
                                    }
                                }
                            }
                         
                        }
                        break;
                    case DuCanShuType.总线状态:
                        {
                            for (int i = 0; i < QiTaJiCunQiDu.Count; i++)
                            {
                                if (JiCunQiDuType.ContainsKey(QiTaJiCunQiDu[i].WeiYiBiaoShi))
                                {
                                    if (JiCunQiDuType[QiTaJiCunQiDu[i].WeiYiBiaoShi] == canShu)
                                    {
                                        QiTaJiCunQiDu[i].Value = ChangYong.TryInt(shuju, 1) == 0 ? 1 : 0;
                                        break;
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }

            }

            return false;
        }
        public void FuZhiJiCunQiIO(DuCanShuType canShu, List<bool> shuju,List<KaCanShuModel> liskamodel)
        {
            for (int c = 0; c < liskamodel.Count; c++)
            {
                KaCanShuModel model = liskamodel[c];
                if (DuiYingJiCunQi.ContainsKey(model))
                {
                    List<JiCunQiModel> jicunqis = DuiYingJiCunQi[model];
                    if (canShu.ToString().Contains("IO"))
                    {
                        if (model.IsZhou == false)
                        {
                            if (canShu == DuCanShuType.DuIO)
                            {
                                if (model.IsXieIO == false)
                                {
                                    int bitno = model.BitNoOrZhouHao;
                                    for (int i = 0; i < jicunqis.Count; i++)
                                    {
                                        if (bitno < shuju.Count)
                                        {
                                            jicunqis[i].Value = shuju[bitno] ? 1 : 0;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (model.IsXieIO)
                                {
                                    int bitno = model.BitNoOrZhouHao;
                                    for (int i = 0; i < jicunqis.Count; i++)
                                    {
                                        if (bitno < shuju.Count)
                                        {
                                            jicunqis[i].Value = shuju[bitno] ? 1 : 0;
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
            }
         
        }


        public KaCanShuModel GetHuoQu( JiCunQiModel mode, out XieCaoZuoType xie)
        {
            xie = XieCaoZuoType.Wu;
            foreach (var item in DuiYingJiCunQi.Keys)
            {
                
                List<JiCunQiModel> lis = DuiYingJiCunQi[item];
                for (int i = 0; i < lis.Count; i++)
                {
                    if (lis[i].WeiYiBiaoShi.Equals(mode.WeiYiBiaoShi))
                    {
                        if (item.IsZhou)
                        {
                            if (JiCunQiType.ContainsKey(lis[i].WeiYiBiaoShi))
                            {
                                xie = JiCunQiType[lis[i].WeiYiBiaoShi];
                                return item;
                            }
                           
                        }
                        else
                        {
                            if (item.IsXieIO)
                            {
                                xie = XieCaoZuoType.XieIO;
                                return item;
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < QiTaJiCunQiXie.Count; i++)
            {
                if (QiTaJiCunQiXie[i].WeiYiBiaoShi.Equals(mode.WeiYiBiaoShi))
                {
                    if (JiCunQiType.ContainsKey(QiTaJiCunQiXie[i].WeiYiBiaoShi))
                    {
                        xie = JiCunQiType[QiTaJiCunQiXie[i].WeiYiBiaoShi];
                        return null;
                    }
                }
            }

            return null;
        }


        /// <summary>
        /// 配置用的 读取
        /// </summary>
        /// <returns></returns>
        public List<LSModel> GetSheBei()
        {
            string wenjinlujing = GetLuJing();
            JosnOrSModel JosnOrSModesl = new JosnOrSModel(wenjinlujing);
            List<LSModel> LisSheBesi = JosnOrSModesl.GetLisTModel<LSModel>();
            if (LisSheBesi == null)
            {
                LisSheBesi = new List<LSModel>();
            }
            return LisSheBesi;
        }

        /// <summary>
        /// 配置用的 保存
        /// </summary>
        /// <param name="shebei"></param>
        public void BaoCun(List<LSModel> shebei)
        {
            string wenjinlujing = GetLuJing();
            JosnOrSModel JosnOrSModesl = new JosnOrSModel(wenjinlujing);
            JosnOrSModesl.XieTModel(shebei);
        }

        private string GetLuJing()
        {
            string lujing = string.Format("{0}{1}", Directory.GetCurrentDirectory(), @"\SheBeiPeiZhi");
            if (Directory.Exists(lujing) == false)
            {
                Directory.CreateDirectory(lujing);
            }
            string zhenlujing = string.Format(@"{0}\{1}.txt", lujing, WenJianName);
            return zhenlujing;
        }

        public object GetZhi(JiCunQiModel model,out DuCanShuType xie)
        {
            xie = DuCanShuType.Wu;
            foreach (var item in JiCunQiDuType.Keys)
            {
                if (item.Equals(model.WeiYiBiaoShi))
                {
                    xie = JiCunQiDuType[model.WeiYiBiaoShi];
                    return model.Value;
                }
            }
         
            return null;
        }


        public ZhouPeiZhiModel GetZhouPeiZhi(KaCanShuModel ka)
        {
            if (ZhouPeiZhi.ContainsKey(ka))
            {
                return ZhouPeiZhi[ka];
            }
            return null;
        }

        public void SetZhouPeiZhi(KaCanShuModel ka, ZhouPeiZhiModel model)
        {
            if (ZhouPeiZhi.ContainsKey(ka))
            {
                model.ZhouPeiZhiID = ka.ZhouPeiZhiID;
                ZhouPeiZhi[ka] = model;
            }
            else
            {
                model.ZhouPeiZhiID = ka.ZhouPeiZhiID;
                ZhouPeiZhi.Add(ka,model);
            }
            IsShuXinZhouPeiZhi = true;
            List<LSModel> xin = GetSheBei();
            bool iscunzai = false;
            for (int i = 0; i < xin.Count; i++)
            {
                List<ZhouPeiZhiModel> kas = xin[i].LisZhouPeiZhiModels;
               
                for (int c = 0; c < kas.Count; c++)
                {
                    if (kas[c].ZhouPeiZhiID== model.ZhouPeiZhiID)
                    {
                        kas[c] = model;
                        iscunzai = true;
                        break;
                    }
                }
                if (iscunzai) { break; }
            }
            if (iscunzai==false)
            {
                for (int i = 0; i < xin.Count; i++)
                {
                    List<KaCanShuModel> kas = xin[i].LisKaModels;
                    bool cunzai = false;
                    for (int c = 0; c < kas.Count; c++)
                    {
                        if (kas[c].ZhouPeiZhiID == model.ZhouPeiZhiID)
                        {
                            xin[i].LisZhouPeiZhiModels.Add(model);
                            cunzai = true;
                            break;
                        }
                    }
                    if (cunzai) { break; }
                }
            }
            BaoCun(xin);


        }

        public void SetIO(string name,bool zhi)
        {
            for (int i = 0; i < QiTaJiCunQiXie.Count; i++)
            {
                if (QiTaJiCunQiXie[i].WeiYiBiaoShi.Equals(name))
                {
                    JiCunQiModel model =ChangYong.FuZhiShiTi(QiTaJiCunQiXie[i]);
                    model.Value = zhi ? 1 : 0;
                    if (model != null)
                    {
                        if (AnJianZhou != null)
                        {
                            AnJianZhou(model);
                        }
                    }
                    return;
                }
            }
        }

        public void ZhouGongJu(KaCanShuModel cahushu1, XieCaoZuoType leixing,object zhi)
        {       
            if (cahushu1 != null&&cahushu1.IsZhou)
            {
                JiCunQiModel jiCunQiModel = null;
                if (DuiYingJiCunQi.ContainsKey(cahushu1))
                {
                    List<JiCunQiModel> jicun = DuiYingJiCunQi[cahushu1];
                    for (int i = 0; i < jicun.Count; i++)
                    {
                        if (JiCunQiType.ContainsKey(jicun[i].WeiYiBiaoShi))
                        {
                            if (JiCunQiType[jicun[i].WeiYiBiaoShi] == leixing)
                            {
                                jiCunQiModel = ChangYong.FuZhiShiTi(jicun[i]);
                                jiCunQiModel.Value = zhi;
                                break;
                            }
                        }
                    }
                    
                }
                if (jiCunQiModel!=null)
                {
                    if (AnJianZhou!=null)
                    {
                        AnJianZhou(jiCunQiModel);
                    }
                }
            }
        }


        private List<JiCunQiModel> AddZhouJiCunQi(string kaname)
        {
            List<JiCunQiModel> jicunqis = new List<JiCunQiModel>();
            //使能
            {
                JiCunQiModel mjicunqimode = new JiCunQiModel();
                mjicunqimode.WeiYiBiaoShi = $"{kaname}使能";
                mjicunqimode.SheBeiID = SheBeiID;
                mjicunqimode.MiaoSu = $"该寄存器是读写:0:1:无单位:采用比较关系 1表示使能";
                QiTaJiCunQiDu.Add( mjicunqimode);
                QiTaJiCunQiXie.Add(mjicunqimode);
                if (JiCunQiDuType.ContainsKey(mjicunqimode.WeiYiBiaoShi) == false)
                {
                    JiCunQiDuType.Add(mjicunqimode.WeiYiBiaoShi, DuCanShuType.使能);
                }
                if (JiCunQiType.ContainsKey(mjicunqimode.WeiYiBiaoShi) == false)
                {
                    JiCunQiType.Add(mjicunqimode.WeiYiBiaoShi, XieCaoZuoType.Zhou使能);
                }
                jicunqis.Add(mjicunqimode);
            }
            //位置
            {
                JiCunQiModel mjicunqimode = new JiCunQiModel();
                mjicunqimode.WeiYiBiaoShi = $"{kaname}位置";
                mjicunqimode.SheBeiID = SheBeiID;
                mjicunqimode.MiaoSu = $"该寄存器是读:0:1:pus:采用比较大小";
                QiTaJiCunQiDu.Add(mjicunqimode);
               
                if (JiCunQiDuType.ContainsKey(mjicunqimode.WeiYiBiaoShi) == false)
                {
                    JiCunQiDuType.Add(mjicunqimode.WeiYiBiaoShi, DuCanShuType.位置);
                }
               
                jicunqis.Add(mjicunqimode);
            }
            //速度
            {
                JiCunQiModel mjicunqimode = new JiCunQiModel();
                mjicunqimode.WeiYiBiaoShi = $"{kaname}速度";
                mjicunqimode.SheBeiID = SheBeiID;
                mjicunqimode.MiaoSu = $"该寄存器是读:0:1:pus:采用比较大小";
                QiTaJiCunQiDu.Add(mjicunqimode);
                QiTaJiCunQiXie.Add(mjicunqimode);
                if (JiCunQiDuType.ContainsKey(mjicunqimode.WeiYiBiaoShi) == false)
                {
                    JiCunQiDuType.Add(mjicunqimode.WeiYiBiaoShi, DuCanShuType.速度);
                }
                if (JiCunQiType.ContainsKey(mjicunqimode.WeiYiBiaoShi) == false)
                {
                    JiCunQiType.Add(mjicunqimode.WeiYiBiaoShi, XieCaoZuoType.Zhou速度);
                }
                jicunqis.Add(mjicunqimode);
            }
            //轴状态
            {
                JiCunQiModel mjicunqimode = new JiCunQiModel();
                mjicunqimode.WeiYiBiaoShi = $"{kaname}轴状态";
                mjicunqimode.SheBeiID = SheBeiID;
                mjicunqimode.MiaoSu = $"该寄存器是读:0:1:无单位:字符串";
                QiTaJiCunQiDu.Add(mjicunqimode);

                if (JiCunQiDuType.ContainsKey(mjicunqimode.WeiYiBiaoShi) == false)
                {
                    JiCunQiDuType.Add(mjicunqimode.WeiYiBiaoShi, DuCanShuType.轴状态);
                }

                jicunqis.Add(mjicunqimode);
            }
            //报警
            {
                JiCunQiModel mjicunqimode = new JiCunQiModel();
                mjicunqimode.WeiYiBiaoShi = $"{kaname}报警";
                mjicunqimode.SheBeiID = SheBeiID;
                mjicunqimode.MiaoSu = $"该寄存器是读:0:1:无单位:1是报警 0是无报警";
                QiTaJiCunQiDu.Add(mjicunqimode);

                if (JiCunQiDuType.ContainsKey(mjicunqimode.WeiYiBiaoShi) == false)
                {
                    JiCunQiDuType.Add(mjicunqimode.WeiYiBiaoShi, DuCanShuType.超限报警);
                }
                jicunqis.Add(mjicunqimode);
            }
            //位置清零
            {
                JiCunQiModel mjicunqimode = new JiCunQiModel();
                mjicunqimode.WeiYiBiaoShi = $"{kaname}位置清零";
                mjicunqimode.SheBeiID = SheBeiID;
                mjicunqimode.MiaoSu = $"该寄存器是写:--:--:无单位: 把轴的位置清零";
                QiTaJiCunQiXie.Add(mjicunqimode);

                if (JiCunQiType.ContainsKey(mjicunqimode.WeiYiBiaoShi) == false)
                {
                    JiCunQiType.Add(mjicunqimode.WeiYiBiaoShi, XieCaoZuoType.Zhou位置清零);
                }
                jicunqis.Add(mjicunqimode);
            }
            //运动状态
            {
                JiCunQiModel mjicunqimode = new JiCunQiModel();
                mjicunqimode.WeiYiBiaoShi = $"{kaname}运动状态";
                mjicunqimode.SheBeiID = SheBeiID;
                mjicunqimode.MiaoSu = $"该寄存器是读:0:1:无单位:采用比较关系0表示运动 1表示停止";
                QiTaJiCunQiDu.Add(mjicunqimode);

                if (JiCunQiDuType.ContainsKey(mjicunqimode.WeiYiBiaoShi) == false)
                {
                    JiCunQiDuType.Add(mjicunqimode.WeiYiBiaoShi, DuCanShuType.运动状态);
                }
                jicunqis.Add(mjicunqimode);
            }
            //轴停止
            {
                JiCunQiModel mjicunqimode = new JiCunQiModel();
                mjicunqimode.WeiYiBiaoShi = $"{kaname}轴停止";
                mjicunqimode.SheBeiID = SheBeiID;
                mjicunqimode.MiaoSu = $"该寄存器是写:--:--:无单位:轴停止";
                QiTaJiCunQiXie.Add(mjicunqimode);

                if (JiCunQiType.ContainsKey(mjicunqimode.WeiYiBiaoShi) == false)
                {
                    JiCunQiType.Add(mjicunqimode.WeiYiBiaoShi, XieCaoZuoType.Zhou停止);
                }
                jicunqis.Add(mjicunqimode);
            }
            //绝对位置写
            {
                JiCunQiModel mjicunqimode = new JiCunQiModel();
                mjicunqimode.WeiYiBiaoShi = $"{kaname}绝对位置写";
                mjicunqimode.SheBeiID = SheBeiID;
                mjicunqimode.MiaoSu = $"该寄存器是写:--:--:无单位:轴去绝对的位置";
                QiTaJiCunQiXie.Add(mjicunqimode);

                if (JiCunQiType.ContainsKey(mjicunqimode.WeiYiBiaoShi) == false)
                {
                    JiCunQiType.Add(mjicunqimode.WeiYiBiaoShi, XieCaoZuoType.ZhouJ位置);
                }
                jicunqis.Add(mjicunqimode);
            }
            //ZhouX位置
            {
                JiCunQiModel mjicunqimode = new JiCunQiModel();
                mjicunqimode.WeiYiBiaoShi = $"{kaname}相对位置写";
                mjicunqimode.SheBeiID = SheBeiID;
                mjicunqimode.MiaoSu = $"该寄存器是写:--:--:无单位:轴去相对的位置";
                QiTaJiCunQiXie.Add(mjicunqimode);

                if (JiCunQiType.ContainsKey(mjicunqimode.WeiYiBiaoShi) == false)
                {
                    JiCunQiType.Add(mjicunqimode.WeiYiBiaoShi, XieCaoZuoType.ZhouX位置);
                }
                jicunqis.Add(mjicunqimode);
            }
            //恒速
            {
                JiCunQiModel mjicunqimode = new JiCunQiModel();
                mjicunqimode.WeiYiBiaoShi = $"{kaname}恒速";
                mjicunqimode.SheBeiID = SheBeiID;
                mjicunqimode.MiaoSu = $"该寄存器是写:--:--:无单位:轴恒速运动";
                QiTaJiCunQiXie.Add(mjicunqimode);

                if (JiCunQiType.ContainsKey(mjicunqimode.WeiYiBiaoShi) == false)
                {
                    JiCunQiType.Add(mjicunqimode.WeiYiBiaoShi, XieCaoZuoType.Zhou恒速);
                }
                jicunqis.Add(mjicunqimode);
            }
            //回零
            {
                JiCunQiModel mjicunqimode = new JiCunQiModel();
                mjicunqimode.WeiYiBiaoShi = $"{kaname}回零";
                mjicunqimode.SheBeiID = SheBeiID;
                mjicunqimode.MiaoSu = $"该寄存器是写:--:--:无单位:轴回零";
                QiTaJiCunQiXie.Add(mjicunqimode);

                if (JiCunQiType.ContainsKey(mjicunqimode.WeiYiBiaoShi) == false)
                {
                    JiCunQiType.Add(mjicunqimode.WeiYiBiaoShi, XieCaoZuoType.Zhou回零);
                }
                jicunqis.Add(mjicunqimode);
            }
            {
                JiCunQiModel mjicunqimode = new JiCunQiModel();
                mjicunqimode.WeiYiBiaoShi = "写热复位";
                mjicunqimode.SheBeiID = SheBeiID;
                mjicunqimode.MiaoSu = $"该寄存器是写:0:1:无单位:写热复位";
                QiTaJiCunQiXie.Add(mjicunqimode);
                if (JiCunQiType.ContainsKey(mjicunqimode.WeiYiBiaoShi) == false)
                {
                    JiCunQiType.Add(mjicunqimode.WeiYiBiaoShi, XieCaoZuoType.热复位);
                }
                jicunqis.Add(mjicunqimode);
            }

            return jicunqis;
        }

    }

    public enum DuCanShuType
    { 
        位置,
        速度,
        使能,
        轴状态,
        运动状态,
        超限报警,
        DuIO,
        XieIO,
        总线状态,
        Wu,
    }

    public enum XieCaoZuoType
    {
        ZhouX位置,
        ZhouJ位置,
        Zhou速度,
        Zhou恒速,
        Zhou使能,
        Zhou报警清除,
        Zhou位置清零,
        Zhou停止,
        Zhou回零,
        硬件复位,
        热复位,
        所有轴停止,
        所有轴回零,
        XieIO,
        Wu,
    }

}
