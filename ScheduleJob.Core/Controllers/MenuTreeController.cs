using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScheduleJob.Core.Contract.Response;
using ScheduleJob.Core.Contract.VO;
using ScheduleJob.Core.IServices;

namespace ScheduleJob.Core.Controllers
{
    /// <summary>
    /// 菜单树
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MenuTreeController : ControllerBase
    {
        
        IMenuServices _menuServices;
        public MenuTreeController(IMenuServices menuServices)
        {
            _menuServices = menuServices;
        }

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("TreeList")]
        //[AllowAnonymous]
        public async Task<BaseResponse<RouterBar>> GetMenusTreeList(int id)
        {
            var ss = await _menuServices.RouterBar(id);
            var data = new BaseResponse<RouterBar>();
            //data.success = true;
            //data.response = ss;
            //data.msg = "获取成功";
            //return data;
            return null;
        }
    }
}