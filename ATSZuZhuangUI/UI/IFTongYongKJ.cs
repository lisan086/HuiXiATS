using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSJianMianJK;
using ATSJianMianJK.Log;
using ZuZhuangUI.Model;

namespace ATSZuZhuangUI.Lei.GongNengLei.KongJian
{
    public interface IFTongYongKJ
    {
        void SetLog(int tdtd, RiJiModel riji);
        void ShuXin();
        void SetCanShu(ZiYuanModel ziYuanModel);
        void SetModel(SheBeiZhanModel ziYuanModel);
        void SetCanShu(EventType arg1, JieMianShiJianModel model);
        void SetTanChuan(int tdtd,string msg);
        void Close();
       
    }
}
