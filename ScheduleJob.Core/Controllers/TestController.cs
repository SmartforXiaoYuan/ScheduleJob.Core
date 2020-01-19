using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using Quartz.Impl;

namespace ScheduleJob.Core.Controllers
{
    //三大核心对象
    //调度器StdSchedulerFactory、IScheduler:单元/实例，完成定时任务的配置
    //IJob：任务
    //触发器tigger：定时策略



    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        //private readonly ISchedulerFactory _schedulerFactory;
        public IScheduler _scheduler;



        [HttpGet]
        public async Task<string[]> Get()
        {
            //创建一个工厂
            NameValueCollection param = new NameValueCollection()
            {
                {  "testJob","test"}
            };

            StdSchedulerFactory factory = new StdSchedulerFactory(param);

            ISchedulerFactory _schedulerFactory = factory;

            //1、通过调度工厂获得调度器
            _scheduler = await _schedulerFactory.GetScheduler();
            //2、开启调度器
            await _scheduler.Start();
            //3、创建一个触发器
            var trigger = TriggerBuilder.Create()
                            .WithSimpleSchedule(x => x.WithIntervalInSeconds(2).RepeatForever())//每两秒执行一次
                            .Build();
            //4、创建任务
            var jobDetail = JobBuilder.Create<MyJob>()
                            .WithIdentity("job", "group")
                            .Build();
            //5、将触发器和任务器绑定到调度器中
            await _scheduler.ScheduleJob(jobDetail, trigger);
            return await Task.FromResult(new string[] { "value1", "value2" });
        }




        private IScheduler scheduler;
        /// <summary>
        /// 创建调度任务的入口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task Start()
        {
            await StartJob();
            //StartJob().GetAwaiter().GetResult();
        }
        /// <summary>
        /// 创建调度任务的公共调用中心
        /// </summary>
        /// <returns></returns>
        public async Task StartJob()
        {
            //创建一个工厂
            //NameValueCollection param = new NameValueCollection()
            //{
            //    {  "testJob","test"}
            //};
            //StdSchedulerFactory factory = new StdSchedulerFactory(param);
            StdSchedulerFactory factory = new StdSchedulerFactory();
            //创建一个调度器
            scheduler = await factory.GetScheduler();
            CacheStatic.Add("1", scheduler);
            //开始调度器
            await scheduler.Start();

            //每三秒打印一个info日志
            await CreateJob<StartLogInfoJob>("_StartLogInfoJob", "_StartLogInfoJob", " 0/3 * * * * ? ");

            //每五秒打印一个debug日志
            await CreateJob<StartLogDebugJob>("_StartLogDebugJob", "_StartLogDebugJob", " 0/5 * * * * ? ");

            //调度器时间生成地址--        http://cron.qqe2.com

        }

        /// <summary>
        /// 停止调度器            
        /// </summary>
        public void Stop()
        {
            var sc=  CacheStatic.cache["1"];
            sc.Shutdown();
  
        }

        /// <summary>
        /// 创建运行的调度器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="group"></param>
        /// <param name="cronTime"></param>
        /// <returns></returns>
        public async Task CreateJob<T>(string name, string group, string cronTime) where T : IJob
        {
            //创建一个作业
            var job = JobBuilder.Create<T>()
                .WithIdentity("name" + name, "gtoup" + group)
                .WithDescription("测试的描述")
                .Build();

            //创建一个触发器
            var tigger = (ICronTrigger)TriggerBuilder.Create()
                .WithIdentity("name" + name, "group" + group)
                .StartNow()
                .WithCronSchedule(cronTime)
                .Build();

            //把作业和触发器放入调度器中
            await scheduler.ScheduleJob(job, tigger);
        }
    }
    public class StartLogInfoJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await Start();
        }
        public async Task Start()
        {
            Console.WriteLine($"{DateTime.Now.ToString()}：调度打印Debug");
        }
    }


    public class StartLogDebugJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await Start();
        }
        public async Task Start()
        {
            Console.WriteLine($"{DateTime.Now.ToString()}：调度打印Info");
        }
    }
    public class MyJob : IJob//创建IJob的实现类，并实现Excute方法。
    {
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                Console.WriteLine($"{DateTime.Now.ToString()}：开始执行同步第三方数据");
            });
        }
    }

    public class CacheStatic
    {
        public  static Dictionary<string, IScheduler> cache = new Dictionary<string, IScheduler>();

        public static void  Add(string key, IScheduler sch)
        {
            cache.Add(key, sch);
        }
    }
}