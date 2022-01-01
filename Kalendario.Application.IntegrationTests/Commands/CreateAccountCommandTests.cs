using System;
using System.Threading.Tasks;
using FluentAssertions;
using Kalendario.Application.Commands.Admin;
using Kalendario.Application.Common.Exceptions;
using Kalendario.Core.Domain;
using NUnit.Framework;

namespace Kalendario.Application.IntegrationTests.Commands;

using static Testing;
public class CreateAccountCommandTests : TestBase
{
    [Test]
    public async Task UnauthenticatedUsers_ShouldThrow_AuthenticationException()
    {
        RunAsAnonymousUser();
        var command = new CreateAccountCommand();
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<UnauthorizedAccessException>();
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