using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace common
{
    /// <summary>
    /// 交易日历工具类
    /// </summary>
    public class TradeDateHelper
    {
        static TradeDateHelper()
        {
            LoadDataFromFile();
        }

        static void LoadDataFromFile()
        {
            var fileName = "tradedates.txt";

            if(!File.Exists(fileName))
            {
                fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                if (!File.Exists(fileName))
                {
                    throw new Exception("not find tradedate.txt");
                }
            }

            var lines = File.ReadAllLines(fileName);
            tradeDateSet = lines.ToHashSet();
        }

        static HashSet<string> tradeDateSet = new HashSet<string>();

        /// <summary>
        /// 只通过日期来判断是否是交易日
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsTradeDate(string date)
        {
            if(date.Length == 8)
                return tradeDateSet.Contains(date);

            date = date.Replace("-", "").Replace(" ", "").Replace("_", "");
            if(date.Length == 8)
                return tradeDateSet.Contains(date);

            return false;
        }

        /// <summary>
        /// 获得当前的交易日
        /// 格式： yyyyMMdd
        /// 
        /// 理论上
        /// 当前日时交易日，并且时间在 15:00 之前，则返回当前交易日数据
        /// 当前日时交易日，时间在 15:00 之后，则返回下一个交易日，可能时明天，也可能是下周一
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentTradeDate()
        {
            return GetCurrentTradeDate(DateTime.Now, 10);
        }

        /// <summary>
        /// 获得当前的交易日
        /// 格式： yyyyMMdd
        /// 
        /// 理论上
        /// 当前日时交易日，并且时间在 15:00 之前，则返回当前交易日数据
        /// 当前日时交易日，时间在 15:00 之后，则返回下一个交易日，可能时明天，也可能是下周一
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentTradeDate(DateTime dateTime)
        {
            return GetCurrentTradeDate(dateTime, 10);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="tryDateNum">理论上，国内放假，不太可能连续放10天的，这个是防止出错的方案</param>
        /// <returns></returns>
        private static string GetCurrentTradeDate(DateTime dateTime, int tryDateNum = 0)
        {
            if(dateTime.Hour < 15)
            {
                string date = dateTime.ToString("yyyyMMdd");
                if (tradeDateSet.Contains(date))
                {
                    return date;
                }
            }

            if(tryDateNum <= 0)
            {
                return "";
            }

            return GetCurrentTradeDate(dateTime.Date.AddDays(1), tryDateNum - 1);
        }
    }
}
