using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using CommLei.DataChuLi;
using CommLei.GongYeJieHe;
using CommLei.JiChuLei;
using MeiGeXinLei.Frm;
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
                return "美格信";
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
                bool zhen = false;
                if (LisLianJieQi.Count > 0)
                {
                    zhen = true;
                }
                foreach (var item in LisLianJieQi.Keys)
                {
                    if (LisLianJieQi[item].TongXinState == false)
                    {
                        return false;
                    }
                }
                return zhen;
            }
        }

        /// <summary>
        /// 连接器
        /// </summary>
        private Dictionary<int, ABSLianJieQi> LisLianJieQi = new Dictionary<int, ABSLianJieQi>();

        private Dictionary<int, FanXingJiHeLei<List<JiCunQiModel>>> SengData = new Dictionary<int, FanXingJiHeLei<List<JiCunQiModel>>>();


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
                for (int i = 0; i < sModels.Count; i++)
                {
                    if (LisLianJieQi.ContainsKey(sModels[i].SheBeiID) == false)
                    {
                        SetLianJieQiModel model = new SetLianJieQiModel();
                        model.IPOrCOMStr = sModels[i].IpOrCom;
                        model.SpeedOrPort = sModels[i].Port;
                        ABSLianJieQi lianjieqi = null;
                        if (model.IPOrCOMStr.Contains("COM"))
                        {
                            lianjieqi = new RJ232LianJieQi();
                            lianjieqi.SetCanShu(model);
                        }
                        else
                        {
                            lianjieqi = new RJ45TcpLianJieQi();
                            lianjieqi.SetCanShu(model);
                        }
                        LisLianJieQi.Add(sModels[i].SheBeiID, lianjieqi);

                        SengData.Add(sModels[i].SheBeiID, new FanXingJiHeLei<List<JiCunQiModel>>());
                        Thread xiancheng = new Thread(ReadWork);
                        xiancheng.IsBackground = true;
                        xiancheng.DisableComObjectEagerCleanup();
                        xiancheng.Start(sModels[i]);

                    }
                }
            }
        }

        private void ZhongShengIO_XieIOEvent(JiCunQiModel obj)
        {
            XieShuJu(new List<JiCunQiModel>() { obj });
        }

        public override void Open()
        {
            StringBuilder msg = new StringBuilder();
            foreach (var item in LisLianJieQi.Keys)
            {
                ResultModel rm = LisLianJieQi[item].Open();
                if (rm.IsSuccess)
                {
                    PeiZhiLei.DataMoXing.SetState(item,true);
                }
                msg.AppendLine(rm.Msg);
            }
            DengDaiOpen = true;
            ChuFaMsg(MsgDengJi.SheBeiZhengChang, msg.ToString());

        }
        public override void Close()
        {
            ZongKaiGuan = false;
            if (LisLianJieQi.Count > 0)
            {
                Thread.Sleep(100);
                Parallel.ForEach(LisLianJieQi.Keys, (x) => {
                    if (LisLianJieQi.ContainsKey(x))
                    {
                        LisLianJieQi[x].Close();
                    }
                });
                Thread.Sleep(200);
            }

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
        private int GetDiZhi(JiCunQiModel jicunqi)
        {
            Dictionary<string, CunModel> sModels = PeiZhiLei.DataMoXing.JiLu;
            if (sModels.ContainsKey(jicunqi.WeiYiBiaoShi))
            {
                CunModel models = sModels[jicunqi.WeiYiBiaoShi];
                if (models.ZhiLingType != ZhiLingType.ZiJieGuo)
                {
                    PeiZhiLei.DataMoXing.SetZhengZaiValue(models.JiCunQi.WeiYiBiaoShi, 0);
                    return models.ZongSheBeiId;
                }
            }
            return -1;
        }
        private void JieRu(List<JiCunQiModel> canshus)
        {
            foreach (var item in canshus)
            {
                int ipcom = GetDiZhi(item);
              
                if (LisLianJieQi.ContainsKey(ipcom))
                {
                    SengData[ipcom].Add(new List<JiCunQiModel>() { item });
                }
            }
        }
        public override JiaoYanJieGuoModel JiaoYanChengGong(JiCunQiModel jicunqiid)
        {
            JiaoYanJieGuoModel models = new JiaoYanJieGuoModel();
            models.WeiYiBiaoShi = jicunqiid.WeiYiBiaoShi;
            models.SheBeiID = jicunqiid.SheBeiID;
            CunModel xinr = PeiZhiLei.DataMoXing.IsChengGong(models.WeiYiBiaoShi);
            if (xinr != null)
            {
                if (xinr.IsZhengZaiCe == 1)
                {
                    models.Value = xinr.JiCunQi.Value;
                    models.IsZuiZhongJieGuo = JieGuoType.ChengGongJiGuo;
                }
                else if (xinr.IsZhengZaiCe == 3)
                {
                    models.Value = xinr.JiCunQi.Value;
                    models.IsZuiZhongJieGuo = JieGuoType.ShiBaiJiGuo;
                }
                else if (xinr.IsZhengZaiCe == 2)
                {
                    models.Value = xinr.JiCunQi.Value;
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
            SaoMaModel zsmodel = zhi as SaoMaModel;
            ABSLianJieQi lianjieqi = LisLianJieQi[zsmodel.SheBeiID];
            int yanshi = 5;
            FanXingJiHeLei<List<JiCunQiModel>> sengData = SengData[zsmodel.SheBeiID];

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
                    int  shuju = sengData.GetCount();
                    if (shuju>0)
                    {
                        List<JiCunQiModel> duixiang = new List<JiCunQiModel>();
                        duixiang = sengData.GetModel_Head_RomeHead();

                        int count = duixiang.Count;
                        for (int i = 0; i < count; i++)
                        {
                            CunModel dizhi = PeiZhiLei.DataMoXing.GetModel(duixiang[i]);
                            if (dizhi!=null)
                            {
                                dizhi.JiCunQi.Value = duixiang[i].Value;
                                WriteRec(lianjieqi, dizhi);
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"{zsmodel.SheBeiID}:写报错:{ex}");
                }
                #endregion
             
                Thread.Sleep(yanshi);
            }
        }

        public void WriteRec(ABSLianJieQi lianjieqi,CunModel model)
        {
            if (model.ZhiLingType == ZhiLingType.BFTongDao)
            {
                PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 0);
                XieData(lianjieqi, false, model);
            }
            else if (model.ZhiLingType == ZhiLingType.TZBFTongDao)
            {
                PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 0);
                XieData(lianjieqi, false, model);
            }
            else if (model.ZhiLingType == ZhiLingType.KaiCai)
            {
                PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 0);
                XieData(lianjieqi, false, model);
            }
            else if (model.ZhiLingType == ZhiLingType.CJJieGuo)
            {
                PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 0);
                XieData(lianjieqi, true, model);
            }
            else if (model.ZhiLingType==ZhiLingType.QiTa)
            {
                object qiTaLeiType = ChangYong.GetMeiJuZhi(typeof(QiTaLeiType) ,model.Name);
                if (qiTaLeiType != null)
                {
                    if ((QiTaLeiType)qiTaLeiType == QiTaLeiType.GW)
                    {
                        PeiZhiLei.DataMoXing.GWFuZhi(model.JiCunQi.WeiYiBiaoShi, model.JiCunQi.Value.ToString());
                    }
                    else if ((QiTaLeiType)qiTaLeiType == QiTaLeiType.XinZaoBi)
                    {
                        string[] fenge=model.JiCunQi.Value.ToString().Split(',');
                        if (fenge.Length >= 2)
                        {
                            PeiZhiLei.DataMoXing.GetAudioSNR(model.JiCunQi.WeiYiBiaoShi, fenge[0], fenge[1]);
                        }
                        else
                        {
                            PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "配置有问题");
                            PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);
                        }
                    }
                    else if ((QiTaLeiType)qiTaLeiType == QiTaLeiType.ZuoFenLiDu)
                    {
                        string[] fenge = model.JiCunQi.Value.ToString().Split(',');
                        if (fenge.Length >= 2)
                        {
                            PeiZhiLei.DataMoXing.GetAudioBalanceLeft(model.JiCunQi.WeiYiBiaoShi, fenge[0], fenge[1]);
                        }
                        else
                        {
                            PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "配置有问题");
                            PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);
                        }
                    }
                    else if ((QiTaLeiType)qiTaLeiType == QiTaLeiType.YouFenLiDu)
                    {
                        string[] fenge = model.JiCunQi.Value.ToString().Split(',');
                        if (fenge.Length >= 2)
                        {
                            PeiZhiLei.DataMoXing.GetAudioBalanceRight(model.JiCunQi.WeiYiBiaoShi, fenge[0], fenge[1]);
                        }
                        else
                        {
                            PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "配置有问题");
                            PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);
                        }
                    }
                    else
                    {
                        PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "没有改功能");
                        PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);
                    }
                }
                else
                {
                    PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "配置有问题");
                    PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);
                }
            }
                    
        }
        private void XieData(ABSLianJieQi lianjieqi,bool isfuzhi, CunModel model)
        {
            byte[] data = Encoding.ASCII.GetBytes(model.ZhiLing);
            lianjieqi.Send(data);
            DateTime shijian = DateTime.Now;
            List<byte> datalis = new List<byte>();
            for (; ZongKaiGuan;)
            {
                ResultModel rm = lianjieqi.Read();
                if (rm.IsSuccess)
                {
                    datalis.AddRange(rm.TData);
                }
                if (datalis.Count > 0)
                {
                    string str = Encoding.Default.GetString(datalis.ToArray());
                    if (str.Contains(model.ZhiLingJieSu))
                    {
                        if (isfuzhi)
                        {
                            str = str.Replace("\r\n", "");
                            string[] strings = str.Split(new string[] { "#$^data^" }, StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < strings.Length; i++)
                            {
                                if (strings[i].Contains("Data Info") && strings[i].Contains("Data:"))
                                {
                                    string name = ChangYong.StrDataCut(strings[i], "Data Info:", "Test", 2);
                                    string zhi = ChangYong.StrDataCut(strings[i], "Data:{", "}", 2).Replace("\n", "");
                                    PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, name, zhi);
                                }

                            }
                        }
                        ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"{model.Name},发送数据正常:发{model.ZhiLing},收:{Encoding.Default.GetString(datalis.ToArray())}");
                        PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "OK");
                        PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 1);
                        return;
                    }

                }
                if ((DateTime.Now - shijian).TotalMilliseconds >= model.Time)
                {
                    PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 3);
                    ChuFaMsg(MsgDengJi.SheBeiBaoWen,$"{model.Name},发送数据超时:发{model.ZhiLing},收:{Encoding.Default.GetString(datalis.ToArray())}");
                    return ;
                }
                Thread.Sleep(10);
            }

            PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi,2);
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

        public override KJPeiZhiJK GetCanShuKJ(string jicunweiyibiaoshi)
        {
            JiCunQiModel jiCunQiModel = new JiCunQiModel();
            jiCunQiModel.WeiYiBiaoShi = jicunweiyibiaoshi;
            CunModel cunModel = PeiZhiLei.DataMoXing.GetModel(jiCunQiModel);
            if (cunModel != null)
            {
                if (cunModel.ZhiLingType == ZhiLingType.BFTongDao || cunModel.ZhiLingType == ZhiLingType.TZBFTongDao || cunModel.ZhiLingType == ZhiLingType.KaiCai || cunModel.ZhiLingType == ZhiLingType.CJJieGuo || cunModel.ZhiLingType == ZhiLingType.ZiJieGuo)
                {
                    CanShuKJ kj = new CanShuKJ();
                    kj.SetCanShu(new List<string>(), true, false);
                    return kj;
                }
                else
                {
                    if (cunModel.ZhiLingType == ZhiLingType.QiTa)
                    {
                        QiTaLeiType shujutype = ChangYong.GetMeiJuZhi<QiTaLeiType>(cunModel.Name);
                        if (shujutype == QiTaLeiType.GW)
                        {
                            CanShuKJ kj = new CanShuKJ();
                            kj.SetCanShu(PeiZhiLei.DataMoXing.GetQiTaShuJu(), false, true);
                            return kj;
                        }
                        else
                        {
                            CanShuKJ kj = new CanShuKJ();
                            kj.SetCanShu(PeiZhiLei.DataMoXing.GetQiTaShuJu(), false, false);
                            return kj;
                        }
                    }
                    
                }
            }
            return base.GetCanShuKJ(jicunweiyibiaoshi);
        }
        public override void Clear(bool isquanbu, JiCunQiModel model)
        {
          
        }
    }
}
