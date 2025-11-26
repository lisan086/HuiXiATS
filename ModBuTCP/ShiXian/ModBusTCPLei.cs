using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommLei.DataChuLi;
using CommLei.GongYeJieHe;
using CommLei.JiChuLei;
using ModBuTCP.Frm;
using ModBuTCP.Model;
using SSheBei.ABSSheBei;
using SSheBei.CRCJiaoYan;
using SSheBei.LianJieQi;
using SSheBei.Model;

namespace ModBuTCP.ShiXian
{
    public class ModBusTCPLei : ABSNSheBei
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
                return "ModbusTCP设备";
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

        private Dictionary<int, FanXingJiHeLei<List<DataCunModel>>> SengData = new Dictionary<int, FanXingJiHeLei<List<DataCunModel>>>();


        public ModBusTCPLei()
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
                List<SheBeiModel> sModels = PeiZhiLei.DataMoXing.LisSheBei;
                for (int i = 0; i < sModels.Count; i++)
                {
                    if (LisLianJieQi.ContainsKey(sModels[i].SheBeiID) == false)
                    {
                        SetLianJieQiModel model = new SetLianJieQiModel();
                        model.IPOrCOMStr = sModels[i].IpOrCom;
                        model.SpeedOrPort = sModels[i].Port;
                        ABSLianJieQi lianjieqi = new RJ45TcpLianJieQi();                    
                        lianjieqi.SetCanShu(model);
                        LisLianJieQi.Add(sModels[i].SheBeiID, lianjieqi);
                        SengData.Add(sModels[i].SheBeiID, new FanXingJiHeLei<List<DataCunModel>>());
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
                PeiZhiLei.DataMoXing.SetTx(item,rm.IsSuccess);
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
                for (int i = 0; i < canshus.Count; i++)
                {
                    DataCunModel dataCunModel = PeiZhiLei.DataMoXing.GetModel(canshus[i].WeiYiBiaoShi, true);
                    if (dataCunModel!=null)
                    {
                        if (dataCunModel.YingYongType != YingYongType.DuPuTong)
                        {
                            
                            if (SengData.ContainsKey(dataCunModel.SheBeiID))
                            {
                                dataCunModel.JiCunQiModel = canshus[i];
                                SengData[dataCunModel.SheBeiID].Add(new List<DataCunModel>() { dataCunModel });
                                if (dataCunModel.IsHuiLing == 1)
                                {
                                    if (dataCunModel.JiCunQiModel.Value.ToString().Equals(dataCunModel.HuiLingZhi) == false)
                                    {
                                        Task.Factory.StartNew(() =>
                                        {
                                            DataCunModel shuju = dataCunModel.FuZhi();
                                            shuju.JiCunQiModel.Value = shuju.HuiLingZhi;
                                            int huilingyanshi = shuju.HuiLingYanShi;
                                            if (huilingyanshi <= 0)
                                            {
                                                huilingyanshi = 250;
                                            }
                                            Thread.Sleep(huilingyanshi);

                                            SengData[shuju.SheBeiID].Add(new List<DataCunModel>() { shuju });
                                        }
                                        );
                                    }

                                }
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
          
            DataCunModel zhen = PeiZhiLei.DataMoXing.IsChengGong(jicunqiid);
            if (zhen!=null)
            {
                if (zhen.JiCunQiModel.IsKeKao)
                {
                    if (zhen.YingYongType == YingYongType.DuPuTong )
                    {
                        models.Value = zhen.JiCunQiModel.Value;
                        models.IsZuiZhongJieGuo = JieGuoType.ChengGongJiGuo;
                    }
                    else if (zhen.YingYongType == YingYongType.DuXieYiQi)
                    {
                        if (zhen.IsXieWan == 1)
                        {
                            models.Value = zhen.JiCunQiModel.Value;
                            models.IsZuiZhongJieGuo = JieGuoType.ChengGongJiGuo;
                        }
                        else if (zhen.IsXieWan >= 2)
                        {

                            models.Value = zhen.JiCunQiModel.Value;
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
                        if (zhen.IsXieWan == 1)
                        {
                            models.Value = zhen.JiCunQiModel.Value;
                            models.IsZuiZhongJieGuo = JieGuoType.ChengGongJiGuo;
                        }
                        else if (zhen.IsXieWan >= 2)
                        {

                            models.Value = zhen.JiCunQiModel.Value;
                            models.IsZuiZhongJieGuo = JieGuoType.ShiBaiJiGuo;
                        }
                        else
                        {
                            models.Value = "";
                            models.IsZuiZhongJieGuo = JieGuoType.JingXingZhong;
                        }
                    }
                }
                else
                {
                    models.Value = zhen.JiCunQiModel.Value;
                    models.IsZuiZhongJieGuo = JieGuoType.BuKeKaoJieGuo;
                }
            }
            else
            {
                models.Value = "未找到";
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
                List<JiCunQiModel> Getdu = PeiZhiLei.DataMoXing.LisDuXie;
                shuju = Getdu;

            }
            return shuju;
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

        public override List<JiCunQiModel> GetShuJu()
        {

            List<JiCunQiModel> Get = PeiZhiLei.DataMoXing.LisDu;

            return Get;
        }

        public override object GetLianJieQi(int id)
        {
            if (LisLianJieQi.ContainsKey(id))
            {
                return LisLianJieQi[id];
            }
            return null;
        }
        private void ReadWork(object zhi)
        {
            SheBeiModel zsmodel = zhi as SheBeiModel;
            ABSLianJieQi lianjieqi = LisLianJieQi[zsmodel.SheBeiID];
            int yanshi = 5;
            FanXingJiHeLei<List<DataCunModel>> sengData = SengData[zsmodel.SheBeiID];
            SerialDataGuDingJieXi dingJieXi = new SerialDataGuDingJieXi();
            DateTime jishicongnian = DateTime.Now;
            DateTime chongliantime = DateTime.Now;

         
            while (ZongKaiGuan)
            {
                if (DengDaiOpen == false)
                {
                    Thread.Sleep(10);
                    continue;
                }
                if (GuanBiDuXie)
                {
                    Thread.Sleep(10);
                    continue;
                }
                if (lianjieqi.TongXinState == false)
                {
                    if ((DateTime.Now - chongliantime).TotalMilliseconds >= 1000)
                    {
                    
                        bool zhen=  lianjieqi.ChongLian();
                        chongliantime = DateTime.Now;
                        PeiZhiLei.DataMoXing.SetTx(zsmodel.SheBeiID, zhen);
                    }
                   
                }
            
                #region 写
                try
                {
                    
                    int count = sengData.GetCount();
                    if (count>0)
                    {
                        List<DataCunModel> duixiang = sengData.GetModel_Head_RomeHead();                 
                        for (int i = 0; i < duixiang.Count; i++)
                        {
                            if (lianjieqi.TongXinState)
                            {
                                WriteCMD(duixiang[i], lianjieqi);
                                Thread.Sleep(zsmodel.XieYanShi);
                            }
                            else
                            {
                                PeiZhiLei.DataMoXing.SetZhengTaiState(duixiang[i].SheBeiID, duixiang[i].JiCunQiModel.WeiYiBiaoShi, 2);
                               
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
                        {
                            List<JiLuModel> lis = PeiZhiLei.DataMoXing.GetJiLuSheBei(zsmodel.SheBeiID);
                            for (int i = 0; i < lis.Count; i++)
                            {
                                FuZhi(zsmodel.SheBeiID, lis[i], dingJieXi, lianjieqi,zsmodel.DuYanShi);
                            }
                        }
                        {
                            List<DataCunModel> lis = PeiZhiLei.DataMoXing.GetTeShuBoolDu(zsmodel.SheBeiID);
                            for (int i = 0; i < lis.Count; i++)
                            {
                                FuZhi(zsmodel.SheBeiID, lis[i], dingJieXi, lianjieqi, zsmodel.DuYanShi);
                            }
                        }
                    }
                    ChuFaDu(zsmodel.SheBeiID);
                }
                catch (Exception ex)
                {
                    ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"{zsmodel.SheBeiID}:读报错:{ex}");
                }
                #endregion
                Thread.Sleep(yanshi);
            }
        }

       

        private void FuZhi(int zongshebeiid, JiLuModel model, SerialDataGuDingJieXi jiexieqi, ABSLianJieQi lianjieqi,int timeout=300)
        {
            if (model.ShuJu != null)
            {
                jiexieqi.Clear();
                DateTime chaoshi = DateTime.Now;
                int chaoshitime = timeout;
                List<byte> data = new List<byte>();
                ResultModel rm = lianjieqi.Send(model.ShuJu.ToArray());
                ChuFaMsg(MsgDengJi.SheBeiBaoWen, rm.Msg);
                for (; ZongKaiGuan && lianjieqi.TongXinState;)
                {
                    Thread.Sleep(1);
                    rm = lianjieqi.Read();
                    if (rm.IsSuccess)
                    {
                        jiexieqi.AddByteList(rm.TData);
                        data.AddRange(rm.TData);
                    }
                    jiexieqi.JieXiWanMeiData((model.Count * 2) + 3, new byte[] { (byte)1, 0x03, (byte)(model.Count * 2) }, false, null, false);
                    if (jiexieqi.DataCount > 0)
                    {
                        byte[] xindata = jiexieqi.GetShiShiData();
                        if (xindata != null && xindata.Length >= model.Count * 2 + 3)
                        {
                            PeiZhiLei.DataMoXing.SetJiCunQiValue(zongshebeiid, xindata.ToList(), model);
                       
                            break;
                        }
                    }
                    if ((DateTime.Now - chaoshi).TotalMilliseconds >= chaoshitime)
                    {
                       
                        break;
                    }

                }
             
                ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"{zongshebeiid}读取的数据,{(DateTime.Now - chaoshi).TotalMilliseconds}:{ChangYong.ByteOrString(data, " ")}");
                data.Clear();

            
            }
        }

        private void FuZhi(int zongshebeiid, DataCunModel model, SerialDataGuDingJieXi jiexieqi, ABSLianJieQi lianjieqi, int timeout = 300)
        {
            if (model != null)
            {
                jiexieqi.Clear();
                DateTime chaoshi = DateTime.Now;
                int chaoshitime = timeout;
                List<byte> data = new List<byte>();
                List<byte> sends = GetTesShuBoolMa(model);
                byte teshugongma = (byte)model.TeShuGongNengMa;
                ResultModel rm = lianjieqi.Send(sends.ToArray());
                ChuFaMsg(MsgDengJi.SheBeiBaoWen, rm.Msg);
                for (; ZongKaiGuan && lianjieqi.TongXinState;)
                {
                    Thread.Sleep(1);
                    rm = lianjieqi.Read();
                    if (rm.IsSuccess)
                    {
                        jiexieqi.AddByteList(rm.TData);
                        data.AddRange(rm.TData);
                    }
                    jiexieqi.JieXiWanMeiData((model.Count ) + 3, new byte[] { (byte)1, teshugongma, (byte)(model.Count ) }, false, null, false);
                    if (jiexieqi.DataCount > 0)
                    {
                        byte[] xindata = jiexieqi.GetShiShiData();
                        if (xindata != null && xindata.Length >= model.Count  + 3)
                        {
                            PeiZhiLei.DataMoXing.SetJiCunQiValue(model, xindata.ToList());
                            break;
                        }
                    }
                    if ((DateTime.Now - chaoshi).TotalMilliseconds >= chaoshitime)
                    {

                        break;
                    }

                }

                ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"{zongshebeiid}读取的数据,{(DateTime.Now - chaoshi).TotalMilliseconds}:{ChangYong.ByteOrString(data, " ")}");
                data.Clear();


            }
        }

        private void WriteCMD(DataCunModel model,ABSLianJieQi lianJieQi)
        {      
            switch (model.DataType)
            {
                case DataType.Int:
                    {
                        byte[] shuju = model.GetZhi(model.JiCunQiModel.Value);
                        if (shuju != null)
                        {
                            List<byte> baowen = XieZhiLing(model.JiCunDiZhi, model.Count, shuju);
                            if (baowen.Count > 0)
                            {
                                lianJieQi.Send(baowen.ToArray());
                                PeiZhiLei.DataMoXing.SetZhengTaiState(model.SheBeiID, model.JiCunQiModel.WeiYiBiaoShi, 1);
                                ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"发送写报文:{ChangYong.ByteOrString(baowen, " ")}");
                            }
                            else
                            {
                                PeiZhiLei.DataMoXing.SetZhengTaiState(model.SheBeiID, model.JiCunQiModel.WeiYiBiaoShi, 2);

                                ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"{model.Name}发送写报文:没有报文数据");
                            }
                        }
                        else
                        {

                            PeiZhiLei.DataMoXing.SetZhengTaiState(model.SheBeiID,model.JiCunQiModel.WeiYiBiaoShi,2);

                            ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"{model.Name}发送写报文:数据没有解析出来");
                        }
                    }
                    break;
                case DataType.Float:
                    {
                        byte[] shuju = model.GetZhi(model.JiCunQiModel.Value);
                        if (shuju != null)
                        {
                            List<byte> baowen = XieZhiLing(model.JiCunDiZhi, model.Count, shuju);
                            if (baowen.Count > 0)
                            {
                                lianJieQi.Send(baowen.ToArray());
                                PeiZhiLei.DataMoXing.SetZhengTaiState(model.SheBeiID, model.JiCunQiModel.WeiYiBiaoShi, 1);
                                ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"发送写报文:{ChangYong.ByteOrString(baowen, " ")}");
                            }
                            else
                            {
                                PeiZhiLei.DataMoXing.SetZhengTaiState(model.SheBeiID, model.JiCunQiModel.WeiYiBiaoShi, 2);

                                ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"{model.Name}发送写报文:没有报文数据");
                            }
                        }
                        else
                        {

                            PeiZhiLei.DataMoXing.SetZhengTaiState(model.SheBeiID, model.JiCunQiModel.WeiYiBiaoShi, 2);

                            ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"{model.Name}发送写报文:数据没有解析出来");
                        }

                    }
                    break;
                case DataType.String:
                    {
                        byte[] shuju = model.GetZhi(model.JiCunQiModel.Value);
                        if (shuju != null)
                        {
                            List<byte> zifus = shuju.ToList();
                            zifus.Insert(0, (byte)shuju.Length);
                            zifus.Insert(0, 0xFE);
                            List<byte> baowen = XieZhiLing(model.JiCunDiZhi, model.Count, zifus.ToArray());
                            if (baowen.Count > 0)
                            {
                                lianJieQi.Send(baowen.ToArray());
                                PeiZhiLei.DataMoXing.SetZhengTaiState(model.SheBeiID, model.JiCunQiModel.WeiYiBiaoShi, 1);
                                ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"发送写报文:{ChangYong.ByteOrString(baowen, " ")}");
                            }
                            else
                            {
                                PeiZhiLei.DataMoXing.SetZhengTaiState(model.SheBeiID, model.JiCunQiModel.WeiYiBiaoShi, 2);

                                ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"{model.Name}发送写报文:没有报文数据");
                            }
                        }
                        else
                        {

                            PeiZhiLei.DataMoXing.SetZhengTaiState(model.SheBeiID, model.JiCunQiModel.WeiYiBiaoShi, 2);

                            ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"{model.Name}发送写报文:数据没有解析出来");
                        }
                    }
                    break;
                case DataType.String16OrACII:
                    {
                        byte[] shuju = model.GetZhi(model.JiCunQiModel.Value);
                        if (shuju != null)
                        {
                            List<byte> baowen = XieZhiLing(model.JiCunDiZhi, model.Count, shuju);
                            if (baowen.Count > 0)
                            {
                                lianJieQi.Send(baowen.ToArray());
                                PeiZhiLei.DataMoXing.SetZhengTaiState(model.SheBeiID, model.JiCunQiModel.WeiYiBiaoShi, 1);
                                ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"发送写报文:{ChangYong.ByteOrString(baowen, " ")}");
                            }
                            else
                            {
                                PeiZhiLei.DataMoXing.SetZhengTaiState(model.SheBeiID, model.JiCunQiModel.WeiYiBiaoShi, 2);

                                ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"{model.Name}发送写报文:没有报文数据");
                            }
                        }
                        else
                        {

                            PeiZhiLei.DataMoXing.SetZhengTaiState(model.SheBeiID, model.JiCunQiModel.WeiYiBiaoShi, 2);

                            ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"{model.Name}发送写报文:数据没有解析出来");
                        }
                    }
                    break;
                case DataType.Bool:
                    {
                        byte[] shuju = model.GetZhi(model.JiCunQiModel.Value);
                        if (shuju != null)
                        {
                           
                            if (shuju.Length > 0)
                            {
                                lianJieQi.Send(shuju);
                                PeiZhiLei.DataMoXing.SetZhengTaiState(model.SheBeiID, model.JiCunQiModel.WeiYiBiaoShi, 1);
                                ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"发送写报文:{ChangYong.ByteOrString(shuju, " ")}");
                            }
                            else
                            {
                                PeiZhiLei.DataMoXing.SetZhengTaiState(model.SheBeiID, model.JiCunQiModel.WeiYiBiaoShi, 2);

                                ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"{model.Name}发送写报文:没有报文数据");
                            }
                        }
                        else
                        {

                            PeiZhiLei.DataMoXing.SetZhengTaiState(model.SheBeiID, model.JiCunQiModel.WeiYiBiaoShi, 2);

                            ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"{model.Name}发送写报文:数据没有解析出来");
                        }
                    }
                    break;
                case DataType.TeShuBool:
                    {
                        byte[] shuju = model.GetZhi(model.JiCunQiModel.Value);
                        if (shuju != null)
                        {

                            if (shuju.Length > 0)
                            {
                                lianJieQi.Send(shuju);
                                PeiZhiLei.DataMoXing.SetZhengTaiState(model.SheBeiID, model.JiCunQiModel.WeiYiBiaoShi, 1);
                                ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"发送写报文:{ChangYong.ByteOrString(shuju, " ")}");
                            }
                            else
                            {
                                PeiZhiLei.DataMoXing.SetZhengTaiState(model.SheBeiID, model.JiCunQiModel.WeiYiBiaoShi, 2);

                                ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"{model.Name}发送写报文:没有报文数据");
                            }
                        }
                        else
                        {

                            PeiZhiLei.DataMoXing.SetZhengTaiState(model.SheBeiID, model.JiCunQiModel.WeiYiBiaoShi, 2);

                            ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"{model.Name}发送写报文:数据没有解析出来");
                        }
                    }
                    break;
                default:
                    {
                        PeiZhiLei.DataMoXing.SetZhengTaiState(model.SheBeiID, model.JiCunQiModel.WeiYiBiaoShi, 2);

                        ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"{model.Name}没有找到");
                    }
                    break;
            }
           
        }


        private List<byte> XieZhiLing(int dizhi, int count, byte[] shuju)
        {
            List<byte> baowen = new List<byte>();
            baowen.Add(0x00);
            baowen.Add(0x00);
            baowen.Add(0x00);
            baowen.Add(0x00);

            int changdu = 7 + count * 2;

            List<byte> sju2 = CRC.ShiOrByte2(changdu, true).ToList();
            baowen.Add(sju2[0]);
            baowen.Add(sju2[1]);
            baowen.Add(0x01);
            baowen.Add(0x10);
            List<byte> sju = CRC.ShiOrByte2(dizhi, true).ToList();
            baowen.Add(sju[0]);
            baowen.Add(sju[1]);
            baowen.Add(0x00);
            baowen.Add((byte)count);
            baowen.Add((byte)(count * 2));
            for (int i = 0; i < count * 2; i += 2)
            {
                if (shuju.Length > i)
                {
                    if (shuju.Length > i + 1)
                    {
                        baowen.Add(shuju[i + 1]);
                    }
                    else
                    {
                        baowen.Add(0x00);
                    }
                    baowen.Add(shuju[i]);
                }
                else
                {
                    baowen.Add(0x00);
                    baowen.Add(0x00);
                }
            }
            return baowen;
        }

        private List<byte> GetTesShuBoolMa(DataCunModel model)
        {
            //Tx:00 00 00 00 00 06 01 03 1F 37 00 01
            List<byte> baowen = new List<byte>();
            baowen.Add(0x00);
            baowen.Add(0x00);
            baowen.Add(0x00);
            baowen.Add(0x00); 
            baowen.Add(0x00);
            baowen.Add(0x06);
            baowen.Add(0x01);
            baowen.Add((byte)model.TeShuGongNengMa);
            List<byte> sju = CRC.ShiOrByte2(model.JiCunDiZhi, true).ToList();
            baowen.Add(sju[0]);
            baowen.Add(sju[1]);
            baowen.Add(0x00);
            baowen.Add(0x01);
         
            return baowen;
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
                SheBeiModel item = PeiZhiLei.DataMoXing.LisSheBei[i];
                ZiTxModel zmodel = new ZiTxModel();
                zmodel.Tx = item.Tx;
                zmodel.ZiSheBeiID = item.SheBeiID;
                zmodel.ZiSheBeiName = item.SheBeiName;
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
