using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScheduleJob.Core.AuthHelper;
using ScheduleJob.Core.Contract;
using ScheduleJob.Core.Contract.Models;
using ScheduleJob.Core.Contract.Response;
using ScheduleJob.Core.Extensions;
using ScheduleJob.Core.IRepository.UnitOfWork;
using ScheduleJob.Core.IServices;

namespace ScheduleJob.Core.Controllers
{

    /// <summary>
    /// 用户管理
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize(Permissions.Name)]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        readonly IUserInfoServices _sysUserInfoServices;
        readonly IUserRoleServices _userRoleServices;
        readonly IRoleServices _roleServices;
        //private readonly IUser _user;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="sysUserInfoServices"></param>
        /// <param name="userRoleServices"></param>
        /// <param name="roleServices"></param>
        public UserController(IUnitOfWork unitOfWork, IUserInfoServices sysUserInfoServices, IUserRoleServices userRoleServices, IRoleServices roleServices)
        {
            _unitOfWork = unitOfWork;
            _sysUserInfoServices = sysUserInfoServices;
            _userRoleServices = userRoleServices;
            _roleServices = roleServices;
        }
        /// <summary>
        /// 获取用户详情根据token
        /// 【无权限】
        /// </summary>
        /// <param name="token">令牌</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<BaseResponse<UserInfo>> GetInfoByToken(string token)
        {
            var data = new BaseResponse<UserInfo>();
            if (!string.IsNullOrEmpty(token))
            {
                var tokenModel = JwtHelper.SerializeJwt(token);
                if (tokenModel != null && tokenModel.Uid > 0)
                {
                    var userinfo = await _sysUserInfoServices.QueryById(tokenModel.Uid);
                    if (userinfo != null)
                    {
                        data.Data = userinfo;

                        data.Msg = "获取成功";
                    }
                }

            }
            return data;
        }

        /// <summary>
        /// 获取全部用户
        /// </summary>
        /// <param name="page"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        // GET: api/User
        [HttpGet]
        [Authorize(Permissions.Name)]
        public async Task<BaseResponse<PageModel<UserInfo>>> Get(int page = 1, string key = "")
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
            {
                key = "";
            }
            int intPageSize = 50;
            var data = await _sysUserInfoServices.QueryPage(a => a.DataFlag == 1 && ((a.UserName != null && a.UserName.Contains(key)) || (a.NickName != null && a.NickName.Contains(key))), page, intPageSize, " Id desc ");
            #region MyRegion
            var allUserRoles = await _userRoleServices.Query();
            var allRoles = await _roleServices.Query(d => d.IsDrop == false);
            var sysUserInfos = data.Models;
            foreach (var item in sysUserInfos)
            {
                var currentUserRoles = allUserRoles.Where(d => d.UserId == item.Id)?.Select(d => d.RoleId).ToList();
                item.RIDs = currentUserRoles;
                item.RoleNames = allRoles.Where(d => currentUserRoles.Contains((int)d.Id))?.Select(d => d.Name).ToList();
            }
            data.Models = sysUserInfos;
            #endregion

            return new BaseResponse<PageModel<UserInfo>>()
            {
                Data = data
            };

        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public string Get(string id)
        {
            return "value";
        }



        /// <summary>
        /// 添加一个用户
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        // POST: api/User
        [HttpPost]
        [AllowAnonymous]
        public async Task<BaseResponse<string>> Post(UserInfo userInfo)
        {
            var data = new BaseResponse<string>();
            //sysUserInfo.UserPWD = MD5Helper.MD5Encrypt32(sysUserInfo.uLoginPWD);
            userInfo.DataFlag = 1;
            userInfo.Status = 1;
            var id = await _sysUserInfoServices.Add(userInfo);
            if (id > 0)
            {
                data.Msg = "添加成功";
            }

            return data;
        }

        /// <summary>
        /// 更新用户与角色
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        // PUT: api/User/5
        [HttpPut]
        public async Task<BaseResponse<string>> Put([FromBody] UserInfo userInfo)
        {
            // 这里使用事务处理

            var data = new BaseResponse<string>();
            try
            {
                if (userInfo != null && userInfo.Id > 0)
                {
                    if (userInfo.RIDs.Count > 0)
                    {
                        _unitOfWork.BeginTran();
                        // 无论 Update Or Add , 先删除当前用户的全部 U_R 关系
                        var usreroles = (await _userRoleServices.Query(d => d.UserId == userInfo.Id)).Select(d => d.RoleId.ToString()).ToArray();
                        if (usreroles.Count() > 0)
                        {
                            var isAllDeleted = await _userRoleServices.DeleteByIds(usreroles);
                        }

                        // 然后再执行添加操作
                        var userRolsAdd = new List<UserRole>();
                        userInfo.RIDs.ForEach(async rid =>
                        {
                            await _userRoleServices.Add(new UserRole((int)userInfo.Id, rid)); ;
                        });
                    }
                    var success = await _sysUserInfoServices.Update(userInfo);

                    _unitOfWork.CommitTran();

                    if (success)
                    {
                        data.Msg = "更新成功";
                    }
                }
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTran();
                //_logger.LogError(ex, ex.Message);
            }
            return data;
        }

        /// <summary>
        /// 删除用户
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
                var userDetail = await _sysUserInfoServices.QueryById(id);
                userDetail.DataFlag = 0;
                var res = await _sysUserInfoServices.Update(userDetail);
                if (res)
                {
                    data.Msg = "删除成功";
                }
            }

            return data;
        }
    }
}