using CommLei.DataChuLi;
using CommLei.GongYeJieHe;
using CommLei.JiChuLei;
using SSheBei.ABSSheBei;
using SSheBei.CRCJiaoYan;
using SSheBei.LianJieQi;
using SSheBei.Model;
using System;
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
                return "新的数据设备";
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

        private Dictionary<int, FanXingJiHeLei<CunModel>> SengData = new Dictionary<int, FanXingJiHeLei<CunModel>>();


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
                        lianjieqi = new RJ232LianJieQi();
                        lianjieqi.SetCanShu(model);
                        LisLianJieQi.Add(sModels[i].SheBeiID, lianjieqi);
                      
                        SengData.Add(sModels[i].SheBeiID, new FanXingJiHeLei<CunModel>());
                        Thread xiancheng = new Thread(ReadWork);
                        xiancheng.IsBackground = true;
                        xiancheng.DisableComObjectEagerCleanup();
                        xiancheng.Start(sModels[i]);
                       
                    }
                }
            }
        }

        private void ZhongShengIO_XieIOEvent(CunModel obj)
        {
            XieShuJu(new List<JiCunQiModel>() { obj.JiCunQi });
        }

        public override void Open()
        {
            StringBuilder msg = new StringBuilder();
            foreach (var item in LisLianJieQi.Keys)
            {
                ResultModel rm = LisLianJieQi[item].Open();
                msg.AppendLine(rm.Msg);
                
                PeiZhiLei.DataMoXing.SetTX(item,-1, rm.IsSuccess);
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
        private void JieRu(List<JiCunQiModel> canshus)
        {
            foreach (var item in canshus)
            {
                CunModel cunModel = PeiZhiLei.DataMoXing.GetCunModel(item, true);
                if (cunModel != null)
                {
                    if (LisLianJieQi.ContainsKey(cunModel.SheBeiID))
                    {
                        if (LisLianJieQi[cunModel.SheBeiID].TongXinState)
                        {
                            if (cunModel.IsDu.ToString().ToLower().Contains("du") == false)
                            {
                                PeiZhiLei.DataMoXing.SetSate(cunModel, 0);
                                cunModel.JiCunQi = item;
                                SengData[cunModel.SheBeiID].Add(cunModel);
                            }

                        }
                    }
                }
            }
        }
        public override JiaoYanJieGuoModel JiaoYanChengGong(JiCunQiModel jicunqiid)
        {
            JiaoYanJieGuoModel models = new JiaoYanJieGuoModel();
            models.WeiYiBiaoShi = jicunqiid.WeiYiBiaoShi;
            models.SheBeiID = jicunqiid.SheBeiID;
            CunModel iskekao = PeiZhiLei.DataMoXing.GetCunModel(jicunqiid,false);
            if (iskekao != null)
            {
                if (iskekao.JiCunQi.IsKeKao)
                {
                    if (iskekao.JiCunQi.DuXie == 1)
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
                    else
                    {
                        int zhen = iskekao.XieState;
                        models.Value = iskekao.JiCunQi.Value;
                        if (zhen == 1)
                        {
                            models.IsZuiZhongJieGuo = JieGuoType.ChengGongJiGuo;
                        }
                        else
                        {
                            if (zhen == 3)
                            {
                                models.IsZuiZhongJieGuo = JieGuoType.BuKeKaoJieGuo;
                            }
                            else
                            {
                                models.IsZuiZhongJieGuo = JieGuoType.JingXingZhong;
                            }
                        }
                    }
                }
                else
                {
                    models.Value = "信号不可靠";
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
            FanXingJiHeLei<CunModel>sengData = SengData[zsmodel.SheBeiID];
            
            DateTime chongliantime = DateTime.Now;                 
            while (ZongKaiGuan)
            {
                if (DengDaiOpen == false)
                {
                    Thread.Sleep(10);
                    continue;
                }
                if (lianjieqi.TongXinState==false)
                {                
                    if ((DateTime.Now- chongliantime).TotalMilliseconds>=1000)
                    {
                        lianjieqi.ChongLian();
                        chongliantime = DateTime.Now;
                    }
                    if (lianjieqi.TongXinState == false)
                    {
                        PeiZhiLei.DataMoXing.SetTX(zsmodel.SheBeiID,-1,false);
                        Thread.Sleep(10);
                       
                    }
                }
                #region 写
                try
                {
                    int count = sengData.GetCount();
                    if (count>0)
                    {
                        if (count>=2)
                        {
                            count = 2;
                        }
                        for (int i = 0; i < count; i++)
                        {
                            CunModel model = sengData.GetModel_Head_RomeHead();
                            if (model!=null)
                            {
                                XieShuJu(model,lianjieqi);
                            }
                           
                        }
                       
                    }
                }
                catch (Exception ex)
                {

                    ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"{zsmodel.SheBeiID}:写报错:{ex}");
                }
                #endregion
                #region 读
                try
                {
                    if (lianjieqi.TongXinState)
                    {
                        FuZhi(zsmodel, lianjieqi);
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

        private void XieShuJu(CunModel cunmodel, ABSLianJieQi lianjieqi)
        {
            PeiZhiLei.DataMoXing.SetSate(cunmodel,0);
            bool zhen = false;
            int xietime = 20;
            List<List<byte>> zhiling = PeiZhiLei.DataMoXing.GetSendCMD(cunmodel,out xietime);
            if (zhiling.Count > 0)
            {
                for (int i = 0; i < zhiling.Count; i++)
                {
                    if (zhiling[i].Count > 0)
                    {
                        zhen = true;
                        ResultModel rm = lianjieqi.Send(zhiling[i].ToArray());
                        ChuFaMsg(MsgDengJi.SheBeiBaoWen, rm.Msg);
                        Thread.Sleep(xietime);
                    }
                }
            }
            if (zhen)
            {
                PeiZhiLei.DataMoXing.SetXieJiCunQiZhi(cunmodel, "OK");
                PeiZhiLei.DataMoXing.SetSate(cunmodel, 1);
            }
            else
            {
                PeiZhiLei.DataMoXing.SetXieJiCunQiZhi(cunmodel, "NG");
                PeiZhiLei.DataMoXing.SetSate(cunmodel, 2);
            }


        }

        private void FuZhi(ZSModel model,ABSLianJieQi lianjieqi)
        {
            for (int i = 0; i < model.LisSheBei.Count; i++)
            {
                bool zhen = false;
                ZiSheBeiModel zishebei = model.LisSheBei[i];
                for (int c = 0; c < zishebei.ZhiLingS.Count; c++)
                {                
                    DuZhiLingModel dumosdwl = zishebei.ZhiLingS[c];
                    if (dumosdwl!=null&&string.IsNullOrEmpty(dumosdwl.DuZhiLing)==false)
                    {
                        zhen = true;
                        ResultModel rm = lianjieqi.Send(dumosdwl.SendZhiLing);
                        ChuFaMsg(MsgDengJi.SheBeiBaoWen, rm.Msg);
                        List<byte> datas = new List<byte>();
                        PeiZhiLei.DataMoXing.ClearData(zishebei.ZiID);
                        DateTime shijians = DateTime.Now;
                        for (; ZongKaiGuan && lianjieqi.TongXinState;)
                        {
                          
                            rm = lianjieqi.Read();
                            if (rm.IsSuccess)
                            {
                                PeiZhiLei.DataMoXing.JieShouShuJu(rm.TData, dumosdwl.ShuJuChangDu, zishebei.ZiID);
                                datas.AddRange(rm.TData);
                            }
                            int jieguo = PeiZhiLei.DataMoXing.JiaoYanShuJu(zishebei.ZiID, dumosdwl);
                            if (jieguo == 1)
                            {
                                zishebei.TxCiShu = 5;
                                break;
                            }
                            if ((DateTime.Now - shijians).TotalMilliseconds >= zishebei.DuChaoShiTime)
                            {
                                zishebei.TxCiShu--;
                                ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"数据读取超时");
                                break;
                            }
                        }
                        if (zishebei.TxCiShu < 0)
                        {
                            PeiZhiLei.DataMoXing.SetTX(model.SheBeiID, zishebei.ZiID, false);
                        }
                        else
                        {
                            PeiZhiLei.DataMoXing.SetTX(model.SheBeiID, zishebei.ZiID, true);
                        }
                        ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"{model.SheBeiID}读取的数据,{(DateTime.Now - shijians).TotalMilliseconds}:{ChangYong.ByteOrString(datas, " ")}");
                        datas.Clear();
                       
                    }
                }
                if (zhen && model.LisSheBei.Count>1)
                {
                    Thread.Sleep(model.QieHuanTime);
                }
            }
          
        }
     
     


        public override KJPeiZhiJK GetCanShuKJ(string jicunweiyibiaoshi)
        {
            JiCunQiModel jiCunQiModel = new JiCunQiModel();
            jiCunQiModel.WeiYiBiaoShi = jicunweiyibiaoshi;
            CunModel cunModel = PeiZhiLei.DataMoXing.GetCunModel(jiCunQiModel,false);
            if (cunModel != null&&cunModel.IsDu.ToString().ToLower().Contains("xie"))
            {
                //CanShuKJ kj = new CanShuKJ();
             
                //kj.SetShuJu(cunModel.JiLu,cunModel.IsDu.ToString().Contains("全")==false);
                //return kj;
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
                List<ZiSheBeiModel> dizhis = PeiZhiLei.DataMoXing.LisSheBei[i].LisSheBei;
                foreach (var item in dizhis)
                {
                    ZiTxModel zmodel = new ZiTxModel();
                    zmodel.Tx = item.Tx;
                    zmodel.ZiSheBeiID = item.ZiID;
                    zmodel.ZiSheBeiName = $"{item.Name}"; 
                    if (zmodel.Tx == false)
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
