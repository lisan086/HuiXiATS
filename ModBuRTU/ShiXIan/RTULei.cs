using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using SundyChengZhong.Frm;
using SundyChengZhong.Model;

namespace SundyChengZhong.ShiXIan
{
    public class RTULei : ABSNSheBei
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
                return "MobusRTU";
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
        /// <summary>
        /// 连接器
        /// </summary>
        private Dictionary<int, ABSLianJieQi> LisLianJieQi = new Dictionary<int, ABSLianJieQi>();

        private Dictionary<int, FanXingJiHeLei<List<CZJiCunQiModel>>> SengData = new Dictionary<int, FanXingJiHeLei<List<CZJiCunQiModel>>>();
        public RTULei()
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
                ChuFaMsg(MsgDengJi.SheBeiZhengChang,"初始化称重设备");
                PeiZhiLei.IniData(SheBeiID,SheBeiName);
                PeiZhiLei.XieIOEvent += ZhongShengIO_XieIOEvent;
                List<SheBeiModel> sheBeiModels = PeiZhiLei.DataMoXing.LisSheBei;
                ZongKaiGuan = true;
                for (int i = 0; i < sheBeiModels.Count; i++)
                {
                    SetLianJieQiModel model = new SetLianJieQiModel();
                    model.IPOrCOMStr = sheBeiModels[i].Com;
                    model.SpeedOrPort = sheBeiModels[i].Port;
                    ABSLianJieQi LianJieQi = new RJ232LianJieQi();
                    LianJieQi.SetCanShu(model);
                    if (LisLianJieQi.ContainsKey(sheBeiModels[i].SheBeiID)==false)
                    {
                        LisLianJieQi.Add(sheBeiModels[i].SheBeiID, LianJieQi);
                        SengData.Add(sheBeiModels[i].SheBeiID,new FanXingJiHeLei<List<CZJiCunQiModel>>());
                    }
                    Thread DuTH = new Thread(ReadWork);
                    DuTH.DisableComObjectEagerCleanup();
                    DuTH.IsBackground = true;
                    DuTH.Start(sheBeiModels[i]);
                }
                DengDaiOpen = true;
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
                Dictionary<int, List<CZJiCunQiModel>> lisd = new Dictionary<int, List<CZJiCunQiModel>>();
                for (int i = 0; i < canshus.Count; i++)
                {
                    CZJiCunQiModel dataCunModel = PeiZhiLei.DataMoXing.GetModel(canshus[i].WeiYiBiaoShi, true);
                    if (dataCunModel != null && dataCunModel.DataSType != DataSType.DuPuTong)
                    {
                        PeiZhiLei.DataMoXing.SetZhengTaiState(dataCunModel.SheBeiID,dataCunModel.JiCunQiModel.WeiYiBiaoShi,0);
                        dataCunModel.JiCunQiModel=canshus[i];
                        if (lisd.ContainsKey(dataCunModel.SheBeiID) == false)
                        {
                            lisd.Add(dataCunModel.SheBeiID, new List<CZJiCunQiModel>());
                        }
                        lisd[dataCunModel.SheBeiID].Add(dataCunModel);
                    }
                }
                foreach (var item in lisd.Keys)
                {
                    if (SengData.ContainsKey(item))
                    {
                        SengData[item].Add(lisd[item]);
                    }
                }
            }
        }

        public override JiaoYanJieGuoModel JiaoYanChengGong(JiCunQiModel jicunqiid)
        {
            JiaoYanJieGuoModel models = new JiaoYanJieGuoModel();
            models.WeiYiBiaoShi = jicunqiid.WeiYiBiaoShi;
            models.SheBeiID = jicunqiid.SheBeiID;
          
            CZJiCunQiModel zhen = PeiZhiLei.DataMoXing.GetModel(jicunqiid.WeiYiBiaoShi, true);
            if (DengDaiOpen)
            {
                if (zhen != null)
                {
                    if (zhen.JiCunQiModel.IsKeKao)
                    {
                        if (zhen.DataSType == DataSType.DuPuTong)
                        {
                            models.Value = zhen.JiCunQiModel.Value;
                            models.IsZuiZhongJieGuo = JieGuoType.ChengGongJiGuo;
                        }
                        else if (zhen.DataSType == DataSType.DuXieYiQi)
                        {
                            if (zhen.IsXieWan == 1)
                            {
                                models.Value = zhen.JiCunQiModel.Value;
                                models.IsZuiZhongJieGuo = JieGuoType.ChengGongJiGuo;
                            }
                            else if (zhen.IsXieWan >= 2)
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
                            if (zhen.IsXieWan == 1)
                            {
                                models.Value = zhen.JiCunQiModel.Value;
                                models.IsZuiZhongJieGuo = JieGuoType.ChengGongJiGuo;
                            }
                            else if (zhen.IsXieWan >= 2)
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
                    }
                    else
                    {
                        models.Value = zhen.JiCunQiModel.Value;
                        models.IsZuiZhongJieGuo = JieGuoType.BuKeKaoJieGuo;
                    }
                }
                else
                {
                    models.Value = "没有找到";
                    models.IsZuiZhongJieGuo = JieGuoType.MeiZhaoDaoJiGuo;
                }
            }
            else
            {
                models.Value = "设备没有打开";
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
            jieMianFrmModel.Form = new PeiZhiFrm(PeiZhiLei);
            return jieMianFrmModel;
        }

        public override List<JiCunQiModel> GetShuJu()
        {

            List<JiCunQiModel> Get = PeiZhiLei.DataMoXing.LisDu;

            return Get;
        }

        private void ReadWork(object zhi)
        {
            SheBeiModel zsmodel = zhi as SheBeiModel;
            ABSLianJieQi lianjieqi = LisLianJieQi[zsmodel.SheBeiID];
            int yanshi = 5;
            FanXingJiHeLei<List<CZJiCunQiModel>> sengData = SengData[zsmodel.SheBeiID];
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
                if (lianjieqi.TongXinState == false)
                {
                    PeiZhiLei.DataMoXing.SetTx(zsmodel.SheBeiID, false);
                    if ((DateTime.Now - chongliantime).TotalMilliseconds >= 500)
                    {
                        bool zhen = lianjieqi.ChongLian();
                        chongliantime = DateTime.Now;
                        PeiZhiLei.DataMoXing.SetTx(zsmodel.SheBeiID, zhen);
                    }
                   
                }

                #region 写
                try
                {

                    int count = sengData.GetCount();
                    if (count > 0)
                    {
                        List<CZJiCunQiModel> duixiang = sengData.GetModel_Head_RomeHead();
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
                        List<JiLuModel> lis = PeiZhiLei.DataMoXing.GetJiLuSheBei(zsmodel.SheBeiID);
                        for (int i = 0; i < lis.Count; i++)
                        {
                            FuZhi(zsmodel.SheBeiID, lis[i], dingJieXi, lianjieqi,zsmodel.DuYanShi);
                        }
                    }

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
                int changdu = model.Count / 8;
                int yuchang = model.Count % 8;
                if (yuchang>0)
                {
                    changdu++;
                }
                if (model.IsXianQuan)
                {
                    for (; ZongKaiGuan && lianjieqi.TongXinState;)
                    {
                        Thread.Sleep(1);
                        rm = lianjieqi.Read();
                        if (rm.IsSuccess)
                        {
                            jiexieqi.AddByteList(rm.TData);
                            data.AddRange(rm.TData);
                        }
                        jiexieqi.JieXiWanMeiData((changdu) + 5, new byte[] { (byte)model.SheBeiDiZhi, 0x03, (byte)(changdu) }, true, JiaoYanWenDu, false);
                        if (jiexieqi.DataCount > 0)
                        {
                            byte[] xindata = jiexieqi.GetShiShiData();
                            if (xindata != null && xindata.Length >= (changdu) + 5)
                            {
                                PeiZhiLei.DataMoXing.SetJiCunQiValue(zongshebeiid, xindata.ToList(), model);

                                break;
                            }
                        }
                        if ((DateTime.Now - chaoshi).TotalMilliseconds >= chaoshitime)
                        {
                          //  PeiZhiLei.DataMoXing.SetTxBuHeGe(zongshebeiid, model.SheBeiDiZhi);
                            break;
                        }

                    }
                }
                else
                {
                    for (; ZongKaiGuan && lianjieqi.TongXinState;)
                    {
                        Thread.Sleep(1);
                        rm = lianjieqi.Read();
                        if (rm.IsSuccess)
                        {
                            jiexieqi.AddByteList(rm.TData);
                            data.AddRange(rm.TData);
                        }
                        jiexieqi.JieXiWanMeiData((model.Count * 2) + 5, new byte[] { (byte)model.SheBeiDiZhi, 0x03, (byte)(model.Count * 2) }, true, JiaoYanWenDu, false);
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
                           // PeiZhiLei.DataMoXing.SetTxBuHeGe(zongshebeiid, model.SheBeiDiZhi);
                            break;
                        }

                    }
                }

                ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"{zongshebeiid}读取的数据,{(DateTime.Now - chaoshi).TotalMilliseconds}:{ChangYong.ByteOrString(data, " ")}");
                data.Clear();


            }
        }

       

        private bool JiaoYanWenDu(List<byte> canshu)
        {
            if (canshu == null || canshu.Count <= 1)
            {
                return false;
            }
            List<byte> data = canshu.GetRange(0, canshu.Count - 2);
            byte[] shuju = CRC.ToModbus(data, false);
            if (shuju != null && shuju.Length >= 2)
            {
                if (shuju[0] == canshu[canshu.Count - 2] && shuju[1] == canshu[canshu.Count - 1])
                {
                    return true;
                }
            }
            return true;
        }


        /// <summary>
        /// 获取十进制值
        /// </summary>
        /// <param name="len"></param>
        /// <param isgaoweizaiqian="len">true高位在前，低位在后</param>
        /// <returns></returns>
        public List<byte> GetShiJianZhi(int len, bool isgaoweizaiqian)
        {
            List<byte> shuju = new List<byte>();
            byte[] intBuff = BitConverter.GetBytes(len);
            if (isgaoweizaiqian)
            {
                if (intBuff.Length >= 2)
                {
                    shuju.Add(intBuff[1]);
                    shuju.Add(intBuff[0]);
                }
            }
            else
            {
                if (intBuff.Length >= 2)
                {
                    shuju.Add(intBuff[0]);
                    shuju.Add(intBuff[1]);
                }
            }
            return shuju;
        }

        private void WriteCMD(CZJiCunQiModel zhenshi, ABSLianJieQi lianjieqi)
        {
            if (zhenshi.XieGNM == 5)
            {
                List<byte> xieshuju = new List<byte>();
                xieshuju.Add((byte)zhenshi.SheBeiDiZhi);
                xieshuju.Add((byte)zhenshi.XieGNM);
                int dizhi = zhenshi.JiCunDiZhi;
                byte[] dishi = PeiZhiLei.DataMoXing.GetBtyez(dizhi);
                xieshuju.AddRange(dishi);
                if (ChangYong.TryInt(zhenshi.JiCunQiModel.Value, 0) == 1)
                {
                    xieshuju.Add(0xFF);
                    xieshuju.Add(0x00);
                }
                else
                {
                    xieshuju.Add(0x00);
                    xieshuju.Add(0x00);
                }
                byte[] shu = CRC.ToModbus(xieshuju, false);
                xieshuju.AddRange(shu);
                if (xieshuju.Count > 0)
                {
                    lianjieqi.Send(xieshuju.ToArray());
                    PeiZhiLei.DataMoXing.SetZhengTaiState(zhenshi.SheBeiID, zhenshi.JiCunQiModel.WeiYiBiaoShi, 1);
                    ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"发送写报文:{ChangYong.ByteOrString(xieshuju, " ")}");
                }
                else
                {
                    PeiZhiLei.DataMoXing.SetZhengTaiState(zhenshi.SheBeiID, zhenshi.JiCunQiModel.WeiYiBiaoShi, 2);
                    ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"发送写报文:{ChangYong.ByteOrString(xieshuju, " ")}");
                }
            }
            else
            {
                List<byte> xieshuju = new List<byte>();
                xieshuju.Add((byte)zhenshi.SheBeiDiZhi);
                xieshuju.Add(0x06);
                int dizhi = zhenshi.JiCunDiZhi;
                byte[] dishi = PeiZhiLei.DataMoXing.GetBtyez(dizhi);
                xieshuju.AddRange(dishi);
                float zhi = ChangYong.TryFloat(zhenshi.JiCunQiModel.Value, 0);

                if (zhenshi.BeiChuShu == 0)
                {
                    zhenshi.BeiChuShu = 1;

                }
                if (zhenshi.XiaoShuWei <= 0)
                {
                    zhenshi.XiaoShuWei = 1;

                }
                if (zhenshi.XiaoShuWei >= 15)
                {
                    zhenshi.XiaoShuWei = 14;
                }
                int xinzhi = (int)Math.Round(((zhi-zhenshi.BZhi) * zhenshi.BeiChuShu), zhenshi.XiaoShuWei);

                byte[] dishsi = GetShiJianZhi(xinzhi, true).ToArray();
                xieshuju.AddRange(dishsi);
                byte[] shu = CRC.ToModbus(xieshuju, false);
                xieshuju.AddRange(shu);
                if (xieshuju.Count > 0)
                {
                    lianjieqi.Send(xieshuju.ToArray());
                    PeiZhiLei.DataMoXing.SetZhengTaiState(zhenshi.SheBeiID, zhenshi.JiCunQiModel.WeiYiBiaoShi, 1);
                    ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"发送写报文:{ChangYong.ByteOrString(xieshuju, " ")}");
                }
                else
                {
                    PeiZhiLei.DataMoXing.SetZhengTaiState(zhenshi.SheBeiID, zhenshi.JiCunQiModel.WeiYiBiaoShi, 2);
                    ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"发送写报文:{ChangYong.ByteOrString(xieshuju, " ")}");
                }
            }
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
