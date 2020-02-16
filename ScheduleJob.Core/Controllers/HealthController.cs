using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ScheduleJob.Core.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HealthController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;

        public HealthController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// 心跳检查
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            var feature = HttpContext.Features.Get<IHttpConnectionFeature>();
            var clientIP = HttpContext.Request.Headers["X-Forwarded-For"];
            var remoteIP = feature.RemoteIpAddress;
            var serverIP = feature.LocalIpAddress;
            _logger.LogError("HealthCheck Controller info Log");
            return Content($@"客户端IP:{clientIP}<br>通过：{remoteIP}<br>服务器IP:{serverIP}<br>{DateTime.Now.Date:yyy-MM-dd}<br><a href='/'>Home</a><br/>User.Identity.IsAuthenticated:{HttpContext.User.Identity.IsAuthenticated}", "text/html; charset=utf-8");

        }

        //[HttpGet]
        //public IActionResult AppOffline()
        //{
        //    var feature = HttpContext.Features.Get<IHttpConnectionFeature>();
        //    var clientIP = HttpContext.Request.Headers["X-Forwarded-For"];
        //    var remoteIP = feature.RemoteIpAddress;
        //    var serverIP = feature.LocalIpAddress;
        //}
    }
}