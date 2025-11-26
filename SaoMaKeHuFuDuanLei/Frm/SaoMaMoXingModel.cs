using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiBanSaoMaQi.Frm;
using YiBanSaoMaQi.Model;

namespace SaoMaKeHuFuDuanLei.Frm
{
    public class SaoMaMoXingModel
    {
        public int XunShuHao { get; set; }
        public List<SaoMaShuJuModel> YiDuiShuJu { get; set; } = new List<SaoMaShuJuModel>();

        public string BiaoShi { get; set; } = "";


        public int PaiXu { get; set; } = 0;

        /// <summary>
        /// 1  表示结束
        /// </summary>
        public int JieSu { get; set; } = 0;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("new barcode.barcode");
            for (int i = 0; i < YiDuiShuJu.Count; i++)
            {
                if (i == YiDuiShuJu.Count - 1)
                {
                    sb.AppendLine($"{YiDuiShuJu[i].ShunXuID}!{YiDuiShuJu[i].ShangMa}!{YiDuiShuJu[i].HouMa}!{BiaoShi}");
                }
                else
                {
                    sb.AppendLine($"{YiDuiShuJu[i].ShunXuID}!{YiDuiShuJu[i].ShangMa}!{YiDuiShuJu[i].HouMa}!{BiaoShi}");
                }
            }
            return sb.ToString();
        }
    }

    public class ZiZengIDModel
    {
        public int ID { get; set; } = 1;
    }
}
