using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace YiBanSaoMaQi.Model
{
    /// <summary>
    /// 扫码model
    /// </summary>
    public class SaoMaModel
    {

        /// <summary>
        /// can设备类型
        /// </summary>
        public Define DeviceType { get; set; } = Define.ZCAN_USBCANFD_200U;

        /// <summary>
        /// s设备索引
        /// </summary>
        public uint DeviceIndex { get; set; } = 0;

        public uint HaoBoTeLu { get; set; } = 500000;

        public uint BaiBoTeLu { get; set; } = 500000;

        /// <summary>
        /// 表示通道数
        /// </summary>
        public int IndexCount { get; set; } = 1;

        /// <summary>
        /// 1表示CANFD
        /// </summary>
        public int IsFD { get; set; } = 0;
        /// <summary>
        ///
        /// </summary>
        public bool TX { get; set; } = false;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = "";

        public int ChaoShiTime { get; set; } = 1000;
        /// <summary>
        /// 设备ID
        /// </summary>
        public int SheBeiID { get; set; }

        public int IsFenKaiShou = 0;
        /// <summary>
        /// 一直发的指令
        /// </summary>
        public string YiZhiFaDeZhiLing { get; set; } = "";

        public IntPtr Device_handle { get; set; } = new IntPtr();
        public IntPtr Ptr { get; set; } = new IntPtr();

        public int IsYiZhiXie { get; set; } = -1;


        public Dictionary<int, IntPtr> Canshuju { get; set; } = new Dictionary<int, IntPtr>();
    }


    public enum Define : uint
    {
        TYPE_CAN = 0,
        TYPE_CANFD = 1,
        ZCAN_USBCAN1 = 3,
        ZCAN_USBCAN2 = 4,
        ZCAN_PCI9820I = 16,
        ZCAN_CANETUDP = 12,
        ZCAN_CANETTCP = 17,
        ZCAN_CANWIFI_TCP = 25,
        ZCAN_USBCAN_E_U = 20,
        ZCAN_USBCAN_2E_U = 21,
        ZCAN_USBCAN_4E_U = 31,
        ZCAN_PCIECANFD_100U = 38,
        ZCAN_PCIECANFD_200U = 39,
        ZCAN_PCIECANFD_200U_EX = 62,
        ZCAN_PCIECANFD_400U = 61,
        ZCAN_USBCANFD_200U = 41,
        ZCAN_USBCANFD_400U = 76,
        ZCAN_USBCANFD_100U = 42,
        ZCAN_USBCANFD_MINI = 43,
        ZCAN_USBCANFD_800U = 59,
        ZCAN_CLOUD = 46,
        ZCAN_CANFDNET_200U_TCP = 48,
        ZCAN_CANFDNET_200U_UDP = 49,
        ZCAN_CANFDNET_400U_TCP = 52,
        ZCAN_CANFDNET_400U_UDP = 53,
        ZCAN_CANFDNET_800U_TCP = 57,
        ZCAN_CANFDNET_800U_UDP = 58,
       
    }
}
