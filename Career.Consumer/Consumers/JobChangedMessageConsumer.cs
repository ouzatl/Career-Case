using Career.Contract.Job;
using MassTransit;

namespace Career.Consumer.Consumers
{
    public class JobChangedMessageConsumer : IConsumer<IJobChangedMessage>
    {
        public Task Consume(ConsumeContext<IJobChangedMessage> context)
        {
            throw new NotImplementedException();
        }
    }
}