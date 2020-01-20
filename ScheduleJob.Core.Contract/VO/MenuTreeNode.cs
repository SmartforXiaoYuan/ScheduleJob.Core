using System;
using System.Collections.Generic;
using System.Text;

namespace ScheduleJob.Core.Contract.VO
{
    public class RouterBar
    {
        public int id { get; set; }
        public int pid { get; set; }
        public int order { get; set; }
        public string name { get; set; }
        public bool IsHide { get; set; } = false;
        public bool IsButton { get; set; } = false;

        public string Func { get; set; }
        //图标名称
        public string iconCls { get; set; }
        /// <summary>
        /// 前端路由地址
        /// </summary>
        public string path { get; set; }

        /// <summary>
        /// API路由地址
        /// </summary>
        public string APIAddress { get; set; }
        public NavigationBarMeta meta { get; set; }
        public List<RouterBar> children { get; set; }



    }

    /// <summary>
    /// 定义一个Vue路由属性类
    /// </summary>
    public class NavigationBarMeta
    {
        public string title { get; set; }
        public bool requireAuth { get; set; } = true;
    }
}
