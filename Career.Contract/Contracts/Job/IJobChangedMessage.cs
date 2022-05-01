using Career.Contract.Contracts.Job;

namespace Career.Contract.Job
{
    public interface IJobChangedMessage
    {
        Guid MessageId { get; set; }
        JobContract Job { get; set; }
        DateTime CreatedDate { get; set; }
    }
}