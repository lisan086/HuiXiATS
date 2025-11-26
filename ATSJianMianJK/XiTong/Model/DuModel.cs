using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommLei.JiChuLei;

namespace ATSJianMianJK.XiTong.Model
{
    /// <summary>
    /// 绑定那些寄存器
    /// </summary>
    public class DuModel
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public string JiCunQiName { get; set; } = "";
        /// <summary>
        /// 设备id
        /// </summary>
        public int SheBeiID { get; set; }
        /// <summary>
        /// true 表示可靠
        /// </summary>
        public bool IsKeKao { get; set; } = false;
        /// <summary>
        /// 参数
        /// </summary>
        public object Value { get; set; } = "";
     

        /// <summary>
        /// 读寄存器类型
        /// </summary>
        public string Type { get; set; } = "";
        /// <summary>
        /// 匹配类型
        /// </summary>
        public PiPeiType PiPeiType { get; set; } = PiPeiType.Zhi;

        /// <summary>
        /// 值之间采用,隔开
        /// </summary>
        public string PiPeiValue { get; set; } = "";

        /// <summary>
        /// true 表示写
        /// </summary>
        public bool IsXie { get; set; } = false;
        /// <summary>
        /// 读才有匹配值
        /// </summary>
        public List<string> LisPiPeiValue
        {
            get
            {
                return ChangYong.JieGeStr(PiPeiValue, ',');
            }

        }
        public DuModel FuZhi()
        {
            DuModel model = new DuModel();
            model.IsKeKao =IsKeKao;
            model.IsXie = IsXie;
            model.Value = Value;
            model.Type = Type;
            model.LisPiPeiValue.AddRange(LisPiPeiValue.ToArray());
            model.PiPeiValue=PiPeiValue;
            model.JiCunQiName =JiCunQiName;
            model.PiPeiType = PiPeiType;
            model.SheBeiID = SheBeiID;
            return model;
        }
    }


    public enum PiPeiType
    {

        /// <summary>
        /// 包含
        /// </summary>
        BaoHan,
        /// <summary>
        /// 等于
        /// </summary>
        DengYu,
        /// <summary>
        /// 大于
        /// </summary>
        DaYu,
        /// <summary>
        /// 大于等于
        /// </summary>
        DaYuDengYu,
        /// <summary>
        /// 小于
        /// </summary>
        XiaoYu,
        /// <summary>
        /// 小于等于
        /// </summary>
        XiaoYuDengYu,

        /// <summary>
        /// 两者之间
        /// </summary>
        LiangZheZhiJian,
        /// <summary>
        /// 值类型
        /// </summary>
        Zhi,
    }

}
