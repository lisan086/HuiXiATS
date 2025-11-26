using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Common.DataChuLi;
using Microsoft.CSharp;

namespace SuanShuSheBei.Frm.JiaoBenKuanJia
{

    /// <summary>
    /// 继承脚本
    /// </summary>
    public class XinJiChengJiaoBen
    {
        /// <summary>
        /// 命名空间与程序集的集合，版本号
        /// </summary>
        CanShuJiHeModel _CanShuJiHeModel;
        /// <summary>
        /// 脚本编译的对象
        /// </summary>
        CSharpCodeProvider objCSharpCodePrivoder;
        /// <summary>
        /// 缓存脚本初始类的基础内容
        /// </summary>
        private StringBuilder _HuanCunZhengTiShuChu = new StringBuilder();
        /// <summary>
        /// 用于替换类容
        /// </summary>
        private const string TiHuanString = "%%&*";
       
        private List<string> _CuoWuXinXi;

 

        /// <summary>
        /// 错误消息
        /// </summary>
        public List<string> CuoWuXinXi
        {
            get { return _CuoWuXinXi; }
        }
        /// <summary>
        /// 构造函数脚本初始类的基础内容
        /// </summary>
        public XinJiChengJiaoBen()
        {           
            _CanShuJiHeModel = new CanShuJiHeModel();
            _CanShuJiHeModel.YingYongJi.Add("Common.dll");
            _CanShuJiHeModel.UsingJi.Add("using CommLei.DataChuLi;");
            _CanShuJiHeModel.UsingJi.Add("using CommLei.GongYeJieHe;");
            _CanShuJiHeModel.UsingJi.Add("using CommLei.JiChuLei;");
            _CanShuJiHeModel.UsingJi.Add("using Common.DataChuLi;");
            objCSharpCodePrivoder = new CSharpCodeProvider(_CanShuJiHeModel.BanBenHao);
            _CuoWuXinXi = new List<string>();
            ZiJiModel();
        }
        /// <summary>
        /// 增加脚本
        /// </summary>
        public void ZiJiModel()
        {
            if (_CanShuJiHeModel != null)
            {
                if (_CanShuJiHeModel.UsingJi.Count > 0)
                {
                    _CanShuJiHeModel.UsingJi.ForEach((x) => { _HuanCunZhengTiShuChu.AppendLine(x); });
                    _HuanCunZhengTiShuChu.AppendLine("namespace DynamicCodeGenerate");
                    _HuanCunZhengTiShuChu.AppendLine("{");
                    _HuanCunZhengTiShuChu.AppendLine("public class HelloWorld:JiaoBenLei");
                    _HuanCunZhengTiShuChu.AppendLine("{");
                            
                    _HuanCunZhengTiShuChu.AppendLine(" public HelloWorld() ");
                    _HuanCunZhengTiShuChu.AppendLine("{");
                   
                    _HuanCunZhengTiShuChu.AppendLine("}");

                    _HuanCunZhengTiShuChu.AppendLine("public override object OutPut(string canshu)");
                    _HuanCunZhengTiShuChu.AppendLine("{");
                    _HuanCunZhengTiShuChu.AppendLine(TiHuanString);
                    _HuanCunZhengTiShuChu.AppendLine("}");


                

                    _HuanCunZhengTiShuChu.AppendLine("}");
                  
                    _HuanCunZhengTiShuChu.AppendLine("}");
                }

            }
        }

        /// <summary>
        /// 用于清理静态缓存
        /// </summary>
        public void Clear()
        {

            JingTaiHuanCunLei.JieJueHuanCun.Clear();
          
            JingTaiHuanCunLei.JieJueHuanCunFangFa.Clear();
            JingTaiHuanCunLei.JieJueJieKouCuoWuXinXi.Clear();
        }
        /// <summary>
        /// 用于把已建好的脚本编译下缓存起来
        /// </summary>
        /// <param name="jiekouname">接口名称</param>
        /// <param name="jubumodel">接口需要model</param>
        /// <param name="fanhuihanshu">接口脚本</param>
        public void SetDuiXiang(string jiekouname, string fanhuihanshu)
        {
            if (string.IsNullOrEmpty(fanhuihanshu) || !fanhuihanshu.Contains("return"))
            {
                return;
            }
            if (!JingTaiHuanCunLei.JieKouLei.Keys.Contains(jiekouname))
            {
                #region MyRegion
                CompilerParameters objCompilerParameters = new CompilerParameters();
                if (ZenJiaDuiXiang(objCompilerParameters))
                {
                    if (!string.IsNullOrEmpty(_HuanCunZhengTiShuChu.ToString()))
                    {
                        _CuoWuXinXi.Clear();
                        objCompilerParameters.GenerateExecutable = false;
                        objCompilerParameters.GenerateInMemory = true;
                        string shuju = TiHuanCanShu(fanhuihanshu);
                        CompilerResults cr = objCSharpCodePrivoder.CompileAssemblyFromSource(objCompilerParameters, shuju);

                        if (cr.Errors.HasErrors)
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine(string.Format("{0}:", jiekouname));
                            foreach (CompilerError err in cr.Errors)
                            {
                                sb.AppendLine(err.ErrorText);

                            }
                            JingTaiHuanCunLei.JieJueJieKouCuoWuXinXi.Add(jiekouname, sb);
                        }
                        else
                        {
                            #region MyRegion
                            //  通过反射，调用HelloWorld的实例
                            Assembly objAssembly = cr.CompiledAssembly;
                            Type[] leixing = objAssembly.GetExportedTypes();
                            if (leixing.Length > 0)
                            {
                                JiaoBenLei duixiang = (JiaoBenLei)Activator.CreateInstance(leixing[0]);
                             
                                JingTaiHuanCunLei.JieKouLei.Add(jiekouname, duixiang);
                               
                            }
                            #endregion
                        }
                    }

                }
                #endregion
            }

        }


    

