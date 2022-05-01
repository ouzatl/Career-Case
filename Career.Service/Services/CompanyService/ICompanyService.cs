
using Career.Contract.Contracts.Company;

namespace Career.Service.Services.CompanyService
{
    public interface ICompanyService : IBaseService
    {
        Task Add(CompanyContract contract);
    }
}