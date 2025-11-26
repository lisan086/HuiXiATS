using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSJianCeXianTi.Model;

namespace ATSJianCeXianTi.JKKJ.UIFrm
{
    interface IFUIFrm
    {
        event Action<ZhiJieGuo> FanHuiJieGuoEvent;
        /// <summary>
        /// 窗体的ID
        /// </summary>
        int TypeID { get; }
        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="canshu"></param>
        /// <returns></returns>
        void SetCanShu(TangChuanUIModel canshu);
        void SetXianShi(bool isxianshi);
        Form GetFrm();
    }
}
