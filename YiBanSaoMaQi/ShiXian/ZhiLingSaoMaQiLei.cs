using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
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
                return "一般扫码器";
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

        private Dictionary<int, FanXingJiHeLei<List<CunModel>>> SengData = new Dictionary<int, FanXingJiHeLei<List<CunModel>>>();


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

                        SengData.Add(sModels[i].SheBeiID, new FanXingJiHeLei<List<CunModel>>());
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
            Clear(false,obj);
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
        private CunModel GetDiZhi(JiCunQiModel jicunqi)
        {
            CunModel model = PeiZhiLei.DataMoXing.GetModel(jicunqi);
            if (model!=null)
            {
                CunModel models = model.FuZhi();
                PeiZhiLei.DataMoXing.SetZhuangTaiZhi(models, 0);
                models.JiCunQi = jicunqi;
                return models;
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
                    if (LisLianJieQi.ContainsKey(ipcom.ZongSheBeiId))
                    {
                        SengData[ipcom.ZongSheBeiId].Add(new List<CunModel>() { ipcom });
                    }
                }
            }
        }
        public override JiaoYanJieGuoModel JiaoYanChengGong(JiCunQiModel jicunqiid)
        {
            JiaoYanJieGuoModel models = new JiaoYanJieGuoModel();
            models.WeiYiBiaoShi = jicunqiid.WeiYiBiaoShi;
            models.SheBeiID = jicunqiid.SheBeiID;
            CunModel xinr = PeiZhiLei.DataMoXing.IsChengGong(jicunqiid.WeiYiBiaoShi);
            if (xinr != null)
            {
                if (xinr.IsZhengZaiCe == 1)
                {
                    models.Value = xinr.JiCunQi.Value;
                    models.IsZuiZhongJieGuo = JieGuoType.ChengGongJiGuo;
                }
                else if (xinr.IsZhengZaiCe == 3)
                {
                    models.Value = "";
                    models.IsZuiZhongJieGuo = JieGuoType.ShiBaiJiGuo;
                }
                else if (xinr.IsZhengZaiCe == 2)
                {
                    models.Value ="";
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
                models.Value = "";
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
            FanXingJiHeLei<List<CunModel>> sengData = SengData[zsmodel.SheBeiID];

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
                        List<CunModel> duixiang = new List<CunModel>();
                        duixiang = sengData.GetModel_Head_RomeHead();

                        int count = duixiang.Count;
                        for (int i = 0; i < count; i++)
                        {
                            CunModel dizhi = duixiang[i];
                            if (dizhi!=null)
                            {
                                switch (dizhi.IsDu)
                                {
                                    case CunType.Xie开启设备:
                                        {
                                            bool zhen = lianjieqi.ChongLian();
                                            PeiZhiLei.DataMoXing.SetState(dizhi.ZongSheBeiId,zhen);
                                            PeiZhiLei.DataMoXing.SetJiCunQiValue(dizhi,zhen?"OK":"NG");
                                            PeiZhiLei.DataMoXing.SetZhuangTaiZhi(dizhi, zhen ? 1 : 2);
                                        }
                                        break;
                                    case CunType.Xie开启扫码:
                                        {
                                            WriteRec(lianjieqi, dizhi, zsmodel);
                                        }
                                        break;
                                    case CunType.Xie关闭扫码:
                                        {
                                            try
                                            {
                                                SaoMaModel maModel = PeiZhiLei.DataMoXing.GetSheBeiModel(dizhi);
                                                if (maModel != null)
                                                {
                                                    if (maModel.FaGeShi == 1)
                                                    {
                                                        byte[] data = Encoding.ASCII.GetBytes(maModel.JieGuoSaoMaZhiLing);
                                                        lianjieqi.Send(data);
                                                    }
                                                    else if (maModel.FaGeShi == 2)
                                                    {
                                                        byte[] data = ChangYong.HexStringToByte(maModel.JieGuoSaoMaZhiLing);
                                                        lianjieqi.Send(data);
                                                    }

                                                }
                                            }
                                            catch 
                                            {

                                          
                                            }                                       
                                            PeiZhiLei.DataMoXing.SetJiCunQiValue(dizhi, "OK" );
                                            PeiZhiLei.DataMoXing.SetZhuangTaiZhi( dizhi, 1);
                                        }
                                        break;
                                    case CunType.Xie关闭设备:
                                        {
                                            lianjieqi.Close();
                                            PeiZhiLei.DataMoXing.SetState(dizhi.ZongSheBeiId, false);
                                            PeiZhiLei.DataMoXing.SetJiCunQiValue(dizhi, "OK");
                                            PeiZhiLei.DataMoXing.SetZhuangTaiZhi(dizhi, 1);
                                        }
                                        break;
                                    default:
                                        break;
                                }
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

        public void WriteRec(ABSLianJieQi lianjieqi,CunModel model, SaoMaModel maModel)
        {         
       
            if (maModel != null)
            {
                int cishu = 3;
                for (int i = 0; i < cishu; i++)
                {
                    byte[] data = GetSendData(maModel);
                    lianjieqi.Send(data);                 
                    DateTime shijian = DateTime.Now;
                    List<byte> datalis = new List<byte>();
                    int changdu = ChangYong.TryInt(maModel.ChangDu, 0);
                    for (; ZongKaiGuan;)
                    {
                        ResultModel rm = lianjieqi.Read();
                        if (rm.IsSuccess)
                        {
                            datalis.AddRange(rm.TData);
                        }
                        if (maModel.JieXieDataType == JieXieDataType.ChangDu)
                        {
                            if (datalis.Count >= changdu)
                            {
                                string msg = "";
                                if (maModel.JieXiType == 1)
                                {
                                    msg = Encoding.ASCII.GetString(datalis.ToArray());

                                }
                                else if (maModel.JieXiType == 2)
                                {
                                    msg = ChangYong.ByteOrString(datalis, "");
                                }
                                bool zhen= SetMa(model, msg, maModel.Name, datalis,i>=cishu-1);
                                if (zhen)
                                {
                                    return;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        else if (maModel.JieXieDataType == JieXieDataType.FenGe)
                        {
                            if (maModel.JieXiType == 1)
                            {
                                if (datalis.Count > 0)
                                {
                                    string shuju = Encoding.ASCII.GetString(datalis.ToArray());
                                    if (shuju.Contains(maModel.ChangDu))
                                    {
                                        try
                                        {
                                            string msg = shuju.Split(new string[] { maModel.ChangDu }, StringSplitOptions.None)[0];
                                            bool zhen = SetMa(model, msg, maModel.Name, datalis, i >= cishu - 1);
                                            if (zhen)
                                            {
                                                return;
                                            }
                                            else
                                            {
                                                break;
                                            }

                                        }
                                        catch
                                        {


                                        }

                                        return;
                                    }
                                }
                            }
                            else if (maModel.JieXiType == 2)
                            {
                                if (datalis.Count > 0)
                                {
                                    string shuju = ChangYong.ByteOrString(datalis, "");
                                    if (shuju.Contains(maModel.ChangDu))
                                    {
                                        try
                                        {
                                            string msg = shuju.Split(new string[] { maModel.ChangDu }, StringSplitOptions.None)[0];
                                            bool zhen = SetMa(model, msg, maModel.Name, datalis, i >= cishu - 1);
                                            if (zhen)
                                            {
                                                return;
                                            }
                                            else
                                            {
                                                break;
                                            }

                                        }
                                        catch
                                        {


                                        }

                                        return;
                                    }
                                }
                            }
                        }
                        else if (maModel.JieXieDataType == JieXieDataType.BaoHan)
                        {
                            if (maModel.JieXiType == 1)
                            {
                                if (datalis.Count > 0)
                                {
                                    string shuju = Encoding.ASCII.GetString(datalis.ToArray());
                                    if (shuju.Contains(maModel.ChangDu))
                                    {
                                        try
                                        {
                                            bool zhen = SetMa(model, shuju, maModel.Name, datalis, i >= cishu - 1);
                                            if (zhen)
                                            {
                                                return;
                                            }
                                            else
                                            {
                                                break;
                                            }

                                        }
                                        catch
                                        {


                                        }

                                        return;
                                    }
                                }
                            }
                            else if (maModel.JieXiType == 2)
                            {
                                if (datalis.Count > 0)
                                {
                                    string shuju = ChangYong.ByteOrString(datalis, "");
                                    if (shuju.Contains(maModel.ChangDu))
                                    {
                                        try
                                        {

                                            bool zhen = SetMa(model, shuju, maModel.Name, datalis, i >= cishu - 1);
                                            if (zhen)
                                            {
                                                return;
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                        catch
                                        {


                                        }

                                        return;
                                    }
                                }
                            }
                        }
                        if ((DateTime.Now - shijian).TotalMilliseconds >= maModel.Time)
                        {
                            if (i == cishu - 1)
                            {
                                PeiZhiLei.DataMoXing.SetZhuangTaiZhi(model, 3);
                            }
                            ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"{maModel.Name}:接收数据超时{ChangYong.ByteOrString(datalis, " ")}");
                            break;
                        }
                        Thread.Sleep(2);
                    }
                    Thread.Sleep(20);
                }

            }
            else
            {
                PeiZhiLei.DataMoXing.SetZhuangTaiZhi(model, 2);
            }
        }

        private byte[] GetSendData(SaoMaModel maModel)
        {
            if (maModel.FaGeShi == 1)
            {
                byte[] data = Encoding.ASCII.GetBytes(maModel.KaiShiSaoMaZhiLing);
                ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"{maModel.Name}:发送指令:{ChangYong.ByteOrString(data, " ")} {maModel.KaiShiSaoMaZhiLing}");
                return data;
            }
            else if (maModel.FaGeShi == 2)
            {
                byte[] data = ChangYong.HexStringToByte(maModel.KaiShiSaoMaZhiLing);
                ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"{maModel.Name}:发送指令:{ChangYong.ByteOrString(data, " ")} {maModel.KaiShiSaoMaZhiLing}");
                return data;
            }
            return null;
        }
        private bool SetMa(CunModel model, string msg,string shebeiname,List<byte> datalis,bool isqiangzhi1xie)
        {
            if (isqiangzhi1xie)
            {
                PeiZhiLei.DataMoXing.SetJiCunQiValue(model, msg);
                PeiZhiLei.DataMoXing.SetZhuangTaiZhi(model, 1);
                ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"{shebeiname}:接收数据:{ChangYong.ByteOrString(datalis, " ")},{msg}");
                datalis.Clear();
                PeiZhiLei.DataMoXing.ShangYiCiMa[model.ZongSheBeiId] = msg;
                return true;
            }
            else
            {
                string shangcima = PeiZhiLei.DataMoXing.ShangYiCiMa[model.ZongSheBeiId];
                if (msg.Equals(shangcima) == false)
                {
                    PeiZhiLei.DataMoXing.SetJiCunQiValue(model, msg);
                    PeiZhiLei.DataMoXing.SetZhuangTaiZhi(model, 1);
                    ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"{shebeiname}:接收数据:{ChangYong.ByteOrString(datalis, " ")},{msg}");
                    datalis.Clear();
                    PeiZhiLei.DataMoXing.ShangYiCiMa[model.ZongSheBeiId] = msg;
                    return true;
                }
                else
                {
                    PeiZhiLei.DataMoXing.ShangYiCiMa[model.ZongSheBeiId] = msg;
                    return false;
                }
            }
          
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
                SaoMaModel item=PeiZhiLei.DataMoXing.LisSheBei[i];
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
    }
}
