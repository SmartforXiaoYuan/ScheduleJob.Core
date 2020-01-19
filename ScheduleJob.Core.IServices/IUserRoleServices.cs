using ScheduleJob.Core.Contract.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleJob.Core.IServices
{
    /// <summary>
    /// UserRoleServices
    /// </summary>	
    public interface IUserRoleServices : IBaseServices<UserRole>
    {

        Task<UserRole> SaveUserRole(int uid, int rid);
        Task<int> GetRoleIdByUid(int uid);
    }
}
