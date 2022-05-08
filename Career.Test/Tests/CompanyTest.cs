using System.Threading.Tasks;
using Xunit;

namespace Career.Test.Tests
{
    public class CompanyTest
    {
        [Fact]
        public async Task CREATE_JOB_SUCCES_CASE()
        {
            // var result = await ProductMoq.GetProductService()
            //     .CreateProduct(new ProductContract { ProductCode = "P1", Price = 100, Stock = 1000 });

            // Assert.True(result);
        }

        [Fact]
        public async Task SEARCH_JOB_INFO_NULL_PRODUCT_CODE_FAIL_CASE()
        {
            // var result = await ProductMoq.GetProductService()
            //     .GetProductInfo(null);

            // Assert.Null(result);
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