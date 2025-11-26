using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SSheBei.Model;

namespace SundyChengZhong.Model
{
    public class SheBeiModel
    {
        public string SheBeiName { get; set; } = "";
        public string Com { get; set; } = "COM99";
        public int Port { get; set; } = 38400;
        public int SheBeiID { get; set; } = 1;

        public bool Tx { get; set; } = false;

        /// <summary>
        /// 单位是ms
        /// </summary>
        public int QiHuanTime { get; set; } = 80;


        public int XieYanShi { get; set; } = 10;


        public int DuYanShi { get; set; } = 300;

        public List<CZJiCunQiModel> LisJiCunQis { get; set; } = new List<CZJiCunQiModel>();
    }

    public class CZJiCunQiModel
    {
        /// <summary>
        /// 名称(唯一)
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
        /// <summary>
        /// 小数位
        /// </summary>
        public int XiaoShuWei { get; set; } = 0;

        public double BeiChuShu { get; set; } = 1;

        public double BZhi { get; set; } = 0;

        /// <summary>
        /// 设备地址
        /// </summary>
        public int SheBeiDiZhi { get; set; } = 1;

        /// <summary>
        /// 0表示正在写 1表示写完 2表示写失败
        /// </summary>
        public int IsXieWan { get; set; } = 1;

        /// <summary>
        /// 写功能码
        /// </summary>
        public int XieGNM { get; set; }

        /// <summary>
        /// 读功能码
        /// </summary>
        public int DuGNM { get; set; }

        public DataSType DataSType { get; set; } = DataSType.DuPuTong;

        public string MiaoSu { get; set; } = "";

        /// <summary>
        /// 寄存器
        /// </summary>
        public JiCunQiModel JiCunQiModel { get; set; }

        public int SheBeiID { get; set; }
    }

    public class JiLuModel
    {

        public int SheBeiDiZhi { get; set; }
        public int SheBeiID { get; set; }

        /// <summary>
        /// 存数据
        /// </summary>
        public List<byte> ShuJu { get; set; } = new List<byte>();
        /// <summary>
        /// 最小寄存器偏移
        /// </summary>
        public int JiCunQiZuiXiaoPianYi { get; set; } = 0;

        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; } = 0;

        /// <summary>
        /// true表示是线圈
        /// </summary>
        public bool IsXianQuan { get; set; } = false;

        public List<CZJiCunQiModel> Lis = new List<CZJiCunQiModel>();

    }


    public enum DataSType
    {
        DuPuTong,
        XiePuTong,
        DuXieYiQi,
    }
}
