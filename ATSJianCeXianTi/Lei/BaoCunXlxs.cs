using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATSJianCeXianTi.Model;
using CommLei.DataChuLi;

namespace ATSJianCeXianTi.Lei
{
    internal class BaoCunXlxs
    {
        public void DaoChu(List<TestModel> lis, string erweima, bool ishege,string mingcheng)
        {
            string lujing = string.Format("{0}{1}", Directory.GetCurrentDirectory(), $@"\TestJieGuo\{DateTime.Now.ToString("yyyyMMdd")}");
            if (Directory.Exists(lujing) == false)
            {
                Directory.CreateDirectory(lujing);
            }
            string jieguo = ishege ? "PASS" : "Fail";
            lujing = $"{lujing}\\{mingcheng}_{erweima}_{DateTime.Now.ToString("yyyyMMddHHmmss")}_{jieguo}.xlsx";


            DuRuExclWenDan duRuExclWenDan = new DuRuExclWenDan();
            Dictionary<String, string> dic = new Dictionary<String, string>();

            TestModel model = new TestModel();

            duRuExclWenDan.DaoChuExc(lujing, GetDaoChuNeiRong(model.GetTestData(), lis));

        }

        private Dictionary<string, List<object>> GetDaoChuNeiRong(List<string> lies, List<TestModel> lisdata)
        {
            Dictionary<string, List<object>> BiaoGe = new Dictionary<string, List<object>>();
            List<TestModel> LieModels = lisdata;
            if (LieModels is IList)
            {


                foreach (var item in LieModels)
                {

                    for (int i = 0; i < lies.Count; i++)
                    {

                        object zhi = ConvertEnumerationItem(item, lies[i]);
                        if (BiaoGe.ContainsKey(lies[i]) == false)
                        {
                            BiaoGe.Add(lies[i], new List<object>());
                        }
                        BiaoGe[lies[i]].Add(zhi);

                    }

                }
            }
            return BiaoGe;
        }
        private object ConvertEnumerationItem(object item, string fieldName)
        {
            DataRow row = item as DataRow;
            if (row != null)
            {
                if (!string.IsNullOrEmpty(fieldName))
                {
                    if (row.Table.Columns.Contains(fieldName))
                        return row[fieldName];
                }
                return row[0];
            }
            else
            {
                PropertyDescriptor descriptor = TypeDescriptor.GetProperties(item).Find(fieldName, true);
                if (descriptor != null)
                    return (descriptor.GetValue(item) ?? null);
            }
            return null;
        }
    }
}
