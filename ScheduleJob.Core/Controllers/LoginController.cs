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

namespace ScheduleJob.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {




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


    }
}