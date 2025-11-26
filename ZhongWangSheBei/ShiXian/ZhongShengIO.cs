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
                return "中盛的IO继电器";
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
                else
                {
                    int zhen = PeiZhiLei.DataMoXing.IsChengGong(jicunqiid);
                    models.Value = zhen;
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
                #region 写
                try
                {
                    bool shuju = sengData.IsEmpty;
                    if (shuju == false)
                    {
                        naxieyaodu.Clear();
                        int count = sengData.Count;
                        for (int i = 0; i < count; i++)
                        {
                            List<JiCunQiModel> duixiang = new List<JiCunQiModel>();
                            bool zhen = sengData.TryDequeue(out duixiang);
                            if (zhen && duixiang.Count > 0)
                            {
                                foreach (var item in duixiang)
                                {
                                    int dizhi = GetDiZhi(item);
                                    if (dizhi >= 0)
                                    {
                                        TongYi(item, lianjieqi);
                                        if (naxieyaodu.IndexOf(dizhi) < 0)
                                        {
                                            naxieyaodu.Add(dizhi);
                                        }
                                       
                                        Thread.Sleep(10);
                                    }
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
                            model.CiShu=5;
                        }
                        break;

                    }
                    if ((DateTime.Now - date).TotalMilliseconds >= model.ChaoShiTime)
                    {
                        model.CiShu--;
                       
                        ChuFaMsg(MsgDengJi.SheBeiBaoWen,$"数据读取超时");
                        break;
                    }
                }
                if (model.CiShu<0)
                {
                    PeiZhiLei.DataMoXing.SetTX(zongshebeiid, model.ZSID, false, false);
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

        /// <summary>
        /// 闭合继电器通道
        /// </summary>
        /// <param name="ch">通道</param>
        /// <param name="sw">开关状态</param>
        /// <returns></returns>
        private void TongYi(JiCunQiModel model,ABSLianJieQi lianjieqi)
        {
            Dictionary<string, CunModel> sModels = PeiZhiLei.DataMoXing.JiLu;
            if (sModels.ContainsKey(model.WeiYiBiaoShi))
            {
                CunModel models = sModels[model.WeiYiBiaoShi];
                switch (models.IsDu)
                {
                 
                    case CunType.Xie全开:
                        {
                            if (models.JiLu < 49)
                            {
                                byte[] shuju = QuanDongZuos((byte)models.ZDiZhi, false);
                                lianjieqi.Send(shuju);
                            }
                            else
                            {
                                byte[] shuju = XieDuoGe((byte)models.ZDiZhi,0,72, true);
                                lianjieqi.Send(shuju);
                                Thread.Sleep(300);
                            }
                            if (models.XieRuChaoShi > 0)
                            {
                                Thread.Sleep(models.XieRuChaoShi);
                            }
                        }
                        break;
                    case CunType.Xie全关:
                        {
                            if (models.JiLu < 49)
                            {
                                byte[] shuju = QuanDongZuos((byte)models.ZDiZhi, true);
                                lianjieqi.Send(shuju);
                            }
                            else
                            {
                                byte[] shuju = XieDuoGe((byte)models.ZDiZhi, 0, 72, false);
                                lianjieqi.Send(shuju);
                                Thread.Sleep(300);

                            }
                            if (models.XieRuChaoShi > 0)
                            {
                                Thread.Sleep(models.XieRuChaoShi);
                            }
                        }
                        break;
                    case CunType.Xie开:
                        {
                            List<int> jidianqi = ChangYong.JieGeInt(model.Value.ToString(), '|');
                            Dictionary<byte, byte> jiexie = new Dictionary<byte, byte>();
                            byte jilu = 0;
                            int shujiaru = 1;
                            for (int i = 0; i < jidianqi.Count; i++)
                            {
                                byte jicunqidizhi = (byte)(jidianqi[i] - 1);

                                if (i == 0)
                                {
                                    jilu = jicunqidizhi;

                                    if (jiexie.ContainsKey(jilu) == false)
                                    {
                                        jiexie.Add(jilu, 1);
                                    }
                                }
                                else
                                {
                                    if (jicunqidizhi - jilu == shujiaru)
                                    {
                                        jiexie[jilu]++;
                                        shujiaru++;
                                    }
                                    else
                                    {
                                        jilu = jicunqidizhi;
                                        shujiaru = 1;
                                        if (jiexie.ContainsKey(jilu) == false)
                                        {
                                            jiexie.Add(jilu, 1);
                                        }
                                    }

                                }
                            }
                            List<byte> shulangs = jiexie.Keys.ToList();
                            for (int i = 0; i < shulangs.Count; i++)
                            {
                                byte ss = jiexie[shulangs[i]];
                                if (ss == 1)
                                {
                                  
                                    byte[] dange = XieYiGe((byte)models.ZDiZhi, shulangs[i], true);
                                    lianjieqi.Send(dange);
                                    Thread.Sleep(5);
                                }
                                else
                                {
                                    byte[] dange = XieDuoGe((byte)models.ZDiZhi, shulangs[i], ss, true);
                                    lianjieqi.Send(dange);
                                    Thread.Sleep(5);

                                }
                                if (models.XieRuChaoShi > 0)
                                {
                                    Thread.Sleep(models.XieRuChaoShi);
                                }
                            }
                        }
                        break;
                    case CunType.Xie关:
                        {
                            List<int> jidianqi = ChangYong.JieGeInt(model.Value.ToString(), '|');
                            Dictionary<byte, byte> jiexie = new Dictionary<byte, byte>();
                            byte jilu = 0;
                            int shujiaru = 1;
                            for (int i = 0; i < jidianqi.Count; i++)
                            {
                                byte jicunqidizhi = (byte)(jidianqi[i] - 1);

                                if (i == 0)
                                {
                                    jilu = jicunqidizhi;

                                    if (jiexie.ContainsKey(jilu) == false)
                                    {
                                        jiexie.Add(jilu, 1);
                                    }
                                }
                                else
                                {
                                    if (jicunqidizhi - jilu == shujiaru)
                                    {
                                        jiexie[jilu]++;
                                        shujiaru++;
                                    }
                                    else
                                    {
                                        jilu = jicunqidizhi;
                                        shujiaru = 1;
                                        if (jiexie.ContainsKey(jilu) == false)
                                        {
                                            jiexie.Add(jilu, 1);
                                        }
                                    }

                                }
                            }
                            List<byte> shulangs = jiexie.Keys.ToList();
                            for (int i = 0; i < shulangs.Count; i++)
                            {
                                byte ss = jiexie[shulangs[i]];
                                if (ss == 1)
                                {
                                    byte[] dange = XieYiGe((byte)models.ZDiZhi, shulangs[i], false);
                                    lianjieqi.Send(dange);
                                    Thread.Sleep(5);
                                }
                                else
                                {
                                    byte[] dange = XieDuoGe((byte)models.ZDiZhi, shulangs[i], ss, false);
                                    lianjieqi.Send(dange);
                                    Thread.Sleep(5);

                                }
                                if (models.XieRuChaoShi > 0)
                                {
                                    Thread.Sleep(models.XieRuChaoShi);
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
                    
        }


        private byte[] QuanDongZuos(byte addr, bool isquanguan)
        {
            List<byte> buf = new List<byte>();
            buf.Add(addr);
            buf.Add(0x06);
            buf.Add(0x00);
            buf.Add(0x34);
            buf.Add(0x00);
            if (isquanguan)
            {
                buf.Add(0x00);
            }
            else
            {
                buf.Add(0x01);
            }
          
            byte[] crcdata = CRC.ToModbus(buf, false);
            buf.AddRange(crcdata);
            return buf.ToArray();
        }

      
        private byte[] XieDuoGe( byte addr, byte jicundizhi, byte shuliang, bool isguan)
        {

            List<byte> buf = new List<byte>();
            buf.Add(addr);
            buf.Add(0x10);
            buf.Add(0x00);
            buf.Add(jicundizhi);
            buf.Add(0x00);
            buf.Add(shuliang);
            buf.Add((byte)(shuliang * 2));
            for (int i = 0; i < shuliang; i++)
            {
                if (isguan)
                {
                    buf.Add(0x00);
                    buf.Add(0x01);
                }
                else
                {
                    buf.Add(0x00);
                    buf.Add(0x00);
                }
            }
            byte[] crcdata = CRC.ToModbus(buf,false);
            buf.AddRange(crcdata);
            return buf.ToArray();
        }

        private byte[] XieYiGe(byte addr, byte ch, bool iskai)
        {
        

            List<byte> buf = new List<byte>();
            buf.Add(addr);
            buf.Add(0x06);
            buf.Add(0x00);
            buf.Add((byte)ch);
            buf.Add(0x00);
            if (iskai)
            {
                buf.Add(0x01);
            }
            else
            {
                buf.Add(0x00);
            }

            byte[] crcdata = CRC.ToModbus(buf, false);
            buf.AddRange(crcdata);
            return buf.ToArray();
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

        private int GetDiZhi(JiCunQiModel jicunqi)
        {
           
            Dictionary<string, CunModel> sModels = PeiZhiLei.DataMoXing.JiLu;
            if (sModels.ContainsKey(jicunqi.WeiYiBiaoShi))
            {
                CunModel models = sModels[jicunqi.WeiYiBiaoShi];
                if (models.IsDu != CunType.DuJiCunQi)
                {
                    
                    return models.ZiSheBeiID;
                }
            }
            return -1;
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
