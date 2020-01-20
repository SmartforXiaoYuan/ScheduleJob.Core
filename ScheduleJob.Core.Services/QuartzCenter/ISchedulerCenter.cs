using Quartz;
using ScheduleJob.Core.Contract.Response;
using ScheduleJob.Core.Contract.ScheduleModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleJob.Core.Services.QuartzCenter
{
    /// <summary>
    /// Scheduler基本操作
    /// </summary>
    public interface ISchedulerCenter
    {
        Task<SchedulerMetaData> GetSchedulerInfo();
        /// <summary>
        /// 开启任务调度
        /// </summary>
        /// <returns></returns>
        Task<JobResuleModel> StartScheduleAsync();

        /// <summary>
        /// 关闭，关机。并清理关联的资源。（停止任务调度）
        /// </summary>
        /// <returns></returns>
        Task<JobResuleModel> SchedulerShutdownAsync();

        /// <summary>
        /// 持久化
        /// </summary>
        /// <param name="scheduleEntity"></param>
        /// <returns></returns>
        Task<JobResuleModel> AddScheduleJobAsync(ScheduleEntity scheduleEntity);
        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="scheduleEntity"></param>
        /// <returns></returns>
        Task<JobResuleModel> DelJobAsync(ScheduleEntity scheduleEntity);

    }
}
