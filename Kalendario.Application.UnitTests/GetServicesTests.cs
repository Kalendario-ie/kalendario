using System.Threading;
using System.Threading.Tasks;
using Kalendario.Application.Queries;
using Kalendario.Application.Results;
using Kalendario.Application.Test.Common;
using Kalendario.Application.UnitTests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kalendario.Application.UnitTests
{
    [TestClass]
    public class GetServicesTests : QueryTestFixture
    {
        [TestMethod]
        public async Task EnsureOnlyCurrentUserAccountEmployeesShows()
        {
            var sut = new GetServicesRequest.Handler(Context, Mapper);

            var result = await sut.Handle(new GetServicesRequest(), CancellationToken.None);
            
            Assert.IsInstanceOfType(result, typeof(GetServicesResult));
            Assert.IsTrue(result.Services.Count > 0);
            
            foreach (var employee in result.Services)
            {
                Assert.AreEqual(employee.AccountId, Constants.CurrentUserAccountId);
            }
        }
    }
}