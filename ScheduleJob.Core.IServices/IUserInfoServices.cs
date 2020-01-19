using ScheduleJob.Core.Contract.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleJob.Core.IServices
{
    public interface IUserInfoServices : IBaseServices<UserInfo>
    {
        Task<UserInfo> SaveUserInfo(UserInfo sysUserInfo);
        Task<string> GetUserRoleNameStr(string loginName, string loginPwd);
    }
}
