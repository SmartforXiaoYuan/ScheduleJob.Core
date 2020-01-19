using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScheduleJob.Core.Contract.Models
{
    public class RoleModulePermission : RootEntity
    {
        // 下边三个实体参数，只是做传参作用，所以忽略下
        [SugarColumn(IsIgnore = true)]
        public Role Role { get; set; }
        [SugarColumn(IsIgnore = true)]
        public ModuleApi ModuleApi { get; set; }
        [SugarColumn(IsIgnore = true)]
        public MenuInfo MenuInfo { get; set; }
    }

   
   
   
}
