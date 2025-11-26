using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Common.SheBeiTeXing;
using SSheBei.Model;

namespace DBPLCS7.Model
{
    /// <summary>
    /// PLC设备的model
    /// </summary>
    public class PLCShBeiModel
    {
        [SheBeiLieTeXing(true, "PCLType")]
        public PCLType PCLType { get; set; } = PCLType.PLC1500;
        /// <summary>
        /// plc的名称
        /// </summary>
        [SheBeiLieTeXing(true, "PLCName")]
        public string PLCName { get; set; } = "";

        /// <summary>
        /// plc的IP或者com口
        /// </summary>
        [SheBeiLieTeXing(true, "IP")]
        public string IP { get; set; } = "";

        /// <summary>
        /// plc的端口号 作为唯一键
        /// </summary>
        public int Port { get; set; } = 502;
        /// <summary>
        /// plc的插槽号
        /// </summary>
        [SheBeiLieTeXing(true, "Rack")]
        public short Rack { get; set; } = 0;
        /// <summary>
        /// plc的slot
        /// </summary>
        [SheBeiLieTeXing(true, "Slot")]
        public short Slot { get; set; } = 1;

        /// <summary>
        /// 采集延时ms
        /// </summary>
        [SheBeiLieTeXing(true, "CaiJiYanShi")]
        public int CaiJiYanShi { get; set; } = 5;

        /// <summary>
        /// 写入延时ms
        /// </summary>
        [SheBeiLieTeXing(true, "XieRuYanShi")]
        public int XieRuYanShi { get; set; } = 10;


        public bool Tx { get; set; } = false;

      

        /// <summary>
        /// 读数据
        /// </summary>
        public List<PLCJiCunQiModel> JiCunQi { get; set; }=new List<PLCJiCunQiModel> ();
  
    }


    /// <summary>
    /// plc的寄存器model
    /// </summary>
    public class PLCJiCunQiModel
    {


        /// <summary>
        /// 读的变量名 唯一
        /// </summary>
        [SheBeiLieTeXing(true, "Name")]
        public string Name { get; set; } = "";
        /// <summary>
        /// plc变量的数据类型,分为Int，DWord等
        /// </summary>
        [SheBeiLieTeXing(true, "PLCDataType")]
        public PLCDataType PLCDataType { get; set; } = PLCDataType.Int;

        /// <summary>
        /// 表示服务的插槽的DB快 -2表示intput -3表示output -4表示Memory
        /// </summary>
        [SheBeiLieTeXing(true, "DBKuan")]
        public short DBKuan { get; set; } = 540;

        /// <summary>
        /// plc的处于DB块的偏移量
        /// </summary>
        [SheBeiLieTeXing(true, "PianYiLiang")]
        public short PianYiLiang { get; set; } = 0;

        [SheBeiLieTeXing(true, "Count")]
        public int Count { get; set; } = 1;
     
        /// <summary>
        /// 读的的参数值
        /// </summary>
        public object Value { get; set; } = new object();
        /// <summary>
        /// 信号类型
        /// </summary>
        [SheBeiLieTeXing(true, "XinHaoType")]
        public XinHaoType XinHaoType { get; set; } = XinHaoType.DBData;

        [SheBeiLieTeXing(true, "GongNengType")]
        public GongNengType GongNengType { get; set; } = GongNengType.DuXieYiQi;
        /// <summary>
        /// 子地址
        /// </summary>
        [SheBeiLieTeXing(true, "AdRm")]
        public int AdRm { get; set; } = -1;

        /// <summary>
        /// 1表示是IO
        /// </summary>
        [SheBeiLieTeXing(true, "IO")]
        public int IsIO { get; set; } =0;
        /// <summary>
        /// 子地址
        /// </summary>
        [SheBeiLieTeXing(true, "IOLuZhi")]
        public string IOLuZhi { get; set; } = "1";
        /// <summary>
        /// 子地址
        /// </summary>
        [SheBeiLieTeXing(true, "IORedZhi")]
        public string IORedZhi { get; set; } = "0";

