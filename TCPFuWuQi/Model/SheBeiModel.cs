using System;
using System.Collections.Generic;
using System.ComponentModel;
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

     
    }
    public class DataCunModel
    {
        /// <summary>
        /// 这个要唯一名称
        /// </summary>
        public string Name { get; set; } = "";
        public int SheBeiID { get; set; } = 1;
        public YingYongType YingYongType { get; set; } = YingYongType.DuQingQiuASCII;
      

        /// <summary>
        /// 0是进行中 1是写成功 2是写失败 3是超时
        /// </summary>
        public int IsXieWang { get; set; } = 0;
        public JiCunQiModel JiCunQiModel { get; set; } = null;

        public DataCunModel FuZhi()
        {
            DataCunModel mdoel = new DataCunModel();
            mdoel.Name = Name;
            mdoel.IsXieWang = IsXieWang;
            mdoel.JiCunQiModel = JiCunQiModel.FuZhi();
            mdoel.SheBeiID = SheBeiID;
            mdoel.YingYongType = YingYongType;
            return mdoel;
        }

    }


  

    public enum YingYongType
    {
        [Description("接收服务的数据 参数是ASCII")]
        DuQingQiuASCII,
        [Description("写请求数据")]
        XieShuJuASCII,
    }
}
