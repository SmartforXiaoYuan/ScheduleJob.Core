using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScheduleJob.Core.Utility
{
    /// <summary>
    /// 泛型递归求树形结构
    /// </summary>
    public static class RecursionHelper
    {
        public static void LoopToAppendChildren(List<MenuTree> all, MenuTree curItem, int pid)
        {
            var subItems = all.Where(ee => ee.Pid ==Convert.ToInt32(curItem.value)).ToList();
            if (subItems.Count > 0)
            {
                curItem.children = new List<MenuTree>();
                curItem.children.AddRange(subItems);
            }
            else
            {
                curItem.children = null;
            }
            foreach (var subItem in subItems)
            {
                if (Convert.ToInt32(subItem.value) == pid && pid > 0)
                {
                   
                }
                LoopToAppendChildren(all, subItem, pid);
            }
        }
       

        public static void LoopToAppendChildrenT<T>(List<T> all, T curItem, string parentIdName = "Pid", string idName = "value", string childrenName = "children")
        {
            var subItems = all.Where(ee => ee.GetType().GetProperty(parentIdName).GetValue(ee, null).ToString() == curItem.GetType().GetProperty(idName).GetValue(curItem, null).ToString()).ToList();

            if (subItems.Count > 0) curItem.GetType().GetField(childrenName).SetValue(curItem, subItems);
            foreach (var subItem in subItems)
            {
                LoopToAppendChildrenT(all, subItem);
            }
        }

        public static void LoopNaviBarAppendChildren(List<RouterBar> all, RouterBar curItem)
        {
            var subItems = all.Where(ee => ee.pid == curItem.id).ToList();
            if (subItems.Count > 0)
            {
                curItem.children = new List<RouterBar>();
                curItem.children.AddRange(subItems);
            }
            else
            {
                curItem.children = null;
            }
            foreach (var subItem in subItems)
            {
                LoopNaviBarAppendChildren(all, subItem);
            }
        }
    }
    public class MenuTree
    {
        /// <summary>
        /// 前端用
        /// </summary>
        public int value { get; set; }
        public int Pid { get; set; }
        /// <summary>
        /// 前端控件用（name）
        /// </summary>
        public string label { get; set; }
        public int order { get; set; }
        public bool disabled { get; set; }
        public List<MenuTree> children { get; set; } = new List<MenuTree>();

        /// <summary>
        /// input
        /// </summary>
        public string text { get; set; }
    }
    public class RouterBar
    {
 

        public int id { get; set; }
        public int pid { get; set; }
        public int order { get; set; }
        public string name { get; set; }
        public bool IsHide { get; set; } = false;
        public bool leaf { get; set; } = false;

        public string Func { get; set; }
        //图标名称
        public string icon { get; set; }
        /// <summary>
        /// 前端路由地址
        /// </summary>
        public string path { get; set; }

        /// <summary>
        /// API路由地址
        /// </summary>
        public string APIAddress { get; set; }

        public List<RouterBar> children { get; set; } = new List<RouterBar>();

        public NavigationBarMeta meta { get; set; } = new NavigationBarMeta();

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
