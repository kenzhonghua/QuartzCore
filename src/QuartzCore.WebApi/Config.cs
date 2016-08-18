using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QuartzCore.WebApi
{
    public class Config
    {
        public static IConfigurationBuilder builder = null;
        public static IDisposable callbackRegistration;
        public static IConfigurationRoot configuration = null;

        private static ConfigModel configInfo = null;
        private const string key = "QuartzCore";

        public static ConfigModel ConfigInfo
        {
            get
            {
                configInfo = LoadConfig(key);
                return configInfo;
            }
        }

        public static ConfigModel LoadConfig(string key)
        {
            if (builder == null || configuration == null)
            {
                builder = new ConfigurationBuilder()
                                    .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "QuartzCore.json"), optional: true, reloadOnChange: true)
                                    .AddEnvironmentVariables();
                configuration = builder.Build();
            }
            if (configInfo == null)
            {
                configInfo = new ConfigModel();
                configuration.GetSection(key).Bind(configInfo);
                callbackRegistration = configuration.GetReloadToken().RegisterChangeCallback(OnSettingChanged, configuration);
            }
            return configInfo;
        }

        private static void OnSettingChanged(object state)
        {
            callbackRegistration?.Dispose();
            IConfiguration configuration = (IConfiguration)state;
            configInfo = null;
        }
    }

    public class ConfigModel
    {
        /// <summary>
        /// IP
        /// </summary>
        public List<string> ServerIp { get; set; }

        /// <summary>
        /// 端口号
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// HostName
        /// </summary>
        public string HostName { get; set; }

    }

}
