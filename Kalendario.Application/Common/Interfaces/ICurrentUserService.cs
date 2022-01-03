using System;

namespace Kalendario.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        public string UserId { get; }

        public Guid AccountId { get; }
       
        public bool IsAuthenticated { get; }
    }
}