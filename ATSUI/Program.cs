using ATSJianMianJK.QuanXian;
using CommLei.DataChuLi;
using Common.HttpFanWen;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATSUI
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool createdNew = false;
            //判断互斥体
            Mutex mutex = new Mutex(false, string.Format("{0}Atsu", Application.ProductName), out createdNew);
            if (createdNew)
            {
                try
                {

                    if (HCDanGeDataLei<GengXinQi>.Ceratei().LisWuLiao.JingYong == 0)
                    {
                        string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\GenXinQi\ShangChuanYuGengXin.exe";
                        if (!string.IsNullOrEmpty(path))
                        {
                            if (File.Exists(path))
                            {
                                if (HCDanGeDataLei<GengXinQi>.Ceratei().LisWuLiao.GengXinBiaoZhi == 1)
                                {
                                    HCDanGeDataLei<GengXinQi>.Ceratei().LisWuLiao.GengXinBiaoZhi = 0;
                                    HCDanGeDataLei<GengXinQi>.Ceratei().BaoCun();
                                    Process.Start(path, "sss");
                                    System.Environment.Exit(0);
                                    return;
                                }
                            }

                        }
                        HCDanGeDataLei<GengXinQi>.Ceratei().LisWuLiao.GengXinBiaoZhi = 1;
                        HCDanGeDataLei<GengXinQi>.Ceratei().BaoCun();
                    }
                    else
                    {
                        HCDanGeDataLei<GengXinQi>.Ceratei().LisWuLiao.GengXinBiaoZhi = 1;
                        HCDanGeDataLei<GengXinQi>.Ceratei().BaoCun();
                    }

                }
                catch
                {
                    HCDanGeDataLei<GengXinQi>.Ceratei().LisWuLiao.GengXinBiaoZhi = 1;
                    HCDanGeDataLei<GengXinQi>.Ceratei().BaoCun();

                }
            }
          
            if (createdNew)
            {
               
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                int denglu=HCDanGeDataLei<GengXinQi>.Ceratei().LisWuLiao.IsXuYaoDengLu;
                if (denglu == 1)
                {
                    YongHuDengLuFrm fem = new YongHuDengLuFrm();
                    if (fem.ShowDialog() == DialogResult.OK)
                    {
                        Application.Run(new MianFrm(0));
                    }
                }
                else
                {
                    QuanXianLei.CerateDanLi().DengLu("admin", "admin123");
                    Application.Run(new MianFrm(1));
                }
            }
        }
    }
}
