using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using Quartz.Spi;
using ScheduleJob.Core.Contract;
using ScheduleJob.Core.Contract.Response;
using ScheduleJob.Core.Contract.ScheduleModels;
using ScheduleJob.Core.IRepository;
using ScheduleJob.Core.IServices.QuartzCenter;
using ScheduleJob.Core.Services.BASE;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScheduleJob.Core.Services.QuartzCenter
{
    public class SchedulerService : BaseService<ScheduleEntity>, ISchedulerService
    {

        IScheduleRepositoty _scheduleRepositoty;

        ISchedulerCenter _schedulerCenter;
        public SchedulerService( IScheduleRepositoty scheduleRepositoty, ISchedulerCenter schedulerCenter)
        {
            _scheduleRepositoty = scheduleRepositoty;
            _schedulerCenter = schedulerCenter;
        }

        
        public async Task<PageModel<ScheduleEntity>> GetScheduleJobByPage(BaseQuery query, string key = "")
        {
            var list = await base.QueryPage(a => a.IsDrop == false && a.Name.Contains(key), query.PageIndex, query.PageSize, " Id desc "); ;
            return list;
        }
        /// <summary>
        /// 获取所有未删除的Job
        /// </summary>
        /// <returns></returns>
        public async Task<List<ScheduleEntity>> GetAllScheduleNotIsDrop()
        {
            var list = await base.Query(a => a.IsDrop == false); ;
            return list;
        }

        public async Task<bool> AddScheduleAsync(ScheduleEntity sysSchedule)
        {
            return await base.Add(sysSchedule) > 0;
        }

        public async Task<bool> UpdateScheduleAsync(ScheduleEntity sysSchedule)
        {
            sysSchedule.UpdateDate = DateTime.Now;
            //需要考虑一下 POST参数、时间策略 需要先停止然后再启动
            var isNeedStop =await IsNeedStopIJobAsync(sysSchedule);
            if (isNeedStop)
            {
                await StopJob(sysSchedule.Id);
            }
            if(sysSchedule.IsStart&& !sysSchedule.IsDrop)
            {
                await StartJob(sysSchedule.Id);
            }
            return await Update(sysSchedule);
        }      

        /// <summary>
        /// 启动新的Job
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<JobResuleModel> StartJob(int Id)
        {
            var model = await base.QueryById(Id);
            var ResuleModel = await _schedulerCenter.AddScheduleJobAsync(model);
            if (ResuleModel.IsSuccess)
                model.IsStart = true;
            await UpdateScheduleAsync(model);
            return ResuleModel;
        }
        /// <summary>
        /// 停止(删除)Job
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<JobResuleModel> StopJob(int Id)
        {
            var model = await base.QueryById(Id);
            var ResuleModel = await _schedulerCenter.DelJobAsync(model);
            if (ResuleModel.IsSuccess)
                model.IsStart = false;
            await UpdateScheduleAsync(model);
            return ResuleModel;
        }

        /// <summary>
        /// POST参数、时间策略 需要先停止然后再启动
        /// </summary>
        /// <param name="sysSchedule"></param>
        /// <returns></returns>
        public async Task<bool> IsNeedStopIJobAsync(ScheduleEntity sysSchedule)
        {
            bool success = false;
            var oldSysSchedule = await base.QueryById(sysSchedule.Id);
            if (!sysSchedule.IsDrop)
            {
                success = true;
            }
            if (oldSysSchedule.Cron != sysSchedule.Cron || oldSysSchedule.ApiUrl != sysSchedule.ApiUrl || oldSysSchedule.RequestValue != sysSchedule.RequestValue || oldSysSchedule.MethodType != sysSchedule.MethodType || !sysSchedule.IsStart)
            {
                success = true;
            }
            return success;
        }



    }




}
