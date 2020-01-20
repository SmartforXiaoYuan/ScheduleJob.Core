using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScheduleJob.Core.Extensions
{
    /// <summary>
    /// 三大核心对象：
    /// IScheduler：单元/实例
    /// 只有单元启动，里面的作业才能正常启动
    /// IJob:任务，定时执行动作就是Job 默认无状态
    /// ITrigger:定时策略
    /// </summary>
    public static class QuartzSetup
    {
        public static void AddQuartz(this IServiceCollection services, Type jobType)
        {
            services.Add(new ServiceDescriptor(typeof(IJob), jobType, ServiceLifetime.Transient));
            services.AddSingleton<IJobFactory, ScheduledJobFactory>();//将IJobFactory接口实现的实例分配给我们的IScheduler实例，该实例将用于在每个调度触发器上实例化作业实例
            services.AddSingleton<IScheduler>(provider =>
            {
                var schedulerFactory = new StdSchedulerFactory();
                var scheduler = schedulerFactory.GetScheduler().Result;
                scheduler.JobFactory = provider.GetService<IJobFactory>();
                scheduler.Start();
                return scheduler;
            });

        }

        //public static void UseQuartz(this IApplicationBuilder app)
        //{
        //    app.ApplicationServices.GetService<IScheduler>()
        //        .ScheduleJob(app.ApplicationServices.GetService<IJobDetail>(),
        //        app.ApplicationServices.GetService<ITrigger>()
        //        );
        //}
    }
}
