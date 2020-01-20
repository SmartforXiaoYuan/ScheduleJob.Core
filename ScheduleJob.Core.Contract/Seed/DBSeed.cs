using ScheduleJob.Core.Contract.Models;
using ScheduleJob.Core.Contract.ScheduleModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleJob.Core.Contract.Seed
{
    /// <summary>
    /// 简陋版本
    /// </summary>
   public class DBSeed
    {
        /// <summary>
        /// 异步添加种子数据
        /// </summary>
        /// <param name="myContext"></param>
        /// <returns></returns>
        public static async Task SeedAsync(MyContext myContext)
        {
            try
            {
                // 创建数据库
                //myContext.Db.DbMaintenance.CreateDatabase();
                // 创建表
                myContext.CreateTableByEntity(false,
                    typeof(UserInfo),
                    typeof(UserRole),
                    typeof(Role),
                    typeof(ModuleApi),
                    typeof(MenuInfo),
                    typeof(ScheduleEntity)
                    
                    );

                // 后期单独处理某些表
                // myContext.Db.CodeFirst.InitTables(typeof(sysUserInfo));

                Console.WriteLine("Database: created success!");
                Console.WriteLine();

                Console.WriteLine();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
