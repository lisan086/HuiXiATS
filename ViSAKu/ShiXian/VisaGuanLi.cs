using SSheBei.ABSSheBei;
using SSheBei.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViSAKu.Frm;
using SSheBei.LianJieQi;
using System.Collections.Concurrent;
using System.Threading;
using ViSAKu.Model;
using CommLei.GongYeJieHe;
using CommLei.JiChuLei;
using System.Data.SqlClient;
using System.IO;
using JieMianLei.FuFrom.KJ;
using System.Reflection;
using ViSaJiChu;
using System.Resources;

namespace ViSAKu.ShiXian
{
    /// <summary>
    /// 所有ViSA管理
    /// </summary>
    public class VisaGuanLi : ABSNSheBei
    {

        /// <summary>
        /// visa管理
        /// </summary>
        private VisaGuanXin VisaGuanXin;
        private PeiZhiLei PeiZhiLei;
        /// <summary>
        /// 线程总开关
        /// </summary>
        private bool ZongKaiGuan = true;
        /// <summary>
        /// true  表示线程开始工作
        /// </summary>
        private bool DengDaiOpen = false;
        public override string SheBeiType
        {
            get
            {
                return "VISA管理";
            }
        }

        public override string BanBenHao
        {
            get
            {
                return "V1.0";
            }
        }

        /// <summary>
        /// 不起作用
        /// </summary>
        public override bool TongXin
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// 发送指令
        /// </summary>
        private FanXingJiHeLei<DataLieModel> SendZhiLing = new FanXingJiHeLei<DataLieModel>();

        public VisaGuanLi()
        {
            PeiZhiLei = new PeiZhiLei();
            VisaGuanXin = new VisaGuanXin();
            VisaGuanXin.MsgEvent += VisaGuanXin_MsgEvent;
        }

        private void VisaGuanXin_MsgEvent(string arg1, int arg2)
        {
            ChuFaMsg(arg2==2?MsgDengJi.SheBeiCuoWu: MsgDengJi.SheBeiZhengChang, arg1);
        }

        public override void IniData(bool ispeizhi)
        {
       
            PeiZhiLei.IsPeiZhi = ispeizhi;
            PeiZhiLei.WenJianName = PeiZhiObjName;
            GetVisaNames(true);       
            if (ispeizhi == false)
            {
                PeiZhiLei.IniData(SheBeiID,SheBeiName);
                PeiZhiLei.XieIOEvent += PeiZhiLei_XieIOEvent;
                PeiZhiLei.VisaGuanXin=VisaGuanXin;
                Thread xiancheng = new Thread(ReadWork);
                xiancheng.IsBackground = true;
                xiancheng.DisableComObjectEagerCleanup();
                xiancheng.Start();           
            }
        }

        private string PeiZhiLei_XieIOEvent(JiCunQiModel obj)
        {
            if (obj==null)
            {
                return "传来的对象是空";
            }
            DataLieModel liemode = PeiZhiLei.DataMoXing.GetModel(obj);
            if (liemode!=null)
            {
                DateTime shijian = DateTime.Now;
                int chaottime = 2000;
              
                PeiZhiLei.DataMoXing.SetXiWan(liemode.JiCunQiModel.WeiYiBiaoShi,0);
                if (liemode.IsDu!=CunType.DuShuJu)
                {
                    JiCunQiModel model = new JiCunQiModel();
                    model.WeiYiBiaoShi = obj.WeiYiBiaoShi;
                    model.Value = obj.Value;                 
                    XieShuJu(new List<JiCunQiModel>() { model });
                }             
                for (; ZongKaiGuan;)
                {
                    JiaoYanJieGuoModel modesss = JiaoYanChengGong(obj);
                    if (modesss.IsZuiZhongJieGuo != JieGuoType.JingXingZhong)
                    {
                        double haomiao = (DateTime.Now - shijian).TotalMilliseconds;
                        return $"用时:{haomiao}ms {modesss.Value}";
                    }
                    if ((DateTime.Now - shijian).TotalMilliseconds >= chaottime)
                    {
                        double haomiao = (DateTime.Now - shijian).TotalMilliseconds;
                        return $"超时退出:{haomiao}ms {modesss.Value}";
                    }
                    Thread.Sleep(1);
                }
            }
            return "没有找到";
        }

