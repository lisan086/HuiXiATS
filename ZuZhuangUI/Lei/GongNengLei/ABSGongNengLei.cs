using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATSJianMianJK.Log;
using ZuZhuangUI.Model;

namespace ZuZhuangUI.Lei.GongNengLei
{
    public abstract  class ABSGongNengLei
    {
        public int SheBeiID { get; set; } = -1;

        public string SheBeiName { get; set; } = "";
        public abstract void IniData(SheBeiZhanModel model);


        public abstract void Close();

        public virtual void CaoZuo(DoType doType,JieMianCaoZuoModel model)
        { 

        }

        protected  void WriteLog(RiJiEnum riji, string msg)
        {
            RiJiLog.Cerate().Add(riji, $"{SheBeiName}:{msg}", SheBeiID);
        }

        
    }
}
