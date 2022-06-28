using DownByWind.Entity;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLib.Entity
{
    /// <summary>
    /// 一个组合后的，用于业务处理的K线数据
    /// </summary>
    [Alias("Bars2")]
    public class Bar2 : Bar
    {
        /// <summary>
        /// 均线数据
        /// </summary>
        public MA MA { get; set; } = new MA();
    }
}
