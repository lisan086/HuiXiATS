using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSheBei.Model;

namespace ZhongWangSheBei.Model
{
    /// <summary>
    /// 存数据model
    /// </summary>
    public  class CunModel
    {
        /// <summary>
        /// 总设备Model
        /// </summary>
        public int ZongSheBeiId { get; set; }
        /// <summary>
        /// 子设备Model
        /// </summary>
        public int ZiSheBeiID { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public int JiCunDiZhi { get; set; }

        /// <summary>
        /// 地址号
        /// </summary>
        public int ZDiZhi { get; set; } = 1;

        public int JiLu { get; set; }

        public int XieRuChaoShi { get; set; } = 0;

        /// <summary>
        /// true  表示读
        /// </summary>
        public CunType IsDu { get; set; } = CunType.DuJiCunQi;

        public JiCunQiModel JiCunQi { get; set; } = new JiCunQiModel();

        public CunModel FuZhi()
        {
            CunModel model = new CunModel();
            model.IsDu = IsDu;
            model.JiCunDiZhi = JiCunDiZhi;
            model.JiCunQi = JiCunQi.FuZhi();
            model.JiLu = JiLu;
            model.ZDiZhi = ZDiZhi;
            model.ZiSheBeiID = ZiSheBeiID;
            model.ZongSheBeiId = ZongSheBeiId;
            model.XieRuChaoShi = XieRuChaoShi;
            return model;
        }
    }


    /// <summary>
    /// 存的类型
    /// </summary>
    public enum CunType
    {
        /// <summary>
        /// 读寄存器
        /// </summary>
        [Description("读继电器值,返回1表示开 0表示关")]
        DuJiCunQi,

        /// <summary>
        /// 寄存器全开
        /// </summary>
        [Description("写全开继电器,把所有继电器置1,不需要参数")]
        Xie全开,
        /// <summary>
        /// 寄存器全关
        /// </summary>
        [Description("写全关继电器,把所有继电器置0 不需要参数")]
        Xie全关,
        /// <summary>
        /// 寄存器开
        /// </summary>
        [Description("写开继电器,把设置的继电器置1,需要参数:1|2 类型")]
        Xie开,
        /// <summary>
        /// 寄存器关
        /// </summary>
        [Description("写关继电器,把设置的继电器置0,需要参数:1|2 类型")]
        Xie关,
      
    }
}
