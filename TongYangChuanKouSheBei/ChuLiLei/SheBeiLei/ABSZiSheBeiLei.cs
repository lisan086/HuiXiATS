using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongWangSheBei.Model;

namespace XiangTongChuanKouSheBei.ChuLiLei.SheBeiLei
{
    internal abstract  class ABSZiSheBeiLei
    {
        protected ZiSheBeiModel ZiSheBeiModel;
        public void SetCanShu(ZiSheBeiModel sheBeiModel)
        {
            ZiSheBeiModel=sheBeiModel;
        }

        /// <summary>
        /// 获取发送的指令
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public abstract FanHuiModel GetSendCMD(CunModel model);

        /// <summary>
        /// 1 接收数据
        /// </summary>
        /// <returns></returns>
        public abstract void JieShouShuJu(byte[] shuju,int changdu);

        /// <summary>
        /// 1是校验数据完成
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public abstract int JiaYanShuJu(DuZhiLingModel model);

        public abstract void ClearData();
    }


    public class FanHuiModel
    {
        public List<List<byte>> SendData { get; set; } = new List<List<byte>>();

        public int XieTime { get; set; } = 20;
    }
}
