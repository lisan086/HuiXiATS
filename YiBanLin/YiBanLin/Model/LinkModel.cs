using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiBanLin.Model
{
    /// <summary>
    /// Link模型
    /// </summary>
    public class LinModel
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public int SheBeiID { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name { get; set; } = "LIN设备";

        /// <summary>
        /// 波特率
        /// </summary>
        public int BaudRate { get; set; } = 19200;

        /// <summary>
        /// 主从模式(0=从机，1=主机)
        /// </summary>
        public byte MasterMode { get; set; } = 0;


        /// <summary>
        /// 校验类型
        /// </summary>
        public byte CheckType { get; set; } = 1; // 默认增强校验

       
        /// <summary>
        /// 数据长度
        /// </summary>
        public byte DataLen { get; set; } = 8;

        public int DuiYingLin { get; set; } = 0;

        /// <summary>
        /// BREAK信号位数(10-26)
        /// </summary>
        public byte BreakBits { get; set; } = 13;

        public bool Tx { get; set; } = false;

        public int ChaoShiTime { get; set; } = 5000;
    }
}
