using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommLei.DataChuLi;
using CommLei.JiChuLei;
using Common.DataChuLi;
using ZuZhuangUI.Model;

namespace ZuZhuangUI.Lei
{
    public class PeiFangChuLi
    {
        /// <summary>
        /// 站点的配置
        /// </summary>
        public List<SheBeiZhanModel> BTKLineModel;

        private string WenJianLuJing = "";

        private string QuanLuJing = "";

        public string XiuGaiDePeiFangName { get; set; } = "";

        public string WenJianLuJing1
        {
            get
            {
                return WenJianLuJing;
            }


        }

        public PeiFangChuLi()
        {
            BTKLineModel = new List<SheBeiZhanModel>();
            WenJianLuJing = HCDanGeDataLei<PeiFangXuanZeModel>.Ceratei().LisWuLiao.LuJin;
            QuanLuJing= string.Format(@"{0}{1}", AppDomain.CurrentDomain.BaseDirectory, "ATSPeiFang");//项目
            XiuGaiDePeiFangName = WenJianLuJing;
        }

    
        /// <summary>
        /// 设置当前配方
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool SetDanQianBaoPeiFang(string filename)
        {
            Dictionary<string,string> lujing = GetPeiFangNames();
            if (lujing.ContainsKey(filename))
            {
                HCDanGeDataLei<PeiFangXuanZeModel>.Ceratei().LisWuLiao.LuJin = filename;
                HCDanGeDataLei<PeiFangXuanZeModel>.Ceratei().BaoCun();
                WenJianLuJing = filename;
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// 获取配方名称与路径
        /// </summary>
        /// <returns></returns>
        public Dictionary<string,string> GetPeiFangNames()
        {
            string TexboxPath = QuanLuJing;
            if (!Directory.Exists(TexboxPath))
            {
                Directory.CreateDirectory(TexboxPath);

            }
            Dictionary<string, string> list = new Dictionary<string, string>();


            try
            {
                //目录下所有文件路径
                string[] txt = Directory.GetFiles(TexboxPath);
                foreach (string item in txt)
                {
                    list.Add(ChangYong.GetWenJianName(item), item);//文件名
                }

            }
            catch
            {

            }
            return list;
        }

        public void JiaZaiPeiFang(string filename)
        {
            Dictionary<string, string> wenjians = GetPeiFangNames();
            if (wenjians.ContainsKey(filename))
            {
                JosnOrModel josnOrModel = new JosnOrModel(wenjians[filename]);
                BTKLineModel = josnOrModel.GetLisTModel<SheBeiZhanModel>();
                if (BTKLineModel == null)
                {
                    BTKLineModel = new List<SheBeiZhanModel>();
                }
                XiuGaiDePeiFangName = filename;
            }
          
        }

        public bool BaoCun(string filename)
        {
            XiuGaiDePeiFangName = filename;
            Dictionary<string, string> wenjians = GetPeiFangNames();
            if (wenjians.ContainsKey(filename))
            {
                JosnOrModel josnOrModel = new JosnOrModel(wenjians[filename]);
                josnOrModel.XieTModel(BTKLineModel);
                return true;
            }
            else
            {
                string lujing = $"{QuanLuJing}\\{filename}.txt";
                JosnOrModel josnOrModel = new JosnOrModel(lujing);
                josnOrModel.XieTModel(BTKLineModel);
                return true;
            }
            
        }

    }

    public class PeiFangXuanZeModel
    {
        public string LuJin { get; set; } = "";
    }
}
