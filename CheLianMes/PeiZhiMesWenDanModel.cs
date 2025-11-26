using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheLianMes
{
    public  class PeiZhiMesWenDanModel
    {
        public string SheBeiBianHao { get; set; } = "";

        public string QianZhanWangZhi { get; set; } = "http://10.70.90.111:8712/op/checksn";

        public string ShuJuWangZhi { get; set; } = "http://10.70.90.111:8712/assy-aoi/upload";

        public string YuanGongBianMa { get; set; } = "";

        public string TongDaoName { get; set; } = "";
    }
}
