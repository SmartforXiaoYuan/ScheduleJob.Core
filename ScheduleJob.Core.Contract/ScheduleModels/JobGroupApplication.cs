using System;
using System.Collections.Generic;
using System.Text;

namespace ScheduleJob.Core.Contract.ScheduleModels
{
   public  class JobGroupApplication : RootEntity
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        public string GroupName { get; set; }

        ///// <summary>
        ///// 权限控制
        ///// </summary>
        //public string Role { get; set; }
    }
}
