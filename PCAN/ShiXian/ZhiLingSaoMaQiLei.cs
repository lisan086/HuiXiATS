using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using CommLei.DataChuLi;
using CommLei.GongYeJieHe;
using CommLei.JiChuLei;
using Pcan.Frm;
using PCANFD.ShiXian;
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
                return "PCANFD";
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

        private Dictionary<int,bool> KaiGuan = new Dictionary<int, bool>();


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
                    if (SengData.ContainsKey(sModels[i].SheBeiID) == false)
                    {
                        KaiGuan.Add(sModels[i].SheBeiID,false);
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
            foreach (var item in SengData.Keys)
            {
                PeiZhiLei.DataMoXing.SetHeGe(item,false);
            }
           
            DengDaiOpen = true;
            ChuFaMsg(MsgDengJi.SheBeiZhengChang, "打开PCAN设备");
         
        }
        public override void Close()
        {
            ZongKaiGuan = false;
            Thread.Sleep(100);
            List<SaoMaModel> lisd = PeiZhiLei.DataMoXing.LisSheBei;
            foreach (var item in lisd)
            {
                GuaanBiCAN(item.PHandle);
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
            CunModel cunmodel = PeiZhiLei.DataMoXing.GetModel(jicunqi);
            if (cunmodel!=null)
            {            
                PeiZhiLei.DataMoXing.SetZhengZaiValue(cunmodel.JiCunQi.WeiYiBiaoShi, 0);
                return cunmodel;
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
                    if (SengData.ContainsKey(ipcom.ZongSheBeiId))
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
            CunModel xinr = PeiZhiLei.DataMoXing.IsChengGong(models.WeiYiBiaoShi);
            if (xinr != null)
            {
                if (xinr.IsZhengZaiCe == 1)
                {
                    models.Value = xinr.JiCunQi.Value;
                    models.IsZuiZhongJieGuo = JieGuoType.ChengGongJiGuo;
                }
                else if (xinr.IsZhengZaiCe >= 2)
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
          
            int yanshi = 5;
            FanXingJiHeLei<List<CunModel>> sengData = SengData[zsmodel.SheBeiID];
          
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
                            if (duixiang[i]!=null)
                            {
                              
                                WriteRec(duixiang[i], zsmodel);
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

        public void WriteRec(CunModel model,SaoMaModel moggf)
        {
            PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi,0);
            if (model.IsDu == CunType.XieCANYouZhiRec)
            {
                if (KaiGuan[moggf.SheBeiID])
                {
                    string recid = "";
                    bool zhne = WriteFrame(model, moggf.PHandle, moggf.Name, out recid);
                    if (zhne)
                    {
                        DuShuJu(model, moggf, recid);

                    }
                    else
                    {
                        PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "写数据没有成功");
                        PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);
                    }
                }
                else
                {
                    PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "设备没有通信");
                    PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);
                }

            }
            else if (model.IsDu == CunType.XieCANWuZhiRec)
            {
                if (KaiGuan[moggf.SheBeiID])
                {
                    string recid = "";
                    bool zhne = WriteFrame(model, moggf.PHandle, moggf.Name, out recid);
                    if (zhne)
                    {
                        PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "OK");
                        PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 1);
                    }
                    else
                    {
                        PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "写数据没有成功");
                        PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);
                    }
                }
                else
                {
                    PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "设备没有通信");
                    PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);
                }
            }
            else if (model.IsDu == CunType.XieCANZhiDu)
            {
                if (KaiGuan[moggf.SheBeiID])
                {
                    try
                    {
                        DuShuJu(model, moggf,ChangYong.TryStr(model.JiCunQi.Value,""));
                    }
                    catch
                    {
                        PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "传来的数据有问题");
                        PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);
                    }

                }
                else
                {
                    PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "设备没有通信");
                    PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);
                }

            }
            else if (model.IsDu == CunType.XieGuanCAN)
            {
                KaiGuan[moggf.SheBeiID] = false;
                PeiZhiLei.DataMoXing.SetHeGe(moggf.SheBeiID, false);
                GuaanBiCAN(moggf.PHandle);
                PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "OK");
                PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 1);
                ChuFaMsg(MsgDengJi.SheBeiZhengChang,$"{moggf.Name}:关闭pcan");
                return;
            }
            else if (model.IsDu == CunType.XieLianJieCAN)
            {
                bool zhen = DaKaiCAN(moggf.PHandle);
                KaiGuan[moggf.SheBeiID] = zhen;
                PeiZhiLei.DataMoXing.SetHeGe(moggf.SheBeiID, zhen);
                PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "OK");

                PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, zhen
                    ? 1 : 2);
                ChuFaMsg(MsgDengJi.SheBeiZhengChang, $"{moggf.Name}:打开pcan {zhen}");
                return;
            }
            else
            {
                PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);
            }
        }

        /// <summary>
        /// Configures the PCAN-Trace file for a PCAN-Basic Channel
        /// </summary>
        private void ConfigureTraceFile(ushort hhh)
        {
            UInt32 iBuffer;
            TPCANStatus stsResult;

            // Configure the maximum size of a trace file to 5 megabytes
            //
            iBuffer = 5;
            stsResult = PCANBasic.SetValue(hhh, TPCANParameter.PCAN_TRACE_SIZE, ref iBuffer, sizeof(UInt32));
            if (stsResult != TPCANStatus.PCAN_ERROR_OK)
            {
                ChuFaMsg(MsgDengJi.SheBeiBaoWen,GetFormatedError(stsResult));
            }

            // Configure the way how trace files are created: 
            // * Standard name is used
            // * Existing file is ovewritten, 
            // * Only one file is created.
            // * Recording stopts when the file size reaches 5 megabytes.
            //
            iBuffer = PCANBasic.TRACE_FILE_SINGLE | PCANBasic.TRACE_FILE_OVERWRITE;
            stsResult = PCANBasic.SetValue(hhh, TPCANParameter.PCAN_TRACE_CONFIGURE, ref iBuffer, sizeof(UInt32));
            if (stsResult != TPCANStatus.PCAN_ERROR_OK)
                ChuFaMsg(MsgDengJi.SheBeiBaoWen, GetFormatedError(stsResult));
        }
        /// <summary>
        /// Help Function used to get an error as text
        /// </summary>
        /// <param name="error">Error code to be translated</param>
        /// <returns>A text with the translated error</returns>
        private string GetFormatedError(TPCANStatus error)
        {
            StringBuilder strTemp;

            // Creates a buffer big enough for a error-text
            //
            strTemp = new StringBuilder(256);
            // Gets the text using the GetErrorText API function
            // If the function success, the translated error is returned. If it fails,
            // a text describing the current error is returned.
            //
            if (PCANBasic.GetErrorText(error, 0, strTemp) != TPCANStatus.PCAN_ERROR_OK)
                return string.Format("An error occurred. Error-code's text ({0:X}) couldn't be retrieved", error);
            else
                return strTemp.ToString();
        }

        private void GuaanBiCAN(ushort hh)
        {
            try
            {
                PCANBasic.Uninitialize(hh);

            }
            catch 
            {

             
            }
        }

        private bool DaKaiCAN(ushort hh)
        {
            StringBuilder msg = new StringBuilder();
            List<SaoMaModel> lisd = PeiZhiLei.DataMoXing.LisSheBei;
            bool jia=false;
            foreach (var item in lisd)
            {
                if (item.PHandle!=hh)
                {
                    continue;
                }
                TPCANStatus stsResult = PCANBasic.InitializeFD(
                      item.PHandle,
                   item.m_Baudrate);

                if (stsResult != TPCANStatus.PCAN_ERROR_OK)
                {
                    if (stsResult != TPCANStatus.PCAN_ERROR_CAUTION)
                    {
                        msg.AppendLine($"{item.Name}:{GetFormatedError(stsResult)}");
                    }
                    item.TX = false;
                }
                else
                {
                    ConfigureTraceFile(item.PHandle);

                    msg.AppendLine($"{item.Name}:成功CAN成功");

                    jia = true;
                    item.TX = true;
                }
            }
         
            ChuFaMsg(MsgDengJi.SheBeiZhengChang, msg.ToString());
            return jia;
        }

        private void DuShuJu(CunModel model, SaoMaModel moggf,string recid)
        {
            int id = -1;
            try
            {
                id = (int)Convert.ToUInt32(recid, 16);
            }
            catch
            {

               
            }
            DateTime shijian = DateTime.Now;
            for (; ZongKaiGuan;)
            {
                TPCANMsgFD CANMsg;

                TPCANStatus stsResult;
                ulong CANTimeStamp;

                stsResult = PCANBasic.ReadFD(moggf.PHandle, out CANMsg, out CANTimeStamp);
                if (stsResult != TPCANStatus.PCAN_ERROR_QRCVEMPTY)
                {
                    if (id<0)
                    {
                        string jieguo = $"{CANMsg.ID.ToString("X2")},{ChangYong.ByteOrString(CANMsg.DATA, " ")}";
                        PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, jieguo);
                        PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 1);
                        ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"{moggf.Name}:接收{jieguo}");

                        return;
                    }
                    else
                    {
                        if (CANMsg.ID == id)
                        {
                            string jieguo = $"{CANMsg.ID.ToString("X2")},{ChangYong.ByteOrString(CANMsg.DATA, " ")}";
                            PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, jieguo);
                            PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 1);
                            ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"{moggf.Name}:接收{jieguo}");
                            return;
                        }
                    }

                }

                bool zhen = ChaoShiTime(shijian, model, moggf);
                if (zhen)
                {
                    return;
                }

            }
        }

        private bool ChaoShiTime(DateTime kaishi, CunModel model, SaoMaModel moggf)
        {
            if ((DateTime.Now - kaishi).TotalMilliseconds >= model.ChaoTime)
            {
                PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "DataChaoShi");
                PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 3);
                ChuFaMsg(MsgDengJi.SheBeiZhengChang, $"{moggf.Name}:{model.IsDu} 数据超时");
                return true;
            }
            return false;
        }

        private bool WriteFrame(CunModel model,ushort hh,string name,out string id)
        {
            id = "";
            string[] zhi=model.JiCunQi.Value.ToString().Split(',');
            if (zhi.Length >= 2)
            {
                if (zhi.Length >= 3)
                {
                    id = zhi[2];
                }
                try
                {
                    TPCANMsgFD CANMsg;               
                    CANMsg = new TPCANMsgFD();
                    CANMsg.DATA = new byte[64];

                    byte[] DataBuffer = ChangYong.HexStringToByte(zhi[1]);
                    CANMsg.ID = Convert.ToUInt32(zhi[0], 16);
                  
                    CANMsg.DLC = Convert.ToByte(DataBuffer.Length);
                    CANMsg.MSGTYPE = false ? TPCANMessageType.PCAN_MESSAGE_EXTENDED : TPCANMessageType.PCAN_MESSAGE_STANDARD;
                    CANMsg.MSGTYPE |= true ? TPCANMessageType.PCAN_MESSAGE_FD : TPCANMessageType.PCAN_MESSAGE_STANDARD;
                    CANMsg.MSGTYPE |= false ? TPCANMessageType.PCAN_MESSAGE_BRS : TPCANMessageType.PCAN_MESSAGE_STANDARD;
                    for (int i = 0; i < DataBuffer.Length; i++)
                    {

                        CANMsg.DATA[i] = DataBuffer[i];
                    }

               
                    TPCANStatus tPCANStatus = PCANBasic.WriteFD(hh, ref CANMsg);
                    if (tPCANStatus == TPCANStatus.PCAN_ERROR_OK)
                    {
                        ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"{name}:发送{CANMsg.ID.ToString("X2")},{ChangYong.ByteOrString(CANMsg.DATA, " ")} 成功");

                        return true;
                    }                 		
                    else
                    {
                        ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"{name}:发送{CANMsg.ID.ToString("X2")},{ChangYong.ByteOrString(CANMsg.DATA, " ")} 失败 {GetFormatedError(tPCANStatus)}");
                     
                        return false;
                    }
                }
                catch 
                {

                }
       


            }
            ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"{name}:{model.JiCunQi.Value} 数据有问题");
            return false;

        }

        public override KJPeiZhiJK GetCanShuKJ(string jicunweiyibiaoshi)
        {
            JiCunQiModel jiCunQiModel = new JiCunQiModel();
            jiCunQiModel.WeiYiBiaoShi = jicunweiyibiaoshi;
            CunModel cunModel = PeiZhiLei.DataMoXing.GetModel(jiCunQiModel);
            if (cunModel != null )
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
            model.IsXuYaoPanDuan = false;
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

        public override void Clear(bool isquanbu, JiCunQiModel model)
        {
            
        }
    }
}
