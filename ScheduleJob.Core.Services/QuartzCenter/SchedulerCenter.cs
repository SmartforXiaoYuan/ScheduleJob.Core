using Newtonsoft.Json;
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
    /// 控制中心
    /// </summary>
    public class SchedulerCenter : ISchedulerCenter
    {
        /// <summary>
        /// 单例的只有一个
        /// </summary>

        private readonly Quartz.IScheduler _quartzScheduler;
        public SchedulerCenter(IScheduler quartzScheduler)
        {
            _quartzScheduler = quartzScheduler;
        }

        /// <summary>
        /// 调度器信息
        /// </summary>
        /// <returns></returns>
        public async Task<SchedulerMetaData> GetSchedulerInfo()
        {
            var mate = await _quartzScheduler.GetMetaData();
            var json = JsonConvert.SerializeObject(mate);
            return mate;
        }

        #region Schedule
        /// <summary>
        /// 开启任务调度
        /// </summary>
        /// <returns></returns>
        public async Task<JobResuleModel> StartScheduleAsync()
        {
            var result = new JobResuleModel();
            result.IsSuccess = false;
            if (!this._quartzScheduler.IsStarted)
            {
                //等待任务运行完成
                await this._quartzScheduler.Start();
                await Console.Out.WriteLineAsync("任务调度开启！");
                result.IsSuccess = true;
                result.Message = $"任务调度开启成功";
            }
            return result;
        }

        /// <summary>
        /// 关闭，关机。并清理关联的资源。
        /// </summary>
        /// <returns></returns>
        public async Task<JobResuleModel> SchedulerShutdownAsync()
        {
            var result = new JobResuleModel();
            result.IsSuccess = false;
            if (!this._quartzScheduler.IsShutdown)
            {
                //等待任务运行完成
                await this._quartzScheduler.Shutdown();
                await Console.Out.WriteLineAsync("任务调度停止！");
                result.IsSuccess = true;
                result.Message = $"任务调度停止成功";

            }
            return result;
        }

        #region 备用
        /// <summary>
        /// 停止
        /// </summary>
        /// <returns></returns>
        public async Task StopScheduleAsync()
        {
            if (_quartzScheduler.IsStarted)
            {
                await _quartzScheduler.Standby();
            }
        }

        /// <summary>
        /// 重启
        /// </summary>
        /// <returns></returns>
        public async Task ResumScheduler()
        {
            if (_quartzScheduler.InStandbyMode)
            {
                //Standby  靠边站的，
                await _quartzScheduler.Start();
            }
        }
        #endregion
        #endregion

        /// <summary>
        /// 添加一个计划任务（映射程序集指定IJob实现类）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="scheduleEntity"></param>
        /// <returns></returns>
        public async Task<JobResuleModel> AddScheduleJobAsync(ScheduleEntity scheduleEntity)
        {
            var result = new JobResuleModel();
            try
            {
                if (scheduleEntity != null&& scheduleEntity.Id>0)
                {
                    JobKey jobKey = new JobKey(scheduleEntity.Id.ToString(), scheduleEntity.JobGroupName);
                    if (await _quartzScheduler.CheckExists(jobKey))
                    {
                        result.IsSuccess = false;
                        result.Message = $"该任务计划已经在执行:【{scheduleEntity.Name}】,请勿重复启动！";
                        return result;
                    }
                    #region 设置开始时间和结束时间
                    //if (scheduleEntity.BeginTime == null)
                    //{
                    //    scheduleEntity.BeginTime = DateTime.Now;
                    //}
                    scheduleEntity.BeginTime = DateTime.Now;
                    DateTimeOffset starRunTime = DateBuilder.NextGivenSecondDate(scheduleEntity.BeginTime, 1);//设置开始时间
                    scheduleEntity.EndTime = DateTime.MaxValue.AddDays(-1);
                    //if (scheduleEntity.EndTime == null)
                    //{
                    //    scheduleEntity.EndTime = DateTime.MaxValue.AddDays(-1);
                    //}
                    DateTimeOffset endRunTime = DateBuilder.NextGivenSecondDate(scheduleEntity.EndTime, 1);//设置暂停时间
                    #endregion
 
                    //判断任务调度是否开启
                    if (!_quartzScheduler.IsStarted)
                    {
                        await StartScheduleAsync();
                    }
                    //参数
                    //IJobDetail job = new JobDetailImpl(sysSchedule.Id.ToString(), sysSchedule.JobGroup, jobType);
                    #region 泛型传递
                    IJobDetail job = JobBuilder.Create<HttpJob>()
                        .WithIdentity(scheduleEntity.Id.ToString(), scheduleEntity.JobGroupName)//需要和CreateCronTrigger中的forJob对应
                        .Build();
                    #endregion
                    ITrigger trigger = CreateCronTrigger(scheduleEntity);
                    //触发器来安排作业
                    await _quartzScheduler.ScheduleJob(job, trigger);
                    result.IsSuccess = true;
                    result.Message = $"启动任务:【{scheduleEntity.Name}】成功";
                    return result;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = $"任务计划不存在:【{scheduleEntity.Name}】";
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public async Task AddOrUpdateJob(string name, string group, string des, string dll, string typename)
        //{
        //    //StoreDurably  持久化存储
        //    //该工作在孤儿之后是否应该继续存储（没有触发器就是孤儿）
        //    var dllinstanck = System.Reflection.Assembly.LoadFile(dll).CreateInstance(typename);
        //    IJobDetail job = JobBuilder.Create(dllinstanck.GetType()).WithIdentity(name, group).WithDescription(des).StoreDurably(true).Build();
        //    await _quartzScheduler.AddJob(job, true);
        //}

        /// <summary>
        /// 删除Job
        /// </summary>
        /// <param name="scheduleEntity"></param>
        /// <returns></returns>
        public async Task<JobResuleModel> DelJobAsync(ScheduleEntity scheduleEntity)
        {
            var result = new JobResuleModel();
            result.IsSuccess = false;
            JobKey jobKey = new JobKey(scheduleEntity.Id.ToString(), scheduleEntity.JobGroupName);
            if (await _quartzScheduler.CheckExists(jobKey))
            {
                result.IsSuccess = true;
                result.Message = $"(停止)任务:【{scheduleEntity.Name}】成功";
                await _quartzScheduler.DeleteJob(jobKey);
            }
            return result;
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
                   .WithIdentity(sysSchedule.Id.ToString(), sysSchedule.JobGroupName)
                   .StartAt(sysSchedule.BeginTime.Value)//开始时间
                   .EndAt(sysSchedule.EndTime.Value)//结束数据
                   .WithCronSchedule(sysSchedule.Cron)//指定cron表达式
                   .ForJob(sysSchedule.Id.ToString(), sysSchedule.JobGroupName)//作业名称
                   .Build();
        }

        #endregion
    }
}
