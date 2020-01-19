using ScheduleJob.Core.Contract.Models;
using ScheduleJob.Core.IRepository;
using ScheduleJob.Core.IServices;
using ScheduleJob.Core.Services.BASE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleJob.Core.Services
{
    /// <summary>
    /// sysUserInfoServices
    /// </summary>	
    public class UserInfoService : BaseService<UserInfo>, IUserInfoServices
    {
        //[FromServiceContext]
        //private UserInfoRepository _dal { get; set; }

        IUserInfoRepository _dal { get; set; }
        IRoleRepository _roleRepository;
        IUserRoleServices _userRoleServices;
        public UserInfoService(IUserInfoRepository dal, IRoleRepository roleRepository, IUserRoleServices userRoleServices)
        {
            this._dal = dal;
            base.BaseDal = dal;
            this._roleRepository = roleRepository;
            _userRoleServices = userRoleServices;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="loginPwd"></param>
        /// <returns></returns>
        public async Task<UserInfo> SaveUserInfo(UserInfo sysUserInfo)
        {
            UserInfo model = new UserInfo();
            var userList = await base.Query(a => a.UserName == sysUserInfo.UserName && a.UserPWD == sysUserInfo.UserPWD);
            if (userList.Count > 0)
            {
                model = userList.FirstOrDefault();
            }
            else
            {
                sysUserInfo.DataFlag = 1;
                var id = await base.Add(sysUserInfo);
                model = await base.QueryById(id);
            }
            return model;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="loginPwd"></param>
        /// <returns></returns>
        public async Task<string> GetUserRoleNameStr(string loginName, string loginPwd)
        {
            string roleName = "";
            var user = (await base.Query(a => a.UserName == loginName && a.UserPWD == loginPwd)).FirstOrDefault();
            var roleList = await _roleRepository.Query(a => a.IsDrop == false);
            if (user != null)
            {
                var userRoles = await _userRoleServices.Query(ur => ur.UserId == user.Id);
                if (userRoles.Count > 0)
                {
                    var arr = userRoles.Select(ur => ur.RoleId.ToString()).ToList();
                    var roles = roleList.Where(d => arr.Contains(d.Id.ToString()));

                    roleName = string.Join(',', roles.Select(r => r.Name).ToArray());
                }
            }
            return roleName;
        }

    }
}
