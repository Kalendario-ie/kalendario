using System;

namespace Kalendario.Core.Domain
{
    public class BaseEntity
    {
        public Guid Id { get; set; }

        public DateTime? DateCreated { get; set; }
        
        public Guid UserCreated { get; set; }
        
        public DateTime? DateModified { get; set; }
        
        public Guid UserModified { get; set; }
    }
}