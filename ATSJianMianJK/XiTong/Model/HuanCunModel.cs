using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSJianMianJK.XiTong.Model
{
    /// <summary>
    /// 缓存参数
    /// </summary>
    public class HuanCunModel
    {
        /// <summary>
        /// 这个参数是唯一的
        /// </summary>
        public string HuanCunName { get; set; } = "";

        /// <summary>
        /// true 表示长久缓存
        /// </summary>
        public bool IsChangJiuHuanCun { get; set; } = false;

        /// <summary>
        /// 缓存值
        /// </summary>
        public object Value { get; set; } = "";

        /// <summary>
        /// true表示二维码
        /// </summary>
        public bool IsErWeiMa { get; set; } = false;

        /// <summary>
        /// 默认值
        /// </summary>
        public object MoRenZhi { get; set; } = "";

        public int TDID { get; set; } =1;

        public void Clear()
        {
            if (IsChangJiuHuanCun == false)
            {
                Value = MoRenZhi;
            }
        }
        public void SetCanShu(object value)
        {
            Value = value;
        }

        public object GetZhi()
        {
            return Value;
        }

        public HuanCunModel FuZhi()
        {
            HuanCunModel huanCunModel = new HuanCunModel();
            huanCunModel.HuanCunName = HuanCunName;
            huanCunModel.IsChangJiuHuanCun = IsChangJiuHuanCun;
            huanCunModel.IsErWeiMa = IsErWeiMa;
            huanCunModel.MoRenZhi = MoRenZhi;
            huanCunModel.TDID = TDID;
            huanCunModel.Value = Value;
            return huanCunModel;
        }
    }
}
