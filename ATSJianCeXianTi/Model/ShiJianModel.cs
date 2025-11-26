using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSJianCeXianTi.Model
{
    public  class ShiJianModel
    {
        public int TDID { get; set; } = -1;

        public string PeiFangName { get; set; } = "";

        /// <summary>
        /// 测试的项目
        /// </summary>
        public XiangMuModel XiangMuModel { get; set; } = new XiangMuModel();

    

        /// <summary>
        /// 提示框的数据
        /// </summary>
        public TangTiShiKuangModel TangTiShiKuangModel { get; set; } = new TangTiShiKuangModel();

        /// <summary>
        /// 配方数据
        /// </summary>
        public ZongTestModel ZongTestModel { get; set; } = new ZongTestModel();
    }
}
