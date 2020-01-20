using ScheduleJob.Core.Contract.Models;
using ScheduleJob.Core.Contract.VO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleJob.Core.IServices
{
    /// <summary>
    /// 菜单管理服务层接口定义
    /// </summary>
    public interface IMenuServices
    {
        /// <summary>
        /// 获取菜单列表，非树形
        /// </summary>
        /// <returns></returns>
        Task<List<MenuInfo>> GetMenuList();
        /// <summary>
        /// 根据ID获取一个菜单的对象
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        Task<MenuInfo> GetMenu(int menuId);
        /// <summary>
        /// 获取树形菜单，渲染左侧菜单使用
        /// </summary>
        /// <returns></returns>
        Task<RouterBar> RouterBar(int userId);
        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="sysMenu"></param>
        Task<bool> AddMenu(MenuInfo sysMenu);
        /// <summary>
        /// 根据表达式获取指定的数据
        /// </summary>
        /// <returns></returns>
        List<MenuInfo> GetAllListByWhere(System.Collections.Generic.List<int> Ids);
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="sysMenus"></param>
        /// <returns></returns>
        Task<bool> DeleteMenu(System.Collections.Generic.List<MenuInfo> sysMenus);
        
        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="sysMenu"></param>
        Task<bool> UpdateMenu(MenuInfo sysMenu);
    }
}
