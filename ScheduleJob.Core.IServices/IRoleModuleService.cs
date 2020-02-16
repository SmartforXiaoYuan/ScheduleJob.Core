using ScheduleJob.Core.Contract.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleJob.Core.IServices
{
   public interface IRoleModuleService : IBaseServices<RoleModulePermission>
    {
        Task<List<RoleModulePermission>> GetRoleModule();
        Task<List<RoleModulePermission>> TestModelWithChildren();
    }
}
