using AutoMapper;
using Career.Common.Logging;
using Career.Repository.Repositories.BannedWordsRepository;
using Career.Repository.Repositories.CompanyRepository;
using Career.Repository.Repositories.JobRepository;
using Career.Service.Services.CompanyService;
using Moq;

namespace Career.Test.Moqs
{
    public class CompanyMoq
    {
        #region Moqs

        public static ICompositeLogger LoggerMoq() => Mock.Of<ICompositeLogger>();
        public static IMapper MapperMoq() => Mock.Of<IMapper>();
        public static IJobRepository JobRepositoryMoq() => Mock.Of<IJobRepository>();
        public static IBannedWordsRepository BannedWordsRepositoryMoq() => Mock.Of<IBannedWordsRepository>();
        public static ICompanyRepository CompanyRepositoryMoq() => Mock.Of<ICompanyRepository>();

        #endregion

        #region GetMoqs

        public static ICompanyService GetCompanyService() => new CompanyService(
            CompanyRepositoryMoq(),
            LoggerMoq(),
            MapperMoq());

        #endregion
    }
}