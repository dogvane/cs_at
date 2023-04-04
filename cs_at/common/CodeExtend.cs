using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace common.Extend
{
    public static class CodeExtend
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

    }
}
