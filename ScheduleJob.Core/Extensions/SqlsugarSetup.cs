using Microsoft.Extensions.DependencyInjection;
using ScheduleJob.Core.Utility;
using ScheduleJob.Core.Utility.DB;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ScheduleJob.Core.Extensions
{
    /// <summary>
    /// SqlSugar 启动服务
    /// </summary>
    public static class SqlsugarSetup
    {
        private static string _connectionString = Contract.Seed.MyContext.GetMainConnectionDb().Conn;
      
        public static void AddSqlsugarSetup(this IServiceCollection services)
        { 
            // 默认添加主数据库连接
            MainDb.CurrentDbConnId = Appsettings.app(new string[] { "MainDB" });
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddScoped<SqlSugar.ISqlSugarClient>(o =>
            {
                //var listConfig = new List<ConnectionConfig>();
                //BaseDBConfig.MutiConnectionString.ForEach(m =>
                //{
                //    listConfig.Add(new ConnectionConfig()
                //    {
                //        ConfigId = m.ConnId.ObjToString().ToLower(),
                //        ConnectionString = m.Conn,
                //        DbType = (DbType)m.DbType,
                //        IsAutoCloseConnection = true,
                //        IsShardSameThread = false,
                //        AopEvents = new AopEvents
                //        {
                //            OnLogExecuting = (sql, p) =>
                //            {
                //                // 多库操作的话，此处暂时无效果，在另一个地方有效，具体请查看BaseRepository.cs
                //            }
                //        },
                //        MoreSettings = new ConnMoreSettings()
                //        {
                //            IsAutoRemoveDataCache = true
                //        }
                //        //InitKeyType = InitKeyType.SystemTable
                //    }
                //   );
                //});
                //return new SqlSugarClient(listConfig);
                var db = new SqlSugar.SqlSugarClient(new SqlSugar.ConnectionConfig()
                {
                    ConnectionString = _connectionString,//BaseDBConfig.ConnectionString,//必填, 数据库连接字符串
                    DbType = SqlSugar.DbType.Sqlite,//(SqlSugar.DbType)BaseDBConfig.DbType,//必填, 数据库类型
                    IsAutoCloseConnection = true,//默认false, 时候知道关闭数据库连接, 设置为true无需使用using或者Close操作
                    InitKeyType = SqlSugar.InitKeyType.SystemTable//默认SystemTable, 字段信息读取, 如：该属性是不是主键，标识列等等信息
                });
                return db;
            });
        }
    }
}
