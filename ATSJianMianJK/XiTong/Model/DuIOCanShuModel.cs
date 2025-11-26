using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommLei.JiChuLei;

namespace ATSJianMianJK.XiTong.Model
{
    public  class DuIOCanShuModel
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
        /// IO类型
        /// </summary>
        public IOType Type { get; set; } = IOType.IOKeYiYunXin;

        public string CanShu { get; set; } = "";

        public bool ShangYiCiValue { get; set; } = false;

        public DateTime DateTime { get; set; } = DateTime.Now;

        public double MiaShu{ get; set; } = 1;

        public int GaiBian { get; set; } = 0;
        /// <summary>
        /// 寄存器
        /// </summary>
        public List<DuModel> LisJiCunQi { get; set; } = new List<DuModel>();

        /// <summary>
        /// 1 采用的是&& 2采用||
        /// </summary>
        public PanDuanType PanDuanFangShi { get; set; } = PanDuanType.YuPanDuan;

        /// <summary>
        /// 获取读出来的内容是否满足情况
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool GetBool(bool meiyouzhi)
        {
            bool zhen = false;
            if (LisJiCunQi.Count > 0)
            {
                switch (PanDuanFangShi)
                {
                    case PanDuanType.YuPanDuan:
                        {
                            if (LisJiCunQi.Count == 1)
                            {
                                int zhi = PanDuan(LisJiCunQi[0]);
                                if (zhi == 0)
                                {
                                    zhen = meiyouzhi;
                                }
                                else
                                {
                                    zhen = zhi == 1;
                                }
                            }
                            else
                            {
                                zhen = true;
                                for (int i = 0; i < LisJiCunQi.Count; i++)
                                {
                                    if (PanDuan(LisJiCunQi[i]) != 1)
                                    {
                                        zhen = false;
                                        break;
                                    }
                                }

                            }
                        }
                        break;
                    case PanDuanType.HuoPanDun:
                        {
                            if (LisJiCunQi.Count == 1)
                            {
                                int zhi = PanDuan(LisJiCunQi[0]);
                                if (zhi == 0)
                                {
                                    zhen = meiyouzhi;
                                }
                                else
                                {
                                    zhen = zhi == 1;
                                }
                            }
                            else
                            {
                                zhen = false;
                                for (int i = 0; i < LisJiCunQi.Count; i++)
                                {
                                    if (PanDuan(LisJiCunQi[i]) == 1)
                                    {
                                        zhen = true;
                                        break;
                                    }
                                }

                            }
                        }
                        break;

                    default:
                        break;
                }

            }
            else
            {
                zhen = meiyouzhi;
            }
            return zhen;
        }

        /// <summary>
        /// true表示存在寄存器
        /// </summary>
        /// <returns></returns>
        public bool IsCunZaiJiCunQi()
        {
            return LisJiCunQi.Count > 0;
        }

        /// <summary>
        /// 0表示是空 1表示满足匹配 2表示不满足
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private int PanDuan(DuModel model)
        {
            if (model == null)
            {
                return 0;
            }

            if (model.PiPeiType == PiPeiType.BaoHan)
            {
                return model.PiPeiValue.IndexOf(model.Value.ToString()) >= 0 ? 1 : 2;
            }
            else if (model.PiPeiType == PiPeiType.DaYu)
            {
                double zhi = ChangYong.TryDouble(model.Value, 0d);
                double pandunzhi = 0d;
                if (model.LisPiPeiValue.Count > 0)
                {
                    pandunzhi = ChangYong.TryDouble(model.LisPiPeiValue[0], 0d);
                }
                return zhi > pandunzhi ? 1 : 2;
            }
            else if (model.PiPeiType == PiPeiType.DaYuDengYu)
            {
                double zhi = ChangYong.TryDouble(model.Value, 0d);
                double pandunzhi = 0d;
                if (model.LisPiPeiValue.Count > 0)
                {
                    pandunzhi = ChangYong.TryDouble(model.LisPiPeiValue[0], 0d);
                }
                return zhi >= pandunzhi ? 1 : 2;
            }
            else if (model.PiPeiType == PiPeiType.DengYu)
            {
                double zhi = ChangYong.TryDouble(model.Value, 0d);
                double pandunzhi = 0d;
                if (model.LisPiPeiValue.Count > 0)
                {
                    pandunzhi = ChangYong.TryDouble(model.LisPiPeiValue[0], 0d);
                }
                return zhi == pandunzhi ? 1 : 2;
            }
            else if (model.PiPeiType == PiPeiType.LiangZheZhiJian)
            {
                double zhi = ChangYong.TryDouble(model.Value, 0d);
                double pandunzhi1 = 0d;
                double panduanzhi2 = 0d;
                if (model.LisPiPeiValue.Count >= 2)
                {
                    pandunzhi1 = ChangYong.TryDouble(model.LisPiPeiValue[0], 0d);
                    panduanzhi2 = ChangYong.TryDouble(model.LisPiPeiValue[1], 0d);
                }
                return (zhi >= pandunzhi1 && zhi <= panduanzhi2) ? 1 : 2;
            }
            else if (model.PiPeiType == PiPeiType.XiaoYu)
            {
                double zhi = ChangYong.TryDouble(model.Value, 0d);
                double pandunzhi = 0d;
                if (model.LisPiPeiValue.Count > 0)
                {
                    pandunzhi = ChangYong.TryDouble(model.LisPiPeiValue[0], 0d);
                }
                return zhi < pandunzhi ? 1 : 2;
            }
            else if (model.PiPeiType == PiPeiType.XiaoYuDengYu)
            {
                double zhi = ChangYong.TryDouble(model.Value, 0d);
                double pandunzhi = 0d;
                if (model.LisPiPeiValue.Count > 0)
                {
                    pandunzhi = ChangYong.TryDouble(model.LisPiPeiValue[0], 0d);
                }
                return zhi <= pandunzhi ? 1 : 2;
            }
            return 2;
        }

        public bool IsKeYiEvent()
        {
            if (Type==IOType.IOPingZhengCount || Type==IOType.IOChuFaEvent)
            {
                return true;
            }
            return false;
        }
        public bool IsKeYiChuFa()
        {
            if (GaiBian == 1)
            {
                if ((DateTime.Now - DateTime).TotalSeconds >= MiaShu)
                {
                    GaiBian = 0;
                    DateTime = DateTime.Now;
                    return true;
                }
            }
           
            return false;
        }
    }

    public enum IOType
    {
        IOKeYiYunXin,
        IOPingZhengCount,
        IOChuFaEvent,
        IOQiTa,
    }
    /// <summary>
    /// 判断方式
    /// </summary>
    public enum PanDuanType
    {
        /// <summary>
        /// &的判断
        /// </summary>
        YuPanDuan,
        /// <summary>
        /// 或判断
        /// </summary>
        HuoPanDun,

    }
}
