using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommLei.JiChuLei;

namespace ATSJianMianJK.XiTong.Model
{
    public  class DuShuJuModel
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
        /// 数据类型
        /// </summary>
        public string Type { get; set; } = "";
        /// <summary>
        /// 寄存器
        /// </summary>
        public List<DuModel> LisJiCunQi { get; set; } = new List<DuModel>();


        public List<DuModel> GetDuModel()
        {
            List<DuModel> lis = new List<DuModel>();
            foreach (var item in LisJiCunQi)
            {
                lis.Add(item.FuZhi());
            }
            return lis;
        }
        /// <summary>
        /// true表示存在寄存器
        /// </summary>
        /// <returns></returns>
        public bool IsCunZaiJiCunQi()
        {
            return LisJiCunQi.Count > 0;
        }
     
    }
}
