using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuHuDuanDoIP.Model
{
    /// <summary>
    /// 设备参数
    /// </summary>
    public  class SheBeiModel
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public int SheBeiID { get; set; }
        /// <summary>
        /// IP
        /// </summary>
        public string IP { get; set; } = "";

        /// <summary>
        /// 端口 默认端口是13400
        /// </summary>
        public int DuanKou { get; set; } = 13400;

        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name { get; set; } = "";
       
        /// <summary>
        /// 请求地址
        /// </summary>
        public string SA { get; set; } = "OE 80";
        /// <summary>
        /// 过滤的读报文
        /// </summary>
        public List<string> GuoLuBaoWen { get; set; } = new List<string>();

        /// <summary>
        /// 采集的时间(s)
        /// </summary>
        public float ChaoTime { get; set; } = 20;

        /// <summary>
        /// 秘钥路径
        /// </summary>
        public string MiYaoPath { get; set; } = "OE 80";

        /// <summary>
        /// 心跳指令
        /// </summary>
        public string XinTiaoZhiLing { get; set; } = "02 FD 80 01 00 00 00 06 0E 80 1F FF 3E 80";

        /// <summary>
        /// 握手指令
        /// </summary>
        public string WoShouZhiLing { get; set; } = "02FD0005000000070E800000000000";

        public bool TX { get; set; } = false;
    }
}
