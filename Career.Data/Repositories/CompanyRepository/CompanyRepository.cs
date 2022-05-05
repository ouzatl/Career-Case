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

        public async Task<IList<Company>> Get(string phoneNumber)
        {
            var company = await _dbContext.Set<Company>().Where(x => x.PhoneNumber == phoneNumber).ToListAsync();

            return company;
        }

    }
}