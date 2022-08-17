using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SinaSpider.Utils
{
    public static class Extends
    {
        /// <summary>
        /// 获得交易代码的前面的字母
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetCodeStart(this string code)
        {
            var reg = new Regex(@"\d+");
            var replaceCode = reg.Match(code).Value;
            if (string.IsNullOrEmpty(replaceCode))
                return code;

            return code.Replace(replaceCode, "");
        }

        /// <summary>
        /// 检查当前文件是否在今天被写入过
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>
        /// true 表示有写入操作
        /// false 表示无写入操作
        /// </returns>
        public static bool CheckWriteToDay(this string fileName)
        {
            if (!File.Exists(fileName))
                return false;

            if (File.GetLastWriteTime(fileName).ToString("yyyyMMdd") == DateTime.Now.ToString("yyyyMMdd"))
                return true;

            return false;
        }

        /// <summary>
        /// 将数据写入csv文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="datas">数据</param>
        /// <param name="fileName">csv的文件名</param>
        public static void WriteToCSVFile<T>(this List<T> datas, string fileName)
        {
            using (var writer = new StreamWriter(fileName))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(datas);
            }
        }

        /// <summary>
        /// 从csv文件里读取一组数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName">csv文件名</param>
        /// <returns></returns>
        public static List<T> ReadByCSVFile<T>(this string fileName)
        {
            if (!File.Exists(fileName))
            {
                return new List<T>();
            }

            using (var reader = new StreamReader(fileName))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<T>();
                return records.ToList();
            }
        }
    }
}
