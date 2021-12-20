using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kalendario.Application.Queries;
using Kalendario.Application.ResourceModel;
using Kalendario.Application.Results;
using Kalendario.Application.Test.Common;
using Kalendario.Application.UnitTests.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kalendario.Application.UnitTests
{
    [TestClass]
    public class GetAccountsTest : QueryTestFixture
    {
        [TestMethod]
        public async Task EnsureOnlyCurrentUserAccountEmployeesShows()
        {
            var validIds = await Context.Customers
                .IgnoreQueryFilters()
                .Where(c => c.AccountId == Constants.CurrentUserAccountId)
                .Select(c => c.Id)
                .ToListAsync();
            
            var sut = new GetCustomersRequest.Handler(Context, Mapper);
            var result = await sut.Handle(new GetCustomersRequest(), CancellationToken.None);
            
            Assert.IsInstanceOfType(result, typeof(GetAllResult<CustomerResourceModel>));
            Assert.IsTrue(validIds.Count > 0);
            Assert.AreEqual(validIds.Count, result.Entities.Count);

            var isCurrentUserAccount = result.Entities
                .All(entity => validIds.Contains(entity.Id));
            Assert.IsTrue(isCurrentUserAccount);
        }
    }
}