using CommLei.DataChuLi;
using CommLei.JiChuLei;
using SSheBei.ABSSheBei;
using SSheBei.CRCJiaoYan;
using SSheBei.LianJieQi;
using SSheBei.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZhongWangSheBei.Frm;
using ZhongWangSheBei.Model;

namespace ZhongWangSheBei.ShiXian
{
    public class ZhongShengIO : ABSNSheBei
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
                return "中创电源模块";
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
                if (LisLianJieQi.Count>0)
                {
                    zhen = true;
                }
                foreach (var item in LisLianJieQi.Keys)
                {
                    if (LisLianJieQi[item].TongXinState==false)
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

        private Dictionary<int, ConcurrentQueue<List<JiCunQiModel>>> SengData = new Dictionary<int, ConcurrentQueue<List<JiCunQiModel>>>();


        public ZhongShengIO()
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
                PeiZhiLei.IniData(SheBeiID,SheBeiName);
                PeiZhiLei.XieIOEvent += ZhongShengIO_XieIOEvent;
                List<ZSModel> sModels = PeiZhiLei.DataMoXing.LisSheBei;
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
                      
                        SengData.Add(sModels[i].SheBeiID, new ConcurrentQueue<List<JiCunQiModel>>());
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
            if (canshus == null|| canshus.Count==0)
            {
                return;
            }
            if (DengDaiOpen)
            {
                JieRu(canshus);
            }
        }

