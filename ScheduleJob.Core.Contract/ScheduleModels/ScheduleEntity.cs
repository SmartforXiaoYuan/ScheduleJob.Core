using System;
using System.Collections.Generic;
using System.Text;

namespace ScheduleJob.Core.Contract.ScheduleModels
{
    /// <summary>
    /// 任务计划表
    /// </summary>
    public class ScheduleEntity:RootEntity
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 任务分组
        /// </summary>
        public int JobGroupId { get; set; }
        /// <summary>
        /// 任务分组
        /// </summary>
        public string JobGroupName { get; set; }
        /// <summary>
        /// 任务运行时间表达式
        /// </summary>
        public string Cron { get; set; }

        /// <summary>
        /// url
        /// </summary>
        public string ApiUrl { get; set; }
        /// <summary>
        /// 任务描述
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 执行次数
        /// </summary>
        public int RunTimes { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginTime { get; set; } = Convert.ToDateTime("1900-01-01");
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; } = Convert.ToDateTime("1900-01-01");

        /// <summary>
        /// 最后执行时间
        /// </summary>
        public DateTime? LastRunTime { get; set; } = Convert.ToDateTime("1900-01-01");
        /// <summary>
        /// 是否启动
        /// </summary>
        public bool IsStart { get; set; } = false;
        /// <summary>
        /// 请求体(仅POST有效)
        /// </summary>
        public string RequestValue { get; set; }

        /// <summary>
        /// POST or GET
        /// </summary>
        public string MethodType { get; set; }

        /// <summary>
        /// 负责人工号(不填默认是你,多个负责人间用半角分号隔开，比如：123456;11134)
        /// </summary>
        public string HandlerJobNum { get; set; }

        /// <summary>
        /// 报警邮箱
        /// </summary>
        public string ErrorEmail { get; set; }
    }
 

}
