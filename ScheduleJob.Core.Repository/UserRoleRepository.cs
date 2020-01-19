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
    /// UserRoleRepository
    /// </summary>	
    public class UserRoleRepository : BaseRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
