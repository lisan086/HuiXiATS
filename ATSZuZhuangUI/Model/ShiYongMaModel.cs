using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommLei.JiChuLei;

namespace ATSJuanChengZuZhuangUI.Model
{
    public  class ShuChuMaModel
    {
        public List<string> LisMa { get; set; } = new List<string>();

        public string Ma { get; set; } = "";

        public override string ToString()
        {
            return $"{ChangYong.FenGeDaBao(LisMa," ")} {Ma}";
        }

        public string GetMa(string mamingcheng)
        {
            if (mamingcheng == "Ma0")
            {
                return Ma;
            }
            else
            {
                string qihuan = mamingcheng.Replace("Ma","");
                int index = ChangYong.TryInt(qihuan,-1);
                if (index <= 0 || index-1 >= LisMa.Count)
                {
                    return "";
                }
                else
                {
                    return LisMa[index - 1];
                }
            }
        }
    }
}
