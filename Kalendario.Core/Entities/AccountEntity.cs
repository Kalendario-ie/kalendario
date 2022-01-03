using System;

namespace Kalendario.Core.Entities
{
    public class AccountEntity : BaseEntity
    {
        public Guid AccountId { get; set; }
        public Account Account { get; set; }
    }
}