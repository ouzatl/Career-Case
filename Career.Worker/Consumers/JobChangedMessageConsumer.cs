using Career.Common.Logging;
using Career.Contract.ElasticSearchModel.Job;
using Career.Contract.Job;
using Career.Data.ElasticSearch;
using MassTransit;

namespace Career.Worker.Consumers
{
    public class JobChangedMessageConsumer : IConsumer<IJobChangedMessage>
    {
        private readonly IElasticSearchContext _elasticSearchContext;
        private readonly ICompositeLogger _compositeLogger;

        public JobChangedMessageConsumer
        (
            IElasticSearchContext elasticSearchContext,
            ICompositeLogger compositeLogger
        )
        {
            _elasticSearchContext = elasticSearchContext;
            _compositeLogger = compositeLogger;
        }
        public async Task Consume(ConsumeContext<IJobChangedMessage> context)
        {
            try
            {
                var jobElastic = new JobElasticModel
                {
                    CompanyId = context.Message.Job.CompanyId,
                    Position = context.Message.Job.Position,
                    Description = context.Message.Job.Description,
                    AvailableFrom = context.Message.Job.AvailableFrom,
                    Benefits = context.Message.Job.Benefits,
                    TypeOfWork = context.Message.Job.TypeOfWork,
                    Salary = context.Message.Job.Salary
                };

                var result = await _elasticSearchContext.AddData<JobElasticModel, string>("job", jobElastic);
                if (!result)
                    throw new ArgumentException("unsuccessfull response");
            }
            catch (System.Exception ex)
            {
                _compositeLogger.Error("JobChangedMessageConsumer Consume Method", ex);
            }
        }
    }
}