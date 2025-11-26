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
        public int IsZhengZaiCe { get; set; } = 0;
          
        /// <summary>
        /// true  表示读
        /// </summary>
        public CunType IsDu { get; set; } = CunType.XieTestShuJu;

        /// <summary>
        /// 1表示亮度 2表示X色坐标 3表示Y色坐标 -1表示写
        /// </summary>
        public int DuType { get; set; } = -1;

        /// <summary>
        /// true  表示读
        /// </summary>
        public DataType SheMeYanSe { get; set; } = DataType.Blue;

        public JiCunQiModel JiCunQi { get; set; } = new JiCunQiModel();

        public CunModel FuZhi()
        {
            CunModel model= new CunModel();
            model.IsDu = IsDu; 
           
            model.IsZhengZaiCe = IsZhengZaiCe; 
            model.JiCunQi = JiCunQi; 
            model.ZongSheBeiId = ZongSheBeiId;
            model.SheMeYanSe= SheMeYanSe;
            model.JiCunQi = JiCunQi.FuZhi();
            model.DuType = DuType;
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
        [Description("写测试参数为ASCII【指令#jieshoucount】")]
        XieTestShuJu,
        [Description("写配置参数为ASCII【(指令#IsFanHui(1表示返回))*指令#IsFanHui(1表示返回))】")]
        XiePeiZhi,
        [Description("求的色域,没有参数")]
        XieQiuSeYu,
        [Description("求的色d对比度,没有参数")]
        XieQiuDuiBiDu,
        [Description("清理数据，没有参数")]
        XieQingLi,
        [Description("读数据")]
        DuShuJu,
        [Description("写测试参数为ASCII【指令#jieshoucount】")]
        XieJunYunDu,
        [Description("求的色d对比度,没有参数")]
        XieQiuJunYuanDu,
    }


    public enum DataType
    {
        Wu,
        Red,
        Green,
        Blue,
        Black,
        White,
        JunYunDu,
    }
}
