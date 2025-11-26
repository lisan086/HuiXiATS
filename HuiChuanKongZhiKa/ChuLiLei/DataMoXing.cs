using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using CommLei.DataChuLi;
using CommLei.JiChuLei;
using LeiSaiDMC.Frm.KJ;
using LeiSaiDMC.Model;
using SSheBei.CRCJiaoYan;
using SSheBei.Model;

namespace LeiSaiDMC.Frm
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
        public List<LSModel> LisSheBei = new List<LSModel>();

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
            LisSheBei = JosnOrSModel.GetLisTModel<LSModel>();
            if (LisSheBei == null)
            {
                LisSheBei = new List<LSModel>();
            }
            Dictionary<int, List<int>> leixing = new Dictionary<int, List<int>>();
            for (int c = 0; c < LisSheBei.Count; c++)
            {
                LSModel shebei = LisSheBei[c];
                //轴
                {
                    shebei.LisZhouModel.Sort((x, y) => {
                        if (x.ZhouNO > y.ZhouNO)
                        {
                            return 1;
                        }
                        else
                        {
                            return -1;
                        }
                    });
                    for (int f = 0; f < shebei.LisZhouModel.Count; f++)
                    {
                        ZhouModel zhouModel = shebei.LisZhouModel[f];
                        zhouModel.SheBeiID = shebei.SheBeiID;
                        List<string> quanju = ChangYong.MeiJuLisName(typeof(IOType));
                        for (int i = 0; i < quanju.Count; i++)
                        {
                            if (quanju[i].StartsWith("Zhou"))
                            {
                                CunModel cunmodel = new CunModel();
                                cunmodel.SheBeiID = shebei.SheBeiID;
                                cunmodel.ZhouOrIOID = zhouModel.ZhouNO;
                                cunmodel.CanShuType = CanShuType.DuShuJu;
                                cunmodel.IOTYpe = ChangYong.GetMeiJuZhi<IOType>(quanju[i]);
                                cunmodel.IsWanCheng = 1;
                                JiCunQiModel model = new JiCunQiModel();
                                model.WeiYiBiaoShi = $"{zhouModel.ZhouName}-{quanju[i]}";
                                model.SheBeiID = SheBeiID;
                                model.MiaoSu = ChangYong.GetEnumDescription(cunmodel.CanShuType);
                                model.DuXie = 1;
                                LisDu.Add(model);
                                LisDuXie.Add(model);
                                if (JiLu.ContainsKey(model.WeiYiBiaoShi) == false)
                                {
                                    cunmodel.JiCunQi = model;
                                    JiLu.Add(model.WeiYiBiaoShi, cunmodel);
                                }
                            }                         
                        }                      
                    }
                }
                //IO
                {
                    shebei.IOS.Sort((x, y) => {
                        if (x.BitNo > y.BitNo)
                        {
                            return 1;
                        }
                        else
                        {
                            return -1;
                        }
                    });
                    for (int f = 0; f < shebei.IOS.Count; f++)
                    {
                        IOModel zhouModel = shebei.IOS[f];
                        zhouModel.SheBeiID = shebei.SheBeiID;
                        if (zhouModel.IOLeiXing == 1)
                        {
                            CunModel cunmodel = new CunModel();
                            cunmodel.SheBeiID = shebei.SheBeiID;
                            cunmodel.ZhouOrIOID = zhouModel.BitNo;
                            cunmodel.CanShuType = CanShuType.DuIO;
                            cunmodel.IOTYpe = IOType.DuIO;
                            cunmodel.IsWanCheng = 1;
                            JiCunQiModel model = new JiCunQiModel();
                            model.WeiYiBiaoShi = $"{shebei.Name}-{zhouModel.IOName}";
                            model.SheBeiID = SheBeiID;
                            model.MiaoSu = ChangYong.GetEnumDescription(cunmodel.CanShuType);
                            model.DuXie = 1;
                            LisDu.Add(model);
                            LisDuXie.Add(model);
                            if (JiLu.ContainsKey(model.WeiYiBiaoShi) == false)
                            {
                                cunmodel.JiCunQi = model;
                                JiLu.Add(model.WeiYiBiaoShi, cunmodel);
                            }
                        }
                        else
                        {
                            CunModel cunmodel = new CunModel();
                            cunmodel.SheBeiID = shebei.SheBeiID;
                            cunmodel.ZhouOrIOID = zhouModel.BitNo;
                            cunmodel.CanShuType = CanShuType.DuXieIO;
                            cunmodel.IOTYpe = IOType.DuXieIO;
                            cunmodel.IsWanCheng = 0;
                            JiCunQiModel model = new JiCunQiModel();
                            model.WeiYiBiaoShi = $"{shebei.Name}-{zhouModel.IOName}";
                            model.SheBeiID = SheBeiID;
                            model.MiaoSu = ChangYong.GetEnumDescription(cunmodel.CanShuType);
                            model.DuXie = 3;
                            LisDu.Add(model);
                            LisXie.Add(model);
                            LisDuXie.Add(model);
                            if (JiLu.ContainsKey(model.WeiYiBiaoShi) == false)
                            {
                                cunmodel.JiCunQi = model;
                                JiLu.Add(model.WeiYiBiaoShi, cunmodel);
                            }
                        }

                    }
                }
                //全局
                {
                    List<string> quanju = ChangYong.MeiJuLisName(typeof(CanShuType));
                    for (int i = 0; i < quanju.Count; i++)
                    {
                        if (quanju[i].StartsWith("Xie"))
                        {
                            CunModel cunmodel = new CunModel();
                            cunmodel.SheBeiID = shebei.SheBeiID;
                            cunmodel.ZhouOrIOID = -1;
                            cunmodel.CanShuType = ChangYong.GetMeiJuZhi<CanShuType>(quanju[i]);
                            cunmodel.IsWanCheng = 0;
                            cunmodel.IOTYpe = IOType.Wu;
                            JiCunQiModel model = new JiCunQiModel();
                            model.WeiYiBiaoShi = $"{quanju[i]}";
                            model.SheBeiID = SheBeiID;
                            model.MiaoSu = ChangYong.GetEnumDescription(cunmodel.CanShuType);
                            model.DuXie = 1;
                            LisXie.Add(model);
                            LisDuXie.Add(model);
                            if (JiLu.ContainsKey(model.WeiYiBiaoShi) == false)
                            {
                                cunmodel.JiCunQi = model;
                                JiLu.Add(model.WeiYiBiaoShi, cunmodel);
                            }
                        }

                    }
                }
               
            }          
            KeyS = JiLu.Keys.ToList();
        }

        public void SetTX(int shebeiid, bool zhuangtai)
        {
            for (int i = 0; i < LisSheBei.Count; i++)
            {
                if (LisSheBei[i].SheBeiID == shebeiid)
                {
                    LisSheBei[i].TX= zhuangtai;
                    break;
                }
            }
            for (int i = 0; i < KeyS.Count; i++)
            {
                CunModel cunModel = JiLu[KeyS[i]];
                if (cunModel.SheBeiID == shebeiid)
                {
                    cunModel.JiCunQi.IsKeKao = zhuangtai;
                }
            }
        }
        public void SetSate(CunModel model, int state)
        {
            if (JiLu.ContainsKey(model.JiCunQi.WeiYiBiaoShi))
            {
                CunModel xinmodel = JiLu[model.JiCunQi.WeiYiBiaoShi];
                xinmodel.IsWanCheng = state;
                if (state == 0)
                {
                    xinmodel.JiCunQi.Value = "";
                }
            }
        }

        public void SetXieJiCunQiZhi(CunModel model, object shuju)
        {
            if (JiLu.ContainsKey(model.JiCunQi.WeiYiBiaoShi))
            {
                CunModel xinmodel = JiLu[model.JiCunQi.WeiYiBiaoShi];
                xinmodel.JiCunQi.Value = shuju;
            }
        }

        public CunModel GetCunModel(JiCunQiModel model, bool isfuzhi)
        {
            if (JiLu.ContainsKey(model.WeiYiBiaoShi))
            {
                CunModel xinmodel = JiLu[model.WeiYiBiaoShi];
                if (isfuzhi)
                {
                    return xinmodel.FuZhi();
                }
                else
                {
                    return xinmodel;
                }
            }
            return null;
        }

        public List<CunModel> GetSheBeiJiCunQi(int shebeiid,int zhouno)
        {
            List<CunModel> lis = new List<CunModel>();
            for (int i = 0; i < KeyS.Count; i++)
            {
                CunModel cun = JiLu[KeyS[i]];
                if (cun.SheBeiID == shebeiid)
                {
                    if (cun.ZhouOrIOID == zhouno)
                    {
                        lis.Add(cun);
                    }
                }
            }
            return lis;
        }

        public ZhouModel GetZhou(int shebeiid, int zhouno)
        {
         
            for (int i = 0; i < LisSheBei.Count; i++)
            {
                LSModel cun = LisSheBei[i];
                if (cun.SheBeiID == shebeiid)
                {
                    List<ZhouModel> lisshuju = cun.LisZhouModel;
                    foreach (var item in lisshuju)
                    {
                        if (item.ZhouNO== zhouno)
                        {
                            return item;
                        }
                    }
                }
            }
            return null;
        }

      

        public void SetZhouDu(ZhouModel zhouhao, IOType oTYpe,object  zhi)
        {
            for (int i = 0; i < KeyS.Count; i++)
            {
                CunModel shujus= JiLu[KeyS[i]];
                if (shujus.SheBeiID== zhouhao.SheBeiID&& shujus.ZhouOrIOID== zhouhao.ZhouNO)
                {
                    if (shujus.IOTYpe== oTYpe)
                    {
                        shujus.JiCunQi.Value = zhi;
                        break;
                    }
                }
            }
        }

      
        public CunModel GetCunModel(int shebeiid,CanShuType canShuType)
        {
            for (int i = 0; i < KeyS.Count; i++)
            {
                CunModel cun = JiLu[KeyS[i]];
                if (cun.SheBeiID == shebeiid)
                {
                    if (cun.CanShuType== canShuType)
                    {
                        return cun.FuZhi();
                    }
                   
                }
            }
            return null;
        }
    }
}
