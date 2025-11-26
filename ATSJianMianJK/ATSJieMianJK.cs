using ATSJianMianJK.Log;
using ATSJianMianJK.QuanXian;
using CommLei.JieMianLei;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATSJianMianJK
{
    /// <summary>
    /// 挂接界面接口的
    /// </summary>
    public abstract  class ATSJieMianJK
    {

        /// <summary>
        /// 加载控件之前会给一些方法调用
        /// </summary>
        public abstract void SetCanShu(ZiYuanModel ziYuanModel);
        /// <summary>
        /// 加载主控件
        /// </summary>
        /// <returns></returns>
        public abstract Control LoadKJ();

        /// <summary>
        /// true表示接受缩放
        /// </summary>
        /// <returns></returns>
        public abstract bool IsJieShouSouFang();

        /// <summary>
        /// 关闭用的
        /// </summary>
        public abstract void Close();

        /// <summary>
        /// 获取标题
        /// </summary>
        /// <returns></returns>
        public abstract string GetBiaoTi();

       

        /// <summary>
        /// 获取权限菜单
        /// </summary>
        /// <returns></returns>
        public abstract List<QuanXianModel> GetQuanXian();

        /// <summary>
        /// 切换用户的变化
        /// </summary>
        public abstract void QieHuanYongHu();

        /// <summary>
        /// 获取版本号
        /// </summary>
        /// <returns></returns>
        public abstract string GetBanBenHao();

        /// <summary>
        /// 设置日记用的
        /// </summary>
        /// <param name="lismodel"></param>
        public abstract void  SetLog(List<RiJiModel> lismodel);


        public virtual void ShuaXin()
        { }

    }


    /// <summary>
    /// 资源参数
    /// </summary>
    public class ZiYuanModel
    {
       
        /// <summary>
        /// 权限
        /// </summary>
        public QuanXianLei QuanXian { get; set; }
        /// <summary>
        /// 跨线程调用
        /// </summary>
        public Controlinkove It { get; set; }

        /// <summary>
        /// 提示框
        /// </summary>
        public Action<string> TiShiKuang { get; set; }

        /// <summary>
        /// 是否选择框
        /// </summary>
        public Func<string,bool> ShiYuFou { get; set; }

        /// <summary>
        /// 用于执行的耗时任务
        /// </summary>
        public Action<Action> HaoShiRenWu { get; set; }
    }
}
