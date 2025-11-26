using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommLei.JiChuLei;

namespace ATSJianCeXianTi.Model
{
    /// <summary>
    /// 通道参数
    /// </summary>
    public class TDModel
    {
        /// <summary>
        /// TDID
        /// </summary>
        public int TDID { get; set; } = 0;
        /// <summary>
        /// 通道名称
        /// </summary>
        public string TDName { get; set; } = "";

    
        /// <summary>
        /// 用于配置通道
        /// </summary>
        public PeiZhiModel PeiZhiCanShu { get; set; } = new PeiZhiModel();

        /// <summary>
        /// 手动参数
        /// </summary>
        public ShouDongKongZhiModel ShouDongCanShu { get; set; } = new ShouDongKongZhiModel();

        /// <summary>
        /// 传感器感应参数
        /// </summary>
        public GanYingModel GanYingModel { get; set; } = new GanYingModel();

        /// <summary>
        /// 每次测试的复位参数
        /// </summary>
        public FuWeiModel FuWeiCanShu { get; set; } = new FuWeiModel();

    }
    /// <summary>
    /// 每次测试能复位的参数
    /// </summary>
    public class FuWeiModel
    {
        #region 计时
        private double ZongTestTime = 0;
        private DateTime JiShiTime = DateTime.Now;
        private bool _IsKaiShiTime = false;
        private int JiShuCount = 1;
        #endregion


        /// <summary>
        /// 二维码的值
        /// </summary>
        public string MaZhi { get; set; } = "";

        public bool IsKaiShiTime 
        {
            get 
            { 
                return _IsKaiShiTime;
            }
            set
            {
                if (value==false)
                {
                    if (_IsKaiShiTime!=value)
                    {
                        if (JiShuCount == 0)
                        {
                            JiShuCount = 1;
                            ZongTestTime = (DateTime.Now - JiShiTime).TotalSeconds;
                        }
                    }
                   
                }
                _IsKaiShiTime = value;
            }
        }

        public string KaiShiTime { get; set; } = "";
        /// <summary>
        /// 1是合格 2是不合格 3是未测试
        /// </summary>
        public int JieGuo { get; set; } = 3;
        public void Clear()
        {
            MaZhi = "";
            JieGuo = 3;         
            JiShiTime = DateTime.Now;
            ZongTestTime = 0;
            JiShuCount = 0;
            IsKaiShiTime = true;
            KaiShiTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public double GetTestMiao()
        {
            if (IsKaiShiTime)
            {
                JiShuCount = 0;
                ZongTestTime = (DateTime.Now - JiShiTime).TotalSeconds;
              
            }
           
            return ZongTestTime;
        }
    }
    public class GanYingModel
    {
        /// <summary>
        /// true表示急停
        /// </summary>
        public bool IsJiTing { get; set; } = false;

        /// <summary>
        /// true 表示启动测试
        /// </summary>
        public bool QiDongTest { get; set; } = false;

        /// <summary>
        /// true是有物品
        /// </summary>
        public bool WuPingZhuangTai { get; set; } = false;

        /// <summary>
        /// true表示处于点击模式
        /// </summary>
        public bool IsDianJian { get; set; } = false;
        /// <summary>
        /// 通道状态
        /// </summary>
        public TDStateType TDState { get; set; } = TDStateType.ZhengChangKongXian;

        public bool PeiFangGengGai { get; set; } = false;

        public string QieHuanXingHao { get; set; } = "";
        public string GetTDState()
        {
            return ChangYong.GetEnumDescription(TDState);
        }
    }


