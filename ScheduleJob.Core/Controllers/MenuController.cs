using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScheduleJob.Core.Contract;
using ScheduleJob.Core.Contract.Models;
using ScheduleJob.Core.Contract.Response;
using ScheduleJob.Core.IServices;

namespace ScheduleJob.Core.Controllers
{
    /// <summary>
    /// 菜单管理
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        IMenuServices _menuServices;
        public MenuController(IMenuServices menuServices)
        {
            _menuServices = menuServices;
        }

        /// <summary>
        /// 获取全部菜单
        /// </summary>
        /// <returns></returns>
        // GET: api/User
        [HttpGet]
        public async Task<BaseResponse<List<MenuInfo>>> Get()
        {
            BaseResponse<List<MenuInfo>> response = new BaseResponse<List<MenuInfo>>();
            var data =  await  _menuServices.GetMenuList();
            response.Data = data;
            return response;
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public string Get(string id)
        {
            return "value";
        }



        /// <summary>
        /// 添加一个菜单
        /// </summary>
        /// <param name="menuInfo"></param>
        /// <returns></returns>
        // POST: api/User
        [HttpPut]
        public async Task<BaseResponse<string>> Post(MenuInfo menuInfo)
        {
            var data = new BaseResponse<string>();
            var res= await _menuServices.AddMenu(menuInfo);
            if (res)
            {
                data.Msg = "添加成功";
            }

            return data;
        }

    }
}