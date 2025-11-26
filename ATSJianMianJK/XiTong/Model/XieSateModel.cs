using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JieMianLei.FuFrom.KJ;

namespace ATSJianMianJK.XiTong.Model
{
    /// <summary>
    /// 系统写配置
    /// </summary>
    public class XieSateModel
    {

        /// <summary>
        /// 通道id
        /// </summary>
        public int TDID { get; set; } = -1;
        /// <summary>
        /// 名称 这个是唯一
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// 寄存类型
        /// </summary>
        public XieSateType Type { get; set; } = XieSateType.QiTaXie;
        /// <summary>
        /// 用的是秒
        /// </summary>
        public double Miao { get; set; } = 5;
        public DateTime JiShi { get; set; } =DateTime.Now;
        /// <summary>
        /// 这个动作需要操作那些硬件设备IO
        /// </summary>
        public List<XieModel> LisXies { get; set; } = new List<XieModel>();

        public bool IsMaZuXie()
        {
            if (Miao<=0)
            {
                Miao = 1;
            }
            if ((DateTime.Now- JiShi).TotalMilliseconds>= Miao*1000)
            {
                JiShi=DateTime.Now;
                return true;
            }
            return false;
        }
    }





    /// <summary>
    /// 配置寄存器与硬件设备关联
    /// </summary>
    public class XieModel
    {

        public List<JiChuXieDYModel> TiaoJianJiChu { get; set; } = new List<JiChuXieDYModel>();
        public List<JiChuXieDYModel> FuZhiJiChu { get; set; } = new List<JiChuXieDYModel>();
        /// <summary>
        /// 测试等待单位是s
        /// </summary>
        public float DengDaiTime { get; set; } = 0f;

        /// <summary>
        /// 执行顺序
        /// </summary>
        public int ShunXu { get; set; } = 0;
        /// <summary>
        /// 写的时候判断
        /// </summary>
        public GenJuType GenJuType { get; set; } = GenJuType.FuZhi;

        /// <summary>
        /// 1是取与 2是取或
        /// </summary>
        public int IfPanDuanType { get; set; } = 1;

        /// <summary>
        /// 其他变量
        /// </summary>
        public string QiTaLiang { get; set; } = "";
    }

    public class JiChuXieDYModel
    {
        /// <summary>
        /// 左边参数
        /// </summary>
        public ZBLModel ZBLModel { get; set; } = new ZBLModel();
        /// <summary>
        /// 中间变量
        /// </summary>
        public ZhongJianType ZhongJianType { get; set; } = ZhongJianType.FuZhi;
        /// <summary>
        /// 右边参数
        /// </summary>
        public YBLModel YBLModel { get; set; } = new YBLModel();

        public int ShunXu { get; set; } = 1;
        /// <summary>
        /// 1 表示该信号可以参与屏蔽功能
        /// </summary>
        public int IsPingBi { get; set; } = 0;
        public override string ToString()
        {
            return $"{ZBLModel.SheBeiID}:{ZBLModel.ZBianLiangName},{ZBLModel.ZLeiXing} {ZhongJianType} {YBLModel.SheBeiID}:{YBLModel.YouCanShu},{YBLModel.ZLeiXing}";
        }
    }

    /// <summary>
    /// 左边变量
    /// </summary>
    public class ZBLModel
    {
        /// <summary>
        /// 左边变量名称
        /// </summary>
        public string ZBianLiangName { get; set; } = "";

        /// <summary>
        /// 设备id或者通道id
        /// </summary>
        public int SheBeiID { get; set; } = -1;

        /// <summary>
        /// 左边的变量类型
        /// </summary>
        public ZblLeiXing ZLeiXing { get; set; } = ZblLeiXing.DuJC;

    }

    /// <summary>
    /// 左边变量
    /// </summary>
    public class YBLModel
    {
        /// <summary>
        /// 右边参数
        /// </summary>
        public string YouCanShu { get; set; } = "";

        /// <summary>
        /// 设备id或者通道id
        /// </summary>
        public int SheBeiID { get; set; } = -1;

        /// <summary>
        /// 左边的变量类型
        /// </summary>
        public YblLeiXing ZLeiXing { get; set; } = YblLeiXing.ChangLiangZhi;

    }


    /// <summary>
    /// 左边变量的类型
    /// </summary>
    public enum ZblLeiXing
    {
       
        /// <summary>
        /// 缓存变量
        /// </summary>
        HuanCunLiang,
        /// <summary>
        /// 读块
        /// </summary>
        DuIOKuai,
        /// <summary>
        /// model数据
        /// </summary>
        XieJC,
        /// <summary>
        /// 开关数据
        /// </summary>
        DuJC,     
        /// <summary>
        /// 读寄存器返回的
        /// </summary>
        DuXieJCRec,
    }

    /// <summary>
    /// 左边变量的类型
    /// </summary>
    public enum YblLeiXing
    {

        /// <summary>
        /// 缓存变量
        /// </summary>
        HuanCunLiang,     
        /// <summary>
        /// 开关数据
        /// </summary>
        DuJC,
        /// <summary>
        /// 常量值
        /// </summary>
        ChangLiangZhi,

        /// <summary>
        /// 两个 常量值#
        /// </summary>
        LiangGeZhi,
    }

    /// <summary>
    /// 中间类型
    /// </summary>
    public enum ZhongJianType
    {
        /// <summary>
        /// 包含
        /// </summary>
        BaoHan ,
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
        /// 赋值
        /// </summary>
        FuZhi,
     
    }

    public enum GenJuType
    {
        /// <summary>
        /// if判断
        /// </summary>
        If,
        /// <summary>
        /// 赋值判断
        /// </summary>
        FuZhi,
        /// <summary>
        /// 等待判断
        /// </summary>
        DengDaiPanDuan,
        /// <summary>
        /// 等待
        /// </summary>
        Wait,
     
    }
    /// <summary>
    /// 写类型
    /// </summary>
    public enum XieSateType
    {
       ZiDongXie,
       QiTaXie,
    }
}
