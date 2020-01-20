using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScheduleJob.Core.Contract 
{
   public class RootEntity
    {
        [SugarColumn(IsNullable = false, IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        /// <summary>
        /// 创建时间//   类型后面加问号代表可以为空
        /// </summary>
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        /// <summary>
        /// 创建人ID
        /// </summary>
        public long? CreatedId { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = false, ColumnDataType = "nvarchar")]
        public string CreatedName { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdateDate { get; set; }
        /// <summary>
        /// 创建人姓名
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = false, ColumnDataType = "nvarchar")]
        public string UpdateName { get; set; }
        /// <summary>
        /// 修改人ID
        /// </summary>
        public long? UpdateId { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>
        public bool IsDrop { get; set; } = false;
    }
}
