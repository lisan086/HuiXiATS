using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommLei.JiChuLei;
using SSheBei.CRCJiaoYan;
using SSheBei.Model;

namespace ModBuTCP.Model
{
    public  class SheBeiModel
    {
        public string IpOrCom { get; set; } = "192.168.10.105";
        public int Port { get; set; } = 502;
        public int SheBeiID { get; set; } = 1;

        public int DiZhi { get; set; } = 1;

        /// <summary>
        /// 设备名称
        /// </summary>
        public string SheBeiName { get; set; } ="";

        public bool Tx { get; set; } = false;

        public int XieYanShi { get; set; } = 20;

        public int DuYanShi { get; set; } = 500;

        /// <summary>
        /// 寄存器数据
        /// </summary>
        public List<DataCunModel> DataCunModels { get; set; } = new List<DataCunModel>();

     
    }
    public class DataCunModel
    {
        /// <summary>
        /// 这个要唯一名称
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// 寄存地址
        /// </summary>
        public int JiCunDiZhi { get; set; } = 1;
      
        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; } = 1;



    


        public DataType DataType { get; set; } = DataType.Int;

        public YingYongType YingYongType { get; set; } = YingYongType.DuPuTong;

        public int ZiDiZhi { get; set; } = -1;


        /// <summary>
        /// 0表示正在写 1表示写成功 2表示写失败
        /// </summary>
        public int IsXieWan { get; set; } = 0;

        /// <summary>
        /// 设备ID,不需要配置
        /// </summary>
        public int SheBeiID { get; set; } = 1;

        public float BZhi { get; set; } = 0;

        public JiCunQiModel JiCunQiModel { get; set; } = null;
     
      
      
    }


  
    public enum DataType
    {
        Int,
    }

    public enum YingYongType
    { 
        DuPuTong,
        XiePuTong,
    }
}
