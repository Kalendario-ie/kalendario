using System;

namespace Kalendario.Core.Domain
{
    public class AccountEntity : BaseEntity
    {
        public Guid AccountId { get; set; }
        public Account Account { get; set; }
    }
}