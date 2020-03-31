using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScheduleJob.Core.AOP
{
    //PM> Install-Package Serilog
    //PM> Install-Package Serilog.AspNetCore
    //PM> Install-Package Serilog.Sinks.Console
    //PM> Install-Package Serilog.Sinks.File
    //https://www.jianshu.com/p/16d3f58dcc8a
    /// <summary>
    /// Castle代理
    /// 拦截器LogAOP 继承IInterceptor接口
    /// </summary>
    public class SeriLogAOP : IInterceptor
    {
        //readonly ILogger log = LogManager.GetLogger(typeof(Program));
        public void Intercept(IInvocation invocation)
        {
            throw new NotImplementedException();
        }
    }
}
