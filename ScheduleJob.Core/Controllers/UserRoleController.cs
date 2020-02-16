using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScheduleJob.Core.IServices;

namespace ScheduleJob.Core.Controllers
{
    /// <summary>
    /// 用户角色
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        readonly IUserInfoServices _sysUserInfoServices;
        readonly IUserRoleServices _userRoleServices;
        readonly IRoleServices _roleServices;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sysUserInfoServices"></param>
        /// <param name="userRoleServices"></param>
        /// <param name="roleServices"></param>
        public UserRoleController(IUserInfoServices sysUserInfoServices, IUserRoleServices userRoleServices, IRoleServices roleServices)
        {
            this._sysUserInfoServices = sysUserInfoServices;
            this._userRoleServices = userRoleServices;
            this._roleServices = roleServices;
        }

    }
}