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
        public int JieXiGeShi { get; set; }

       

        /// <summary>
        /// 0-进行中 1表示成功 2表示失败 3表示失败
        /// </summary>
        public int IsZhengZaiCe { get; set; } = 0;


        /// <summary>
        /// true  表示读
        /// </summary>
        public CunType IsDu { get; set; } = CunType.Du读数据;

        public JiCunQiModel JiCunQi { get; set; } = new JiCunQiModel();

        public CunModel FuZhi()
        {
            CunModel model = new CunModel();
            model.IsDu = IsDu;
            model.IsZhengZaiCe = IsZhengZaiCe;
            model.JiCunQi = JiCunQi.FuZhi();
         
            model.JieXiGeShi= JieXiGeShi;
            model.ZongSheBeiId = ZongSheBeiId;
     
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
        [Description("需要写开始扫码才有数据")]
        Du读数据,


        /// <summary>
        /// 寄存器关
        /// </summary>
        [Description("不需要参数")]
        Xie开启设备,

        /// <summary>
        /// 寄存器关
        /// </summary>
        [Description("不需要参数")]
        Xie开启扫码,

        /// <summary>
        /// 寄存器关
        /// </summary>
        [Description("不需要参数")]
        Xie关闭设备,


        /// <summary>
        /// 寄存器关
        /// </summary>
        [Description("不需要参数")]
        Xie关闭扫码,
    }
}
