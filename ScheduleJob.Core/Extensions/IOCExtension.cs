using Microsoft.Extensions.DependencyInjection;
using ScheduleJob.Core.IRepository;
using ScheduleJob.Core.IRepository.UnitOfWork;
using ScheduleJob.Core.IServices;
using ScheduleJob.Core.IServices.QuartzCenter;
using ScheduleJob.Core.Repository;
using ScheduleJob.Core.Repository.UnitOfWork;
using ScheduleJob.Core.Services;
 
using ScheduleJob.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScheduleJob.Core.Extensions
{
    public static class IOCExtension
    {
        /// <summary>
        /// 服务层注入
        /// </summary>
        /// <param name="services"></param>
        public static void ServerExtension(this IServiceCollection services)
        {
            //注入用户管理服务层实现
            services.AddScoped<IUserInfoServices, UserInfoService>();
            services.AddScoped<IUserRoleServices, UserRoleService>();
            services.AddScoped<IRoleServices, RoleService>();
            services.AddScoped<ITasksQzServices, TasksQzServices>();
            services.AddScoped<IMenuServices, MenuService>();
            services.AddScoped<IRoleModuleService, RoleModuleService>();
            

        }
        /// <summary>
        /// 仓储层注入
        /// </summary>
        /// <param name="services"></param>
        public static void RepositotyExtension(this IServiceCollection services)
        {
            services.AddScoped<ITasksQzRepository, TasksQzRepository>();
            services.AddScoped<IUserInfoRepository, UserInfoRepository>(); 
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IScheduleRepositoty, ScheduleRepositoty>();
            services.AddScoped<IMenuRepositoty, MenuRepositoty>();
            services.AddScoped<IRoleModuleRepository, RoleModuleRepository>();

        }
        /// <summary>
        /// 公共扩展注入
        /// </summary>
        /// <param name="services"></param>
        public static void CommonExtension(this IServiceCollection services)
        {
            services.AddSingleton(new Appsettings());
            services.AddDbSetup();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
          
        }
        /// <summary>
        /// Job扩展注入
        /// </summary>
        /// <param name="services"></param>
        public static void JobExtension(this IServiceCollection services)
        {
            //services.AddQuartz(typeof(Services.QuartzCenter.HttpJob));
            //services.AddSingleton<ISchedulerCenter_bk, SchedulerCenter>();
            //services.AddTransient<TestJobOne>();//Job使用瞬时依赖注入
        }

    }
}
