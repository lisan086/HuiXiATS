using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommLei.DataChuLi;
using CommLei.GongYeJieHe;
using CommLei.JiChuLei;
using Pcan.Frm;
using SSheBei.ABSSheBei;
using SSheBei.CRCJiaoYan;
using SSheBei.LianJieQi;
using SSheBei.Model;
using TuLuSILin;
using YiBanLin.Frm;
using YiBanLin.Model;

namespace YiBanLin.ShiXian
{
    public class ZhiLingLinLei : ABSNSheBei
    {
        private PeiZhiLei PeiZhiLei;
        /// <summary>
        /// 线程总开关
        /// </summary>
        private bool ZongKaiGuan = true;
        /// <summary>
        /// true  表示线程开始工作
        /// </summary>
        private bool DengDaiOpen = false;
     

        private Dictionary<int,Int32> DevHandle = new Dictionary<int, int>();
        public bool[] LIN_SendMsgFlag = new bool[2] { false, false };
        public String[] MSGTypeStr = new String[10] { "UN", "MW", "MR", "SW", "SR", "BK", "SY", "ID", "DT", "CK" };
        public String[] CKTypeStr = new String[5] { "STD", "EXT", "USER", "NONE", "ERROR" };


        public override string SheBeiType
        {
            get
            {
                return "图莫斯Lin";
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

        

        private Dictionary<int, FanXingJiHeLei<List<CunModel>>> SengData = new Dictionary<int, FanXingJiHeLei<List<CunModel>>>();


        public ZhiLingLinLei()
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
                List<LinModel> sModels = PeiZhiLei.DataMoXing.LisSheBei;
                for (int i = 0; i < sModels.Count; i++)
                {
                    if (SengData.ContainsKey(sModels[i].SheBeiID) == false)
                    {
                        DevHandle.Add(sModels[i].SheBeiID,0);
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
            XieShuJu(new List<JiCunQiModel>() { obj });
        }

        public override void Open()
        {
            foreach (var item in DevHandle.Keys)
            {
                DaKai(item);
            }           
            DengDaiOpen =true;
        }
        public override void Close()
        {
            ZongKaiGuan = false;
            try
            {
                foreach (var item in DevHandle.Keys)
                {
                    USB_DEVICE.USB_CloseDevice(DevHandle[item]);
                }
            
                ChuFaMsg(MsgDengJi.SheBeiZhengChang, "LIN 已关闭!");

            }
            catch (Exception)
            {

                ChuFaMsg(MsgDengJi.SheBeiZhengChang, "LIN关闭失败!");

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
            Dictionary<string, CunModel> sModels = PeiZhiLei.DataMoXing.JiLu;
            if (sModels.ContainsKey(jicunqi.WeiYiBiaoShi))
            {
                CunModel models = sModels[jicunqi.WeiYiBiaoShi];
                models.IsZhengZaiCe = 0;
                models.JiCunQi.Value = "";
                CunModel xinlian = models.FuZhi();
                xinlian.JiCunQi= jicunqi;
                return xinlian;
            }
            return null;
        }
        private void JieRu(List<JiCunQiModel> canshus)
        {
            foreach (var item in canshus)
            {
                CunModel ipcom = GetDiZhi(item);
                if (ipcom!=null)
                {
                    if (SengData.ContainsKey(ipcom.ZongSheBeiId))
                    {                                       
                        SengData[ipcom.ZongSheBeiId].Add(new List<CunModel>() { ipcom});
                    }
                }
            }
        }
        public override JiaoYanJieGuoModel JiaoYanChengGong(JiCunQiModel jicunqiid)
        {
            JiaoYanJieGuoModel models = new JiaoYanJieGuoModel();
            models.WeiYiBiaoShi = jicunqiid.WeiYiBiaoShi;
            models.SheBeiID = jicunqiid.SheBeiID;
            CunModel xinr = IsKeKao(jicunqiid);
            if (xinr != null)
            {
              
                if (xinr.IsZhengZaiCe == 1)
                {
                    models.Value = xinr.JiCunQi.Value;
                    models.IsZuiZhongJieGuo = JieGuoType.ChengGongJiGuo;
                }
                else if (xinr.IsZhengZaiCe == 3||xinr.IsZhengZaiCe==2)
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
            LinModel zsmodel = zhi as LinModel;
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
                try
                {
                    int count = sengData.GetCount();
                    if (count > 0)
                    {
                        List<CunModel> duixiang = new List<CunModel>();
                        duixiang = sengData.GetModel_Head_RomeHead();
                      
                        for (int i = 0; i < duixiang.Count; i++)
                        {
                           
                            if (duixiang[i] != null)
                            {
                                WriteRec(duixiang[i],zsmodel.SheBeiID,zsmodel.ChaoShiTime,zsmodel.MasterMode==1);

                            }
                        }
                    }
                }
                catch
                {

                 
                }
                
                Thread.Sleep(yanshi);
            }
        }

        public void WriteRec( CunModel model,int shebeiid,int chaoshitime,bool iszhu)
        {
            PeiZhiLei.DataMoXing.SetZhengZaiValue(model,0);
            if (model.IsDu == CunType.XieFanHuiLin0)
            {
                if (iszhu)
                {
                    XieLinShuJuZhu(model, shebeiid, 0, chaoshitime);
                }
                else
                {
                    XieLinShuJuCong(model, shebeiid, 0, chaoshitime);
                    return;
                }
            }
            else if (model.IsDu==CunType.XieFanHuiLin1)
            {
                if (iszhu)
                {
                    XieLinShuJuZhu(model, shebeiid, 1, chaoshitime);
                }
                else
                {
                    XieLinShuJuCong(model, shebeiid, 1, chaoshitime);
                    return;
                }
            }
            else if (model.IsDu == CunType.XieGuanLin)
            {
                GuanBiLin(shebeiid);
                PeiZhiLei.DataMoXing.SetZhengZaiValue(model, 1);
                return;
            }
            else if (model.IsDu == CunType.XieKaiLin)
            {
                DaKai(shebeiid);
                LinModel shemodel = PeiZhiLei.DataMoXing.GetSheBeiModel(shebeiid);
                if (shemodel != null && shemodel.Tx)
                {
                    PeiZhiLei.DataMoXing.SetZhengZaiValue(model, 1);
                    return;
                }
            }
    
            PeiZhiLei.DataMoXing.SetZhengZaiValue(model, 2);
        }





        private CunModel IsKeKao(JiCunQiModel model)
        {

            Dictionary<string, CunModel> sModels = PeiZhiLei.DataMoXing.JiLu;
            if (sModels.ContainsKey(model.WeiYiBiaoShi))
            {
                CunModel models = ChangYong.FuZhiShiTi(sModels[model.WeiYiBiaoShi]);
                return models;
            }

            return null;
        }

        private void XieLinShuJuCong(CunModel model,int shebeiid,byte ddd,int chaoshitime)
        {
            string[] fenge = model.JiCunQi.Value.ToString().Split(',');
            if (fenge.Length >= 3)
            {
               
                int MsgIndex = 0;
                //设置ID为LIN_EX_MSG_TYPE_SW模式，从机接收到主机发送的帧头后就会返回预先定义好的数据
                USB2LIN_EX.LIN_EX_MSG[] LINSlaveMsg = new USB2LIN_EX.LIN_EX_MSG[1];
                //配置第一帧数据
                LINSlaveMsg[MsgIndex] = new USB2LIN_EX.LIN_EX_MSG();
                LINSlaveMsg[MsgIndex].MsgType = USB2LIN_EX.LIN_EX_MSG_TYPE_SW;//从机发送数据模式
                LINSlaveMsg[MsgIndex].CheckType = USB2LIN_EX.LIN_EX_CHECK_EXT;//配置为增强校验
                LINSlaveMsg[MsgIndex].PID = ChangYong.TryByte(fenge[0], 0x01);//可以只传入ID，校验位底层会自动计算
                LINSlaveMsg[MsgIndex].Data = new Byte[8];//必须分配8字节空间
                LINSlaveMsg[MsgIndex].DataLen = 8;//实际要发送的数据字节数
                byte[] DataBuffer = ChangYong.HexStringToByte(fenge[1]);
                for (int i = 0; i < LINSlaveMsg[MsgIndex].DataLen; i++)//循环填充8字节数据
                {
                    if (i >= DataBuffer.Length)
                    {
                        LINSlaveMsg[MsgIndex].Data[i] = 0x00;
                    }
                    else
                    {
                        LINSlaveMsg[MsgIndex].Data[i] = DataBuffer[i];
                    }

                }

                int ret = USB2LIN_EX.LIN_EX_SlaveSetIDMode(DevHandle[shebeiid], ddd, LINSlaveMsg, LINSlaveMsg.Length);

                /********************需要配置更多帧，请按照前面的方式继续添加********************/
                //默认所有ID都是从机接收数据模式，该模式下可以用于数据的监听
                //调用该函数后，只会修改传入对应ID的模式，其他的不会被改变
                if (ret != USB2LIN_EX.LIN_EX_SUCCESS)
                {
                    ChuFaMsg(MsgDengJi.SheBeiCuoWu, "Config LIN ID Mode failed!");
                    PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, $"数据发送失败{ret}");
                    PeiZhiLei.DataMoXing.SetZhengZaiValue(model, 2);
                    return;
                }
                else
                {
                    ChuFaMsg(MsgDengJi.SheBeiZhengChang, "Config LIN ID Mode Success!");
                    bool zhen = LIN_GetMsgThreadCong(DevHandle[shebeiid], ddd, model.JiCunQi.WeiYiBiaoShi, fenge[2], chaoshitime);
                    if (zhen)
                    {
                        PeiZhiLei.DataMoXing.SetZhengZaiValue(model, 1);
                        return;
                    }
                    else
                    {
                        PeiZhiLei.DataMoXing.SetZhengZaiValue(model, 2);
                        return;
                    }

                }

            }
            PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "配置参数不对");
            PeiZhiLei.DataMoXing.SetZhengZaiValue(model, 2);
        }

        private  bool LIN_GetMsgThreadCong(int DevHandle, byte Channel,string weiyibiaoshi,string id,int changoshitime)
        {
            int huiid = 0;
            bool iskong = string.IsNullOrEmpty(id);
            if (iskong==false)
            {
                huiid = ChangYong.TryByte(id,0);
            }
            DateTime dateTime = DateTime.Now;
            bool Zhen = false;
            while (ZongKaiGuan)
            {
                USB2LIN_EX.LIN_EX_MSG[] LINMsg = new USB2LIN_EX.LIN_EX_MSG[512];//缓冲区尽量大一点，防止益处
                //将数组转换成指针
                IntPtr pt = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(USB2LIN_EX.LIN_EX_MSG)) * LINMsg.Length);

                int ret = USB2LIN_EX.LIN_EX_SlaveGetData(DevHandle, Channel, pt);
                
                for (int i = 0; i < ret; i++)
                {
                    LINMsg[i] = (USB2LIN_EX.LIN_EX_MSG)Marshal.PtrToStructure((IntPtr)(pt + i * Marshal.SizeOf(typeof(USB2LIN_EX.LIN_EX_MSG))), typeof(USB2LIN_EX.LIN_EX_MSG));
                    ChuFaMsg(MsgDengJi.SheBeiZhengChang, string.Format("{0},{1}", LINMsg[i].PID, ChangYong.ByteOrString(LINMsg[i].Data, " ")));
                    if (iskong)
                    {
                        if (LINMsg[i].Data != null && LINMsg[i].Data.Length > 0)
                        {
                            string weiyi = string.Format("{0},{1} ", LINMsg[i].PID, ChangYong.ByteOrString(LINMsg[i].Data, " "));
                            PeiZhiLei.DataMoXing.SetJiCunQiValue(weiyibiaoshi, weiyi);
                            Marshal.FreeHGlobal(pt);
                            return true;
                        }
                    }
                    else
                    {
                        if (LINMsg[i].PID == huiid)
                        {
                            string weiyi = string.Format("{0},{1} ", LINMsg[i].PID, ChangYong.ByteOrString(LINMsg[i].Data, " "));
                            PeiZhiLei.DataMoXing.SetJiCunQiValue(weiyibiaoshi, weiyi);
                            Marshal.FreeHGlobal(pt);
                            return true;
                        }
                    }
                   
                  
                }

               
                //释放内存
                Marshal.FreeHGlobal(pt);
                Thread.Sleep(30);
                if ((DateTime.Now-dateTime).TotalMilliseconds>=changoshitime)
                {
                    break;
                }
            }
            return Zhen;
        }


