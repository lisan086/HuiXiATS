using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using CommLei.DataChuLi;
using CommLei.GongYeJieHe;
using CommLei.JiChuLei;
using DBPLCS7.Frm.frm;
using DBPLCS7.Frm.KJ;
using DBPLCS7.Frm.Lei;
using DBPLCS7.Model;
using S7.Net;
using SSheBei.ABSSheBei;
using SSheBei.CRCJiaoYan;
using SSheBei.LianJieQi;
using SSheBei.Model;

namespace DBPLCS7.ShiXian
{
    public  class XiMenPLC: ABSNSheBei
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
                return "西门子PLC";
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
                    if (LisLianJieQi[item].IsConnected == false)
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
        public  Dictionary<int, Plc> LisLianJieQi = new Dictionary<int, Plc>();

        private Dictionary<int, FanXingJiHeLei<List<PLCJiCunQiModel>>> SengData = new Dictionary<int, FanXingJiHeLei<List<PLCJiCunQiModel>>>();


        public XiMenPLC()
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
                PeiZhiLei.XieIOEvent += PeiZhiLei_XieIOEvent;
                List<PLCShBeiModel> sModels = PeiZhiLei.MoXing.LisSheBei;
                for (int i = 0; i < sModels.Count; i++)
                {
                    if (LisLianJieQi.ContainsKey(sModels[i].Port) == false)
                    {
                        Plc PLCS7 = null;
                        CpuType cpuType = CpuType.S7200;
                        if (sModels[i].PCLType == PCLType.PLC300_1200)
                        {
                            cpuType=CpuType.S71200;
                        }
                        else if (sModels[i].PCLType == PCLType.PLC1500)
                        {
                            cpuType = CpuType.S71500;
                        }
                        //创建PLC对象
                        PLCS7 = new Plc(cpuType, sModels[i].IP, sModels[i].Rack, sModels[i].Slot);
                        LisLianJieQi.Add(sModels[i].Port, PLCS7);

                        SengData.Add(sModels[i].Port, new FanXingJiHeLei<List<PLCJiCunQiModel>>());
                        Thread xiancheng = new Thread(ReadWork);
                        xiancheng.IsBackground = true;
                        xiancheng.DisableComObjectEagerCleanup();
                        xiancheng.Start(sModels[i]);

                    }
                }
            }
        }

        private void PeiZhiLei_XieIOEvent(JiCunQiModel obj)
        {
            XieShuJu(new List<JiCunQiModel>() { obj });
        }

        public override void Open()
        {
           
            foreach (var item in LisLianJieQi.Keys)
            {
                try
                {
                    LisLianJieQi[item].Open();
                    PeiZhiLei.MoXing.SetTx(item, LisLianJieQi[item].IsConnected);
                    if (LisLianJieQi[item].IsConnected)
                    {
                        ChuFaMsg(MsgDengJi.SheBeiZhengChang, $"{item} 打开成功");                      
                    }
                    else
                    {
                        ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"{item} 打开失败");
                    }
                }
                catch (Exception ex)
                {
                    ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"{item} 打开报错:{ex}");

                }               
            }
            DengDaiOpen = true;
        }
        public override void Close()
        {
            ZongKaiGuan = false;
            if (LisLianJieQi.Count > 0)
            {
                Thread.Sleep(100);
                Parallel.ForEach(LisLianJieQi.Keys, (x) => {
                    try
                    {
                        if (LisLianJieQi.ContainsKey(x))
                        {
                            if (LisLianJieQi[x] != null)
                            {
                                LisLianJieQi[x].Close();
                            }
                        }
                    }
                    catch 
                    {

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
                Dictionary<int, List<PLCJiCunQiModel>> huwus = new Dictionary<int, List<PLCJiCunQiModel>>();
                for (int i = 0; i < canshus.Count; i++)
                {
                    PLCJiCunQiModel models = PeiZhiLei.MoXing.GetPLCDian(canshus[i]);
                    if (models != null&&(models.GongNengType==GongNengType.DuXieYiQi||models.GongNengType==GongNengType.XiePuTong))
                    {
                        PeiZhiLei.MoXing.SetZhuangTaiZhi(canshus[i],0);
                        models.Value = canshus[i].Value;
                        if (huwus.ContainsKey(models.SheBeiID) == false)
                        {
                            huwus.Add(models.SheBeiID, new List<PLCJiCunQiModel>());
                        }
                        huwus[models.SheBeiID].Add(models);
                    }
                }
                foreach (var item in huwus.Keys)
                {
                    if (SengData.ContainsKey(item))
                    {
                        SengData[item].Add(huwus[item]);
                    }
                }
            }
        }

        public override JiaoYanJieGuoModel JiaoYanChengGong(JiCunQiModel jicunqiid)
        {
            JiaoYanJieGuoModel models = new JiaoYanJieGuoModel();
            models.WeiYiBiaoShi = jicunqiid.WeiYiBiaoShi;
            models.SheBeiID = jicunqiid.SheBeiID;
            PLCJiCunQiModel mosd = PeiZhiLei.MoXing.GetPLCDian(jicunqiid);
            if (DengDaiOpen)
            {
                if (mosd != null)
                {
                    bool iskekao = mosd.JiCunQiModel.IsKeKao;
                    if (iskekao)
                    {
                        if (mosd.GongNengType == GongNengType.DuPuTong || mosd.GongNengType == GongNengType.DuXinTiao)
                        {
                            object zhen = mosd.Value;
                            models.Value = zhen;
                            models.IsZuiZhongJieGuo = JieGuoType.ChengGongJiGuo;
                        }
                        else  if (mosd.GongNengType == GongNengType.DuXieYiQi)
                        {
                            if (mosd.IsXieWan == 1)
                            {
                                object zhen = mosd.Value;
                                models.Value = zhen;
                                models.IsZuiZhongJieGuo = JieGuoType.ChengGongJiGuo;
                            }
                            else if (mosd.IsXieWan >= 2)
                            {
                                object zhen = mosd.Value;
                                models.Value = zhen;
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
                            if (mosd.IsXieWan == 1)
                            {
                                object zhen = mosd.Value;
                                models.Value = "OK";
                                models.IsZuiZhongJieGuo = JieGuoType.ChengGongJiGuo;
                            }
                            else if (mosd.IsXieWan >= 2)
                            {
                                object zhen = "NG";
                                models.Value = zhen;
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
                        models.Value = mosd.Value;
                        models.IsZuiZhongJieGuo = JieGuoType.BuKeKaoJieGuo;
                    }
                }
                else
                {
                    models.Value = "未找到";
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
                List<JiCunQiModel> Get = PeiZhiLei.MoXing.LisDu;
                shuju = ChangYong.FuZhiShiTi(Get);
            }
            else if (type == 2)
            {
                List<JiCunQiModel> Get = PeiZhiLei.MoXing.LisXie;
                shuju = ChangYong.FuZhiShiTi(Get);
            }
            else if (type == 3)
            {
                List<JiCunQiModel> Get = PeiZhiLei.MoXing.LisDuXie;
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
            jieMianFrmModel.Form = new PeiZhiFrm(PeiZhiLei);
            return jieMianFrmModel;
        }

        public override List<JiCunQiModel> GetShuJu()
        {

            List<JiCunQiModel> Get = PeiZhiLei.MoXing.LisDu;

            return Get;
        }

     
        private void ReadWork(object zhi)
        {
            PLCShBeiModel zsmodel = zhi as PLCShBeiModel;
            Plc lianjieqi = LisLianJieQi[zsmodel.Port];
            int yanshi = 5;
            FanXingJiHeLei<List<PLCJiCunQiModel>> sengData = SengData[zsmodel.Port];
            List<JiLvDaXiaoModel> dulis = PeiZhiLei.MoXing.JiLuDBKuaiZuiXiao[zsmodel.Port];
            SerialDataGuDingJieXi dingJieXi = new SerialDataGuDingJieXi();
         
            DateTime xietxtime = DateTime.Now;
            DateTime chongliantime = DateTime.Now;
            bool Tx = lianjieqi.IsConnected;
            zsmodel.Tx = Tx;
            int quehuanshijian = zsmodel.CaiJiYanShi;
            if (quehuanshijian <= 0)
            {
                yanshi = quehuanshijian;
            }
            int xierutime = zsmodel.XieRuYanShi;
            if (xierutime<=0)
            {
                xierutime = 5;
            }
            int xiecount = 0;
            int duxintiao = 0;
            int cishu = 5;
            while (ZongKaiGuan)
            {
              
                if (DengDaiOpen == false)
                {
                    Thread.Sleep(10);
                    continue;
                }
                if (Tx == false)
                {
                    if (Tx == false)
                    {
                        PeiZhiLei.MoXing.SetTx(zsmodel.Port,false);
                       
                    }
                    if ((DateTime.Now - chongliantime).TotalMilliseconds >= 1000)
                    {
                        bool zhen= ChongXinOpen(zsmodel, lianjieqi);
                        if (zhen)
                        {
                            Tx = true;
                            PeiZhiLei.MoXing.SetTx(zsmodel.Port,true);
                        }
                        chongliantime = DateTime.Now;
                    }
                  
                }
               
                #region 写
                try
                {
                    //写数据
                    int count = sengData.GetCount();
                    if (count>0)
                    {
                        if (count>10)
                        {
                            count = 10;
                        }
                        List<PLCJiCunQiModel> xies = sengData.GetModel_Head_RomeHead();
                        for (int i = 0; i < xies.Count; i++)
                        {
                            if (Tx == true)
                            {
                                XieShuJu(xies[i], lianjieqi);
                                Thread.Sleep(xierutime);
                            }
                            else
                            {
                                PeiZhiLei.MoXing.SetZhuangTaiZhi(xies[i].JiCunQiModel,2);
                            }
                        }
                       
                    }
                }
                catch (Exception ex)
                {
                    ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"{zsmodel.PLCName}:写报错:{ex}");
                }
                #endregion
                #region 读
                try
                {
                    if (Tx)
                    {
                        DateTime dateTime2 = DateTime.Now;
                        foreach (var item1 in dulis)
                        {
                            DuShuJu(item1, lianjieqi);

                        }
                        PLCJiCunQiModel dumodel = PeiZhiLei.MoXing.GetPLCDian(new JiCunQiModel(), zsmodel.Port, 1);
                        if (dumodel != null)
                        {
                            int duzhi = ChangYong.TryInt(dumodel.Value, 0);
                            if (duzhi != duxintiao)
                            {
                                duxintiao = duzhi;
                                cishu = 15;
                            }
                            else
                            {
                                cishu--;
                            }
                            if (cishu < 0)
                            {
                                Tx = false;
                            }

                        }
                        if (Tx)
                        {
                            ChuFaDu(zsmodel.Port);
                        }
                        if (xiecount >= 2)
                        {
                            xiecount = 0;
                        }
                        if ((DateTime.Now - xietxtime).TotalMilliseconds > 500 && Tx)
                        {
                            PLCJiCunQiModel dumodel2 = PeiZhiLei.MoXing.GetPLCDian(new JiCunQiModel(), zsmodel.Port, 2);
                            if (dumodel2 != null)
                            {
                                dumodel2.Value = xiecount;
                                XieShuJu(dumodel2, lianjieqi);
                                xiecount++;
                            }
                            xietxtime = DateTime.Now;
                        }
                        double shijian1 = (DateTime.Now - dateTime2).TotalMilliseconds;
                    }
                 
                }
                catch (Exception ex)
                {
                    ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"{zsmodel.PLCName}:读报错:{ex}");
                }
                #endregion
                Thread.Sleep(yanshi);
            }
        }

        private void DuShuJu(JiLvDaXiaoModel jiLvDa,Plc PLCS7)//数据读
        {         
            try
            {
                if (jiLvDa.IsXianShi)
                {
                    jiLvDa.ShuJu.Clear();
                    DateTime shijian = DateTime.Now;
                    int db = jiLvDa.DBKuai;
                    if (db < 0)
                    {
                        db = 0;
                    }
                    try
                    {
                       
                        byte[] shuju = PLCS7.ReadBytes(jiLvDa.DataType, db, jiLvDa.JiCunQiZuiXiaoPianYi, jiLvDa.JiCunQiDuShuLiang);//
                        if (shuju != null)
                        {
                            jiLvDa.ShuJu.AddRange(shuju);
                        }
                        foreach (var item in jiLvDa.BangDianJiCunQi)
                        {
                            int zhengshipianyi = item.PianYiLiang - jiLvDa.JiCunQiZuiXiaoPianYi;
                            List<byte> qushu = new List<byte>();
                            int sdcount = item.GetCount();
                            for (int i = zhengshipianyi; i < zhengshipianyi + sdcount; i++)
                            {
                                qushu.Add(shuju[i]);
                            }
                            switch (item.PLCDataType)
                            {
                                case PLCDataType.Int:
                                    {
                                        item.JiCunQiModel.IsKeKao = true;
                                        if (item.IsXieWan!=1)
                                        {
                                            item.IsXieWan = 1;
                                        }
                                        item.Value = JiXieShuJu<short>(qushu);
                                        item.JiCunQiModel.Value = item.Value;
                                    }
                                    break;
                                case PLCDataType.Real:
                                    {
                                        if (item.IsXieWan != 1)
                                        {
                                            item.IsXieWan = 1;
                                        }
                                        item.Value = JiXieShuJu<float>(qushu);
                                        item.JiCunQiModel.IsKeKao = true;
                                        item.JiCunQiModel.Value = item.Value;
                                    }
                                    break;
                                case PLCDataType.DWord:
                                    {
                                        if (item.IsXieWan != 1)
                                        {
                                            item.IsXieWan = 1;
                                        }
                                        item.Value = JiXieShuJu<UInt32>(qushu);
                                        item.JiCunQiModel.IsKeKao = true;
                                        item.JiCunQiModel.Value = item.Value;
                                    }
                                    break;
                                case PLCDataType.Bool:
                                    {
                                        if (item.IsXieWan != 1)
                                        {
                                            item.IsXieWan = 1;
                                        }
                                        if (item.AdRm >= 0)
                                        {
                                            List<int> zhuanhuan = ChangYong.Get10Or2(qushu[0], 8);
                                            if (item.AdRm < zhuanhuan.Count)
                                            {
                                                item.Value = zhuanhuan[item.AdRm];
                                                item.JiCunQiModel.IsKeKao = true;
                                                item.JiCunQiModel.Value = item.Value;
                                            }
                                        }
                                        else
                                        {
                                            item.Value = JiXieShuJu<bool>(qushu);
                                            item.JiCunQiModel.IsKeKao = true;
                                            item.JiCunQiModel.Value = item.Value;
                                        }
                                    }
                                    break;
                                case PLCDataType.DInt:
                                    {
                                        if (item.IsXieWan != 1)
                                        {
                                            item.IsXieWan = 1;
                                        }
                                        item.Value = JiXieShuJu<Int32>(qushu);
                                        item.JiCunQiModel.IsKeKao = true;
                                        item.JiCunQiModel.Value = item.Value;
                                    }
                                    break;
                                case PLCDataType.Byte:
                                    {
                                        if (item.IsXieWan != 1)
                                        {
                                            item.IsXieWan = 1;
                                        }
                                        item.Value = JiXieShuJu<byte>(qushu);
                                        item.JiCunQiModel.IsKeKao = true;
                                        item.JiCunQiModel.Value = item.Value;
                                    }
                                    break;                          
                                case PLCDataType.LReal:
                                    {
                                        if (item.IsXieWan != 1)
                                        {
                                            item.IsXieWan = 1;
                                        }
                                        item.Value = JiXieShuJu<double>(qushu);
                                        item.JiCunQiModel.IsKeKao = true;
                                        item.JiCunQiModel.Value = item.Value;
                                    }
                                    break;
                                case PLCDataType.StringX2:
                                    {
                                        if (item.IsXieWan != 1)
                                        {
                                            item.IsXieWan = 1;
                                        }
                                        item.Value = ChangYong.ByteOrString(qushu, "");
                                        item.JiCunQiModel.IsKeKao = true;
                                        item.JiCunQiModel.Value = item.Value;
                                    }
                                    break;
                                case PLCDataType.String:
                                    {
                                        if (item.IsXieWan != 1)
                                        {
                                            item.IsXieWan = 1;
                                        }
                                        string dshuju = "";
                                        if (qushu.Count > 2)
                                        {
                                            int count = qushu[1];
                                            try
                                            {
                                                dshuju = Encoding.ASCII.GetString(qushu.ToArray(),2, count);
                                            }
                                            catch 
                                            {

                                              
                                            }
                                        }                                    
                                        item.Value = dshuju;
                                        item.JiCunQiModel.IsKeKao = true;
                                        item.JiCunQiModel.Value = dshuju;
                                    }
                                    break;
                                case PLCDataType.String16OrACSII:
                                    {
                                        if (item.IsXieWan != 1)
                                        {
                                            item.IsXieWan = 1;
                                        }
                                        item.Value = Encoding.ASCII.GetString(qushu.ToArray()).Replace("\0","");
                                        item.JiCunQiModel.IsKeKao = true;
                                        item.JiCunQiModel.Value = item.Value;
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }

                    }
                    catch
                    {


                    }
                    double shijian1 = (DateTime.Now - shijian).TotalMilliseconds;
                }


            }
            catch
            {

            }
        }


        /// <summary>
        /// 解析读的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="shuju"></param>
        /// <returns></returns>
        private object JiXieShuJu<T>(List<byte> shuju)//数据转换
        {

            object zhi = new object();
            if (typeof(T) == typeof(short))
            {
                if (BitConverter.IsLittleEndian)
                {
                    shuju.Reverse();
                    byte[] bytes = shuju.ToArray();
                    zhi = BitConverter.ToInt16(bytes, 0);
                }

            }
            else if (typeof(T) == typeof(int))
            {
                if (BitConverter.IsLittleEndian)
                {
                    shuju.Reverse();
                    byte[] bytes = shuju.ToArray();
                    zhi = BitConverter.ToInt32(bytes, 0);
                }
            }
            else if (typeof(T) == typeof(UInt32))
            {
                if (BitConverter.IsLittleEndian)
                {
                    shuju.Reverse();
                    byte[] bytes = shuju.ToArray();
                    zhi = BitConverter.ToUInt32(bytes, 0);
                }
            }
            else if (typeof(T) == typeof(string))
            {
                zhi = Encoding.ASCII.GetString(shuju.ToArray());
            }
            else if (typeof(T) == typeof(bool))
            {
                zhi = shuju[0] == 0x01;
            }
            else if (typeof(T) == typeof(byte))
            {
                zhi = shuju[0];
            }
            else if (typeof(T) == typeof(float))
            {
                if (BitConverter.IsLittleEndian)
                {
                    shuju.Reverse();
                    byte[] bytes = shuju.ToArray();
                    zhi = BitConverter.ToSingle(bytes, 0);
                }
            }
            else if (typeof(T) == typeof(double))
            {
                if (BitConverter.IsLittleEndian)
                {
                    shuju.Reverse();
                    byte[] bytes = shuju.ToArray();
                    zhi = BitConverter.ToDouble(bytes, 0);
                }
            }

            return zhi;
        }


        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="item"></param>
        public  void XieShuJu(PLCJiCunQiModel item,Plc PLCS7)
        {

            if (item.PianYiLiang >= 0)
            {

                try
                {
                    int dbkuai = item.DBKuan;
                    DataType dataType = DataType.DataBlock;
                    if (dbkuai < 0)
                    {
                        if (dbkuai == -4)
                        {
                            dataType = DataType.Memory;
                        }
                        dbkuai = 0;

                    }
                    switch (item.PLCDataType)
                    {
                        case PLCDataType.Int:
                            {
                                object zhi = XieRuZhi<short>(item.Value);
                                if (zhi != null)
                                {
                                    PLCS7.Write(dataType, dbkuai,
         item.PianYiLiang, zhi);
                                }
                            }
                            break;
                        case PLCDataType.Real:
                            {
                                object zhi = XieRuZhi<double>(item.Value);
                                if (zhi != null)
                                {
                                    PLCS7.Write(dataType, dbkuai,
         item.PianYiLiang, zhi);
                                }
                            }
                            break;
                        case PLCDataType.DWord:
                            {
                                object zhi = XieRuZhi<UInt32>(item.Value);
                                if (zhi != null)
                                {
                                    PLCS7.Write(dataType, dbkuai,
         item.PianYiLiang, zhi);
                                    // object zhis=  PLCS7.Read(DataType.DataBlock, item.DBKuan, item.PianYiLiang,VarType.DInt,1);
                                }
                            }
                            break;
                        case PLCDataType.Bool:
                            {
                                object zhi = XieRuZhi<bool>(item.Value);
                                if (zhi != null)
                                {
                                    PLCS7.Write(dataType, dbkuai,
         item.PianYiLiang, zhi, item.AdRm);
                                }
                            }
                            break;
                        case PLCDataType.DInt:
                            {
                                object zhi = XieRuZhi<int>(item.Value);
                                if (zhi != null)
                                {
                                    PLCS7.Write(dataType, dbkuai,
         item.PianYiLiang, zhi);
                                }
                            }
                            break;
                        case PLCDataType.Byte:
                            {
                                object zhi = XieRuZhi<byte>(item.Value);
                                if (zhi != null)
                                {
                                    PLCS7.Write(dataType, dbkuai,
         item.PianYiLiang, zhi);
                                }
                            }
                            break;
                        case PLCDataType.String:
                            {
                                byte[] zhi = Encoding.ASCII.GetBytes(item.Value.ToString());
                                int count = zhi.Length;
                                if (count < 253)
                                {
                                    List<byte> xieshuju = new List<byte>();
                                    xieshuju.Add(0xFE);
                                    xieshuju.Add((byte)count);
                                    xieshuju.AddRange(zhi);
                                    PLCS7.Write(dataType, dbkuai,
       item.PianYiLiang, xieshuju.ToArray());
                                }
                               
                            }
                            break;
                        case PLCDataType.StringX2:
                            {
                                object zhi = ChangYong.HexStringToByte(item.Value.ToString());
                                if (zhi != null)
                                {
                                    PLCS7.Write(dataType, dbkuai,
         item.PianYiLiang, zhi);
                                }
                            }
                            break;
                        case PLCDataType.String16OrACSII:
                            {
                                byte[] zhi = Encoding.ASCII.GetBytes(item.Value.ToString());                             
                                if (zhi != null)
                                {
                                    PLCS7.Write(dataType, dbkuai,
         item.PianYiLiang, zhi);
                                }
                            }
                            break;
                        case PLCDataType.LReal:
                            {
                                object zhi = XieRuZhi<byte[]>(item.Value);
                                if (zhi != null)
                                {

                                    PLCS7.Write(dataType, dbkuai,
          item.PianYiLiang, zhi);

                                }
                            }
                            break;
                        default:
                            break;
                    }
                   
                    PeiZhiLei.MoXing.SetZhuangTaiZhi(item.JiCunQiModel, 1);
                }
                catch 
                {

                    PeiZhiLei.MoXing.SetZhuangTaiZhi(item.JiCunQiModel, 2);
                }
            }
            else
            {
                PeiZhiLei.MoXing.SetZhuangTaiZhi(item.JiCunQiModel, 2);
            }
        }
        /// <summary>
        /// 把数据转换相应的plc数据类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objecj"></param>
        /// <returns></returns>
        public  object XieRuZhi<T>(object objecj)//
        {

            object zhi = null;
            if (typeof(T) == typeof(short))
            {
                short zhi1 = 0;
                bool chenggong = short.TryParse(objecj.ToString(), out zhi1);
                if (chenggong)
                {
                    zhi = zhi1;
                }
            }
            else if (typeof(T) == typeof(int))
            {
                int zhi1 = 0;
                bool chenggong = int.TryParse(objecj.ToString(), out zhi1);
                if (chenggong)
                {
                    zhi = zhi1;
                }
            }
            else if (typeof(T) == typeof(UInt32))
            {
                UInt32 zhi1 = 0;
                bool chenggong = UInt32.TryParse(objecj.ToString(), out zhi1);
                if (chenggong)
                {
                    zhi = zhi1;
                }
            }
            else if (typeof(T) == typeof(string))
            {
                zhi = Encoding.ASCII.GetBytes(objecj.ToString());

            }
            else if (typeof(T) == typeof(bool))
            {
                if (objecj.ToString().ToLower().Equals("true") || objecj.ToString().ToLower().Equals("1"))
                {
                    zhi = true;
                }
                else if (objecj.ToString().ToLower().Equals("false") || objecj.ToString().ToLower().Equals("0"))
                {
                    zhi = false;
                }

            }
            else if (typeof(T) == typeof(byte))
            {
                byte zhi1 = 0;
                bool chenggong = byte.TryParse(objecj.ToString(), out zhi1);
                if (chenggong)
                {
                    zhi = zhi1;
                }
            }
            else if (typeof(T) == typeof(double))
            {
                double zhi1 = 0;
                bool chenggong = double.TryParse(objecj.ToString(), out zhi1);
                if (chenggong)
                {
                    zhi = zhi1;
                }
            }
            else if (typeof(T) == typeof(byte[]))
            {
                double zhi1 = 0;
                bool chenggong = double.TryParse(objecj.ToString(), out zhi1);
                if (chenggong)
                {
                    zhi = zhi1;
                    byte[] shuju = BitConverter.GetBytes(zhi1);

                    List<byte> jiedu = new List<byte>();
                    for (int i = shuju.Length - 1; i >= 0; i--)
                    {
                        jiedu.Add(shuju[i]);
                    }
                    zhi = jiedu.ToArray();
                }

            }
            return zhi;
        }
        /// <summary>
        /// 重新连接
        /// </summary>
        private bool ChongXinOpen(PLCShBeiModel zsmodel, Plc plc)
        {
            try
            {
                ChuFaMsg(MsgDengJi.SheBeiZhengChang, $"{zsmodel.PLCName},{zsmodel.IP}:发起重连");
                CpuType cpuType = CpuType.S7200;
                if (zsmodel.PCLType == PCLType.PLC300_1200)
                {
                    cpuType = CpuType.S71200;
                }
                else if (zsmodel.PCLType == PCLType.PLC1500)
                {
                    cpuType = CpuType.S71500;
                }
                //创建PLC对象
                plc = new Plc(cpuType, zsmodel.IP, zsmodel.Rack, zsmodel.Slot);
                plc.Open();
                if (plc.IsConnected)
                {
                    ChuFaMsg(MsgDengJi.SheBeiZhengChang, $"{zsmodel.PLCName},{zsmodel.IP}:发起重连成功");
                    return true;
                }

            }
            catch (Exception ex)
            {

                ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"{zsmodel.PLCName},{zsmodel.IP}:重连报错:{ex}");
            }
            return false;
        }

        public override KJPeiZhiJK GetCanShuKJ(string jicunweiyibiaoshi)
        {
            JiCunQiModel jiCunQiModel = new JiCunQiModel();
            jiCunQiModel.WeiYiBiaoShi = jicunweiyibiaoshi;
            PLCJiCunQiModel cunModel = PeiZhiLei.MoXing.GetPLCDian(jiCunQiModel);
            if (cunModel != null)
            {
                if (cunModel.IsIO == 1)
                {
                    List<string> lis = new List<string>();
                    lis.Add(cunModel.IOLuZhi);
                    lis.Add(cunModel.IORedZhi);
                    CanShuKJ kj = new CanShuKJ();
                    kj.SetData(lis);
                    return kj;
                }
            }
            return base.GetCanShuKJ(jicunweiyibiaoshi);
        }
        public override TxModel GetMeiGeTx()
        {
            TxModel model = new TxModel();
            model.SheBeiName = SheBeiName;
            model.SheBeiTD = SheBeiID;
            model.SheBeuZu = FenZu;
            bool ischengg = true;

            for (int i = 0; i < PeiZhiLei.MoXing.LisSheBei.Count; i++)
            {
                PLCShBeiModel item = PeiZhiLei.MoXing.LisSheBei[i];
                ZiTxModel zmodel = new ZiTxModel();
                zmodel.Tx = item.Tx;
                zmodel.ZiSheBeiID = item.Port;
                zmodel.ZiSheBeiName = item.PLCName;
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
