using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ScheduleJob.Core.Extensions
{
    public static class SwaggerSetup
    {
        private static string apiVersionName = "V1";

        public static void AddSwaggerSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc(apiVersionName, new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Version = apiVersionName,
                    Title = $"{apiVersionName}doc",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Url = new Uri("http://www.baidu.com"),
                        Name = "Job"
                    }
                });

                try
                {
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, "ScheduleJob.Core.xml");//这个就是刚刚配置的xml文件名
                    option.IncludeXmlComments(xmlPath, true);//默认的第二个参数是false，这个是controller的注释，记得修改
                    var xmlModelPath = Path.Combine(AppContext.BaseDirectory, "ScheduleJob.Core.Contract.xml");//这个就是Model层的xml文件名
                    option.IncludeXmlComments(xmlModelPath);
                }
                catch (Exception ex)
                {
                    //log.Error("ProjectD.Core.ProjectD.Core.Model.xml 丢失，请检查并拷贝。\n" + ex.Message);
                }

                #region 配置(JWT) 
                //请求才能携带参数
                option.OperationFilter<SecurityRequirementsOperationFilter>();
                //开启权限 小锁
                option.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                    Type = SecuritySchemeType.ApiKey
                });
                #endregion
            });
        }
    }
}
