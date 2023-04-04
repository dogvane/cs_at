using common;
using MysteelSpider.Entitys;
using Newtonsoft.Json;
using SpiderTools;
using SpiderTools.Download;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteelSpider
{
    /// <summary>
    /// 快讯的爬虫
    /// </summary>
    public class FastCommentSpider : Spider
    {

        Dictionary<int, FastcommentEntity> dayOfCache = new Dictionary<int, FastcommentEntity>();

        string dataBaseFolder = ConfigHelper.GetDataFolder("mysheel/fastcomment");

        /// <summary>
        /// 下一次刷新的时间
        /// </summary>
        DateTime nextRefreshTime = DateTime.Now;
        
        /// <summary>
        /// 下一次文件信息刷新缓存的时间
        /// </summary>
        DateTime nextFileFlushTime = DateTime.Now.AddHours(1);

        /// <summary>
        /// 获得今天的用于保存图片的地址
        /// 文件按照年做划分
        /// </summary>
        /// <returns></returns>
        string GetToDayFileName(DateTime time)
        {
            return Path.Combine(dataBaseFolder, time.Year.ToString(), time.ToString("MM_dd") + ".json");
        }

        /// <summary>
        /// 注意，这个爬虫正常情况下需要一直保持运行
        /// 只有偶尔需要重新爬取的时候，需要修改间隔参数
        /// </summary>
        public FastCommentSpider(int intervalMS = 5000)
        {
            DownloadScheduler = new DownloadScheduler(this, new DownloadSchedulerOptions
            {
                Concurrency = 1,
                SiteSingleAccessIntervalMS = intervalMS,
                Retry = 2,
            });

            DownloadScheduler.DefaultOnSuccess = OnSuccess;
            DownloadScheduler.Start();

            // 刷新时间是今天的 23:59:59.999
            nextRefreshTime = DateTime.Now.Date.AddDays(1).AddMilliseconds(-1);

            var todayFile = GetToDayFileName(nextRefreshTime);
            if (File.Exists(todayFile))
            {
                var json = File.ReadAllText(todayFile);
                var list = JsonConvert.DeserializeObject<List<FastcommentEntity>>(json);
                foreach (var item in list)
                {
                    dayOfCache.Add(item.id, item);
                }
            }
            else
            {
                new FileInfo(todayFile).Directory.Create();
            }
        }

        public void WaitDown()
        {
            while (true)
            {
                if(DownloadScheduler.WaitCount == 0)
                {
                    DownloadScheduler.Add("https://api.mysteel.com/newsflash/flashnews/query_newsflash.htm?pageNo=1&pageSize=20", "GET");
                }

                Thread.Sleep(1000);
            }
        }

        private void OnSuccess(RequestBase task, string result)
        {
            Console.WriteLine(DateTime.Now.ToString());
            // 使用 json 序列化代码，反序列化 result 成为 FastCommentResponse 对象
            var response = JsonConvert.DeserializeObject<FastCommentResponse>(result);

            // 合并历史数据并按照日期保存到文件里
            var lessItems = refreshItems(response.list);
            if (lessItems.Count > 0)
            {
                // 刷新文件数据到文件
                FlushCacheToFile();

                nextRefreshTime = nextRefreshTime.AddDays(1);
                dayOfCache.Clear();

                // 重新将剩下的数据加入缓存
                refreshItems(lessItems);
            }

            // 这里定时每隔1小时刷新一次数据到文件里
            if (DateTime.Now > nextFileFlushTime)
            {
                FlushCacheToFile();
                nextFileFlushTime = nextFileFlushTime.AddHours(1);
            }
        }

        public void FlushCacheToFile()
        {
            var todayFile = GetToDayFileName(nextRefreshTime);
            Console.WriteLine($"flush file :{todayFile}");
            File.WriteAllText(todayFile, JsonConvert.SerializeObject(dayOfCache.Values.ToArray()));
        }

        private List<FastcommentEntity> refreshItems(List<FastcommentEntity> list)
        {
            var retClone = list.ToList();

            foreach (var item in list)
            {
                DateTime datetime = (DateTime)item.publisherTime;
                if (datetime < nextRefreshTime)
                {
                    if (!dayOfCache.ContainsKey(item.id))
                    {
                        dayOfCache.Add(item.id, item);
                    }
                    retClone.Remove(item);
                }
            }

            return retClone;
        }
    }
}
