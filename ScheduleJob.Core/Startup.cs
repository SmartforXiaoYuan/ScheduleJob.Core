using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ScheduleJob.Core.Extensions;

namespace ScheduleJob.Core
{
    public class Startup
    {
        private object apiVersionNmae = "V1";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.JobExtension();
            services.CommonExtension();
            services.AddSwaggerSetup();
            services.AddSqlsugarSetup();
            #region 接口控制反转依赖注入  -netcore自带方法
            services.ServerExtension();
            services.RepositotyExtension();
         
            #endregion

            //.AddNewtonsoftJson(options =>  //(默认小写)修改api返回的字段
            //{
            //    // 忽略循环引用
            //    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //    // 不使用驼峰
            //    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            //    // 设置时间格式
            //    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            //    // 如字段为null值，该字段不会返回到前端
            //    // options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            //}); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //启用中间件服务生成Swagger作为JSON终结点
            app.UseSwagger();
            //UI话
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint($"/swagger/{apiVersionNmae}/swagger.json", $"{apiVersionNmae} doc.");

                option.RoutePrefix = "";
                //option.HeadContent = "dsadasf";
                //option.IndexStream = () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("xxx.html");//自定义模板
            });

          

            app.UseRouting();

            app.UseAuthentication();//先开启认证中间件
            app.UseAuthorization(); //然后是授权中间件(自带开启)

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
