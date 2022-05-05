using Career.Contract.ElasticSearchModel.Job;
using Career.Contract.Job;
using Career.Data.ElasticSearch;
using MassTransit;

namespace Career.Consumer.Consumers
{
    public class JobChangedMessageConsumer : IConsumer<IJobChangedMessage>
    {
        // private readonly IElasticSearchContext _elasticSearchContext;
        // public JobChangedMessageConsumer(IElasticSearchContext elasticSearchContext)
        // {
        //     _elasticSearchContext = elasticSearchContext;
        // }
        public async Task Consume(ConsumeContext<IJobChangedMessage> context)
        {
            try
            {
                // var jobElastic = new JobElasticModel
                // {
                //     Id = context.Message.Job.Id,
                //     CompanyId = context.Message.Job.CompanyId,
                //     Position = context.Message.Job.Position,
                //     Description = context.Message.Job.Description,
                //     AvailableFrom = context.Message.Job.AvailableFrom,
                //     Benefits = context.Message.Job.Benefits,
                //     TypeOfWork = context.Message.Job.TypeOfWork,
                //     Salary = context.Message.Job.Salary
                // };

                // _elasticSearchContext.AddData<JobElasticModel, string>("JOB", jobElastic);
                await context.RespondAsync<string>(new
                {
                    value = $"Received : {context.Message.MessageId}"
                });
            }
            catch (System.Exception ex)
            {
            }
        }
    }
}