// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json.Linq;
using System.Linq;
using SinaSpider.Entitys.Sina;
using SpiderTools;
using SpiderTools.Download;
using System.Text.Json;
using System.Text;
using System.Text.RegularExpressions;
using SinaSpider.Utils;

internal class _SinaSpider : Spider
{
    public _SinaSpider()
    {
        DownloadScheduler = new DownloadScheduler(this, new DownloadSchedulerOptions
        {
            Concurrency = 2,
            SiteSingleAccessIntervalMS = 1000,
            Retry = 2,
        });

        if (File.Exists("trade.json"))
        {
            marketTradeCodes.AddRange(JsonSerializer.Deserialize<MarketTradeCode[]>(File.ReadAllText("trade.json", Encoding.UTF8)));
        }

        DownloadScheduler.Start();
    }

    List<MarketTradeCode> marketTradeCodes = new List<MarketTradeCode>();

    internal void WaitDown()
    {
        // 这里是要单次一轮的请求，所以会等待请求都结束
        while (DownloadScheduler.WaitCount > 0)
        {
            Thread.Sleep(100);
        }

        Console.WriteLine("wait finish.");
    }

    void SaveCode()
    {
        if (marketTradeCodes.Count > 0)
        {
            var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(marketTradeCodes, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText("trade.json", jsonStr, Encoding.UTF8);
        }
    }

    public void Stop()
    {
        DownloadScheduler.Stop();
    }

    public void 开始获取最新交易代码()
    {
        更新获得新浪当前记录的交易代码();
    }


    void 更新获得新浪当前记录的交易代码()
    {
        // https://vip.stock.finance.sina.com.cn/quotes_service/api/json_v2.php/Market_Center.getHQNodes

        var url = "https://vip.stock.finance.sina.com.cn/quotes_service/api/json_v2.php/Market_Center.getHQNodes";


        DownloadScheduler.Add(url, (request, response) =>
        {
            var ret = JsonDocument.Parse(response);
            var root = ret.RootElement;
            var x = root[1][0];

            Queue<KeyValuePair<List<String>, JsonElement>> source = new Queue<KeyValuePair<List<string>, JsonElement>>();
            source.Enqueue(new KeyValuePair<List<string>, JsonElement>(new List<string>() { root[0].ToString() }, root[1]));
            List<dynamic> rets = new List<dynamic>();
            // 本来是用递归的，但是匿名函数不好写递归，懒得再开新函数，所以这里用队列的方式把递归函数展开
            while (source.Count > 0)
            {
                (List<string> parent, JsonElement element) = source.Dequeue();

                if (element[0].ValueKind == JsonValueKind.Array)
                {
                    foreach (var sub in element.EnumerateArray())
                    {
                        source.Enqueue(new KeyValuePair<List<string>, JsonElement>(parent, sub));
                    }
                }
                else
                {
                    var name = element[0].ToString();

                    if (element[1].ValueKind == JsonValueKind.Array)
                    {
                        // 是数组，还得往下找
                        var p = new List<string>();
                        p.AddRange(parent);
                        p.Add(name);
                        foreach (var sub in element[1].EnumerateArray())
                        {
                            source.Enqueue(new KeyValuePair<List<string>, JsonElement>(p, sub));
                        }
                    }
                    else
                    {
                        // 没有数组可以分解了
                        var code = element[2].ToString();
                        rets.Add(new
                        {
                            name,
                            route = String.Join("-", parent),
                            code,
                        });
                    }
                }
            }

            var 上海期货交易所 = rets.Where(o => o.route == "行情中心-期货-上海期货交易所").ToList();
            var 大连商品交易所 = rets.Where(o => o.route == "行情中心-期货-大连商品交易所").ToList();
            var 郑州商品交易所 = rets.Where(o => o.route == "行情中心-期货-郑州商品交易所").ToList();
            var 中国金融期货交易所 = rets.Where(o => o.route == "行情中心-期货-中国金融期货交易所").ToList();

            上海期货交易所.ForEach(o => 获得当前在用的代码(o.code));
            大连商品交易所.ForEach(o => 获得当前在用的代码(o.code));
            郑州商品交易所.ForEach(o => 获得当前在用的代码(o.code));
            中国金融期货交易所.ForEach(o => 获得当前在用的代码(o.code));
        });
    }
    
    void 获得当前在用的代码(string code)
    {
        // https://vip.stock.finance.sina.com.cn/quotes_service/api/json_v2.php/Market_Center.getHQFuturesData?node=zj_qh

        var baseUrl = "https://vip.stock.finance.sina.com.cn/quotes_service/api/json_v2.php/Market_Center.getHQFuturesData?node={0}";
        var url = string.Format(baseUrl, code);

        if ("trade.json".CheckWriteToDay())
        {
            // 交易代码文件是今天更新的，则不再重新获取了
            return;
        }

        DownloadScheduler.Add(url, (request, resopnse) => {
            var tradeCodes = JsonSerializer.Deserialize<MarketTradeCode[]>(resopnse);
            // todo 这里和数据库比较，如果发现有新代码，则加入数据库里

            foreach(var item in tradeCodes)
            {
                if(!marketTradeCodes.Any(o=>o.symbol == item.symbol))
                {
                    Console.WriteLine("new tradeCode {0}", item.symbol);
                    marketTradeCodes.Add(item);
                }
            }
        });
    }

    public void tset()
    {
        更新日K数据("RB2210");
    }

    public void 开始更新日K数据()
    {
        foreach(var item in marketTradeCodes)
        {
            更新日K数据(item.symbol);
        }
    }

    void 更新日K数据(string code)
    {
        // https://stock2.finance.sina.com.cn/futures/api/jsonp.php//InnerFuturesNewService.getDailyKLine?symbol=SP2209

        var baseUrl = "https://stock2.finance.sina.com.cn/futures/api/jsonp.php//InnerFuturesNewService.getDailyKLine?symbol={0}";
        var url = string.Format(baseUrl, code);


        var dayFile = string.Format("sina/{0}/{1}/dailybar.json", code.GetCodeStart(), code);
        if (dayFile.CheckWriteToDay())
        {
            // 交易代码文件是今天更新的，则不再重新获取了
            return;
        }

        var fileInfo = new FileInfo(dayFile);
        Directory.CreateDirectory(fileInfo.DirectoryName);


        DownloadScheduler.Add(url, (request, responseBody) =>
        {
            // 返回不是标准的json数据，需要做一下预处理
            var start = responseBody.IndexOf("(") + 1;
            var end = responseBody.LastIndexOf(")");
            var json = responseBody.Substring(start, end - start);
            var datas = JsonSerializer.Deserialize<KBar[]>(json);

            List<KBar> bars = new List<KBar>();

            if (File.Exists(dayFile))
            {
                bars.AddRange(JsonSerializer.Deserialize<KBar[]>(File.ReadAllText(dayFile)));
            }

            // 合并已知的和新爬取到的，按照日志来
            var newBars = bars.UnionBy(datas, o => o.d).ToList();
            if (newBars.Count != bars.Count)
            {
                newBars.Sort((o1, o2) => DateTime.Parse(o1.d) > DateTime.Parse(o2.d) ? 1 : -1);
                File.WriteAllText(dayFile, JsonSerializer.Serialize(newBars));
            }
        });

    }

    public void 开始更新分钟K线数据()
    {
        foreach (var item in marketTradeCodes)
        {
            更新1分钟K线数据(item.symbol);
        }
    }


    private void 更新1分钟K线数据(string code)
    {

        // 一分钟的k线图下载的数据地址
        // https://stock2.finance.sina.com.cn/futures/api/jsonp.php//InnerFuturesNewService.getFewMinLine?symbol=RB2210&type=1

        var baseUrl = "https://stock2.finance.sina.com.cn/futures/api/jsonp.php//InnerFuturesNewService.getFewMinLine?symbol={0}&type=1";
        string url = string.Format(baseUrl, code);

        var jsonFileName = string.Format("sina/{0}/{1}/min1Bar.json", code.GetCodeStart(), code);
        var fileInfo = new FileInfo(jsonFileName);
        Directory.CreateDirectory(fileInfo.DirectoryName);

        DownloadScheduler.Add(url, (request, responseBody) =>
        {
            // 返回不是标准的json数据，需要做一下预处理
            var start = responseBody.IndexOf("(") + 1;
            var end = responseBody.LastIndexOf(")");
            var json = responseBody.Substring(start, end - start);
            var datas = JsonSerializer.Deserialize<KBar[]>(json);

            List<KBar> bars = new List<KBar>();

            if (File.Exists(jsonFileName))
            {
                bars.AddRange(JsonSerializer.Deserialize<KBar[]>(File.ReadAllText(jsonFileName)));
            }

            // 合并已知的和新爬取到的，按照日志来
            var newBars = bars.UnionBy(datas, o => o.d).ToList();
            if (newBars.Count != bars.Count)
            {
                newBars.Sort((o1, o2) => DateTime.Parse(o1.d) > DateTime.Parse(o2.d) ? 1 : -1);
                File.WriteAllText(jsonFileName, JsonSerializer.Serialize(newBars));
            }
        });
    }

    public void 开始更新分时图()
    {
        foreach (var item in marketTradeCodes)
        {
            更新分时图(item.symbol);
        }
    }


    private void 更新分时图(string code)
    {
        // 分时图时刻都会更新，要注意，每天的实际数据要在收盘后更新

        // https://stock2.finance.sina.com.cn/futures/api/jsonp.php//InnerFuturesNewService.getMinLine?symbol=rb2210

        var baseUrl = "https://stock2.finance.sina.com.cn/futures/api/jsonp.php//InnerFuturesNewService.getMinLine?symbol={0}";
        string url = string.Format(baseUrl, code);


        DownloadScheduler.Add(url, (request, responseBody) =>
        {
            // 返回不是标准的json数据，需要做一下预处理
            var start = responseBody.IndexOf("(") + 1;
            var end = responseBody.LastIndexOf(")");
            var json = responseBody.Substring(start, end - start);
            if(json == "null")
            {
                // 如果当前品种没交易，或者在每天的开盘前
                // 分时图的数据实际上是空的
                return;
            }

            var arrs = JArray.Parse(json);
            JArray first = (JArray)arrs[0];
            string date = first[6].ToString();

            var jsonFileName = string.Format("sina/{0}/{1}/ts/{2}.csv", code.GetCodeStart(), code, date);
            var fileInfo = new FileInfo(jsonFileName);
            Directory.CreateDirectory(fileInfo.DirectoryName);
            List<TimePrice> prices = new List<TimePrice>(arrs.Count);

            foreach (JArray item in arrs)
            {
                var p = new TimePrice
                {
                    Date = item[0].ToString(),
                    Price = double.Parse(item[1].ToString()),
                    AveragePrice = double.Parse(item[2].ToString()),
                    Volume = int.Parse(item[3].ToString()),
                    Interest = int.Parse(item[4].ToString()),
                };

                prices.Add(p);
            }

            prices.WriteToCSVFile(jsonFileName);
        });
    }
}