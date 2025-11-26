using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSheBei.Model;

namespace ZhongWangSheBei.Model
{
    public  class ZSModel
    {    
        /// <summary>
        /// 485读数的切换时间
        /// </summary>
        public int QieHuanTime { get; set; } = 80;
        /// <summary>
        /// ip与COM口
        /// </summary>
        public string IpOrCom { get; set; } = "COM99";
        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; } = 38400;

        /// <summary>
        /// 设备ID
        /// </summary>
        public int SheBeiID { get; set; }


        /// <summary>
        /// 超时time
        /// </summary>
        public int ChaoShiTime { get; set; } = 150;
        public int XieRuChaoShi { get; set; } = 0;
        /// <summary>
        /// 多少路
        /// </summary>
        public List<ZiSheBeiModel> LisJiLu { get; set; } = new List<ZiSheBeiModel>();
    }
    public class ZiSheBeiModel
    {
        /// <summary>
        /// 中盛配置ID
        /// </summary>
        public int ZSID { get; set; } = 0;
        /// <summary>
        /// 地址号
        /// </summary>
        public int DiZhi { get; set; } = 1;

        public string ZiName { get; set; } = "";
        /// <summary>
        /// 有多少路
        /// </summary>
        public int JiLu { get; set; } = 0;

        /// <summary>
        /// true  表示通信成功 不需要配置
        /// </summary>
        public bool Tx { get; set; } = false;

        /// <summary>
        /// 对应的指令 不需要配置
        /// </summary>
        public List<byte> ZhiLing { get; set; } = new List<byte>();

        /// <summary>
        /// 不需要配置
        /// </summary>      
        public int ChangDu { get; set; }

        public int ChaoShiTime { get; set; } = 150;
        public int CiShu { get; set; } = 5;

      
    }
}
