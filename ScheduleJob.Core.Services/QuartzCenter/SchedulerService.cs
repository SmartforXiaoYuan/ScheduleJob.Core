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
    /// <summary>
    /// Singleton
    /// </summary>
    public class SchedulerService : BaseService<ScheduleEntity>, ISchedulerService
    {
        /// <summary>
        /// 单例的只有一个
        /// </summary>

        private readonly Quartz.IScheduler _quartzScheduler;
        IScheduleRepositoty _scheduleRepositoty;
        public SchedulerService(IScheduler quartzScheduler, IScheduleRepositoty scheduleRepositoty)
        {
            _scheduleRepositoty = scheduleRepositoty;
            _quartzScheduler = quartzScheduler;
        }

        #region MyRegion
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

        public bool AddSchedule(ScheduleEntity sysSchedule)
        {
            return base.Add(sysSchedule).Result > 0;
        }

        public async Task<bool> AddScheduleAsync(ScheduleEntity sysSchedule)
        {
            return await base.Add(sysSchedule) > 0;
        }

        public async Task<bool> UpdateScheduleAsync(ScheduleEntity sysSchedule)
        {
            sysSchedule.UpdateDate = DateTime.Now;
            return await Update(sysSchedule);
        }
        /// <summary>
        /// 根据Id查询出一个Job任务实体
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<JobResuleModel> StartJob(int Id)
        {
            var model = await base.QueryById(Id);
            var ResuleModel = await AddScheduleJobAsync(model);
            if (ResuleModel.IsSuccess)
                model.IsStart = true;
            await UpdateScheduleAsync(model);
            return ResuleModel;
        }
        /// <summary>
        /// 根据Id查询出一个Job任务实体
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<JobResuleModel> StopJob(int Id)
        {
            var model = await base.QueryById(Id);
            var ResuleModel = await StopScheduleJobAsync(model);
            if (ResuleModel.IsSuccess)
                model.IsStart = false;
            await UpdateScheduleAsync(model);
            return ResuleModel;
        }
        #endregion




        /// <summary>
        /// 开启任务调度
        /// </summary>
        /// <returns></returns>
        public async Task<JobResuleModel> StartScheduleAsync()
        {
            var result = new JobResuleModel();
            try
            {
                //this._quartzScheduler.Result.JobFactory = this._iocjobFactory;
                if (!this._quartzScheduler.IsStarted)
                {
                    //等待任务运行完成
                    await this._quartzScheduler.Start();
                    await Console.Out.WriteLineAsync("任务调度开启！");
                    result.IsSuccess = true;
                    result.Message = $"任务调度开启成功";
                    return result;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = $"任务调度已经开启";
                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 停止任务调度
        /// </summary>
        /// <returns></returns>
        public async Task<JobResuleModel> StopScheduleAsync()
        {
            var result = new JobResuleModel();
            try
            {
                if (!this._quartzScheduler.IsShutdown)
                {
                    //等待任务运行完成
                    await this._quartzScheduler.Shutdown();
                    await Console.Out.WriteLineAsync("任务调度停止！");
                    result.IsSuccess = true;
                    result.Message = $"任务调度停止成功";
                    return result;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = $"任务调度已经停止";
                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 添加一个计划任务（映射程序集指定IJob实现类）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sysSchedule"></param>
        /// <returns></returns>
        public async Task<JobResuleModel> AddScheduleJobAsync(ScheduleEntity sysSchedule)
        {
            var result = new JobResuleModel();
            try
            {
                if (sysSchedule != null)
                {
                    JobKey jobKey = new JobKey(sysSchedule.Id.ToString(), sysSchedule.JobGroup);
                    if (await _quartzScheduler.CheckExists(jobKey))
                    {
                        result.IsSuccess = false;
                        result.Message = $"该任务计划已经在执行:【{sysSchedule.Name}】,请勿重复启动！";
                        return result;
                    }
                    #region 设置开始时间和结束时间
                    if (sysSchedule.BeginTime == null)
                    {
                        sysSchedule.BeginTime = DateTime.Now;
                    }
                    DateTimeOffset starRunTime = DateBuilder.NextGivenSecondDate(sysSchedule.BeginTime, 1);//设置开始时间
                    if (sysSchedule.EndTime == null)
                    {
                        sysSchedule.EndTime = DateTime.MaxValue.AddDays(-1);
                    }
                    DateTimeOffset endRunTime = DateBuilder.NextGivenSecondDate(sysSchedule.EndTime, 1);//设置暂停时间
                    #endregion

                    //判断任务调度是否开启
                    if (!_quartzScheduler.IsStarted)
                    {
                        await StartScheduleAsync();
                    }
                    //job.JobDataMap.Add("JobParam", sysSchedule.JobParams);   
                    #region 泛型传递

                    IJobDetail job = JobBuilder.Create<HttpJob>()
                        .WithIdentity(sysSchedule.Name, sysSchedule.JobGroup)
                        .Build();
                    #endregion
                    ITrigger trigger;
                    trigger = CreateCronTrigger(sysSchedule);
                    // 触发器 
                    await _quartzScheduler.ScheduleJob(job, trigger);
                    result.IsSuccess = true;
                    result.Message = $"启动任务:【{sysSchedule.Name}】成功";
                    return result;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = $"任务计划不存在:【{sysSchedule.Name}】";
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 暂停一个指定的计划任务
        /// </summary>
        /// <returns></returns>
        public async Task<JobResuleModel> StopScheduleJobAsync(ScheduleEntity sysSchedule)
        {
            var result = new JobResuleModel();
            try
            {
                JobKey jobKey = new JobKey(sysSchedule.Id.ToString(), sysSchedule.JobGroup);
                if (!await _quartzScheduler.CheckExists(jobKey))
                {
                    result.IsSuccess = false;
                    result.Message = $"未找到要暂停的任务:【{sysSchedule.Name}】";
                    return result;
                }
                else
                {
                    await this._quartzScheduler.PauseJob(jobKey);
                    result.IsSuccess = true;
                    result.Message = $"暂停任务:【{sysSchedule.Name}】成功";
                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        #region 创建触发器帮助方法
        /// <summary>
        /// 创建类型Cron的触发器
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private ITrigger CreateCronTrigger(ScheduleEntity sysSchedule)
        {
            // 作业触发器
            return TriggerBuilder.Create()
                   .WithIdentity(sysSchedule.Id.ToString(), sysSchedule.JobGroup)
                   .StartAt(sysSchedule.BeginTime.Value)//开始时间
                   .EndAt(sysSchedule.EndTime.Value)//结束数据
                   .WithCronSchedule(sysSchedule.Cron)//指定cron表达式
                   .ForJob(sysSchedule.Id.ToString(), sysSchedule.JobGroup)//作业名称
                   .Build();
        }

        public Task<JobResuleModel> ResumeJob(ScheduleEntity scheduleEntity)
        {
            throw new NotImplementedException();
        }
        #endregion
    }




}
