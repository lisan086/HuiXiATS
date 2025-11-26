using CommLei.GongYeJieHe;
using CommLei.JiChuLei;
using CommLei.JieMianLei;
using KuHuDuanDoIP.Frm;
using KuHuDuanDoIP.Model;
using SSheBei.ABSSheBei;
using SSheBei.LianJieQi;
using SSheBei.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace KuHuDuanDoIP.ShiXian
{
    public class DoIP : ABSNSheBei
    {
        private PeiZhiLei PeiZhiLei;
        /// <summary>
        /// 线程总开关
        /// </summary>
        private bool ZongKaiGuan = false;
        private bool DuanKaiLianJie = false;

        /// <summary>
        /// 获取锁的校验码
        /// </summary>
        private RWCmd RWCmd = new RWCmd();


        public override string SheBeiType
        {
            get
            {
                return "客户端DoIP";
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

        /// <summary>
        /// 连接器
        /// </summary>
        private Dictionary<int, ABSLianJieQi> LianJieQis = new Dictionary<int, ABSLianJieQi>();

        /// <summary>
        /// 连接器
        /// </summary>
        private Dictionary<int, FanXingJiHeLei<SendZhiLingModel>> SendZhiLing = new Dictionary<int, FanXingJiHeLei<SendZhiLingModel>>();

        /// <summary>
        /// 连接器
        /// </summary>
        private Dictionary<int,bool> SengZhuangTai = new Dictionary<int,bool>();


        /// <summary>
        /// 发送的指令
        /// </summary>
        public FanXingJiHeLei<ZhiLingModel> SengData = new FanXingJiHeLei<ZhiLingModel>();

        public DoIP()
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
                
                List<SheBeiModel> sModels = PeiZhiLei.MoXing.LisSheBei;
                for (int i = 0; i < sModels.Count; i++)
                {
                    if (LianJieQis.ContainsKey(sModels[i].SheBeiID)==false)
                    {
                        SetLianJieQiModel model = new SetLianJieQiModel();
                        model.IPOrCOMStr = sModels[i].IP;
                        model.SpeedOrPort = sModels[i].DuanKou;
                        ABSLianJieQi lianjieqi = null;
                        lianjieqi = new TCPLianJieQiN();
                        lianjieqi.SetCanShu(model);
                        LianJieQis.Add(sModels[i].SheBeiID, lianjieqi);
                        SendZhiLing.Add(sModels[i].SheBeiID,new FanXingJiHeLei<SendZhiLingModel>());
                        SengZhuangTai.Add(sModels[i].SheBeiID, false);
                        Thread threads = new Thread(SendXinTiao);
                        threads.IsBackground = true;
                        threads.DisableComObjectEagerCleanup();
                        threads.Start(sModels[i].SheBeiID);
                    }
                }
               
                PeiZhiLei.XieIOEvent += ZhongShengIO_XieIOEvent;
               
                Thread thread = new Thread(SendShuJu);
                thread.IsBackground = true;
                thread.DisableComObjectEagerCleanup();
                thread.Start();
               
            }
        }

     

        private int ZhongShengIO_XieIOEvent(ZhiLingModel obj)
        {
            ZhiLingModel mogg = PeiZhiLei.MoXing.GetModel(obj.ZhiLingType);
            if (mogg != null)
            {
                PeiZhiLei.MoXing.SetZhengZaiValue(mogg, 0);
                ZhiLingModel xinmodel = obj.FuZhi();
                xinmodel.JiCunQiModel = mogg.JiCunQiModel.FuZhi();
                SengData.Add(xinmodel);
            }
          
            return 0;
        }

        public override void Open()
        {
            DuanKaiLianJie = true;
            ChuFaMsg(MsgDengJi.SheBeiZhengChang,"打开Doip设备");
        }
        public override void Close()
        {
            PeiZhiLei.IsGuanBi = true;
            ZongKaiGuan = false;
            Thread.Sleep(100);
            foreach (var item in LianJieQis.Keys)
            {
                try
                {
                    LianJieQis[item].Close();
                }
                catch 
                {

                    
                }
            }
        }


        public override void XieShuJu(List<JiCunQiModel> canshus)
        {
            if (canshus == null || canshus.Count == 0)
            {
                return;
            }
            for (int i = 0; i < canshus.Count; i++)
            {
                ZhiLingModel models = PeiZhiLei.MoXing.GetModel(canshus[i]);
                if (models != null)
                {
                    PeiZhiLei.MoXing.SetZhengZaiValue(models, 0);
                    models.JiCunQiModel = canshus[i];
                    models.SetCanShu(canshus[i].Value.ToString());
                    SengData.Add(models);
                }

            }
        }

        public override JiaoYanJieGuoModel JiaoYanChengGong(JiCunQiModel jicunqiid)
        {
            JiaoYanJieGuoModel models = new JiaoYanJieGuoModel();
            models.SheBeiID = jicunqiid.SheBeiID;
            models.WeiYiBiaoShi = jicunqiid.WeiYiBiaoShi;
            ZhiLingModel zhilingmodel=PeiZhiLei.MoXing.IsChengGong(jicunqiid);
            if (zhilingmodel != null)
            {
                if (zhilingmodel.JiCunQiModel.IsKeKao)
                {
                    if (zhilingmodel.IsXieWan == 1)
                    {
                        models.Value = zhilingmodel.JiCunQiModel.Value;
                        models.IsZuiZhongJieGuo = JieGuoType.ChengGongJiGuo;
                    }
                    else if (zhilingmodel.IsXieWan == 2)
                    {
                        models.Value = zhilingmodel.JiCunQiModel.Value;
                        models.IsZuiZhongJieGuo = JieGuoType.ShiBaiJiGuo;
                    }
                    else if (zhilingmodel.IsXieWan == 3)
                    {
                        models.Value = zhilingmodel.JiCunQiModel.Value;
                        models.IsZuiZhongJieGuo = JieGuoType.ShiBaiJiGuo;
                    }
                }
                else
                {
                    models.Value = "TongXinBuKeKao";
                    models.IsZuiZhongJieGuo = JieGuoType.BuKeKaoJieGuo;
                }
            }
            else
            {
                models.Value = "NOJiCunQi";
                models.IsZuiZhongJieGuo = JieGuoType.MeiZhaoDaoJiGuo;
            }
            return models;
        }



        public override List<JiCunQiModel> PeiZhiDuXie(int type)
        {
            if (type == 1)
            {
                List<JiCunQiModel> shuju = new List<JiCunQiModel>();
                shuju.AddRange(PeiZhiLei.MoXing.LisDu);
                return shuju;
            }
            else if (type == 2)
            {
                List<JiCunQiModel> shuju = new List<JiCunQiModel>();
                shuju.AddRange(PeiZhiLei.MoXing.LisXie);
                return shuju;
            }
            else if (type == 3)
            {
                List<JiCunQiModel> shuju = new List<JiCunQiModel>();
                shuju.AddRange(PeiZhiLei.MoXing.LisDuXie);
                return shuju;
            }
            return new List<JiCunQiModel>();
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
            return PeiZhiLei.MoXing.LisDu;
        }

        /// <summary>
        /// 监听接收线程
        /// </summary>
        /// <param name="userState"></param>
        private void SendXinTiao(object shebeiid)
        {
            int shebeid = ChangYong.TryInt(shebeiid,-1);
            if (shebeid<0)
            {
                return;
            }
            FanXingJiHeLei<SendZhiLingModel> sendshuju = SendZhiLing[shebeid];
            ABSLianJieQi lianjieqi = LianJieQis[shebeid];
            DateTime shijian = DateTime.Now;
            while (ZongKaiGuan)
            {           
                if (DuanKaiLianJie == false)
                {                 
                    Thread.Sleep(20);
                    continue;
                }             
                try
                {
                    if (SengZhuangTai[shebeid])
                    {
                        if ((DateTime.Now - shijian).TotalMilliseconds >= 2000)
                        {

                            Dictionary<int, byte[]> xintiao = PeiZhiLei.MoXing.XiTiaoByte;
                            foreach (var item in xintiao.Keys)
                            {
                                if (item == shebeid)
                                {
                                    SendZhiLingModel models = new SendZhiLingModel();
                                    models.ZhiLingName = $"{item}:心跳";
                                    models.ZhiLing = xintiao[item];
                                    sendshuju.Add(models);
                                  
                                    break;
                                }
                            }
                            shijian = DateTime.Now;
                        }
                    }
                    int count = sendshuju.GetCount();
                    if (count>0)
                    {
                        SendZhiLingModel shuju = sendshuju.GetModel_Head_RomeHead();
                        XieCMD(lianjieqi, shuju);
                    }
                }
                catch (Exception e)
                {
                    ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"心跳发送:{e}");              
                }
                Thread.Sleep(10);

            }
            ChuFaMsg(MsgDengJi.SheBeiZhengChang, $"# Recv 线程退出");

        }

      

        /// <summary>
        /// 发送数据
        /// </summary>
        private void SendShuJu()
        {
          
            while (ZongKaiGuan)
            {              
                if (DuanKaiLianJie == false)
                {
                    Thread.Sleep(20);
                    continue;
                }         
                try
                {
                    int incout = SengData.GetCount();
                    if (incout > 0)
                    {

                        ZhiLingModel model = SengData.GetModel_Head_RomeHead();
                        if (model != null)
                        {
                            WriteCMD(model);
                        }
                     
                    }
                }
                catch (Exception ex)
                {
                    ChuFaMsg(MsgDengJi.SheBeiBaoWen,$"发送错误：{ex.Message}");
                   
                }

                Thread.Sleep(10);
            }

        }

    
        private bool XieCMD(ABSLianJieQi lienjieqi, SendZhiLingModel mingcheng)
        { 
            if (mingcheng.ZhiLing != null)
            {
                ResultModel resultModel = lienjieqi.Send(mingcheng.ZhiLing);
                ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"{mingcheng}:{resultModel.Msg}");
                return resultModel.IsSuccess;
            }
            return false;
        }

        private void WriteCMD(ZhiLingModel model)
        {
            SheBeiModel shebeimodel=PeiZhiLei.MoXing.GetSheBeiModel(model);
            if (shebeimodel != null)
            {
                if (model.ZhiLingType == ZhiLingType.XieDoipOpenTCP)
                {
                    if (LianJieQis.ContainsKey(shebeimodel.SheBeiID))
                    {
                        ResultModel resultModel= LianJieQis[shebeimodel.SheBeiID].Open();
                        if (resultModel.IsSuccess)
                        {
                            try
                            {
                                PeiZhiLei.MoXing.SetHeGe(shebeimodel.SheBeiID,true);
                                SengZhuangTai[shebeimodel.SheBeiID] = true;
                            }
                            catch
                            {

                            }
                           
                        }

                        PeiZhiLei.MoXing.SetJiCunQiValue(model,"OK");
                        PeiZhiLei.MoXing.SetZhengZaiValue(model,1);
                        ChuFaMsg(MsgDengJi.SheBeiXie,$"打开设备:{resultModel.Msg}");
                    }
                    else
                    {
                        PeiZhiLei.MoXing.SetJiCunQiValue(model, "设备没有找到");
                        PeiZhiLei.MoXing.SetZhengZaiValue(model, 2);
                        ChuFaMsg(MsgDengJi.SheBeiXie, $"设备没有找到:{shebeimodel.SheBeiID}");
                    }          
                }
                else if (model.ZhiLingType == ZhiLingType.XieDoipGuanBiTCP)
                {
                    if (LianJieQis.ContainsKey(shebeimodel.SheBeiID))
                    {
                        try
                        {
                            SengZhuangTai[shebeimodel.SheBeiID] = false;
                        }
                        catch
                        {

                        }
                        ResultModel resultModel = LianJieQis[shebeimodel.SheBeiID].Close();                         
                        PeiZhiLei.MoXing.SetJiCunQiValue(model, "OK");
                        PeiZhiLei.MoXing.SetZhengZaiValue(model, 1);
                        PeiZhiLei.MoXing.SetHeGe(shebeimodel.SheBeiID, false);
                        ChuFaMsg(MsgDengJi.SheBeiXie, $"关闭设备:{resultModel.Msg}");
                    }
                    else
                    {
                        PeiZhiLei.MoXing.SetJiCunQiValue(model, "设备没有找到");
                        PeiZhiLei.MoXing.SetZhengZaiValue(model, 2);
                        ChuFaMsg(MsgDengJi.SheBeiXie, $"设备没有找到:{shebeimodel.SheBeiID}");
                    }
                }
                else if (model.ZhiLingType == ZhiLingType.XieDoipYouFanHui)
                {
                    bool islianjie = false;
                    for (int i = 0; i < 15; i++)
                    {
                        if (LianJieQis[shebeimodel.SheBeiID].TongXinState == false)
                        {
                            ResultModel rm = LianJieQis[shebeimodel.SheBeiID].Open();
                            ChuFaMsg(MsgDengJi.SheBeiZhengChang, rm.Msg);
                        }
                        else
                        {
                            PeiZhiLei.MoXing.SetHeGe(shebeimodel.SheBeiID, true);
                            SengZhuangTai[shebeimodel.SheBeiID] = true;
                            islianjie = true;
                            break;
                        }
                    }
                    if (islianjie)
                    {
                        byte[] baowen = FaSongShuJu(model, shebeimodel);
                        {
                            SendZhiLingModel zhilingmodel = new SendZhiLingModel();
                            zhilingmodel.ZhiLingName=model.ZhiLingName;
                            zhilingmodel.ZhiLing = baowen;
                            SendZhiLing[shebeimodel.SheBeiID].Add(zhilingmodel);
                        }
                        byte[] sa = PeiZhiLei.MoXing.SheBeiSa[shebeimodel.SheBeiID];
                        List<byte> ShouJiShuJu = new List<byte>();
                        List<List<byte>> xinssbao = new List<List<byte>>();
                        float chaoshi = shebeimodel.ChaoTime * 1000;
                        DateTime dateTime = DateTime.Now;
                        Thread.Sleep(10);
                        for (; ZongKaiGuan;)
                        {
                            ResultModel rm = LianJieQis[shebeimodel.SheBeiID].Read();
                            if (rm.IsSuccess)
                            {
                                ShouJiShuJu.AddRange(rm.TData);
                                ChuFaMsg(MsgDengJi.SheBeiBaoWen, rm.Msg);
                            }
                            try
                            {
                                JieXiData(ShouJiShuJu, sa, xinssbao);
                            }
                            catch
                            {


                            }

                            if (xinssbao.Count > 0)
                            {
                                bool zhen = false;
                                StringBuilder sb = new StringBuilder();
                                for (int i = 0; i < xinssbao.Count; i++)
                                {
                                    string receiveString = ChangYong.ByteOrString(xinssbao[i], " ");
                                    if (shebeimodel.GuoLuBaoWen.IndexOf(receiveString) < 0)
                                    {
                                        int jiesguo = PanDuanHeGe(model, xinssbao[i]);
                                        zhen = jiesguo < 3;
                                        sb.AppendLine($"接收正常数据:{receiveString}");
                                    }
                                    else
                                    {
                                        sb.AppendLine($"过滤接收数据:{receiveString}");
                                    }
                                }
                                xinssbao.Clear();
                                ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"{sb.ToString()}");
                                sb.Clear();
                                if (zhen)
                                {
                                    break;
                                }
                            }

                            if ((DateTime.Now - dateTime).TotalMilliseconds >= chaoshi)
                            {
                                PeiZhiLei.MoXing.SetJiCunQiValue(model, "ChaoShi");
                                PeiZhiLei.MoXing.SetZhengZaiValue(model, 3);
                                break;
                            }
                            Thread.Sleep(1);
                        }
                        try
                        {
                            Task.Factory.StartNew(() => { PeiZhiLei.MoXing.BaoCunJiLu(PeiZhiLei.JiLuPathName, model); });
                        }
                        catch
                        {


                        }
                    }
                    else
                    {
                        PeiZhiLei.MoXing.SetJiCunQiValue(model, "设备通信不上");
                        PeiZhiLei.MoXing.SetZhengZaiValue(model, 2);
                    }
                }
                else if (model.ZhiLingType== ZhiLingType.XieDoipGetZiData)
                {

                }
              
            }
            else
            {
                PeiZhiLei.MoXing.SetJiCunQiValue(model,"NOSheBei");
                PeiZhiLei.MoXing.SetZhengZaiValue(model,2);
            }
           
        }


        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="ShouJiShuJu"></param>
        /// <param name="xinssbao"></param>
        private void JieXiData(List<byte> ShouJiShuJu, byte[] sa, List<List<byte>> xinssbao)
        {

            int quchangdu = 4;
            int conut = ShouJiShuJu.Count;
            List<byte> chaozhao = new List<byte>() { 0x02, 0xFD, 0x80 };
            while (ShouJiShuJu.Count > 4 && DuanKaiLianJie)
            {
                int zhaochu = ShouJiShuJu.HanYouJiHe(chaozhao, 0);

                if (zhaochu >= 0)
                {
                    if (zhaochu > 0)
                    {
                        for (int i = 0; i < zhaochu; i++)
                        {
                            ShouJiShuJu.RemoveAt(0);
                        }
                    }
                    if (ShouJiShuJu.Count >= quchangdu + chaozhao.Count + 1)
                    {
                        List<byte> huoqu = ShouJiShuJu.GetRange(chaozhao.Count + 1, quchangdu);
                        int chazhi = 4 - huoqu.Count;
                        if (chazhi > 0)
                        {
                            for (int i = 0; i < chazhi; i++)
                            {
                                huoqu.Insert(0, 0x00);
                            }
                        }
                        int changdu = GetChangDu(huoqu);
                        if (changdu < 4)
                        {
                            ShouJiShuJu.RemoveAt(0);
                        }
                        else
                        {
                            if (ShouJiShuJu.Count >= 12)
                            {
                                if (IsSaXiangTong(ShouJiShuJu[10], ShouJiShuJu[11],sa))
                                {
                                    if (ShouJiShuJu.Count >= quchangdu + chaozhao.Count + changdu + 1)
                                    {

                                        List<byte> xinjiequ = ShouJiShuJu.GetRange(0, quchangdu + chaozhao.Count + changdu + 1);

                                        xinssbao.Add(xinjiequ);
                                        ShouJiShuJu.RemoveRange(0, quchangdu + chaozhao.Count + changdu + 1);

                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    ShouJiShuJu.RemoveAt(0);
                                }
                            }
                            else
                            {
                                break;
                            }

                        }
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    int zhaochus = ShouJiShuJu.IndexOf(chaozhao[0], 1);
                    if (zhaochus < 0)
                    {
                        ShouJiShuJu.RemoveRange(0, conut);
                    }
                    else
                    {
                        for (int i = 0; i < zhaochu; i++)
                        {
                            ShouJiShuJu.RemoveAt(0);
                        }
                    }
                }

            }
        }
        private int GetChangDu(List<byte> changdu)
        {
            if (BitConverter.IsLittleEndian)
            {
                List<byte> ssd = new List<byte>()
            {
                changdu[3],
                changdu[2],
                changdu[1],
                changdu[0],
            };
                return BitConverter.ToInt32(ssd.ToArray(), 0);
            }
            else
            {
                return BitConverter.ToInt32(changdu.ToArray(), 0);
            }
        }

   

        /// <summary>
        /// 1表示合格 2表示不合格 3表示进行中 
        /// </summary>
        /// <param name="zhilingmodel"></param>
        /// <returns></returns>
        private int PanDuanHeGe(ZhiLingModel zhilingmodel,List<byte> framelis)
        {                 
            byte[] shujubaowen = ChangYong.HexStringToByte(zhilingmodel.ZhiLingShuJu);
          
            int count = framelis.Count;
            if (count > 0)
            {
                byte[] frame = framelis.ToArray();
                {
                    byte di13ge = frame[12];
                    byte[] jiequdata = frame.SubArray(12, frame.Length - 12);
                    if (di13ge == (shujubaowen[0] + 0x40))
                    {
                        PeiZhiLei.MoXing.SetJiCunQiValue(zhilingmodel, jiequdata);
                        PeiZhiLei.MoXing.SetZhengZaiValue(zhilingmodel, 1);
                        return 1;
                    }
                }
                {
                    if (frame != null)
                    {

                        byte di13ge = frame[12];
                        byte[] jiequdata = frame.SubArray(12, frame.Length - 12);
                        if (di13ge == 7F)
                        {
                            if (jiequdata[jiequdata.Length - 1] == 0x78)
                            {                           
                                return 3;
                            }
                            else
                            {
                                PeiZhiLei.MoXing.SetJiCunQiValue(zhilingmodel, "NG");
                                PeiZhiLei.MoXing.SetZhengZaiValue(zhilingmodel, 2);
                                return 2;
                            }
                        }


                    }
                }

            }
            return 2;
        }

        public override void Clear(bool isquanbu, JiCunQiModel model)
        {
           
        }

        public override KJPeiZhiJK GetCanShuKJ(string jicunweiyibiaoshi)
        {
            JiCunQiModel jiCunQiModel = new JiCunQiModel();
            jiCunQiModel.WeiYiBiaoShi = jicunweiyibiaoshi;
            ZhiLingModel cunModel = PeiZhiLei.MoXing.GetModel(jiCunQiModel);
            if (cunModel != null)
            {
                if (cunModel.ZhiLingType == ZhiLingType.XieDoipGuanBiTCP || cunModel.ZhiLingType == ZhiLingType.XieDoipOpenTCP)
                {
                    WuCanShuKJ kj = new WuCanShuKJ();
                    return kj;
                }
                else if (cunModel.ZhiLingType== ZhiLingType.XieDoipYouFanHui)
                {
                    CanShuKJ kj = new CanShuKJ();
                    kj.SetData( PeiZhiLei.MoXing.IPS, PeiZhiLei.MoXing.GetJiLuData(PeiZhiLei.JiLuPathName));
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
            model.IsXuYaoPanDuan = false;
            for (int i = 0; i < PeiZhiLei.MoXing.LisSheBei.Count; i++)
            {
                SheBeiModel item = PeiZhiLei.MoXing.LisSheBei[i];
                ZiTxModel zmodel = new ZiTxModel();
                zmodel.Tx = item.TX;
                zmodel.ZiSheBeiID = item.SheBeiID;
                zmodel.ZiSheBeiName = item.IP;
                if (zmodel.Tx == false)
                {
                    ischengg = false;
                }

                model.LisTx.Add(zmodel);

            }
            model.ZongTX = ischengg;
            return model;
        }

        /// <summary>
        /// 1表示路由激活 2表示诊断信息
        /// </summary>
        /// <param name="baoWenType"></param>     
        /// <param name="shuju"></param>
        /// <returns></returns>
        private  byte[] GetBaoWen(byte[] biaozhi, byte[] sa, byte[] ta, byte[] data)
        {
            List<byte> xieyizucheng = new List<byte>();
            byte kaitou = 2;
            byte qufan = (byte)~kaitou;
            xieyizucheng.AddRange(new byte[] { kaitou, qufan });//开头部分
            xieyizucheng.AddRange(biaozhi);
            List<byte> shuju = new List<byte>();
            shuju.AddRange(sa);
            shuju.AddRange(ta);
            shuju.AddRange(data);
            int len = shuju.Count;
            List<byte> changdu = Get10OrB4(len, true);
            xieyizucheng.AddRange(changdu.ToArray());
            xieyizucheng.AddRange(shuju.ToArray());
            return xieyizucheng.ToArray();
        }



        private  byte[] FaSongShuJu(ZhiLingModel model, SheBeiModel shebeimodel)
        {
            byte[] ta = ChangYong.HexStringToByte(model.TaDiZhi);
            if (ta == null || ta.Length <= 1)
            {
                return null;
            }
            string zhiling = model.ZhiLingShuJu;
        
            byte[] shujubaowen = ChangYong.HexStringToByte(zhiling);
            byte[] biaozhi = ChangYong.HexStringToByte(model.FuZaiLeiXing);
            byte[] sa = PeiZhiLei.MoXing.SheBeiSa[shebeimodel.SheBeiID];
            byte[] baowen = GetBaoWen(biaozhi, sa, ta, shujubaowen);

            return baowen;
        }

        /// <summary>
        /// 把一个10进制转成4个16进制的byte数据
        /// </summary>
        /// <param name="len"></param>
        /// <param name="isgaoweizaiqian">true高位在前，低位在后</param>
        /// <returns></returns>
        private List<byte> Get10OrB4(int len, bool isgaoweizaiqian)
        {
            List<byte> shuju = new List<byte>();
            byte[] intBuff = BitConverter.GetBytes(len);
            if (isgaoweizaiqian)
            {
                if (intBuff.Length >= 4)
                {
                    shuju.Add(intBuff[3]);
                    shuju.Add(intBuff[2]);
                    shuju.Add(intBuff[1]);
                    shuju.Add(intBuff[0]);
                }
            }
            else
            {
                if (intBuff.Length >= 4)
                {
                    shuju.Add(intBuff[0]);
                    shuju.Add(intBuff[1]);
                    shuju.Add(intBuff[2]);
                    shuju.Add(intBuff[3]);
                }
            }
            return shuju;
        }


        private  bool IsSaXiangTong(byte shuju10, byte shuju11, byte[] Sa)
        {
            if (Sa != null)
            {
                if (Sa.Length >= 2)
                {
                    if (Sa[0] == shuju10 && Sa[1] == shuju11)
                    {
                        return true;
                    }
                }
                else
                {
                    if (Sa[0] == shuju11)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }

    /// <summary>
    /// TCP连接器
    /// </summary>
    public class TCPLianJieQiN : ABSLianJieQi
    {
        #region 变量区

        /// <summary>
        /// 表示发送报文的客户端
        /// </summary>
        public TcpClient TcpClient;
        /// <summary>
        /// 表示报文链接流
        /// </summary>
        public NetworkStream NetworkStream;
        /// <summary>
        /// 以二进制流写入
        /// </summary>
        public BinaryWriter XieRu;
        /// <summary>
        /// 存放rjmodel
        /// </summary>
        private SetLianJieQiModel RJmodel;
        /// <summary>
        /// 描述
        /// </summary>
        private string ComMiaoSu = "";
        public bool TX = false;
        private IPAddress IPAddress;
        #endregion
        /// <summary>
        /// 构造函数
        /// </summary>     
        public TCPLianJieQiN()
        {

        }



        /// <summary>
        /// 名称
        /// </summary>
        public override string Name
        {
            get
            {
                return ComMiaoSu;
            }
        }
        /// <summary>
        /// true表示通信成功
        /// </summary>
        public override bool TongXinState
        {
            get
            {
                return TX;

            }
        }

        /// <summary>
        /// 关闭连接器
        /// </summary>
        /// <returns></returns>
        public override ResultModel Close()
        {
            ResultModel rm = new ResultModel();
            try
            {
                if (XieRu != null)
                {
                    XieRu.Close();
                    XieRu.Dispose();
                    rm.SetValue(true, string.Format("{0}:关闭成功", Name));
                }

            }
            catch (Exception ex)
            {
                rm.SetValue(false, string.Format("{0}:关闭出现异常:{1}", Name, ex.Message));


            }
            try
            {
                if (NetworkStream != null)
                {
                    NetworkStream.Close();
                    NetworkStream.Dispose();
                    rm.SetValue(true, string.Format("{0}:关闭成功", Name));
                }

            }
            catch (Exception ex)
            {
                rm.SetValue(false, string.Format("{0}:关闭出现异常:{1}", Name, ex.Message));


            }
            try
            {
                TcpClient.Close();
                rm.SetValue(true, string.Format("{0}:关闭成功", Name));

            }
            catch (Exception ex)
            {
                rm.SetValue(false, string.Format("{0}:关闭出现异常:{1}", Name, ex.Message));


            }

            return rm;
        }

        /// <summary>
        /// 打开连接器
        /// </summary>
        /// <returns></returns>
        public override ResultModel Open()
        {
            ResultModel rm = new ResultModel();
            try
            {

                ChongLian();

                if (TcpClient.Connected)
                {
                   
                    rm.SetValue(true, string.Format("{0}:打开成功", ComMiaoSu));

                }
                else
                {

                    rm.SetValue(false, string.Format("{0}:打开失败", ComMiaoSu));
                }

            }
            catch (Exception ex)
            {
                rm.SetValue(false, string.Format("{0}:打开异常:{1}", ComMiaoSu, ex.Message));


            }
            return rm;
        }

        /// <summary>
        /// 连接器读数
        /// </summary>
        /// <returns></returns>
        public override ResultModel Read()
        {
            ResultModel rm = new ResultModel();
            try
            {
                if (TongXinState)
                {
                    if (NetworkStream.DataAvailable)
                    {
                      
                        byte[] receiveBuffer = new byte[1024];
                        int bytesRead = NetworkStream.Read(receiveBuffer, 0, receiveBuffer.Length);
                        if (bytesRead > 0)
                        {
                            byte[] ReceivDate = new byte[bytesRead];
                            Array.Copy(receiveBuffer, ReceivDate, bytesRead);
                           
                            rm.SetValue(true, string.Format("{0}:读取数据成功:{1}", ComMiaoSu, ChangYong.ByteOrString(ReceivDate, " ")), ReceivDate);
                        }
                     

                    }


                }
                else
                {
                    rm.SetValue(false, string.Format("{0}:连接口是关闭状态，不能读取数据", ComMiaoSu));
                }
            }
            catch (Exception ex)
            {

                rm.SetValue(false, string.Format("{0}:读取数据异常;信息:{1}", ComMiaoSu, ex.Message));


            }


            return rm;
        }

        /// <summary>
        /// 连接器发送数据
        /// </summary>
        /// <param name="bytData"></param>
        /// <returns></returns>
        public override ResultModel Send(byte[] bytData)
        {
            ResultModel rm = new ResultModel();
            try
            {
                if (TongXinState)
                {
                    XieRu.Write(bytData);
                    XieRu.Flush();               
                    rm.SetValue(true, string.Format("{0}:发送数据成功:{1}", Name, ChangYong.ByteOrString(bytData, " ")));
                }
                else
                {
                    rm.SetValue(false, string.Format("{0}:连接口是关闭状态，不能发送数据:{1}", Name, ChangYong.ByteOrString(bytData, " ")));
                }
            }
            catch (Exception ex)
            {
                rm.SetValue(false, string.Format("{0}:发送数据异常:{1};信息:{2}", Name, ChangYong.ByteOrString(bytData, " "), ex.Message));


            }
            return rm;
        }
        /// <summary>
        /// 连接器设置参数
        /// </summary>
        /// <param name="model"></param>
        public override void SetCanShu(SetLianJieQiModel model)
        {
            this.RJmodel = model;
            ComMiaoSu = string.Format("{0}*{1}TCP", RJmodel.IPOrCOMStr, RJmodel.SpeedOrPort);
           
            IPAddress = IPAddress.Parse(RJmodel.IPOrCOMStr);
        }
        /// <summary>
        /// 重连
        /// </summary>
        /// <returns></returns>
        public override bool ChongLian()
        {
            try
            {
                try
                {
                    Close();
                }
                catch
                {

                }
                this.TcpClient = new TcpClient();
                Task jieguo = TcpClient.ConnectAsync(IPAddress, RJmodel.SpeedOrPort);
                jieguo.Wait(800);
                if (TcpClient.Connected)
                {
                    NetworkStream = TcpClient.GetStream();
                    TX = true;
                    return true;
                }
            }
            catch
            {


            }
            return false;
        }
    }
}
