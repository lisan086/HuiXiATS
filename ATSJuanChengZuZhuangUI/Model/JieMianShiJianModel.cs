using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZuZhuangUI.Model
{
    public class JieMianShiJianModel
    {
        public int GWID { get; set; } = -1;
        /// <summary>
        /// 对象值
        /// </summary>
        public object Value { get; set; } = "";

        public bool JieGuo { get; set; } = false;
    }

    /// <summary>
    /// 事件的类型
    /// </summary>
    public enum EventType
    {      
        /// <summary>
        /// 进站二维码
        /// </summary>
        JinZhanErWeiMa,
        /// <summary>
        /// 进站流程
        /// </summary>
        JinZhan,
        /// <summary>
        /// 出站流程
        /// </summary>
        ChuZhan,
        /// <summary>
        /// 出站二维码
        /// </summary>
        ChuZhanErWeiMa,
        /// <summary>
        /// 出站数据
        /// </summary>
        ChuZhanData,
        /// <summary>
        /// 测试总结果
        /// </summary>
        TestZongJieGuo,
      
        ShouZhanChuMa,
    }
}
