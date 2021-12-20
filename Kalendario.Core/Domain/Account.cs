using System;

namespace Kalendario.Core.Domain
{
    public class Account : BaseEntity
    {
        public string Name { get; set; }

        public string Avatar { get; set; }
    }
}