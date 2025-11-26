using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommLei.DataChuLi;
using CommLei.GongYeJieHe;
using CommLei.JiChuLei;
using SSheBei.ABSSheBei;
using SSheBei.CRCJiaoYan;
using SSheBei.LianJieQi;
using SSheBei.Model;
using YiBanSaoMaQi.Frm;
using YiBanSaoMaQi.Model;

namespace YiBanSaoMaQi.ShiXian
{
    public class ZhiLingSaoMaQiLei : ABSNSheBei
    {
        private PeiZhiLei PeiZhiLei;
        /// <summary>
        /// 线程总开关
        /// </summary>
        private bool ZongKaiGuan = false;
        /// <summary>
        /// true  表示线程开始工作
        /// </summary>
        private bool DengDaiOpen = false;


        public override string SheBeiType
        {
            get
            {
                return "PingIP";
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
        /// 通信不起作用
        /// </summary>
        public override bool TongXin
        {
            get
            {
              
                return true;
            }
        }


        private FanXingJiHeLei<CunModel> SengData = new FanXingJiHeLei<CunModel>();


        public ZhiLingSaoMaQiLei()
        {
            ZongKaiGuan = true;
            PeiZhiLei = new PeiZhiLei();
        }

        public override void IniData(bool ispeizhi)
        {

            PeiZhiLei.IsPeiZhi = ispeizhi;
            PeiZhiLei.WenJianName = PeiZhiObjName;

            if (ispeizhi == false)
            {
                PeiZhiLei.IniData(SheBeiID, SheBeiName);
                PeiZhiLei.XieIOEvent += ZhongShengIO_XieIOEvent;
                List<SaoMaModel> sModels = PeiZhiLei.DataMoXing.LisSheBei;
                Thread xiancheng = new Thread(ReadWork);
                xiancheng.IsBackground = true;
                xiancheng.DisableComObjectEagerCleanup();
                xiancheng.Start(sModels);
            }
        }

        private void ZhongShengIO_XieIOEvent(JiCunQiModel obj)
        {
            XieShuJu(new List<JiCunQiModel>() { obj });
        }

        public override void Open()
        {
            StringBuilder msg = new StringBuilder();
            msg.Append("Ping设备打开");
            PeiZhiLei.GetWangKa();
            DengDaiOpen = true;
            ChuFaMsg(MsgDengJi.SheBeiZhengChang, msg.ToString());

        }
        public override void Close()
        {
            ZongKaiGuan = false;
            
        }

        public override void XieShuJu(List<JiCunQiModel> canshus)
        {
            if (canshus == null || canshus.Count == 0)
            {
                return;
            }
            if (DengDaiOpen)
            {
                JieRu(canshus);
            }
        }
        private CunModel GetDiZhi(JiCunQiModel jicunqi)
        {
            Dictionary<string, CunModel> sModels = PeiZhiLei.DataMoXing.JiLu;
            if (sModels.ContainsKey(jicunqi.WeiYiBiaoShi))
            {
                CunModel models = sModels[jicunqi.WeiYiBiaoShi];
                if (models.IsDu == CunType.KaiShiPing)
                {
                    PeiZhiLei.DataMoXing.SetZhengZaiValue(jicunqi.WeiYiBiaoShi,0);
                    CunModel xinmodel = models.FuZhi();
                    xinmodel.JiCunQi = jicunqi;
                    return xinmodel;
                }
            }
            return null;
        }
        private void JieRu(List<JiCunQiModel> canshus)
        {
            foreach (var item in canshus)
            {
                CunModel ipcom = GetDiZhi(item);

                if (ipcom != null)
                {
                    SengData.Add(ipcom);
                }
            }
        }
        public override JiaoYanJieGuoModel JiaoYanChengGong(JiCunQiModel jicunqiid)
        {
            JiaoYanJieGuoModel models = new JiaoYanJieGuoModel();
            models.WeiYiBiaoShi = jicunqiid.WeiYiBiaoShi;
            models.SheBeiID = jicunqiid.SheBeiID;
            CunModel xinr = PeiZhiLei.DataMoXing.GetModel(jicunqiid);
            if (xinr != null)
            {
                if (xinr.IsZhengZaiCe == 1)
                {
                    models.Value = xinr.JiCunQi.Value;
                    models.IsZuiZhongJieGuo = JieGuoType.ChengGongJiGuo;
                }
                else if (xinr.IsZhengZaiCe == 3)
                {
                    models.Value = "ChaoShi";
                    models.IsZuiZhongJieGuo = JieGuoType.ShiBaiJiGuo;
                }
                else if (xinr.IsZhengZaiCe == 2)
                {
                    models.Value = "NG";
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
                models.Value = "没有找到";
                models.IsZuiZhongJieGuo = JieGuoType.MeiZhaoDaoJiGuo;
            }
            return models;
        }



        public override List<JiCunQiModel> PeiZhiDuXie(int type)
        {
            List<JiCunQiModel> shuju = new List<JiCunQiModel>();
            if (type == 1)
            {
                List<JiCunQiModel> Get = PeiZhiLei.DataMoXing.LisDu;
                shuju = ChangYong.FuZhiShiTi(Get);
            }
            else if (type == 2)
            {
                List<JiCunQiModel> Get = PeiZhiLei.DataMoXing.LisXie;
                shuju = ChangYong.FuZhiShiTi(Get);
            }
            else if (type == 3)
            {
                List<JiCunQiModel> Get = PeiZhiLei.DataMoXing.LisDuXie;
                shuju = ChangYong.FuZhiShiTi(Get);
            }
            return shuju;
        }



        public override JieMianFrmModel GetFrm(bool istiaoshi)
        {
            PeiZhiLei.IsTiaoShi = istiaoshi;
            JieMianFrmModel jieMianFrmModel = new JieMianFrmModel();
            jieMianFrmModel.SheBeiName = SheBeiName;
            jieMianFrmModel.SheBeiID = SheBeiID;
            jieMianFrmModel.Form = new TiaoShiOrPeiZhiFrm(PeiZhiLei);
            return jieMianFrmModel;
        }

        public override List<JiCunQiModel> GetShuJu()
        {

            List<JiCunQiModel> Get = PeiZhiLei.DataMoXing.LisDu;

            return Get;
        }


        private void ReadWork(object zhi)
        {
           List< SaoMaModel> zsmodel = zhi as List<SaoMaModel>;
         
            int yanshi = 5;
         

            DateTime chongliantime = DateTime.Now;

            while (ZongKaiGuan)
            {
                if (DengDaiOpen == false)
                {
                    Thread.Sleep(10);
                    continue;
                }
               
                #region 写
                try
                {
                    int  shuju = SengData.GetCount();
                    if (shuju>0)
                    {
                      
                        CunModel duixiang = SengData.GetModel_Head_RomeHead();
                        WriteRec( duixiang);

                    }
                }
                catch (Exception ex)
                {
                    ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"Ping:写报错:{ex}");
                }
                #endregion
             
                Thread.Sleep(yanshi);
            }
        }

        public void WriteRec(CunModel model)
        {
            if (model.IsDu==CunType.KaiShiPing)
            {
                PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 0);

                KaiPing();
                PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 1);
            }
                     
        }

