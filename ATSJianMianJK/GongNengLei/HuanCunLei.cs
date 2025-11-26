using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATSJianMianJK.XiTong.Model;
using CommLei.DataChuLi;
using CommLei.JiChuLei;

namespace ATSJianMianJK.GongNengLei
{
    public  class HuanCunLei
    {
        private List<int> Tds = new List<int>();
        private  Dictionary<int, List<HuanCunModel>> Lis = new Dictionary<int, List<HuanCunModel>>();
        #region 单利
        private readonly static object _DuiXiang = new object();

        private static HuanCunLei _LogTxt = null;



        private HuanCunLei()
        {
            ChuShiHua();
        }
        /// <summary>
        /// 单例类，必须KaiqiRiZhi设置为True才能写日志
        /// </summary>
        /// <returns>返回NewXieRiZhiLog</returns>
        public static HuanCunLei Cerate()
        {
            if (_LogTxt == null)
            {
                lock (_DuiXiang)
                {
                    if (_LogTxt == null)
                    {
                        _LogTxt = new HuanCunLei();
                    }
                }
            }
            return _LogTxt;
        }
        #endregion

        private void ChuShiHua()
        {
            for (int i = 0; i < 9; i++)
            {
                Tds.Add(i+1);
            }
            ShuaXinHuanCun();
        }
      

        public List<int> GetTDs()
        {
            return Tds;
        }
        public  void ShuaXinHuanCun()
        {
            Lis.Clear();
            List<HuanCunModel> lismodel = HCLisDataLei<HuanCunModel>.Ceratei().LisWuLiao;
            foreach (var item in Tds)
            {
                for (int i = 0; i < lismodel.Count; i++)
                {
                    int tdid = item;
                    lismodel[i].TDID = tdid;
                    lismodel[i].Clear();
                    if (Lis.ContainsKey(tdid) == false)
                    {
                        Lis.Add(tdid, new List<HuanCunModel>());
                    }
                    Lis[tdid].Add(lismodel[i].FuZhi());
                }
            }
          
         
            
        }

        public  void Clear(int tdid)
        {
            if (Lis.ContainsKey(tdid))
            {
                List<HuanCunModel> keys = Lis[tdid];
                foreach (var item in keys)
                {
                    item.Clear();
                }
            }
        }

        public  bool SetHuanCun(int tdid, string keyname, object zhi)
        {
            try
            {
                if (Lis.ContainsKey(tdid))
                {
                    List<HuanCunModel> keys = Lis[tdid];
                    foreach (var item in keys)
                    {
                        if (item.HuanCunName == keyname)
                        {
                            item.SetCanShu(zhi);
                            return true;
                        }
                    }

                }


            }
            catch
            {


            }
            return false;
        }

        public  bool SetHuanCunMa(int tdid, object zhi)
        {
            try
            {
                if (Lis.ContainsKey(tdid))
                {
                    List<HuanCunModel> keys = Lis[tdid];
                    foreach (var item in keys)
                    {
                        if (item.IsErWeiMa)
                        {
                            item.SetCanShu(zhi);
                            return true;
                        }
                    }

                }


            }
            catch
            {


            }
            return false;
        }

        public  object GetHuanCun(int tdid, string keyname,string shibaizhi)
        {
            try
            {
                if (Lis.ContainsKey(tdid))
                {
                    List<HuanCunModel> keys = Lis[tdid];
                    foreach (var item in keys)
                    {
                        if (item.HuanCunName == keyname)
                        {

                            return item.GetZhi();
                        }
                    }

                }


            }
            catch
            {


            }
            return shibaizhi;
        }

        public List<HuanCunModel> GetHunCunTD(int tdid)
        {
            if (Lis.ContainsKey(tdid))
            { 
                return Lis[tdid];
            }
            return new List<HuanCunModel>();
        }
    }
}
