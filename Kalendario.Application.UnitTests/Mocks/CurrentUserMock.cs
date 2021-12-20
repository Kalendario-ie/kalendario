using System;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Test.Common;

namespace Kalendario.Application.UnitTests.Mocks
{
    public class CurrentUserMock : ICurrentUserService
    {
        public CurrentUserMock()
        {
            UserId = Constants.CurrentUserId;
            AccountId = Constants.CurrentUserAccountId;
            IsAuthenticated = true;
        }

        public Guid UserId { get; }
        public Guid AccountId { get; }
        public bool IsAuthenticated { get; }
    }
}