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
    public class GetEmployeesTests : QueryTestFixture
    {
        [TestMethod]
        public async Task EnsureOnlyCurrentUserAccountEmployeesShows()
        {
            var sut = new GetEmployeesRequest.Handler(Context, Mapper);

            var result = await sut.Handle(new GetEmployeesRequest(), CancellationToken.None);
            
            Assert.IsInstanceOfType(result, typeof(GetEmployeesResult));
            Assert.IsTrue(result.Employees.Count > 0);
            
            foreach (var employee in result.Employees)
            {
                Assert.AreEqual(employee.AccountId, Constants.CurrentUserAccountId);
            }
        }
    }
}