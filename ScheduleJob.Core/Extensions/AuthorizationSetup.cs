using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ScheduleJob.Core.AuthHelper;
using ScheduleJob.Core.AuthHelper.Policys;
using ScheduleJob.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleJob.Core.Extensions
{
    public static class AuthorizationSetup
    {
        public static void AddAuthorizationSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            #region JWT
            #region 参数
            //读取配置文件
            var jwtSettings = new JwtSettings()
            {
                SecretKey = Appsettings.app(new string[] { "JwtSettings", "SecretKey" }),
                Issuer = Appsettings.app(new string[] { "JwtSettings", "Issuer" }),
                Audience = Appsettings.app(new string[] { "JwtSettings", "Audience" }),
            };
            var symmetricKeyAsBase64 = jwtSettings.SecretKey;
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            // 如果要数据库动态绑定，这里先留个空，后边处理器里动态赋值
            var permission = new List<PermissionItem>();

            // 角色与接口的权限要求参数
            var permissionRequirement = new PermissionRequirement(
                "/api/denied",// 拒绝授权的跳转地址（目前无用）
                permission,//角色菜单实体
                ClaimTypes.Role,//基于角色的授权
                jwtSettings.Issuer,//发行人
                jwtSettings.Audience,//听众
                signingCredentials,//签名凭据
                expiration: TimeSpan.FromSeconds(60 * 60)//接口的过期时间
                );
            #endregion

            // 令牌验证参数
            var tokenValidationParameters = new TokenValidationParameters
            {
                //令牌验证3+2
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,//发行人
                ValidateAudience = true,
                ValidAudience = jwtSettings.Audience,//订阅人
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(30),
                RequireExpirationTime = true,
            };

            //No.1 基于自定义角色的策略授权
            services.AddAuthorization(options =>
            {
                //基于需要Requirements
                options.AddPolicy(Permissions.Name, policy => policy.Requirements.Add(permissionRequirement));
            });
            //No.2 配置认证服务 开启Bearer认证
            services.AddAuthentication(options =>
            {
                //认证middleware配置
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(t =>//No3.添加JwtBearer服务
            {
                t.TokenValidationParameters = tokenValidationParameters;
                t.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        // 如果过期，则把<是否过期>添加到，返回头信息中
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            //注入权限处理核心控制器,将自定义的授权处理器 匹配给官方授权处理器接口，这样当系统处理授权的时候，就会直接访问我们自定义的授权处理器了。
            services.AddScoped<IAuthorizationHandler, PermissionHandler>();//注意此处的注入类型取决于你获取角色Action信息的注入类型如果你服务层用AddScoped此处也必须是AddScoped
            //将授权必要类注入生命周期内
            services.AddSingleton(permissionRequirement);

            #endregion

        }
    }


    /// <summary>
    /// 权限变量配置
    /// </summary>
    public static class Permissions
    {
        public const string Name = "Permission";
    }
}
