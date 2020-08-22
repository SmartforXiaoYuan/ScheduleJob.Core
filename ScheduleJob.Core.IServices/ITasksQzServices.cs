using ScheduleJob.Core.Contract;
using ScheduleJob.Core.Contract.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleJob.Core.IServices
{
    public interface ITasksQzServices : IBaseServices<TasksQz>
    {
        Task<int> AddAsync(TasksQzInputDto model);
       
    }
}
