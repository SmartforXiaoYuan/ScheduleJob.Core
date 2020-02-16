using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ScheduleJob.Core.Contract.Seed;
using Serilog;
using Serilog.Events;

namespace ScheduleJob.Core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // 配置 Serilog 
            #region Serilog
            //Log.Logger = new LoggerConfiguration()
            //    // 最小的日志输出级别
            //    .MinimumLevel.Information()
            //    // 日志调用类命名空间如果以 Microsoft 开头，覆盖日志输出最小级别为 Information
            //    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            //    .Enrich.FromLogContext()
            //    // 配置日志输出到控制台
            //    .WriteTo.Console()
            //    // 配置日志输出到文件，文件输出到当前项目的 logs 目录下
            //    // 日记的生成周期为每天
            //    .WriteTo.File(Path.Combine("logs", @"logx.txt"), rollingInterval: RollingInterval.Day)
            //    // 创建 logger
            //    .CreateLogger();
            #endregion



            // 创建可用于解析作用域服务的新 Microsoft.Extensions.DependencyInjection.IServiceScope。
            //CreateHostBuilder(args).Build().Run();//原本
            // 生成承载 web 应用程序的 Microsoft.AspNetCore.Hosting.IWebHost。Build是WebHostBuilder最终的目的，将返回一个构造的WebHost，最终生成宿主。
            var host = CreateHostBuilder(args).Build();

            // 创建可用于解析作用域服务的新 Microsoft.Extensions.DependencyInjection.IServiceScope。
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var configuration = services.GetRequiredService<IConfiguration>();
                    if (Convert.ToBoolean(configuration.GetSection("AppSettings")["SeedDBEnabled"]))
                    {
                        var myContext = services.GetRequiredService<MyContext>();
                        DBSeed.SeedAsync(myContext).Wait();
                    }
                }
                catch (Exception e)
                {
                    throw;
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                     //.UseSerilog()//Serilog.AspNetCore  注入了会自动释放
                     .UseStartup<Startup>();
                });
    }
}
