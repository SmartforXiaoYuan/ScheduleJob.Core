using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScheduleJob.Core.AuthHelper;
using ScheduleJob.Core.Contract.Models;
using ScheduleJob.Core.Contract.Response;
using ScheduleJob.Core.IServices;
using ScheduleJob.Core.Utility;
using ScheduleJob.Core.Utility.HttpContextUser;

namespace ScheduleJob.Core.Controllers
{
    /// <summary>
    /// 菜单树
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MenuTreeController : ControllerBase
    {
        readonly IHttpContextAccessor _httpContext;
        IMenuServices _menuServices;
        readonly IUser _user;
        readonly IUserRoleServices _userRoleServices;

        IRoleModuleService _roleModuleServive;
        public MenuTreeController(IMenuServices menuServices, IHttpContextAccessor httpContext, IUser user, IUserRoleServices userRoleServices, IRoleModuleService roleModuleServive)
        {
            _menuServices = menuServices;
            _httpContext = httpContext;
            _user = user; this._userRoleServices = userRoleServices;
            _roleModuleServive = roleModuleServive;
        }
        /// <summary>
        /// 查询树形 Table
        /// </summary>
        /// <param name="f">父节点</param>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        [HttpGet]
        //[AllowAnonymous]
        public async Task<BaseResponse<List<MenuInfo>>> GetTreeTable(int f = 0, string key = "")
        {
            List<MenuInfo> permissions = new List<MenuInfo>();
            //var apiList = await _menuServices.Query(d => d.IsDrop == false);
            var menusList = await _menuServices.Query(d => d.IsDrop == false);
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
            {
                key = "";
            }

            if (key != "")
            {
                permissions = menusList.Where(a => a.Name.Contains(key)).OrderBy(a => a.OrderSort).ToList();
            }
            else
            {
                permissions = menusList.Where(a => a.ParentId == f).OrderBy(a => a.OrderSort).ToList();
            }
            foreach (var item in permissions)
            {
                List<int> pidarr = new List<int>
                {
                    item.ParentId
                };
                if (item.ParentId > 0)
                {
                    pidarr.Add(0);
                }
                var parent = menusList.FirstOrDefault(d => d.Id == item.ParentId);

                while (parent != null)
                {
                    pidarr.Add(parent.Id);
                    parent = menusList.FirstOrDefault(d => d.Id == parent.ParentId);
                }
                item.PidArr = pidarr.OrderBy(d => d).Distinct().ToList();
                item.hasChildren = menusList.Where(d => d.ParentId == item.Id).Any();
            }
            var data = new BaseResponse<List<MenuInfo>>();
            data.Data = permissions;
            return data;
        }
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[Route("GetMenusTreeList")]
        //[AllowAnonymous]
        public async Task<BaseResponse<MenuTree>> GetMenusTreeList(int pid = 0)
        {
            var data = new BaseResponse<MenuTree>();
            var permissions = await _menuServices.Query(d => d.IsDrop == false);
            var permissionTrees = (from child in permissions
                                   where child.IsDrop == false
                                   orderby child.Id
                                   select new MenuTree
                                   {
                                       value = child.Id,
                                       label = child.Name,
                                       Pid = child.ParentId,
                                       order = child.OrderSort,
                                   }).ToList();
            MenuTree rootRoot = new MenuTree
            {
                value = 0,
                Pid = 0,
                label = "根节点"
            };
            permissionTrees = permissionTrees.OrderBy(d => d.order).ToList();

            RecursionHelper.LoopToAppendChildren(permissionTrees, rootRoot, pid);
            data.Data = rootRoot;
            data.Msg = "获取成功";
            return data;

        }


        /// <summary>
        /// 获取路由树
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<BaseResponse<RouterBar>> GetNavigationBar(int uid)
        {
            var data = new BaseResponse<RouterBar>();
            try
            {
                #region MyRegion
                //RouterBar rootRoot = new RouterBar()
                //{
                //    id = 0,
                //    pid = 0,
                //    order = 0,
                //    name = "根节点",
                //    path = "",
                //    icon = "",
                //    //meta = new NavigationBarMeta(),
                //};
                //RouterBar lst1 = new RouterBar
                //{
                //    id = 1,
                //    APIAddress = "",
                //    icon = "fa-qq",
                //    leaf = true,
                //    path = "/",
                //    name = "首页",
                //    pid = 1
                //};
                //RouterBar lst1_1 = new RouterBar
                //{
                //    id = 1,
                //    APIAddress = "",
                //    leaf = true,
                //    path = "",
                //    name = "About",
                //    pid = 1
                //};
                //lst1.children.Add(lst1_1);
                //RouterBar lst2 = new RouterBar
                //{
                //    id = 1,
                //    APIAddress = "",
                //    icon = "fa-users",
                //    leaf = false,
                //    path = "/Home",
                //    name = "权限管理",
                //    pid = 1
                //};
                //RouterBar lst2_1 = new RouterBar
                //{
                //    id = 1,
                //    APIAddress = "",
                //    icon = "fa-qq",
                //    leaf = false,
                //    path = "/User/User",
                //    name = "用户管理",
                //    pid = 1
                //};
                //RouterBar lst2_2 = new RouterBar
                //{
                //    id = 1,
                //    APIAddress = "",
                //    icon = "fa-qq",
                //    leaf = false,
                //    path = "/User/Roles",
                //    name = "角色管理",
                //    pid = 1
                //};
                //RouterBar lst2_3 = new RouterBar
                //{
                //    id = 1,
                //    APIAddress = "",
                //    icon = "fa-qq",
                //    leaf = false,
                //    path = "/Menu/Menu",
                //    name = "菜单管理",
                //    pid = 1
                //};
                //lst2.children.Add(lst2_1);
                //lst2.children.Add(lst2_2);
                //lst2.children.Add(lst2_3);
                //rootRoot.children.Add(lst1);
                //rootRoot.children.Add(lst2);
                //data.Data = rootRoot;
                #endregion
                // 三种方式获取 uid
                var uidInHttpcontext1 = (from item in _httpContext.HttpContext.User.Claims
                                         where item.Type == "jti"
                                         select item.Value).FirstOrDefault();
                var uidInHttpcontext = (JwtHelper.SerializeJwt(_httpContext.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "")))?.Uid;
                var uName = _user.Name;
                if (uid > 0 && uid == uidInHttpcontext)
                {
                    var roleId = ((await _userRoleServices.Query(d => d.UserId == uid)).FirstOrDefault()?.RoleId);
                    if (roleId > 0)
                    {
                        var menus = await _menuServices.Query(d => d.IsDrop == false);
                        var menusTrees = (from item in menus
                                          where item.IsDrop == false
                                          orderby item.Id
                                          select new RouterBar
                                          {
                                              id = item.Id,
                                              APIAddress = item.ApiUrl,
                                              icon = item.Icon,
                                              leaf = item.RoutePath == "-" ? false : true,
                                              path = item.RoutePath,
                                              name = item.Name,
                                              pid = item.ParentId,
                                              order = item.OrderSort,
                                          }).ToList();

                        RouterBar rootRoot = new RouterBar()
                        {
                            id = 0,
                            pid = 0,
                            order = 0,
                            name = "根节点",
                            path = "",
                            icon = ""
                        };
                        menusTrees = menusTrees.OrderBy(d => d.order).ToList();

                        RecursionHelper.LoopNaviBarAppendChildren(menusTrees, rootRoot);
                        data.Data = rootRoot;
                        data.Msg = "获取成功";
                    }
                }
            }
            catch (Exception ex)
            {


            }
            return data;
        }


