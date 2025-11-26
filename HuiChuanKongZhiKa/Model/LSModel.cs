using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSheBei.Model;

namespace LeiSaiDMC.Model
{
    /// <summary>
    /// 总model
    /// </summary>
    public class LSModel
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public int SheBeiID { get; set; } = -1;

        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// 卡的ID 
        /// </summary>
        public short CardNO { get; set; } = 0;

        /// <summary>
        /// 用相对路径
        /// </summary>
        public string DevWenJianXML { get; set; } = "";

        /// <summary>
        /// 用相对路径
        /// </summary>
        public string SysWenJianXML { get; set; } = "";

        /// <summary>
        /// 卡的最大IO 不需要配置
        /// </summary>
        public int ZuiDaDuIO { get; set; } = 0;

        /// <summary>
        /// 卡的最大IO 不需要配置
        /// </summary>
        public int ZuiDaXieIO { get; set; } = 0;

        /// <summary>
        /// 总线报警
        /// </summary>
        public bool ZongXianBaoJing { get; set; } = false;

        /// <summary>
        /// true  通信 不需要配置
        /// </summary>
        public bool TX { get; set; } = false;

        /// <summary>
        /// 采用断线保持
        /// </summary>
        public bool IsDuanXianBaoChi { get; set; } = false;

        /// <summary>
        /// 卡的参数model
        /// </summary>
        public List<ZhouModel> LisZhouModel { get; set; } = new List<ZhouModel>();

        /// <summary>
        /// 挂接多少IO
        /// </summary>
        public List<IOModel> IOS { get; set; } = new List<IOModel>();
    }
    /// <summary>
    /// 轴model
    /// </summary>
    public class ZhouModel
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public int SheBeiID { get; set; } = -1;
        /// <summary>
        /// 名称
        /// </summary>
        public string ZhouName { get; set; } = "";

        /// <summary>
        /// 轴的ID 
        /// </summary>
        public short ZhouNO { get; set; } = 0;


        /// <summary>
        /// 误差上
        /// </summary>
        public double WuChaS { get; set; } = 0;

        /// <summary>
        /// 误差下
        /// </summary>
        public double WuChaX { get; set; } = 0;

        /// <summary>
        /// 距离最低限制
        /// </summary>
        public double JuLiZuiDi { get; set; } = 0;

        /// <summary>
        /// 距离最高限制
        /// </summary>
        public double JuLiZuiGao { get; set; } = -1;

        public bool IsZaiXian { get; set; } = false;

        /// <summary>
        /// 同一坐标类型
        /// </summary>
        public int TongYiType { get; set; } = 1;


        public double SuDu { get; set; } = 1;
        public double JiaSuDu { get; set; } = 0;

        public double HomeSuDu { get; set; } = 1;

        public double HomeJiaSuDu { get; set; } = 1;

        public int HuiLingMoShi { get; set; } = 27;

    }

    public class IOModel
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public int SheBeiID { get; set; } = -1;
        /// <summary>
        /// 名称
        /// </summary>
        public string IOName { get; set; } = "";
        /// <summary>
        /// IO位置
        /// </summary>
        public short BitNo { get; set; } = 0;
        /// <summary>
        /// 卡的ID 不需要配置
        /// </summary>
        public short CardNO { get; set; } = -1;


        /// <summary>
        /// 1表示读 2表示写
        /// </summary>
        public int IOLeiXing { get; set; } = 1;


    }



    public class CunModel
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public int SheBeiID { get; set; } = -1;

        /// <summary>
        /// 轴用的配置参数 -1表示全局的
        /// </summary>
        public int ZhouOrIOID { get; set; } = 0;
        public JiCunQiModel JiCunQi { get; set; } = null;
        public CanShuType CanShuType { get; set; } = CanShuType.DuIO;
        /// <summary>
        /// 1表示完成 0表示进行中 2表示失败 3表示超时
        /// </summary>
        public int IsWanCheng { get; set; } = 0;
        public IOType IOTYpe { get; set; } = IOType.DuIO;
        public CunModel FuZhi()
        {
            CunModel cunModel = new CunModel();
            cunModel.CanShuType = CanShuType;
            cunModel.IsWanCheng = IsWanCheng;
            cunModel.ZhouOrIOID = ZhouOrIOID;
            cunModel.SheBeiID = SheBeiID;
            cunModel.JiCunQi = JiCunQi.FuZhi();
            cunModel.IOTYpe = IOTYpe;
            return cunModel;
        }
    }

    public enum IOType
    {
        Zhou位置,
        Zhou目标位置,
        Zhou速度,
        Zhou使能,
        Zhou模式,
        Zhou运行状态,
        Zhou原点,
        Zhou到位,
        Zhou忙,
        Zhou急停,
        Zhou报警,
        Zhou在线,
        DuIO,
        DuXieIO,
        Wu,
    }

    public enum CanShuType
    {
        DuShuJu,
        DuIO,
        DuXieIO,
        XieShuJu,
    }

    public enum GongNengType
    {
        XieZhou正常配置,
        XieZhou回零配置,
        XieZhou使能,
        XieZhou不使能,
        XieZhou停止,
        XieZhou回零,
        XieZhou退出回零,
        XieZhou相对位置运动,
        XieZhou绝对位置运动,
        XieZhou恒速运动,
        XieZhou取消急停,
        XieZhou急停,
    }


    public class SendDataModel
    {

        public GongNengType GongNengType { get; set; } = GongNengType.XieZhou使能;

        public List<ZhouCanShuModel> Zhous { get; set; } = new List<ZhouCanShuModel>();

    }

    public class ZhouCanShuModel
    {
        public short ZhouNo { get; set; } = -1;
        public double SuDu { get; set; } = 0;
        public double JiaSuDu { get; set; } = 0;
        public int HuiLingType { get; set; } = 27;

        public double WeiZhi { get; set; } = 0;

        /// <summary>
        /// 1是采用配置
        /// </summary>
        public int IsCaiYongPeiZhi { get; set; } = 1;
    }
}
