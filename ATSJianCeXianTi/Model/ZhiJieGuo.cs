using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSheBei.Model;

namespace ATSJianCeXianTi.Model
{
    public  class ZhiJieGuo
    {
           
        /// <summary>
        /// true  表示下一步
        /// </summary>
        public bool IsXiaYiBu { get; set; } = true;

        /// <summary>
        /// 返回的值
        /// </summary>
        public object RecZhi { get; set; } = "";

        /// <summary>
        /// 是否合格
        /// </summary>
        public bool IsHeGe { get; set; } = false;

        public string TestTime { get; set; } = "0";

        public int IsString { get; set; } = 1;

        public ZhiJieGuo FuZhi()
        {
            ZhiJieGuo model = new ZhiJieGuo();
            model.IsXiaYiBu = IsXiaYiBu;
            model.IsHeGe = IsHeGe;
            model.TestTime = TestTime;
            model.IsString = IsString;
            model.RecZhi = RecZhi;
        
            return model;
        }
    }


   
}
