using ATSJianCeXianTi.GongNengLei;
using ATSJianCeXianTi.Model;
using ATSJianMianJK.XiTong.Model;
using CommLei.DataChuLi;
using CommLei.JiChuLei;
using Common.DataChuLi;
using SSheBei.ABSSheBei;
using SSheBei.Model;
using SSheBei.PeiZhi;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSJianCeXianTi.Lei
{
    /// <summary>
    /// 用于每个项目的检测
    /// </summary>
    public class JianCeDuiXiang
    {
        public event Action<TangChuanUIModel, int,int> ChuLiUIEvent;
        private XuQiTaLei XuQiTaLei = new XuQiTaLei();
        public int TDID { get; set; } = -1;
        /// <summary>
        /// 功能执行对象
        /// </summary>
        private Dictionary<string, ABSGongNengLei> ZhiXing = new Dictionary<string, ABSGongNengLei>();
        public JianCeDuiXiang(int tdid)
        {
            TDID= tdid;
            {
                ABSGongNengLei xiangguo = new CaoZuoSheBeiLei();
                xiangguo.TDID = TDID;
                ZhiXing.Add(xiangguo.GetTestBiaoZhi(), xiangguo);
            }
            {
                ABSGongNengLei xiangguo = new YanShiLei();
                xiangguo.TDID = TDID;
                ZhiXing.Add(xiangguo.GetTestBiaoZhi(), xiangguo);
            }
            {
                DengDaiTuiChuLei xiangguo = new DengDaiTuiChuLei();
                xiangguo.TDID = TDID;
                ZhiXing.Add(xiangguo.GetTestBiaoZhi(), xiangguo);
            }
            {
                TangChuanLei xiangguo = new TangChuanLei();
                xiangguo.TDID = TDID;
                xiangguo.ChuLiUIEvent += ChuFaUI;
                ZhiXing.Add(xiangguo.GetTestBiaoZhi(), xiangguo);
            }
            {
                string mulu = string.Format("{0}{1}", Directory.GetCurrentDirectory(), @"\GongNengQu");
                if (Directory.Exists(mulu)==false)
                {
                    Directory.CreateDirectory(mulu);
                }
                JieKouJiaZaiLei<ABSGongNengLei> wenss = new JieKouJiaZaiLei<ABSGongNengLei>();
                List<Type> shebeistypes = wenss.JiaZaiLisType(mulu);
                foreach (var item in shebeistypes)
                {
                    try
                    {
                        ABSGongNengLei tongjijiekou = (ABSGongNengLei)Activator.CreateInstance(item);                    
                        tongjijiekou.TDID = TDID;
                        ZhiXing.Add(tongjijiekou.GetTestBiaoZhi(), tongjijiekou);
                    }
                    catch
                    {

                     
                    }
                  
                }
            }
        }
        /// <summary>
        /// 检测开始
        /// </summary>
        public void KaiShi()
        {
           
        }
        /// <summary>
        /// 程序退出
        /// </summary>
        public void Close()
        {
            ZhiXingTiaoChu(1);
        }


        public ZhiJieGuo JianCe(TestModel model)
        {
            ZhiJieGuo zhiJieGuo = new ZhiJieGuo();
            DateTime shijian = DateTime.Now;
            if (ZhiXing.ContainsKey(model.GongNengType))
            {
                XuQiTaLei.FanHuiCanShu(model,TDID);
                zhiJieGuo = ZhiXing[model.GongNengType].ZhiXingJieGuo(model);
                if (zhiJieGuo.IsHeGe)
                {
                    XuQiTaLei.FuZhiCanShu(model, TDID,zhiJieGuo.RecZhi);
                }
                zhiJieGuo.TestTime = (DateTime.Now - shijian).TotalSeconds.ToString("0.000");

            }
            else
            {
                zhiJieGuo.IsHeGe = false;
                zhiJieGuo.RecZhi = "该功能未实现";
                zhiJieGuo.TestTime = (DateTime.Now - shijian).TotalSeconds.ToString("0.000");
                zhiJieGuo.IsString = 1;
            }
            return zhiJieGuo;
        }

      

        /// <summary>
        /// 执行退出 1表示流程退出 2表示窗体关闭
        /// </summary>
        /// <param name="biaozhi"></param>
        public void ZhiXingTiaoChu(int biaozhi)
        {
            foreach (var item in ZhiXing.Keys)
            {
                ZhiXing[item].TiaoChu(biaozhi);
            }
        }

        /// <summary>
        /// 获取功能块
        /// </summary>
        /// <returns></returns>
        public List<string> GetGongNeng()
        {
            return ZhiXing.Keys.ToList();
        }
        /// <summary>
        /// 获取每个功能块的判断方式
        /// </summary>
        /// <param name="gongneng"></param>
        /// <returns></returns>
        public List<string> GetPanDuanFangShi(string gongneng)
        {
            if (ZhiXing.ContainsKey(gongneng))
            {
                return ZhiXing[gongneng].GetPanDuanFangShi();
            }
            return new List<string>() { "不判断"};
        }

        /// <summary>
        /// 获取每个功能块的判断方式
        /// </summary>
        /// <param name="gongneng"></param>
        /// <returns></returns>
        public List<string> GetLeiXing(string gongneng)
        {
            if (ZhiXing.ContainsKey(gongneng))
            {
                return ZhiXing[gongneng].GetLeiXing();
            }
            return new List<string>() { "一般" };
        }
        /// <summary>
        /// 获取每个功能块对应的设备
        /// </summary>
        /// <param name="gongNeng"></param>
        /// <returns></returns>
        public List<string> GetSheBei(string gongNeng)
        {
            if (ZhiXing.ContainsKey(gongNeng))
            {
               return  ZhiXing[gongNeng].GetSheBei();
            }
            return new List<string>() { "-1:无设备" };
        }

        /// <summary>
        /// 获取每个功能块对应的设备
        /// </summary>
        /// <param name="gongNeng"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<JiCunQiModel> GetCMDSend(string gongNeng,int id,bool isdu)
        {
            if (ZhiXing.ContainsKey(gongNeng))
            {
               return  ZhiXing[gongNeng].GetCMDSend(id, isdu);
            }
            return new List<JiCunQiModel>();
        }

        /// <summary>
        /// 获取对应的接口控件
        /// </summary>
        /// <param name="gongNeng"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public KJPeiZhiJK GetJKKJ(string gongNeng, int shebeiid, string weiyibiaoshi)
        {
            if (ZhiXing.ContainsKey(gongNeng))
            {
                return ZhiXing[gongNeng].GetKJ(shebeiid,weiyibiaoshi);
            }
            return new MoRenKJ();
        }


        public List<string> GetHuanCunName()
        {
            List<HuanCunModel> huancun = HCLisDataLei<HuanCunModel>.Ceratei().LisWuLiao;

            List<string> lis = new List<string>();
            for (int i = 0; i < huancun.Count; i++)
            {
                lis.Add(huancun[i].HuanCunName);
            }
            return lis;
        }

        public List<XuQiuModel> GetXuQiuModel()
        {
            return XuQiTaLei.GetFangFaMing();
        }

        public  void UIFanHuiJieGuo(ZhiJieGuo model)
        {
            foreach (var item in ZhiXing.Keys)
            {
                if (ZhiXing[item] is TangChuanLei)
                {
                    ZhiXing[item].UIFanHuiJieGuo(model.FuZhi());
                }
            }
        }

        private void ChuFaUI(TangChuanUIModel lis, int type)
        {
            if (ChuLiUIEvent != null)
            {
                ChuLiUIEvent(lis, type,TDID);
            }
        }
    }
}