        private void XieLinShuJuZhu(CunModel model, int shebeiid, byte ddd, int chaoshitime)
        {
            string[] fenge = model.JiCunQi.Value.ToString().Split(',');
            if (fenge.Length >= 3)
            {
                int huiid = 0;
                bool iskong = string.IsNullOrEmpty(fenge[2]);
                if (iskong == false)
                {
                    huiid = ChangYong.TryByte(fenge[2], 0);
                }

                Byte[] DataBuffer = ChangYong.HexStringToByte(fenge[1]); 
                USB2LIN_EX.LIN_EX_MSG[] LINMsg = new USB2LIN_EX.LIN_EX_MSG[2];
                USB2LIN_EX.LIN_EX_MSG[] LINOutMsg = new USB2LIN_EX.LIN_EX_MSG[512];
                //添加第一帧数据
                LINMsg[0] = new USB2LIN_EX.LIN_EX_MSG();
                LINMsg[0].MsgType = USB2LIN_EX.LIN_EX_MSG_TYPE_BK;//只发送BREAK信号，一般用于唤醒休眠中的从设备
                LINMsg[0].Timestamp = 10;//发送该帧数据之后的延时时间，最小建议设置为1
               
                //添加第二帧数据
                LINMsg[1] = new USB2LIN_EX.LIN_EX_MSG();
                LINMsg[1].MsgType = USB2LIN_EX.LIN_EX_MSG_TYPE_MW;//主机发送数据
                LINMsg[1].DataLen = 8;//实际要发送的数据字节数
                LINMsg[1].Timestamp = 10;//发送该帧数据之后的延时时间
                LINMsg[1].CheckType = USB2LIN_EX.LIN_EX_CHECK_EXT;//增强校验
                LINMsg[1].PID = ChangYong.TryByte(fenge[0], 0x01);//;//可以只传入ID，校验位底层会自动计算
                LINMsg[1].Data = new Byte[8];//必须分配8字节空间
                for (int i = 0; i < LINMsg[1].DataLen; i++)//循环填充8字节数据
                {
                    if (i >= DataBuffer.Length)
                    {
                        LINMsg[1].Data[i] = 0x00;
                    }
                    else
                    {
                        LINMsg[1].Data[i] = DataBuffer[i];
                    }

                }

                IntPtr pt = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(USB2LIN_EX.LIN_EX_MSG)) * LINOutMsg.Length);//

