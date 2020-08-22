using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ScheduleJob.Core.Contract.Dtos
{
    public class TasksQzInputDto
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        [Required(ErrorMessage = "不能为空")]
        [MaxLength(100, ErrorMessage = "最大长度为100个字符")]
        public string Name { get; set; }
        /// <summary>
        /// 任务分组
        /// </summary>
        [Required(ErrorMessage = "不能为空")]
        public string JobGroup { get; set; }
        /// <summary>
        /// 任务运行时间表达式
        /// </summary>
        public string Cron { get; set; }
        /// <summary>
        /// 任务所在DLL对应的程序集名称
        /// </summary>
        public string AssemblyName { get; set; }
        /// <summary>
        /// 任务所在类
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// 任务描述
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 触发器类型（0、simple 1、cron）
        /// </summary>
        public int TriggerType { get; set; }
        /// <summary>
        /// 执行间隔时间, 秒为单位
        /// </summary>
        public int IntervalSecond { get; set; }
        /// <summary>
        /// 执行传参
        /// </summary>
        public string JobParams { get; set; }

        public int? IsDeleted { get; set; }

        #region HTTP

        /// <summary>
        /// （0、否 1、是）
        /// </summary>
        public int IsApiUrl { get; set; }
        /// <summary>
        /// url
        /// </summary>
        public string ApiUrl { get; set; }

        /// <summary>
        /// 请求体(仅POST有效)
        /// </summary>
        public string RequestValue { get; set; }

        /// <summary>
        /// POST or GET
        /// </summary>
        public int MethodType { get; set; }

        /// <summary>
        /// 负责人工号(不填默认是你,多个负责人间用半角分号隔开，比如：123456;11134)
        /// </summary>
        public string HandlerJobNum { get; set; }

        /// <summary>
        /// 报警邮箱
        /// </summary>
        public string ErrorEmail { get; set; }

        #endregion
    }
}
