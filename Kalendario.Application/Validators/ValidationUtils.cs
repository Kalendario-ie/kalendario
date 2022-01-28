using System;
using System.Threading.Tasks;
using FluentValidation.Results;
using Kalendario.Application.Common.Exceptions;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Core.Entities;

namespace Kalendario.Application.Validators;

public static class ValidationUtils
{
    public static async Task ThrowIfNotExist<T>(Guid id, IKalendarioDbContext context, ICurrentUserManager currentUserManager) where T : AccountEntity
    {
        var entity = await context.Set<T>().FindAsync(id);
        if (entity == null || entity.AccountId != currentUserManager.CurrentUserAccountId)
            throw new ValidationException(new ValidationFailure("Id", $"{typeof(T)} does not exist."));
    }
}