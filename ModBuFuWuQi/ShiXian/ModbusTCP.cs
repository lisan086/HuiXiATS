using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Modbus.Data;
using Modbus.Device;
using System.Xml.Linq;
using ModBuTCP.Model;
using CommLei.JiChuLei;

namespace ModBuFuWuQi.ShiXian
{
    public  class ModbusTCP
    {
        /// <summary>
        /// 1是正常消息 2是错误消息
        /// </summary>
        public event Action<int, string> MsgEvent;
        private ModbusSlave _slave;
        private DataStore _dataStore;
        private SheBeiModel SheBeiModel;
        private TcpListener TcpListener;
        private List<DataCunModel> LisXieShuJu = new List<DataCunModel>();
        public void SetCanShu(SheBeiModel shebeinodel)
        {
            SheBeiModel = shebeinodel;
            for (int i = 0; i < shebeinodel.DataCunModels.Count; i++)
            {
                if (shebeinodel.DataCunModels[i].YingYongType== YingYongType.XiePuTong)
                {
                    LisXieShuJu.Add(shebeinodel.DataCunModels[i]);
                }
            }
        }

        public bool Open()
        {
                   
            try
            {             
                // 1. 初始化数据存储区
                _dataStore = DataStoreFactory.CreateDefaultDataStore();
                // 订阅数据写入事件，以便在PLC写入时得到通知
                _dataStore.DataStoreWrittenTo += OnDataStoreWritten;
            
                // 2. 创建并启动TCP监听
                IPAddress ip = IPAddress.Parse(SheBeiModel.IpOrCom);
                TcpListener = new TcpListener(ip, SheBeiModel.Port);
                TcpListener.Start();

                // 3. 创建Modbus从站，并将监听器传递给它
                _slave = ModbusTcpSlave.CreateTcp((byte)SheBeiModel.DiZhi, TcpListener);
                _slave.DataStore = _dataStore;
                _slave.ModbusSlaveRequestReceived += _slave_ModbusSlaveRequestReceived;
                _slave.Listen();

                
                ChuFaMsg(1,$"[{SheBeiModel.SheBeiName}] Modbus TCP从站已开始在 {SheBeiModel.IpOrCom}:{SheBeiModel.Port} 监听。");
                return true;
            }
            catch (Exception ex)
            {
                ChuFaMsg(1, $"[{SheBeiModel.SheBeiName}] Modbus TCP从站已开始在 {SheBeiModel.IpOrCom}:{SheBeiModel.Port} 失败:{ex}。");
                Close();
            }
            return false;
          
        }


        public void Close()
        {
            try
            {
                if (TcpListener != null)
                {
                    TcpListener.Stop();                   
                }
            }
            catch
            {

            }

            try
            {
                if (_slave != null)
                {
                    _slave.Dispose();
                    _slave = null;
                }
            }
            catch 
            {
              
            }

            if (_dataStore != null)
            {
                _dataStore.DataStoreWrittenTo -= OnDataStoreWritten;
                _dataStore = null;
            }
        }

