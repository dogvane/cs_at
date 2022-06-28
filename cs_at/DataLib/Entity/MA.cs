using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownByWind.Entity
{
    /// <summary>
    /// 均线
    /// </summary>
    public class MA
    {
        Dictionary<MAType, double> maData = new Dictionary<MAType, double>(16);

        /// <summary>
        /// 获得特定类型的均线数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public double GetData(MAType type)
        {
            if(maData.TryGetValue(type, out double value))
            {
                return value;
            }

            return 0; 
        }

        /// <summary>
        /// 设置均线数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public void SetData(MAType type, double value)
        {
            maData[type] = value;
        }

        /// <summary>
        /// 一个简化操作的索引器
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public double this[MAType type]
        {
            get
            {
                return GetData(type);
            }
            set
            {
                SetData(type, value);
            }
        }

        public double _5 { get => GetData(MAType._5); }
        public double _10 { get => GetData(MAType._10); }
        public double _20 { get => GetData(MAType._20); }
        public double _40 { get => GetData(MAType._40); }
        public double _120 { get => GetData(MAType._120); }
        public double _240 { get => GetData(MAType._240); }

    }

    /// <summary>
    /// 一些常见的均线类型
    /// </summary>
    public enum MAType
    {
        /// <summary>
        /// 5日均线
        /// </summary>
        _5 = 5,

        /// <summary>
        /// 10日均线
        /// </summary>

        _10 = 10,

        /// <summary>
        /// 20日均线（类似月均线）
        /// </summary>
        _20 = 20,

        /// <summary>
        /// 40日均线（类似一个半月）
        /// </summary>
        _40 = 40,

        /// <summary>
        /// 60日均线（类似2个月）
        /// </summary>
        _60 = 60,

        /// <summary>
        /// 半年线
        /// </summary>
        _120 = 120,

        /// <summary>
        /// 年线
        /// </summary>
        _240 = 240,
    }
}
