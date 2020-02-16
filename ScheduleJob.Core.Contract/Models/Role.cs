using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScheduleJob.Core.Contract.Models
{
   public class Role: RootEntity
    {
      
        /// <summary>
        /// 角色名
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string Name { get; set; }
        /// <summary>
        ///描述
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
        public string Description { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>
        public bool Enabled { get; set; } = true;

        [SugarColumn(IsIgnore = true)]
        public List<int> MenuIds { get; set; }
    }
}
