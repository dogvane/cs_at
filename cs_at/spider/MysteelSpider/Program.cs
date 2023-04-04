using MysteelSpider.Entitys;
using System;

namespace MysteelSpider
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            // Console.TreatControlCAsInput = true;

            var spider = new FastCommentSpider();
            
            // 处理 close 关闭按钮
            AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
            {
                Console.WriteLine("ProcessExit");
                spider.FlushCacheToFile();
            };          
            
            spider.WaitDown();
        }
    }
}