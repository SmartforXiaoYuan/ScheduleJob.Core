using Microsoft.Extensions.Configuration;
using System;
using System.Threading;

namespace ScheduleJob.Core.Utility
{
    public class AppSettingx : Singleton<AppSettingx>
    {
        private IConfigurationRoot Config { get; }
        public AppSettingx()
        {
            Config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                //.AddJsonFile($"appsettings.{Kernel.Environment}.json", optional: true)
                .Build();
        }
        public static string GetConfig(string name)
        {
            return Instance.Config.GetSection(name).Value;
        }

    }

    /// <summary>
    /// 封装单例类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> where T : class, new()
    {
        private static T _instance;
        private static object _syncRoot = new Object();
        /// <summary>
        /// 单例实例
        /// </summary>
        public static T Instance
        {
            get
            {
                var instance = _instance;
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        instance = Volatile.Read(ref _instance);
                        if (instance == null)
                        {
                            instance = new T();
                        }
                        Volatile.Write(ref _instance, instance);
                    }
                }
                return instance;
            }
        }
    }
}