        private void KaiPing()
        {

            Task task = Task.Run(() =>
            {
                Ping Ping = new Ping();
                List<SaoMaModel> shebeis = PeiZhiLei.DataMoXing.LisSheBei;
                for (int i = 0; i < shebeis.Count; i++)
                {
                    DateTime dateTime = DateTime.Now;
                    CunModel mode = PeiZhiLei.DataMoXing.IsChengGong(shebeis[i].SheBeiID,CunType.XiePing);
                    if (mode==null)
                    {

                        continue;
                    }
                    if (shebeis[i].GuanLianSBID.Count>0)
                    {
                        bool zhen= EnableLocalNetwork(shebeis[i].WangKaName);
                        PeiZhiLei.DataMoXing.SetHeGe(shebeis[i].SheBeiID,zhen);
                        bool zhens = false;
                        for (int c = 0; c < shebeis[i].GuanLianSBID.Count; c++)
                        {
                            if (shebeis[i].GuanLianSBID[c] != shebeis[i].SheBeiID)
                            {
                                SaoMaModel xinmode = PeiZhiLei.DataMoXing.GetSheBeiModel(shebeis[i].GuanLianSBID[c]);
                                if (xinmode!=null)
                                {
                                    DisableLocalNetwork(xinmode.WangKaName);
                                    PeiZhiLei.DataMoXing.SetHeGe(shebeis[i].GuanLianSBID[c],false);
                                    zhens = true;
                                }
                            }
                        }
                        if (zhens)
                        {
                            Thread.Sleep(4000);
                        }
                    }
                    for (; ZongKaiGuan;)
                    {
                        try
                        {
                            IPStatus jiguo = PingTest(shebeis[i].Ip,Ping);
                            if (jiguo == IPStatus.Success)
                            {
                                PeiZhiLei.DataMoXing.SetJiCunQiValue(mode.JiCunQi.WeiYiBiaoShi,"OK");
                                PeiZhiLei.DataMoXing.SetZhengZaiValue(mode.JiCunQi.WeiYiBiaoShi,1);
                                break;
                            }
                            if ((DateTime.Now - dateTime).TotalMilliseconds >= shebeis[i].Time)
                            {
                                PeiZhiLei.DataMoXing.SetJiCunQiValue(mode.JiCunQi.WeiYiBiaoShi, "NG");
                                PeiZhiLei.DataMoXing.SetZhengZaiValue(mode.JiCunQi.WeiYiBiaoShi, 2);
                                break;
                            }
                        }
                        catch
                        {


                        }

                        Thread.Sleep(5);
                    }
                }


            });
        }

