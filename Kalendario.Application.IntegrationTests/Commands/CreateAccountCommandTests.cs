using System;
using System.Threading.Tasks;
using FluentAssertions;
using Kalendario.Application.Authorization;
using Kalendario.Application.Commands.Admin;
using Kalendario.Application.Common.Exceptions;
using Kalendario.Core.Entities;
using Kalendario.Core.Infrastructure;
using NUnit.Framework;

namespace Kalendario.Application.IntegrationTests.Commands;

using static Testing;
public class CreateAccountCommandTests : TestBase
{
    [Test]
    public async Task UnauthenticatedUser_ShouldThrow_AuthenticationException()
    {
        RunAsAnonymousUser();
        var command = new CreateAccountCommand();
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<UnauthorizedAccessException>();
    }
    
    [Test]
    public async Task UnauthorizedUser_ShouldThrow_ForbiddenAccessException()
    {
        await RunAsDefaultUserAsync();
        var command = new CreateAccountCommand();
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ForbiddenAccessException>();
    }
    
    [TestCase("")]
    [TestCase("1")]
    [TestCase("123456789")]
    [TestCase("NameWithAsterisk*")]
    [TestCase("NameWithDollarSigh$")]
    [TestCase("0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567891")]
    public async Task InvalidName_ShouldThrow_ValidationException(string name)
    {
        await RunAsAdministratorAsync(typeof(Account), Account.CreateRole);

        var command = new CreateAccountCommand{ Name = name};
        
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [TestCase("NameWithNoSpace")]
    [TestCase("Name with spaces")]
    public async Task ValidName_ShouldCreateAccount(string name)
    {
        await RunAsAdministratorAsync(typeof(Account), Account.CreateRole);

        var command = new CreateAccountCommand{ Name = name};
        var result = await FluentActions.Invoking(() => SendAsync(command)).Invoke();

        var account = await FindAsync<Account>(result);
        Assert.IsNotNull(account);
        Assert.AreEqual(name, account.Name);

    }

    [Test]
    public async Task CreateAccount_ShouldUpdateUser()
    {
        var name = "NameWithNoSpace";
        var userId = await RunAsAdministratorAsync(typeof(Account), Account.CreateRole);

        var command = new CreateAccountCommand {Name = name};
        var result = await FluentActions.Invoking(() => SendAsync(command)).Invoke();

        var account = await FindAsync<Account>(result);
        Assert.IsNotNull(account);
        Assert.AreEqual(name, account.Name);
        
        var user = await FindAsync<ApplicationUser>(userId);
        Assert.AreEqual(account.Id, user.AccountId);
        
        Assert.IsTrue(await IsInRoleAsync(user, AuthorizationHelper.MasterRole));

        Assert.False(await IsInRoleAsync(user, AuthorizationHelper.RoleName(typeof(Account), Account.CreateRole)));
    }

    [Test]
    public async Task Should_ThrowError_OnRepeatedNames()
    {
        await RunAsAdministratorAsync(typeof(Account), Account.CreateRole);

        var name = "Companywith";

        var command1 = new CreateAccountCommand{ Name = name};
        var result = await FluentActions.Invoking(() => SendAsync(command1)).Invoke();

        var command2 = new CreateAccountCommand{ Name = name};
        await FluentActions.Invoking(() => SendAsync(command2)).Should().ThrowAsync<ValidationException>();
    }
}