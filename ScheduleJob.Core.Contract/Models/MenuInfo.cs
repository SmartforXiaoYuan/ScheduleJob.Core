using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScheduleJob.Core.Contract.Models
{
   public class MenuInfo: RootEntity
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string Name { get; set; }

        /// <summary>
        /// 前端路由地址 
        /// </summary>
        public string RoutePath { get; set; }

        /// <summary>
        /// 父ID 上级菜单ID
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? ParentId { get; set; }

        /// <summary>
        /// API数据请求地址
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
        public string ApiUrl { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int OrderSort { get; set; }
        /// <summary>
        /// /描述
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
        public string Description { get; set; }
        /// <summary>
        /// 是否是右侧菜单
        /// </summary>
        public bool IsMenu { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
        public string Icon { get; set; }

    }
}
