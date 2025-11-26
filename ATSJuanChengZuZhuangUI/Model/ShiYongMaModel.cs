using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSJuanChengZuZhuangUI.Model
{
    public  class ShiYongMaModel
    {
        public string Ma { get; set; }

        /// <summary>
        /// 1 表示使用
        /// </summary>
        public int IsShiYong { get; set; } = 0;

        public string ShiYongTime { get; set; } = "";
    }
}
