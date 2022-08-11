using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinaSpider.Entitys.Sina
{

    // "symbol":"SP0","exchange":"shfe","name":"\u7eb8\u6d46\u8fde\u7eed"


    /// <summary>
    /// 这里主要是从网站抓取代码时返回的数据结构
    /// 源数据还有很多结构，展示不需要
    /// </summary>
    public class MarketTradeCode
    {
        /// <summary>
        /// 交易代码
        /// </summary>
        public string symbol { get; set; }

        /// <summary>
        /// 交易所
        /// </summary>
        public string exchange { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        public string name { get; set; }

    }
}
