using Career.Contract.Contracts.Job;
using Career.Contract.ElasticSearchModel.Job;

namespace Career.Service.Services.JobService
{
    public interface IJobService : IBaseService
    {
        Task<bool> Add(JobContract contract);
        Task<bool> AddBannedWords(string words);
        Task<List<JobElasticModel>> Search(string text);
    }
}