using ScheduleJob.Core.Contract;
using ScheduleJob.Core.Contract.Response;
using ScheduleJob.Core.Contract.ScheduleModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleJob.Core.IServices.QuartzCenter
{

    public interface ISchedulerService
    {
        #region MyRegion
        Task<PageModel<ScheduleEntity>> GetScheduleJobByPage(BaseQuery query, string key = "");
        Task<List<ScheduleEntity>> GetAllScheduleNotIsDrop();
        bool AddSchedule(ScheduleEntity sysSchedule);
        Task<bool> AddScheduleAsync(ScheduleEntity sysSchedule);
        Task<bool> UpdateScheduleAsync(ScheduleEntity sysSchedule);
        Task<JobResuleModel> StartJob(int Id);
        Task<JobResuleModel> StopJob(int Id);
        #endregion



        /// <summary>
        /// 开启任务调度
        /// </summary>
        /// <returns></returns>
        Task<JobResuleModel> StartScheduleAsync();
        /// <summary>
        /// 停止任务调度
        /// </summary>
        /// <returns></returns>
        Task<JobResuleModel> StopScheduleAsync();
        /// <summary>
        /// 持久化
        /// </summary>
        /// <param name="sysSchedule"></param>
        /// <returns></returns>
        Task<JobResuleModel> AddScheduleJobAsync(ScheduleEntity scheduleEntity);
        /// <summary>
        /// 停止一个任务
        /// </summary>
        /// <param name="sysSchedule"></param>
        /// <returns></returns>
        Task<JobResuleModel> StopScheduleJobAsync(ScheduleEntity scheduleEntity);
        /// <summary>
        /// 恢复一个任务
        /// </summary>
        /// <param name="sysSchedule"></param>
        /// <returns></returns>
        Task<JobResuleModel> ResumeJob(ScheduleEntity scheduleEntity);

    }
}
