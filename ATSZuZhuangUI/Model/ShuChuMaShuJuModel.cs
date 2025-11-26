using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZuZhuangUI.Model
{
    public  class ShuChuMaShuJuModel
    {
        public string Ma1 { get; set; } = "";
        public string Ma2 { get; set; } = "";
        public string Ma3 { get; set; } = "";
        public string Ma4 { get; set; } = "";
        public string Ma5 { get; set; } = "";
        public string Ma6 { get; set; } = "";
        public string Ma7 { get; set; } = "";
        public string Ma8 { get; set; } = "";


        public bool IsQuanBuWeiKong()
        {
            bool flag = true;
            if (string.IsNullOrEmpty(Ma1)&& string.IsNullOrEmpty(Ma2) && string.IsNullOrEmpty(Ma3) && string.IsNullOrEmpty(Ma4) && string.IsNullOrEmpty(Ma5) && string.IsNullOrEmpty(Ma6) && string.IsNullOrEmpty(Ma7) && string.IsNullOrEmpty(Ma8))
            {
                flag = false;
            }   
            return flag; 
        }

        public string GetLog()
        { 
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(Ma1) == false)
            {
               sb.Append(Ma1+ " ");
            }
            if (string.IsNullOrEmpty(Ma2) == false)
            {
                sb.Append(Ma2 + " ");
            }
            if (string.IsNullOrEmpty(Ma3) == false)
            {
                sb.Append(Ma3 + " ");
            }
            if (string.IsNullOrEmpty(Ma4) == false)
            {
                sb.Append(Ma4 + " ");
            }
            if (string.IsNullOrEmpty(Ma5) == false)
            {
                sb.Append(Ma5 + " ");
            }
            if (string.IsNullOrEmpty(Ma6) == false)
            {
                sb.Append(Ma6 + " ");
            }
            if (string.IsNullOrEmpty(Ma7) == false)
            {
                sb.Append(Ma7 + " ");
            }
            if (string.IsNullOrEmpty(Ma8) == false)
            {
                sb.Append(Ma8 + " ");
            }
            return sb.ToString();
        }

        public List<string> GetLisMa()
        {
            List<string> sb = new List<string>();
            if (string.IsNullOrEmpty(Ma1) == false)
            {
                sb.Add(Ma1);
            }
            if (string.IsNullOrEmpty(Ma2) == false)
            {
                sb.Add(Ma2);
            }
            if (string.IsNullOrEmpty(Ma3) == false)
            {
                sb.Add(Ma3);
            }
            if (string.IsNullOrEmpty(Ma4) == false)
            {
                sb.Add(Ma4);
            }
            if (string.IsNullOrEmpty(Ma5) == false)
            {
                sb.Add(Ma5);
            }
            if (string.IsNullOrEmpty(Ma6) == false)
            {
                sb.Add(Ma6);
            }
            if (string.IsNullOrEmpty(Ma7) == false)
            {
                sb.Add(Ma7);
            }
            if (string.IsNullOrEmpty(Ma8) == false)
            {
                sb.Add(Ma8);
            }
            return sb;
        }
    }
}
