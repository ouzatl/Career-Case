using Career.Contract.Contracts.Job;

namespace Career.Contract.Job
{
    public class JobChangedMessage : IJobChangedMessage
    {
        public Guid MessageId { get; set; }
        public JobContract Job { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}