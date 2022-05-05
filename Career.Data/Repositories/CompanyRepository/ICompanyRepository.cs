using Career.Data.Models;

namespace Career.Data.Repositories.CompanyRepository
{
    public interface ICompanyRepository : IBaseRepository<Company>
    {
        Task<IList<Company>> Get(string phoneNumber);
    }
}