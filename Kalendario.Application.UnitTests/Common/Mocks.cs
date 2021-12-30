using System;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Test.Common;
using Kalendario.Common;
using Moq;

namespace Kalendario.Application.UnitTests.Common;

public static class Mocks
{
    public static Mock<ICurrentUserService> CurrentUserServiceMock()
    {
        var mock = new Mock<ICurrentUserService>();
        mock.Setup(m => m.UserId)
            .Returns(Constants.CurrentUserId);
        mock.Setup(m => m.AccountId)
            .Returns(Constants.CurrentUserAccountId);
        mock.Setup(m => m.IsAuthenticated)
            .Returns(true);
        return mock;
    }

    public static Mock<IDateTime> DatetimeMock()
    {
        var mock = new Mock<IDateTime>();
        mock.Setup(m => m.Now)
            .Returns(DateTime.Now);
        return mock;
    }

    public static Mock<IIdentityService> IIdentityServiceMock()
    {
        return new Mock<IIdentityService>();
    }
}