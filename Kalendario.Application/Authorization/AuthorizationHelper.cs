using System.Collections.Generic;
using System.Linq;
using Kalendario.Application.Common.Interfaces;

namespace Kalendario.Application.Authorization;

public static class AuthorizationHelper
{
    public static List<string> GetAllRoles()
    {
        var roleTypes = new List<string> {"Create", "Update", "Delete", "View"};

        return typeof(IKalendarioDbContext)
            .GetProperties()
            .Select(p => p.PropertyType.GetGenericArguments()[0].Name)
            .SelectMany(model => roleTypes.Select(role => $"{model}_{role}"))
            .ToList();
    }
}