using Career.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Career.Data.Repositories.BannedWordsRepository
{
    public class BannedWordsRepository : BaseRepository<BannedWords>, IBannedWordsRepository
    {
        private readonly DbContext _dbContext;

        public BannedWordsRepository(CareerContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

    }
}