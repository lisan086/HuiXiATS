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
        }

        //获取文件路径,根据bool isfile给的值，false则添加
        public List<string> GetPeiFang(bool isfile)
        {
            string TexboxPath = string.Format(@"{0}{1}", AppDomain.CurrentDomain.BaseDirectory, "PeiFang");//项目
            if (!Directory.Exists(TexboxPath))
            {
                Directory.CreateDirectory(TexboxPath);

            }
            List<string> list = new List<string>();


            try
            {
                //目录下所有文件路径
                string[] txt = Directory.GetFiles(TexboxPath);
                foreach (string item in txt)
                {
                    if (isfile)
                    {
                        list.Add(ChangYong.GetWenJianName(item));//文件名
                    }
                    else
                    {
                        list.Add(item);//路径
                    }
                }

            }
            catch
            {

            }
            return list;
        }

        public bool SetDanQianBaoPeiFang(string filename)
        {
            List<string> lujing = GetPeiFang(false);
            for (int i = 0; i < lujing.Count; i++)
            {
                string wuhouzui = ChangYong.GetWenJianName(lujing[i]);//无后缀
                if (wuhouzui.Equals(filename))
                {
                    HCDanGeDataLei<PeiFangXuanZeModel>.Ceratei().LisWuLiao.LuJin = lujing[i];
                    HCDanGeDataLei<PeiFangXuanZeModel>.Ceratei().BaoCun();
                    return true;
                }
            }
            return false;
        }

        public void JiaZaiPeiFang(string wenjianpath)
        {
            JosnOrModel josnOrModel = new JosnOrModel(wenjianpath);
            BTKLineModel = josnOrModel.GetLisTModel<SheBeiZhanModel>();
            if (BTKLineModel == null)
            {
                BTKLineModel = new List<SheBeiZhanModel>();
            }
            WenJianLuJing = wenjianpath;
        }

        public bool BaoCun(string wenjianpath)
        {
            if (string.IsNullOrEmpty(wenjianpath))
            {
                if (string.IsNullOrEmpty(WenJianLuJing))
                {
                    return false;
                }
                JosnOrModel josnOrModel = new JosnOrModel(WenJianLuJing);
                josnOrModel.XieTModel(BTKLineModel);
                return true;
            }
            else
            {
                JosnOrModel josnOrModel = new JosnOrModel(wenjianpath);
                josnOrModel.XieTModel(BTKLineModel);
                return true;
            }
        }

        public void GengHuanDanQianPeiFang()
        {
            HCDanGeDataLei<PeiFangXuanZeModel>.Ceratei().LisWuLiao.LuJin = WenJianLuJing;
            HCDanGeDataLei<PeiFangXuanZeModel>.Ceratei().BaoCun();
        }
    }

    public class PeiFangXuanZeModel
    {
        public string LuJin { get; set; } = "";
    }
}
