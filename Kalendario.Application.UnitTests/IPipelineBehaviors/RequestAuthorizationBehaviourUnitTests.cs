using System;
using System.Threading;
using System.Threading.Tasks;
using Kalendario.Application.Common.Behaviours;
using Kalendario.Application.Common.Exceptions;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Common.Security;
using Kalendario.Core.Domain;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Mocks = Kalendario.Application.UnitTests.Common.Mocks;

namespace Kalendario.Application.UnitTests.IPipelineBehaviors;

[TestClass]
public class RequestAuthorizationBehaviourUnitTests
{
    private Mock<IIdentityService> _iIdentityServiceMock;
    private Mock<ICurrentUserService> _currentUserServiceMock;

    [TestInitialize]
    public void Setup()
    {
        this._iIdentityServiceMock = Mocks.IIdentityServiceMock();
        this._currentUserServiceMock = Mocks.CurrentUserServiceMock();
    }

    [TestMethod]
    public async Task EnsureThrowsException_OnIdentityServiceReturningFalse()
    {
        var simpleRequest = new SimpleRequest();

        var validationBehavior =
            new RequestAuthorizationBehaviour<SimpleRequest, Unit>(_iIdentityServiceMock.Object,
                _currentUserServiceMock.Object);

        _iIdentityServiceMock.Setup(i => i.IsInRoleAsync(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(() => Task.FromResult(false));

        await Assert.ThrowsExceptionAsync<ForbiddenAccessException>(() => validationBehavior.Handle(simpleRequest,
            CancellationToken.None,
            () => Task.FromResult(Unit.Value)));
    }

    [TestMethod]
    public async Task EnsureCompletes_OnIdentityServiceReturningTrue()
    {
        var simpleRequest = new SimpleRequest();

        var validationBehavior =
            new RequestAuthorizationBehaviour<SimpleRequest, Unit>(_iIdentityServiceMock.Object,
                _currentUserServiceMock.Object);

        _iIdentityServiceMock.Setup(i => i.IsInRoleAsync(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(() => Task.FromResult(true));

        var unit = await validationBehavior.Handle(simpleRequest, CancellationToken.None,
            () => Task.FromResult(Unit.Value));

        Assert.AreEqual(Unit.Value, unit);
    }

    [TestMethod]
    public async Task EnsureThrowsException_OnIdentityServiceReturningFalse_MultipleRoles()
    {
        var multipleRolesRequest = new MultipleRolesRequest();

        var validationBehavior =
            new RequestAuthorizationBehaviour<MultipleRolesRequest, Unit>(_iIdentityServiceMock.Object,
                _currentUserServiceMock.Object);

        _iIdentityServiceMock.Setup(i => i.IsInRoleAsync(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(() => Task.FromResult(false));

        await Assert.ThrowsExceptionAsync<ForbiddenAccessException>(() => validationBehavior.Handle(
            multipleRolesRequest,
            CancellationToken.None,
            () => Task.FromResult(Unit.Value)));
    }


    [TestMethod]
    public async Task EnsureCompletes_OnIdentityServiceReturningTrueToSingleRole_MultipleRolesRequest()
    {
        var multipleRolesRequest = new MultipleRolesRequest();

        var validationBehavior =
            new RequestAuthorizationBehaviour<MultipleRolesRequest, Unit>(_iIdentityServiceMock.Object,
                _currentUserServiceMock.Object);

        _iIdentityServiceMock.Setup(i => i.IsInRoleAsync(It.IsAny<string>(), It.IsAny<string>()))
            .Returns((string userId, string role) =>
                Task.FromResult(role.Contains(BaseEntity.CreateRole)));

        var unit = await validationBehavior.Handle(multipleRolesRequest, CancellationToken.None,
            () => Task.FromResult(Unit.Value));

        Assert.AreEqual(Unit.Value, unit);
    }
}

[Authorize(typeof(BaseEntity), BaseEntity.CreateRole)]
public class SimpleRequest : IRequest<Unit>
{
}

[Authorize(typeof(BaseEntity), $"{BaseEntity.CreateRole},{BaseEntity.DeleteRole},{BaseEntity.ViewRole}")]
public class MultipleRolesRequest : IRequest<Unit>
{
}