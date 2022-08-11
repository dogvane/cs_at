using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinaSpider.Entitys.Sina
{

    // {"d":"2022-07-13 09:13:00","o":"3905.000","h":"3906.000","l":"3900.000","c":"3902.000","v":"6843","p":"1983181"}

    /// <summary>
    /// k 线用的数据格式
    /// </summary>
    public class KBar
    {
        /// <summary>
        /// 时间
        /// </summary>
        public string d { get; set; }

        /// <summary>
        /// 开盘价
        /// </summary>
        public string o { get; set; }

        /// <summary>
        /// 最高价
        /// </summary>
        public string h { get; set; }

        /// <summary>
        /// 最低价
        /// </summary>
        public string l { get; set; }

        /// <summary>
        /// 收盘价
        /// </summary>
        public string c { get; set; }

        /// <summary>
        /// 应该是成交量（手）
        /// </summary>
        public string v { get; set; }

        /// <summary>
        /// 持仓数量
        /// </summary>
        public string p { get; set; }

        /// <summary>
        /// 应该是结算价
        /// </summary>
        public string s { get; set; }
    }
}
