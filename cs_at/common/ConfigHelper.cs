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
            Console.WriteLine($"base path: {directory}");
            foreach(var fileName in Directory.GetFiles(directory, "*.json"))
            {
                var info = new FileInfo(fileName);
                if(info.Name.Contains("config", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        Console.WriteLine($"load config file: {fileName}");

                        var builder = new ConfigurationBuilder()
                            .AddJsonFile(info.FullName, false, false);

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
        /// 获得数据存放的目录
        /// </summary>
        /// <param name="subFolder">如果传参，则需做子目录配置</param>
        /// <param name="checkAndCreate">检查目录是否存在，如果不存在则默认进行创建</param>
        /// <returns></returns>
        public static string GetDataFolder(string subFolder = null, bool checkAndCreate = true)
        {
            var folder = GetValue("datafolder");
            if (folder == null)
                folder = Path.Combine(AppContext.BaseDirectory, "data");

            if(!string.IsNullOrEmpty(subFolder))
                folder = Path.Combine(folder, subFolder);

            if(checkAndCreate && !Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            return folder;
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
                var section = item.GetSection(key);
                if (section != null && section.Value != null)
                {
                    return section.Value;
                }
            }

            return defaultValue;
        }
    }
}
