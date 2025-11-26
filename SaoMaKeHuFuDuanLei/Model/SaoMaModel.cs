using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CommLei.DataChuLi;

namespace YiBanSaoMaQi.Model
{
    /// <summary>
    /// 扫码model
    /// </summary>
    public class IPFuWuPeiModel
    {
        public string IP { get; set; } = "";

        public int Port { get; set; } = 9989;

        /// <summary>
        /// 1是服务器 2是客户端 3是不需要开启的客户端
        /// </summary>
        public int IsFuWuDuan { get; set; } = 1;

        public string ZhanDianName { get; set; } = "";

        public int IsTongYiGe { get; set; } = 0;

        public int IsQingQiu { get; set; } = 1;


        public string DiZhi { get; set; } = "";

        public string DataLuJing { get; set; } = "";
        public int PaiXu { get; set; } = 0;

        public bool Tx { get; set; } = false;
        public Socket Socket { get; set; } = null;
        public DateTime JiShiQi { get; set; } = DateTime.Now;
        public SerialFeiDataJieXi JieXieQi { get; set; } = new SerialFeiDataJieXi();

    }

  
}