                int ret = USB2LIN_EX.LIN_EX_MasterSync(DevHandle[shebeiid], ddd, LINMsg, pt, LINMsg.Length);
                if (ret < USB2LIN_EX.LIN_EX_SUCCESS)
                {
              
                    Marshal.FreeHGlobal(pt);
                    ChuFaMsg(MsgDengJi.SheBeiCuoWu, "MasterSync LIN  failed!");
                    PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, $"数据发送失败{ret}");
                    PeiZhiLei.DataMoXing.SetZhengZaiValue(model, 2);
                    return;
                }
                else
                {
                    //主机发送数据成功后，也会接收到发送出去的数据，通过接收回来的数据跟发送出去的数据对比，可以判断发送数据的时候，数据是否被冲突
               
                    for (int i = 0; i < ret; i++)
                    {
                        LINOutMsg[i] = (USB2LIN_EX.LIN_EX_MSG)Marshal.PtrToStructure((IntPtr)(pt + i * Marshal.SizeOf(typeof(USB2LIN_EX.LIN_EX_MSG))), typeof(USB2LIN_EX.LIN_EX_MSG));                   
                        ChuFaMsg(MsgDengJi.SheBeiZhengChang, string.Format("{0},{1}", LINOutMsg[i].PID, ChangYong.ByteOrString(LINOutMsg[i].Data, " ")));
                        if (iskong)
                        {
                            if (LINOutMsg[i].Data != null && LINOutMsg[i].Data.Length > 0)
                            {
                                string weiyi = string.Format("{0},{1} ", LINOutMsg[i].PID, ChangYong.ByteOrString(LINOutMsg[i].Data, " "));
                                PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, weiyi);
                                PeiZhiLei.DataMoXing.SetZhengZaiValue(model,1);
                                Marshal.FreeHGlobal(pt);
                                return ;
                            }
                        }
                        else
                        {
                            if (LINOutMsg[i].PID == huiid)
                            {
                                string weiyi = string.Format("{0},{1} ", LINOutMsg[i].PID, ChangYong.ByteOrString(LINOutMsg[i].Data, " "));
                                PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, weiyi);
                                PeiZhiLei.DataMoXing.SetZhengZaiValue(model, 1);
                                Marshal.FreeHGlobal(pt);
                         
                                return ;
                            }
                        }


                    }
                    Marshal.FreeHGlobal(pt);
                }
              
               

            }
            PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "配置参数不对");
            PeiZhiLei.DataMoXing.SetZhengZaiValue(model, 2);
        }

        

        private void DaKai(int shebeiid)
        {
            PeiZhiLei.DataMoXing.SetTX(shebeiid, false);
            LinModel shebaimodel = PeiZhiLei.DataMoXing.GetSheBeiModel(shebeiid);
            if (shebaimodel == null)
            {
                ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"{shebeiid},图莫斯Lin查找失败");
                return;
            }
            int linindex =shebaimodel.DuiYingLin;
            if (linindex<0)
            {
                linindex = 0;
            }
          
            USB_DEVICE.DEVICE_INFO DevInfo = new USB_DEVICE.DEVICE_INFO();
            Int32[] DevHandles = new Int32[20];

            bool state;
            Int32 DevNum, ret = 0;

            //扫描查找设备
            DevNum = USB_DEVICE.USB_ScanDevice(DevHandles);
            if (DevNum <= 0)
            {
                ChuFaMsg(MsgDengJi.SheBeiCuoWu, "No device connected!");
                return;
            }
            else
            {
                ChuFaMsg(MsgDengJi.SheBeiZhengChang, $"Have {DevNum} device connected!");
            }
            if (linindex>=DevNum)
            {
                ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"{shebeiid},图莫斯Lin配置对应的Lin超过数量,{linindex}");
                return;
            }
            DevHandle[shebeiid] = DevHandles[linindex];

            //打开设备
            state = USB_DEVICE.USB_OpenDevice(DevHandle[shebeiid]);
            if (!state)
            {
                ChuFaMsg(MsgDengJi.SheBeiCuoWu, "Open device error!");
                return;
            }
            else
            {
                ChuFaMsg(MsgDengJi.SheBeiZhengChang, "Open device success!");
            }
            //获取固件信息
            StringBuilder FuncStr = new StringBuilder(256);
            state = USB_DEVICE.DEV_GetDeviceInfo(DevHandle[shebeiid], ref DevInfo, FuncStr);
            if (!state)
            {
                ChuFaMsg(MsgDengJi.SheBeiCuoWu, "Get device infomation error!");
                return;
            }
            else
            {
                ChuFaMsg(MsgDengJi.SheBeiZhengChang, "Firmware Info:");
                ChuFaMsg(MsgDengJi.SheBeiZhengChang, "    Name:" + Encoding.Default.GetString(DevInfo.FirmwareName));
                ChuFaMsg(MsgDengJi.SheBeiZhengChang, "    Build Date:" + Encoding.Default.GetString(DevInfo.BuildDate));
                ChuFaMsg(MsgDengJi.SheBeiZhengChang, string.Format("    Firmware Version:v{0}.{1}.{2}", (DevInfo.FirmwareVersion >> 24) & 0xFF, (DevInfo.FirmwareVersion >> 16) & 0xFF, DevInfo.FirmwareVersion & 0xFFFF));
                ChuFaMsg(MsgDengJi.SheBeiZhengChang, string.Format("    Hardware Version:v{0}.{1}.{2}", (DevInfo.HardwareVersion >> 24) & 0xFF, (DevInfo.HardwareVersion >> 16) & 0xFF, DevInfo.HardwareVersion & 0xFFFF));
                ChuFaMsg(MsgDengJi.SheBeiZhengChang, "    Functions:" + DevInfo.Functions.ToString("X8"));
                ChuFaMsg(MsgDengJi.SheBeiZhengChang, "    Functions String:" + FuncStr);
            }

            //设置LIN0
            ret = USB2LIN_EX.LIN_EX_Init(DevHandle[shebeiid], 0, shebaimodel.BaudRate, shebaimodel.MasterMode);
            if (ret != USB2LIN_EX.LIN_EX_SUCCESS)
            {
                ChuFaMsg(MsgDengJi.SheBeiCuoWu, "Config LIN0 failed!");
                return;
            }
            else
            {
                ChuFaMsg(MsgDengJi.SheBeiZhengChang, "Config LIN0 Success!");
            }
            //设置LIN1
            ret = USB2LIN_EX.LIN_EX_Init(DevHandle[shebeiid], 1, shebaimodel.BaudRate, shebaimodel.MasterMode);
            if (ret != USB2LIN_EX.LIN_EX_SUCCESS)
            {
                ChuFaMsg(MsgDengJi.SheBeiCuoWu, "Config LIN1 failed!");

                return;
            }
            else
            {
                ChuFaMsg(MsgDengJi.SheBeiZhengChang, "Config LIN1 Success!");
                PeiZhiLei.DataMoXing.SetTX(shebeiid,true);
            }

        }

        private void GuanBiLin(int shebeiid)
        {
            try
            {
                PeiZhiLei.DataMoXing.SetTX(shebeiid, false);
                USB_DEVICE.USB_CloseDevice(DevHandle[shebeiid]);
            }
            catch 
            {

            }
        }

        public override KJPeiZhiJK GetCanShuKJ(string jicunweiyibiaoshi)
        {
            JiCunQiModel jiCunQiModel = new JiCunQiModel();
            jiCunQiModel.WeiYiBiaoShi = jicunweiyibiaoshi;
            CunModel cunModel = PeiZhiLei.DataMoXing.GetModel(jiCunQiModel);
            if (cunModel != null)
            {
                CanShuKJ kj = new CanShuKJ();

                kj.SetData(cunModel.IsDu);
                return kj;
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

            for (int i = 0; i < PeiZhiLei.DataMoXing.LisSheBei.Count; i++)
            {
                LinModel item = PeiZhiLei.DataMoXing.LisSheBei[i];
                ZiTxModel zmodel = new ZiTxModel();
                zmodel.Tx = item.Tx;
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
