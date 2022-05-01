using Career.Contract.Contracts.Job;

namespace Career.Service.Services.JobService
{
    public interface IJobService : IBaseService
    {
        Task Add(JobContract contract);
        Task AddBannedWords(string words);
    }
}