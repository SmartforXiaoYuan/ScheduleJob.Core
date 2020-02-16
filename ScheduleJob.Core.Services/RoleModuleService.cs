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
    public class RoleModuleService : BaseService<RoleModulePermission>, IRoleModuleService
    {
        readonly IRoleModuleRepository _dal;
        readonly IMenuRepositoty _menuRepositoty;
        readonly IRoleRepository _roleRepository;

        // 将多个仓储接口注入
        public RoleModuleService(IRoleModuleRepository dal, IMenuRepositoty menuRepositoty, IRoleRepository roleRepository)
        {
            this._dal = dal;
            this._menuRepositoty = menuRepositoty;
            this._roleRepository = roleRepository;
            base.BaseDal = dal;
        }

        public async Task<List<RoleModulePermission>> GetRoleModule()
        {
            var roleModulePermissions = await base.Query(a => a.IsDrop == false);
            var roles = await _roleRepository.Query(a => a.IsDrop == false);
            var menus = await _menuRepositoty.Query(a => a.IsDrop == false);

            if (roleModulePermissions.Count > 0)
            {
                foreach (var item in roleModulePermissions)
                {
                    item.Role = roles.FirstOrDefault(d => d.Id == item.RoleId);
                    item.MenuInfo = menus.FirstOrDefault(d => d.Id == item.MenuId);
                }

            }
            return roleModulePermissions;
        }

        public Task<List<RoleModulePermission>> TestModelWithChildren()
        {
            throw new NotImplementedException();
        }
    }
}
