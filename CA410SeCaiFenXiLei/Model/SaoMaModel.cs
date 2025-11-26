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
        /// ip与COM口
        /// </summary>
        public string IpOrCom { get; set; } = "COM99";
        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; } = 38400;

     
        /// <summary>
        ///
        /// </summary>
        public bool TX { get; set; } = false;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// 设备ID
        /// </summary>
        public int SheBeiID { get; set; }

        public int TxCount { get; set; } = 0;


        public int XieTime { get; set; } = 10;

        public int DuChaoShiTime { get; set; } = 2000;

        public double JiZhunACD { get; set; } = 0.158;
        public double BiaoZhunRX { get; set; } = 0.670;
        public double BiaoZhunRY { get; set; } = 0.330;
        public double BiaoZhunGX { get; set; } = 0.21;
        public double BiaoZhunGY { get; set; } = 0.710;
        public double BiaoZhunBX { get; set; } = 0.140;
        public double BiaoZhunBY { get; set; } = 0.80;

    }
}
