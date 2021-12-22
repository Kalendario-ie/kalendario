using System;
using Kalendario.Application.Common.Interfaces;

namespace Kalendario.Api.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService()
        {
            
        }
        public Guid UserId { get; }
        public Guid AccountId { get; }
        public bool IsAuthenticated { get; }
    }
}