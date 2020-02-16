using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScheduleJob.Core.Contract;
using ScheduleJob.Core.Contract.Models;
using ScheduleJob.Core.Contract.Response;
using ScheduleJob.Core.IServices;

namespace ScheduleJob.Core.Controllers
{
    /// <summary>
    /// 角色
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        readonly IRoleServices _roleServices;
        IRoleModuleService _roleModuleServive;
        public RoleController(IRoleServices roleServices, IRoleModuleService roleModuleServive)
        {
            _roleServices = roleServices;
            //_user = user;
            _roleModuleServive = roleModuleServive;
        }
        /// <summary>
        /// 获取全部角色
        /// </summary>
        /// <param name="page"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        // GET: api/User
        [HttpGet]
        public async Task<BaseResponse<PageModel<Role>>> Get(int page = 1, string key = "")
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
            {
                key = "";
            }
            int intPageSize = 50;
            var data = await _roleServices.QueryPage(a => a.IsDrop == false && (a.Name != null && a.Name.Contains(key)), page, intPageSize, " Id desc ");
            data.Models.ForEach(x =>
            {
                x.MenuIds = _roleModuleServive.Query(d => d.RoleId == x.Id).Result.Select(a => a.MenuId).ToList();
            });
            return new BaseResponse<PageModel<Role>>()
            {
                Data = data
            };
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        // POST: api/User
        [HttpPost]
        public async Task<BaseResponse<string>> Post(Role role)
        {
            var data = new BaseResponse<string>();
            //role.CreateId = _user.ID;
            //role.CreateBy = _user.Name;
            //role.CreateBy = "yyj";
            //role.DataFlag = 1;
            //role.Name = "admin";
            //role.Description = "admin的权限";
            //role.CreateId = 0;
            //role.CreateTime = DateTime.Now;
            role.UpdateName = role.CreatedName;
            role.UpdateId = role.CreatedId;
            var id = await _roleServices.Add(role);
            if (id > 0)
            {
                data.Msg = "添加成功";
            }
            return data;
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        // PUT: api/User/5
        [HttpPut]
        public async Task<BaseResponse<string>> Put(Role role)
        {
            var data = new BaseResponse<string>();
            if (role != null && role.Id > 0)
            {
                var res = await _roleServices.Update(role);
                if (res)
                {
                    data.Msg = "更新成功";
                }
            }
            return data;
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/ApiWithActions/5
        [HttpDelete]
        public async Task<BaseResponse<string>> Delete(int id)
        {
            var data = new BaseResponse<string>();
            if (id > 0)
            {
                var userDetail = await _roleServices.QueryById(id);
                userDetail.IsDrop = false;
                var res = await _roleServices.Update(userDetail);
                if (res)
                {
                    data.Msg = "删除成功";
                }
            }
            return data;
        }

    }
}