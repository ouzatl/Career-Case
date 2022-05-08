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
        public async Task SEARCH_JOB_SUCCES_CASE()
        {
            // var result = await JobMoq.GetJobService()
            //     .Search("a");
        }

        [Fact]
        public async Task SEARCH_PRODUCT_INFO_EMPTY_PRODUCT_CODE_FAIL_CASE()
        {
            // var result = await ProductMoq.GetProductService()
            //     .GetProductInfo("");

            // Assert.Null(result);
        }
    }
}