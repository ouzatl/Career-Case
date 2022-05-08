using AutoMapper;
using Career.Common.Logging;
using Career.ElasticSearch.ElasticSearch;
using Career.Repository.Repositories.BannedWordsRepository;
using Career.Repository.Repositories.CompanyRepository;
using Career.Repository.Repositories.JobRepository;
using Career.Service.Services.JobService;
using MassTransit;
using Microsoft.Extensions.Caching.Distributed;
using Moq;

namespace Career.Test.Moqs
{
    public class JobMoq
    {
        #region Moqs

        public static ICompositeLogger LoggerMoq() => Mock.Of<ICompositeLogger>();
        public static IJobRepository JobRepositoryMoq() => Mock.Of<IJobRepository>();
        public static IBannedWordsRepository BannedWordsRepositoryMoq() => Mock.Of<IBannedWordsRepository>();
        public static ICompanyRepository CompanyRepositoryMoq() => Mock.Of<ICompanyRepository>();
        public static IMapper MapperMoq() => Mock.Of<IMapper>();
        public static IDistributedCache DistributedCacheMoq() => Mock.Of<IDistributedCache>();
        public static IPublishEndpoint PublishEndpointMoq() => Mock.Of<IPublishEndpoint>();
        public static ISendEndpointProvider SendEndpointProviderMoq() => Mock.Of<ISendEndpointProvider>();
        public static IElasticSearchContext ElasticSearchContextMoq() => Mock.Of<IElasticSearchContext>();


        #endregion

        #region GetMoqs

        public static IJobService GetJobService() => new JobService(
            JobRepositoryMoq(),
            BannedWordsRepositoryMoq(),
            CompanyRepositoryMoq(),
            DistributedCacheMoq(),
            LoggerMoq(),
            MapperMoq(),
            PublishEndpointMoq(),
            ElasticSearchContextMoq(),
            SendEndpointProviderMoq());

        #endregion
    }
}