using Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common
{
    /// <summary>
    /// 配置文件的辅助类
    /// 目的是方便处理一些配置文件
    /// </summary>
    public class ConfigHelper
    {
        private static Dictionary<string,IConfiguration> _configurationMap = new Dictionary<string, IConfiguration>();
        static List<IConfiguration> _allconfigurations = new List<IConfiguration>();

        static ConfigHelper()
        {
            //在当前目录或者根目录中寻找文件

            var directory = AppContext.BaseDirectory;

            foreach(var fileName in Directory.GetFiles(directory, "*.json"))
            {
                var info = new FileInfo(fileName);
                if(info.Name.Contains("config", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        var builder = new ConfigurationBuilder()
                            .AddJsonFile(info.FullName, false, true);

                        var _configuration = builder.Build();
                        _allconfigurations.Add(_configuration);

                        _configurationMap.Add(info.Name, _configuration);
                    }
                    catch(Exception ex)
                    {
                        Logger.Error($"load config file:{fileName} fail.", ex);
                    }
                }
            }
        }

        /// <summary>
        /// 获取配置值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetValue(string key, string defaultValue = null)
        {
            foreach(var item in _allconfigurations)
            {
                if (item.GetSection(key) != null)
                    return item.GetSection(key).Value;
            }

            return defaultValue;
        }
    }
}
