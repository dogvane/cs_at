using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SpiderTools.Download
{
    /// <summary>
    /// 下载任务
    /// </summary>
    public class DownTask
    {
        //public string DownString(string url)
        //{

        //}

        /// <summary>
        /// 下载任务对应的Request 信息
        /// </summary>
        public RequestBase Request { get; set; }


        /// <summary>
        /// 提交和下载用来解析字符串的编码器
        /// 默认是utf8
        /// </summary>
        public Encoding Encoding { get; set; } = Encoding.UTF8;

        /// <summary>
        /// 当前的下载是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 当前请求的错误次数
        /// 下载失败时，运行一定的错误重试
        /// </summary>
        public int ErrorCount { get; set; }

        /// <summary>
        /// 任务的创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 当前任务的一些扩展数据
        /// </summary>
        public Dictionary<string, object> Propertys { get; private set; } = new Dictionary<string, object>();

        /// <summary>
        /// 获得指定的扩展数据，并转换为特定类型
        /// 但是，这里，如果类型不一致，也会返回null，要注意判断
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetProperty<T>(string key)
        {
            if(Propertys.TryGetValue(key, out object value))
            {
                if(value != null && value.GetType() == typeof(T))
                {
                    return (T)value;
                }
            }

            return default;
        }
    }
}
