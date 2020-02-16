using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ScheduleJob.Core.AuthHelper.Policys;
using ScheduleJob.Core.IServices;

namespace ScheduleJob.Core.Controllers
{
    //[Route("api/[controller]/[action]")]
    [Route("api/Login")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        readonly IUserInfoServices _sysUserInfoServices;
        readonly IUserRoleServices _userRoleServices;
        readonly IRoleServices _roleServices;
        readonly PermissionRequirement _requirement;


        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="sysUserInfoServices"></param>
        /// <param name="userRoleServices"></param>
        /// <param name="roleServices"></param>
        /// <param name="requirement"></param>
        public LoginController(IUserInfoServices sysUserInfoServices, IUserRoleServices userRoleServices, IRoleServices roleServices, PermissionRequirement requirement)
        {
            this._sysUserInfoServices = sysUserInfoServices;
            this._userRoleServices = userRoleServices;
            this._roleServices = roleServices;
            _requirement = requirement;
        }



        [HttpGet]
        public string GetToken()
        {
            // 3 + 2
            SecurityToken securityToken = new JwtSecurityToken(
                issuer: "ProjectD.Core",
                audience: "audience",
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes("projectddddddddchangyidian")), SecurityAlgorithms.HmacSha256),
                expires: DateTime.Now.AddHours(1),
                claims: new Claim[] {
                    new Claim(ClaimTypes.Role,"User"),
                    new Claim(JwtRegisteredClaimNames.Jti,"yuan"),
                    new Claim(ClaimTypes.Email,"123@123.com"),
                    new Claim(JwtRegisteredClaimNames.Email,"123@123.com"),
                }
            );
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return jwtToken;
        }

        [HttpGet]
        [Route("JWTToken3.0")]
        public async Task<object> GetJwtToken3(string name = "", string pass = "")
        {
            string jwtStr = string.Empty;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pass))
            {
                return new JsonResult(new
                {
                    Status = false,
                    message = "用户名或密码不能为空"
                });
            }

            //pass = MD5Helper.MD5Encrypt32(pass);

            var user = await _sysUserInfoServices.Query(d => d.UserName == name && d.UserPWD == pass);
            if (user.Count > 0)
            {
                var userRoles = await _sysUserInfoServices.GetUserRoleNameStr(name, pass);
                //如果是基于用户的授权策略，这里要添加用户;如果是基于角色的授权策略，这里要添加角色
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, name),
                    new Claim(JwtRegisteredClaimNames.Jti, user.FirstOrDefault().Id.ToString()),
                    new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(_requirement.Expiration.TotalSeconds).ToString()) };
                claims.AddRange(userRoles.Split(',').Select(s => new Claim(ClaimTypes.Role, s)));

                //用户标识
                var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
                identity.AddClaims(claims);

                var token = JwtToken.BuildJwtToken(claims.ToArray(), _requirement);
                return new JsonResult(token);
            }
            else
            {
                return new JsonResult(new
                {
                    success = false,
                    message = "认证失败"
                });
            }



        }

    }
}