        [HttpGet]
        public async Task<BaseResponse<AssignShow>> GetPermissionIdByRoleId(int rid = 0)
        {
            var data = new BaseResponse<AssignShow>();
            //var rms = await _roleModuleServive.Query(d => d.IsDrop == false && d.RoleId == rid);
            //var permissions = await _roleModuleServive.Query(d => d.IsDrop == false);
            //List<string> assignbtns = new List<string>();

            //foreach (var item in permissionTrees)
            //{
            //    var pername = permissions.FirstOrDefault(d => d.IsButton && d.Id == item)?.Name;
            //    if (!string.IsNullOrEmpty(pername))
            //    {
            //        assignbtns.Add(pername + "_" + item);
            //    }
            //}
            //data.Data = new AssignShow()
            //{
            //    permissionids = permissionTrees,
            //    assignbtns = assignbtns,
            //};
            //data.Msg = "获取成功";
            return data;
        }
        /// <summary>
        /// 保存菜单权限分配
        /// </summary>
        /// <param name="assignView"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BaseResponse<string>> Assign([FromBody] AssignView assignView)
        {
            var data = new BaseResponse<string>();
            try
            {
                if (assignView.rid > 0)
                {
                    var roleModulePermissions = await _roleModuleServive.Query(d => d.RoleId == assignView.rid);
                    var remove = roleModulePermissions.Where(d => !assignView.pids.Contains(d.MenuId)).Select(c => (object)c.Id);
                    await _roleModuleServive.DeleteByIds(remove.ToArray());//删除
                    foreach (var item in assignView.pids)
                    {
                        var rmpitem = roleModulePermissions.Where(d => d.MenuId == item);
                        if (!rmpitem.Any())
                        {

                            RoleModulePermission roleModulePermission = new RoleModulePermission()
                            {
                                IsDrop = false,
                                RoleId = assignView.rid,
                                MenuId = item,
                            };
                            roleModulePermission.CreatedId = _user.ID;
                            roleModulePermission.CreatedName = _user.Name;
                            roleModulePermission.UpdateId = _user.ID;
                            roleModulePermission.UpdateName = _user.Name;

                            var res = (await _roleModuleServive.Add(roleModulePermission)) > 0;

                        }
                    }
                    data.Msg = "保存成功";
                }
            }
            catch (Exception ex)
            {
                data.Code = 301;
                data.Msg = ex.ToString();
            }

            return data;
        }
    }

    public class AssignView
    {
        public List<int> pids { get; set; }
        public int rid { get; set; }
    }
    public class AssignShow
    {
        public List<int> permissionids { get; set; }
        public List<string> assignbtns { get; set; }
    }
}