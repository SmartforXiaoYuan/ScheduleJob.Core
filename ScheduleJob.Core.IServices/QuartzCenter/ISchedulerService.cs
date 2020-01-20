using Quartz;
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
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<PageModel<ScheduleEntity>> GetScheduleJobByPage(BaseQuery query, string key = "");
        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        Task<List<ScheduleEntity>> GetAllScheduleNotIsDrop();
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="sysSchedule"></param>
        /// <returns></returns>
        Task<bool> AddScheduleAsync(ScheduleEntity sysSchedule);
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="sysSchedule"></param>
        /// <returns></returns>
        Task<bool> UpdateScheduleAsync(ScheduleEntity sysSchedule);
        Task<JobResuleModel> StartJob(int Id);
        Task<JobResuleModel> StopJob(int Id);
        #endregion



    }
}
