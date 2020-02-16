using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ScheduleJob.Core.Contract.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScheduleJob.Core.Middlewares
{
    /// <summary>
    /// 错误处理中间件
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        /// <summary>
        /// 错误处理的构造函数
        /// </summary>
        /// <param name="next"></param>
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            DateTime beginTime = DateTime.Now;
            Exception exception = null;
            try
            {
                var now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                await next(context);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                DateTime endTime = DateTime.Now;
                TimeSpan ts = endTime - beginTime;
                if (exception != null)
                {
                    //发生异常的时候，执行异常处理
                    await HandleExceptionAsync(context, exception, (long)ts.TotalMilliseconds);
                }
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex, long times)
        {
            var uuidN = Guid.NewGuid().ToString("N");  //错误唯一ID，用于排查问题
            BaseResponse res = new BaseResponse
            {
                Code = 500,
                Msg = "服务器正忙，请稍后重试!",
            };
            res.SetResponseStatus(false);
            //拼装服务器信息
            var connInfo = context.Connection;
            var serInfo = $",time:{times}ms";
            serInfo += $",ip:{connInfo.LocalIpAddress}:{connInfo.LocalPort}";
            res.ServerInfo = serInfo.TrimStart(',');
            if (ex != null)
            {
                res.Msg = $"{ex.Message}[errorid={uuidN}]";
            }
            else
            {
                res.Msg = "未知异常，请联系管理员!";
            }
            var result = JsonConvert.SerializeObject(res);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200;//这里的http status=200前端会认为接口是通的
            return context.Response.WriteAsync(result);
        }
    }
}
