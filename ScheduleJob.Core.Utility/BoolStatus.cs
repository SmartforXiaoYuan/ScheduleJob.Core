using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ScheduleJob.Core.Utility
{
    /// <summary>
    /// 是否删除
    /// </summary>
    public enum IsDeleted : int
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Normal = 0,
        /// <summary>
        /// 删除
        /// </summary>
        [Description("已删除")]
        Deleted
    }


    /// <summary>
    /// 布尔值状态
    /// </summary>
    public enum BoolStatus : int
    {
        /// <summary>
        /// 假(0)
        /// </summary>
        False = 0,
        /// <summary>
        /// 真(1)
        /// </summary>
        True = 1,
    }
}
