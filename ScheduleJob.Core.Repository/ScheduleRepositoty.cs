using ScheduleJob.Core.Contract.ScheduleModels;
using ScheduleJob.Core.IRepository;
using ScheduleJob.Core.IRepository.UnitOfWork;
using ScheduleJob.Core.Repository.BASE;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScheduleJob.Core.Repository
{
   public class ScheduleRepositoty : BaseRepository<ScheduleEntity>, IScheduleRepositoty
    {
        public ScheduleRepositoty(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
