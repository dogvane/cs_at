// See https://aka.ms/new-console-template for more information
using System.Text;

Console.WriteLine("对新浪的数据爬取");
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);


var spider = new _SinaSpider();

spider.开始获取最新交易代码();
spider.WaitDown();
spider.开始更新日K数据();
spider.WaitDown();
spider.开始更新分钟K线数据();
spider.WaitDown();
spider.开始更新分时图();
spider.WaitDown();

spider.Stop();