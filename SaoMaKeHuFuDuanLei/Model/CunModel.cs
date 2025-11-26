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
        /// 0-进行中 1表示成功 2表示失败 3表示失败
        /// </summary>
        public int IsZhengZaiCe { get; set; } = 0;


        /// <summary>
        /// true  表示读
        /// </summary>
        public CunType IsDu { get; set; } = CunType.XieQingQiuShuJu;

        public JiCunQiModel JiCunQi { get; set; } = new JiCunQiModel();

        public CunModel FuZhi()
        {
            CunModel model = new CunModel();
            model.IsDu = IsDu;
            model.IsZhengZaiCe = IsZhengZaiCe;
            model.JiCunQi = JiCunQi.FuZhi();
         
        
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
        [Description("写请求数据 不需要参数")]
        XieQingQiuShuJu,
        /// <summary>
        /// 读寄存器
        /// </summary>
        [Description("写做完[biaoshi*hege(1是合格)]")]
        XieZuoWanShuJu,

        /// <summary>
        /// 读寄存器
        /// </summary>
        [Description("写扫码数据 参数:[hao,qianma,houma*hao,qianma,houma*hao,qianma,houma]")]
        XieMaShuJu,
        /// <summary>
        /// 读寄存器
        /// </summary>
        [Description("读文件数据 参数:[1是需要读文件 0是不读文件]")]
        DuWenJianState,
        /// <summary>
        /// 读寄存器
        /// </summary>
        [Description("读文件数据 参数:[biaoshi]")]
        XieWenJianData,
      
    }
}
