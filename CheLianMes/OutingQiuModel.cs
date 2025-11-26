using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheLianMes
{
    //
    // 摘要:
    //     http请求返回的内容
    //
    // 类型参数:
    //   T:
    public class OutingQiuModel<T> where T : new()
    {
        //
        // 摘要:
        //     接口返回是否成功,成功为true,失败为false
        public bool ChengGong { get; set; }

        //
        // 摘要:
        //     接口返回的josn数据，如果不是josn数据，就用空model
        public T ShuJu { get; set; }

        //
        // 摘要:
        //     一些由客户端失败的消息
        public string Msg { get; set; } = "";


        //
        // 摘要:
        //     请求返回的内容
        public OutingQiuModel()
        {
            ChengGong = false;
            ShuJu = new T();
        }
    }

    public class OutQianGongXuJieGuoModel
    {
        public string success { get; set; } = "";
        public string code { get; set; } = "";
        public string message { get; set; } = "";

    }

    public class OutGuoZhanJieGuoModel
    {
        public string success { get; set; } = "";
        public string code { get; set; } = "";
        public string message { get; set; } = "";

    }
}
