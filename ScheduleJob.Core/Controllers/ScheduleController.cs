using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScheduleJob.Core.Contract;
using ScheduleJob.Core.Contract.Response;
using ScheduleJob.Core.Contract.ScheduleModels;
using ScheduleJob.Core.IServices.QuartzCenter;

namespace ScheduleJob.Core.Controllers
{
    /// <summary>
    /// 任务管理
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        ISchedulerService _schedulerService;
        public ScheduleController(ISchedulerService schedulerService)
        {
            _schedulerService = schedulerService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        // GET: api/Schedule 
        [HttpGet]
        [Route("GetAllJob")]
        public async Task<List<ScheduleEntity>> Get()
        {
            var data = await _schedulerService.GetAllScheduleNotIsDrop();
            return data;
        }

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="page"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        // GET: api/Schedule
        [HttpGet]
        public async Task<BaseResponse<PageModel<ScheduleEntity>>> Get(int page = 1, string key = "")
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
            {
                key = "";
            }
            BaseQuery query = new BaseQuery();
            var data = await _schedulerService.GetScheduleJobByPage(query, key);
            return new BaseResponse<PageModel<ScheduleEntity>>()
            {
                Data = data
            };
        }

        /// <summary>
        /// 添加一个Job
        /// </summary>
        /// <param name="scheduleEntity"></param>
        /// <returns></returns>
        // POST: api/Schedule
        [Route("AddSchedule")]
        [HttpPost]
        public async Task<BaseResponse<string>> Post(ScheduleEntity scheduleEntity)
        {
            var data = new BaseResponse<string>();
            var success = await _schedulerService.AddScheduleAsync(scheduleEntity);
            if (success)
            {
                data.Msg = "添加成功";
            }
            return data;
        }
        /// <summary>
        /// 更新用户与角色
        /// </summary>
        /// <param name="scheduleEntity"></param>
        /// <returns></returns>
        [Route("UpdateSchedule")]
        [HttpPut]
        public async Task<BaseResponse<string>> Put(ScheduleEntity scheduleEntity)
        {
            var data = new BaseResponse<string>();
            if (scheduleEntity != null && scheduleEntity.Id > 0)
            {
                var success = await _schedulerService.UpdateScheduleAsync(scheduleEntity);
                if (success)
                {
                    data.Msg = "更新成功";
                }
            }
            return data;
        }
        /// <summary>
        /// 启动计划任务
        /// </summary>
        /// <param name="JobId"></param>
        /// <returns></returns>
        [Route("StartJob")]
        [HttpGet]
        public async Task<BaseResponse<string>> StartJob(int jobId)
        {
            var data = new BaseResponse<string>();
            try
            {
                var Resultmodel = await _schedulerService.StartJob(jobId);
                data.Msg = Resultmodel.Message;
                return data;
            }
            catch (Exception ex)
            {
                data.Msg = ex.Message;
                return data;
            }
        }
        /// <summary>
        /// 停止一个计划任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [Route("StopJob")]
        [HttpGet]
        public async Task<BaseResponse<string>> StopJob(int jobId)
        {
            var data = new BaseResponse<string>();
            try
            {
                var Resultmodel = await _schedulerService.StopJob(jobId);
                data.Msg = Resultmodel.Message;
                return data;
            }
            catch (Exception ex)
            {
                data.Msg = ex.Message;
                return data;
            }
        }
    }
}