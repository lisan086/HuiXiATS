using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSJianCeXianTi.Model
{
    /// <summary>
    /// 界面刷新的用的model
    /// </summary>
    public class XiangMuModel
    {
       
        /// <summary>
        /// 步骤
        /// </summary>
        public BuZhouType BuZhouType { get; set; } = BuZhouType.Wu;
      
        /// <summary>
        /// 总结果 true表示合格
        /// </summary>
        public bool ZongJieGuo { get; set; } = false;

        /// <summary>
        /// 不合格项目
        /// </summary>
        public List<string> BuHeGeXiangMu { get; set; } = new List<string>();

        /// <summary>
        /// 测试项目
        /// </summary>
        public TestModel TestModel { get; set; } = new TestModel();
        /// <summary>
        /// 执行百分比
        /// </summary>
        public double ZhiXingBaiFenBi { get; set; } = 0;

        public int TDID { get; set; } = -1;
        /// <summary>
        /// 底部描述
        /// </summary>
        public string DiLanMiaoSu { get; set; } = "";

        /// <summary>
        /// 二维码
        /// </summary>
        public string ErWeiMa { get; set; } = "";

        /// <summary>
        /// true  表示总项
        /// </summary>
        public bool IsZongXiang { get; set; } = true;

    }

    public enum BuZhouType
    { 
        ZhunBeiJianCe,
        KaiShiJianCe,
        DXiangMuJianCe,
        DXiangMuJieSu,
        ZongJieSu,
        Wu,
    }
}