        public override JiaoYanJieGuoModel JiaoYanChengGong(JiCunQiModel jicunqiid)
        {
            JiaoYanJieGuoModel models = new JiaoYanJieGuoModel();
            models.WeiYiBiaoShi = jicunqiid.WeiYiBiaoShi;
            models.SheBeiID = jicunqiid.SheBeiID;
            CunModel iskekao = IsKeKao(jicunqiid);
            if (iskekao != null)
            {
                if (iskekao.JiCunQi.DuXie==1)
                {
                    models.Value = iskekao.JiCunQi.Value;
                    if (iskekao.JiCunQi.IsKeKao)
                    {
                        models.IsZuiZhongJieGuo = JieGuoType.ChengGongJiGuo;
                    }
                    else
                    {
                        models.IsZuiZhongJieGuo = JieGuoType.BuKeKaoJieGuo;
                    }
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
            ZSModel zsmodel = zhi as ZSModel;
            ABSLianJieQi lianjieqi = LisLianJieQi[zsmodel.SheBeiID];
            int yanshi = 5;
            ConcurrentQueue<List<JiCunQiModel>> sengData = SengData[zsmodel.SheBeiID];
            SerialDataGuDingJieXi dingJieXi = new SerialDataGuDingJieXi();
            DateTime jishicongnian = DateTime.Now;
            DateTime chongliantime = DateTime.Now;

            int quehuanshijian = zsmodel.QieHuanTime;
            if (quehuanshijian <= 0)
            {
                quehuanshijian = 5;
            }       
            List<int> naxieyaodu = new List<int>();
            while (ZongKaiGuan)
            {
                if (DengDaiOpen == false)
                {
                    Thread.Sleep(10);
                    continue;
                }
                if (lianjieqi.TongXinState==false)
                {
                    naxieyaodu.Clear();
                    if ((DateTime.Now- chongliantime).TotalMilliseconds>=1000)
                    {
                        lianjieqi.ChongLian();
                        chongliantime = DateTime.Now;
                    }
                    if (lianjieqi.TongXinState == false)
                    {
                        PeiZhiLei.DataMoXing.SetTX(zsmodel.SheBeiID,-1,true,false);
                        Thread.Sleep(10);
                        continue;
                    }
                }             
               
                #region 读
                try
                {
                    if (lianjieqi.TongXinState)
                    {
                        if (naxieyaodu.Count == 0)
                        {
                            for (int i = 0; i < zsmodel.LisJiLu.Count; i++)
                            {
                                FuZhi(zsmodel.SheBeiID, zsmodel.LisJiLu[i], dingJieXi, lianjieqi);
                                if (!(ZongKaiGuan && lianjieqi.TongXinState))
                                {
                                    break;
                                }
                                bool shuju = sengData.IsEmpty;
                                if (shuju == false)
                                {
                                    int count = sengData.Count;
                                    if (count > 0)
                                    {
                                        break;
                                    }
                                }
                                if (zsmodel.LisJiLu.Count > 1)
                                {
                                    Thread.Sleep(quehuanshijian);
                                }
                            }
                        }
                        else
                        {
                            if (naxieyaodu.Count == 1)
                            {
                                for (int i = 0; i < zsmodel.LisJiLu.Count; i++)
                                {
                                    if (zsmodel.LisJiLu[i].ZSID == naxieyaodu[0])
                                    {
                                        FuZhi(zsmodel.SheBeiID,zsmodel.LisJiLu[i], dingJieXi, lianjieqi);
                                        break;
                                    }

                                }
                            }
                            else
                            {
                                for (int i = 0; i < zsmodel.LisJiLu.Count; i++)
                                {
                                    if (naxieyaodu.IndexOf(zsmodel.LisJiLu[i].ZSID) >= 0)
                                    {
                                        FuZhi(zsmodel.SheBeiID, zsmodel.LisJiLu[i], dingJieXi, lianjieqi);
                                    }
                                    if (naxieyaodu.Count > 1)
                                    {
                                        Thread.Sleep(quehuanshijian);
                                    }
                                }

                            }
                        }

                    }
                  
                }
                catch(Exception ex)
                {
                    ChuFaMsg(MsgDengJi.SheBeiCuoWu,$"{zsmodel.SheBeiID}:读报错:{ex}");
                }
                #endregion
                Thread.Sleep(yanshi);
            }
        }



        private void FuZhi(int zongshebeiid,  ZiSheBeiModel model, SerialDataGuDingJieXi serialDataGuDingJieXi,ABSLianJieQi lianjieqi)
        {         
            if (model.ZhiLing != null)
            {
                ResultModel rm=lianjieqi.Send(model.ZhiLing.ToArray());
                ChuFaMsg(MsgDengJi.SheBeiBaoWen, rm.Msg);
                DateTime date = DateTime.Now;
                serialDataGuDingJieXi.Clear();
                List<byte> datas = new List<byte>();
                for (; ZongKaiGuan && lianjieqi.TongXinState;)
                {
                    DateTime shijians = DateTime.Now;
                    rm = lianjieqi.Read();               
                    if (rm.IsSuccess)
                    {
                        serialDataGuDingJieXi.AddByteList(rm.TData);
                        datas.AddRange(rm.TData);
                    }
                    serialDataGuDingJieXi.JieXiWanMeiData(model.ChangDu+ 5, new byte[] { (byte)model.DiZhi, 0x03, (byte)(model.ChangDu) }, true, JiaoYanWenDu, false);
                    if (serialDataGuDingJieXi.DataCount > 0)
                    {

                        byte[] xindata = serialDataGuDingJieXi.GetShiShiData();

                        if (xindata != null && xindata.Length >= model.ChangDu + 3)
                        {
                            List<byte> shujus = new List<byte>();
                            for (int i = 3; i < xindata.Length - 2; i++)
                            {
                                shujus.Add(xindata[i]);
                            }
                            PeiZhiLei.DataMoXing.SetJiCunQiValue(zongshebeiid, model.ZSID, shujus);
                        
                        }
                        break;

                    }
                    if ((DateTime.Now - date).TotalMilliseconds >= 300)
                    {
                        PeiZhiLei.DataMoXing.SetTX(zongshebeiid, model.ZSID,false,false);
                        ChuFaMsg(MsgDengJi.SheBeiBaoWen,$"数据读取超时");
                        break;
                    }
                }
                ChuFaMsg(MsgDengJi.SheBeiBaoWen,$"{model.ZSID}读取的数据,{(DateTime.Now-date).TotalMilliseconds}:{ChangYong.ByteOrString(datas," ")}");
                datas.Clear();
            }
        }
     
        private bool JiaoYanWenDu(List<byte> canshu)
        {
            if (canshu == null || canshu.Count <= 1)
            {
                return false;
            }
            List<byte> data = canshu.GetRange(0, canshu.Count - 2);
            byte[] shuju =CRC.ToModbus(data, false);
            if (shuju != null && shuju.Length >= 2)
            {
                if (shuju[0] == canshu[canshu.Count - 2] && shuju[1] == canshu[canshu.Count - 1])
                {
                    return true;
                }
            }
            return true;
        }

     



     
        private CunModel IsKeKao(JiCunQiModel model)
        {
           
            Dictionary<string, CunModel> sModels = PeiZhiLei.DataMoXing.JiLu;
            if (sModels.ContainsKey(model.WeiYiBiaoShi))
            {
                CunModel models = sModels[model.WeiYiBiaoShi];
                return models;
            }

            return null;
        }

     

        private void JieRu(List<JiCunQiModel> canshus)
        {
            foreach (var item in canshus)
            {
               
                CunModel cunModel=  IsKeKao(item);
                if (cunModel != null)
                {
                    if (LisLianJieQi.ContainsKey(cunModel.ZongSheBeiId))
                    {
                        if (LisLianJieQi[cunModel.ZongSheBeiId].TongXinState)
                        {
                            if (cunModel.IsDu != CunType.DuJiCunQi)
                            {
                                SengData[cunModel.ZongSheBeiId].Enqueue(new List<JiCunQiModel>() { item });
                            }
                        }
                    }
                }
            }
        }

        public override KJPeiZhiJK GetCanShuKJ(string jicunweiyibiaoshi)
        {
            JiCunQiModel jiCunQiModel = new JiCunQiModel();
            jiCunQiModel.WeiYiBiaoShi = jicunweiyibiaoshi;
            CunModel cunModel = IsKeKao(jiCunQiModel);
            if (cunModel != null&&cunModel.IsDu.ToString().ToLower().Contains("xie"))
            {
                CanShuKJ kj = new CanShuKJ();
             
                kj.SetShuJu(cunModel.JiLu,cunModel.IsDu.ToString().Contains("全")==false);
                return kj;
            }
            return base.GetCanShuKJ(jicunweiyibiaoshi);
        }
        public override void Clear(bool isquanbu, JiCunQiModel model)
        {
           
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
              
                List<ZiSheBeiModel> shebei = PeiZhiLei.DataMoXing.LisSheBei[i].LisJiLu;
                foreach (var item in shebei)
                {
                    ZiTxModel zmodel = new ZiTxModel();
                    zmodel.Tx = item.Tx;
                    zmodel.ZiSheBeiID = item.ZSID;
                    zmodel.ZiSheBeiName = item.ZiName;
                    if (zmodel.Tx==false)
                    {
                        ischengg = false;
                    }

                    model.LisTx.Add(zmodel);
                }
                
            }
            model.ZongTX = ischengg;
            return model;
        }
    }
}
