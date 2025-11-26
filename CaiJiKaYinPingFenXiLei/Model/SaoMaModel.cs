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
        /// 采集任务
        /// </summary>
        public string TaskName { get; set; } = "";
        /// <summary>
        /// 采集通道
        /// </summary>
        public string CaiJiPort { get; set; } = "Dev1/ai0";

     
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


        public int CaiJiShuLiang { get; set; } = 41000;

        /// <summary>
        /// 采用率
        /// </summary>
        public double CaiYangLv { get; set; } = 44100.0;
        public double LvBoPinLv { get; set; } = 50;
        public double LvBoWidth { get; set; } =10;
        public double LinMinDuadVoltage { get; set; } = 5;
        public double LinMinDuFaDaZengYi { get; set; } =20;
        public double CaiJiMiaoSu { get; set; } = 1;
        public double linMinDuDaQiYa { get; set; } = 1;

        public double GuiYiZhi { get; set; } = 32768.0;
        public bool IsLvBo { get; set; } = false;


        public double FuZhi { get; set; } = 5;

        public double ZaoYing { get; set; } = 0.05;
    }
}
