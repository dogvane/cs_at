using SpiderTools.Download;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderTools
{
    public class Spider : ISpider
    {
        private DownloadScheduler _downloadScheduler;

        public Spider()
        {
        }

        /// <summary>
        /// 默认的下载器
        /// </summary>

        public DownloadScheduler DownloadScheduler
        {
            get
            {
                if (_downloadScheduler == null)
                {
                    _downloadScheduler = new DownloadScheduler(this, new DownloadSchedulerOptions());
                }

                return _downloadScheduler;
            }
            set { _downloadScheduler = value; }
        }

    }
}
