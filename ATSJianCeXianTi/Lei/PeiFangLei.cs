using ATSJianCeXianTi.Model;
using CommLei.DataChuLi;
using CommLei.JiChuLei;
using Common.DataChuLi;
using SSheBei.ABSSheBei;
using SSheBei.Model;
using SSheBei.ZongKongZhi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSJianCeXianTi.Lei
{
    /// <summary>
    /// 配方的类
    /// </summary>
    public class PeiFangLei
    {
        private JianCeDuiXiang jianCeDui;
        public JianCeDuiXiang JianCeDui { get { return jianCeDui; } }
        private string WenJianLuJing = "";
        public PeiFangLei()
        {
            jianCeDui = new JianCeDuiXiang(-1);
            WenJianLuJing = string.Format(@"{0}{1}", AppDomain.CurrentDomain.BaseDirectory, "ATSPeiFang");//项目
            if (!Directory.Exists(WenJianLuJing))
            {
                Directory.CreateDirectory(WenJianLuJing);

            }
        }


        /// <summary>
        /// 加载配方 
        /// </summary>
        /// <param name="wenjianname"></param>
        /// <returns></returns>
        public ZongTestModel JiaZaiPeiFang(string wenjianname)
        {
            return GetPeiFang(wenjianname);
        }

        /// <summary>
        /// 保存配方
        /// </summary>
        /// <param name="model"></param>
        /// <param name="wenjianname"></param>
        /// <returns></returns>
        public bool BaoCun(ZongTestModel model,string wenjianname)
        {
            string lujing = string.Format("{0}\\{1}.txt", WenJianLuJing, wenjianname);          
            JosnOrModel josnOrModel = new JosnOrModel(lujing);
            josnOrModel.XieTModel(model);
            return true;
        }

        /// <summary>
        /// 获取配方名称
        /// </summary>
        /// <returns></returns>
        public List<string> GetPeiFangNames()
        {
            List<string> mingceng = new List<string>();
            string[] txt = Directory.GetFiles(WenJianLuJing, "*.txt");
            for (int i = 0; i < txt.Length; i++)
            {
                string wenjianming = ChangYong.GetWenJianName(txt[i]);
                mingceng.Add(wenjianming);
            }
            return mingceng;
        }

        private ZongTestModel GetPeiFang(string wenjianname)
        {

          
            try
            {
                //目录下所有文件路径
                string[] txt = Directory.GetFiles(WenJianLuJing, "*.txt");
                foreach (string item in txt)
                {
                    ///保存文件名:id:name
                    string wenjianming = ChangYong.GetWenJianName(item);
                    if (wenjianming.Equals(wenjianname))
                    {
                        JosnOrModel josnOrModel = new JosnOrModel(item);
                        ZongTestModel model = josnOrModel.GetTModel<ZongTestModel>();
                        return model;
                    }
                   
                }

            }
            catch
            {

            }
            return null;
        }


    }
}
