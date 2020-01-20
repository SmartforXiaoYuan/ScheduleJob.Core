using ScheduleJob.Core.Contract.Models;
using ScheduleJob.Core.Contract.VO;
using ScheduleJob.Core.IRepository;
using ScheduleJob.Core.IServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleJob.Core.Services
{
    public class MenuService : IMenuServices
    {
        IMenuRepositoty _menuRepositoty;
        public MenuService(IMenuRepositoty menuRepositoty)
        {
            _menuRepositoty = menuRepositoty;
        }

        public async Task<bool> AddMenu(MenuInfo sysMenu)
        {
            return await _menuRepositoty.Add(sysMenu) > 0;
        }

        public async Task<bool> DeleteMenu(List<MenuInfo> sysMenus)
        {
            throw new NotImplementedException();
        }

        public List<MenuInfo> GetAllListByWhere(List<int> Ids)
        {
            throw new NotImplementedException();
        }

        public async Task<MenuInfo> GetMenu(int menuId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MenuInfo>> GetMenuList()
        {
            List<MenuInfo> lst = await _menuRepositoty.Query();
            return lst;
        }

        public Task<RouterBar> RouterBar(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateMenu(MenuInfo sysMenu)
        {
            throw new NotImplementedException();
        }
    }
}
