using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Core.Domain;

namespace Kalendario.Application.Authorization;

public static class AuthorizationHelper
{
    public static string RoleName(Type entityType, string role)
    {
        return $"{entityType.Name}_{role}";
    }

    public static List<string> GetAllRoles()
    {
        return typeof(IKalendarioDbContext)
            .GetProperties()
            .Select(p => p.PropertyType.GetGenericArguments()[0])
            .SelectMany(entityType => FindEntityRoles(entityType).Select(role => RoleName(entityType, role)))
            .ToList();
    }

    private static IEnumerable<string> FindEntityRoles(Type type)
    {
        while (true)
        {
            var entityRolesMethod = type.GetMethod(nameof(BaseEntity.EntityRoles), BindingFlags.Public | BindingFlags.Static);

            if (entityRolesMethod != null)
            {
                return (IEnumerable<string>) entityRolesMethod.Invoke(null, null);
            }

            type = type.BaseType;
        }
    }
}