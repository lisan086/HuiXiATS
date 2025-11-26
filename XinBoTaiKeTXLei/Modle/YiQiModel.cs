using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoTaiKeTXLei.Modle
{
    public class YiQiModel
    {
        /// <summary>
        /// IP
        /// </summary>
        public string IP { get; set; } = "";

        /// <summary>
        /// 端口 默认端口是13400
        /// </summary>
        public int DuanKou { get; set; } = 13400;

        /// <summary>
        /// 字符长度
        /// </summary>
        public int DuZiFuShu { get; set; } = 267;

        /// <summary>
        /// 字符长度
        /// </summary>
        public int XieZiFuShu { get; set; } = 267;

        /// <summary>
        /// 设备id
        /// </summary>
        public int SheBeiID { get; set; } = 1;

        /// <summary>
        /// 超时时间(ms);
        /// </summary>
        public int ChaoTime { get; set; } = 30000;

        /// <summary>
        /// 是CMD路径
        /// </summary>
        public string QuanXianPath { get; set; } = "";

        
        /// <summary>
        /// true表示通讯
        /// </summary>
        public bool TX { get; set; } = false;
    }
}
