using Kalendario.Core.Common;

namespace Kalendario.Core.Entities
{
    public class Customer : AccountEntity, ISoftDeletable
    {
        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Warning { get; set; }
        public bool IsDeleted { get; set; }
    }
}