    /// <summary>
    /// 通道状态
    /// </summary>
    public enum TDStateType
    {
        /// <summary>
        /// 调试空闲
        /// </summary>
        [Description("调试空闲")]
        TiaoShiKongXian,
        /// <summary>
        /// 调试工作
        /// </summary>
        [Description("调试工作")]
        TiaoShiGongZuo,
        /// <summary>
        /// 正常空闲
        /// </summary>
        [Description("正常空闲")]
        ZhengChangKongXian,
        /// <summary>
        /// 正常工作
        /// </summary
        [Description("正常工作")]
        ZhengChengGongZuo,
    }
    public class ShouDongKongZhiModel
    {
        /// <summary>
        /// true是启用mes
        /// </summary>
        public bool IsQiYongMes { get; set; } = true;
        /// <summary>
        /// true是在调试模式
        /// </summary>
        public bool IsTiaoShi { get; set; } = false;
        /// <summary>
        /// true是NG就跳出
        /// </summary>
        public bool IsNGTiaoChu { get; set; } = true;
        /// <summary>
        /// true 表示暂停
        /// </summary>
        public bool IsZhanTing { get; set; } = false;

        /// <summary>
        /// true表示跳出整个检测
        /// </summary>
        public bool IsTiaoChuJianCe { get; set; } = false;

        /// <summary>
        ///true 表示断点跳出检测
        /// </summary>
        public bool IsJiXuCeShi { get; set; } = false;

        /// <summary>
        /// true  表示老化测试
        /// </summary>
        public LaoHuaType LaoHuaTest { get; set; } = LaoHuaType.WuLaoHua;
        /// <summary>
        /// 老化次数
        /// </summary>
        public int LaoHuaCiShu { get; set; } = 0;

        /// <summary>
        /// 老化剩余次数
        /// </summary>
        public int LaoHuaShengYuCiShu { get; set; } = 0;

        /// <summary>
        /// 老化合格次数
        /// </summary>
        public int LaoHuaHeGeCiShu { get; set; } = 0;

        /// <summary>
        /// 老化剩余不合格次数
        /// </summary>
        public int LaoHuaBuHeGeCiShu { get; set; } = 0;
        /// <summary>
        /// true 表示手动测试
        /// </summary>
        public bool SDTest { get; set; } = false;

        /// <summary>
        /// 调试要测试那些
        /// </summary>
        public List<int> JiGeXuHao = new List<int>();
    }
    /// <summary>
    /// 老化的类型
    /// </summary>
    public enum LaoHuaType
    {
        HeGeLaoHua,
        CiShuLaoHua,
        WuLaoHua,
    } 

    /// <summary>
    /// 用于配置
    /// </summary>
    public class PeiZhiModel
    {
        /// <summary>
        /// true 表示遮挡
        /// </summary>
        public bool IsXianShiZheDang { get; set; } = true;
        /// <summary>
        /// true表示需要扫码 在调试模式下失效
        /// </summary>
        public bool IsSaoMa { get; set; } = true;

        /// <summary>
        /// 配方文件
        /// </summary>
        public string PeiFangName { get; set; } = "";

        public string JiaJuHao { get; set; } = "";
        /// <summary>
        /// 设备属于哪个组
        /// </summary>
        public string SheBeiZu { get; set; } = "";
        /// <summary>
        /// 启动的测试名称
        /// </summary>
        public string QiDongName { get; set; } = "";

        /// <summary>
        /// 急停的名称
        /// </summary>
        public string JiTingName { get; set; } = "";
        /// <summary>
        /// 停止名称
        /// </summary>
        public string TingZhiName { get; set; } = "";
        /// <summary>
        /// 扫码名称
        /// </summary>
        public string SaoMaName { get; set; } = "";
        /// <summary>
        /// 扫码值
        /// </summary>
        public string DuSaoMaZhi { get; set; } = "";

        /// <summary>
        /// 状态名称
        /// </summary>
        public string WuPingStateName { get; set; } = "";

        public string QieHuanXingHaoName { get; set; } = "";

        /// <summary>
        /// 1 是等于 2是包含
        /// </summary>
        public int QieHuanType { get; set; } = 1;

        /// <summary>
        /// 1表示不传过站 
        /// </summary>
        public int BuChuanGuoZhan { get; set; } = 2;
        /// <summary>
        /// 1表示自动切配方
        /// </summary>
        public int IsZiDongQiePeiFang { get; set; } = 0;

        public string MesDuState { get; set; } = "";

        public string MesXieState { get; set; } = "";

        public string DuDianJianMoShi { get; set; } = "";
    }
}
