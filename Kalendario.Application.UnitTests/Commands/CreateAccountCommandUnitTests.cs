using System.Threading;
using System.Threading.Tasks;
using Kalendario.Application.Commands.Admin;
using Kalendario.Application.Common.Exceptions;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.UnitTests.Common;
using Kalendario.Common;
using Kalendario.Core.Domain;
using Kalendario.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Mocks = Kalendario.Application.UnitTests.Common.Mocks;

namespace Kalendario.Application.UnitTests.Commands;

[TestClass]
public class CreateAccountCommandUnitTests
{
    private Mock<ICurrentUserService> _currentUserServiceMock;
    private Mock<IDateTime> _datetimeMock;
    private KalendarioDbContext _context;

    [TestInitialize]
    public void Setup()
    {
        this._currentUserServiceMock = Mocks.CurrentUserServiceMock();
        this._datetimeMock = Mocks.DatetimeMock();
        this._context = KalendarioContextFactory.Create(_currentUserServiceMock.Object, _datetimeMock.Object);
    }

    [TestMethod]
    public async Task Should_CreateAccount()
    {
        var accountName = "Teste1";
        var preAccounts = await this._context.Accounts.ToListAsync();

        var createAccountCommand = new CreateAccountCommand {Name = accountName};
        var result = await new CreateAccountCommand.Handler(_context)
            .Handle(createAccountCommand, CancellationToken.None);

        var postAccounts = await this._context.Accounts.ToListAsync();
        Assert.AreEqual(preAccounts.Count + 1, postAccounts.Count);

        var createdAccount = await _context.Accounts.FindAsync(result);

        Assert.IsInstanceOfType(createdAccount, typeof(Account));
        Assert.AreEqual(accountName, createdAccount.Name);
    }

    [TestMethod]
    public async Task Should_ThrowError_OnRepeatedNames()
    {
        var accountName = "Teste1";

        var createAccountCommand = new CreateAccountCommand {Name = accountName};
        var handler = new CreateAccountCommand.Handler(_context);

        var account1 = await handler
            .Handle(createAccountCommand, CancellationToken.None);

        var createAccountCommand2 = new CreateAccountCommand() {Name = accountName};
        
        await Assert.ThrowsExceptionAsync<ValidationException>(
            () => handler.Handle(createAccountCommand2, CancellationToken.None));
    }
}