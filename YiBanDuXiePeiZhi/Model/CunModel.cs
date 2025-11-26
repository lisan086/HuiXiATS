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

        public string Name { get; set; } = "";

        /// <summary>
        /// 总设备Model
        /// </summary>
        public int Time { get; set; } = 1000;

        /// <summary>
        /// 总设备Model
        /// </summary>
        public int IsZhengZaiCe { get; set; } = 0;

        public int JieShouCount { get; set; } = 0;

        public string ZhiLing { get; set; } = "";

        public string MiaoSu { get; set; } = "";
           
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
            model.Name = Name;
            model.ZhiLing = ZhiLing;
            model.MiaoSu = MiaoSu;
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
        [Description("读数据ASCII解析,用ASCii写")]
        DuAsciiXie,
        [Description("读数据ASCII解析,用16写")]
        DuAsciiAnd16Xie,       
    }
}
