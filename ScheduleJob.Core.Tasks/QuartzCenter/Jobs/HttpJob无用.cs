
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using ScheduleJob.Core.Contract.ScheduleModels;
using ScheduleJob.Core.IServices.QuartzCenter;
using ScheduleJob.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
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
        ISchedulerService _schedulerService;
        public HttpJob(ISchedulerService schedulerService)
        {
            //this.logger = logger;
            //this.configuration = configuration;
            _schedulerService = schedulerService;
        }
        public async System.Threading.Tasks.Task Execute(IJobExecutionContext context)
        {
            //this.logger.LogWarning($"Hello from scheduled task {DateTime.Now.ToLongTimeString()}");
            //await Task.CompletedTask;
           
            ScheduleEntity taskOptions =await GetTaskOptionsAsync(context);
            AbstractTrigger trigger = (context as JobExecutionContextImpl).Trigger as AbstractTrigger;


            if (taskOptions == null)
            {
                //"未到找作业或可能被移除"
                await System.Threading.Tasks.Task.CompletedTask;
            }
            if (string.IsNullOrEmpty(taskOptions.ApiUrl) || taskOptions.ApiUrl == "/")
            {
                //"未配置url"
                await System.Threading.Tasks.Task.CompletedTask;
            }
            Dictionary<string, string> header = new Dictionary<string, string>();
            //if (!string.IsNullOrEmpty(sysScheduleModel.AuthKey)
            //    && !string.IsNullOrEmpty(sysScheduleModel.AuthValue))
            //{
            //    header.Add(sysScheduleModel.AuthKey.Trim(), taskOptions.AuthValue.Trim());
            //}

            if (taskOptions.MethodType?.ToUpper() == "GET")
            {
                await HttpUtil.HttpGetAsync(taskOptions.ApiUrl, header);
            }
            else
            {
                await HttpUtil.HttpPostAsync(taskOptions.ApiUrl, null, null, 30000, header);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context">通过作业上下文获取作业对应的配置参数</param>
        /// <returns></returns>
        public async Task<ScheduleEntity> GetTaskOptionsAsync(IJobExecutionContext context)
        {
            var _taskList = await _schedulerService.GetAllScheduleNotIsDrop();
            AbstractTrigger trigger = (context as JobExecutionContextImpl).Trigger as AbstractTrigger;
            ScheduleEntity taskOptions = _taskList.Where(x => x.Id.ToString() == trigger.Name && x.JobGroupName == trigger.Group).FirstOrDefault();
            return taskOptions ?? _taskList.Where(x => x.Name == trigger.JobName && x.JobGroupName == trigger.JobGroup).FirstOrDefault();
        }

     
    }

}
