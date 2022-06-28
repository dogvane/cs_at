using DataLib.Entity;
using DataLib.Repostiory;
using DownByWind.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLib.Service
{
    /// <summary>
    /// K线相关的业务代码
    /// </summary>
    public class BarService
    {
        BarRepository barRepository = new BarRepository();

        CodeInfoRepository codeInfoRepository = new CodeInfoRepository();

        /// <summary>
        /// 获得某个代码的 K 线数据
        /// </summary>
        /// <param name="codeId"></param>
        /// <param name="type"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public List<Bar2> GetBars(string codeId, DateTime startTime, DateTime endTime,  BarType type)
        {
            List<Bar2> rets = barRepository.GetBar2(codeId, startTime, endTime, type);
            if(rets.Count == 0)
            {
                List<Bar> sourceDatas = new List<Bar>();

                if (type != BarType.Day)
                {
                    // 如果不是日线，则只获取分钟线
                    sourceDatas = barRepository.GetBar(codeId, startTime, endTime, BarType.M1);
                    if(type != BarType.M1)
                    {
                        // 非分钟数据，则需要根据当前的1分钟K线数据，来构建一个特定分钟的K线数据
                    }
                }
                else
                {
                    sourceDatas = barRepository.GetBar(codeId, startTime, endTime, BarType.Day);
                }

                if (sourceDatas.Count == 0)
                {
                    Console.WriteLine($"无效的K线代码 {codeId}");
                    return rets;
                }

                rets = BuildBar2(sourceDatas, type);
            }

            return rets;
        }

        /// <summary>
        /// 获得某个代码的K线数据
        /// </summary>
        /// <param name="codeId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<Bar2> GetBars(string codeId, BarType type = BarType.Day)
        {
            CodeInfo code = codeInfoRepository.GetByCodeId(codeId);
            
            if (code != null)
            {
                return GetBars(code.WindCode, code.IssueDate, code.LastTradeDate, type);
            }

            return new List<Bar2>();
        }

        /// <summary>
        /// 根据原始数据构建新的K线数据
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        List<Bar2> BuildBar2(List<Bar> source, BarType type)
        {
            if (source == null || source.Count == 0)
                return new List<Bar2>();

            Console.WriteLine($"build {type} {source.Count}");
            List<Bar2> ret = new List<Bar2>();

            if(source.First().BarType == type)
            {
                // 原样输出，只需要计算MA即可
                foreach(var item in source)
                {
                   var bar2 = JsonConvert.DeserializeObject<Bar2>(JsonConvert.SerializeObject(item));
                    ret.Add(bar2);
                }

                // 直接计算MA,理论上source过来的数据，都是排序好了的。
                // 这里使用正序的方式来进行均线的处理
                foreach (var maType in new[] { 5, 10, 20, 40, 60, 120, 240 })
                {
                    var ctype = (MAType)maType;

                    for (var startIndex = maType; startIndex < ret.Count; startIndex++)
                    {
                        var value = ret.Skip(startIndex - maType).Take(maType).Average(o => o.C) ?? 0;
                        ret[startIndex].MA[ctype] = value;
                    }
                }
            }

            return ret;
        }
    }
}
