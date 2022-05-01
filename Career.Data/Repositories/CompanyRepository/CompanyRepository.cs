using Career.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Career.Data.Repositories.CompanyRepository
{
    public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
    {
        private readonly DbContext _dbContext;

        public CompanyRepository(CareerContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

    }
}