        private IPStatus PingTest(string IPAddress,Ping ping, int pingTimeOut = 1000)
        {
            
            ChuFaMsg(MsgDengJi.SheBeiCuoWu,$"开始ping:{IPAddress}");
            IPStatus pingReply = IPStatus.TimedOut;
            pingReply = ping.Send(IPAddress, pingTimeOut).Status;
            ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"结束ping:{pingReply}");
            return pingReply;
        }
        /// <summary>
        /// 禁用网卡
        /// </summary>5
        /// <param name="adapter">网卡对象</param>
        /// <returns></returns>
        private bool DisableLocalNetwork(string wankaname)
        {
            if (PeiZhiLei. WangKas.ContainsKey(wankaname))
            {
                try
                {
                    ManagementBaseObject inParams = PeiZhiLei.WangKas[wankaname].GetMethodParameters("Disable");
                    ManagementBaseObject outParams = PeiZhiLei.WangKas[wankaname].InvokeMethod("Disable", inParams, null);
                    uint resultCode = (uint)outParams["returnValue"];
                    return resultCode == 0;
                }
                catch 
                {

                    
                }
                
            }
            return false;
        }
        /// <summary>
        /// 启用网卡
        /// </summary>
        /// <param name="adapter">网卡对象</param>
        /// <returns></returns>
        private bool EnableLocalNetwork(string wankaname)
        {
            if (PeiZhiLei.WangKas.ContainsKey(wankaname))
            {
                try
                {
                    ManagementBaseObject inParams = PeiZhiLei.WangKas[wankaname].GetMethodParameters("Enable");
                    ManagementBaseObject outParams = PeiZhiLei.WangKas[wankaname].InvokeMethod("Enable", inParams, null);
                    uint resultCode = (uint)outParams["returnValue"];
                    return resultCode == 0;
                }
                catch 
                {

                  
                }
               
            }
            return false;


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
                SaoMaModel item = PeiZhiLei.DataMoXing.LisSheBei[i];
                ZiTxModel zmodel = new ZiTxModel();
                zmodel.Tx = item.TX;
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
