using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCANFD.ShiXian;

namespace YiBanSaoMaQi.Model
{
    /// <summary>
    /// 扫码model
    /// </summary>
    public class SaoMaModel
    {
       
        /// <summary>
        /// P句柄 为10进制
        /// </summary>
        public ushort PHandle { get; set; } = 81;
  

        /// <summary>
        ///配置参数
        /// </summary>
        public string m_Baudrate { get; set; } = "f_clock_mhz=80, nom_brp=8, nom_tseg1=16, nom_tseg2=3, nom_sjw=1, data_brp=4, data_tseg1=7, data_tseg2=2, data_sjw=1";
      
        /// <summary>
        ///
        /// </summary>
        public bool TX { get; set; } = false;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// 超时时间(ms)
        /// </summary>
        public int ChaoShiTime { get; set; } = 1000;
        /// <summary>
        /// 设备ID
        /// </summary>
        public int SheBeiID { get; set; }
    }
}
