using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScheduleJob.Core.Extensions
{
    /// <summary>
    /// 暂时没用到
    /// </summary>
    public class ScheduledJobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ScheduledJobFactory(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var serviceScope = _serviceProvider.CreateScope();
            var job = serviceScope.ServiceProvider.GetService(typeof(IJob)) as IJob;
            var ss =serviceScope.ServiceProvider.GetService(typeof(Services.QuartzCenter.HttpJob)) as IJob; 
            return job;
            //return _serviceProvider.GetService(typeof(IJob)) as IJob;
         }

        public void ReturnJob(IJob job)
        {
            var disposable = job as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }

    }
}