        /// <summary>
        /// 解析脚本
        /// </summary>
        /// <param name="jiekouname">接口名称</param>
        /// <param name="jubumodel">接口model</param>
        /// <param name="fanhuihanshu">接口方式</param>
        /// <param name="value">接口model的值</param>
        /// <param name="IsShiBai">是否失败,失败表示false</param>
        /// <returns></returns>
        public object ShuChuYuJuJieGuo(string jiekouname, string canshu,out bool ischenggong)
        {
            ischenggong = false;
            if (JingTaiHuanCunLei.JieKouLei.ContainsKey(jiekouname))
            {             
                JiaoBenLei jiaoben = JingTaiHuanCunLei.JieKouLei[jiekouname];
                object jieguo = jiaoben.OutPut(canshu);
                if (jieguo==null)
                {
                    ischenggong = false;
                }
                else
                {
                    ischenggong = true;
                }
                return jieguo;
            }
            else
            {
                if (JingTaiHuanCunLei.JieJueJieKouCuoWuXinXi.ContainsKey(jiekouname))
                {
                    return JingTaiHuanCunLei.JieJueJieKouCuoWuXinXi[jiekouname].ToString();
                }
            }
            return string.Format("{0}", "接口不存在");
        }

      

        private bool ZenJiaDuiXiang(CompilerParameters objCompilerParameters)
        {
            if (_CanShuJiHeModel == null)
            {
                return false;
            }
            if (_CanShuJiHeModel.YingYongJi.Count > 0)
            {
                _CanShuJiHeModel.YingYongJi.ForEach((x) => { objCompilerParameters.ReferencedAssemblies.Add(x); });
                return true;
            }
            return false;
        }

        private string TiHuanCanShu(string fanhuihanshu)
        {
            string shuju = _HuanCunZhengTiShuChu.ToString().Replace(TiHuanString, fanhuihanshu);
          
            return shuju;
        }
    }

    /// <summary>
    /// 用于脚本引用的类
    /// </summary>
    [DataContract]
    public class CanShuJiHeModel
    {
        /// <summary>
        /// 引用程序集（有默认的程序集集合）
        /// </summary>
        [DataMember]
        public List<string> YingYongJi { get; set; }
        /// <summary>
        /// 导入的命名空间集合（有默认的命名空间的集合）
        /// </summary>
        [DataMember]
        public List<string> UsingJi { get; set; }
        /// <summary>
        /// 版本号默认的4.0
        /// </summary>
        [DataMember]
        public IDictionary<string, string> BanBenHao { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public CanShuJiHeModel()
        {
            YingYongJi = new List<string>();
            UsingJi = new List<string>();
            BanBenHao = new Dictionary<string, string> { { "CompilerVersion", "v4.0" } };
            ZiDongZengJia();
        }
        /// <summary>
        /// 清理程序集
        /// </summary>
        public void Clear()
        {
            YingYongJi.Clear();
            UsingJi.Clear();
            BanBenHao.Clear();
        }

        /// <summary>
        /// 增加默认的程序集和命名空间的集合
        /// </summary>
        public void ZiDongZengJia()
        {
            YingYongJi.Clear();
            UsingJi.Clear();
            YingYongJi.Add("System.dll");
            YingYongJi.Add("System.Xml.dll");
            YingYongJi.Add("System.Data.dll");
            YingYongJi.Add("System.Drawing.dll");
            YingYongJi.Add("System.Core.dll");
            YingYongJi.Add("System.xml.Linq.dll");
            YingYongJi.Add("System.Windows.Forms.dll");
            YingYongJi.Add("System.Deployment.dll");
            YingYongJi.Add("Microsoft.Csharp.dll");
            UsingJi.Add("using System;");
            UsingJi.Add("using System.Collections.Generic;");//using System.Collections.Generic;
            UsingJi.Add("using System.Collections;");//using System.Collections.Generic;
            UsingJi.Add("using System.ComponentModel;");
            UsingJi.Add("using System.Data;");
            UsingJi.Add("using System.Drawing;");
            UsingJi.Add("using System.Text;");
            UsingJi.Add("using System.Windows.Forms;");
            UsingJi.Add("using System.Reflection;");
            UsingJi.Add("using Microsoft.CSharp;");
            UsingJi.Add("using System.IO;");
            UsingJi.Add("using System.Xml;");
            UsingJi.Add("using System.Linq;");
        }
    }

   

    /// <summary>
    /// 缓存对象的
    /// </summary>
    public static class JingTaiHuanCunLei
    {
        /// <summary>
        /// 缓存的是脚本
        /// </summary>
        public static Dictionary<string, object> JieJueHuanCun = new Dictionary<string, object>();
  

        /// <summary>
        /// 缓存接口需要调用的方法
        /// </summary>
        public static Dictionary<string, MethodInfo> JieJueHuanCunFangFa = new Dictionary<string, MethodInfo>();
        /// <summary>
        /// 缓存错误信息
        /// </summary>
        public static Dictionary<string, StringBuilder> JieJueJieKouCuoWuXinXi = new Dictionary<string, StringBuilder>();

        /// <summary>
        /// 缓存接口需要调用的方法
        /// </summary>
        public static Dictionary<string, JiaoBenLei> JieKouLei = new Dictionary<string, JiaoBenLei>();


    }
}
