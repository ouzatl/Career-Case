using System.Text;
using AutoMapper;
using Career.Common.Constants;
using Career.Common.Logging;
using Career.Contract.Contracts.Job;
using Career.Contract.Job;
using Career.Data.Models;
using Career.Data.Repositories.BannedWordsRepository;
using Career.Data.Repositories.CompanyRepository;
using Career.Data.Repositories.JobRepository;
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


        public JobService(
            IJobRepository jobRepository,
            IBannedWordsRepository bannedWordsRepository,
            ICompanyRepository companyRepository,
            IDistributedCache distributedCache,
            ICompositeLogger logger,
            IMapper mapper,
            IPublishEndpoint endpoint
            )
        {
            _jobRepository = jobRepository;
            _bannedWordsRepository = bannedWordsRepository;
            _companyRepository = companyRepository;
            _distributedCache = distributedCache;
            _logger = logger;
            _mapper = mapper;
            _endpoint = endpoint;
        }

        public async Task Add(JobContract contract)
        {
            try
            {
                var company = await _companyRepository.Get(contract.CompanyId);

                if (company != null && company.MaxPostCount > 0)
                {

                    var job = _mapper.Map<Job>(contract);

                    job.AvailableUntil = contract.AvailableFrom.AddDays(15);
                    job.Quailty = await CalculateQuality(job);
                    job.IsActive = true;

                    var result = await _jobRepository.Add(job);

                    if (result != null)
                    {
                        await UpdateCompany(company);

                        await _endpoint.Publish<IJobChangedMessage>(new JobChangedMessage()
                        {
                            MessageId = new Guid(),
                            Job = contract,
                            CreatedDate = DateTime.Now
                        });

                        //düzgün bir response dönce controller seviyesinde sarmala.

                    }
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error("JobService Add Method: ", ex);
            }
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

        public async Task AddBannedWords(string words)
        {
            try
            {
                await _bannedWordsRepository.Add(new BannedWords { Name = words, IsActive = true });
            }
            catch (System.Exception ex)
            {
                _logger.Error("JobService AddBannedWords Method: ", ex);
            }
        }
    }
}