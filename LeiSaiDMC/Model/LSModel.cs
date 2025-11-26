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
        /// 卡的ID 不需要配置
        /// </summary>
        public ushort CardNO { get; set; } = 0;

        /// <summary>
        /// 卡的类型
        /// </summary>
        public int CardType { get; set; } = 86066;


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
        /// 卡的参数model
        /// </summary>
        public List<KaCanShuModel> LisKaModels { get; set; } = new List<KaCanShuModel>();

        /// <summary>
        /// 轴的配置参数
        /// </summary>
        public List<ZhouPeiZhiModel> LisZhouPeiZhiModels { get; set; } = new List<ZhouPeiZhiModel>();
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
        /// 轴号
        /// </summary>
        public ushort ZhouHao { get; set; } = 0;

        /// <summary>
        /// 卡的ID 不需要配置
        /// </summary>
        public ushort CardNO { get; set; } = 0;
        /// <summary>
        /// 轴用的配置参数
        /// </summary>
        public int ZhouPeiZhiID { get; set; } = 0;
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
        public ushort BitNo { get; set; } = 0;
        /// <summary>
        /// 卡的ID 不需要配置
        /// </summary>
        public ushort CardNO { get; set; } = 0;

        /// <summary>
        /// id
        /// </summary>
        public int IOID { get; set; } = 0;
    }

    /// <summary>
    /// 卡的参数model
    /// </summary>
    public class KaCanShuModel
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string KaName { get; set; } = "";

        /// <summary>
        /// true 表示轴
        /// </summary>
        public bool IsZhou { get; set; } = false;

        /// <summary>
        /// IO位置或者轴号
        /// </summary>
        public ushort BitNoOrZhouHao { get; set; } = 0;

        /// <summary>
        /// 不是轴才有用
        /// </summary>
        public bool IsXieIO { get; set; } = false;

        /// <summary>
        /// 卡的ID 不需要配置
        /// </summary>
        public ushort CardNO { get; set; } = 0;
        /// <summary>
        /// 轴用的配置参数
        /// </summary>
        public int ZhouPeiZhiID { get; set; } = 0;
          
    }

    /// <summary>
    /// 轴配置Model
    /// </summary>
    public class ZhouPeiZhiModel
    {
        /// <summary>
        /// 轴的id
        /// </summary>
        public int ZhouPeiZhiID { get; set; } = 0;

        /// <summary>
        /// 运动起始速度
        /// </summary>
        public double Low_Vel { get; set; } = 0;
        /// <summary>
        /// 运动速度
        /// </summary>
        public double High_VelZ { get; set; } = 10;

        /// <summary>
        /// 加速时间(s)
        /// </summary>
        public double Tacc { get; set; } = 0.1;
        /// <summary>
        /// 减速时间(s)
        /// </summary>
        public double Tdec { get; set; } = 0.1;

        /// <summary>
        /// 轴停止时间
        /// </summary>
        public double StopTime { get; set; } = 0.1;

        /// <summary>
        /// 停止速度
        /// </summary>
        public double Stop_Vel { get; set; } = 0;

        /// <summary>
        /// s曲线的时间  取值为0到1
        /// </summary>
        public double Spara { get; set; } = 0;
        /// <summary>
        /// 距离最低限制
        /// </summary>
        public double JuLiZuiDi { get; set; } = 0;

        /// <summary>
        /// 误差上
        /// </summary>
        public double WuChaS { get; set; } = 0;

        /// <summary>
        /// 误差下
        /// </summary>
        public double WuChaX { get; set; } = 0;
        /// <summary>
        /// 距离最高限制
        /// </summary>
        public double JuLiZuiGao { get; set; } = -1;

        /// <summary>
        /// 回零的模式
        /// </summary>
        public ushort HomeMothod { get; set; } = 0;

        /// <summary>
        /// 回零运动高速
        /// </summary>
        public double HomeHigh_Vel { get; set; } = 15;

        /// <summary>
        /// 回零运动起始速度
        /// </summary>
        public double HomeLow_Vel { get; set; } = 0;

        /// <summary>
        /// 回零加速时间(s)
        /// </summary>
        public double HomeTacc { get; set; } = 0.1;
        /// <summary>
        ///  回零减速时间(s)
        /// </summary>
        public double HomeTdec { get; set; } = 0.1;

        /// <summary>
        /// 回零的拍偏移
        /// </summary>
        public double HomeOffsetpos { get; set; } = 0;
    }

    public class CunModel
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public int SheBeiID { get; set; } = -1;
        /// <summary>
        /// 轴用的配置参数
        /// </summary>
        public int Zid { get; set; } = 0;

        /// <summary>
        /// 1表示完成 0表示进行中 2表示失败 3表示超时
        /// </summary>
        public int IsWanCheng { get; set; } = 0;
        public JiCunQiModel JiCunQiModel { get; set; } = null;
        public CanShuType CanShuType { get; set; } = CanShuType.DuIO;

        public CunModel FuZhi()
        {
            CunModel cunModel = new CunModel();
            cunModel.CanShuType = CanShuType;
            cunModel.IsWanCheng = IsWanCheng;
            cunModel.Zid = Zid;
            cunModel.SheBeiID = SheBeiID;
            cunModel.JiCunQiModel = JiCunQiModel.FuZhi();
            return cunModel;
        }
    }
    public enum CanShuType
    {
        Du位置,
        Du速度,
        Du使能,
        Du轴状态,
        Du运动状态,
        Du超限报警,
        Du总线状态,
        DuIO,
        XieZhouX位置,
        XieZhouJ位置,
        XieZhou速度,
        XieZhou恒速,
        XieZhou使能,
        XieZhou报警清除,
        XieZhou位置清零,
        XieZhou停止,
        XieZhou回零,
        Xie硬件复位,
        Xie热复位,
        Xie所有轴停止,
        Xie所有轴回零,
        XieIO,
    }
}
