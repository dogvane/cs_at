using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console; // 针对Console的日志输出
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Common
{
    /// <summary>
    /// 默认的日志记录器
    /// 这样可以方便切换日志组件
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// 默认构造方法
        /// </summary>
        static Logger()
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                  .AddFilter("Program", LogLevel.Debug)
                  .AddConsole();
            });

            s_logger = loggerFactory.CreateLogger("Program");
        }

        static ILogger s_logger;

        /// <summary>
        /// 输出Debug信息
        /// </summary>
        /// <param name="message">日志消息</param>
        public static void Debug(string message)
        {
            s_logger.LogDebug(message);
        }

        /// <summary>
        /// 输出Debug信息
        /// </summary>
        /// <param name="format">日志格式 {0} {1} 的替换方式</param>
        /// <param name="param">替换参数</param>
        public static void Debug(string format, params object[] param)
        {
            s_logger.LogDebug(format, param);
        }

        /// <summary>
        /// 输出Info信息
        /// </summary>
        /// <param name="message">日志消息</param>
        public static void Info(string message)
        {
            s_logger.LogInformation(message);
        }

        /// <summary>
        /// 输出Info信息
        /// </summary>
        /// <param name="format">日志格式 {0} {1} 的替换方式</param>
        /// <param name="param">替换参数</param>
        public static void Info(string format, params object[] param)
        {
            s_logger.LogInformation(format, param);
        }

        /// <summary>
        /// 输出Warn信息
        /// </summary>
        /// <param name="message">日志消息</param>
        public static void Warn(string message)
        {
            s_logger.LogWarning(message);
        }

        /// <summary>
        /// 输出Warn信息
        /// </summary>
        /// <param name="format">日志格式 {0} {1} 的替换方式</param>
        /// <param name="param">替换参数</param>
        public static void Warn(string format, params object[] param)
        {
            s_logger.LogWarning(format, param);
        }

        /// <summary>
        /// 输出Error信息
        /// </summary>
        /// <param name="message">日志消息</param>
        public static void Error(string message)
        {
            s_logger.LogError(message);
        }

        /// <summary>
        /// 输出Error信息
        /// </summary>
        /// <param name="format">日志格式 {0} {1} 的替换方式</param>
        /// <param name="param">替换参数</param>
        public static void Error(string format, string param)
        {
            s_logger.LogError(format, param);
        }

        /// <summary>
        /// 输出Error信息
        /// </summary>
        /// <param name="format">日志格式 {0} {1} 的替换方式</param>
        /// <param name="param">替换参数</param>
        public static void Error(string format, int param)
        {
            s_logger.LogError(format, param);
        }

        /// <summary>
        /// 输出Error信息
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="message">日志消息</param>
        public static void Error(Exception ex, string message)
        {
            s_logger.LogError(ex, message);
        }

        /// <summary>
        /// 输出Error信息
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="message">日志消息</param>
        public static void Error(string message, Exception ex)
        {
            s_logger.LogError(ex, message);
        }

        /// <summary>
        /// 输出Error信息
        /// </summary>
        /// <param name="format">日志格式 {0} {1} 的替换方式</param>
        /// <param name="param">替换参数</param>
        public static void Error(Exception ex, string format, params object[] param)
        {
            s_logger.LogError(ex, format, param);
        }

        /// <summary>
        /// 输出Error信息
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="param1">替换参数</param>
        /// <param name="param2">替换参数</param>
        public static void Error(string message, string param1, string param2)
        {
            s_logger.LogError(message, new object[] { param1, param2 });
        }

        /// <summary>
        /// 输出Error信息
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="param1">替换参数</param>
        /// <param name="param2">替换参数</param>
        /// <param name="param3">替换参数</param>
        public static void Error(string message, string param1, string param2, string param3)
        {
            s_logger.LogError(message, new object[] { param1, param2, param3 });
        }

        /// <summary>
        /// 输出Error信息
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="param1">替换参数</param>
        /// <param name="ex">异常</param>
        public static void Error(Exception ex, string message, string param1)
        {
            s_logger.LogError(ex, message, param1);
        }

        /// <summary>
        /// 输出Error信息
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="ex">异常</param>
        /// <param name="param1">替换参数</param>
        /// <param name="param2">替换参数</param>
        public static void Error(Exception ex, string message, string param1, string param2)
        {
            s_logger.LogError(ex, message, param1, param2);
        }

        /// <summary>
        /// 输出Error信息
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="ex">异常</param>
        /// <param name="param1">替换参数</param>
        /// <param name="param2">替换参数</param>
        /// <param name="param3">替换参数</param>
        public static void Error(Exception ex, string message, string param1, string param2, string param3)
        {
            s_logger.LogError(ex, message, param1, param2, param3);
        }

    }
}