        public override void Open()
        {
            List<SheBeiVisaModel> lis = PeiZhiLei.DataMoXing.LisSheBei;
            for (int i = 0; i < lis.Count; i++)
            {
                
                bool zhen = OpenResourceN(lis[i].LianJieName);
                if (zhen)
                {
                    lis[i].IsConnect = true;
                    PeiZhiLei.DataMoXing.SetTxState(lis[i].LianJieName, true);
                    ChuFaMsg(MsgDengJi.SheBeiZhengChang, $"{lis[i].Name}，连接成功");
                }
                else
                {
                    lis[i].IsConnect = false;
                    PeiZhiLei.DataMoXing.SetTxState(lis[i].LianJieName, false);
                    ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"{lis[i].Name}，连接失败");
                }
            }
            DengDaiOpen = true;
        }

        public override void Close()
        {
            ZongKaiGuan = false;         
            Thread.Sleep(100);
            VisaGuanXin.Close();
          
        }

        public override List<JiCunQiModel> PeiZhiDuXie(int type)
        {
            List<JiCunQiModel> shuju = new List<JiCunQiModel>();
            if (type == 1)
            {
                List<JiCunQiModel> Get = PeiZhiLei.DataMoXing.LisDu;
                shuju = Get;
            }
            else if (type == 2)
            {
                List<JiCunQiModel> Get = PeiZhiLei.DataMoXing.LisXie;
                shuju = Get;
            }
            else if (type == 3)
            {
                List<JiCunQiModel> Get = PeiZhiLei.DataMoXing.LisDuXie;
                shuju = Get;
            }
            return shuju;

        }
        /// <summary>
        /// 没有数据的
        /// </summary>
        /// <returns></returns>
        public override List<JiCunQiModel> GetShuJu()
        {
           
            List<JiCunQiModel> Get = PeiZhiLei.DataMoXing.LisDu;
         
            return Get;
        }

        public override JieMianFrmModel GetFrm(bool istiaoshi)
        {
            PeiZhiLei.IsTiaoShi = istiaoshi;
            JieMianFrmModel jieMianFrmModel = new JieMianFrmModel();
            jieMianFrmModel.SheBeiName = SheBeiName;
            jieMianFrmModel.SheBeiID = SheBeiID;
            jieMianFrmModel.Form = new PeiZhiFrm(PeiZhiLei);
            return jieMianFrmModel;
        }
     
        public override JiaoYanJieGuoModel JiaoYanChengGong(JiCunQiModel jicunqiid)
        {
            JiaoYanJieGuoModel models = new JiaoYanJieGuoModel();
            models.WeiYiBiaoShi = jicunqiid.WeiYiBiaoShi;
            models.SheBeiID = jicunqiid.SheBeiID;
            DataLieModel xinr = PeiZhiLei.DataMoXing.GetModel(jicunqiid);
            if (xinr != null)
            {          
                if (xinr.JiCunQiModel.IsKeKao)
                {
                    if (xinr.IsXieWan == 1)
                    {
                        models.Value = xinr.JiCunQiModel.Value;               
                        models.IsZuiZhongJieGuo = JieGuoType.ChengGongJiGuo;
                    }
                    else if (xinr.IsXieWan == 2)
                    {
                        models.Value = xinr.JiCunQiModel.Value;
                        models.IsZuiZhongJieGuo = JieGuoType.ShiBaiJiGuo;
                    }
                    else
                    {
                        models.Value = "";
                        models.IsZuiZhongJieGuo = JieGuoType.JingXingZhong;
                    }
                }
                else
                {
                    models.Value = "通信不成功，不可靠";
                    models.IsZuiZhongJieGuo = JieGuoType.BuKeKaoJieGuo;
                }
            }
            else
            {
                models.Value = "没有找到";
                models.IsZuiZhongJieGuo = JieGuoType.MeiZhaoDaoJiGuo;
            }
            return models;

        }

        public override void XieShuJu(List<JiCunQiModel> canshus)
        {
            if (ZongKaiGuan)
            {
                List<DataLieModel> lismodel = new List<DataLieModel>();

                for (int i = 0; i < canshus.Count; i++)
                {
                    DataLieModel model=PeiZhiLei.DataMoXing.GetModel(canshus[i]);
                    if (model != null&&model.IsDu!=CunType.DuShuJu)
                    {
                        model.JiCunQiModel.Value = "";
                        PeiZhiLei.DataMoXing.SetXiWan(canshus[i].WeiYiBiaoShi, 0);
                        DataLieModel xinmodel = model.FuZhi();
                        xinmodel.JiCunQiModel=canshus[i].FuZhi();
                        lismodel.Add(xinmodel);
                    }
                }
                SendZhiLing.Add(lismodel.ToArray());
            }
        }


