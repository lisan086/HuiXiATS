using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TuLuSILin
{
    public   class USB_DEVICE
    {
        //定义电压输出值
        public const Byte POWER_LEVEL_NONE = 0;	//不输出
        public const Byte POWER_LEVEL_1V8 = 1;	//输出1.8V
        public const Byte POWER_LEVEL_2V5 = 2;	//输出2.5V
        public const Byte POWER_LEVEL_3V3 = 3;	//输出3.3V
        public const Byte POWER_LEVEL_5V0 = 4;	//输出5.0V
        //设备信息定义
        public struct DEVICE_INFO
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public Byte[] FirmwareName;   //固件名称字符串
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public Byte[] BuildDate;    //固件编译时间字符串
            public UInt32 HardwareVersion;//硬件版本号
            public UInt32 FirmwareVersion;//固件版本号
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public UInt32[] SerialNumber; //适配器序列号
            public UInt32 Functions;      //适配器当前具备的功能
        }
        //方法定义
        /**
          * @brief  初始化USB设备，并扫描设备连接数，必须调用
          * @param  pDevHandle 每个设备的设备号存储地址
          * @retval 扫描到的设备数量
          */
        [DllImport("USB2XXX.dll")]
        public static extern Int32 USB_ScanDevice(Int32[] pDevHandle);
        /**
          * @brief  打开设备，必须调用
          * @param  DevHandle 设备索引号
          * @retval 打开设备的状态
          */
        [DllImport("USB2XXX.dll")]
        public static extern bool USB_OpenDevice(Int32 DevHandle);
        /**
          * @brief  关闭设备
          * @param  DevHandle 设备索引号
          * @retval 关闭设备的状态
          */
        [DllImport("USB2XXX.dll")]
        public static extern bool USB_CloseDevice(Int32 DevHandle);
        /**
          * @brief  复位设备程序，复位后需要重新调用USB_ScanDevice，USB_OpenDevice函数
          * @param  DevHandle 设备索引号
          * @retval 复位设备的状态
          */
        [DllImport("USB2XXX.dll")]
        public static extern bool USB_ResetDevice(Int32 DevHandle);

        /**
          * @brief  检测到USB断开连接后，重新连接设备
          * @param  DevHandle 设备号
          * @retval 重连状态
          */
        [DllImport("USB2XXX.dll")]
        public static extern bool USB_RetryConnect(Int32 DevHandle);

        /**
          * @brief  获取设备信息，比如设备名称，固件版本号，设备序号，设备功能说明字符串等
          * @param  DevHandle 设备索引号
          * @param  pDevInfo 设备信息存储结构体指针
          * @param  pFunctionStr 设备功能说明字符串
          * @retval 获取设备信息的状态
          */
        [DllImport("USB2XXX.dll")]
        public static extern bool DEV_GetDeviceInfo(Int32 DevHandle, ref DEVICE_INFO pDevInfo, StringBuilder pFunctionStr);
        /**
          * @brief  擦出用户区数据
          * @param  DevHandle 设备索引号
          * @retval 用户区数据擦出状态
          */
        [DllImport("USB2XXX.dll")]
        public static extern bool DEV_EraseUserData(Int32 DevHandle);

        /**
          * @brief  向用户区域写入用户自定义数据，写入数据之前需要调用擦出函数将数据擦出
          * @param  DevHandle 设备索引号
          * @param  OffsetAddr 数据写入偏移地址，起始地址为0x00，用户区总容量为0x10000字节，也就是64KBye
          * @param  pWriteData 用户数据缓冲区首地址
          * @param  DataLen 待写入的数据字节数
          * @retval 写入用户自定义数据状态
          */
        [DllImport("USB2XXX.dll")]
        public static extern bool DEV_WriteUserData(Int32 DevHandle, Int32 OffsetAddr, byte[] pWriteData, Int32 DataLen);

        /**
          * @brief  从用户自定义数据区读出数据
          * @param  DevHandle 设备索引号
          * @param  OffsetAddr 数据写入偏移地址，起始地址为0x00，用户区总容量为0x10000字节，也就是64KBye
          * @param  pReadData 用户数据缓冲区首地址
          * @param  DataLen 待读出的数据字节数
          * @retval 读出用户自定义数据的状态
          */
        [DllImport("USB2XXX.dll")]
        public static extern bool DEV_ReadUserData(Int32 DevHandle, Int32 OffsetAddr, byte[] pReadData, Int32 DataLen);

        /**
          * @brief  设置可变电压输出引脚输出电压值
          * @param  DevHandle 设备索引号
          * @param  PowerLevel 输出电压值
          * @retval 设置输出电压状态
          */
        [DllImport("USB2XXX.dll")]
        public static extern bool DEV_SetPowerLevel(Int32 DevHandle, byte PowerLevel);
        /**
          * @brief  或者CAN或者LIN的时间戳原始值
          * @param  DevHandle 设备索引号
          * @param  pTimestamp 时间戳指针
          * @retval 获取时间戳状态
          */
        [DllImport("USB2XXX.dll")]
        public static extern bool DEV_GetTimestamp(Int32 DevHandle, byte BusType, Int32[] pTimestamp);

        /**
          * @brief  复位CAN/LIN时间戳，需要在初始化CAN/LIN之后调用
          * @param  DevHandle 设备索引号
          * @retval 复位时间戳状态
          */
        [DllImport("USB2XXX.dll")]
        public static extern bool DEV_ResetTimestamp(Int32 DevHandle);
        /**
          * @brief  获取dll编译日期
          * @param  pDateTime 输出DLL编译日期字符串
          * @retval 获取dll编译日期字符串
          */
        [DllImport("USB2XXX.dll")]
        public static extern bool DEV_GetDllBuildTime(StringBuilder pDateTime);
    }


   public  class USB2LIN_EX
    {
        //定义函数返回错误代码
        public const Int32 LIN_EX_SUCCESS = (0);   //函数执行成功
        public const Int32 LIN_EX_ERR_NOT_SUPPORT = (-1);  //适配器不支持该函数
        public const Int32 LIN_EX_ERR_USB_WRITE_FAIL = (-2);  //USB写数据失败
        public const Int32 LIN_EX_ERR_USB_READ_FAIL = (-3);  //USB读数据失败
        public const Int32 LIN_EX_ERR_CMD_FAIL = (-4);  //命令执行失败
        public const Int32 LIN_EX_ERR_CH_NO_INIT = (-5);  //该通道未初始化
        public const Int32 LIN_EX_ERR_READ_DATA = (-6);  //LIN读数据失败
        //LIN和校验模式
        public const Byte LIN_EX_CHECK_STD = 0;	//标准校验，不含PID
        public const Byte LIN_EX_CHECK_EXT = 1;	//增强校验，包含PID
        public const Byte LIN_EX_CHECK_USER = 2;  //自定义校验类型，需要用户自己计算并传入Check，不进行自动校验
        public const Byte LIN_EX_CHECK_NONE = 3;  //接收数据校验错误
        public const Byte LIN_EX_CHECK_ERROR = 4;  //接收数据校验错误
        //定义主从模式
        public const Byte LIN_EX_MASTER = 1;//主机
        public const Byte LIN_EX_SLAVE = 0;//从机

        public const Byte LIN_EX_MSG_TYPE_UN = 0;  //未知类型
        public const Byte LIN_EX_MSG_TYPE_MW = 1;	//主机向从机发送数据
        public const Byte LIN_EX_MSG_TYPE_MR = 2;	//主机从从机读取数据
        public const Byte LIN_EX_MSG_TYPE_SW = 3;	//从机发送数据
        public const Byte LIN_EX_MSG_TYPE_SR = 4;	//从机接收数据
        public const Byte LIN_EX_MSG_TYPE_BK = 5;	//只发送BREAK信号，若是反馈回来的数据，表明只检测到BREAK信号
        public const Byte LIN_EX_MSG_TYPE_SY = 6;	//表明检测到了BREAK，SYNC信号
        public const Byte LIN_EX_MSG_TYPE_ID = 7;	//表明检测到了BREAK，SYNC，PID信号
        public const Byte LIN_EX_MSG_TYPE_DT = 8;	//表明检测到了BREAK，SYNC，PID,DATA信号
        public const Byte LIN_EX_MSG_TYPE_CK = 9;	//表明检测到了BREAK，SYNC，PID,DATA,CHECK信号

        //LIN数据帧格式定义
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct LIN_EX_MSG
        {
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 Timestamp;    //时间戳
            [MarshalAs(UnmanagedType.U1)]
            public Byte MsgType;	    //帧类型
            [MarshalAs(UnmanagedType.U1)]
            public Byte CheckType;    //校验类型
            [MarshalAs(UnmanagedType.U1)]
            public Byte DataLen;	    //LIN数据段有效数据字节数
            [MarshalAs(UnmanagedType.U1)]
            public Byte Sync;			//固定值，0x55
            [MarshalAs(UnmanagedType.U1)]
            public Byte PID;			//帧ID		
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.U1)]
            public Byte[] Data;	    //数据
            [MarshalAs(UnmanagedType.U1)]
            public Byte Check;		//校验,只有校验数据类型为LIN_EX_CHECK_USER的时候才需要用户传入数据
            [MarshalAs(UnmanagedType.U1)]
            public Byte BreakBits;  //该帧的BRAK信号位数，有效值为10到26，若设置为其他值则默认为13位
            [MarshalAs(UnmanagedType.U1)]
            public Byte Reserve1;
        }
        //初始化
        [DllImport("USB2XXX.dll")]
        public static extern Int32 LIN_EX_Init(Int32 DevHandle, Byte LINIndex, Int32 BaudRate, Byte MasterMode);
        //主机模式操作函数
        [DllImport("USB2XXX.dll")]
        public static extern Int32 LIN_EX_MasterSync(Int32 DevHandle, Byte LINIndex, LIN_EX_MSG[] pInMsg, IntPtr pOutMsg, Int32 MsgLen);
        [DllImport("USB2XXX.dll")]
        public static extern Int32 LIN_EX_MasterBreak(Int32 DevHandle, Byte LINIndex);
        [DllImport("USB2XXX.dll")]
        public static extern Int32 LIN_EX_MasterWrite(Int32 DevHandle, Byte LINIndex, Byte PID, Byte[] pData, Byte DataLen, Byte CheckType);
        [DllImport("USB2XXX.dll")]
        public static extern Int32 LIN_EX_MasterRead(Int32 DevHandle, Byte LINIndex, Byte PID, Byte[] pData);
        //从机模式操作函数
        [DllImport("USB2XXX.dll")]
        public static extern Int32 LIN_EX_SlaveSetIDMode(Int32 DevHandle, Byte LINIndex, LIN_EX_MSG[] pLINMsg, Int32 MsgLen);
        [DllImport("USB2XXX.dll")]
        public static extern Int32 LIN_EX_SlaveGetIDMode(Int32 DevHandle, Byte LINIndex, IntPtr pLINMsg);
        [DllImport("USB2XXX.dll")]
        public static extern Int32 LIN_EX_SlaveGetData(Int32 DevHandle, Byte LINIndex, IntPtr pLINMsg);
        //电源控制相关函数
        [DllImport("USB2XXX.dll")]
        public static extern Int32 LIN_EX_CtrlPowerOut(Int32 DevHandle, Byte LINIndex, Byte State);
        [DllImport("USB2XXX.dll")]
        public static extern Int32 LIN_EX_GetVbatValue(Int32 DevHandle, Int16[] pBatValue);
        //主机模式自动发送数据相关函数
        [DllImport("USB2XXX.dll")]
        public static extern Int32 LIN_EX_MasterStartSch(Int32 DevHandle, Byte LINIndex, LIN_EX_MSG[] pLINMsg, Int32 MsgLen);
        [DllImport("USB2XXX.dll")]
        public static extern Int32 LIN_EX_MasterStopSch(Int32 DevHandle, Byte LINIndex);
        [DllImport("USB2XXX.dll")]
        public static extern Int32 LIN_EX_MasterGetSch(Int32 DevHandle, Byte LINIndex, IntPtr pLINMsg);

        [DllImport("USB2XXX.dll")]
        public static extern Int32 LIN_EX_MasterOfflineSch(Int32 DevHandle, Byte LINIndex, Int32 BaudRate, LIN_EX_MSG[] pLINMsg, Int32 MsgLen);
        [DllImport("USB2XXX.dll")]
        public static extern Int32 LIN_EX_DecodeListFile(string pFileName, Byte CheckType, Int32 BaudRate, Byte[] pReadDataList, Byte ReadDataListLen, Byte[] pCheckTypeList, Byte CheckTypeListLen);
        [DllImport("USB2XXX.dll")]
        public static extern Int32 LIN_EX_GetListFileMsg(Int32 MsgIndex, Int32 MsgLen, IntPtr pLINMsg);
    }
}
