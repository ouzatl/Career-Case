using Career.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Career.Data.Repositories.JobRepository
{
    public class JobRepository : BaseRepository<Job>, IJobRepository
    {
        private readonly DbContext _dbContext;

        public JobRepository(CareerContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

    }
}