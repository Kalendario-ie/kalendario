using System;
using Kalendario.Application.Common.Interfaces;
using Moq;

namespace Kalendario.Application.IntegrationTests.Common;

public static class TestMocks
{
    public static Mock<ICurrentUserService> CurrentUserServiceMock(Guid userId, Guid accountId, bool isAuthenticated)
    {
        var mock = new Mock<ICurrentUserService>();
        mock.Setup(m => m.UserId)
            .Returns(userId.ToString());
        mock.Setup(m => m.AccountId)
            .Returns(accountId);
        mock.Setup(m => m.IsAuthenticated)
            .Returns(isAuthenticated);
        return mock;
    }
}