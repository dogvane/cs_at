using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace at.strategy
{
    /// <summary>
    /// 策略的基类
    /// </summary>
    public abstract class BaseStrategy
    {
        /// <summary>
        /// 触发行情数据
        /// </summary>
        /// <param name="tickItem"></param>
        public abstract void OnTick(TickItem tickItem);
    }
}
