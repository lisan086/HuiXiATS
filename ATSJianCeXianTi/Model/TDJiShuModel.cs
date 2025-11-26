using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSJianCeXianTi.Model
{
    /// <summary>
    /// 通道计数model
    /// </summary>
    public class TDJiShuModel
    {
        /// <summary>
        /// 通道iD
        /// </summary>
        public int TDID { get; set; } = 0;
      
        /// <summary>
        /// 总合格数
        /// </summary>
        public int ZHeGeCount { get; set; } = 0;
        /// <summary>
        /// 总NG数
        /// </summary>
        public int ZNGCount { get; set; } = 0;


        /// <summary>
        /// 换班合格数
        /// </summary>
        public int HBHeGeCount { get; set; } = 0;

        /// <summary>
        /// 换班NG数
        /// </summary>
        public int HBNGCount { get; set; } = 0;
    }

    public class GongZhuanModel
    {
      
        public int TDID { get; set; }
        public string GongZhuangName { get; set; } = "";

        public string JianLiTime { get; set; } = "";

        public string GengHuanTime { get; set; } = "";

        /// <summary>
        /// 1表示正在用
        /// </summary>
        public int IsZhengZaiYong { get; set; } = 0;

        public List<ZhenModel> ZhenModels { get; set; } = new List<ZhenModel>();

        /// <summary>
        /// 1 高频 2低频
        /// </summary>
        /// <param name="type"></param>
        public void SetShuJu(int type)
        {
            for (int i = 0; i < ZhenModels.Count; i++)
            {
                if (ZhenModels[i].IsGaoPin==type)
                {
                    ZhenModels[i].ShiYongCount++;
                }
            }
        }
        /// <summary>
        /// 获取数量 0表示正常 1表示超过限值还可以用 2表示超过探针数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="ischaoxian"></param>
        /// <returns></returns>
        public int GetShuJu(int type,out int ischaoxian)
        {
            int count = 0;
            ischaoxian = 0;

            for (int i = 0; i < ZhenModels.Count; i++)
            {
                if (ZhenModels[i].IsGaoPin == type)
                {
                    count+= ZhenModels[i].ShiYongCount;
                    float shiyong = ZhenModels[i].ShiYongCount;
                    float zongshu = ZhenModels[i].XianZhiCount;
                    if (shiyong >= zongshu)
                    {
                        ischaoxian = 2;
                    }
                    else
                    {
                        float bnili = (shiyong / zongshu) * 100f;
                        if (bnili>= ZhenModels[i].BaoJingBaiFenBi)
                        { 
                            ischaoxian = 1;
                        }
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// 更换探针
        /// </summary>
        /// <param name="type"></param>
        public void GengHuanTangZheng(int type)
        {
            for (int i = 0; i < ZhenModels.Count; i++)
            {
                if (ZhenModels[i].IsGaoPin == type)
                {
                    ZhenModels[i].ShiYongCount=0;
                }
            }
        }
    }

    public class ZhenModel
    {
        /// <summary>
        /// 1:高频针 2:低频针
        /// </summary>
        public int IsGaoPin { get; set; } = 1;

        /// <summary>
        /// 使用次数
        /// </summary>
        public int XianZhiCount { get; set; } = 50000;

        /// <summary>
        /// 使用次数
        /// </summary>
        public int ShiYongCount { get; set; } = 0;

        /// <summary>
        /// 报警限值
        /// </summary>
        public float BaoJingBaiFenBi { get; set; } = 75;
    }


    public class TDTianShuModel
    {

        public int TDID { get; set; } = 0;
        public string Time { get; set; } = "";

    
        public List<ShiShuModel> JiShu { get; set; } = new List<ShiShuModel>();

        /// <summary>
        /// 1是合格数 2是不合格数 3是总数 4是比例
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public double GetShuJu(int type)
        {
            if (type == 1)
            {
                int hegeshu = 0;
                for (int i = 0; i < JiShu.Count; i++)
                {
                    hegeshu += JiShu[i].ZHeGeCount;
                }
                return hegeshu;
            }
            else if (type == 2)
            {
                int hegeshu = 0;
                for (int i = 0; i < JiShu.Count; i++)
                {
                    hegeshu += JiShu[i].ZNGCount;
                }
                return hegeshu;
            }
            else if (type == 3)
            {
                int hegeshu = 0;
                for (int i = 0; i < JiShu.Count; i++)
                {
                    hegeshu += JiShu[i].ZNGCount;
                    hegeshu += JiShu[i].ZHeGeCount;
                }
                return hegeshu;
            }
            else if (type == 4)
            {
                double buhgeshu = 0;
                double hegeshu = 0;
                for (int i = 0; i < JiShu.Count; i++)
                {
                    buhgeshu += JiShu[i].ZNGCount;
                    hegeshu += JiShu[i].ZHeGeCount;
                }
                double zongshu = buhgeshu + hegeshu;
                if (zongshu == 0)
                {
                    zongshu = 1;
                }
                return (hegeshu / zongshu) * 100;
            }
            return 0;
        }

        public void SetShuJu(bool ishege)
        {

            int huoquxiaoshi = DateTime.Now.Hour;
            bool iscunzai = false;
            for (int i = 0; i < JiShu.Count; i++)
            {
                if (JiShu[i].XiaoShi == huoquxiaoshi)
                {
                    if (ishege)
                    {
                        JiShu[i].ZHeGeCount++;
                    }
                    else
                    {
                        JiShu[i].ZNGCount++;
                    }
                    iscunzai = true;
                    break;
                }
            }
            if (iscunzai == false)
            {
                ShiShuModel shiShuModel = new ShiShuModel();
                shiShuModel.XiaoShi = huoquxiaoshi;
                if (ishege)
                {
                    shiShuModel.ZHeGeCount++;
                }
                else
                {
                    shiShuModel.ZNGCount++;
                }
                JiShu.Add(shiShuModel);
            }
        }

      
    }

    public class ShiShuModel
    {

        public int XiaoShi { get; set; } = 0;
        /// <summary>
        /// 总合格数
        /// </summary>
        public int ZHeGeCount { get; set; } = 0;
        /// <summary>
        /// 总NG数
        /// </summary>
        public int ZNGCount { get; set; } = 0;
    }

    public class PeiFangJiShuModel
    {

        public int TDID { get; set; } = 0;

        /// <summary>
        /// 配方名称
        /// </summary>
        public string PeiFangName { get; set; } = "";

        public int IsZhengYong { get; set; } = 0;

        /// <summary>
        /// 总合格数
        /// </summary>
        public int ZHeGeCount { get; set; } = 0;
        /// <summary>
        /// 总NG数
        /// </summary>
        public int ZNGCount { get; set; } = 0;

        /// <summary>
        /// 1是合格数 2是不合格数 3是总数 4是比例
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public double GetShuJu(int type)
        {
            if (type == 1)
            {


                return ZHeGeCount;
            }
            else if (type == 2)
            {


                return ZNGCount;
            }
            else if (type == 3)
            {

                return ZNGCount + ZHeGeCount;
            }
            else if (type == 4)
            {
                double buhgeshu = ZNGCount;
                double hegeshu = ZHeGeCount;

                double zongshu = buhgeshu + hegeshu;
                if (zongshu == 0)
                {
                    zongshu = 1;
                }
                return (hegeshu / zongshu) * 100;
            }
            return 0;
        }

        public void SetShuJu(bool ishege)
        {
            if (ishege)
            {
                ZHeGeCount++;
            }
            else
            {
                ZNGCount++;
            }
        }

    }
}
