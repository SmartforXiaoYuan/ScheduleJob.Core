using ScheduleJob.Core.Contract;
using ScheduleJob.Core.Contract.Dtos;
using ScheduleJob.Core.IRepository;
using ScheduleJob.Core.IServices;
using ScheduleJob.Core.Services.BASE;
using ScheduleJob.Core.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleJob.Core.Services
{
    public partial class TasksQzServices : BaseService<TasksQz>, ITasksQzServices
    {
        ITasksQzRepository _dal;
       

        public TasksQzServices(ITasksQzRepository dal )
        {
            this._dal = dal;
            base.BaseDal = dal;
        }


        public async Task<int> AddAsync(TasksQzInputDto model)
        {
            if (model.IsApiUrl == (int)BoolStatus.True)
            {
                model.AssemblyName = "SC.TS.Tasks";
                model.ClassName = "Job_HttpApi_Quartz";
                //return 0;
            }
            //var entity = model.MapTo<TasksQz>();
            var entity = new TasksQz();
            var id = (await _dal.Add(entity));
            return id;
         
        }

        public async Task TestDemo()
        {
             
        }
    }
}
