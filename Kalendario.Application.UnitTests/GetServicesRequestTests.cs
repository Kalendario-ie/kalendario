using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kalendario.Application.Queries.Admin;
using Kalendario.Application.ResourceModels.Admin;
using Kalendario.Application.Results;
using Kalendario.Application.Test.Common;
using Kalendario.Application.UnitTests.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kalendario.Application.UnitTests;

[TestClass]
public class GetServicesRequestTests : QueryTestFixture
{
    [TestMethod]
    public async Task EnsureReturnsOnlyCurrentAccountEntities()
    {
        var validIds = await Context.Services
            .IgnoreQueryFilters()
            .Where(c => c.AccountId == Constants.CurrentUserAccountId)
            .Select(c => c.Id)
            .ToListAsync();

        var sut = new GetServicesRequest.Handler(Context, Mapper);

        var result = await sut.Handle(new GetServicesRequest(), CancellationToken.None);

        Assert.IsInstanceOfType(result, typeof(GetAllResult<ServiceAdminResourceModel>));
        Assert.IsTrue(result.Entities.Count > 0);
        Assert.AreEqual(validIds.Count, result.Entities.Count);


        var isCurrentUserAccount = result.Entities
            .All(entity => validIds.Contains(entity.Id));
        Assert.IsTrue(isCurrentUserAccount);
    }

    [TestMethod]
    public async Task EnsureEmptyAccountGuidReturnsNothing()
    {
        this.CurrentUserServiceMock.Setup(m => m.AccountId)
            .Returns(Guid.Empty)
            ;
        var sut = new GetEmployeesRequest.Handler(Context, Mapper);

        var result = await sut.Handle(new GetEmployeesRequest(), CancellationToken.None);

        Assert.IsInstanceOfType(result, typeof(GetAllResult<EmployeeAdminResourceModel>));
        Assert.AreEqual(0, result.Entities.Count);
        Assert.AreEqual(0, result.TotalCount);
        Assert.AreEqual(0, result.FilteredCount);
    }
}