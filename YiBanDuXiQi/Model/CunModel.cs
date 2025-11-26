using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSheBei.Model;

namespace YiBanSaoMaQi.Model
{
    /// <summary>
    /// 存数据model
    /// </summary>
    public class CunModel
    {
        /// <summary>
        /// 总设备Model
        /// </summary>
        public int ZongSheBeiId { get; set; }


        /// <summary>
        /// 总设备Model
        /// </summary>
        public int Time { get; set; } = 1000;

        /// <summary>
        /// 总设备Model
        /// </summary>
        public int IsZhengZaiCe { get; set; } = 0;

        public int JieShouCount { get; set; } = 0;
           
        /// <summary>
        /// true  表示读
        /// </summary>
        public CunType IsDu { get; set; } = CunType.XieAsciiFanHuiAscii;

        public JiCunQiModel JiCunQi { get; set; } = new JiCunQiModel();

        public CunModel FuZhi()
        {
            CunModel model= new CunModel();
            model.IsDu = IsDu; 
            model.Time = Time;
            model.IsZhengZaiCe = IsZhengZaiCe; 
            model.JiCunQi = JiCunQi; 
            model.ZongSheBeiId = ZongSheBeiId;
            model.JieShouCount = JieShouCount;
            model.JiCunQi = JiCunQi.FuZhi();
            return model;
        }
    }


    /// <summary>
    /// 存的类型
    /// </summary>
    public enum CunType
    {
        /// <summary>
        /// 寄存器关
        /// </summary>
        [Description("写的指令为ASCII码,返回数据的是ASCII码")]
        XieAsciiFanHuiAscii,
        /// <summary>
        /// 寄存器关
        /// </summary>
        [Description("写的指令为16进制码比如02 03 04,返回的数据是ASCII码")]
        Xie16FanHuiAscii,
        [Description("写的指令为16进制码比如02 03 04,不返回数据")]
        Xie16WuFanHui,
        [Description("写的指令为ASCII码,不返回数据")]
        XieAsciiWuFanHui,
        [Description("写的指令为配置的指令1ASCII码模式,返回的数据是ASCII码")]
        XieZhiLing1FanHuiAscii,
        [Description("写的指令为配置的指令2ASCII码模式,返回的数据是ASCII码")]
        XieZhiLing2FanHuiAscii,
        [Description("写的指令为配置的指令1ASCII码模式,无返回数据")]
        XieZhiLing1WuFanHui,
        [Description("写的指令为配置的指令1ASCII码模式,无返回数据")]
        XieZhiLing2WuFanHui,
        [Description("写moubusRTU协议 参数有两个 设备地址,寄存器地址,功能码,子索引，返回读数数据0或者1")]
        XieModBusRTUDan1FanHui,
        [Description("写moubusRTU协议 参数有两个 设备地址,寄存器地址,功能码,1or0，返回读数为short")]
        XieModBusRTUDan2FanHui,
        [Description("写moubusRTU协议 参数有两个 设备地址,寄存器地址,功能码,1or0，返回读数为int")]
        XieModBusRTUDan4FanHui,

     
    }
}
