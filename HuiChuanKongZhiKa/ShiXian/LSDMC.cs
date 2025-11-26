using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LeiSaiDMC.ShiXian
{
    public class Imc60
    {

        #region STATIC
        public const string Imc60ApiDllName = "IMC_Library_x64.dll";
        #endregion

        #region FUNCTION RETURN DEFINE
        public const uint EXE_SUCCESS = 0x00000000;
        public const uint ERR_HANDLE = 0xFFFFFFFF;
        #endregion

        /// @brief  主站信息结构体
        [StructLayout(LayoutKind.Sequential)]
        public struct TMasterInfo
        {
            public UInt32 cycleTime; // 规划周期
            public Int16 sysHwCfg;         // 系统类型: 0: Ecat+端子板, 1 Ecat, 2: 端子板
            public Int16 aliasMode;        // 别名模式: 0 未开启, 1 开启
            public Int16 pdoLen;           // pdo总长度
            public Int16 slaveCnt;         // 从站个数
            public Int16 axisCnt;          // 轴个数
            public Int16 dioModuleCnt;     // dio模块个数
            public Int16 aioModuleCnt;     // aio模块个数
            public Int16 regModuleCnt;     // reg模块个数
            public Int16 diCnt;            // di数量
            public Int16 doCnt;            // do数量
            public Int16 aiCnt;            // ai,AD数量
            public Int16 aoCnt;            // ao, DA数量
            public Int16 regInCnt;         // reg输入数量
            public Int16 regOutCnt;        // reg输出数量
            public Int16 encCnt;           // enc通道数量
            public Int16 align;
        };

        /// @brief  从站信息结构体
        [StructLayout(LayoutKind.Sequential)]
        public struct TSlaveInfo
        {
            public Int16 devType;           // 设备类型
            public UInt32 vendorId;         // 设备ID
            public UInt32 productCode;      // 产品编码
            public UInt32 revisionNo;       // 版本号
            public Int16 axChn;             // 从站起始通道号
            public Int16 axisCnt;           // 当前从站轴数
            public Int16 actStation;        // 当前从站对应的实际站号
            public UInt32 aliasNo;          // 别名
            public UInt32 align;
        };

        /// @brief  PdoEntry结构体
        [StructLayout(LayoutKind.Sequential)]
        public struct TPdoEntry
        {
            public UInt16 Index;            // 索引
            public Byte SubIndex;           // 子索引
            public Byte DevType;            // entry类型
            public Int16 BitOfs;            // 位偏移
            public UInt32 BitLen;           // 位长度
        };

        /// @brief  回零参数结构体
        [StructLayout(LayoutKind.Sequential)]
        public struct THomingPara
        {
            public Int16 homeMethod;     // 回原点方法
            public UInt32 highVel; // 高速搜索减速点速度 pulse/s
            public UInt32 lowVel;  // 搜索原点低 pulse/s
            public UInt32 acc;     // 加速度 pulse/s^2
            public Int32 offset;           // 回原点后的零点偏执 pulse
        };
        /// @brief  多维位置比较数据结构体
        [StructLayout(LayoutKind.Sequential)]
        public struct TMultiCmpData
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public Int32[] compareData; // 比较的位置
        };

        /// @brief    规划运动限制参数结构体
        [StructLayout(LayoutKind.Sequential)]
        public struct TMtPara
        {
            public double bgVel;    // 起始速度值   unit/s
            public double maxVel;   // 最大速度     unit/s
            public double maxAcc;   // 最大加速度   unit/s^2
            public double maxDec;   // 最大减速度   unit/s^2
            public double maxJerk;  // 最大加加速度   unit/s^3
            public double stopDec;  // 平滑停止减速度 unit/s^2
            public double eStopDec; // 急停减速度   unit/s^2
        };

        /// @brief  轴属性配置结构体
        [StructLayout(LayoutKind.Sequential)]
        public struct TAxAttriPara
        {
            public Int16 arrivalBand;   // 到位误差 pulse
            public UInt16 arrivalTime;   // 到位保持时间 周期
            public Int32 errorLmt;        // 最大跟随误差 pulse
            public Int32 softPosLimitPos; // 软正限位 pulse
            public Int32 softNegLimitPos; // 软负限位 pulse
        };

        /// @brief  轴安全检查配置结构体
        [StructLayout(LayoutKind.Sequential)]
        public struct TAxCheckEn
        {
            public Int16 alarmEn;    // 报警是否有效标志
            public Int16 softLmtEn;  // 软限位是否有效标志
            public Int16 hwLmtEn;    // 硬限位是否有效标志
            public Int16 errorLmtEn; // 跟随误差是否检查标志
        };

        /// @brief  插补高级参数结构体
        [StructLayout(LayoutKind.Sequential)]
        public struct TCrdAdvParam
        {
            public Int16 userVelMode;       // 用户速度规划模式: 0 系统前瞻速度规划, 1 用户设定速度规划(默认: 0)
            public Int16 transMode;         // 过渡模式 0: 无过渡 1: 圆弧过渡 (默认:1)
            public Int16 noDataProtect;     // 数据断流保护: 0不保护, 1保护 (默认: 1)
            public Int16 circAccChangeEn;   // 圆弧变加速使能: 0不变加速, 1变加速 (默认: 0)
            public Int16 noCoplaneCircOptm; // 异面圆弧优化: 0不开启, 1开启
            public double turnCoef;         // 拐弯系数：[0.01,100](默认：1.0)
            public double tol;              // 插补精度：[0,1e9](默认：0 , 单位取决于设置的当量)
        };

        /// @brief  电子齿轮参数结构体
        [StructLayout(LayoutKind.Sequential)]
        public struct TGearParam
        {
            public Int32 masterScale;    // 主轴齿数: (0,intMax]
            public Int32 slaveScale;     // 从轴齿数: [intMin,0) || (0,intMax]
            public Int16 masterNo;       // 主轴轴号: [0,63] 
            public Int16 masterType;     // 主轴类型: 0 轴规划, 1 轴编码器, 10 端子板编码器, 11 EtherCAT编码器
            public Int16 dirMode;        // 方向模式: 0 跟随方向不限制, 1 正向绝对位置跟随, 2 负向绝对位置跟随, 3 正向相对位置跟随, 4 负向相对位置跟随
            public Int32 masterSlopeDis; // 主轴离合区:[0,intMax], 0表示没有加速过程，直接跟随
        };

        /// @brief  表补偿参数结构体
        public struct TTableCompParam
        {
            public Int16 tableId;       // 表索引:[0,0]
            public Int16 dimension;     // 补偿维数:{1,2,3}
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public Int16[] srcAxNo;     // 参考轴号:[0,63],数组有效的元素与补偿维数有关,如补偿维数=1,仅用到srcAxNo[0], 下同
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public Int16[] srcType;     // 参考轴类型:0 轴规划, 1 轴编码器
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public Int32[] startPos;    // 补偿起始位置:[intMin,intMax]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public Int32[] count;       // 补偿点个数:[2,40000] （有效的元素的乘积不得超过40000，且各不小于2)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public Int32[] step;        // 补偿间隔:[1,intMax]
        };


        /// @brief  采样配置参数结构体
        [StructLayout(LayoutKind.Sequential)]
        public struct TSamplePara
        {
            public Int16 interval; // 采样时间间隔
            public Int16 trigType; // 触发采样类型: 0 立即 1 延时 2 本地di 3 ECAT di
            public Int16 delay;    // 延时时间 单位：周期
            public Int16 diNo;     // di输入号
            public Int16 diLevel;  // di的触发输入值 0或1
        };

        /// @brief  事件配置参数结构体
        /// 
        #region Event
        [StructLayout(LayoutKind.Sequential)]
        public struct TEvent
        {
            public Int16 type;   //事件类型
            public Int16 index;   //事件变量索引
            public UInt32 loop;   //事件最大允许触发次数
            public double value;  //设置比较值
            public Int16 condition;  //事件条件
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct TTaskCrdStart
        {
            public Int16 crdNo;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct TTaskStartPtp
        {
            public Int16 axNo;
            public Int16 posType;
            public double tgtPos;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct TTaskStartJog
        {
            public Int16 axNo;
            public double tgtVel;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct TTaskStartGear
        {
            public Int16 axNo;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct TTaskStartPt
        {
            public Int16 sysNo;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct TTaskStartPvt
        {
            public Int16 axNum;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public Int16[] axArray;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct TTaskUpdatePtpMvPara
        {
            public Int16 axNo;
            public double tgtVel;
            public double tgtAcc;
            public double tgtDec;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct TTaskUpdateJogMvPara
        {
            public Int16 axNo;
            public double tgtVel;
            public double tgtAcc;
            public double tgtDec;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct TTaskUpdatePtpTgtPos
        {
            public Int16 axNo;
            public double tgtPos;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct TTaskSetEcatDoBit
        {
            public Int16 doNo;
            public Int16 value;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct TTaskSetEcatGrpDo
        {
            public Int16 groupNo;
            public Int16 value;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct TTaskSetEcatDoBitInverse
        {
            public Int16 doIndex;
            public Int16 value;
            public Int16 inverseTime;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct TTaskSetEcatDaVal
        {
            public Int16 daIndex;
            public Int16 value;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct TTaskStopMove
        {
            public Int16 axNo;
            public Int16 stopType;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct TTaskPulseDo
        {
            public Int16 taskPulseDoIndex;
            public Int16 firstLevel;
            public Int32 highLevelTime;
            public Int32 lowLevelTime;
            public Int16 pulseNum;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct UTask
        {
            public TTaskCrdStart taskCrdStart;
            public TTaskStartPtp taskStartPtp;
            public TTaskStartJog taskStartJog;
            public TTaskStartGear taskStartGear;
            public TTaskStartPt taskStartPt;
            public TTaskStartPvt taskStartPvt;
            public TTaskUpdatePtpMvPara taskUpdatePtpMvPara;
            public TTaskUpdateJogMvPara taskUpdateJogMvPara;
            public TTaskSetEcatDoBit taskSetEcatDoBit;
            public TTaskSetEcatGrpDo taskSetEcatGrpDo;
            public TTaskSetEcatDoBitInverse taskSetEcatDoBitInverse;
            public TTaskSetEcatDaVal taskSetEcatDaVal;
            public TTaskUpdatePtpTgtPos taskUpdatePtpTgtPos;
            public TTaskPulseDo taskPulseDo;
            public TTaskStopMove taskStopMove;
        };
        #endregion

        /// @defgroup EcatDef EtherCAT相关宏定义
        /// @brief EtherCAT相关宏定义
        #region

        /// @defgroup MasterSts 主站状态码
        /// @brief 板卡ECAT主站状态定义
        #region
        public const uint EC_MASTER_IDLE = (0); ///< EtherCat主站尚未初始化
        public const uint EC_MASTER_INIT = (1); ///< EtherCat主站初始化
        public const uint EC_MASTER_SCAN_SLAVE = (2); ///< EtherCat主站正在扫描从站设备
        public const uint EC_MASTER_SCAN_SLAVE_END = (3); ///< EtherCat主站扫描从站设备结束
        public const uint EC_MASTER_SCAN_MODULES = (4); ///< EtherCat主站正在扫描从站设备MODULES
        public const uint EC_MASTER_SCAN_MODULES_END = (5); ///< EtherCat主站扫描从站设备MODULES结束
        public const uint EC_MASTER_OP = (6); ///< EtherCat主站进入OP状态
        public const uint EC_MASTER_ERR = (7); ///< EtherCat主站链路状态有错误
        #endregion

        /// @defgroup SlaveSts 从站状态码
        /// @brief 板卡EtherCAT从站状态定义
        #region
        public const uint EC_SLAVE_STATE_UNKNOWN = (0x00); ///< EtherCat从站在未知状态
        public const uint EC_SLAVE_STATE_INIT = (0x01); ///< EtherCat从站在初始状态
        public const uint EC_SLAVE_STATE_PREOP = (0x02); ///< EtherCat从站在PREOP状态
        public const uint EC_SLAVE_STATE_BOOT = (0x03); ///< EtherCat从站在BOOT状态
        public const uint EC_SLAVE_STATE_SAFEOP = (0x04); ///< EtherCat从站在SAVEOP状态
        public const uint EC_SLAVE_STATE_OP = (0x08); ///< EtherCat从站在OP状态
        public const uint EC_SLAVE_STATE_ACK_ERR = (0x10); ///< EtherCat从站有错误
        #endregion

        /// @defgroup EcatError 协议栈错误码
        /// @brief 协议栈错误码EcatErrorCode定义
        #region
        public const uint ERROR_CODE_NO_SUCH_SLAVE = (0x0003); ///< 配置阶段, 没有这个从站, 处理方法: 检查从站是否掉线, 重新扫描配置
        public const uint ERROR_CODE_INVALID_PDO = (0x0004); ///< 配置阶段, 非法PDO, 处理方法: 检查从站厂家是否更新xml配置文件, 与汇川默认配置不一致
        public const uint ERROR_CODE_INVALID_SDO = (0x0005); ///< 配置阶段, 非法SDO, 处理方法: 检查从站厂家是否更新xml配置文件, 与汇川默认配置不一致
        public const uint ERROR_CODE_INVALID_ENTRY = (0x0006); ///< 配置阶段, 非法ENTRY, 处理方法: 检查从站厂家是否更新xml配置文件, 与汇川默认配置不一致
        public const uint ERROR_CODE_PASECFGFAIL = (0x000D); ///< 解析XML设备配置失败, 处理方法: 设备配置文件已经被破坏, 重新扫描配置
        public const uint ERROR_CODE_CFGREGISTFAIL = (0x0015); ///< 配置reg失败, 处理方法: 检查从站厂家是否更新xml配置文件, 与汇川默认配置不一致
        public const uint ERROR_CODE_CFGDIFFONLINE = (0x0016); ///< 在线从站与配置不一致, 处理方法: 检查从站是否掉线, 重新扫描配置
        public const uint ERROR_CODE_AXIS_NUM_BEYOND = (0x0017); ///< 轴配置数量超过板卡最大支持轴数;
        public const uint ERROR_CODE_SLAVE_OFFLINE = (0x001a); ///< 从站掉线错误, 高8位为从站号, 即bit[15:8]-轴号, 处理方法: 检查从高8位数字开始从站是否掉线, 或者重新扫描配置
        public const uint ERROR_CODE_SDOBF_NONECAT = (0x001b); ///< SDO缓冲区收到非ECAT帧错误, 处理方法: 检查线路是否接错, 是否有其他非Ethercat设备接入
        public const uint ERROR_CODE_PORT0_NOTLINK = (0x001c); ///< 端口未接ECAT设备错误, 处理方法: 检查板卡端线路是否正常接线, 是否接线不可靠
        public const uint ERROR_CODE_SET_CYCLETIME_PARA_ERR = (0x001e); ///< 设置周期时间参数错误, 处理方法: 错误的DC时钟配置, 只支持125us, 250us, 500us, 125us, 1000us, 2000us, 4000us, 8000us周期
        public const uint ERROR_CODE_COE_SDO_INIT_ERR = (0x001f); ///< 在初始化阶段coe配置错误, 处理方法: 检查从站厂家是否更新xml配置文件, 与汇川默认配置不一致; 或者可能例如RTU模块配置错误
        public const uint ERROR_CODE_SLAVE_STATE_ERR = (0x0020); ///< 从站状态错误, 高8位为轴号, 即bit[15:8]-轴号, 处理方法: 检查从高8位数字从站状态, 检查从站设备异常原因
        public const uint ERROR_CODE_SLAVE_SII_ERR = (0x0037); ///< E2ROM 信息有误, 处理方法: 一般为从站保存的e2rom信息有误, 可以使用twincat确认并联系厂家
        public const uint ERROR_CODE_SLAVE_NUM_BEYOND = (0x003b); ///< 从站在线超过最大64个站
        #endregion

        /// @defgroup AbortCode SDO访问舍弃码
        /// @brief 以下宏定义针对IIMC_GetEcatSdo, IMC_SetEcatSdo函数
        #region
        public const uint ABORT_CODE1 = (0x05030000); ///< 从站Toggle bit 没有变化
        public const uint ABORT_CODE2 = (0x05040000); ///< SDO 访问超时
        public const uint ABORT_CODE3 = (0x05040001); ///< 客户端/服务器 命令非法或未知
        public const uint ABORT_CODE4 = (0x05040005); ///< 内存溢出
        public const uint ABORT_CODE5 = (0x06010000); ///< 不支持访问该对象
        public const uint ABORT_CODE6 = (0x06010001); ///< 尝试去读一个只写对象
        public const uint ABORT_CODE7 = (0x06010002); ///< 尝试去写一个只读对象
        public const uint ABORT_CODE8 = (0x06020000); ///< 对象字典中不存在该对象
        public const uint ABORT_CODE9 = (0x06040041); ///< 该对象不能映射成PDO
        public const uint ABORT_CODE10 = (0x06040042); ///< 对象映射成PDO超出 PDO长度
        public const uint ABORT_CODE11 = (0x06040043); ///< 通用参数非法
        public const uint ABORT_CODE12 = (0x06040047); ///< 设备内不兼容
        public const uint ABORT_CODE13 = (0x06060000); ///< 由于硬件原因访问失败
        public const uint ABORT_CODE14 = (0x06070010); ///< 数据类型不匹配, 长度参数
        public const uint ABORT_CODE15 = (0x06070012); ///< 数据类型不匹配, 长度太大
        public const uint ABORT_CODE16 = (0x06070013); ///< 数据类型不匹配, 长度太小
        public const uint ABORT_CODE17 = (0x06090011); ///< 该对象子索引不存在
        public const uint ABORT_CODE18 = (0x06090030); ///< 参数超出范围
        public const uint ABORT_CODE19 = (0x06090031); ///< 参数超出范围太大
        public const uint ABORT_CODE20 = (0x06090032); ///< 参数超出范围太小
        public const uint ABORT_CODE21 = (0x06090036); ///< 最大值小于最小值
        public const uint ABORT_CODE22 = (0x08000000); ///< 一般错误
        public const uint ABORT_CODE23 = (0x08000020); ///< 数据不能被传输或被保存
        public const uint ABORT_CODE24 = (0x08000021); ///< 数据不能被传输或被保存由于本地控制
        public const uint ABORT_CODE25 = (0x08000022); ///< 数据不能被传输或被保存由于当前状态
        public const uint ABORT_CODE26 = (0x08000023); ///< 缺乏对象字典或者对象字典创建失败
        #endregion

        /// @defgroup ServoOpMode 伺服操作模式定义
        /// @brief 定义了伺服402协议中的控制模式
        #region
        public const Int16 TQ_OP_MODE = (4);  ///< TQ模式
        public const Int16 HM_OP_MODE = (6);  ///< 回零模式
        public const Int16 CSP_OP_MODE = (8);  ///< CSP模式
        public const Int16 CSV_OP_MODE = (9);  ///< CSV模式
        public const Int16 CST_OP_MODE = (10); ///< CST模式
        #endregion

        /// @defgroup EcatHomingMethod 402回零方法定义
        /// @brief 定义了伺服402回零模式的回零方法类型
        #region
        public const uint HOME_NLIMT_ZINDEX = (1);  ///< 负限位+Z信号
        public const uint HOME_PLIMT_ZINDEX = (2);  ///< 正限位+Z信号
        public const uint HOME_PHOME_FEDGE_ZINDEX = (3);  ///< 正原点开关下降沿+Z信号
        public const uint HOME_PHOME_REDGE_ZINDEX = (4);  ///< 正原点开关上升沿+Z信号
        public const uint HOME_NHOME_FEDGE_ZINDEX = (5);  ///< 负原点开关下降沿+Z信号
        public const uint HOME_NHOME_REDGE_ZINDEX = (6);  ///< 负原点开关上升沿+Z信号
        public const uint HOME_PLIMT_PHOME_FEDGE_ZINDEX = (7);  ///< 正限位+正原点开关下降沿+Z信号
        public const uint HOME_PLIMT_PHOME_REDGE_ZINDEX = (8);  ///< 正限位+正原点开关上升沿+Z信号
        public const uint HOME_PLIMT_NHOME_REDGE_ZINDEX = (9);  ///< 正限位+负原点开关上升沿+Z信号
        public const uint HOME_PLIMT_NHOME_FEDGE_ZINDEX = (10); ///< 正限位+负原点开关下降沿+Z信号
        public const uint HOME_NLIMT_NHOME_FEDGE_ZINDEX = (11); ///< 负限位+负原点开关下降沿+Z信号
        public const uint HOME_NLIMT_NHOME_REDGE_ZINDEX = (12); ///< 负限位+负原点开关上升沿+Z信号
        public const uint HOME_NLIMT_PHOME_REDGE_ZINDEX = (13); ///< 负限位+正原点开关上升沿+Z信号
        public const uint HOME_NLIMT_PHOME_FEDGE_ZINDEX = (14); ///< 负限位+正原点开关下降沿+Z信号
        public const uint HOME_NLIMT = (17); ///< 负限位
        public const uint HOME_PLIMT = (18); ///< 正限位
        public const uint HOME_PHOME_FEDGE = (19); ///< 正原点开关下降沿
        public const uint HOME_PHOME_REDGE = (20); ///< 正原点开关上升沿
        public const uint HOME_NHOME_FEDGE = (21); ///< 负原点开关下降沿
        public const uint HOME_NHOME_REDGE = (22); ///< 负原点开关上升沿
        public const uint HOME_PLIMT_PHOME_FEDGE = (23); ///< 正限位+正原点开关下降沿
        public const uint HOME_PLIMT_PHOME_REDGE = (24); ///< 正限位+正原点开关上升沿
        public const uint HOME_PLIMT_NHOME_REDGE = (25); ///< 正限位+负原点开关上升沿
        public const uint HOME_PLIMT_NHOME_FEDGE = (26); ///< 正限位+负原点开关下降沿
        public const uint HOME_NLIMT_NHOME_FEDGE = (27); ///< 负限位+负原点开关下降沿
        public const uint HOME_NLIMT_NHOME_REDGE = (28); ///< 负限位+负原点开关上升沿
        public const uint HOME_NLIMT_PHOME_REDGE = (29); ///< 负限位+正原点开关上升沿
        public const uint HOME_NLIMT_PHOME_FEDGE = (30); ///< 负限位+正原点开关下降沿
        public const uint HOME_NEGZINDEX = (33); ///< 负向Z信号
        public const uint HOME_POSZINDEX = (34); ///< 正向Z信号
        public const uint HOME_CURRENT_POS = (35); ///< 当前位置
        #endregion

        /// @defgroup EcatHomingSts 402回零状态定义
        /// @brief 定义了伺服402回零模式下的工作状态
        #region
        public const Int16 HOME_IN_PROGRESS = (0); ///< 正在回零中
        public const Int16 HOME_INTERRUPTED_OR_NOT_START = (1); ///< 回零中断或者没有开始启动
        public const Int16 HOME_ATTAINED_BUT_NOT_REACH = (2); ///< 回零结束, 但没有到设定的目标位置
        public const Int16 HOME_SUCESS = (3); ///< 回零成功
        public const Int16 HOME_ERR_VEL_NOT_ZERO = (4); ///< 回零中发生错误, 同时速度不为0
        public const Int16 HOME_ERR_VEL_ZERO = (5); ///< 回零中发生错误, 同时速度为0
        #endregion

        /// @defgroup CSPHomingMethod CSP回零方法定义
        /// @brief 定义了板卡CSP回零模式的回零方法
        #region
        public const Int32 HOMING_ECAT_CSP_METHOD_NONE = (-1); ///< 非回零模式
        public const Int32 HOMING_ECAT_CSP_METHOD_DI = (0);  ///< DI回零方式
        public const Int32 HOMING_ECAT_CSP_METHOD_INDEX = (1);  ///< Index回零方式
        public const Int32 HOMING_ECAT_CSP_METHOD_LIMIT_INDEX = (2);  ///< 限位+Index回零方式
        public const Int32 HOMING_ECAT_CSP_METHOD_LIMIT_DI = (3);  ///< 限位+Di回零方式
        #endregion

        /// @defgroup CSPHomingSts CSP回零状态定义
        /// @brief 定义了板卡CSP回零模式下的工作状态
        #region
        public const Int16 HOME_CSP_STS_RUNING = (0x01); ///< CSP回零中
        public const Int16 HOME_CSP_STS_STOPPING = (0x02); ///< CSP回零停止中
        public const Int16 HOME_CSP_STS_STOPPED = (0x03); ///< CSP回零已停止
        public const Int16 HOME_CSP_STS_FINISH = (0x04); ///< CSP回零已完成
        public const Int16 HOME_CSP_STS_ERROR = (0x05); ///< CSP回零错误
        #endregion
        #endregion

        /// @defgroup CardSysDef 板卡系统相关宏定义
        #region

        /// @defgroup CardResType 资源类型宏定义
        /// @brief 以下宏定义针对IMC_GetResCount函数
        #region
        public const uint MC_ECAT_DO = (0);  ///< ecat的通用do
        public const uint MC_LOCAL_DO = (1);  ///< localBus的通用do
        public const uint MC_ECAT_DI = (11); ///< ecat的通用DI
        public const uint MC_ECAT_AD = (12); ///< ecat的通用AD
        public const uint MC_ECAT_DA = (13); ///< ecat的通用DA
        public const uint MC_ECAT_AXIS = (15); ///< ecat的通用AXIS
        public const uint MC_ECAT_REG_IN = (16); ///< ecat的通用RegIn
        public const uint MC_ECAT_REG_OUT = (17); ///< ecat的通用RegOut
        public const uint MC_ECAT_ENC = (18); ///< ecat的通用RegOut

        public const uint MC_AXIS = (30); ///< 板卡最大轴数
        public const uint MC_PROFILE = (31); ///< 板卡规划轴数
        public const uint MC_CRD_MAX_CNT = (60); ///< 坐标系最大个数
        public const uint MC_CRD_BUF_LEN = (61); ///< 坐标系缓冲区长度
        #endregion

        /// @defgroup CardVersionType 板卡系统版本类型定义
        /// @brief 以下宏定义针对IMC_GetImcCardVersion函数
        #region
        public const uint SOFT_VERSION = (0);       // 总软件版本
        public const uint DSP_VERSION = (1);        // HAL2软件版本
        public const uint ARM_VERSION = (2);        // ARM软件版本
        public const uint API_VERSION = (3);        // API软件版本
        public const uint FPGA_VERSION = (4);       // FPGA固件版本
        public const uint DSP2_VERSION = (5);       // HAL3固件版本
        public const uint LOCAL_VERSION = (6);      // 端子板固件版本
        public const uint OS_ARM_VERSION = (11);    // ARM系统版本
        public const uint LIB_DSP_VERSION = (12);   // LibInfo软件版本
        public const uint LIB_ECAT_BOARD_APP_VERSION = (20);    // 协议栈软件版本1
        public const uint LIB_ECAT_APP_VERSION = (21);          // 协议栈软件版本2
        public const uint LIB_ECAT_VERSION = (22);              // 协议栈软件版本3
        public const uint LIB_ECAT_PARSER_ENI_VERSION = (23);   // 协议栈软件版本4
        #endregion
        #endregion


        /// @defgroup AxDiStopType 轴DI停止类型定义
        /// @brief 以下宏定义针对IMC_SetAxStopTrigPara、IMC_GetAxStopTrigPara函数
        #region
        public const uint CNST_DI_STOP_TYPE_ECATDI = (0); ///< EcatDI停止类型
        public const uint CNST_DI_STOP_TYPE_PROBLE1_RF = (1); ///< 探针1上升沿或下降沿停止
        public const uint CNST_DI_STOP_TYPE_PROBLE1_R = (2); ///< 探针1上升沿停止
        public const uint CNST_DI_STOP_TYPE_PROBLE1_F = (3); ///< 探针1下降沿停止
        #endregion

        /// @defgroup AxStsBit 轴状态位定义
        /// @brief 以下宏定义针对IMC_GetAxSts函数
        #region
        public const uint AX_ALARM_BIT = (0x00000001); ///< 轴驱动报警
        public const uint AX_SVON_BIT = (0x00000002); ///< 伺服使能
        public const uint AX_BUSY_BIT = (0x00000004); ///< 轴忙状态
        public const uint AX_ARRIVE_BIT = (0x00000008); ///< 轴到位状态
        public const uint AX_POSLMT_BIT = (0x00000010); ///< 正硬限位报警
        public const uint AX_NEGLMT_BIT = (0x00000020); ///< 负硬限位报警
        public const uint AX_SOFT_POSLMT_BIT = (0x00000040); ///< 正软限位报警
        public const uint AX_SOFT_NEGLMT_BIT = (0x00000080); ///< 负软限位报警
        public const uint AX_ERRPOS_BIT = (0x00000100); ///< 轴位置误差越限标志
        public const uint AX_EMG_STOP_BIT = (0x00000200); ///< 运动急停标志
        public const uint AX_ECAT_BIT = (0x00000400); ///< 总线轴标志
        public const uint AX_SW_ABNOR_BIT = (0x00000800); ///< 轴异常报警=(龙门);
        public const uint AX_WARING_BIT = (0x00001000); ///< 轴警告
        public const uint AX_HM_BIT = (0x00002000); ///< 原点信号状态
        public const uint AX_UNLINK_BIT = (0x00004000); ///< 轴掉线状态
        public const uint AX_ECAT_TGTREACH_BIT = (0x00008000); ///< 状态字里的到位状态
        #endregion

        /******************采集数据类型 定义***************************/
        // 单轴数据类型
        public const Int16 SAMPLE_ADDRESS_TYPE_AX_PRF_POS = (0x01); ///< 轴规划位置
        public const Int16 SAMPLE_ADDRESS_TYPE_AX_ENC_POS = (0x02); ///< 轴编码器位置
        public const Int16 SAMPLE_ADDRESS_TYPE_AX_PRF_VEL = (0x03); ///< 轴规划速度
        public const Int16 SAMPLE_ADDRESS_TYPE_AX_ENC_VEL = (0x04); ///< 轴编码器速度
        public const Int16 SAMPLE_ADDRESS_TYPE_AX_PRF_ACC = (0x05); ///< 轴规划加速度
        public const Int16 SAMPLE_ADDRESS_TYPE_AX_ENC_ACC = (0x06); ///< 轴编码器加速度
        public const Int16 SAMPLE_ADDRESS_TYPE_PRF_POS = (0x07); ///< 规划位置
        public const Int16 SAMPLE_ADDRESS_TYPE_PRF_CMPPOS = (0x08); ///< 比较位置
        public const Int16 SAMPLE_ADDRESS_TYPE_AX_TORQ = (0x0a); ///< 轴扭矩
        public const Int16 SAMPLE_ADDRESS_TYPE_AX_STS = (0x0b); ///< 轴状态

        // 资源信号数据类型
        public const Int16 SAMPLE_ADDRESS_TYPE_ECAT_DI = (0x30); ///< ECAT通用DI
        public const Int16 SAMPLE_ADDRESS_TYPE_ECAT_DO = (0x31); ///< ECAT通用DO
        public const Int16 SAMPLE_ADDRESS_TYPE_LOCAL_DI = (0x32); ///< 端子板DI
        public const Int16 SAMPLE_ADDRESS_TYPE_LOCAL_DO = (0x33); ///< 端子板DO
        public const Int16 SAMPLE_ADDRESS_TYPE_ECAT_AD = (0x34); ///< ECAT AD
        public const Int16 SAMPLE_ADDRESS_TYPE_ECAT_DA = (0x35); ///< ECAT DA

        // 总线对象字典采集
        public const Int16 SAMPLE_ADDRESS_TYPE_RXPDO_2 = (0x40); ///< pdo通用, 2字节, 写
        public const Int16 SAMPLE_ADDRESS_TYPE_RXPDO_4 = (0x41); ///< pdo通用, 4字节, 写
        public const Int16 SAMPLE_ADDRESS_TYPE_6040 = (0x42); ///< 控制字
        public const Int16 SAMPLE_ADDRESS_TYPE_607a = (0x43); ///< 目标位置
        public const Int16 SAMPLE_ADDRESS_TYPE_60ff = (0x44); ///< 目标速度
        public const Int16 SAMPLE_ADDRESS_TYPE_6071 = (0x45); ///< 目标力矩
        public const Int16 SAMPLE_ADDRESS_TYPE_6060 = (0x46); ///< 模式控制
        public const Int16 SAMPLE_ADDRESS_TYPE_60fe = (0x47); ///< 数字量输出
        public const Int16 SAMPLE_ADDRESS_TYPE_60b8 = (0x48); ///< 探针控制字

        public const Int16 SAMPLE_ADDRESS_TYPE_TXPDO_2 = (0x50); ///< pdo通用, 2字节, 读
        public const Int16 SAMPLE_ADDRESS_TYPE_TXPDO_4 = (0x51); ///< pdo通用, 4字节, 读
        public const Int16 SAMPLE_ADDRESS_TYPE_6041 = (0x52); ///< 状态字
        public const Int16 SAMPLE_ADDRESS_TYPE_6064 = (0x53); ///< 实际位置
        public const Int16 SAMPLE_ADDRESS_TYPE_606c = (0x54); ///< 实际速度
        public const Int16 SAMPLE_ADDRESS_TYPE_6077 = (0x55); ///< 实际力矩
        public const Int16 SAMPLE_ADDRESS_TYPE_60f4 = (0x56); ///< 跟随误差
        public const Int16 SAMPLE_ADDRESS_TYPE_603f = (0x57); ///< 错误码
        public const Int16 SAMPLE_ADDRESS_TYPE_60fd = (0x58); ///< 数字量输入
        public const Int16 SAMPLE_ADDRESS_TYPE_60b9 = (0x59); ///< 探针状态字
        public const Int16 SAMPLE_ADDRESS_TYPE_60ba = (0x60); ///< 探针位置1
        public const Int16 SAMPLE_ADDRESS_TYPE_60bb = (0x61); ///< 探针位置2

        // 插补数据类型
        public const Int16 SAMPLE_ADDRESS_TYPE_CRD_POSX = (0x100); ///< 插补坐标系, X轴位置
        public const Int16 SAMPLE_ADDRESS_TYPE_CRD_POSY = (0x101); ///< 插补坐标系, Y轴位置
        public const Int16 SAMPLE_ADDRESS_TYPE_CRD_POSZ = (0x102); ///< 插补坐标系, Z轴位置
        public const Int16 SAMPLE_ADDRESS_TYPE_CRD_VEL = (0x110); ///< 插补坐标系, 合成速度

        // 采集触发类型
        public const Int16 SAMPLE_TRIG_IMMEDIATE = (0); ///< 立即采集
        public const Int16 SAMPLE_TRIG_DELAY = (1); ///< 延时采集
        public const Int16 SAMPLE_TRIG_LOCAL_DI = (2); ///< 本地DI触发
        public const Int16 SAMPLE_TRIG_ECAT_DI = (3); ///< ECAT 的DI 触发


        //事件类型定义
        public const Int16 EVENT_TYPE_PRFPOS = (1);
        public const Int16 EVENT_TYPE_PRFVEL = (2);
        public const Int16 EVENT_TYPE_ENCPOS = (3);
        public const Int16 EVENT_TYPE_ENCVEL = (4);
        public const Int16 EVENT_TYPE_CRDPOS_X = (5);
        public const Int16 EVENT_TYPE_CRDPOS_Y = (6);
        public const Int16 EVENT_TYPE_CRDPOS_Z = (7);
        public const Int16 EVENT_TYPE_CRDVEL = (8);
        public const Int16 EVENT_TYPE_AXSTS_ALARM = (9);
        public const Int16 EVENT_TYPE_AXSTS_SVON = (10);
        public const Int16 EVENT_TYPE_AXSTS_BUSY = (11);
        public const Int16 EVENT_TYPE_AXSTS_ARRIVE = (12);
        public const Int16 EVENT_TYPE_AXSTS_POSLMT = (13);
        public const Int16 EVENT_TYPE_AXSTS_NEGLMT = (14);
        public const Int16 EVENT_TYPE_AXSTS_SOFT_POSLMT = (15);
        public const Int16 EVENT_TYPE_AXSTS_SOFT_NEGLMT = (16);
        public const Int16 EVENT_TYPE_AXSTS_ERRPOS = (17);
        public const Int16 EVENT_TYPE_AXSTS_EMGSTOP = (18);
        public const Int16 EVENT_TYPE_AXSTS_ECAT = (19);
        public const Int16 EVENT_TYPE_AXSTS_SW_ABNOR = (20);
        public const Int16 EVENT_TYPE_AXSTS_WARING = (21);
        public const Int16 EVENT_TYPE_AXSTS_HM = (22);
        public const Int16 EVENT_TYPE_AXSTS_UNLINK = (23);
        public const Int16 EVENT_TYPE_AXSTS_TGTREACH = (24);
        public const Int16 EVENT_TYPE_CRDSTS = (25);
        public const Int16 EVENT_TYPE_CRDID = (26);
        public const Int16 EVENT_TYPE_DI = (27);
        public const Int16 EVENT_TYPE_AD = (28);
        public const Int16 EVENT_TYPE_GLOBALVAL = (29);

        //事件条件定义
        public const Int16 EVENT_CONDITION_EQ = (1);   ///< 变量值等于设定值
        public const Int16 EVENT_CONDITION_NE = (2);   ///< 变量值不等于设定值
        public const Int16 EVENT_CONDITION_GT = (3);   ///< 变量值大于设定值
        public const Int16 EVENT_CONDITION_LT = (4);   ///< 变量值小于设定值
        public const Int16 EVENT_CONDITION_CHANGE_TO = (5);   ///< 变量值改变为设定值
        public const Int16 EVENT_CONDITION_CHANGE = (6);   ///< 变量值改变
        public const Int16 EVENT_CONDITION_UP = (7);   ///< 变量值增大
        public const Int16 EVENT_CONDITION_DOWN = (8);  ///< 变量值减小
        public const Int16 EVENT_CONDITION_REMAIN_AT = (9);  ///< 变量值保持为设定值
        public const Int16 EVENT_CONDITION_REMAIN = (10);  ///< 变量值保持不变
        public const Int16 EVENT_CONDITION_CROSS_POSITIVE = (11);  ///< 变量值正向穿越设定值
        public const Int16 EVENT_CONDITION_CROSS_NEGATIVE = (12);  ///< 变量值负向穿越设定值

        //事件功能中任务类型定义
        public const Int16 TASK_TYPE_CRDSTART = (1);
        public const Int16 TASK_TYPE_STARTPTPMOVE = (2);
        public const Int16 TASK_TYPE_STARTJOGMOVE = (3);
        public const Int16 TASK_TYPE_STARTGEARMOVE = (4);
        public const Int16 TASK_TYPE_STARTPTMOVE = (5);
        public const Int16 TASK_TYPE_STARTPVTMOVE = (6);
        public const Int16 TASK_TYPE_UPDATEPTPMOVEPARA = (7);
        public const Int16 TASK_TYPE_UPDATEJOGMOVEPARA = (8);
        public const Int16 TASK_TYPE_SETECATDOBIT = (9);
        public const Int16 TASK_TYPE_SETECATGRPDO = (10);
        public const Int16 TASK_TYPE_SETECATDOBITINVERSE = (11);
        public const Int16 TASK_TYPE_SETECATDAVAL = (12);
        public const Int16 TASK_TYPE_UPDATEPTPTGTPOS = (13);
        public const Int16 TASK_TYPE_PLUSEDO = (14);
        public const Int16 TASK_TYPE_STOPMOVE = (15);



        ///@defgroup CardBasic CardBasic
        ///@brief 板卡基本操作
        #region CardBasic

        ///@defgroup CardOpen CardOpen
        ///@brief 板卡开启与关闭
        #region CardOpen

        /// @brief  获取板卡数量
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetCardsNum", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetCardsNum(ref Int16 pCardsNum, Int16[] pCardIndexArray);

        /// @brief  按卡号开启控制卡
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_OpenCard", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_OpenCard(Int16 cardIndex);

        /// @brief  按卡号关闭控制卡
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CloseCard", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CloseCard(Int16 cardIndex);
        #endregion

        ///@defgroup CardConfigFile CardConfigFile
        ///@brief 板卡配置文件操作
        #region CardConfigFile

        /// @brief  上传控制卡的设备配置文件(包括主站使能, 规划周期, EtherCAT对象以及偏置等), 上传文件的路径在设定目录下
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_UpLoadDeviceConfig", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_UpLoadDeviceConfig(Int16 cardIndex, string pathName);

        /// @brief  下载控制器的设备配置文件(包括主站使能, 规划周期, EtherCAT对象以及偏置等), 下载文件的路径在设定目录下
        /// @attention 当次下载下去之后, 非立即生效, 需要调用扫描板卡指令才能生效
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_DownLoadDeviceConfig", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_DownLoadDeviceConfig(Int16 cardIndex, string pathName);

        /// @brief  下载控制器的系统配置文件(包括轴映射、轴运动参数、IO取反滤波等硬件信号配置参数)
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_DownLoadSystemConfig", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_DownLoadSystemConfig(Int16 cardIndex, string pathName);

        #endregion
        #endregion

        ///@defgroup CardSysConfig CardSysConfig
        ///@brief 板卡系统配置相关函数
        #region CardSysConfig


        ///@defgroup EmgConfig EmgConfig
        ///@brief 急停参数配置函数
        #region EmgConfig

        /// @brief  设置停止信号的滤波系数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetEmgFilter", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetEmgFilter(Int16 cardIndex, Int16 filter);

        /// @brief  获取停止信号的滤波系数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEmgFilter", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEmgFilter(Int16 cardIndex, ref Int16 pFilter);

        /// @brief  设置急停信号触发的电平取反
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetEmgTrigLevelInv", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetEmgTrigLevelInv(Int16 cardIndex, Int16 inverse);

        /// @brief  获取急停信号触发的电平取反
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEmgTrigLevelInv", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEmgTrigLevelInv(Int16 cardIndex, ref Int16 pInverse);

        /// @brief  获取急停状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEmgSts", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEmgSts(Int16 cardIndex, ref Int16 pSts);

        /// @brief  设置急停时是否复位所有 DO 输出的开关使能
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetEmgDoResetFlag", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetEmgDoResetFlag(Int16 cardIndex, Int16 enable);

        /// @brief  读取急停时是否复位所有 DO 输出的开关使能
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEmgDoResetFlag", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEmgDoResetFlag(Int16 cardIndex, ref Int16 pEnable);

        /// @brief  设置是否忽略板卡的ECAT连接状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetEcatLinkStsIgnore", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetEcatLinkStsIgnore(Int16 cardIndex, Int16 flag);
        #endregion

        ///@defgroup Watchdog Watchdog
        ///@brief 看门狗参数配置函数
        #region Watchdog

        /// @brief  设置看门狗, 在报警时, 进行急停动作。主要用于检查应用层软件是否卡死。
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_OpenWatchDog", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_OpenWatchDog(Int16 cardIndex, Int32 feedTime);

        /// @brief  喂看门狗操作
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_FeedWatchDog", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_FeedWatchDog(Int16 cardIndex);

        /// @brief  关闭看门狗检查
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CloseWatchDog", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CloseWatchDog(Int16 cardIndex);
        #endregion


        ///@defgroup CardTimeInfo CardTimeInfo
        ///@brief 板卡内部时钟信息
        #region CardTimeInfo


        /// @brief  获取板卡的CPU负载
        /// @warning 建议负载率不要超过60%
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetCalcLoadRatio", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetCalcLoadRatio(Int16 cardIndex, ref double pLoadRatio);

        /// @brief  根据Type类型, 获取板卡对应的固件版本
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetVersion", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetVersion(Int16 cardIndex, Int16 type, Int16[] pVersion);

        #endregion

        ///@defgroup CardMasterInfo CardMasterInfo
        ///@brief 板卡主站配置信息
        #region CardMasterInfo

        /// @brief  获取主站启用配置, 从xml配置文件获取
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetMasterCfgXml", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetMasterCfgXml(Int16 cardIndex, ref Int16 pHwCfg);

        /// @brief  获取板卡的网口硬件连接状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetHwPortLinkSts", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetHwPortLinkSts(Int16 cardIndex, ref Int16 pLinkSts);

        /// @brief  获取板卡的网口硬件连接状态，Ex指令
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetHwPortLinkStsEx", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetHwPortLinkStsEx(Int16 cardIndex, ref Int16 pLinkSts);


        #endregion

        ///@defgroup CardReset CardReset
        ///@brief 板卡复位功能
        #region CardReset


        /// @brief  复位控制卡系统参数
        /// @details 控制卡规划层软件资源复位
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_ResetSysPara", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_ResetSysPara(Int16 cardIndex);
        #endregion

        ///@defgroup UserCode UserCode
        ///@brief 用户配置
        #region UserCode

        /// @brief  设置用户密码
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_WriteUserCode", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_WriteUserCode(Int16 cardIndex, Byte[] pCode, Int16 len);

        /// @brief  检验设置的用户密码
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CheckUserCode", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CheckUserCode(Int16 cardIndex, Byte[] pCode, Int16 len);

        /// @brief  写入用户数据
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_WriteUserData", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_WriteUserData(Int16 cardIndex, Int16 offset, Byte[] pData, Int16 len);

        /// @brief  读取用户数据
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_ReadUserData", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_ReadUserData(Int16 cardIndex, Int16 offset, Byte[] pData, Int16 len);
        #endregion

        #endregion

        ///@defgroup EtherCAT EtherCAT
        ///@brief 板卡ECAT功能
        #region EtherCAT



        ///@defgroup EcatInit EcatInit
        ///@brief 板卡ECAT初始化功能
        #region EcatInit


        /// @brief  初始化板卡通讯, 主站进入OP状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_InitEcatComm", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_InitEcatComm(Int16 cardIndex);

        /// @brief  开启板卡通讯, 获取EtherCAT总线资源, 建立主站与各从站之间的通讯
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_StartEcatComm", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_StartEcatComm(Int16 cardIndex);

        /// @brief  关闭板卡通讯, 主站退出OP状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_DelEcatComm", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_DelEcatComm(Int16 cardIndex);

        /// @brief  初始化总线, 直到主站进入OP状态, 随后扫描EtherCAT总线资源, 并建立主站与各从站之间的通讯 (该函数会阻塞在EtherCAT的总线扫描同步阶段)
        /// @warning 该指令阻塞执行
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_ScanCardEcat", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_ScanCardEcat(Int16 cardIndex, Int16 waitTime = 40);

        #endregion

        ///@defgroup EcatInfo EcatInfo
        ///@brief 板卡ECAT主站信息获取函数
        #region EcatInfo


        /// @brief  获取EtherCAT主站的状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatMasterSts", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatMasterSts(Int16 cardIndex, ref UInt32 pStatus);

        /// @brief  获取EtherCAT协议栈通讯错误码
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatErrCode", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatErrCode(Int16 cardIndex, ref UInt32 pErrCode);

        /// @brief  获取EtherCAT主站的状态，Ex指令
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatMasterStsEx", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatMasterStsEx(Int16 cardIndex, ref UInt32 pStatus);

        /// @brief  获取EtherCAT协议栈通讯错误码，Ex指令
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatErrCodeEx", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatErrCodeEx(Int16 cardIndex, ref UInt32 pErrCode);

        /// @brief  获取EtherCAT从站当前通讯阶段INIT、PreOP、SafeOp、OP
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatSlaveSts", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatSlaveSts(Int16 cardIndex, Int16 slave, ref Int16 pCurStatus, ref Int16 pReqStatus, ref Int16 pErrCode);

        /// @brief  获取EtherCAT主站信息(包含通讯周期, 从站个数, 资源数量等)
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatMasterInfo", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatMasterInfo(Int16 cardIndex, ref TMasterInfo ptMasterInfo);

        /// @brief  获取EtherCAT从站信息(包含设备类型, 对应轴号, 从站别名等)
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatSlaveInfo", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatSlaveInfo(Int16 cardIndex, Int16 slave, ref TSlaveInfo ptSlaveInfo);

        /// @brief  获取从站连接状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatSlaveOpSts", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatSlaveOpSts(Int16 cardIndex, ref UInt16 pSts, UInt32[] pMask);

        #endregion

        ///@defgroup EcatPDO EcatPDO
        ///@brief 板卡ECAT主站PDO功能
        #region EcatPDO


        /// @brief  根据站号读取EtherCAT从站的PDO数据
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatSlavePdoData", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatSlavePdoData(Int16 cardIndex, Int16 slave, UInt16 index, UInt16 subIndex, Byte[] pData);

        /// @brief  根据站号写入EtherCAT从站的PDO数据, 仅支持非轴从站, 非DIO, AIO, RegInOut类型Pdo
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetEcatSlavePdoData", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetEcatSlavePdoData(Int16 cardIndex, Int16 slave, UInt16 index, UInt16 subIndex, Byte[] pData);

        /// @brief  根据轴号获取EtherCAT从站的PDO数据
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatAxPdoData", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatAxPdoData(Int16 cardIndex, Int16 axNo, UInt16 index, Byte[] pData);

        /// @brief  根据轴号写入EtherCAT从站的PDO数据
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetEcatAxPdoData", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetEcatAxPdoData(Int16 cardIndex, Int16 axNo, UInt16 index, Byte[] pData);

        /// @brief  根据索引和子索引读取从站的PdoEntry结构体信息
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatSlavePdoEntry", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatSlavePdoEntry(Int16 cardIndex, Int16 slave, UInt16 index, UInt16 subIndex, ref TPdoEntry ptPdoEntry);
        #endregion

        ///@defgroup EcatSDO EcatSDO
        ///@brief 板卡ECAT主站SDO功能
        #region EcatSDO

        /// @brief  根据站号获取EtherCAT从站的SDO数据
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatSlaveSdo", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatSlaveSdo(Int16 cardIndex, Int16 slave, UInt16 index, UInt16 subIndex, Byte[] pData, UInt32 dataSize, ref UInt32 pResultSize, ref UInt32 pAbortCode);

        /// @brief  根据站号写入EtherCAT从站的SDO数据
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetEcatSlaveSdo", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetEcatSlaveSdo(Int16 cardIndex, Int16 slave, UInt16 index, UInt16 subIndex, Byte[] pData, UInt32 dataSize, ref UInt32 pAbortCode);

        /// @brief  根据轴号获取EtherCAT从站的SDO数据
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatAxSdo", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatAxSdo(Int16 cardIndex, Int16 axNo, UInt16 index, UInt16 subIndex, Byte[] pData, UInt32 dataSize, ref UInt32 pResultSize, ref UInt32 pAbortCode);

        /// @brief  根据轴号写入EtherCAT从站的SDO数据
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetEcatAxSdo", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetEcatAxSdo(Int16 cardIndex, Int16 axNo, UInt16 index, UInt16 subIndex, Byte[] pData, UInt32 dataSize, ref UInt32 pAbortCode);
        #endregion

        ///@defgroup EcatAxisMap EcatAxisMap
        ///@brief 板卡ECAT主站轴映射功能
        #region EcatAxisMap

        /// @brief  根据轴号获取轴所在从站的站号以及Slot号
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatAxStation", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatAxStation(Int16 cardIndex, Int16 axNo, ref Int16 pSlave, ref Int16 pSlotIndex);

        /// @brief  根据从站号以及Slot号获取对应的轴通道号
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatSlaveAxChn", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatSlaveAxChn(Int16 cardIndex, Int16 slave, Int16 slotIndex, ref Int16 pAxChn);
        #endregion

        ///@defgroup EcatAlias EcatAlias
        ///@brief 板卡ECAT从站别名功能
        #region EcatAlias

        /// @brief  根据从站别名获取从站的SDO数据
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatAliasSdo", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatAliasSdo(Int16 cardIndex, UInt32 aliasNo, UInt16 index, UInt16 subIndex, Byte[] pData, UInt32 dataSize, ref UInt32 pResultSize, ref UInt32 pAbortCode);

        /// @brief  根据从站别名设定从站的SDO数据
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetEcatAliasSdo", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetEcatAliasSdo(Int16 cardIndex, UInt32 aliasNo, UInt16 index, UInt16 subIndex, Byte[] pData, UInt32 dataSize, ref UInt32 pAbortCode);

        /// @brief  设置EtherCAT的DO输出状态保持, 断线后复位总线保持上一次状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetEcatDoStsHold", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetEcatDoStsHold(Int16 cardIndex, Int16 isHold);
        #endregion

        /// @brief  获取EtherCAT的DI输入状态, 按16bits为一组进行获取
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatDi", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatDi(Int16 cardIndex, Int16 packIndex, UInt16[] pValue, Int16 packCnt);

        /// @brief  设置EtherCAT的DO输出状态, 按16bits为一组进行设置
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetEcatDo", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetEcatDo(Int16 cardIndex, Int16 packIndex, UInt16[] pValue, Int16 packCnt);

        /// @brief  获取EtherCAT的DO输出状态, 按16bits为一组进行获取
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatDo", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatDo(Int16 cardIndex, Int16 packIndex, UInt16[] pValue, Int16 packCnt);

        /// @brief  获取EtherCAT的DI输入状态, 按位进行获取
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatDiBit", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatDiBit(Int16 cardIndex, Int16 diIndex, ref Int16 pValue);

        /// @brief  获取EtherCAT的DO输出状态, 按位进行获取
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatDoBit", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatDoBit(Int16 cardIndex, Int16 doIndex, ref Int16 pValue);

        /// @brief  设置EtherCAT的DO输出状态, 按位进行设置
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetEcatDoBit", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetEcatDoBit(Int16 cardIndex, Int16 doIndex, Int16 value);

        /// @brief  设置EtherCAT的DO输出状态以及生效延时, 按位进行设置
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetEcatDoBitInverse", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetEcatDoBitInverse(Int16 cardIndex, Int16 doIndex, Int16 value, Int16 inverseTime);
        #endregion

        ///@defgroup EcatAD EcatAD
        ///@brief 板卡ECAT主站AD/DA/Reg32寄存器功能
        #region EcatAD

        /// @brief  根据通道号获取EtherCAT对应通道的AD值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatAdVal", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatAdVal(Int16 cardIndex, Int16 adIndex, ref Int16 pValue);

        /// @brief  根据通道号设置EtherCAT对应通道的DA值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetEcatDaVal", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetEcatDaVal(Int16 cardIndex, Int16 daIndex, Int16 Value);

        /// @brief  根据通道号获取EtherCAT对应通道的DA值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatDaVal", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatDaVal(Int16 cardIndex, Int16 daIndex, ref Int16 pValue);

        /// @brief  根据通道号获取EtherCAT对应通道的RegIn值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatRegInVal", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatRegInVal(Int16 cardIndex, Int16 regIndex, ref float pValue);

        /// @brief  根据通道号设置EtherCAT对应通道的RegOut值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetEcatRegOutVal", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetEcatRegOutVal(Int16 cardIndex, Int16 regIndex, float Value);

        /// @brief  根据通道号获取EtherCAT对应通道的RegOut值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatRegOutVal", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatRegOutVal(Int16 cardIndex, Int16 regIndex, ref float pValue);

        /// @brief  根据通道号获取EtherCAT对应通道的AD值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatAd", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatAd(Int16 cardIndex, Int16 adIndex, Int16[] pValueArray, Int16 count);

        /// @brief  根据通道号设置EtherCAT对应通道的DA值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetEcatDa", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetEcatDa(Int16 cardIndex, Int16 daIndex, Int16[] pValueArray, Int16 count);

        /// @brief  根据通道号获取EtherCAT对应通道的DA值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatDa", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatDa(Int16 cardIndex, Int16 daIndex, Int16[] pValueArray, Int16 count);

        /// @brief  根据通道号获取EtherCAT对应通道的RegIn值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatRegIn", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatRegIn(Int16 cardIndex, Int16 regIndex, float[] pValueArray, Int16 count);

        /// @brief  根据通道号设置EtherCAT对应通道的RegOut值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetEcatRegOut", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetEcatRegOut(Int16 cardIndex, Int16 regIndex, float[] pValueArray, Int16 count);

        /// @brief  根据通道号获取EtherCAT对应通道的RegOut值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatRegOut", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatRegOut(Int16 cardIndex, Int16 regIndex, float[] pValueArray, Int16 count);
        #endregion

        ///@defgroup EcatEnc EcatEnc
        ///@brief 板卡ECAT主站Enc资源操作
        #region EcatEnc

        /// @brief  设置Ecat编码器计数方向
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetEcatEncDir", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetEcatEncDir(Int16 cardIndex, Int16 encIndex, Int16[] pDirArray, Int16 count = 1);

        /// @brief  读取Ecat编码器计数方向
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatEncDir", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatEncDir(Int16 cardIndex, Int16 encIndex, Int16[] pDirArray, Int16 count = 1);

        /// @brief  设置Ecat编码器值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetEcatEncPos", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetEcatEncPos(Int16 cardIndex, Int16 encIndex, Int32[] pEncPosArray, Int16 count = 1);

        /// @brief  读取Ecat编码器值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatEncPos", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatEncPos(Int16 cardIndex, Int16 encIndex, Int32[] pEncPosArray, Int16 count = 1);

        /// @brief  读取Ecat编码器原始值（PDO原始编码数据）
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatEncPosRaw", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatEncPosRaw(Int16 cardIndex, Int16 encIndex, Int32[] pEncPosArray, Int16 count = 1);

        /// @brief  读取Ecat编码器速度
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatEncVel", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatEncVel(Int16 cardIndex, Int16 encIndex, Int32[] pEncVelArray, Int16 count = 1);
        #endregion

        ///@defgroup EcatAxis EcatAxis
        ///@brief 板卡ECAT相关函数
        #region EcatAxis

        ///@defgroup EcatAxisPDO EcatAxisPDO
        ///@brief 板卡ECAT轴PDO功能
        #region EcatAxisPDO

        /// @brief  设置EtherCAT类型轴控制模式0x6060
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetEcatAxMode", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetEcatAxMode(Int16 cardIndex, Int16 axNo, Int16 ctrlMode);

        /// @brief  读取EtherCAT类型轴控制模式
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatAxMode", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatAxMode(Int16 cardIndex, Int16 axNo, ref Int16 pCtrlMode);

        /// @brief  获取EtherCAT类型轴的对应的数字量输入,pdo必须配置0x60fd
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatAxDi", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatAxDi(Int16 cardIndex, Int16 axNo, ref UInt32 pDiVal);

        /// @brief  设置EtherCAT类型轴的对应的数字量输出,pdo必须配置0x60fe:01
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetEcatAxDo", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetEcatAxDo(Int16 cardIndex, Int16 axNo, UInt32 doVal);

        /// @brief  获取EtherCAT类型轴的对应的数字量输输出,pdo必须配置0x60fe:01
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatAxDo", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatAxDo(Int16 cardIndex, Int16 axNo, ref UInt32 pDoVal);

        /// @brief  获取EtherCAT类型轴的对应的错误码,pdo必须配置0x603f
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatAxErrCode", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatAxErrCode(Int16 cardIndex, Int16 axNo, ref UInt16 pErrCode);

        /// @brief  设置EtherCAT类型轴的最大速度限制,pdo必须配置0x607f
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetEcatAxMaxVelLmt", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetEcatAxMaxVelLmt(Int16 cardIndex, Int16 axNo, UInt32 maxVel);

        /// @brief  获取EtherCAT类型轴的最大速度限制,pdo必须配置0x607f
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatAxMaxVelLmt", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatAxMaxVelLmt(Int16 cardIndex, Int16 axNo, ref UInt32 pMaxVel);

        /// @brief  设置 EtherCAT 类型轴的对应的正向力矩限制,pdo必须配置0x60e0
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetEcatAxPosTorqLmt", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetEcatAxPosTorqLmt(Int16 cardIndex, Int16 axNo, UInt16 posTorqLmt);

        /// @brief  获取 EtherCAT 类型轴的对应的正向力矩限制,pdo必须配置0x60e0
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatAxPosTorqLmt", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatAxPosTorqLmt(Int16 cardIndex, Int16 axNo, ref UInt16 pPosTorqLmt);

        /// @brief  设置 EtherCAT 类型轴的对应的负向力矩限制,pdo必须配置0x60e1
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetEcatAxNegTorqLmt", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetEcatAxNegTorqLmt(Int16 cardIndex, Int16 axNo, UInt16 negTorqLmt);

        /// @brief  获取 EtherCAT 类型轴的对应的负向力矩限制,pdo必须配置0x60e1
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatAxNegTorqLmt", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatAxNegTorqLmt(Int16 cardIndex, Int16 axNo, ref UInt16 pNegTorqLmt);

        /// @brief  设置 EtherCAT 类型轴的最大力矩限制,pdo必须配置0x6072
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetEcatAxMaxTorqLmt", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetEcatAxMaxTorqLmt(Int16 cardIndex, Int16 axNo, UInt16 maxTorqLmt);

        /// @brief  获取 EtherCAT 类型轴的最大力矩限制,pdo必须配置0x6072
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatAxMaxTorqLmt", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatAxMaxTorqLmt(Int16 cardIndex, Int16 axNo, ref UInt16 pMaxTorqLmt);
        #endregion

        ///@defgroup EcatAxisThreshold EcatAxisThreshold
        ///@brief 板卡ECAT轴急停保持参数配置函数
        #region EcatAxisThreshold

        /// @brief  设置EtherCAT轴使能控制延时时间, 主要用于特殊场合, 使能控制字输出, 驱动器寻相后, 才能给出使能状态字
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetEcatAxOnThreshold", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetEcatAxOnThreshold(Int16 cardIndex, Int16 axNo, UInt16 waitPeriod);

        /// @brief  获取EtherCAT轴使能控制延时时间
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatAxOnThreshold", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatAxOnThreshold(Int16 cardIndex, Int16 axNo, ref UInt16 pWaitPeriod);
        #endregion

        ///@defgroup EcatAxisCSV EcatAxisCSV
        ///@brief 板卡ECAT轴CSV模式功能
        #region EcatAxisCSV

        /// @brief  启动 CSV 速度规划
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_StartEcatAxCsvPrf", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_StartEcatAxCsvPrf(Int16 cardIndex, Int16 axNo, Int32 tgtVel, Int32 acc, Int16 prfType);

        /// @brief  更新 CSV 速度规划的目标速度
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_UpdateEcatAxCsvPrf", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_UpdateEcatAxCsvPrf(Int16 cardIndex, Int16 axNo, Int32 tgtVel);

        /// @brief  获取 CSV 速度规划的状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatAxCsvPrfSts", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatAxCsvPrfSts(Int16 cardIndex, Int16 axNo, ref Int16 pStatus);
        #endregion

        ///@defgroup EcatAxisCST EcatAxisCST
        ///@brief 板卡ECAT轴CST模式功能
        #region EcatAxisCST

        /// @brief  启动 CST 线性转矩规划
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_StartEcatAxTorqPrf", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_StartEcatAxTorqPrf(Int16 cardIndex, Int16 axNo, Int16 tgtTorq, Int16 time);

        /// @brief  获取 EtherCAT 类型轴的实际转矩, 根据count值可一次获取多个轴的扭矩
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatAxActTorq", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatAxActTorq(Int16 cardIndex, Int16 axNo, Int16[] pActTorqArray, Int16 count = 1);

        /// @brief  设置 EtherCAT 类型轴的对应的转矩斜坡值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetEcatAxTorqSlope", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetEcatAxTorqSlope(Int16 cardIndex, Int16 axNo, UInt32 torqSlope);

        /// @brief  获取 EtherCAT 类型轴的对应的转矩斜坡值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatAxTorqSlope", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatAxTorqSlope(Int16 cardIndex, Int16 axNo, ref UInt32 pTorqSlope);

        /// @brief  设置 EtherCAT 类型轴的对应的目标转矩
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetEcatAxTgtTorq", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetEcatAxTgtTorq(Int16 cardIndex, Int16 axNo, Int16 tgtTorq);

        /// @brief  获取 EtherCAT 类型轴的对应的目标转矩
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatAxTgtTorq", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatAxTgtTorq(Int16 cardIndex, Int16 axNo, ref Int16 pTgtTorq);
        #endregion

        ///@defgroup EcatAxisCapt EcatAxisCapt
        ///@brief 板卡ECAT轴捕获功能功能
        #region EcatAxisCapt

        /// @brief  设置 EtherCAT 类型轴的位置捕获类型
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetEcatAxCapt", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetEcatAxCapt(Int16 cardIndex, Int16 axNo, Int16 trigType, Int16 edge);

        /// @brief  获取 EtherCAT 类型轴的位置捕获类型
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatAxCapt", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatAxCapt(Int16 cardIndex, Int16 axNo, ref Int16 pTrigType, ref Int16 pEdge);

        /// @brief  获取 EtherCAT 类型轴的位置捕获状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatAxCaptStatus", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatAxCaptStatus(Int16 cardIndex, Int16 axNo, ref Int16 pSts, ref Int32 pPosVal, ref Int32 pNegVal);
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetEcatAxProbeFun", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetEcatAxProbeFun(Int16 cardIndex, Int16 axNo, Int16 probeFun);
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatAxProbeFun", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatAxProbeFun(Int16 cardIndex, Int16 axNo, ref Int16 pProbeFun);
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatAxProbeSts", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatAxProbeSts(Int16 cardIndex, Int16 axNo, ref Int16 pProbeSts, ref Int32 pProbe1Pos, ref Int32 pProbe1Neg, ref Int32 pProbe2Pos, ref Int32 pProbe2Neg);
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetEcatAxProbeContinousCount", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetEcatAxProbeContinousCount(Int16 cardIndex, Int16 axNo, Int16[] pCountArray);
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatAxProbeContinousCount", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatAxProbeContinousCount(Int16 cardIndex, Int16 axNo, Int16[] pCountArray, Int16[] pTrigArray, Int16[] pRestArray);
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatAxProbeContinousData", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatAxProbeContinousData(Int16 cardIndex, Int16 axNo, Int16 probeType, Int32[] pPosArray, Int16 count);
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetEcatAxProbeContinousTable", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetEcatAxProbeContinousTable(Int16 cardIndex, Int16 axNo, Int16 tableIndex);
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatAxProbeContinousTable", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatAxProbeContinousTable(Int16 cardIndex, Int16 axNo, ref Int16 pTableIndex);
        /// @brief 清除 EtherCAT 类型轴的连续位置捕获状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_ClrEcatAxCaptStatus", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_ClrEcatAxCaptStatus(Int16 cardIndex, Int16 axNo);
        #endregion
        #endregion

        ///@defgroup HEcatFunc HEcatFunc
        ///@brief H主站函数
        #region HEcatFunc


        ///@defgroup H-MasterCfg H-MasterCfg
        ///@brief H主站系统配置函数
        #region H

        /// @brief  获取板卡的CPU负载
        /// @warning 建议负载率不要超过60%
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_H_GetCalcLoadRatio", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_H_GetCalcLoadRatio(Int16 cardIndex, ref double pLoadRatio);

        /// @brief  复位控制卡系统参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_H_ResetSysPara", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_H_ResetSysPara(Int16 cardIndex);
        #endregion
        ///@defgroup H-EtherCAT H-EtherCAT
        ///@brief 板卡H主站功能
        #region H

        /// @brief  初始化板卡通讯, 主站进入OP状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_H_InitEcatComm", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_H_InitEcatComm(Int16 cardIndex);

        /// @brief  开启板卡通讯, 初始化EtherCAT总线资源, 建立主站与各从站之间的通讯
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_H_StartEcatComm", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_H_StartEcatComm(Int16 cardIndex);

        /// @brief  关闭板卡通讯, 主站退出OP状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_H_DelEcatComm", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_H_DelEcatComm(Int16 cardIndex);

        /// @brief  初始化总线, 直到主站进入OP状态, 随后扫描EtherCAT总线资源, 并建立主站与各从站之间的通讯 (该函数会阻塞在EtherCAT的总线扫描同步阶段)
        /// @warning 该指令阻塞执行
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_H_ScanCardEcat", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_H_ScanCardEcat(Int16 cardIndex, Int16 waitTime = 40);

        /// @brief  获取EtherCAT主站的状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_H_GetEcatMasterSts", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_H_GetEcatMasterSts(Int16 cardIndex, ref UInt32 pStatus);

        /// @brief  获取EtherCAT协议栈通讯错误码
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_H_GetEcatErrCode", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_H_GetEcatErrCode(Int16 cardIndex, ref UInt32 pErrCode);

        /// @brief  获取EtherCAT主站的状态，Ex指令
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_H_GetEcatMasterStsEx", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_H_GetEcatMasterStsEx(Int16 cardIndex, ref UInt32 pStatus);

        /// @brief  获取EtherCAT协议栈通讯错误码，Ex指令
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_H_GetEcatErrCodeEx", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_H_GetEcatErrCodeEx(Int16 cardIndex, ref UInt32 pErrCode);

        /// @brief  获取EtherCAT从站当前通讯阶段, INIT, PerOP, SafeOP, OP
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_H_GetEcatSlaveSts", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_H_GetEcatSlaveSts(Int16 cardIndex, Int16 slave, ref Int16 pCurStatus, ref Int16 pReqStatus, ref Int16 pErrCode);

        /// @brief  获取EtherCAT主站信息(包含通讯周期, 从站个数, 资源数量等)
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_H_GetEcatMasterInfo", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_H_GetEcatMasterInfo(Int16 cardIndex, ref TMasterInfo ptMasterInfo);

        /// @brief  获取EtherCAT从站信息(包含设备类型, 对应轴号, 从站别名等)
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_H_GetEcatSlaveInfo", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_H_GetEcatSlaveInfo(Int16 cardIndex, Int16 slave, ref TSlaveInfo ptSlaveInfo);

        /// @brief  根据站号读取EtherCAT从站的PDO数据
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_H_GetEcatSlavePdoData", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_H_GetEcatSlavePdoData(Int16 cardIndex, Int16 slave, UInt16 index, UInt16 subIndex, Byte[] pData);

        /// @brief  根据站号写入EtherCAT从站的PDO数据
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_H_SetEcatSlavePdoData", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_H_SetEcatSlavePdoData(Int16 cardIndex, Int16 slave, UInt16 index, UInt16 subIndex, Byte[] pData);

        /// @brief  根据站号获取EtherCAT从站的SDO数据
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_H_GetEcatSlaveSdo", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_H_GetEcatSlaveSdo(Int16 cardIndex, Int16 slave, UInt16 index, UInt16 subIndex, Byte[] pData, UInt32 dataSize, ref UInt32 pResultSize, ref UInt32 pAbortCode);

        /// @brief  根据站号写入EtherCAT从站的SDO数据
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_H_SetEcatSlaveSdo", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_H_SetEcatSlaveSdo(Int16 cardIndex, Int16 slave, UInt16 index, UInt16 subIndex, Byte[] pData, UInt32 dataSize, ref UInt32 pAbortCode);

        /// @brief  获取EtherCAT的DI输入状态, 按16bits为一组进行获取
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_H_GetEcatDi", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_H_GetEcatDi(Int16 cardIndex, Int16 packIndex, UInt16[] pValue, Int16 packCnt);

        /// @brief  设置EtherCAT的DO输出状态, 按16bits为一组进行设置
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_H_SetEcatDo", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_H_SetEcatDo(Int16 cardIndex, Int16 packIndex, UInt16[] pValue, Int16 packCnt);

        /// @brief  获取EtherCAT的DO输出状态, 按16bits为一组进行获取
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_H_GetEcatDo", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_H_GetEcatDo(Int16 cardIndex, Int16 packIndex, UInt16[] pValue, Int16 packCnt);

        /// @brief  获取EtherCAT的DI输入状态, 按位进行获取
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_H_GetEcatDiBit", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_H_GetEcatDiBit(Int16 cardIndex, Int16 diIndex, ref Int16 pValue);

        /// @brief  获取EtherCAT的DO输出状态, 按位进行获取
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_H_GetEcatDoBit", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_H_GetEcatDoBit(Int16 cardIndex, Int16 doIndex, ref Int16 pValue);

        /// @brief  设置EtherCAT的DO输出状态, 按位进行设置
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_H_SetEcatDoBit", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_H_SetEcatDoBit(Int16 cardIndex, Int16 doIndex, Int16 value);

        /// @brief  根据通道号获取EtherCAT对应通道的AD值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_H_GetEcatAdVal", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_H_GetEcatAdVal(Int16 cardIndex, Int16 adIndex, ref Int16 pValue);

        /// @brief  根据通道号设置EtherCAT对应通道的DA值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_H_SetEcatDaVal", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_H_SetEcatDaVal(Int16 cardIndex, Int16 daIndex, Int16 Value);

        /// @brief  根据通道号获取EtherCAT对应通道的DA值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_H_GetEcatDaVal", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_H_GetEcatDaVal(Int16 cardIndex, Int16 daIndex, ref Int16 pValue);

        /// @brief  根据通道号获取EtherCAT对应通道的RegIn值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_H_GetEcatRegInVal", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_H_GetEcatRegInVal(Int16 cardIndex, Int16 regIndex, ref float pValue);

        /// @brief  根据通道号设置EtherCAT对应通道的RegOut值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_H_SetEcatRegOutVal", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_H_SetEcatRegOutVal(Int16 cardIndex, Int16 regIndex, float Value);

        /// @brief  根据通道号获取EtherCAT对应通道的RegOut值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_H_GetEcatRegOutVal", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_H_GetEcatRegOutVal(Int16 cardIndex, Int16 regIndex, ref float pValue);

        /// @brief  根据通道号获取EtherCAT对应通道的AD值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_H_GetEcatAd", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_H_GetEcatAd(Int16 cardIndex, Int16 adIndex, Int16[] pValueArray, Int16 count);

        /// @brief  根据通道号设置EtherCAT对应通道的DA值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_H_SetEcatDa", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_H_SetEcatDa(Int16 cardIndex, Int16 daIndex, Int16[] pValueArray, Int16 count);

        /// @brief  根据通道号获取EtherCAT对应通道的DA值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_H_GetEcatDa", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_H_GetEcatDa(Int16 cardIndex, Int16 daIndex, Int16[] pValueArray, Int16 count);

        /// @brief  根据通道号获取EtherCAT对应通道的RegIn值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_H_GetEcatRegIn", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_H_GetEcatRegIn(Int16 cardIndex, Int16 regIndex, float[] pValueArray, Int16 count);

        /// @brief  根据通道号设置EtherCAT对应通道的RegOut值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_H_SetEcatRegOut", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_H_SetEcatRegOut(Int16 cardIndex, Int16 regIndex, float[] pValueArray, Int16 count);

        /// @brief  根据通道号获取EtherCAT对应通道的RegOut值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_H_GetEcatRegOut", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_H_GetEcatRegOut(Int16 cardIndex, Int16 regIndex, float[] pValueArray, Int16 count);
        #endregion
        #endregion

        ///@defgroup LocalBus LocalBus
        ///@brief 板卡端子板功能
        #region LocalBus

        ///@defgroup LocalInfo LocalInfo
        ///@brief 板卡端子板信息获取
        #region LocalInfo

        /// @brief  获取端子板的工作状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetLocalBoardWorkSts", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetLocalBoardWorkSts(Int16 cardIndex, ref Int16 pValue);
        #endregion

        ///@defgroup LocalParam LocalParam
        ///@brief 板卡端子板参数设置
        #region LocalParam

        /// @brief  设置端子板 DI 滤波时间
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetLocalDiFilterTime", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetLocalDiFilterTime(Int16 cardIndex, UInt16 filterTime);

        /// @brief  读取端子板 DI 滤波时间
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetLocalDiFilterTime", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetLocalDiFilterTime(Int16 cardIndex, ref UInt16 pFilterTime);

        /// @brief  设置端子板编码器滤波参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetLocalEncFilterPara", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetLocalEncFilterPara(Int16 cardIndex, Int16 filterDepth, Int16 filterCoef);

        /// @brief  读取端子板编码器滤波参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetLocalEncFilterPara", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetLocalEncFilterPara(Int16 cardIndex, ref Int16 pFilterDepth, ref Int16 pFilterCoef);

        /// @brief  设置端子板编码器计数方向
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetLocalEncDir", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetLocalEncDir(Int16 cardIndex, Int16 encIndex, Int16[] pDirArray, Int16 count = 1);

        /// @brief  读取端子板编码器计数方向
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetLocalEncDir", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetLocalEncDir(Int16 cardIndex, Int16 encIndex, Int16[] pDirArray, Int16 count = 1);
        #endregion

        ///@defgroup LocalDIO LocalDIO
        ///@brief 板卡端子板IO操作
        #region LocalDIO

        /// @brief  获取本地 DI 值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetLocalDi", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetLocalDi(Int16 cardIndex, ref Int16 pValue);

        /// @brief  设置本地 DO 值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetLocalDo", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetLocalDo(Int16 cardIndex, Int16 value);

        /// @brief  读取本地 DO 值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetLocalDo", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetLocalDo(Int16 cardIndex, ref Int16 pValue);
        #endregion

        /// @brief  按位获取本地第 diIndex 个 DI 值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetLocalDiBit", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetLocalDiBit(Int16 cardIndex, Int16 diIndex, ref Int16 pValue);

        /// @brief  按位设置本地第 doIndex 个 DO 值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetLocalDoBit", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetLocalDoBit(Int16 cardIndex, Int16 doIndex, Int16 value);

        /// @brief  按位设置本地第 doIndex 个 DO 值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetLocalDoBit", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetLocalDoBit(Int16 cardIndex, Int16 doIndex, ref Int16 pValue);

        /// @brief 按位脉冲输出本地第 doIndex 个 DO
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetLocalDoBitPulse", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetLocalDoBitPulse(Int16 cardIndex, Int16 doIndex, Int16 enable, UInt16 highLevelTime, UInt16 lowLevelTime, Int16 pulseNum, Int16 firstLevel);

        ///@defgroup LocalEnc LocalEnc
        ///@brief 板卡端子板编码器操作
        #region LocalEnc

        /// @brief  设置编码器值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetLocalEncPos", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetLocalEncPos(Int16 cardIndex, Int16 encIndex, Int32 encPos);

        /// @brief  读取编码器值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetLocalEncPos", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetLocalEncPos(Int16 cardIndex, Int16 encIndex, Int32[] pEncPosArray, Int16 count = 1);

        /// @brief  读取编码器速度
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetLocalEncVel", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetLocalEncVel(Int16 cardIndex, Int16 encIndex, Int32[] pEncVelArray, Int16 count = 1);
        #endregion

        ///@defgroup LocalCapt LocalCapt
        ///@brief 板卡端子板捕获功能
        #region LocalCapt

        /// @brief  设置端子板编码器位置捕获模式, 配置捕获参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetLocalCaptMode", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetLocalCaptMode(Int16 cardIndex, Int16 encIndex, Int16 trigType, Int16 edge);

        /// @brief  获取端子板编码器位置捕获模式配置参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetLocalCaptMode", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetLocalCaptMode(Int16 cardIndex, Int16 encIndex, ref Int16 pTrigType, ref Int16 pEdge);

        /// @brief  获取端子板编码器位置捕获状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetLocalCaptStatus", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetLocalCaptStatus(Int16 cardIndex, Int16 encIndex, ref Int16 pSts, ref Int32 pPosVal, ref Int32 pNegVal);

        /// @brief  设置端子板编码器连续位置捕获模式, 配置连续捕获参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetLocalCaptRepeatMode", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetLocalCaptRepeatMode(Int16 cardIndex, Int16 encIndex, Int16 trigType, Int16 edge, Int16 captCount);

        /// @brief  设置端子板编码器连续位置捕获模式配置参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetLocalCaptRepeatMode", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetLocalCaptRepeatMode(Int16 cardIndex, Int16 encIndex, ref Int16 pTrigType, ref Int16 pEdge, ref Int16 pCaptCount);

        /// @brief  获取端子板编码器连续位置捕获状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetLocalCaptRepeatStatus", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetLocalCaptRepeatStatus(Int16 cardIndex, Int16 encIndex, ref Int16 pCaptSts, ref Int16 pCaptCount);

        /// @brief  获取端子板编码器连续位置捕获位置
        /// @attention 双沿沿捕获时, 开辟存储数组长度应为 IMC_SetLocalCaptRepeatMode 设置 的 captCount 的 2 倍
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetLocalCaptRepeatPos", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetLocalCaptRepeatPos(Int16 cardIndex, Int16 encIndex, Int16 startNo, Int16 count, Int32[] pCaptPosArray);
        #endregion

        ///@defgroup LocalPWM LocalPWM
        ///@brief 板卡端子板PWM功能
        #region LocalPWM

        /// @brief  设置端子板PWM的输出参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetLocalPwmPara", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetLocalPwmPara(Int16 cardIndex, Int16 chn, Int32 frequency, double dutyRatio);

        /// @brief  获取端子板PWM的输出参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetLocalPwmPara", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetLocalPwmPara(Int16 cardIndex, Int16 chn, ref Int32 pFrequency, ref double pDutyRatio);

        /// @brief  设置端子板PWM的输出频率
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetLocalPwmFrq", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetLocalPwmFrq(Int16 cardIndex, Int16 chn, Int32 frequency);

        /// @brief  设置端子板PWM的输出占空比
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetLocalPwmDuty", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetLocalPwmDuty(Int16 cardIndex, Int16 chn, double dutyRatio);

        /// @brief  设置端子板PWM输出的开关及延时时间
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetLocalPwmOnOffDelay", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetLocalPwmOnOffDelay(Int16 cardIndex, Int16 chn, Int16 onOff, UInt16 delay);
        #endregion

        ///@defgroup LocalCompare LocalCompare
        ///@brief 板卡端子板位置比较功能
        #region LocalCompare

        /// @brief  设置端子板位置比较输出源的配置
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetLocalCmpSrcCfg", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetLocalCmpSrcCfg(Int16 cardIndex, Int16 chn, Int16 compSrc);

        /// @brief  设置端子板比较物理信号输出的配置
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetLocalCmpOutputCfg", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetLocalCmpOutputCfg(Int16 cardIndex, Int16 chn, Int16 ctrlMode, Int16 stLevel, Int16 outputType, UInt16 pulseWidth);

        /// @brief  设置端子板比较数据的类型
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetLocalCmpDataType", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetLocalCmpDataType(Int16 cardIndex, Int16 chn, Int16 type);

        /// @brief  获取端子板比较数据的类型
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetLocalCmpDataType", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetLocalCmpDataType(Int16 cardIndex, Int16 chn, ref Int16 pType);

        /// @brief  设置端子板位置比较的位置类型
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetLocalCmpPosType", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetLocalCmpPosType(Int16 cardIndex, Int16 chn, Int16 type);

        /// @brief  获取端子板位置比较的位置类型
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetLocalCmpPosType", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetLocalCmpPosType(Int16 cardIndex, Int16 chn, ref Int16 pType);

        /// @brief  设置本地输出 EX-O bit0 ~ 3 位位置比较输出类型
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetLocalGpoType", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetLocalGpoType(Int16 cardIndex, Int16 doIndex, Int16 type);

        /// @brief  获取本地输出 EX-O bit0 ~ 3 位位置比较输出类型配置
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetLocalGpoType", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetLocalGpoType(Int16 cardIndex, Int16 doIndex, ref Int16 pType);

        /// @brief  获取端子板位置比较的状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetLocalCmpSts", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetLocalCmpSts(Int16 cardIndex, Int16 chn, ref Int16 pCmpSts, ref Int32 pCmpCount);

        /// @brief  设置端子板手动输出脉冲或电平
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetLocalCmpManualOut", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetLocalCmpManualOut(Int16 cardIndex, Int16 chn, Int16 outval);

        /// @brief  设置端子板多脉冲手动输出
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetLocalCmpManualMultiOut", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetLocalCmpManualMultiOut(Int16 cardIndex, Int16 chn, UInt16 pulseNum, UInt16 pulseCycle);

        /// @brief  设置端子板等间距线性比较输出
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetLocalCmpLinearOut", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetLocalCmpLinearOut(Int16 cardIndex, Int16 chn, Int32 intrvalLen, Int32 cmpCount);

        /// @brief  停止端子板比较输出
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_StopLocalCmpOut", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_StopLocalCmpOut(Int16 cardIndex, Int16 chn);

        /// @brief  启动端子板比较输出
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_StartLocalCmpOut", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_StartLocalCmpOut(Int16 cardIndex, Int16 chn);

        /// @brief  设置端子板一维位置比较输出数据
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetLocalCmpData", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetLocalCmpData(Int16 cardIndex, Int16 chn, Int16 compCount, Int32[] pPosBufArray);
        #endregion

        ///@defgroup LocalMultCompare LocalMultCompare
        ///@brief 板卡端子板多维位置比较功能
        #region LocalMultCompare

        /// @brief  设置端子板位置比较输出源的配置
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetLocalMultiCmpSrcCfg", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetLocalMultiCmpSrcCfg(Int16 cardIndex, Int16 chn, Int16 dimension, Int16[] pCompSrcArray);

        /// @brief  设置端子板比较物理信号输出的配置
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetLocalMultiCmpOutputCfg", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetLocalMultiCmpOutputCfg(Int16 cardIndex, Int16 chn, Int16 stLevel, Int16 outputType, UInt16 pulseWidth);

        /// @brief  获取端子板位置比较的状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetLocalMultiCmpSts", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetLocalMultiCmpSts(Int16 cardIndex, Int16 chn, ref Int16 pCmpSts, ref Int32 pCmpCount);

        /// @brief  设置端子板比较数据的类型
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetLocalMultiCmpDataType", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetLocalMultiCmpDataType(Int16 cardIndex, Int16 chn, Int16 type);

        /// @brief  获取端子板比较数据的类型
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetLocalMultiCmpDataType", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetLocalMultiCmpDataType(Int16 cardIndex, Int16 chn, ref Int16 pType);

        /// @brief  设置端子板多维位置比较参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetLocalMultiCmpPara", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetLocalMultiCmpPara(Int16 cardIndex, Int16 chn, UInt16 syncDeltaPos, Int16 outPinType);

        /// @brief  获取端子板多维位置比较参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetLocalMultiCmpPara", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetLocalMultiCmpPara(Int16 cardIndex, Int16 chn, ref UInt16 pSyncDeltaPos, ref Int16 pOutPinType);


        /// @brief  设置多维比较位置点
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetLocalMultiCmpData", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetLocalMultiCmpData(Int16 cardIndex, Int16 chn, TMultiCmpData[] pComparaDataArray, Int16 count);

        /// @brief  启动端子板比较输出
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_StartLocalMultiCmpOut", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_StartLocalMultiCmpOut(Int16 cardIndex, Int16 chn);

        /// @brief  停止端子板比较输出
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_StopLocalMultiCmpOut", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_StopLocalMultiCmpOut(Int16 cardIndex, Int16 chn);
        #endregion

        ///@defgroup LocalPSO LocalPSO
        ///@brief 板卡端子板PSO功能
        #region LocalPSO

        /// @brief  设置端子板PSO功能参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetLocalPSOPara", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetLocalPSOPara(Int16 cardIndex, Int16 chn, Int16 dimension, Int16 pinType, Int16[] pPsoPosIndexArray, UInt16 outPlsWidth, Int32 syncDeltaPos);

        /// @brief  获取端子板PSO功能参数配置
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetLocalPSOPara", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetLocalPSOPara(Int16 cardIndex, Int16 chn, ref Int16 pDimension, ref Int16 pPinType, Int16[] pPsoPosIndexArray, ref UInt16 pOutPlsWidth, ref Int32 pSyncDeltaPos);

        /// @brief  设置PSO基频
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetLocalPSOBaseFrq", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetLocalPSOBaseFrq(Int16 cardIndex, Int16 chn, Int16 baseFrq);

        /// @brief  获取PSO基频
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetLocalPSOBaseFrq", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetLocalPSOBaseFrq(Int16 cardIndex, Int16 chn, ref Int16 pBaseFrq);

        /// @brief  启动PSO输出
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_StartLocalPSO", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_StartLocalPSO(Int16 cardIndex, Int16 chn);

        /// @brief  停止PSO输出
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_StopLocalPSO", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_StopLocalPSO(Int16 cardIndex, Int16 chn);

        /// @brief  获取PSO输出状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetLocalPSOSts", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetLocalPSOSts(Int16 cardIndex, Int16 chn, ref Int16 pSts, ref UInt16 pPsoCount);
        #endregion

        #endregion

        ///@defgroup Axis Axis
        ///@brief 板卡轴操作相关函数
        #region Axis
        ///@defgroup AxisConfig AxisConfig
        ///@brief 板卡轴配置函数
        #region AxisConfig
        /// @brief  设置轴激活状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetAxActive", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetAxActive(Int16 cardIndex, Int16 axNo, Int16 active);

        /// @brief  读取轴激活状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetAxActive", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetAxActive(Int16 cardIndex, Int16 axNo, ref Int16 pActive);

        /// @brief  设置控制器单轴安全运行参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetAxMaxMtPara", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetAxMaxMtPara(Int16 cardIndex, Int16 axNo, ref TMtPara pMtPara);

        /// @brief  读取控制器单轴安全运行参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetAxMaxMtPara", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetAxMaxMtPara(Int16 cardIndex, Int16 axNo, ref TMtPara pMtPara);

        /// @brief  设置轴当量参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetAxEquiv", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetAxEquiv(Int16 cardIndex, Int16 axNo, double[] pAxEquArray, Int16 count = 1);

        /// @brief  获取轴当量参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetAxEquiv", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetAxEquiv(Int16 cardIndex, Int16 axNo, double[] pAxEquArray, Int16 count = 1);

        /// @brief  设置轴输出绑定
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetAxBondCfg", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetAxBondCfg(Int16 cardIndex, Int16 axNo, Int16 axType, Int16 outputChn);

        /// @brief  获取轴输出绑定
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetAxBondCfg", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetAxBondCfg(Int16 cardIndex, Int16 axNo, ref Int16 pAxType, ref Int16 pOutputChn);

        /// @brief  复位轴绑定状态, 让所有轴恢复到虚轴状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_ResetAxBondCfg", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_ResetAxBondCfg(Int16 cardIndex);

        /// @brief  设置轴属性参数, 包含软限位、到位误差、最大跟随误差
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetAxAttriPara", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetAxAttriPara(Int16 cardIndex, Int16 axNo, ref TAxAttriPara pAxAttriPara);

        /// @brief  获取轴属性参数, 包含软限位、到位误差、最大跟随误差
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetAxAttriPara", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetAxAttriPara(Int16 cardIndex, Int16 axNo, ref TAxAttriPara pAxAttriPara);

        /// @brief  设置轴软限位
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetAxSoftLimit", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetAxSoftLimit(Int16 cardIndex, Int16 axNo, Int32 positive, Int32 negative);

        /// @brief  获取轴软限位
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetAxSoftLimit", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetAxSoftLimit(Int16 cardIndex, Int16 axNo, ref Int32 pPositive, ref Int32 pNegative);

        /// @brief  设置轴到位检查参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetAxArrivalBand", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetAxArrivalBand(Int16 cardIndex, Int16 axNo, Int16 arrivalBand, UInt16 arrivalTime);

        /// @brief  获取轴到位检查参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetAxArrivalBand", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetAxArrivalBand(Int16 cardIndex, Int16 axNo, ref Int16 pArrivalBand, ref UInt16 pArrivalTime);

        /// @brief  设置轴最大跟随误差
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetAxErrorPos", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetAxErrorPos(Int16 cardIndex, Int16 axNo, Int32 errorPos);

        /// @brief  获取轴最大跟随误差
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetAxErrorPos", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetAxErrorPos(Int16 cardIndex, Int16 axNo, ref Int32 pErrorPos);

        /// @brief  设置轴反向间隙补偿
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetAxBacklash", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetAxBacklash(Int16 cardIndex, Int16 axNo, Int32 wholeCmpVal, Int32 cmpVel, Int16 cmpDir);

        /// @brief  获取轴反向间隙补偿
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetAxBacklash", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetAxBacklash(Int16 cardIndex, Int16 axNo, ref Int32 pWholeCmpVal, ref Int32 pCmpVel, ref Int16 pCmpDir);

        /// @brief  设置轴安全检查使能综合配置
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetAxSafeCheck", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetAxSafeCheck(Int16 cardIndex, Int16 axNo, ref TAxCheckEn pAxCheckEn);

        /// @brief  获取轴安全检查使能综合配置
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetAxSafeCheck", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetAxSafeCheck(Int16 cardIndex, Int16 axNo, ref TAxCheckEn pAxCheckEn);

        /// @brief  设置轴报警检查使能配置
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetAxAlarmCheck", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetAxAlarmCheck(Int16 cardIndex, Int16 axNo, Int16 enable, Int16 count = 1);

        /// @brief  设置轴软限位检查使能配置
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetAxSoftLmtsCheck", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetAxSoftLmtsCheck(Int16 cardIndex, Int16 axNo, Int16 enable, Int16 count = 1);

        /// @brief  设置轴硬限位检查使能配置
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetHwLmtsCheck", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetHwLmtsCheck(Int16 cardIndex, Int16 axNo, Int16 enable, Int16 count = 1);

        /// @brief  设置轴跟随误差位检查使能配置
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetAxErrPosCheck", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetAxErrPosCheck(Int16 cardIndex, Int16 axNo, Int16 enable, Int16 count = 1);

        /// @brief  设定轴停止的平滑停止减速度和急停减速度
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetAxStopDec", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetAxStopDec(Int16 cardIndex, Int16 axNo, double decSmoothStop, double decAbruptStop);

        /// @brief  获取轴停止的平滑停止减速度和急停减速度
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetAxStopDec", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetAxStopDec(Int16 cardIndex, Int16 axNo, ref double pDecSmoothStop, ref double pDecAbruptStop);

        /// @brief  设定轴急停减速限制时间
        /// @details 急停时根据当前速度和设定的急停减速度计算停止时间, 若超过设定的限制时间, 则按照当前限制时间重新计算减速度, 否则按照设定的减速度执行
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetAxEmgMaxDecLmt", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetAxEmgMaxDecLmt(Int16 cardIndex, Int16 axNo, UInt16 decLmtTime);

        /// @brief  获取轴急停减速限制时间
        /// @details 急停时根据当前速度和设定的急停减速度计算停止时间, 若超过设定的限制时间, 则按照当前限制时间重新计算加速度, 否则按照设定的加速度执行
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetAxEmgMaxDecLmt", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetAxEmgMaxDecLmt(Int16 cardIndex, Int16 axNo, ref UInt16 pDecLmtTime);

        /// @brief  设置轴的结束速度
        /// @details 点位运动时，运动规划结束速度可以不为0，注意合理设置结束速度值，运动规划完成后，结束速度直接突跳速度为0，可能引起机台抖动
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetAxEndVel", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetAxEndVel(Int16 cardIndex, Int16 axNo, double endVel);

        /// @brief  获取轴的结束速度设定
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetAxEndVel", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetAxEndVel(Int16 cardIndex, Int16 axNo, ref double pEndVel);

        /// @brief  设置圆形软限位参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_ArcZoneLmtSetParam", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_ArcZoneLmtSetParam(Int16 cardIndex, Int16[] pAxNoArray, Int32[] pCenterArray, Int32 radius, Int16 sourceType);

        /// @brief  获取圆形软限位参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_ArcZoneLmtGetParam", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_ArcZoneLmtGetParam(Int16 cardIndex, Int16[] pAxNoArray, Int32[] pCenterArray, ref Int32 pRadius, ref Int16 pSourceType);

        /// @brief  使能圆形软限位
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_ArcZoneLmtEnable", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_ArcZoneLmtEnable(Int16 cardIndex, Int16 enable);

        /// @brief  设置分段软限位参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SegLmtSetParam", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SegLmtSetParam(Int16 cardIndex, Int16[] pAxNoArray, Int16 sourceType, Int32[] pPointXArray, Int32[] pPointYArray, Int16 pointCount, Int16 yLmtDir);

        /// @brief  获取分段软限位参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SegLmtGetParam", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SegLmtGetParam(Int16 cardIndex, Int16[] pAxNoArray, ref Int16 pSourceType, Int32[] pPointXArray, Int32[] pPointYArray, ref Int16 pPointCount, ref Int16 pYLmtDir);

        /// @brief  使能分段软限位
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SegLmtEnable", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SegLmtEnable(Int16 cardIndex, Int16 enable);

        #endregion

        ///@defgroup AxisStatus AxisStatus
        ///@brief 轴状态监控
        #region AxisStatus

        /// @brief  获取轴规划模式, 该值中包含单轴模式和多轴模式的信息
        /// @todo   是否取消单轴多轴状态分区, 底层需要做修改
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetAxPrfMode", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetAxPrfMode(Int16 cardIndex, Int16 axNo, Int16[] pPrfModeArray, Int16 count = 1);

        /// @brief  获取轴状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetAxSts", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetAxSts(Int16 cardIndex, Int16 axNo, Int32[] pAxStsArray, Int16 count = 1);

        /// @brief  清除轴的错误状态, 当发生报警时, 需要调用该指令进行清除, 但如果实际物理报警还在, 则无法清除
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_ClrAxSts", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_ClrAxSts(Int16 cardIndex, Int16 axNo, Int16 count = 1);

        /// @brief  获取轴规划位置
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetAxPrfPos", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetAxPrfPos(Int16 cardIndex, Int16 axNo, double[] pPrfPosArray, Int16 count = 1);

        /// @brief  获取轴规划速度
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetAxPrfVel", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetAxPrfVel(Int16 cardIndex, Int16 axNo, double[] pPrfVelArray, Int16 count = 1);

        /// @brief  获取轴规划加速度
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetAxPrfAcc", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetAxPrfAcc(Int16 cardIndex, Int16 axNo, double[] pPrfAccArray, Int16 count = 1);

        /// @brief  获取轴编码器反馈位置
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetAxEncPos", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetAxEncPos(Int16 cardIndex, Int16 axNo, double[] pEncPosArray, Int16 count = 1);

        /// @brief  获取轴编码器反馈速度
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetAxEncVel", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetAxEncVel(Int16 cardIndex, Int16 axNo, double[] pEncVelArray, Int16 count = 1);

        /// @brief  获取轴编码器反馈加速度
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetAxEncAcc", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetAxEncAcc(Int16 cardIndex, Int16 axNo, double[] pEncAccArray, Int16 count = 1);

        /// @brief  获取指定轴是否均已到位的状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetMultiAxArrivalSts", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetMultiAxArrivalSts(Int16 cardIndex, UInt32 axMask, ref Int16 pSts, Int16 groupNo);

        /// @brief  获取轴反向间隙差值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetAxBacklashCmpVal", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetAxBacklashCmpVal(Int16 cardIndex, Int16 axNo, ref Int32 pCmpVal);

        /// @brief  获取轴规划位置(32位整形指令)
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetAxPrfPos32", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetAxPrfPos32(Int16 cardIndex, Int16 axNo, Int32[] pPrfPosArray, Int16 count = 1);

        /// @brief  获取轴编码器反馈位置(32位整形指令)
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetAxEncPos32", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetAxEncPos32(Int16 cardIndex, Int16 axNo, Int32[] pEncPosArray, Int16 count = 1);

        /// @brief  获取轴规划位置(脉冲位置指令)
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetPrfPos", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetPrfPos(Int16 cardIndex, Int16 axNo, double[] pPrfPosArray, Int16 count = 1);

        /// @brief  获取轴编码器原点位置
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetAxOrgEncPos", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetAxOrgEncPos(Int16 cardIndex, Int16 axNo, Int32[] pOrgEncPosArray, Int16 count = 1);

        /// @brief  获取轴停止原因
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetAxStopReason", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetAxStopReason(Int16 cardIndex, Int16 axNo, Int16[] pAxStopReason, Int16 count = 1);

        /// @brief  获取轴状态(Ex指令)
        /// @todo 补充ex类型指令描述
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetAxStsEx", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetAxStsEx(Int16 cardIndex, Int16 axNo, Int32[] pAxStsArray, Int16 count = 1);

        /// @brief  获取轴规划位置(Ex指令)
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetAxPrfPosEx", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetAxPrfPosEx(Int16 cardIndex, Int16 axNo, double[] pPrfPosArray, Int16 count = 1);

        /// @brief  获取轴编码器反馈位置(Ex指令)
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetAxEncPosEx", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetAxEncPosEx(Int16 cardIndex, Int16 axNo, double[] pEncPosArray, Int16 count = 1);

        /// @brief  获取 EtherCAT 类型轴的实际转矩, 根据count值可一次获取多个轴的扭矩(Ex类型指令)
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetAxActTorqEx", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetAxActTorqEx(Int16 cardIndex, Int16 axNo, Int16[] pActTorqArray, Int16 count = 1);

        #endregion

        ///@defgroup AxisControl AxisControl
        ///@brief 轴功能控制
        #region AxisControl

        /// @brief  使能伺服
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_ServoOn", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_ServoOn(Int16 cardIndex, Int16 axNo, Int16 count = 1);

        /// @brief  关闭伺服
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_ServoOff", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_ServoOff(Int16 cardIndex, Int16 axNo, Int16 count = 1);

        /// @brief  停止单轴轴运动
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_StopMove", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_StopMove(Int16 cardIndex, Int16 axNo, Int16 stopType);

        /// @brief  按位停止单轴轴运动
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_StopMoveBits", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_StopMoveBits(Int16 cardIndex, UInt32 axMask, UInt32 stopTypeBits);

        /// @brief  设置轴的触发停止参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetAxStopTrigPara", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetAxStopTrigPara(Int16 cardIndex, Int16 axNo, Int16 uselessFlag, Int16 bitNo, Int16 stopType, Int16 diType);

        /// @brief  获取轴的触发停止参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetAxStopTrigPara", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetAxStopTrigPara(Int16 cardIndex, Int16 axNo, ref Int16 pUselessFlag, ref Int16 pBitNo, ref Int16 pStopType, ref Int16 pDiType);

        /// @brief  设置当前轴位置为用户指定位置
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetAxCurPos", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetAxCurPos(Int16 cardIndex, Int16 axNo, double setPos);

        /// @brief  同步轴位置, 将轴的规划位置与编码器位置同步
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SyncAxPos", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SyncAxPos(Int16 cardIndex, Int16 axNo);
        #endregion
        #endregion

        ///@defgroup MotionControl MotionControl
        ///@brief 板卡运动模式
        #region MotionControl


        ///@defgroup MotionPara MotionPara
        ///@brief 全局运动参数相关接口
        #region MotionPara

        /// @brief  设置单轴运动参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetAxMvPara", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetAxMvPara(Int16 cardIndex, Int16 axNo, double vel, double acc, double dec);

        /// @brief  获取单轴运动参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetAxMvPara", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetAxMvPara(Int16 cardIndex, Int16 axNo, ref double pVel, ref double pAcc, ref double pDec);

        /// @brief  设定单轴速度规划类型
        /// @details ratio 越大, 则 S 越接近 T 型速度规划, 冲击也越大; 反之, ratio 越小, 则规划越平顺, 冲击越小
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetAxVelType", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetAxVelType(Int16 cardIndex, Int16 axNo, Int16 velType, double ratio);

        /// @brief  获取单轴速度规划类型
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetAxVelType", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetAxVelType(Int16 cardIndex, Int16 axNo, ref Int16 pVelType, ref double pRatio);
        #endregion

        ///@defgroup BasicMotion BasicMotion
        ///@brief 基础运动功能
        #region BasicMotion


        ///@defgroup EcatAxisHoming EcatAxisHoming
        ///@brief 板卡ECAT轴402回零功能功能
        #region EcatAxisHoming

        /// @brief  设置EtherCAT类型轴402回零参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetEcatHomingPara", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetEcatHomingPara(Int16 cardIndex, Int16 axNo, Int16 method, UInt32 highVel, UInt32 lowVel, UInt32 acc, Int32 offset);

        /// @brief  将EtherCAT类型轴设置为回零模式
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetEcatHomingMode", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetEcatHomingMode(Int16 cardIndex, Int16 axNo);

        /// @brief  控制EtherCAT类型轴开始402回零
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_StartEcatHoming", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_StartEcatHoming(Int16 cardIndex, Int16 axNo);

        /// @brief  控制EtherCAT类型轴停止402回零
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_StopEcatHoming", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_StopEcatHoming(Int16 cardIndex, Int16 axNo);

        /// @brief  控制EtherCAT类型轴退出回零模式
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_ExitEcatHomingMode", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_ExitEcatHomingMode(Int16 cardIndex, Int16 axNo);

        /// @brief  获取EtherCAT类型轴的402回零状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetHomingStatus", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetHomingStatus(Int16 cardIndex, Int16 axNo, ref Int16 pStatus);

        /// @brief  EtherCAT类型轴402回零合成函数, 包含设定回零参数, 切回零模式以及启动回零运动
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_StartHoming", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_StartHoming(Int16 cardIndex, Int16 axNo, ref THomingPara pHomingPara);

        /// @brief  完成402回零动作合成函数, 包含停止回零, 退出ecat回零模式, 暂默认切到CSP模式
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_FinishHoming", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_FinishHoming(Int16 cardIndex, Int16 axNo);
        #endregion

        ///@defgroup EcatCSPHoming EcatCSPHoming
        ///@brief 板卡ECAT轴CSP模式规划回零功能功能
        #region EcatCSPHoming

        /// @brief  设置 CSP 模式下回原点中 DI 触发对应 0x60FD(digitalInput)中的 bit 位置
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetEcatAxProbeMaskBit", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetEcatAxProbeMaskBit(Int16 cardIndex, Int16 axNo, Int16 prbDiBitNo);

        /// @brief  获取 CSP 模式下回原点中 DI 触发对应 0x60FD(digitalInput)中的 bit 位置
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatAxProbeMaskBit", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatAxProbeMaskBit(Int16 cardIndex, Int16 axNo, ref Int16 pPrbDiBitNo);

        /// @brief  开始EtherCAT类型轴CSP模式规划回零
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_StartEcatAxCSPHoming", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_StartEcatAxCSPHoming(Int16 cardIndex, Int16 axNo, Int16 method, Int16 dir, Int16 level, double hVel, double lVel, double acc, double offset);

        /// @brief  停止EtherCAT类型轴CSP模式规划回零
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_StopEcatAxCSPHoming", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_StopEcatAxCSPHoming(Int16 cardIndex, Int16 axNo, Int16 stopMode);

        /// @brief  获取EtherCAT类型轴CSP模式规划回零状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEcatAxCSPHomingSts", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEcatAxCSPHomingSts(Int16 cardIndex, Int16 axNo, ref Int16 pHomingMethod, ref Int16 pHomingState);

        /// @brief  完成EtherCAT类型轴CSP模式规划回零
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_FinishEcatAxCSPHoming", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_FinishEcatAxCSPHoming(Int16 cardIndex, Int16 axNo);
        #endregion

        ///@defgroup PTP PTP
        ///@brief PTP运动模式相关接口
        #region PTP

        /// @brief  启动单轴PTP运动
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_StartPtpMove", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_StartPtpMove(Int16 cardIndex, Int16 axNo, double tgtPos, Int16 posType = 0);

        /// @brief  启动多轴PTP的运动
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_StartMultiPtpMove", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_StartMultiPtpMove(Int16 cardIndex, Int16 axNum, Int16[] pAxNoArray, double[] pTgtPosArray, Int16[] pPosTypeArray);

        /// @brief  在线更新PTP目标位置
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_UpdatePtpTgtPos", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_UpdatePtpTgtPos(Int16 cardIndex, Int16 axNo, double tgtPos);

        /// @brief  获取PTP目标位置
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetPtpTgtPos", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetPtpTgtPos(Int16 cardIndex, Int16 axNo, ref double pTgtPos);

        /// @brief  在线更新PTP运动参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_UpdatePtpMvPara", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_UpdatePtpMvPara(Int16 cardIndex, Int16 axNo, double tgtVel, double acc, double dec);

        /// @brief  在线更新PTP目标速度
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_UpdatePtpTgtVel", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_UpdatePtpTgtVel(Int16 cardIndex, Int16 axNo, double tgtVel);

        /// @brief  切换PTP规划模式
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_PtpPrf", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_PtpPrf(Int16 cardIndex, Int16 axNo);
        #endregion

        ///@defgroup Jog Jog
        ///@brief Jog运动模式相关接口
        #region Jog

        /// @brief  启动单轴JOG运动
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_StartJogMove", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_StartJogMove(Int16 cardIndex, Int16 axNo, double tgVel);

        /// @brief  启动多轴JOG运动
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_StartMultiJogMove", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_StartMultiJogMove(Int16 cardIndex, Int16 axNum, Int16[] pAxNoArray, double[] pTgtVelArray);

        /// @brief  在线更新JOG目标速度
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_UpdateJogTgtVel", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_UpdateJogTgtVel(Int16 cardIndex, Int16 axNo, double tgVel);

        /// @brief  在线更新JOG运动参数
        /// 注意：JOG规划减速度仅作用于JOG在线变速
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_UpdateJogMvPara", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_UpdateJogMvPara(Int16 cardIndex, Int16 axNo, double tgVel, double acc, double dec);

        /// @brief  切换JOG规划模式
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_JogPrf", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_JogPrf(Int16 cardIndex, Int16 axNo);
        #endregion

        ///@defgroup PTPS PTPS
        ///@brief PTPS运动模式相关接口
        #region PTPS
        /// @brief  切换PTPS规划模式
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_PrfPtps", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_PrfPtps(Int16 cardIndex, Int16 axNo);

        /// @brief  设置PTPS运动启动速度
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetPtpsBeginVel", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetPtpsBeginVel(Int16 cardIndex, Int16 axNo, double beginVel);

        /// @brief  设置PTPS运动结束速度
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetPtpsEndVel", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetPtpsEndVel(Int16 cardIndex, Int16 axNo, double endVel);

        /// @brief  设置PTPS运动参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetPtpsMovePara", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetPtpsMovePara(Int16 cardIndex, Int16 axNo, double tgVel, double acc, double dec);

        /// @brief  设置PTPS运动参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetPtpsMoveParaEx", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetPtpsMoveParaEx(Int16 cardIndex, Int16 axNo, double tgtVel, double tgtAcc, double tgtDec, double prfAcc, double prfDec);

        /// @brief  启动单轴PTPS快速规划
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_StartPtpsMove", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_StartPtpsMove(Int16 cardIndex, Int16 axNo, double tgtPos, short posType);

        /// @brief  启动单轴PTPS软启动规划
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_StartPtpsMoveA", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_StartPtpsMoveA(Int16 cardIndex, Int16 axNo, double tgtPos, double startSlowPos, double startSlowVel, short posType);

        /// @brief  启动单轴PTPS软着陆规划
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_StartPtpsMoveD", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_StartPtpsMoveD(Int16 cardIndex, Int16 axNo, double tgtPos, double stopSlowPos, double stopSlowVel, short posType);

        /// @brief   启动单轴PTPS多段运动规划，可实现软启动及软着陆
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_StartPtpsMoveM", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_StartPtpsMoveM(Int16 cardIndex, Int16 axNo, double tgtPos, double startSlowPos, double startSlowVel, double stopSlowPos, double stopSlowVel, short posType);

        /// @brief  在线更新PTPS目标位置，无法实现软着陆
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_UpdatePtpsMovePos", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_UpdatePtpsMovePos(Int16 cardIndex, Int16 axNo, double tgtPos);

        /// @brief  在线更新PTPS目标速度，无法实现软着陆
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_UpdatePtpsMoveVel", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_UpdatePtpsMoveVel(Int16 cardIndex, Int16 axNo, double tgtVel);

        /// @brief  在线更新PTPS运动参数，无法实现软着陆
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_UpdatePtpsMovePara", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_UpdatePtpsMovePara(Int16 cardIndex, Int16 axNo, double tgVel, double acc, double dec);

        /// @brief  在线更新PTPS软着陆规划，实现软着陆
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_UpdatePtpsMoveD", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_UpdatePtpsMoveD(Int16 cardIndex, Int16 axNo, double tgtPos, double stopSlowPos, double stopSlowVel);

        /// @brief  获取PTPS运动启动速度
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetPtpsBeginVel", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetPtpsBeginVel(Int16 cardIndex, Int16 axNo, ref double pBeginVel);

        /// @brief  获取PTPS运动结束速度
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetPtpsEndVel", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetPtpsEndVel(Int16 cardIndex, Int16 axNo, ref double pEendVel);

        /// @brief  获取PTPS运动参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetPtpsMovePara", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetPtpsMovePara(Int16 cardIndex, Int16 axNo, ref double pTgtVel, ref double pAcc, ref double pDec);

        /// @brief  获取PTPS运动参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetPtpsMoveParaEx", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetPtpsMoveParaEx(Int16 cardIndex, Int16 axNo, double pTgtVel, double pTgtAcc, double pTgtDec, double tPrfAcc, double tPrfDec);

        #endregion

        ///@defgroup Gear Gear
        ///@brief Gear运动模式相关接口
        #region Gear

        /// @brief  切换为电子齿轮模式
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GearPrf", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GearPrf(Int16 cardIndex, Int16 axNo);

        /// @brief  设置电子齿轮参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GearSetParam", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GearSetParam(Int16 cardIndex, Int16 axNo, ref TGearParam pGearParam);

        /// @brief  获取电子齿轮参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GearGetParam", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GearGetParam(Int16 cardIndex, Int16 axNo, ref TGearParam pGearParam);

        /// @brief  启动电子齿轮运动
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GearStart", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GearStart(Int16 cardIndex, Int16 axNo);

        /// @brief  停止电子齿轮运动
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GearStop", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GearStop(Int16 cardIndex, Int16 axNo, Int16 stopType);

        /// @brief  获取电子齿轮状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GearGetStatus", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GearGetStatus(Int16 cardIndex, Int16 axNo, ref Int16 pStatus, ref Int16 pErr);

        /// @brief  更新电子齿轮比
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GearUpdateScale", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GearUpdateScale(Int16 cardIndex, Int16 axNo, Int32 masterScale, Int32 slaveScale, Int32 masterDis);
        #endregion

        ///@defgroup Pvt Pvt
        ///@brief Pvt运动模式相关接口
        #region Pvt

        /// @brief 切换为PVT模式
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_PvtPrf", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_PvtPrf(Int16 cardIndex, Int16 axNo);

        /// @brief 选择PVT数据表
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_PvtTableSelect", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_PvtTableSelect(Int16 cardIndex, Int16 axNo, Int16 tableId);

        /// @brief 启动PVT运动
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_PvtStart", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_PvtStart(Int16 cardIndex, Int16 axNum, Int16[] pAxArray);

        /// @brief 获取PVT状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_PvtGetStatus", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_PvtGetStatus(Int16 cardIndex, Int16 axNo, Int16[] pTableId, double[] pTime, Int16 count = 1);

        /// @brief 设置PVT循环
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_PvtSetLoop", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_PvtSetLoop(Int16 cardIndex, Int16 axNo, Int32 loop);

        /// @brief 获取PVT循环
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_PvtGetLoop", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_PvtGetLoop(Int16 cardIndex, Int16 axNo, ref Int32 pCurLoop, ref Int32 pLoop);

        /// @brief 向PVT的指定数据表传送PT数据
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_PvtTablePt", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_PvtTablePt(Int16 cardIndex, Int16 tableId, Int32 count, double[] P, double[] T);

        /// @brief 向PVT的指定数据表传送数据, 采用PVT描述方式
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_PvtTablePvt", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_PvtTablePvt(Int16 cardIndex, Int16 tableId, Int32 count, double[] P, double[] V, double[] T);

        /// @brief 向PVT的指定数据表传送数据, 采用Percent描述方式(1段Percent描述方式消耗1~3个缓存空间)
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_PvtTablePercent", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_PvtTablePercent(Int16 cardIndex, Int16 tableId, Int32 count, double[] P, double[] T, double[] percent, double startVel = 0);

        /// @brief 向PVT的指定数据表传送数据, 采用Complete描述方式
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_PvtTableComplete", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_PvtTableComplete(Int16 cardIndex, Int16 tableId, Int32 count, double[] P, double[] T, double startVel = 0, double endVel = 0);
        #endregion


        ///@defgroup Cam Cam
        ///@brief Cam运动模式相关接口
        #region Cam
        /// @brief 设置凸轮规划模式
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CamPrf", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CamPrf(Int16 cardIndex, Int16 axNo);
        /// @brief 凸轮参数设置
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CamSetParam", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CamSetParam(Int16 cardIndex, Int16 axNo, Int16 masterType, Int16 masterNo, Int16 dirMode = 0);
        /// @brief 凸轮参数获取
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CamGetParam", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CamGetParam(Int16 cardIndex, Int16 axNo, ref Int16 pMasterType, ref Int16 pMasterNo, ref Int16 pDirMode);
        /// @brief 凸轮表选择
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CamTableSelect", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CamTableSelect(Int16 cardIndex, Int16 axNo, Int16 tableId);
        /// @brief 启动凸轮运动
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CamStart", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CamStart(Int16 cardIndex, Int16 axNo);
        /// @brief 停止凸轮运动
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CamStop", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CamStop(Int16 cardIndex, Int16 axNo, Int16 stopType);
        /// @brief 获取凸轮状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CamGetStatus", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CamGetStatus(Int16 cardIndex, Int16 axNo, ref Int16 pStatus, ref Int16 pTableId, ref Int32 pCurUserID);
        /// @brief 获取凸轮在表中的主轴位置和从轴位置
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CamGetCurPos", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CamGetCurPos(Int16 cardIndex, Int16 axNo, ref double pMasterPos, ref double pSlavePos);
        /// @brief 设置凸轮循环次数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CamSetLoop", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CamSetLoop(Int16 cardIndex, Int16 axNo, UInt32 loop);
        /// @brief 获取凸轮循环次数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CamGetLoop", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CamGetLoop(Int16 cardIndex, Int16 axNo, ref UInt32 pCurLoop, ref UInt32 pLoop);
        /// @brief 设置凸轮启动方式
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CamSetEvent", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CamSetEvent(Int16 cardIndex, Int16 axNo, Int16 type, Int32 masterPos);
        /// @brief 获取凸轮启动方式
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CamGetEvent", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CamGetEvent(Int16 cardIndex, Int16 axNo, ref Int16 pType, ref Int32 pMasterPos);
        /// @brief 主轴相位偏移(由速度参数指定)
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CamPhasing", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CamPhasing(Int16 cardIndex, Int16 axNo, double phaseShift, double phaseVel, double phaseAcc, double phaseDec, Int16 phaseShiftType = 0);
        /// @brief 主轴相位偏移(指定主轴位移完成偏移)
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CamPhasingDist", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CamPhasingDist(Int16 cardIndex, Int16 axNo, double phaseShift, double masterDist, double accDistRatio, double decDistRatio, Int16 phaseShiftType = 0);
        /// @brief 停止主轴相位偏移
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CamStopPhasing", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CamStopPhasing(Int16 cardIndex, Int16 axNo);
        /// @brief 获取主轴相位偏移状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CamGetPhasing", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CamGetPhasing(Int16 cardIndex, Int16 axNo, ref Int16 phasingSts, ref double pPhase, ref double pPhaseVel);
        /// @brief 清除凸轮表数据
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CamTableClrData", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CamTableClrData(Int16 cardIndex, Int16 tableId);
        /// @brief 获取凸轮表剩余空间
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CamTableGetSpace", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CamTableGetSpace(Int16 cardIndex, Int16 tableId, ref Int32 pSpace);
        /// @brief 重新分配凸轮表数量和大小(总深度为8*2048)
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CamTableResize", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CamTableResize(Int16 cardIndex, Int16 tableNum);
        /// @brief 获取凸轮表数量和大小信息
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CamTableGetInfo", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CamTableGetInfo(Int16 cardIndex, ref Int16 pTableNum, ref Int32 pTableSize);
        /// @brief 凸轮表数据压入指令-通用压入指令
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CamTableData", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CamTableData(Int16 cardIndex, Int16 tableId, Int32 count, Int16[] pCurveType, double[] pKeyPointParam, Int32[] pUserID = null);
        /// @brief 凸轮表获取关键点之间指定相位的位移
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CamTableGetDist", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CamTableGetDist(Int16 cardIndex, Int32 count, Int16[] pCurveType, double[] pKeyPointParam, double x, double[] pOutput);
        #endregion

        #endregion

        ///@defgroup AdvMotion AdvMotion
        ///@brief 高级运动功能
        #region AdvMotion

        ///@defgroup Crd Crd
        ///@brief Crd坐标系插补运动模式相关接口
        #region Crd

        /// @brief  创建插补坐标系, 配置坐标系插补参数
        /// @attention 注意！用户必须定义长度为 3 的数组进行传值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdSetMtSys", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdSetMtSys(Int16 cardIndex, Int16 crdNo, Int16[] pMaskAxNoArray, Int16 lookAheadLen, double estopDec);

        /// @brief  获取插补坐标系配置参数
        /// @attention 注意！用户必须定义长度为 3 的数组进行传值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdGetMtSysParam", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdGetMtSysParam(Int16 cardIndex, Int16 crdNo, Int16[] pMaskAxNoArray, ref Int16 pLookAheadLen, ref double pEstopDec);

        /// @brief  删除插补坐标系
        /// @attention 需对创建的坐标系进行删除, 否则接口报错
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdDeleteMtSys", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdDeleteMtSys(Int16 cardIndex, Int16 crdNo);

        /// @brief  设置插补坐标系高级参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdSetAdvParam", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdSetAdvParam(Int16 cardIndex, Int16 crdNo, ref TCrdAdvParam pCrdAdvParam);

        /// @brief  获取插补坐标系高级参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdGetAdvParam", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdGetAdvParam(Int16 cardIndex, Int16 crdNo, ref TCrdAdvParam pCrdAdvParam);

        /// @brief  插补运动指令用户模式下的运动段的始末速度设置
        /// @details 该指令主要用于用户规划模式下, 指定每段运动, 段的起始和段末的速度。一般情况下, 运动段的起始速度等于上一段的段末速度
        /// @todo   插补模式用户速度功能待完成, 速度参数范围待补充
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdSetUserVelPlan", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdSetUserVelPlan(Int16 cardIndex, Int16 crdNo, double uStartVel, double uEndVel);

        /// @brief  插补运动指令在用户规划模式下设置的运动段的始末速度的获取
        /// @todo   插补模式用户速度功能待完成, 速度参数范围待补充
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdGetUserVelPlan", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdGetUserVelPlan(Int16 cardIndex, Int16 crdNo, ref double pUStartVel, ref double pUEndVel);

        /// @brief  设置插补平滑参数。通过设置平滑等级和平滑精度, 使插补运动轨迹更平滑
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdSetSmoothParam", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdSetSmoothParam(Int16 cardIndex, Int16 crdNo, Int32 smoothLevel, double smoothTol);

        /// @brief  获取插补平滑参数。通过设置平滑等级和平滑精度, 使插补运动轨迹更平滑
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdGetSmoothParam", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdGetSmoothParam(Int16 cardIndex, Int16 crdNo, ref Int32 pSmoothLevel, ref double pSmoothTol);

        /// @brief  设置插补轨迹速度
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdSetTrajVel", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdSetTrajVel(Int16 cardIndex, Int16 crdNo, double tgtVel);

        /// @brief  获取插补轨迹速度
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdGetTrajVel", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdGetTrajVel(Int16 cardIndex, Int16 crdNo, ref double pTgtVel);

        /// @brief  设置插补轨迹加速度和减速度
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdSetTrajAccAndDec", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdSetTrajAccAndDec(Int16 cardIndex, Int16 crdNo, double tgtAcc, double tgtDec);

        /// @brief  获取插补轨迹加速度和减速度
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdGetTrajAccAndDec", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdGetTrajAccAndDec(Int16 cardIndex, Int16 crdNo, ref double pTgtAcc, ref double pTgtDec);

        /// @brief  设置插补强制规划末速度降为 0 标识
        /// @details 如果将强制规划末速度降为 0, 则下面所有线段末速度为 0
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdSetZeroFlag", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdSetZeroFlag(Int16 cardIndex, Int16 crdNo, Int16 ZeroFlag);

        /// @brief  获取插补强制规划末速度降为 0 标识
        /// @details 如果将强制规划末速度降为 0, 则下面所有线段末速度为 0
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdGetZeroFlag", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdGetZeroFlag(Int16 cardIndex, Int16 crdNo, ref Int16 pZeroFlag);

        /// @brief  设置插补运动指令的位置编程模式
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdSetIncMode", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdSetIncMode(Int16 cardIndex, Int16 crdNo, Int16 mode);

        /// @brief  获取插补运动指令的位置编程模式
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdGetIncMode", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdGetIncMode(Int16 cardIndex, Int16 crdNo, ref Int16 pMode);

        /// @brief  设置插补同步跟随轴的速度模式
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdSetFolVelMode", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdSetFolVelMode(Int16 cardIndex, Int16 crdNo, Int16 mode);

        /// @brief  获取插补同步跟随轴的速度模式
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdGetFolVelMode", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdGetFolVelMode(Int16 cardIndex, Int16 crdNo, ref Int16 pMode);

        /// @brief  设置插补运动指令的过渡精度
        /// @details 该指令属于高级参数指令, 只有在启动过渡模式才生效
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdSetTrajTol", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdSetTrajTol(Int16 cardIndex, Int16 crdNo, double tol);

        /// @brief  设置插补运动指令的拐弯系数。
        /// @details 该指令属于高级参数指令, 只有在启动过渡模式才生效, 该参数越 小,  在过渡拐弯时速度降至越低, 参数越大, 则拐弯速度越高, 默认参数为 1
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdSetTrajTurnCoef", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdSetTrajTurnCoef(Int16 cardIndex, Int16 crdNo, double turnCoef);

        /// @brief  三维直线插补运动
        /// @attention[]]pEndPosArray 应传入长度为 3 的数组指针
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdLineXYZ", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdLineXYZ(Int16 cardIndex, Int16 crdNo, double[] pEndPosArray, Int32 userID = 0);

        /// @brief  XY平面内直线插补运动
        /// @attention[]]pEndPosArray 应传入长度为 2 的数组指针
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdLineXY", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdLineXY(Int16 cardIndex, Int16 crdNo, double[] pEndPosArray, Int32 userID = 0);

        /// @brief  XZ平面内直线插补运动
        /// @attention[]]pEndPosArray 应传入长度为 2 的数组指针
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdLineZX", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdLineZX(Int16 cardIndex, Int16 crdNo, double[] pEndPosArray, Int32 userID = 0);

        /// @brief  YZ平面内直线插补运动
        /// @attention[]]pEndPosArray 应传入长度为 2 的数组指针
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdLineYZ", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdLineYZ(Int16 cardIndex, Int16 crdNo, double[] pEndPosArray, Int32 userID = 0);

        /// @brief  给定三点的三维圆弧插补
        /// @details 如果输入三点共线会导致圆弧几何错误, 此函数只能画小于一圈的圆不能画整圆和多圈圆
        /// @attention pMidPos 、pEndPos 应传入长度为 3 的数组指针
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdArcThreePoint", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdArcThreePoint(Int16 cardIndex, Int16 crdNo, double[] pMidPosArray, double[] pEndPosArray, Int32 userID);

        /// @brief  给定圆心, 末点法向量的三维圆弧插补
        /// @details 当 height 不为 0 的时候可衍生为螺旋线插补。当起点和终点重合的时候几何参数错误。
        /// @attention[]]pCenterArray[] pEndPosArray[] pNormalArray 应传入长度为 3 的数组指针
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_Crd3DArcCenterNormal", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_Crd3DArcCenterNormal(Int16 cardIndex, Int16 crdNo, double[] pCenterArray, double[] pEndPosArray, double[] pNormalArray, double height = 0, Int32 turn = 0, Int32 userID = 0);

        /// @brief  给定圆弧半径, 圆弧末点和圆弧法向量的三维圆弧插补
        /// @details 当 height 不为 0 的时候可衍生为螺旋线插补。 此函数无法插补整圆
        /// @attention[]]pEndPosArray[] pNormalArray 应传入长度为 3 的数组指针
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_Crd3DArcRadiusNormal", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_Crd3DArcRadiusNormal(Int16 cardIndex, Int16 crdNo, double radius, double[] pEndPosArray, double[] pNormalArray, double height = 0, Int32 turn = 0, Int32 userID = 0);

        /// @brief  给定圆弧圆心, 圆弧角度和圆弧法向量的三维圆弧插补
        /// @details 当 height 不为 0 的时候可衍生为螺旋线插补。此函数可以画整圆
        /// @attention[]]pCenterArray[] pNormalArray 应传入长度为 3 的数组指针
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_Crd3DArcAngleNormal", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_Crd3DArcAngleNormal(Int16 cardIndex, Int16 crdNo, double[] pCenterArray, double angle, double[] pNormalArray, double height = 0, Int32 userID = 0);

        /// @brief  给定圆弧圆心和圆弧末点, XY平面内的圆弧插补
        /// @attention[]]pCenterArray[] pEndPosArray 应传入长度为 2 的数组指针
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdArcCenterXYPlane", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdArcCenterXYPlane(Int16 cardIndex, Int16 crdNo, double[] pCenterArray, double[] pEndPosArray, Int16 dir, double height = 0, Int32 turn = 0, Int32 userID = 0);

        /// @brief  给定圆弧圆心和圆弧末点, YZ平面内的圆弧插补
        /// @attention[]]pCenterArray[] pEndPosArray 应传入长度为 2 的数组指针
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdArcCenterYZPlane", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdArcCenterYZPlane(Int16 cardIndex, Int16 crdNo, double[] pCenterArray, double[] pEndPosArray, Int16 dir, double height = 0, Int32 turn = 0, Int32 userID = 0);

        /// @brief  给定圆弧圆心和圆弧末点, ZX平面内的圆弧插补
        /// @attention[]]pCenterArray[] pEndPosArray 应传入长度为 2 的数组指针
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdArcCenterZXPlane", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdArcCenterZXPlane(Int16 cardIndex, Int16 crdNo, double[] pCenterArray, double[] pEndPosArray, Int16 dir, double height = 0, Int32 turn = 0, Int32 userID = 0);

        /// @brief  给定圆弧半径和圆弧末点, XY平面内的圆弧插补
        /// @attention[]]pEndPosArray 应传入长度为 2 的数组指针
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdArcRadiusXYPlane", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdArcRadiusXYPlane(Int16 cardIndex, Int16 crdNo, double radius, double[] pEndPosArray, Int16 dir, double height = 0, Int32 turn = 0, Int32 userID = 0);

        /// @brief  给定圆弧半径和圆弧末点, YZ平面内的圆弧插补
        /// @attention[]]pEndPosArray 应传入长度为 2 的数组指针
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdArcRadiusYZPlane", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdArcRadiusYZPlane(Int16 cardIndex, Int16 crdNo, double radius, double[] pEndPosArray, Int16 dir, double height = 0, Int32 turn = 0, Int32 userID = 0);

        /// @brief  给定圆弧半径和圆弧末点, ZX平面内的圆弧插补
        /// @attention[]]pEndPosArray 应传入长度为 2 的数组指针
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdArcRadiusZXPlane", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdArcRadiusZXPlane(Int16 cardIndex, Int16 crdNo, double radius, double[] pEndPosArray, Int16 dir, double height = 0, Int32 turn = 0, Int32 userID = 0);

        /// @brief  给定圆弧圆心和和圆弧角度, XY平面内的圆弧插补
        /// @attention[]]pCenterArray 应传入长度为 2 的数组指针
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdArcAngleXYPlane", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdArcAngleXYPlane(Int16 cardIndex, Int16 crdNo, double[] pCenterArray, double angle, double height = 0, Int32 userID = 0);

        /// @brief  给定圆弧圆心和和圆弧角度, YZ平面内的圆弧插补
        /// @attention[]]pCenterArray 应传入长度为 2 的数组指针
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdArcAngleYZPlane", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdArcAngleYZPlane(Int16 cardIndex, Int16 crdNo, double[] pCenterArray, double angle, double height = 0, Int32 userID = 0);

        /// @brief  给定圆弧圆心和和圆弧角度, ZX平面内的圆弧插补
        /// @attention[]]pCenterArray 应传入长度为 2 的数组指针
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdArcAngleZXPlane", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdArcAngleZXPlane(Int16 cardIndex, Int16 crdNo, double[] pCenterArray, double angle, double height = 0, Int32 userID = 0);

        /// @brief  给定圆心, 末点法向量的三维涡旋线插补
        /// @details 当 height 不为 0 的时候可衍生为螺旋线插补。当起点和终点重合的时候几何参数错误。
        /// @attention[]]pCenterArray[] pEndPosArray[] pNormalArray 应传入长度为 3 的数组指针
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_Crd3DVortexCenterNormal", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_Crd3DVortexCenterNormal(Int16 cardIndex, Int16 crdNo, double[] pCenterArray, double[] pEndPosArray, double[] pNormalArray, double height = 0, Int32 turn = 0, Int32 userID = 0);

        /// @brief  给定涡旋线圆心和涡旋线末点, XY平面内的涡旋线插补
        /// @attention[]]pCenterArray[] pEndPosArray 应传入长度为 2 的数组指针
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdVortexCenterXYPlane", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdVortexCenterXYPlane(Int16 cardIndex, Int16 crdNo, double[] pCenterArray, double[] pEndPosArray, Int16 dir, double height = 0, Int32 turn = 0, Int32 userID = 0);

        /// @brief  给定涡旋线圆心和涡旋线末点, YZ平面内的涡旋线插补
        /// @attention[]]pCenterArray[] pEndPosArray 应传入长度为 2 的数组指针
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdVortexCenterYZPlane", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdVortexCenterYZPlane(Int16 cardIndex, Int16 crdNo, double[] pCenterArray, double[] pEndPosArray, Int16 dir, double height = 0, Int32 turn = 0, Int32 userID = 0);

        /// @brief  给定涡旋线圆心和涡旋线末点, ZX平面内的涡旋线插补
        /// @attention[]]pCenterArray[] pEndPosArray 应传入长度为 2 的数组指针
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdVortexCenterZXPlane", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdVortexCenterZXPlane(Int16 cardIndex, Int16 crdNo, double[] pCenterArray, double[] pEndPosArray, Int16 dir, double height = 0, Int32 turn = 0, Int32 userID = 0);


        /// @brief  插补缓冲区等待延时
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdWaitTime", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdWaitTime(Int16 cardIndex, Int16 crdNo, Int32 waitTime, Int32 userID = 0);

        /// @brief  插补缓冲区等待 DI
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdWaitDI", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdWaitDI(Int16 cardIndex, Int16 crdNo, Int16 diNO, Int16 diType, Int16 diLevel, Int32 userID = 0);

        /// @brief  插补缓冲区等待 DI
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdWaitTimeDI", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdWaitTimeDI(Int16 cardIndex, Int16 crdNo, Int16 diNO, Int16 diType, Int16 diLevel, Int32 waitTime, Int32 userID = 0);

        /// @brief  插补缓冲区输出 DO
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdSetDO", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdSetDO(Int16 cardIndex, Int16 crdNo, Int16 doNO, Int16 doType, Int16 doLevel, Int32 userID = 0);

        /// @brief  插补缓冲区指令, 执行到该段时, 以该段为起始, 在 waitPos 距离处输出指定的 DO 信号
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdSetDistanceDO", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdSetDistanceDO(Int16 cardIndex, Int16 crdNo, double waitPos, Int16 doNO, Int16 doType, Int16 doLevel, Int32 userID = 0);

        /// @brief  插补缓冲区指令, 执行到该段时, 以该段为起始, 延时设定时间, 输出指定的 DO 信号
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdSetTimeDO", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdSetTimeDO(Int16 cardIndex, Int16 crdNo, Int32 waitTime, Int16 doNO, Int16 doType, Int16 doLevel, Int32 userID = 0);

        /// @brief  插补缓冲区启动插补轴以外的轴 PTP 运动
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdPTPMove", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdPTPMove(Int16 cardIndex, Int16 crdNo, Int16 axNo, double tgtPos, double tgtVel, double acc, Int16 mvType, Int16 waitFlag, Int32 userID = 0);

        /// @brief  插补缓冲区中启动插补轴之外的轴同步运动(相对位移)
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdSyncMove", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdSyncMove(Int16 cardIndex, Int16 crdNo, Int16 axNo, double syncPos, Int32 userID = 0);

        /// @brief  插补缓冲区中启动插补轴之外的轴同步运动(绝对位移)
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdSyncMoveAbs", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdSyncMoveAbs(Int16 cardIndex, Int16 crdNo, Int16 axNo, double syncTgtPos, Int32 userID = 0);

        /// @brief  插补缓冲区等待上一条运动指令到位
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdWaitInPos", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdWaitInPos(Int16 cardIndex, Int16 crdNo, Int32 userID = 0);

        /// @brief  缓冲区插补轴外的多轴（两轴）同步运动
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdMultiSyncMove", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdMultiSyncMove(Int16 cardIndex, Int16 crdNo, Int16 axNum, Int16[] pAxNo, double[] pTgtPos, double tgtVel, double tgtAcc, Int16 contiFlag, Int32 userID = 0);

        /// @brief  插补缓存运动停止
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdBufStop", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdBufStop(Int16 cardIndex, Int16 crdNo, Int32 userID = 0);

        /// @brief  插补缓存区设置全局变量
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdSetUserVal", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdSetUserVal(Int16 cardIndex, Int16 crdNo, Int16 index, Int32 value, Int32 userID = 0);

        /// @brief  插补缓存区等待全局变量
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdWaitUserVal", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdWaitUserVal(Int16 cardIndex, Int16 crdNo, Int16 index, Int16 condition, Int32 compareValue, Int32 waitTime, Int32 userID = 0);

        /// @brief  一次性往 DSP 队列发送 PC 部分的队列数据
        /// @details 每次发送若干条, 如果完全发送完成, pIsFinished 标 识记为 1
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdEndData", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdEndData(Int16 cardIndex, Int16 crdNo, ref Int16 pIsFinished);

        /// @brief  插补运动使能函数, 调用此函数后, 会开始插补运动。如果停止插补运动, 需要调用停止运动函数 进行去插补使能
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdStart", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdStart(Int16 cardIndex, Int16 crdNo);

        /// @brief  多个插补坐标系同时运动使能函数, 调用此函数后, 会开始插补运动。如果停止插补运动, 需要调用停止运动函数 进行去插补使能
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdMultiStart", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdMultiStart(Int16 cardIndex, Int16[] pCrdNo, Int16 count);

        /// @brief  调用此函数进行插补停止, 用户可以选择正常平滑停止和急停。
        /// @details 正常平滑停止按照用户设定的轨迹加速度进行减速停止。急停是利用用户在建立坐标系设定的急停加速度进行停止, 停止后系统不会清空队列, 需要用户自己清空队列。
        /// @attention 急停停止后, 插补会报出急停错误号, 需要手动清除此错误才能再次运行
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdStop", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdStop(Int16 cardIndex, Int16 crdNo, Int16 stopType);

        /// @brief  清除缓存区压入曲线数据
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdClrData", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdClrData(Int16 cardIndex, Int16 crdNo);

        /// @brief  清除插补错误号。可以清除 IMC_CrdGetStatus 获取到的故障号
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdClrError", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdClrError(Int16 cardIndex, Int16 crdNo);

        /// @brief  插补倍率设定函数。插补实际进给速度 = 用户插补进给速度 × 倍率值。
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdSetRatio", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdSetRatio(Int16 cardIndex, Int16 crdNo, double ratio);

        /// @brief  插补倍率获取函数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdGetRatio", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdGetRatio(Int16 cardIndex, Int16 crdNo, ref double pRatio);

        /// @brief  插补状态获取。bit0-bit7 是插补运行状态。bi8-bit15 是插补故障状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdGetStatus", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdGetStatus(Int16 cardIndex, Int16 crdNo, ref Int16 pStatus);

        /// @brief  获取插补运动执行缓冲区最后一段的运动状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdGetArrivalSts", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdGetArrivalSts(Int16 cardIndex, Int16 crdNo, ref Int16 pSts);

        /// @brief  获取插补目标位置。目标位置指的是最后压入曲线的末点位置
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdGetTargetPos", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdGetTargetPos(Int16 cardIndex, Int16 crdNo, double[] pPosArray);

        /// @brief  获取插补暂停运动的位置。该位置一般用于当用户使用单轴移动过插补轴后, 需要返回继续加工时使用
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdGetPausePos", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdGetPausePos(Int16 cardIndex, Int16 crdNo, double[] pPosArray);

        /// @brief  当前插补坐标系坐标读取函数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdGetPos", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdGetPos(Int16 cardIndex, Int16 crdNo, double[] pCrdPosArray);

        /// @brief  插补轨迹速度读取函数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdGetVel", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdGetVel(Int16 cardIndex, Int16 crdNo, ref double pCrdVel);

        /// @brief  插补当前运动曲线用户索引获取
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdGetUserID", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdGetUserID(Int16 cardIndex, Int16 crdNo, ref Int32 pUserID);

        /// @brief  获取队列当前的剩余余量
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdGetSpace", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdGetSpace(Int16 cardIndex, Int16 crdNo, ref Int32 pSpace);

        /// @brief  获取 CPU 缓存队列当前的剩余空间
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdGetBufSpace", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdGetBufSpace(Int16 cardIndex, Int16 crdNo, ref Int32 pSpace);

        /// @brief  获取前瞻缓存队列当前的剩余空间
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdGetLookAheadSpace", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdGetLookAheadSpace(Int16 cardIndex, Int16 crdNo, ref Int32 pSpace);

        /// @brief  插补缓冲区模式设置
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdSetBufferMode", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdSetBufferMode(Int16 cardIndex, Int16 crdNo, Int16 bufferMode);

        /// @brief  获取插补缓冲区模式
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdGetBufferMode", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdGetBufferMode(Int16 cardIndex, Int16 crdNo, ref Int16 pBufferMode);

        /// @brief  插补数据恢复
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CrdSetDataRestore", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CrdSetDataRestore(Int16 cardIndex, Int16 crdNo);
        #endregion

        ///@defgroup MultiCrd MultiCrd
        ///@brief MultiCrd 多轴插补运动模式相关接口
        #region MultiCrd


        /// @brief  建立多轴插补系统
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_MultiSetupSys", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_MultiSetupSys(Int16 cardIndex, Int16 groupNo, Int16[] pAxNo, Int16 maxAxNum);

        /// @brief  销毁多轴插补系统
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_MultiDeleteSys", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_MultiDeleteSys(Int16 cardIndex, Int16 groupNo);

        /// @brief  启动多轴插补运动
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_MultiStart", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_MultiStart(Int16 cardIndex, Int16 groupNo);

        /// @brief  停止多轴插补运动
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_MultiStop", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_MultiStop(Int16 cardIndex, Int16 groupNo, Int16 stopType);

        /// @brief  获取多轴插补数据剩余空间
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_MultiGetSpace", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_MultiGetSpace(Int16 cardIndex, Int16 groupNo, ref Int32 pSpace);

        /// @brief  清除多轴插补系统错误状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_MultiClrData", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_MultiClrData(Int16 cardIndex, Int16 groupNo);

        /// @brief  清除多轴插补数据队列
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_MultiClrError", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_MultiClrError(Int16 cardIndex, Int16 groupNo);

        /// @brief  设置多轴插补位置类型
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_MultiSetPosType", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_MultiSetPosType(Int16 cardIndex, Int16 groupNo, Int16 posType);

        /// @brief  获取多轴插补位置类型
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_MultiGetPosType", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_MultiGetPosType(Int16 cardIndex, Int16 groupNo, ref Int16 pPosType);

        /// @brief  获取多轴插补当前段UserID
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_MultiGetUserID", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_MultiGetUserID(Int16 cardIndex, Int16 groupNo, ref Int32 pUserID);

        /// @brief  获取多轴插补系统运行状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_MultiGetSts", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_MultiGetSts(Int16 cardIndex, Int16 groupNo, ref Int16 pSts, ref Int16 pErrocode);

        /// @brief  获取多轴插补运动到位状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_MultiGetArrivalSts", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_MultiGetArrivalSts(Int16 cardIndex, Int16 groupNo, ref Int16 pArrivalSts);

        /// @brief  设置多轴插补系统运动速度倍率
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_MultiSetRatio", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_MultiSetRatio(Int16 cardIndex, Int16 groupNo, double ratio);

        /// @brief  获取多轴插补系统运动速度倍率
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_MultiGetRatio", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_MultiGetRatio(Int16 cardIndex, Int16 groupNo, ref double pRatio);

        /// @brief  获取多轴插补运动轨迹位置
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_MultiGetTrajPos", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_MultiGetTrajPos(Int16 cardIndex, Int16 groupNo, double[] pPos);

        /// @brief  获取多轴插补运动轨迹速度
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_MultiGetTrajVel", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_MultiGetTrajVel(Int16 cardIndex, Int16 groupNo, ref double pVel);

        /// @brief  多轴插补直线运动
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_MultiLineMove", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_MultiLineMove(Int16 cardIndex, Int16 groupNo, double[] pEndPos, double trajVel, double trajAcc, double trajDec, Int16 blendType, double blendRatio, Int32 userID = 0);

        /// @brief  多轴插补等待事件
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_MultiWaitTime", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_MultiWaitTime(Int16 cardIndex, Int16 groupNo, Int32 waitTime, Int32 userID);

        /// @brief  多轴插补等待DI输入事件
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_MultiWaitDI", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_MultiWaitDI(Int16 cardIndex, Int16 groupNo, Int16 diIndex, Int16 diType, Int16 diLevel, Int32 waitTime, Int32 userID);

        /// @brief  多轴插补设置DO输出事件
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_MultiSetDO", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_MultiSetDO(Int16 cardIndex, Int16 groupNo, Int16 doIndex, Int16 doType, Int16 doLevel, Int32 userID);

        /// @brief  多轴插补设置翻转DO输出事件
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_MultiSetReverseDO", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_MultiSetReverseDO(Int16 cardIndex, Int16 groupNo, Int16 doIndex, Int16 doType, Int16 doLevel, Int32 waitTime, Int32 userID);
        #endregion

        ///@defgroup BandPt BandPt
        ///@brief BandPt 缓冲区插补运动模式相关接口
        #region BandPt

        /// @brief  建立多轴捆绑 PT 运行系统
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetupPtPackSys", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetupPtPackSys(Int16 cardIndex, Int16 sysNo, Int16[] pMaskAxNoArray, Int16 maxAxNum);

        /// @brief  销毁捆绑 PT 运动系统
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_DeletePtPackSys", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_DeletePtPackSys(Int16 cardIndex, Int16 sysNo);

        /// @brief  设置捆绑 PT 运动位置的编程类型
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetPtPackIncMode", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetPtPackIncMode(Int16 cardIndex, Int16 sysNo, Int16 incMode);

        /// @brief  获取捆绑 PT 运动位置的编程类型
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetPtPackIncMode", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetPtPackIncMode(Int16 cardIndex, Int16 sysNo, ref Int16 pIncMode);

        /// @brief  使能捆绑 PT 运动数据断流保护。当使能后, 根据缓冲区是否有数据, 同时判断各轴的速度是否大 于设定的阈值。
        /// @details 如果系统中有任意一个轴的速度大于了设定的阈值, 则进行断流保护, 按照设定的 平滑停加速度进行停止。
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_EnablePtPackNoDataProtect", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_EnablePtPackNoDataProtect(Int16 cardIndex, Int16 sysNo, double[] pThresholdVelArray);

        /// @brief  取消捆绑 PT 运动数据断流保护
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_DisablePtPackNoDataProtect", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_DisablePtPackNoDataProtect(Int16 cardIndex, Int16 sysNo);

        /// @brief  获取捆绑 PT 运动数据断流保护的设置状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetPtPackNoDataProtectStatus", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetPtPackNoDataProtectStatus(Int16 cardIndex, Int16 sysNo, ref Int16 pEnSts, double[] pThresholdVelArray);

        /// @brief  添加捆绑 PT 运动数据
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_AddMotionPointPtPack", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_AddMotionPointPtPack(Int16 cardIndex, Int16 sysNo, double[] pPosArray, Int16[] pTypeArray, double T, Int16 dataNum);

        /// @brief  添加捆绑 PT 中的 DO 输出事件
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_AddDoPointPtPack", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_AddDoPointPtPack(Int16 cardIndex, Int16 sysNo, Int16 doNo, Int16 doType, Int16 doLevel);

        /// @brief  启动捆绑 PT 运动
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_StartPtPack", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_StartPtPack(Int16 cardIndex, Int16 sysNo);

        /// @brief  停止捆绑 PT 运动
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_StopPtPack", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_StopPtPack(Int16 cardIndex, Int16 sysNo, Int16 type);

        /// @brief  清除捆绑 PT 运动数据
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_ClrPtPackData", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_ClrPtPackData(Int16 cardIndex, Int16 sysNo);

        /// @brief  清除捆绑 PT 运动错误
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_ClrPtPackError", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_ClrPtPackError(Int16 cardIndex, Int16 sysNo);

        /// @brief  获取捆绑 PT 运动数据的空间
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetPtPackRestSpace", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetPtPackRestSpace(Int16 cardIndex, Int16 sysNo, ref Int16 pSpace);

        /// @brief  获取捆绑 PT 运动的状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetPtPackStatus", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetPtPackStatus(Int16 cardIndex, Int16 sysNo, ref Int16 pStatus);

        /// @brief  获取捆绑 PT 运动错误
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetPtPackError", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetPtPackError(Int16 cardIndex, Int16 sysNo, ref Int16 pErr);

        /// @brief  获取捆绑 PT 运动到位状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetPtPackArrivalSts", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetPtPackArrivalSts(Int16 cardIndex, Int16 sysNo, ref Int16 pSts);
        #endregion
        #endregion

        #endregion
        ///@defgroup Compensation Compensation
        ///@brief 运动补偿功能
        #region Compensation

        ///@defgroup ScrewComp ScrewComp
        ///@brief 螺距误差补偿
        #region ScrewComp
        /// @brief  设置螺距误差补偿表
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetAxScrewCompTable", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetAxScrewCompTable(Int16 cardIndex, Int16 tableId, Int16 cnt, Int32[] pPosCompArray, Int32[] pNegCompArray);

        /// @brief  获取螺距误差补偿表
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetAxScrewCompTable", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetAxScrewCompTable(Int16 cardIndex, Int16 tableId, ref Int16 pCnt, Int32[] pPosCompArray, Int32[] pNegCompArray);

        /// @brief  设置螺距误差补偿参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetAxScrewCompParam", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetAxScrewCompParam(Int16 cardIndex, Int16 axNo, Int16 pointCnt, Int32 startPos, Int32 len);

        /// @brief  获取螺距误差补偿参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetAxScrewCompParam", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetAxScrewCompParam(Int16 cardIndex, Int16 axNo, ref Int16 pPointCnt, ref Int32 pStartPos, ref Int32 pLen);

        /// @brief  使能螺距误差补偿
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_EnableAxScrewComp", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_EnableAxScrewComp(Int16 cardIndex, Int16 axNo, Int16 enable);

        /// @brief  获取螺距误差补偿状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetAxScrewCompSts", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetAxScrewCompSts(Int16 cardIndex, Int16 axNo, ref Int16 pEnable, ref double pCompVal);
        #endregion

        ///@defgroup TableComp TableComp
        ///@brief 表补偿功能
        #region TableComp

        /// @brief  设置表补偿的数据表
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_TableCompSetTable", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_TableCompSetTable(Int16 cardIndex, Int16 tableId, Int32 cnt, Int32[] pDataArray);

        /// @brief  获取表补偿的数据表
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_TableCompGetTable", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_TableCompGetTable(Int16 cardIndex, Int16 tableId, ref Int32 pCnt, Int32[] pDataArray);

        /// @brief  设置表补偿参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_TableCompSetParam", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_TableCompSetParam(Int16 cardIndex, Int16 axNo, ref TTableCompParam pTTableCompParam);

        /// @brief  获取表补偿参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_TableCompGetParam", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_TableCompGetParam(Int16 cardIndex, Int16 axNo, ref TTableCompParam pTTableCompParam);

        /// @brief  使能表补偿
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_TableCompEnable", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_TableCompEnable(Int16 cardIndex, Int16 axNo, Int16 enable);

        /// @brief  获取表补偿状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_TableCompGetSts", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_TableCompGetSts(Int16 cardIndex, Int16 axNo, ref Int16 pEnable, ref double pCompVal);

        /// @brief  重新分配表补偿的表数量和大小(总大小为40000)
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_TableCompTableResize", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_TableCompTableResize(Int16 cardIndex, Int16 tableNum);

        /// @brief  获取表补偿的表数量和大小信息
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_TableCompTableGetInfo", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_TableCompTableGetInfo(Int16 cardIndex, ref Int16 pTableNum, ref Int32 pTableSize);
        #endregion

        ///@defgroup AxPrfComp AxPrfComp
        ///@brief 轴规划补偿功能
        #region AxPrfComp

        /// @brief  设置轴规划补偿
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetAxCompPos", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetAxCompPos(Int16 cardIndex, Int16 axNo, double compPos, double compTime, Int16 posType);

        /// @brief  获取轴规划补偿状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetAxCompSts", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetAxCompSts(Int16 cardIndex, Int16 axNo, ref Int16 pSts, ref double pCompPos);


        #endregion
        #endregion

        ///@defgroup Virtual Resource
        ///@brief 板卡虚拟功能
        #region VirtualResource

        ///@defgroup UserVal
        ///@brief 全局变量功能
        #region UserVal

        /// @brief  设置全局变量
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetUserVal", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetUserVal(Int16 cardIndex, Int16 index, Int32 value);

        /// @brief  读取全局变量
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetUserVal", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetUserVal(Int16 cardIndex, Int16 index, ref Int32 pValue);

        #endregion
        #endregion

        ///@defgroup Sample Sample
        ///@brief 板卡采样功能相关接口
        #region Sample
        /// @brief  配置数据采集的参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_ConfigSamplePara", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_ConfigSamplePara(Int16 cardIndex, ref TSamplePara pSamplePara);

        /// @brief  配置数据采集的采集对象
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_ConfigSampleData", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_ConfigSampleData(Int16 cardIndex, Int16 count, Int16[] pDataTypeArray, Int32[] pDataIndexArray);

        /// @brief  使能数据采集
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_ConfigSampleEnable", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_ConfigSampleEnable(Int16 cardIndex, Int16 enable);

        /// @brief  获取数据采集的状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetSampleStatus", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetSampleStatus(Int16 cardIndex, ref Int16 pStatus, ref Int32 pLen, ref Int32 pLeakageCount);

        /// @brief  获取数据采集的速度
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetSampleData", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetSampleData(Int16 cardIndex, ref Int16 pPackNum, Int16[] pDataArray);
        #endregion

        ///@defgroup MultiAxCmp MultiAxCmp
        ///@brief 板卡多轴位置比较功能相关接口
        #region MultiAxCmp
        /// @brief  配置多轴比较输入源
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_MultiAxCmpSrcCfg", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_MultiAxCmpSrcCfg(Int16 cardIndex, Int16 groupNo, Int16 cmpDimNum, Int16 cmpSrcType, Int16[] pCmpSrcArray, Int32 errorLmt);

        /// @brief  使能多轴比较
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_MultiAxCmpEnable", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_MultiAxCmpEnable(Int16 cardIndex, Int16 groupNo, Int16 enable);

        /// @brief  压入多轴比较数据
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_MultiAxCmpPushData", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_MultiAxCmpPushData(Int16 cardIndex, Int16 groupNo, Int16 cmpDimNum, Int16 eventIndex, Int32[] pCmpPosArray, ref Int16 pSpace);

        /// @brief  清除多轴比较缓冲区数据
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_MultiAxCmpClrData", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_MultiAxCmpClrData(Int16 cardIndex, Int16 groupNo);


        /// @brief  获取多轴比较剩余空间
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_MultiAxCmpGetSpace", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_MultiAxCmpGetSpace(Int16 cardIndex, Int16 groupNo, ref Int16 pSpace);


        /// @brief  获取多轴比较状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_MultiAxCmpGetSts", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_MultiAxCmpGetSts(Int16 cardIndex, Int16 groupNo, ref Int16 pStatus, ref Int16 pCmpCnt, ref Int16 pCmpIndex);
        #endregion

        ///@defgroup PLC PLC
        ///@brief 板卡PLC功能相关接口
        #region PLC
        /// @brief  编译参数结构体
        [StructLayout(LayoutKind.Sequential)]
        public struct TCompileInfo
        {
            public IntPtr pFileName;
            public IntPtr pLineNo;
            public IntPtr pMessage;
        };

        /// @brief  编译参数结构体
        [StructLayout(LayoutKind.Sequential)]
        public struct TVarInfo
        {
            public Int16 id;
            public Int16 dataType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] name;
            //public string name;
        };

        /// @brief  状态参数结构体
        [StructLayout(LayoutKind.Sequential)]
        public struct TThreadSts
        {
            public Int16 run;
            public Int16 error;
            public double result;
            public Int16 line;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct TBindDi
        {
            public Int16 diType;
            public Int16 index; //绑定索引从1 开始
            public Int16 reverse;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct TBindDo
        {
            public Int16 doType;
            public Int16 index; //绑定索引从1 开始
            public Int16 reverse;
        };
        [StructLayout(LayoutKind.Sequential)]
        public struct TBindTimer
        {
            public Int16 timerType;  // 0 TT 1:TF 2:TTF
            public Int32 delay;   // 单位：ms
            public Int16 inputVarId; //输入控制变量
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct TBindCounter
        {
            public Int16 counterType;   //计数器类型：0-等于输出1 1-小于等于输出1 2-大于等于输出1
            public Int16 edge;  //计数沿： 0-上升沿 1-下降沿 2-双边沿
            public Int32 init;    //计数初值：复位变量为1时，内部计数恢复初值，（起点，终点）
            public Int32 target;  //计数器目标值
            public Int32 begin;   //计数器起点
            public Int32 end;     //计数器终点
            public Int16 dir;   //计数方向： 1-正向计数 -1-负向计数
            public Int32 unit;    //计数单位：计数器累加1次需要输入端出现边沿的次数
            public Int16 inputVarId;    //计数器输入变量
            public Int16 resetVarId;    //计数器复位变量
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct TBindFlank
        {
            public Int16 flankType;     //边沿触发器类型：0-下降沿触发 1-上升沿触发 2-双边沿触发
            public Int16 inputVarId;    //边沿触发器输入变量，触发后保持到执行周期结束
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct TBindSrff
        {
            public Int16 setVarId;      //置位变量 置位为1，输出1
            public Int16 resetVarId;    //复位变量 复位为1，置位为0，输出0
        };


        /// @brief  编译
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_Compile", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_Compile(string pFileName, ref TCompileInfo pWrongInfo);

        /// @brief  下载
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_Download", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_Download(Int16 cardIndex, string pFileName);

        /// @brief  获取函数id
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetFunId", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetFunId(Int16 cardIndex, string pFunName, ref Int16 pFunId);

        /// @brief  获取变量id
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetVarId", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetVarId(Int16 cardIndex, string pFunName, string pVarName, ref TVarInfo pVarInfo);

        /// @brief  绑定线程
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_Bind", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_Bind(Int16 cardIndex, Int16 thread, Int16 funId, Int16 page);

        /// @brief  运行线程
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_RunThread", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_RunThread(Int16 cardIndex, Int16 thread);

        /// @brief  按周期运行线程
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_RunThreadPeriod", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_RunThreadPeriod(Int16 cardIndex, Int16 thread, Int16 period, Int16 priority);

        /// @brief  按断点运行线程
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_RunThreadToBreakpoint", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_RunThreadToBreakpoint(Int16 cardIndex, Int16 thread, Int16 line);

        /// @brief  单步运行线程
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_StepThread", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_StepThread(Int16 cardIndex, Int16 thread);

        /// @brief  停止线程
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_StopThread", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_StopThread(Int16 cardIndex, Int16 thread);

        /// @brief  暂停线程
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_PauseThread", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_PauseThread(Int16 cardIndex, Int16 thread);

        /// @brief  获取线程状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetThreadSts", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetThreadSts(Int16 cardIndex, Int16 thread, ref TThreadSts pThreadSts);

        /// @brief  获取线程运行周期
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetThreadTime", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetThreadTime(Int16 cardIndex, Int16 thread, ref Int16 pPeriod, ref UInt32 pExecuteTime, ref UInt32 pExecuteTimeMax);

        /// @brief  获取线程运行时间
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetThreadRunTime", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetThreadRunTime(Int16 cardIndex, Int16 thread, ref UInt32 pExecuteTime, ref UInt32 pExecuteTimeMax);

        /// @brief  设置变量值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetVarValue", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetVarValue(Int16 cardIndex, Int16 page, ref TVarInfo pVarInfo, double[] pValue, Int16 count);

        /// @brief  获取变量值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetVarValue", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetVarValue(Int16 cardIndex, Int16 page, ref TVarInfo pVarInfo, double[] pValue, Int16 count);



        /// @brief  解除绑定
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_UnbindVar", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_UnbindVar(Int16 cardIndex, Int16 thread);

        /// @brief  绑定DI
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_BindDi", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_BindDi(Int16 cardIndex, Int16 thread, ref TVarInfo pVarInfo, ref TBindDi pBindDi);

        /// @brief  绑定DO
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_BindDo", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_BindDo(Int16 cardIndex, Int16 thread, ref TVarInfo pVarInfo, ref TBindDo pBindDo);

        /// @brief  绑定Timer
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_BindTimer", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_BindTimer(Int16 cardIndex, Int16 thread, ref TVarInfo pVarInfo, ref TBindTimer pBindTimer);

        /// @brief  绑定计数器
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_BindCounter", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_BindCounter(Int16 cardIndex, Int16 thread, ref TVarInfo pVarInfo, ref TBindCounter pBindCounter);

        /// @brief  绑定触发器Flank
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_BindFlank", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_BindFlank(Int16 cardIndex, Int16 thread, ref TVarInfo pVarInfo, ref TBindFlank pBindFlank);

        /// @brief  绑定置位器
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_BindSrff", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_BindSrff(Int16 cardIndex, Int16 thread, ref TVarInfo pVarInfo, ref TBindSrff pBindSrff);



        /// @brief  获取绑定DI输入状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetBindDi", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetBindDi(Int16 cardIndex, ref TVarInfo pVarInfo, ref TBindDi pBindDi);

        /// @brief  获取绑定DO输出状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetBindDo", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetBindDo(Int16 cardIndex, ref TVarInfo pVarInfo, ref TBindDo pBindDo);

        /// @brief  获取绑定Timer输出状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetBindTimer", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetBindTimer(Int16 cardIndex, ref TVarInfo pVarInfo, ref TBindTimer pBindTimer, ref Int32 pCount);

        /// @brief  获取绑定计数器输出状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetBindCounter", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetBindCounter(Int16 cardIndex, ref TVarInfo pVarInfo, ref TBindCounter pBindCounter, ref Int32 pUnitCount, ref Int32 pCount);

        /// @brief  获取绑定触发器Flank输出状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetBindFlank", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetBindFlank(Int16 cardIndex, ref TVarInfo pVarInfo, ref TBindFlank pBindFlank);

        /// @brief  获取绑定置位器Srff输出状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetBindSrff", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetBindSrff(Int16 cardIndex, ref TVarInfo pVarInfo, ref TBindSrff pBindSrff);


        /// @brief  获取绑定Di数量
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetBindDiCount", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetBindDiCount(Int16 cardIndex, Int16 thread, ref Int16 pCount);

        /// @brief  获取绑定Do数量
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetBindDoCount", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetBindDoCount(Int16 cardIndex, Int16 thread, ref Int16 pCount);

        /// @brief  获取绑定Timer数量
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetBindTimerCount", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetBindTimerCount(Int16 cardIndex, Int16 thread, ref Int16 pCount);

        /// @brief  获取绑定Counter数量
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetBindCounterCount", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetBindCounterCount(Int16 cardIndex, Int16 thread, ref Int16 pCount);

        /// @brief  获取绑定Flank数量
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetBindFlankCount", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetBindFlankCount(Int16 cardIndex, Int16 thread, ref Int16 pCount);

        /// @brief  获取绑定Srff数量
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetBindSrffCount", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetBindSrffCount(Int16 cardIndex, Int16 thread, ref Int16 pCount);


        /// @brief  获取绑定DI信息
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetBindDiInfo", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetBindDiInfo(Int16 cardIndex, Int16 thread, Int16 index, ref Int16 pVar, ref TBindDi pBindDi);

        /// @brief  获取绑定DO信息
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetBindDoInfo", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetBindDoInfo(Int16 cardIndex, Int16 thread, Int16 index, ref Int16 pVar, ref TBindDo pBindDo);

        /// @brief  获取绑定Timer信息
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetBindTimerInfo", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetBindTimerInfo(Int16 cardIndex, Int16 thread, Int16 index, ref Int16 pVar, ref TBindTimer pBindTimer);

        /// @brief  获取绑定计数器信息
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetBindCounterInfo", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetBindCounterInfo(Int16 cardIndex, Int16 thread, Int16 index, ref Int16 pVar, ref TBindCounter pBindCounter);

        /// @brief  获取绑定触发器Flank信息
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetBindFlankInfo", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetBindFlankInfo(Int16 cardIndex, Int16 thread, Int16 index, ref Int16 pVar, ref TBindFlank pBindFlank);

        /// @brief  获取绑定置位器Srff信息
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetBindSrffInfo", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetBindSrffInfo(Int16 cardIndex, Int16 thread, Int16 index, ref Int16 pVar, ref TBindSrff pBindSrff);
        #endregion

        ///@defgroup PulseDO PulseDO
        ///@brief 板卡脉冲DO输出功能相关接口
        #region PulseDO
        /// @brief  设置脉冲DO输出映射
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetPulseDoMap", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetPulseDoMap(Int16 cardIndex, Int16 index, Int16 doIndex);

        /// @brief   获取脉冲DO输出映射
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetPulseDoMap", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetPulseDoMap(Int16 cardIndex, Int16 index, ref Int16 pDoIndex);

        /// @brief  设置脉冲DO输出通道参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetPulseDoOutputParam", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetPulseDoOutputParam(Int16 cardIndex, Int16 index, UInt16 highLevelTime, UInt16 lowLevTime, Int16 firstLevel, Int16 pulseNum);

        /// @brief  使能脉冲DO输出
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_EnablePulseDo", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_EnablePulseDo(Int16 cardIndex, Int16 index, UInt16 highLevelTime, UInt16 lowLevTime, Int16 firstLevel, Int16 pulseNum);


        /// @brief  停止脉冲DO输出
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_DisablePulseDo", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_DisablePulseDo(Int16 cardIndex, Int16 index);

        /// @brief  获取脉冲DO输出状态
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetPulseDoStatus", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetPulseDoStatus(Int16 cardIndex, Int16 index, ref Int16 pStatus);
        #endregion

        ///@defgroup Event Event
        ///@brief 板卡事件功能相关接口
        #region Event
        /// @brief  设置事件
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetEvent", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetEvent(Int16 cardIndex, Int16 eventIndex, ref TEvent pEvent);

        /// @brief  设置任务
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetTask", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetTask(Int16 cardIndex, Int16 taskIndex, Int16 taskType, IntPtr pTask);

        /// @brief  设置事件与任务连接
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_SetEventTaskLink", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_SetEventTaskLink(Int16 cardIndex, Int16 eventIndex, Int16[] taskIndexArray, Int16 count);

        /// @brief  事件开启
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_EventOn", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_EventOn(Int16 cardIndex, Int16 eventIndex, Int16 count = 1);

        /// @brief  事件关闭
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_EventOff", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_EventOff(Int16 cardIndex, Int16 eventIndex, Int16 count = 1);

        /// @brief  事件强制触发
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_EventForceTrigger", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_EventForceTrigger(Int16 cardIndex, Int16 eventIndex, Int16 count = 1);

        /// @brief  清除事件
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_ClearEvent", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_ClearEvent(Int16 cardIndex, Int16 eventIndex, Int16 count = 1);

        /// @brief  清除任务
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_ClearTask", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_ClearTask(Int16 cardIndex, Int16 taskIndex, Int16 count = 1);

        /// @brief  清除事件与任务的连接
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_ClearEventTaskLink", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_ClearEventTaskLink(Int16 cardIndex, Int16 eventIndex, Int16 count = 1);

        /// @brief  获取设置的事件数量
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEventCount", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEventCount(Int16 cardIndex, ref Int16 pCount);

        /// @brief  获取设置的事件
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEvent", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEvent(Int16 cardIndex, Int16 eventIndex, ref TEvent pEvent);

        /// @brief  获取事件的触发次数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEventTriggerCount", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEventTriggerCount(Int16 cardIndex, Int16 eventIndex, ref Int16 pTriggerCount);

        /// @brief  获取设置的任务数量
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetTaskCount", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetTaskCount(Int16 cardIndex, ref Int16 pCount);

        /// @brief  获取任务
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetTask", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetTask(Int16 cardIndex, Int16 taskIndex, ref Int16 pTaskType, IntPtr pTaskData);

        /// @brief  获取设置的事件与任务连接数量
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetEventTaskLinkCount", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetEventTaskLinkCount(Int16 cardIndex, Int16 eventIndex, ref Int16 pCount);

        /// @brief  获取任务执行结果
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_GetTaskResult", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_GetTaskResult(Int16 cardIndex, Int16 taskIndex, ref Int16 pResult);
        #endregion

        ///@defgroup CmdList CmdList
        ///@brief 板卡指令缓存输出功能相关接口
        #region CmdList

        /// @brief 启动缓存区指令执行
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CmdListStart", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CmdListStart(Int16 cardIndex, Int16 index, Int16 type);

        /// @brief 停止指令缓存区执行
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CmdListStop", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CmdListStop(Int16 cardIndex, Int16 index);

        /// @brief 设置指令缓存区运行模式
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CmdListSetMode", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CmdListSetMode(Int16 cardIndex, Int16 index, Int16 mode);

        /// @brief 获取指令缓存区运行模式
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CmdListGetMode", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CmdListGetMode(Int16 cardIndex, Int16 index, ref Int16 mode);



        /// @brief 获取指令缓存区运行状态、错误码和当前执行段号
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CmdListGetStatus", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CmdListGetStatus(Int16 cardIndex, Int16 index, ref Int16 pStatus, ref Int16 pErrorCode, ref Int32 pCurrentId);

        /// @brief 获取指令缓存区剩余空间
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CmdListGetSpace", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CmdListGetSpace(Int16 cardIndex, Int16 index, ref Int16 pSpace);


        /// @brief 清除指令缓存区数据
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CmdListClearData", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CmdListClearData(Int16 cardIndex, Int16 index);

        /// @brief 清除指令缓存区错误
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CmdListClearError", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CmdListClearError(Int16 cardIndex, Int16 index);

        /// @brief 设置指令缓存区循环执行次数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CmdListSetLoop", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CmdListSetLoop(Int16 cardIndex, Int16 index, Int32 setCount);

        /// @brief 获取指令缓存区循环执行次数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CmdListGetLoop", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CmdListGetLoop(Int16 cardIndex, Int16 index, ref Int32 setCount, ref Int32 doneCount);


        /// @brief 缓存区运动指令，执行轴PTP运动
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CmdListAxMove", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CmdListAxMove(Int16 cardIndex, Int16 index, Int16 axNo, double tgtPos, Int32 userID);

        /// @brief 缓存区运动指令，在线更新ptp运动目标位置
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CmdListUpdateAxMovePos", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CmdListUpdateAxMovePos(Int16 cardIndex, Int16 index, Int16 axNo, double tgtPos, Int32 userID);

        /// @brief 缓存区运动指令，在线更新轴运动速度参数
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CmdListUpdateAxMovePara", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CmdListUpdateAxMovePara(Int16 cardIndex, Int16 index, Int16 axNo, double tgtVel, double tgtAcc, double tgtDec, Int32 userID);

        /// @brief 缓存区运动指令，执行轴ptp运动，目标位置来源为全局变量值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CmdListAxMoveUserVar", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CmdListAxMoveUserVar(Int16 cardIndex, Int16 index, Int16 axNo, Int16 eventIndex, Int32 userID);

        /// @brief 缓存区等待指令，延时等待一段时间
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CmdListWaitTime", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CmdListWaitTime(Int16 cardIndex, Int16 index, Int16 waitTime, Int32 userID);

        /// @brief 缓存区等待指令，等待DI触发
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CmdListWaitDi", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CmdListWaitDi(Int16 cardIndex, Int16 index, Int16 diNo, Int16 diLevel, Int16 diType, Int32 userID);

        /// @brief 缓存区等待指令，等待轴运动到位
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CmdListWaitAxMoveDone", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CmdListWaitAxMoveDone(Int16 cardIndex, Int16 index, Int16 axNo, Int16 arrvType, Int32 userID);

        /// @brief 缓存区等待指令，等待轴位置穿越位置
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CmdListWaitAxPosCross", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CmdListWaitAxPosCross(Int16 cardIndex, Int16 index, Int16 axNo, Int32 Pos, Int16 crossType, Int32 userID);

        /// @brief 缓存区等待指令，等待全局变量值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CmdListWaitUserVar", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CmdListWaitUserVar(Int16 cardIndex, Int16 index, Int16 varIndex, Int32 varValue, Int32 userID);

        /// @brief 缓存区等待指令，等待事件执行完成
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CmdListWaitEventDone", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CmdListWaitEventDone(Int16 cardIndex, Int16 index, Int16 eventIndex, Int32 userID);

        /// @brief 缓存区设置指令，设置DO输出
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CmdListSetDo", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CmdListSetDo(Int16 cardIndex, Int16 index, Int16 doNo, Int16 doLevel, Int16 doType, Int32 userID);

        /// @brief 缓存区设置指令，设置脉冲DO输出
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CmdListSetPulseDo", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CmdListSetPulseDo(Int16 cardIndex, Int16 index, Int16 outIndex, Int32 userID);

        /// @brief 缓存区设置指令，设置事件启动
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CmdListSetUserVal", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CmdListSetUserVal(Int16 cardIndex, Int16 index, Int16 varIndex, Int32 varValue, Int32 userID);

        /// @brief 缓存区设置指令，设置全局变量值
        [DllImport(Imc60ApiDllName, EntryPoint = "IMC_CmdListSetEventTrigger", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 IMC_CmdListSetEventTrigger(Int16 cardIndex, Int16 index, Int16 eventIndex, Int32 userID);

        #endregion

    }
}
