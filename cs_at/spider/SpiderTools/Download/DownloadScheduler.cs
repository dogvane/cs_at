using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SpiderTools.Download
{
    /// <summary>
    /// 下载的调度器
    /// </summary>
    /// <remarks>
    /// 默认基于一个爬虫（针对单一网站的爬虫）
    /// 只使用一个调度器
    /// 调度器的作用是负责数据的下载（包括cache的模拟下载）
    /// 下载队列的管理
    /// 下载限流等操作
    /// </remarks>
    public class DownloadScheduler
    {

        protected ISpider spider;

        /// <summary>
        /// 下载的任务队列
        /// </summary>
        protected readonly Queue<DownTask> downTaskQueue = new Queue<DownTask>();

        /// <summary>
        /// 下载调度器的一些设置
        /// </summary>
        DownloadSchedulerOptions options;

        /// <summary>
        /// 当前等待处理（下载的数量）
        /// </summary>
        public int WaitCount
        {
            get
            {
                return downTaskQueue.Count + (waitDown ? 1 : 0);
            }
        }

        /// <summary>
        /// 构造需要初始化一些自己相关的内容
        /// 也可以留到后面再设置
        /// 或者只使用默认参数
        /// </summary>
        public DownloadScheduler(ISpider spider, DownloadSchedulerOptions options = null)
        {
            this.spider = spider;
            if(options == null)
            {
                this.options = new DownloadSchedulerOptions();
            }
            else
            {
                // todo 注意，未来这里还需要对设置做一些校验才行
                this.options = options;
            }
        }

        /// <summary>
        /// 添加一个下载任务
        /// </summary>
        /// <param name="url">下载地址</param>
        /// <param name="onSuccess">成功后的处理</param>
        /// <param name="onFail">失败后的处理</param>
        public void Add(string url, Action<RequestBase, string> onSuccess, Action<RequestBase, string> onFail = null)
        {
            var task = new DownTask()
            {
                Request = new RequestBase(url),                
            };

            if (onSuccess != null)
                task.Propertys.Add("onSuccessFun", onSuccess);

            if (onFail != null)
                task.Propertys.Add("onFailsFun", onFail);

            downTaskQueue.Enqueue(task);
        }

        public void Start()
        {
            if(thread == null)
            {
                isDwonRun = true;
                thread = new Thread(DownThread);
                thread.Start();
            }
        }

        public void Stop()
        {
            if (thread == null)
                return;

            // 先通过控制变量来控制停止，然后等待一段时间（5s）

            isDwonRun = false;

            while(thread != null)
            {
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// 当前是否要处于下载中
        /// </summary>
        bool isDwonRun = false;

        Thread? thread = null;
        
        bool waitDown = false;

        /// <summary>
        /// 下载的处理线程
        /// </summary>
        void DownThread()
        {
            DateTime nextTime = DateTime.Now;
            DateTime startTime = DateTime.Now;

            while (isDwonRun)
            {
                if (nextTime < DateTime.Now && downTaskQueue.TryDequeue(out var task))
                {
                    try
                    {
                        waitDown = true;

                        startTime = DateTime.Now;
                        Console.WriteLine("begin get request: {0}", task.Request.Uri);

                        WebClient webClient = CreateWebClient(task);
                        byte[] retBytes;
                        if (task.Request.Method == "GET")
                        {
                            retBytes = webClient.DownloadData(task.Request.Uri);
                        }
                        else
                        {
                            retBytes = webClient.UploadData(task.Request.Uri, task.Encoding.GetBytes(task.Request.Body));
                        }

                        Console.WriteLine("end get request: {0} length:{1}", task.Request.Uri, retBytes.Length);

                        var successFun = task.GetProperty<Action<RequestBase, string>>("onSuccessFun");
                        if (successFun != null)
                        {
                            successFun(task.Request, task.Encoding.GetString(retBytes));
                        }

                        var successFunBytes = task.GetProperty<Action<RequestBase, byte[]>>("onSuccessFunBytes");
                        if (successFunBytes != null)
                        {
                            successFunBytes(task.Request, retBytes);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        var failFun = task.GetProperty<Action<RequestBase, string>>("onFailFun");
                        if (failFun != null)
                        {
                            failFun(task.Request, ex.Message);
                        }

                        task.ErrorCount++;
                        if(this.options.Retry < task.ErrorCount)
                        {
                            // 如果还需要重试，就压入队列
                            this.downTaskQueue.Enqueue(task);
                        }
                    }

                    nextTime = startTime.AddMilliseconds(options.SiteSingleAccessIntervalMS);
                }
                else
                {
                    waitDown = false;
                    // 如果是没有拿到下载任务，估计队列空了，等待一下再试
                    Thread.Sleep(100);
                }
            }

            thread = null;
        }


        Dictionary<string, string> defaultHeaders = new Dictionary<string, string>()
        {
            {"user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.127 Safari/537.36" },
            {"accept", "application/json,text/html,application/xhtml+xml,application/xml;q=0.9"},
            {"accept-language", "zh-CN,zh;q=0.9,zh-TW;q=0.8" },
            { "accept-encoding","utf8"}
        };

        /// <summary>
        /// 创建一个下载器
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        WebClient CreateWebClient(DownTask task)
        {
            var ret = new WebClient();

            var overHeader = task.Request.Header.Select(o => o.Key.ToLower()).ToHashSet();
            foreach(var item in defaultHeaders)
            {
                if (!overHeader.Contains(item.Key))
                {
                    ret.Headers.Add(item.Key, item.Value);
                }
            }

            foreach (var item in task.Request.Header)
            {
                ret.Headers.Add(item.Key, item.Value);
            }

            return ret;
        }
    }

    /// <summary>
    /// 下载调度器当前的状态
    /// </summary>
    public enum DownloadSchedulerStatus
    {
        /// <summary>
        /// 停止
        /// </summary>
        Stop = 0,

        /// <summary>
        /// 执行中
        /// </summary>
        Start = 1,
    }


    /// <summary>
    /// 下载调度器的默认参数
    /// </summary>

    public sealed class DownloadSchedulerOptions
    {
        /// <summary>
        /// 网站的访问间隔，单位ms
        /// 1000 表示网站间隔1000ms发起一个请求
        /// 或者说 1qps/s
        /// 如果不设置，这每次请求完成后就立即返回
        /// </summary>
        public int SiteSingleAccessIntervalMS { get; set; } = 0;

        /// <summary>
        /// 并发量
        /// 默认1
        /// 设置多并发，并且 SiteSingleAccessIntervalMS == 0 才会真的在当前下载器里进行多并发
        /// 否则，必须再有设置代理的情况下，使用不同的代理进行并发
        /// </summary>
        public int Concurrency { get; set; } = 1;

        /// <summary>
        /// 下载发生错误的时候，会需要重试几次
        /// 
        /// </summary>
        public int Retry { get; set; }
    }
}
