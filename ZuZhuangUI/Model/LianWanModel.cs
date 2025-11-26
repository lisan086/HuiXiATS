using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZuZhuangUI.Model
{
    /// <summary>
    /// 联网返回参数
    /// </summary>
    public class LianWanModel
    {
        /// <summary>
        /// true表示返回正确
        /// </summary>
        public JinZhanJieGuoType FanHuiJieGuo { get; set; } = JinZhanJieGuoType.NG;

        /// <summary>
        /// 返回的内容
        /// </summary>
        public string NeiRong { get; set; } = "";
    }

    public enum JinZhanJieGuoType
    {
        Pass,
        NG,
    }
}
