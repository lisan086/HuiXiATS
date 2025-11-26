using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using CommLei.DataChuLi;
using CommLei.GongYeJieHe;
using CommLei.JiChuLei;
using NationalInstruments;
using NationalInstruments.DAQmx;
using NationalInstruments.DataInfrastructure;
using SSheBei.ABSSheBei;
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
                return "采集音频分析";
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
            List<SaoMaModel> sModels = PeiZhiLei.DataMoXing.LisSheBei;
            StringBuilder msg = new StringBuilder();
            foreach (var item in sModels)
            {
                PeiZhiLei.DataMoXing.SetHeGe(item.SheBeiID, true);
                msg.AppendLine($"{item.Name},已经打开");
            }
            DengDaiOpen = true;
            ChuFaMsg(MsgDengJi.SheBeiZhengChang, msg.ToString());

        }
        public override void Close()
        {
            ZongKaiGuan = false;
            Thread.Sleep(100);

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
        
        private void JieRu(List<JiCunQiModel> canshus)
        {
            foreach (var item in canshus)
            {
                CunModel ipcom =PeiZhiLei.DataMoXing.GetModel(item);

                if (ipcom != null)
                {
                    if (ipcom.IsDu.ToString().ToLower().StartsWith("du"))
                    {
                        continue;
                    }
                   
                    PeiZhiLei.DataMoXing.SetZhengZaiValue(ipcom.JiCunQi.WeiYiBiaoShi,0);
                    if (SengData.ContainsKey(ipcom.ZongSheBeiId))
                    {
                        CunModel cunmodel = ipcom.FuZhi();
                        cunmodel.JiCunQi = item.FuZhi();
                        SengData[ipcom.ZongSheBeiId].Add(new List<CunModel>() { cunmodel });
                    }
                }
            }
        }
        public override JiaoYanJieGuoModel JiaoYanChengGong(JiCunQiModel jicunqiid)
        {
            JiaoYanJieGuoModel models = new JiaoYanJieGuoModel();
            models.WeiYiBiaoShi = jicunqiid.WeiYiBiaoShi;
            models.SheBeiID = jicunqiid.SheBeiID;
            CunModel xinr = PeiZhiLei.DataMoXing.GetModel(jicunqiid);
            if (xinr != null)
            {
                if (xinr.JiCunQi.IsKeKao)
                {
                    if (xinr.IsZhengZaiCe == 1)
                    {
                        models.Value = xinr.JiCunQi.Value;
                        models.IsZuiZhongJieGuo = JieGuoType.ChengGongJiGuo;
                    }
                    else if (xinr.IsZhengZaiCe == 3)
                    {
                        models.Value = xinr.JiCunQi.Value;
                        models.IsZuiZhongJieGuo = JieGuoType.ShiBaiJiGuo;
                    }
                    else if (xinr.IsZhengZaiCe == 2)
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
                    models.Value = "通讯不可靠";
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
            SaoMaModel zsmodel = zhi as SaoMaModel;
           
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
                if ((DateTime.Now- chongliantime).TotalMilliseconds>=2000)
                {
                  
                    chongliantime = DateTime.Now;
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
                         
                            if (duixiang[i]!= null)
                            {                               
                                WriteRec(duixiang[i], zsmodel);
                                Thread.Sleep(10);
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

        public void WriteRec(CunModel model, SaoMaModel shebeimodel)
        {
            if (model != null)
            {
                if (model.IsDu == CunType.XieKaiShiCaiJi)
                {

                    DateTime kaishi = DateTime.Now;
                    double rate = shebeimodel.CaiYangLv;
                    string taskname = shebeimodel.TaskName;
                    string tongdaoname = shebeimodel.CaiJiPort;
                    int caiyangshuliang = shebeimodel.CaiJiShuLiang;


                    try
                    {
                        #region MyRegion
                        //  2.Create a channel
                        //     tarenwu.AIChannels.CreateVoltageChannel(tongdaoname, "", (AITerminalConfiguration)(-1), -10, 10, AIVoltageUnits.Volts);

                        // // 3.Configure Timing Specs
                        //    tarenwu.Timing.ConfigureSampleClock("", rate, SampleClockActiveEdge.Rising, SampleQuantityMode.FiniteSamples, shebeimodel.CaiJiShuLiang);

                        ////  4.Verify the task
                        //   tarenwu.Control(TaskAction.Verify);
                        ////  5.Read the data
                        //    AnalogMultiChannelReader reader = new AnalogMultiChannelReader(tarenwu.Stream);
                        //  AnalogWaveform<double>[] data = reader.ReadWaveform(shebeimodel.CaiJiShuLiang);
                        //  List<double> shujus = new List<double>();
                        //  foreach (var item in data)
                        //  {
                        //      for (int i = 0; i < item.Samples.Count; i++)
                        //      {
                        //          double value1 = item.Samples[i].Value;
                        //          shujus.Add(value1);
                        //      }

                        //  }
                        //tarenwu.AIChannels.CreateVoltageChannel(tongdaoname, "", AITerminalConfiguration.Rse, -10, 10, AIVoltageUnits.Volts);
                        //tarenwu.Timing.ConfigureSampleClock("", rate, SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples);
                        //AnalogSingleChannelReader read = new AnalogSingleChannelReader(tarenwu.Stream);
                        //tarenwu.Start();
                        //double[] shuju22 = read.ReadMultiSample(caiyangshuliang);
                        #endregion

                        double[] shuju = AcquireData(tongdaoname, caiyangshuliang, rate, shebeimodel.FuZhi);

                        Array.ConvertAll(shuju, x => (double)x / shebeimodel.GuiYiZhi);

                        PeiZhiLei.DataMoXing.YuanShiDian = shuju.ToList();
                        if (shebeimodel.IsLvBo)
                        {
                            shuju = FourierTransform.RemoveSpecificFrequency(shuju, (int)shebeimodel.CaiYangLv, shebeimodel.LvBoWidth);
                        }

                        PeiZhiLei.DataMoXing.SetJiCunQiValue(model, shuju);
                        PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 1);
                        ChuFaMsg(MsgDengJi.SheBeiXie, $"{shebeimodel.Name}写测试数据,用时ms:{(DateTime.Now - kaishi).TotalMilliseconds}:数据{string.Join(" ", shuju)}");
                        PeiZhiLei.DataMoXing.LvBoDianDian = shuju.ToList();

                    }
                    catch
                    {
                        PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "没有找到相应的配置");
                        PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);
                    }




                }
                else if (model.IsDu == CunType.XieCaiJiZaoYing)
                {
                    PeiZhiLei.DataMoXing.ZaoYing.Clear();
                    DateTime kaishi = DateTime.Now;
                    double rate = shebeimodel.CaiYangLv;
                    string taskname = shebeimodel.TaskName;
                    string tongdaoname = shebeimodel.CaiJiPort;
                    int caiyangshuliang = shebeimodel.CaiJiShuLiang;


                    try
                    {
                    

                        double[] shuju = AcquireData(tongdaoname, caiyangshuliang, rate, shebeimodel.FuZhi);
                        PeiZhiLei.DataMoXing.ZaoYing.AddRange(shuju);
                        PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "OK");
                        PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 1);
                        ChuFaMsg(MsgDengJi.SheBeiXie, $"{shebeimodel.Name}写测试数据,用时ms:{(DateTime.Now - kaishi).TotalMilliseconds}:数据{string.Join(" ", shuju)}");
                        PeiZhiLei.DataMoXing.LvBoDianDian = shuju.ToList();

                    }
                    catch
                    {
                        PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "没有找到相应的配置");
                        PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);
                    }
                }
                else
                {
                    PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "没有找到相应的配置");
                    PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);
                }

            }
            else
            {
                PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);
            }
        }

        private  double[] AcquireData(string deviceName, int samplesPerChannel, double sampleRate,double fuzhi)
        {
            // 创建任务
            using (Task task = new Task())
            {
                // 添加模拟输入通道（如 AI0）
                task.AIChannels.CreateVoltageChannel(
                    deviceName,    // 设备名 + 通道
                    "",            // 通道名
                    AITerminalConfiguration.Rse, // 差分输入
                    -fuzhi, fuzhi,            // 输入范围（±10V）
                    AIVoltageUnits.Volts    // 单位
                );

                // 配置采样率与采样数
                task.Timing.ConfigureSampleClock(
                    "",                     // 外部时钟（若为空则使用内部时钟）
                    sampleRate,             // 采样率（Hz）
                    SampleClockActiveEdge.Rising,
                    SampleQuantityMode.FiniteSamples,
                    samplesPerChannel
                );

                // 读取数据
                AnalogMultiChannelReader reader = new AnalogMultiChannelReader(task.Stream);
                double[,] data = reader.ReadMultiSample(samplesPerChannel);

                // 转换为 1D 数组（假设单通道）
                double[] signal = new double[samplesPerChannel];
                for (int i = 0; i < samplesPerChannel; i++)
                {
                    signal[i] = data[0, i];
                }

                return signal;
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
