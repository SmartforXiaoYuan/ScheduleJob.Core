using ScheduleJob.Core.Contract.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleJob.Core.IServices
{
    /// <summary>
    /// RoleServices
    /// </summary>	
    public interface IRoleServices : IBaseServices<Role>
    {
        Task<Role> SaveRole(Role role);
        //Task<string> GetRoleNameByRid(int rid);

    }
}
