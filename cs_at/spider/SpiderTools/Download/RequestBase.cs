using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderTools.Download
{
    /// <summary>
    /// 在做一些网络请求的时候
    /// 这里会放一些公共的请求参数
    /// 
    /// </summary>
    public class RequestBase
    {

        public RequestBase(string url)
        {
            this.Uri = url;
        }

        public RequestBase(string url, string methodType = "POST")
        {
            this.Uri = url;
            // 判断 methodType 是否只是 GET 或者 POST
            // 如果不是这两者，就会报错
            if (methodType == null || (methodType != "GET" && methodType != "POST"))
            {
                throw new ArgumentException($"methodType 只能是 GET 或者 POST");
            }

            this.Method = methodType;
        }

        /// <summary>
        /// 网络地址
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// 默认POST
        /// </summary>
        public string Method { get; set; } = "POST";

        /// <summary>
        /// 请求体，目前的页面请求，大多都是JSON的了
        /// </summary>
        public string Body { get; set; } = "";

        /// <summary>
        /// 自定义的Header
        /// </summary>
        public Dictionary<string, string> Header { get; set; } = new Dictionary<string, string>();
    }
}
