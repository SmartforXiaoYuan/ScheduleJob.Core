using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScheduleJob.Core.Contract.Models
{
    public class UserInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [SugarColumn(IsNullable = false, IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        /// <summary>
        /// 登录账号
        /// </summary>
        [SugarColumn(Length = 200, IsNullable = true, ColumnDataType = "nvarchar")]
        public string UserName { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        [SugarColumn(Length = 200, IsNullable = true, ColumnDataType = "nvarchar")]
        public string UserPWD { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [SugarColumn(Length = 200, IsNullable = true, ColumnDataType = "nvarchar")]
        public string NickName { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        // 性别

        public int Sex { get; set; } = 0;
        // 年龄

        public int Age { get; set; }
        // 生日

        public DateTime Birth { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public System.DateTime CreateTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 更新时间
        /// </summary>
        public System.DateTime UpdateTime { get; set; } = DateTime.Now;

        /// <summary>
        ///最后登录时间 
        /// </summary>
        public DateTime LastErrTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 状态Enabled
        /// </summary>
        public int Status { get; set; }

        ///是否有效(0无效,1有效) 
        [SugarColumn(IsNullable = true, ColumnDescription = "是否有效(0无效,1有效)")]
        public int DataFlag { get; set; }

        #region 扩展字段 IsIgnore = true
        [SugarColumn(IsIgnore = true)]
        public List<int> RIDs { get; set; }
        [SugarColumn(IsIgnore = true)]
        public List<string> RoleNames { get; set; }

        #endregion
    }
}
