using AutoMapper;
using Career.Common.Logging;
using Career.Contract.Contracts.Company;
using Career.Data.Models;
using Career.Data.Repositories.CompanyRepository;

namespace Career.Service.Services.CompanyService
{
    public class CompanyService : BaseService, ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly ICompositeLogger _logger;
        private readonly IMapper _mapper;

        public CompanyService(
            ICompanyRepository companyRepository,
            ICompositeLogger logger,
            IMapper mapper
            )
        {
            _companyRepository = companyRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task Add(CompanyContract contract)
        {
            try
            {
                var companyResult = await _companyRepository.Get(contract.PhoneNumber);

                if (companyResult.Count == 0)
                {
                    var company = _mapper.Map<Company>(contract);

                    company.MaxPostCount = 2;
                    company.IsActive = true;

                    var result = await _companyRepository.Add(company);

                    if (result != null)
                    {
                        //response dön ve sarmala.
                    }
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error("CompanyService Add Method: ", ex);
            }
        }
    }
}