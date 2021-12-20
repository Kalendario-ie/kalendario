using System;
using Kalendario.Core.Domain;

namespace Kalendario.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        public Guid UserId { get; }

        public Guid AccountId { get; }
       
        public bool IsAuthenticated { get; }
    }
}