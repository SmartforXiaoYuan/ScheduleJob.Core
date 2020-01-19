using System;
using System.Collections.Generic;
using System.Text;

namespace ScheduleJob.Core.Contract.Response
{
    public class JobResuleModel
    {
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// 返回消息
        /// </summary>
        public string Message { get; set; }
    }
}
