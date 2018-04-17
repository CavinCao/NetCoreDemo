using System;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Common
{
    public class AppSetting
    {
        private static readonly object objLock = new object();
        private static AppSetting instance = null;

        private IConfigurationRoot Config { get; }

        private AppSetting()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Config = builder.Build();
        }

        public static AppSetting GetInstance()
        {
            if (instance == null)
            {
                lock (objLock)
                {
                    if (instance == null)
                    {
                        instance = new AppSetting();
                    }
                }
            }

            return instance;
        }

        public static string GetConfig(string name)
        {
            return GetInstance().Config.GetSection(name).Value;
        }

        public static T GetConfig<T>(string name)
        {
            string temp = GetConfig(name);
            if (string.IsNullOrWhiteSpace(temp))
                return default(T);

            return (T)Convert.ChangeType(temp, typeof(T));
        }

        public static IEnumerable<string> GetConfigArray(string name)
        {
            var section = GetInstance().Config.GetSection(name);
            if (section == null)
                return null;

            return section.AsEnumerable().Where(m => m.Value != null).Select(m => m.Value);
        }
    }
}