        public JiCunQiModel JiCunQiModel { get; set; } = null;

        public int SheBeiID { get; set; } = -1;

        /// <summary>
        /// 0是进行中 1表示写完 2表示写失败
        /// </summary>
        public int IsXieWan { get; set; } = 0;

        /// <summary>
        /// 获取对应的数量
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            int count = 1;
            switch (PLCDataType)
            {
                case PLCDataType.Int:
                    count = 2;
                    break;
                case PLCDataType.Real:
                    count = 4;
                    break;
                case PLCDataType.DWord:
                    count = 4;
                    break;
                case PLCDataType.Bool:
                    count = 1;
                    break;
                case PLCDataType.DInt:
                    count = 4;
                    break;
                case PLCDataType.Byte:
                    count = 1;
                    break;
                case PLCDataType.String:
                    count = Count;
                    break;
                case PLCDataType.LReal:
                    count = 8;
                    break;
                case PLCDataType.StringX2:
                    count = Count;
                    break;
                case PLCDataType.String16OrACSII:
                    count = Count;
                    break;
                default:
                    break;
            }
            return count;
        }

        public PLCJiCunQiModel FuZhi()
        {
            PLCJiCunQiModel model = new PLCJiCunQiModel();
            model.AdRm = AdRm;
            model.Count = Count;
            model.DBKuan = DBKuan;
            model.GongNengType = GongNengType;
            model.IOLuZhi = IOLuZhi;
            model.IORedZhi = IORedZhi;
            model.IsIO= IsIO;
            model.IsXieWan = IsXieWan;
            model.JiCunQiModel = JiCunQiModel.FuZhi();
            model.Name = Name;
            model.PianYiLiang = PianYiLiang;
            model.PLCDataType = PLCDataType;
            model.SheBeiID = SheBeiID;
            model.Value = Value;
            model.XinHaoType = XinHaoType;
            return model;
        }
    }
    /// <summary>
    /// PLC连接类型
    /// </summary>
    public enum PCLType
    {
        PLC200,
        PLC300_1200,
        PLC1500
    }
    /// <summary>
    /// 数据类型
    /// </summary>
    public enum PLCDataType
    {
        /// <summary>
        /// int16的类型 2个字节
        /// </summary>
        [Description("填写数字参数")]
        Int = 1,
        /// <summary>
        /// double数据类型 4个字节
        /// </summary>
        [Description("填写数字参数")]
        Real = 2,
        /// <summary>
        /// UInt32 数据类型 4个字节
        /// </summary>
        [Description("填写数字参数")]
        DWord = 3,
        /// <summary>
        /// bool数据类型 1个字节
        /// </summary>
        [Description("填写数字参数")]
        Bool = 4,
        /// <summary>
        /// Int32 数据类型 4个字节
        /// </summary>
        [Description("填写数字参数")]
        DInt = 5,
        /// <summary>
        /// byte 数据类型 1个字节
        /// </summary>
        Byte = 6,
        /// <summary>
        /// string 类型
        /// </summary>
        [Description("填写字符串参数")]
        String = 7,
        /// <summary>
        /// 双精度 double  8个数
        /// </summary>
        [Description("填写数字参数")]
        LReal = 8,
        /// <summary>
        /// string x2
        /// </summary>
        [Description("填写字符串参数")]
        StringX2 = 9,
        /// <summary>
        /// string x2
        /// </summary>
        [Description("填写字符串参数")]
        String16OrACSII = 10,
    }

    /// <summary>
    /// 信号类型
    /// </summary>
    public enum XinHaoType
    {
        InPut = 0,
        OutPut = 1,
        DBData = 2,
        Memory = 3,
    }


    public enum GongNengType
    {
        DuPuTong,
        DuXinTiao,
        XieXinTiao,
        XiePuTong,
        DuXieYiQi,
    }
}
