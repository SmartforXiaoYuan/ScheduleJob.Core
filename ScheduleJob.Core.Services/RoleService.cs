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
    /// RoleService
    /// </summary>	
    public class RoleService : BaseService<Role>, IRoleServices
    {

        IRoleRepository _dal;
        public RoleService(IRoleRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<Role> SaveRole(Role roleEntity)
        {
            Role model = new Role();
            var userList = await base.Query(a => a.Name == roleEntity.Name && a.IsDrop == false);
            if (userList.Count > 0)
            {
                model = userList.FirstOrDefault();
            }
            else
            {
                var id = await base.Add(roleEntity);
                model = await base.QueryById(id);
            }

            return model;

        }
    }
}
