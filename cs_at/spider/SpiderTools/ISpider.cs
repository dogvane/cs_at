using SpiderTools.Download;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderTools
{
    /// <summary>
    /// 一个爬虫实例对应的接口
    /// </summary>
    public interface ISpider
    {
        /// <summary>
        /// 下载调度器
        /// </summary>
        DownloadScheduler DownloadScheduler { get; protected set; }
    }
}
