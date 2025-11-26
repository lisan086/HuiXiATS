using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZuZhuangUI.Model
{
    /// <summary>
    /// 界面的操作参数
    /// </summary>
    public class JieMianCaoZuoModel
    {
        /// <summary>
        /// 表示ID
        /// </summary>
        public int GWID { get; set; } = -1;

        /// <summary>
        /// 操作参数
        /// </summary>
        public object CanShu { get; set; } = "";

        /// <summary>
        /// 标识
        /// </summary>
        public bool CaoZuoState { get; set; } = false;

    }

    /// <summary>
    /// 动作类型
    /// </summary>
    public enum DoType
    {
        /// <summary>
        /// 打开  不需要参数
        /// </summary>
        Open,
        /// <summary>
        /// 关闭软件用的 不需要参数
        /// </summary>
        Close,

        SaoMaQueRen,
        HuanPeiFang,
        ShouDongSaoMaWeiTiaoShi,
        TingZhiLaoHua,
        ShouDongLaoHua,
    }


}