        /// <summary>
        /// 当PLC向数据存储区写入数据时，此事件会被触发。
        /// </summary>
        private void OnDataStoreWritten(object sender, DataStoreEventArgs e)
        {
            // 我们只关心保持寄存器的写入
            if (e.ModbusDataType != ModbusDataType.HoldingRegister)
            {
                return;
            }
            try
            {
                ushort shoudizhi= e.StartAddress;

                int jiesudizhi = e.StartAddress + e.Data.B.Count - 1;

                for (int i = 0; i < SheBeiModel.DataCunModels.Count; i++)
                {
                    if (SheBeiModel.DataCunModels[i].YingYongType==YingYongType.DuPuTong)
                    {
                        DataCunModel shuju = SheBeiModel.DataCunModels[i];
                        int count= shuju.Count;

                        int dizhi = shuju.JiCunDiZhi;
                        if (dizhi>= shoudizhi&& dizhi<= jiesudizhi)
                        {
                            if (count == 1)
                            {
                                ushort speedReg1 = e.Data.B[dizhi- shoudizhi];
                                shuju.JiCunQiModel.Value = speedReg1;
                            }
                            else if (count==2)
                            {
                                ushort speedReg1 = e.Data.B[dizhi - shoudizhi];
                                ushort speedReg2= e.Data.B[dizhi - shoudizhi+1];
                                int shizhi= MergeRegistersToInt32(speedReg1, speedReg2);
                                shuju.JiCunQiModel.Value = shizhi;
                            }
                        }
                        
                       
                     
                    }
                }


               
            }
            catch (Exception ex)
            {
               ChuFaMsg(2,$"[{SheBeiModel.SheBeiName}] 处理PLC写入数据时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 供应用程序调用，用于更新PLC可读取的数据区域。
        /// </summary>
        /// <param name="startRegisterOffset">相对于读取区域起点的寄存器地址偏移 (0-based)</param>
        /// <param name="registers">要写入的字节数据</param>
        /// <returns>成功则返回空字符串，否则返回错误信息</returns>
        public void UpdatePlcReadData()
        {
            if (_dataStore == null)
            {
                return;
            }
            try
            {
                if (LisXieShuJu.Count>0)
                {
                 
                    for (int i = 0; i < LisXieShuJu.Count; i++)
                    {
                        int count = LisXieShuJu[i].Count;
                        int shoudizhi = LisXieShuJu[0].JiCunDiZhi;
                        if (count == 1)
                        {
                            short jieguo = ChangYong.TryShort(LisXieShuJu[i].JiCunQiModel.Value, 0);
                            if (jieguo < 0)
                            {
                                jieguo = (short)-jieguo;
                            }
                            _dataStore.HoldingRegisters[shoudizhi] = (ushort)jieguo;
                        }
                        else if (count==2)
                        {
                            int jieguo = ChangYong.TryInt(LisXieShuJu[i].JiCunQiModel.Value, 0);
                            if (jieguo < 0)
                            {
                                jieguo = -jieguo;
                            }
                            List<short> shuju = SplitIntToShorts(jieguo);
                            if (shuju.Count == 2)
                            {
                                _dataStore.HoldingRegisters[shoudizhi] = (ushort)shuju[0];
                                _dataStore.HoldingRegisters[shoudizhi + 1] = (ushort)shuju[1];
                            }
                        }
                    }
                }
             

            }
            catch (Exception ex)
            {
                ChuFaMsg(2,$"{SheBeiModel.SheBeiName}更新PLC读取区域失败: {ex.Message}");
            }

           
        }

        private void _slave_ModbusSlaveRequestReceived(object sender, ModbusSlaveRequestEventArgs e)
        {
            //Debug.WriteLine($"连接：{e.Message}");
        }
        private void ChuFaMsg(int biaozhi,string msg)
        {
            if (MsgEvent!=null)
            {
                MsgEvent(biaozhi, msg);
            }
        }

        /// <summary>
        /// 将两个ushort寄存器合并为int32
        /// </summary>
        /// <param name="highRegister">高16位寄存器（地址通常较小，如地址0）</param>
        /// <param name="lowRegister">低16位寄存器（地址通常较大，如地址1）</param>
        /// <param name="isBigEndian">是否大端模式（高位字节在前，默认true，符合多数Modbus设备）</param>
        /// <returns>合并后的int32</returns>
        private  int MergeRegistersToInt32(ushort highRegister, ushort lowRegister, bool isBigEndian = true)
        {
            // 将两个ushort转换为字节数组（每个ushort占2字节）
            byte[] highBytes = BitConverter.GetBytes(highRegister);
            byte[] lowBytes = BitConverter.GetBytes(lowRegister);

            // 合并字节数组（根据端模式调整顺序）
            byte[] combinedBytes = isBigEndian
                ? new byte[] { highBytes[0], highBytes[1], lowBytes[0], lowBytes[1] } // 大端：高寄存器字节在前
                : new byte[] { lowBytes[1], lowBytes[0], highBytes[1], highBytes[0] }; // 小端：低寄存器字节在前（需反转每个寄存器的内部字节）

            // 转换为int32
            return BitConverter.ToInt32(combinedBytes, 0);
        }

        /// <summary>
        /// 将int拆分为两个short（高16位和低16位）
        /// </summary>
        /// <param name="value">要拆分的int值</param>
        /// <param name="isBigEndian">是否大端模式（高位字节在前，默认true）</param>
        /// <returns>返回元组 (高16位short, 低16位short)</returns>
        public List<short> SplitIntToShorts(int value, bool isBigEndian = true)
        {
            List<short> zijis = new List<short>();
            // 将int转换为4字节数组
            byte[] intBytes = BitConverter.GetBytes(value);

            // 根据端模式处理字节顺序
            if (isBigEndian)
            {
                // 大端模式：高16位取前2字节，低16位取后2字节
                // 注意：BitConverter默认是小端，需反转字节以符合大端
                Array.Reverse(intBytes); // 转换为大端字节序（0x00 01 23 45 → [0x00,0x01,0x23,0x45]）

                // 高16位：前2字节
                byte[] highBytes = new byte[2] { intBytes[0], intBytes[1] };
                // 低16位：后2字节
                byte[] lowBytes = new byte[2] { intBytes[2], intBytes[3] };
                zijis.Add(BitConverter.ToInt16(highBytes, 0));
                zijis.Add(BitConverter.ToInt16(lowBytes, 0));
                return zijis;
            }
            else
            {
                // 小端模式：高16位取后2字节，低16位取前2字节
                // 低16位：前2字节
                byte[] lowBytes = new byte[2] { intBytes[0], intBytes[1] };
                // 高16位：后2字节
                byte[] highBytes = new byte[2] { intBytes[2], intBytes[3] };
                zijis.Add(BitConverter.ToInt16(highBytes, 0));
                zijis.Add(BitConverter.ToInt16(lowBytes, 0));
                return zijis;
            }
        }

    }
}
