using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATSJianCeXianTi.JKKJ.PeiZhi;
using ATSJianMianJK.Log;
using CommLei.JiChuLei;

namespace ATSJianCeXianTi.Model
{
    public class TangChuanUIModel
    {
        public int TDID { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        public string LuJing { get; set; } = "";
        /// <summary>
        /// 信息
        /// </summary>
        public string XingXi { get; set; } = "";

        /// <summary>
        /// true是扫码
        /// </summary>
        public bool IsSaoMa { get; set; } = false;

        /// <summary>
        /// 上限
        /// </summary>
        public string UpStr { get; set; } = "";

        /// <summary>
        /// 下限
        /// </summary>
        public string LowStr { get; set; } = "";

        /// <summary>
        /// 1 是默认的数据
        /// </summary>
        public int Type { get; set; } = 1;

        public float BiLi { get; set; } = 1;
        public List<ZhuXingModel> LisShuJu { get; set; } = new List<ZhuXingModel>();
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < LisShuJu.Count; i++)
            {
                if (i == LisShuJu.Count - 1)
                {
                    sb.Append(string.Format("[{0} {1},{2}]", LisShuJu[i].DianWei, LisShuJu[i].XValue, LisShuJu[i].YValue));
                }
                else
                {
                    sb.Append(string.Format("[{0} {1},{2}],", LisShuJu[i].DianWei, LisShuJu[i].XValue, LisShuJu[i].YValue));
                }
            }
            return base.ToString();
        }

        public bool FenXiDianWei(out double zuidaozhi)
        {
            zuidaozhi = -99999;
            if (Type == 2)
            {
                if (LisShuJu.Count>=2)
                {
                    LisShuJu.RemoveAt(0);
                    LisShuJu.RemoveAt(LisShuJu.Count-1);
                }
                if (LisShuJu.Count > 1)
                {

                    float wushangs = ChangYong.TryFloat(UpStr, 4);
                    float wushangx = ChangYong.TryFloat(LowStr, -4);
                    int count = LisShuJu.Count;
                    bool ishege = true;
                    for (int i = 0; i < count; i++)
                    {
                        if (i + 1 < count)
                        {
                            float x1 = LisShuJu[i].XValue;
                            float y1 = LisShuJu[i].YValue;
                            float x2 = LisShuJu[i + 1].XValue;
                            float y2 = LisShuJu[i + 1].YValue;
                            float chay = y2 - y1;
                            float chax = x2 - x1;
                            if (zuidaozhi < chay)
                            {
                                zuidaozhi = chay;
                            }
                            if (zuidaozhi < chax)
                            {
                                zuidaozhi = chax;
                            }
                            bool zhen1 = false;
                            if (((chay) >= wushangx && (chay) <= wushangs) && (chax >= wushangx && chax <= wushangs))
                            {
                             
                                zhen1 = true;
                            }
                            if (zhen1==false)
                            { 

                            }
                            if (ishege)
                            {
                                ishege = zhen1;
                            }
                        }
                        else
                        {
                            RiJiLog.Cerate().Add(RiJiEnum.TDXieJiLu, ChangYong.HuoQuJsonStr(LisShuJu), TDID);
                            return ishege;
                        }
                    }
                }
                RiJiLog.Cerate().Add(RiJiEnum.TDXieJiLu, ChangYong.HuoQuJsonStr(LisShuJu), TDID);
                return false;
            }
            else
            {
                RiJiLog.Cerate().Add(RiJiEnum.TDXieJiLu, ChangYong.HuoQuJsonStr(LisShuJu), TDID);
                return false;
            }
        }
    }

 
}