        /// <summary>
        /// 用于检测设备
        /// </summary>
        private void ReadWork()
        {
            DateTime shijian = DateTime.Now;
            DateTime dushutime = DateTime.Now;
            while (ZongKaiGuan)
            {
                if (DengDaiOpen == false)
                {
                  
                    Thread.Sleep(20);
                    continue;
                }
                try
                {
                    if ((DateTime.Now - shijian).TotalMilliseconds >= 2000)
                    {
                        VisaGuanXin.ShuaXinShuJu();
                        shijian = DateTime.Now;
                    }
                    
                }
                catch
                {


                }
                try
                {
                    int count = SendZhiLing.GetCount();
                    if (count>0)
                    {
                        if (count>5)
                        {
                            count = 5;
                        }
                        for (int i = 0; i < count; i++)
                        {
                            DataLieModel model = SendZhiLing.GetModel_Head_RomeHead();
                            WriteRec(model);
                            Thread.Sleep(model.XieYanShi);
                        }
                       
                    }
                }
                catch 
                {

                  
                }
                try
                {
                    if ((DateTime.Now - dushutime).TotalMilliseconds >= 150)
                    {
                        DuCanShu();
                        dushutime = DateTime.Now;
                    }
                }
                catch
                {


                }
                Thread.Sleep(5);
            }
        }

        /// <summary>
        /// 获取VisaName
        /// </summary>
        /// <returns></returns>
        private List<string> GetVisaNames(bool iszengpeizhi)
        {
            if (iszengpeizhi)
            {
                PeiZhiLei.LianJieNames.Clear();
            }
            try
            {
                bool zhenyou = true;
                List<string> wenjian = VisaGuanXin.GetVisaNames(out zhenyou);
                foreach (string s in wenjian)
                {
                    if (iszengpeizhi)
                    {
                        PeiZhiLei.LianJieNames.Add(s);
                    }
                   
                }
                return wenjian;
            }
            catch (System.ArgumentException ex)
            {
                ChuFaMsg(MsgDengJi.SheBeiCuoWu, string.Format("查找设备 {0},{1}", ex.Source, ex.Message));
            }
            return new List<string>();
            
        }

        /// <summary>
        /// 连接设备
        /// </summary>
        /// <param name="strResourceName"></param>
        private bool OpenResourceN(string strResourceName)
        {
            try
            {
                bool zhjen=  VisaGuanXin.OpenResourceN(strResourceName);
                return zhjen;
            }         
            catch (Exception ex)
            {
                ChuFaMsg(MsgDengJi.SheBeiCuoWu, string.Format("建立连接 {0},{1}", ex.Source, ex.Message));
            }

            return false;
        }

