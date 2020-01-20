using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using ScheduleJob.Core.Contract.ScheduleModels;
using ScheduleJob.Core.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleJob.Core.Services.QuartzCenter
{
    //一个Job可能会有多个Trigger。导致一个Job，同时执行。
    [DisallowConcurrentExecution]
    public class HttpJob : IJob
    {
        //private readonly IConfiguration configuration;
        //private readonly ILogger<HttpJob> logger;
        public HttpJob()
        {
            //this.logger = logger;
            //this.configuration = configuration;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            //this.logger.LogWarning($"Hello from scheduled task {DateTime.Now.ToLongTimeString()}");
            //await Task.CompletedTask;
            ScheduleEntity sysScheduleModel = new ScheduleEntity();
            AbstractTrigger trigger = (context as JobExecutionContextImpl).Trigger as AbstractTrigger;
            if (sysScheduleModel == null)
            {
                //"未到找作业或可能被移除"
                await Task.CompletedTask;
            }
            if (string.IsNullOrEmpty(sysScheduleModel.ApiUrl) || sysScheduleModel.ApiUrl == "/")
            {
                //"未配置url"
                await Task.CompletedTask;
            }
            Dictionary<string, string> header = new Dictionary<string, string>();
            //if (!string.IsNullOrEmpty(sysScheduleModel.AuthKey)
            //    && !string.IsNullOrEmpty(sysScheduleModel.AuthValue))
            //{
            //    header.Add(sysScheduleModel.AuthKey.Trim(), taskOptions.AuthValue.Trim());
            //}

            if (sysScheduleModel.MethodType?.ToUpper() == "GET")
            {
                await HttpUtil.HttpGetAsync(sysScheduleModel.ApiUrl, header);
            }
            else
            {
                await HttpUtil.HttpPostAsync(sysScheduleModel.ApiUrl, null, null, 30000, header);
            }
        }
    }
}
