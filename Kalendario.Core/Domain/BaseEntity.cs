using System;
using System.Collections.Generic;

namespace Kalendario.Core.Domain;

public class BaseEntity
{
    public Guid Id { get; set; }

    public DateTime? DateCreated { get; set; }

    public Guid UserCreated { get; set; }

    public DateTime? DateModified { get; set; }

    public Guid UserModified { get; set; }

    public const string CreateRole = "Create";

    public const string UpdateRole = "Update";

    public const string DeleteRole = "Delete";

    public const string ViewRole = "View";

    public static IEnumerable<string> EntityRoles()
    {
        return new List<string>() {CreateRole, UpdateRole, DeleteRole, ViewRole};
    }
    
}