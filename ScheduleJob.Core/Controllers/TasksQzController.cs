using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScheduleJob.Core.Contract;
using ScheduleJob.Core.Contract.Dtos;
using ScheduleJob.Core.IServices;
using ScheduleJob.Core.Services.QuartzCenter;
using ScheduleJob.Core.Utility;
using ScheduleJob.Core.Utility.Helper;

namespace ScheduleJob.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksQzController : ControllerBase
    {
        private readonly ITasksQzServices _tasksQzService;
        private readonly ISchedulerCenter _schedulerCenter;
        


        public TasksQzController(ITasksQzServices tasksQzServices, ISchedulerCenter schedulerCenter )
        {
            _tasksQzService = tasksQzServices;
            _schedulerCenter = schedulerCenter;
          
            //Log.Info("测试测试");
        }


        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="page"></param>
        /// <param name="key"></param>
        /// <returns></returns>

        [HttpGet("list")]
        public async Task<MessageModel<PageModel<TasksQz>>> Get(int page = 1, string key = "")
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
            {
                key = "";
            }
            int intPageSize = 50;

            Expression<Func<TasksQz, bool>> whereExpression = a => a.IsDeleted != (int)IsDeleted.Deleted && (a.Name != null && a.Name.Contains(key));

            var data = await _tasksQzService.QueryPage(whereExpression, page, intPageSize, " Id desc ");

            return new MessageModel<PageModel<TasksQz>>()
            {
                msg = "获取成功",
                success = data.PageCount >= 0,
                response = data
            };

        }

        /// <summary>
        /// 添加计划任务（暂不支持url）
        /// </summary>
        /// <param name="tasksQz"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<string>> Post([FromBody] TasksQzInputDto tasksQz)
        {
            var data = new MessageModel<string>();
            var id = (await _tasksQzService.AddAsync(tasksQz));
            data.success = id > 0;
            if (data.success)
            {
                data.response = id.ObjToString();
                data.msg = "添加成功";
            }

            return data;
        }


        /// <summary>
        /// 修改计划任务
        /// </summary>
        /// <param name="tasksQz"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<MessageModel<string>> Put([FromBody] TasksQz tasksQz)
        {
            var data = new MessageModel<string>();
            if (tasksQz != null && tasksQz.Id > 0)
            {
                data.success = await _tasksQzService.Update(tasksQz);
                if (data.success)
                {
                    data.msg = "更新成功";
                    data.response = tasksQz?.Id.ObjToString();
                }
            }

            return data;
        }


        /// <summary>
        /// 启动计划任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [HttpPost("StartJob/{jobId}")]
        public async Task<MessageModel<string>> StartJob(int jobId)
        {
            var data = new MessageModel<string>();

            var model = await _tasksQzService.QueryById(jobId);
            if (model != null)
            {
                var ResuleModel = await _schedulerCenter.AddScheduleJobAsync(model);
                if (ResuleModel.success)
                {
                    model.IsStart = (int)BoolStatus.True;
                    data.success = await _tasksQzService.Update(model);
                }
                data = ResuleModel;
            }
            return data;

        }

        /// <summary>
        /// 停止一个计划任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>        
        [HttpPost]
        [Route("StopJob/{jobId}")]
        public async Task<MessageModel<string>> StopJob(int jobId)
        {
            var data = new MessageModel<string>();

            var model = await _tasksQzService.QueryById(jobId);
            if (model != null)
            {
                var ResuleModel = await _schedulerCenter.StopScheduleJobAsync(model);
                if (ResuleModel.success)
                {
                    model.IsStart = (int)BoolStatus.False;
                    data.success = await _tasksQzService.Update(model);
                }
                data = ResuleModel;
            }
            return data;

        }

        /// <summary>
        /// 重启一个计划任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [HttpPost("ReCovery/{jobId}")]
        public async Task<MessageModel<string>> ReCovery(int jobId)
        {
            var data = new MessageModel<string>();

            var model = await _tasksQzService.QueryById(jobId);
            if (model != null)
            {
                var ResuleModel = await _schedulerCenter.ResumeJob(model);
                if (ResuleModel.success)
                {
                    model.IsStart = (int)BoolStatus.True;
                    data.success = await _tasksQzService.Update(model);
                }
                if (data.success)
                {
                    data.msg = "重启成功";
                    data.response = jobId.ObjToString();
                }
            }
            return data;

        }


    }
}