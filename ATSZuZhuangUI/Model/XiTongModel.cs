using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZuZhuangUI.Model;

namespace ATSFoZhaoZuZhuangUI.Model
{
    public  class XiTongModel
    {
        public ShuJuLisModel QiDongXinHaoName { get; set; } = new ShuJuLisModel();

        public ShuJuLisModel BangDingHuanXing { get; set; } = new ShuJuLisModel();

        /// <summary>
        /// 1是前包含 2是后包含 3是等于 4是包含
        /// </summary>
        public PanDuanMoShi PanDuanMoShi { get; set; } = PanDuanMoShi.BaoHan;

        public bool IsQieHuan { get; set; } = false;

        public ShuJuLisModel XieShuJu { get; set; } =new ShuJuLisModel();

        public string NGZhi { get; set; } = "";

        public string PassZhi { get; set; } = "";
        public string PiPeiZhi { get; set; } = "";
    }

    public enum PanDuanMoShi
    { 
        QianBaoHan,
        HouBaoHan,
        DengYu,
        BaoHan,
    }
}
