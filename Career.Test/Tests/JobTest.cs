using System;
using System.Threading.Tasks;
using Career.Contract.Contracts.Job;
using Career.Test.Moqs;
using Xunit;

namespace Career.Test.Tests
{
    public class JobTest
    {
        [Fact]
        public async Task ADD_JOB_SUCCES_CASE()
        {
            var result = await JobMoq.GetJobService()
                .Add(new JobContract
                {
                    CompanyId = 1,
                    Position = "Senior .Net Developer",
                    Description = "C#",
                    AvailableFrom = DateTime.Now,
                    Benefits = "Double Bonus Per Year",
                    TypeOfWork = "Full-Time",
                    Salary = 3000
                });

            Assert.True(result);
        }

        [Fact]
        public async Task ADD_JOB_FAIL_CASE()
        {
            var result = await JobMoq.GetJobService()
                .Add(new JobContract
                {
                    Position = "Senior .Net Developer",
                    Description = "C#",
                    AvailableFrom = DateTime.Now,
                    Benefits = "Double Bonus Per Year",
                    TypeOfWork = "Full-Time",
                    Salary = 3000
                });

            Assert.False(result);
        }
    }
}