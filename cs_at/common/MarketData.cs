using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common
{
	/// <summary>
	/// 行情数据
	/// </summary>
	/// <remarks>
	/// 当前数据结构是映射自 CThostFtdcDepthMarketDataField ，数据也将原样保存到数据库里
	/// </remarks>
	public class MarketData
    {
		/// <summary>
		/// 交易日
		/// </summary>
		public string TradingDay { get; set; }

		/// <summary>
		/// 合约代码
		/// </summary>
		public string InstrumentID { get; set; }

		/// <summary>
		/// 交易所代码
		/// </summary>
		public string ExchangeID { get; set; }

		/// <summary>
		/// 合约在交易所的代码
		/// </summary>
		public string ExchangeInstID { get; set; }

		/// <summary>
		/// 最新价
		/// </summary>
		public double LastPrice;

		/// <summary>
		/// 上次结算价
		/// </summary>
		public double PreSettlementPrice;

		/// <summary>
		/// 昨收盘
		/// </summary>
		public double PreClosePrice;

		/// <summary>
		/// 昨持仓量
		/// </summary>
		public double PreOpenInterest;

		/// <summary>
		/// 今开盘
		/// </summary>
		public double OpenPrice;

		/// <summary>
		/// 最高价
		/// </summary>
		public double HighestPrice;

		/// <summary>
		/// 最低价
		/// </summary>
		public double LowestPrice;

		/// <summary>
		/// 数量
		/// </summary>
		public int Volume;

		/// <summary>
		/// 成交金额
		/// </summary>
		public double Turnover;

		/// <summary>
		/// 持仓量
		/// </summary>
		public double OpenInterest;

		/// <summary>
		/// 今收盘
		/// </summary>
		public double ClosePrice;

		/// <summary>
		/// 本次结算价
		/// </summary>
		public double SettlementPrice;

		/// <summary>
		/// 涨停板价
		/// </summary>
		public double UpperLimitPrice;

		/// <summary>
		/// 跌停板价
		/// </summary>
		public double LowerLimitPrice;

		/// <summary>
		/// 昨虚实度
		/// </summary>
		public double PreDelta;

		/// <summary>
		/// 今虚实度
		/// </summary>
		public double CurrDelta;

		/// <summary>
		/// 最后修改时间
		/// </summary>
		public string UpdateTime { get; set; }

		/// <summary>
		/// 最后修改毫秒
		/// </summary>
		public int UpdateMillisec;

		/// <summary>
		/// 申买价一
		/// </summary>
		public double BidPrice1;

		/// <summary>
		/// 申买量一
		/// </summary>
		public int BidVolume1;

		/// <summary>
		/// 申卖价一
		/// </summary>
		public double AskPrice1;

		/// <summary>
		/// 申卖量一
		/// </summary>
		public int AskVolume1;

		/// <summary>
		/// 申买价二
		/// </summary>
		public double BidPrice2;

		/// <summary>
		/// 申买量二
		/// </summary>
		public int BidVolume2;

		/// <summary>
		/// 申卖价二
		/// </summary>
		public double AskPrice2;

		/// <summary>
		/// 申卖量二
		/// </summary>
		public int AskVolume2;

		/// <summary>
		/// 申买价三
		/// </summary>
		public double BidPrice3;

		/// <summary>
		/// 申买量三
		/// </summary>
		public int BidVolume3;

		/// <summary>
		/// 申卖价三
		/// </summary>
		public double AskPrice3;

		/// <summary>
		/// 申卖量三
		/// </summary>
		public int AskVolume3;

		/// <summary>
		/// 申买价四
		/// </summary>
		public double BidPrice4;

		/// <summary>
		/// 申买量四
		/// </summary>
		public int BidVolume4;

		/// <summary>
		/// 申卖价四
		/// </summary>
		public double AskPrice4;

		/// <summary>
		/// 申卖量四
		/// </summary>
		public int AskVolume4;

		/// <summary>
		/// 申买价五
		/// </summary>
		public double BidPrice5;

		/// <summary>
		/// 申买量五
		/// </summary>
		public int BidVolume5;

		/// <summary>
		/// 申卖价五
		/// </summary>
		public double AskPrice5;

		/// <summary>
		/// 申卖量五
		/// </summary>
		public int AskVolume5;

		/// <summary>
		/// 当日均价
		/// </summary>
		public double AveragePrice;

		/// <summary>
		/// 业务日期
		/// </summary>
		public string ActionDay { get; set; }
	}
}
