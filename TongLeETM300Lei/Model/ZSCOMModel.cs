using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSheBei.Model;

namespace ZhongWangSheBei.Model
{
    public class ZSModel
    {
      
        /// <summary>
        /// ip与COM口
        /// </summary>
        public string IpOrCom { get; set; } = "COM99";
        public string Name { get; set; } = "";
        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; } = 38400;

        public List<int> DiZhi { get; set; } = new List<int>();

        public int QiShiDiZhi { get; set; } = 16;
        /// <summary>
        /// 设备ID
        /// </summary>
        public int SheBeiID { get; set; }

        public double DianLiuChengShu { get; set; } = 0.01;

        public double DianYaChengShu { get; set; } = 0.001;

        public double GongLuChengShu { get; set; } = 0.001;

        public double SetDianYa { get; set; } = 35;

        public double SetDianLiu { get; set; } = 200;

        public int ChangDu { get; set; } = 84;

        public int ChaoShiTime { get; set; } = 300;

        public int XieRuTime { get; set; } = 50;

        public int QieHuanTime { get; set; } = 50;

        public List<string> DuiYingDiZhi { get; set; } = new List<string>();

        public Dictionary<int, List<byte>>  ZhiLing { get; set; } =new Dictionary<int, List<byte>>();

        public Dictionary<int, bool> Tx { get; set; }=new Dictionary<int, bool>();

        public Dictionary<int, int> TxCiShu { get; set; } = new Dictionary<int, int>();
    }

}
