using System;
using System.Collections.Generic;
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

      
           
        /// <summary>
        /// true  表示读
        /// </summary>
        public CunType IsDu { get; set; } = CunType.XiePing;

        public JiCunQiModel JiCunQi { get; set; } = null;

        public CunModel FuZhi()
        {
            CunModel model= new CunModel();
            model.IsDu = IsDu; 
            model.Time = Time;
            model.IsZhengZaiCe = IsZhengZaiCe; 
            model.JiCunQi = JiCunQi; 
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
        /// 寄存器关
        /// </summary>
        XiePing,    

        KaiShiPing,
    }
}
