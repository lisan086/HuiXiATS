using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATSJianMianJK.XiTong.Model;

namespace ATSJianCeXianTi.Model
{
    /// <summary>
    /// 界面开关调用
    /// </summary>
    public  class JieMianModel
    {
        /// <summary>
        /// 通道ID
        /// </summary>
        public int TDID { get; set; }
       
        /// <summary>
        /// true表示开
        /// </summary>
        public bool CaoZuo { get; set; } = false;

        public LaoHuaType LaoHuaType { get; set; } = LaoHuaType.WuLaoHua;

        /// <summary>
        /// 
        /// </summary>
        public int ZuoWei { get; set; } = -1;

        /// <summary>
        /// 配方名称
        /// </summary>
        public string PeiFangName { get; set; } = "";

        public ZhiJieGuo ZhiJieGuo { get; set; } = new ZhiJieGuo();

    }
    public class TangTiShiKuangModel
    {
        public int TDID { get; set; }

        public string Msg { get; set; } = "";

        public bool IsXuanZe { get; set; } = false;

        public object FanHuiJieGuo { get; set; } = "";
    }
  
    /// <summary>
    /// 事件的类型
    /// </summary>
    public enum EventType
    {
    
        /// <summary>
        /// 测试项目
        /// </summary>
        TestXiangMu,
      
          
        /// <summary>
        /// 提示框
        /// </summary>
        TDTiShiKuang,
      
    }

    /// <summary>
    /// 操作类型
    /// </summary>
    public enum DoType
    {
        /// <summary>
        /// 打开设备
        /// </summary>
        Open,
        /// <summary>
        /// 关闭软件
        /// </summary>
        Close,
        /// <summary>
        /// 操作暂停
        /// </summary>
        CaoZuoZanTing,
        /// <summary>
        /// 操作上传MES
        /// </summary>
        CaoZuoMes,
        /// <summary>
        /// 操作调试
        /// </summary>
        CaoZuoTiaoShi,
        /// <summary>
        /// 操作NG跳出
        /// </summary>
        CaoZuoNGTiaoChu,
        /// <summary>
        /// 操作断点跳出
        /// </summary>
        CaoZuoDuanDianTiaoChu,
        /// <summary>
        /// 总跳出
        /// </summary>
        CaoZuoZongTiaoChu,    
        /// <summary>
        /// 断点
        /// </summary>
        DuanDian,
        /// <summary>
        /// 单步执行
        /// </summary>
        DanBuZhiXing,
        /// <summary>
        /// 加载配方
        /// </summary>
        JiaZaiPeiFang,
        /// <summary>
        /// 手动启动
        /// </summary>
        ShouDongTest,
        /// <summary>
        /// 老化测试
        /// </summary>
        LaoHuaTest,    
        /// <summary>
        /// 换班
        /// </summary>
        HuanBan,
        /// <summary>
        /// 测试多序列
        /// </summary>
        JiGeXuLie,
        /// <summary>
        /// 单步跳出
        /// </summary>
        DanBuTiaoChu,

        UIFanHuiJieGuo,

        DianJianMoShi,
    }

   
}
