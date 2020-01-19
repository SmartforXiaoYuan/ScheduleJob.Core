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
        /// 获取全部用户
        /// </summary>
        /// <param name="page"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        // GET: api/User
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
    }
}