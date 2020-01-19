using ScheduleJob.Core.Contract.Models;
using ScheduleJob.Core.IRepository;
using ScheduleJob.Core.IRepository.UnitOfWork;
using ScheduleJob.Core.Repository.BASE;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScheduleJob.Core.Repository
{
    /// <summary>
    /// RoleRepository
    /// </summary>	
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
