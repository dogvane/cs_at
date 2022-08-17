using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinaSpider.Entitys.Sina
{
    /// <summary>
    /// 分时价格
    /// </summary>
    public class TimePrice
    {
        /// <summary>
        /// 时间
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 当前价格
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// 均价
        /// </summary>
        public double AveragePrice { get; set; }

        /// <summary>
        /// 成交量
        /// </summary>
        public int Volume { get; set; }

        /// <summary>
        /// 当前持仓量
        /// </summary>
        public int Interest { get; set; }
    }
}
