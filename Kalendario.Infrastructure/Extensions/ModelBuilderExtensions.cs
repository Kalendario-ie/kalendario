using System.Linq.Expressions;
using Kalendario.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Kalendario.Infrastructure.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyGlobalFilters<TInterface>(this ModelBuilder builder,
        Expression<Func<TInterface, bool>> expression)
    {
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (entityType.ClrType.GetInterface(typeof(TInterface).Name) == null) continue;
            var parameterExpression = Expression.Parameter(entityType.ClrType);
            var body = ReplacingExpressionVisitor
                .Replace(expression.Parameters.Single(), parameterExpression, expression.Body);
            builder.Entity(entityType.ClrType)
                .HasQueryFilter(Expression.Lambda(body, parameterExpression));
        }
    }
        public static void ApplyGlobalConfiguration(this ModelBuilder builder)
        {
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (entityType.ClrType.GetInterface(nameof(AccountEntity)) == null) continue;
                builder.Entity(entityType.ClrType).HasOne(typeof(Account)).WithMany().HasForeignKey();
            }
        }

    
}