        /// <summary>
        /// 写入有返回的，读取和写失败会返回null,否则饭回数据
        /// </summary>
        /// <param name="visaname"></param>
        /// <param name="strcommand"></param>    
        /// <returns></returns>
        private  void WriteRec(DataLieModel model)
        {
         
            string lieModel = GetZhuanHuanModel(model);
            if (string.IsNullOrEmpty(lieModel)==false )
            {
                PeiZhiLei.DataMoXing.SetXiWan(model.JiCunQiModel.WeiYiBiaoShi, 0);
                bool isfanhi = false;
                switch (model.IsDu)
                {
                    case CunType.XieYouCanShuFanHui:
                        isfanhi = true;
                        break;
                    case CunType.XieYouCanShuWuFanHui:
                        isfanhi = false;
                        break;
                    case CunType.XieWuCanShuFanHui:
                        isfanhi = true;
                        break;
                    case CunType.XieWuCanShuWuFanHui:
                        isfanhi = false;
                        break;
                    default:
                        break;
                }
                {
                    string zhiling = string.Format(model.CMD, model.JiCunQiModel.Value);
                    bool zhen = false;
                    string jieguo = XieShuJu(lieModel, zhiling, isfanhi, out zhen);
                    if (zhen)
                    {
                        PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQiModel.WeiYiBiaoShi, jieguo);
                        PeiZhiLei.DataMoXing.SetXiWan(model.JiCunQiModel.WeiYiBiaoShi, 1);
                    }
                    else
                    {
                        PeiZhiLei.DataMoXing.SetXiWan(model.JiCunQiModel.WeiYiBiaoShi, 2);
                    }
                    ChuFaMsg(MsgDengJi.SheBeiXie, string.Format(" {0}该标识写入:{1}  返回,{2} ", model.JiCunQiModel.WeiYiBiaoShi, zhiling, jieguo));
                    return;
                }


            }
            else
            {
                ChuFaMsg(MsgDengJi.SheBeiXie, string.Format(" {0}该标识不存在", model.JiCunQiModel.WeiYiBiaoShi));
            }
            PeiZhiLei.DataMoXing.SetXiWan(model.JiCunQiModel.WeiYiBiaoShi, 2);
        }


        /// <summary>
        /// 获取转换model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string GetZhuanHuanModel(DataLieModel model)
        {
            string index = "";
            List<SheBeiVisaModel> lis = PeiZhiLei.DataMoXing.LisSheBei;
            for (int i = 0; i < lis.Count; i++)
            {
                if (lis[i].SheBeiID == model.SheBeiID)
                {
                    index = lis[i].LianJieName;
                    return index;
                }

            }
            return index;
        }

     
        private void DuCanShu()
        {
            ParallelLoopResult jieguo= Parallel.ForEach(PeiZhiLei.DataMoXing.LisSheBei, (x) => {
                try
                {
                    if (VisaGuanXin.IsTX(x.LianJieName))
                    {
                        int count = x.LisData.Count;
                        for (int i = 0; i < count; i++)
                        {
                            bool isxie = x.LisData[i].IsDu==CunType.DuShuJu;
                            if (isxie)
                            {
                                bool zhens = false;
                                string jieguo2 = XieShuJu(x.LianJieName, x.LisData[i].CMD,true,out zhens);

                                if (zhens)
                                {
                                    PeiZhiLei.DataMoXing.SetJiCunQiValue(x.LisData[i].JiCunQiModel.WeiYiBiaoShi, jieguo2);
                                    PeiZhiLei.DataMoXing.SetXiWan(x.LisData[i].JiCunQiModel.WeiYiBiaoShi, 1);
                                }
                                else
                                {
                                    PeiZhiLei.DataMoXing.SetXiWan(x.LisData[i].JiCunQiModel.WeiYiBiaoShi, 2);
                                }
                            }
                          
                        }
                    }
                    else
                    {
                        PeiZhiLei.DataMoXing.SetTxState(x.LianJieName,false);
                    }
                }
                catch(Exception ex)
                {

                    ChuFaMsg(MsgDengJi.SheBeiCuoWu,$"{x.LianJieName} 读错误:{ex.Message}");
                }
               
            });

        }

        private string XieShuJu(string lianjiename,string cmd,bool isfanhui,out bool jieguo)
        {
            try
            {
                jieguo = false;
                bool ixecheng = VisaGuanXin.Write(lianjiename, cmd);
                if (ixecheng)
                {
                    if (isfanhui)
                    {
                        string jiestr = VisaGuanXin.Read(lianjiename);
                        jieguo = string.IsNullOrEmpty(jiestr) == false;
                        return jiestr;
                    }
                    jieguo = true;
                    return "";
                }
            }
            catch
            {
                jieguo = false;

            }
            
           
            return "";
        }
        public override TxModel GetMeiGeTx()
        {
            TxModel model = new TxModel();
            model.SheBeiName = SheBeiName;
            model.SheBeiTD = SheBeiID;
            model.SheBeuZu = FenZu;
            bool ischengg = true;

            for (int i = 0; i < PeiZhiLei.DataMoXing.LisSheBei.Count; i++)
            {
                SheBeiVisaModel item = PeiZhiLei.DataMoXing.LisSheBei[i];
                ZiTxModel zmodel = new ZiTxModel();
                zmodel.Tx = item.IsConnect;
                zmodel.ZiSheBeiID = item.SheBeiID;
                zmodel.ZiSheBeiName = item.Name;
                if (zmodel.Tx == false)
                {
                    ischengg = false;
                }

                model.LisTx.Add(zmodel);

            }
            model.ZongTX = ischengg;
            return model;
        }
        public override void Clear(bool isquanbu, JiCunQiModel model)
        {
            
        }
    }
}
