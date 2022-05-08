using System.Text;
using AutoMapper;
using Career.Common.Constants;
using Career.Common.Logging;
using Career.Contract.Contracts.Job;
using Career.Contract.ElasticSearchModel.Job;
using Career.Contract.Job;
using Career.Data.Models;
using Career.ElasticSearch.ElasticSearch;
using Career.Repository.Repositories.BannedWordsRepository;
using Career.Repository.Repositories.CompanyRepository;
using Career.Repository.Repositories.JobRepository;
using MassTransit;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Career.Service.Services.JobService
{
    public class JobService : BaseService, IJobService
    {
        private readonly IJobRepository _jobRepository;
        private readonly IBannedWordsRepository _bannedWordsRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IDistributedCache _distributedCache;
        private readonly ICompositeLogger _logger;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _endpoint;
        private readonly ISendEndpointProvider _endpoint2;

        private readonly IElasticSearchContext _elasticSearchContext;


        public JobService(
            IJobRepository jobRepository,
            IBannedWordsRepository bannedWordsRepository,
            ICompanyRepository companyRepository,
            IDistributedCache distributedCache,
            ICompositeLogger logger,
            IMapper mapper,
            IPublishEndpoint endpoint,
            IElasticSearchContext elasticSearchContext,
            ISendEndpointProvider endpoint2
            )
        {
            _jobRepository = jobRepository;
            _bannedWordsRepository = bannedWordsRepository;
            _companyRepository = companyRepository;
            _distributedCache = distributedCache;
            _logger = logger;
            _mapper = mapper;
            _endpoint = endpoint;
            _endpoint2 = endpoint2;
            _elasticSearchContext = elasticSearchContext;
        }

        public async Task<bool> Add(JobContract contract)
        {
            var result = false;
            try
            {
                var company = await _companyRepository.Get(contract.CompanyId);

                if (company != null && company.MaxPostCount > 0)
                {
                    var job = _mapper.Map<Job>(contract);

                    job.AvailableUntil = contract.AvailableFrom.AddDays(15);
                    job.Quailty = await CalculateQuality(job);
                    job.IsActive = true;

                    var data = await _jobRepository.Add(job);

                    if (data != null)
                    {
                        await UpdateCompany(company);

                        var queue = await _endpoint2.GetSendEndpoint(new Uri("queue:test-queue"));
                        await queue.Send(new JobChangedMessage
                        {
                            MessageId = Guid.NewGuid(),
                            Job = contract,
                            CreatedDate = DateTime.Now
                        });
                    }
                }
            }
            catch (System.Exception ex)
            {
                result = false;
                _logger.Error("JobService Add Method: ", ex);
            }

            return result;
        }

        private async Task UpdateCompany(Company company)
        {
            company.MaxPostCount = company.MaxPostCount - 1;
            await _companyRepository.Update(company);
        }

        private async Task<int> CalculateQuality(Job job)
        {
            var result = default(int);

            if (!string.IsNullOrEmpty(job.TypeOfWork))
                result = result + 1;

            if (job.Salary > default(double))
                result = result + 1;

            if (!string.IsNullOrEmpty(job.Benefits))
                result = result + 1;

            var hasBannedWords = await HasBannedWords(job.Description);
            if (!hasBannedWords)
                result = result + 2;

            return result;
        }

        private async Task<bool> HasBannedWords(string description)
        {
            var bannedWordsList = await GetBannedWords();
            if (bannedWordsList != null && bannedWordsList.Count > 0)
                return bannedWordsList.Any(x => description.Contains(x.Name));

            return false;
        }

        private async Task<List<BannedWords>> GetBannedWords()
        {
            var cacheKey = CacheKeyConstants.BANNED_WORDS;
            var json = string.Empty;
            var bannedWordsList = new List<BannedWords>();
            var bannedWordsFromCache = await _distributedCache.GetAsync(cacheKey);
            if (bannedWordsFromCache != null)
            {
                json = Encoding.UTF8.GetString(bannedWordsFromCache);
                bannedWordsList = JsonConvert.DeserializeObject<List<BannedWords>>(json);
            }
            else
            {
                bannedWordsList = await _bannedWordsRepository.GetAll();
                json = JsonConvert.SerializeObject(bannedWordsList);
                bannedWordsFromCache = Encoding.UTF8.GetBytes(json);
                var options = new DistributedCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromDays(1))
                        .SetAbsoluteExpiration(DateTime.Now.AddMonths(1));
                await _distributedCache.SetAsync(cacheKey, bannedWordsFromCache, options);

            }

            return bannedWordsList;
        }

        public async Task<bool> AddBannedWords(string words)
        {
            var result = true;
            try
            {
                await _bannedWordsRepository.Add(new BannedWords { Name = words, IsActive = true });
            }
            catch (System.Exception ex)
            {
                result = false;
                _logger.Error("JobService AddBannedWords Method: ", ex);
            }

            return result;
        }

        public async Task<List<JobElasticModel>> Search(string text)
        {
            var result = new List<JobElasticModel>();

            try
            {
                var searchQuery = new Nest.SearchDescriptor<JobElasticModel>()
                                          .Query(q =>
                                            q.Match(m => m.Field(f => f.Position).Query(text))
                                            || q.Match(m => m.Field(f => f.Description).Query(text))
                                            || q.Match(m => m.Field(f => f.TypeOfWork).Query(text))
                                          )
                                          .Sort(a => a.Descending(s => s.AvailableFrom));

                var searchResult = await _elasticSearchContext.SimpleSearch<JobElasticModel, string>("job", searchQuery);
                result = searchResult.Documents.ToList();
            }
            catch (Exception ex)
            {
                _logger.Error("JobService Search Method", ex);
            }

            return result;
        }
    }
}