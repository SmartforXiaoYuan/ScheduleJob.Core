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
            #region �ӿڿ��Ʒ�ת����ע��  -netcore�Դ�����
            services.ServerExtension();
            services.RepositotyExtension();
         
            #endregion

            //.AddNewtonsoftJson(options =>  //(Ĭ��Сд)�޸�api���ص��ֶ�
            //{
            //    // ����ѭ������
            //    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //    // ��ʹ���շ�
            //    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            //    // ����ʱ���ʽ
            //    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            //    // ���ֶ�Ϊnullֵ�����ֶβ��᷵�ص�ǰ��
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

            //�����м����������Swagger��ΪJSON�ս��
            app.UseSwagger();
            //UI��
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint($"/swagger/{apiVersionNmae}/swagger.json", $"{apiVersionNmae} doc.");

                option.RoutePrefix = "";
                //option.HeadContent = "dsadasf";
                //option.IndexStream = () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("xxx.html");//�Զ���ģ��
            });

          

            app.UseRouting();

            app.UseAuthentication();//�ȿ�����֤�м��
            app.UseAuthorization(); //Ȼ������Ȩ�м��(�Դ�����)

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
