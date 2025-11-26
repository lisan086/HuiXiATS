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
            for (int c = 0; c < LisSheBei.Count; c++)
            {
                SaoMaModel shebei = LisSheBei[c];
                {
                    List<string> lis = ChangYong.MeiJuLisName(typeof(CunType));
                    for (int i = 0; i < lis.Count; i++)
                    {
                        JiCunQiModel model = new JiCunQiModel();
                        model.SheBeiID = SheBeiID;
                        
                        if (lis[i].ToLower().Contains("du"))
                        {                       
                            model.WeiYiBiaoShi = $"{shebei.Name}:{lis[i]}";                      
                            model.MiaoSu = ChangYong.GetEnumDescription(ChangYong.GetMeiJuZhi<CunType>(lis[i]));
                            model.DuXie = 1;
                            LisDu.Add(model);
                        }
                        else
                        {
                            model.WeiYiBiaoShi = $"{shebei.Name}:{lis[i]}";                           
                            model.MiaoSu = ChangYong.GetEnumDescription(ChangYong.GetMeiJuZhi<CunType>(lis[i]));
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
                            cunModel.Time = shebei.Time;
                            cunModel.JieShouCount = shebei.JieShouCount;
                            cunModel.IsXinXieYi = shebei.IsXinXieYi;
                            JiLu.Add(model.WeiYiBiaoShi, cunModel);
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
                    break;
                }
            }
        }

        public void SetJiCunQiValue(string weiyibiaoshi, List<byte> shuju)
        {
            if (JiLu.ContainsKey(weiyibiaoshi))
            {

                CunModel cunModel = JiLu[weiyibiaoshi];
                cunModel.JiCunQi.IsKeKao = true;
                if (cunModel.IsXinXieYi)
                {
                    cunModel.JiCunQi.Value = Encoding.UTF8.GetString(shuju.ToArray());
                }
                else
                {
                    cunModel.JiCunQi.Value = Encoding.Default.GetString(shuju.ToArray());
                }
            }

        }
  
        public void SetZhengZaiValue(string weiyibiaoshi,int sate)
        {
            if (JiLu.ContainsKey(weiyibiaoshi))
            {
                CunModel cunModel = JiLu[weiyibiaoshi];
                cunModel.JiCunQi.IsKeKao = true;
                cunModel.IsZhengZaiCe = sate;
              
            }

        }

        /// <summary>
        /// 1是成功 0是未测完 3 不存在 其他表示超时
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public CunModel IsChengGong(int zongid, CunType cunType)
        {
            foreach (var item in JiLu.Keys)
            {
                CunModel cunModel = JiLu[item];
                if (cunModel.IsDu == cunType && cunModel.ZongSheBeiId == zongid)
                {
                    return cunModel;
                }

            }

            return null;
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

        public JiCunQiModel GetModel(int zongid,CunType cunType)
        {
            foreach (var item in JiLu.Keys)
            {
                CunModel cunModel = JiLu[item];
                if (cunModel.IsDu == cunType && cunModel.ZongSheBeiId == zongid)
                {
                    return cunModel.JiCunQi;
                }

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


        public byte[] GetZhiLing(CunModel model,out bool fanhui)
        {
            fanhui = true;
            if (model.IsDu == CunType.XieCheLianHuanXing)
            {
                //erweima#huanxingtype#gongweihao#jieshoucount
                if (model.IsXinXieYi)
                {
                    string[] fenge = model.JiCunQi.Value.ToString().Split('#');
                    if (fenge.Length > 3)
                    {
                        XieShiJueModel shijuemodel = new XieShiJueModel();
                        shijuemodel.function = 2;
                        shijuemodel.position.sn = fenge[0];
                        shijuemodel.position.id = fenge[1];
                        shijuemodel.position.flowname = ChangYong.TryInt(fenge[2],0);
                        shijuemodel.position.testsn = 0;
                        model.JieShouCount = ChangYong.TryInt(fenge[3], 2);
                        byte[] data = Encoding.UTF8.GetBytes(ChangYong.HuoQuJsonStr(shijuemodel));
                        return data;
                    }
                }
                else
                {
                    //erweima#huanxingtype#gongweihao#jieshoucount
                    string[] fenge = model.JiCunQi.Value.ToString().Split('#');
                    if (fenge.Length > 3)
                    {
                        JiuHuanXingModel shijuemodel = new JiuHuanXingModel();
                        shijuemodel.方法名 = 2;
                        shijuemodel.参数.型号 = fenge[1];
                        shijuemodel.参数.测试序号 = 0;
                        shijuemodel.参数.工位号 = ChangYong.TryInt(fenge[2], 0);
                        shijuemodel.参数.序列号 = fenge[0];
                      
                        model.JieShouCount = ChangYong.TryInt(fenge[3], 2);
                        byte[] data = Encoding.UTF8.GetBytes(ChangYong.HuoQuJsonStr(shijuemodel));
                        return data;
                    }
                }
            }
            else if (model.IsDu == CunType.XieCheLianXunXu)
            {
                if (model.IsXinXieYi)
                {
                    //erweima#ceshimingceng#xuhao#gongweihao#jieshoucount
                    string[] fenge = model.JiCunQi.Value.ToString().Split('#');
                    if (fenge.Length > 4)
                    {
                        XieShiJueModel shijuemodel = new XieShiJueModel();
                        shijuemodel.function = 1;
                        shijuemodel.position.sn = fenge[0];
                        shijuemodel.position.id = DateTime.Now.ToString("yyyyMMddHHmmss");
                        shijuemodel.position.flowname  = ChangYong.TryInt(fenge[1], 0);
                        shijuemodel.position.testsn = ChangYong.TryInt(fenge[2], -1);
                        model.JieShouCount = ChangYong.TryInt(fenge[4], 2);
                        byte[] data = Encoding.UTF8.GetBytes(ChangYong.HuoQuJsonStr(shijuemodel));
                        return data;
                    }
                }
                else
                {
                    // //erweima#ceshimingceng#xuhao#gongweihao#jieshoucount
                    string[] fenge = model.JiCunQi.Value.ToString().Split('#');
                    if (fenge.Length > 4)
                    {
                        JiuHuanXingModel shijuemodel = new JiuHuanXingModel();
                        shijuemodel.方法名 = 1;
                        shijuemodel.参数.型号 = "";
                        shijuemodel.参数.测试序号 = ChangYong.TryInt(fenge[2],0);
                        shijuemodel.参数.工位号 =0;
                        shijuemodel.参数.序列号 = fenge[0];

                        model.JieShouCount = ChangYong.TryInt(fenge[4], 2);
                        byte[] data = Encoding.UTF8.GetBytes(ChangYong.HuoQuJsonStr(shijuemodel));
                        return data;
                    }
                }
            }
            else
            {
                byte[] data = Encoding.UTF8.GetBytes(model.JiCunQi.Value.ToString());
                return data;
            }
           
          
            return null;       
        }

       
    }
}
