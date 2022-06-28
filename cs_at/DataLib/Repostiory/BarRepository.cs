using DataLib.Entity;
using DownByWind.DbSet;
using DownByWind.Entity;
using ServiceStack.OrmLite;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLib.Repostiory
{

    /// <summary>
    /// K线的数据获取方式
    /// </summary>
    public class BarRepository
    {
        /// <summary>
        /// 通用的k线数据获取接口
        /// </summary>
        /// <param name="windCode"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<Bar> GetBar(string windCode, DateTime startTime, DateTime endTime, BarType type)
        {
            Console.WriteLine($"get bars {windCode} {startTime} {endTime} {(int)type}");
            using (var db = QuantDB.GetCon())
            {
                return db.Select<Bar>(o => 
                o.WindCode == windCode
                && (int)o.BarType == (int)type
                && o.D >= startTime && o.D <= endTime).OrderBy(o=>o.D).ToList();
            }
        }

        /// <summary>
        /// 通用的k线数据获取接口
        /// </summary>
        /// <param name="windCode"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<Bar2> GetBar2(string windCode, DateTime startTime, DateTime endTime, BarType type)
        {
            
            using (var db = QuantDB.GetCon())
            {
                return db.Select<Bar2>(o =>
                o.WindCode == windCode
                && o.BarType == type
                && o.D > startTime && o.D < endTime);
            }
        }
    }
}
