using System;
using System.Collections.Generic;
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
    }
}
