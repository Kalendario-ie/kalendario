using System;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Test.Common;
using Kalendario.Common;
using Kalendario.Core.Domain;
using Kalendario.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Kalendario.Application.UnitTests.Common;

public class KalendarioContextFactory
{
    public static KalendarioDbContext Create(ICurrentUserService currentUserService, IDateTime dateTime)
    {
        var options = new DbContextOptionsBuilder<KalendarioDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new KalendarioDbContext(options, currentUserService, dateTime);
    }

    public static void Destroy(KalendarioDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Dispose();
    }

    public static void PopulateDb(KalendarioDbContext context)
    {
        context.AddRange(new Account()
        {
            Id = Constants.CurrentUserAccountId,
            Name = nameof(Constants.CurrentUserAccountId)
        }, new Account
        {
            Id = Constants.RandomAccountId,
            Name = nameof(Constants.RandomAccountId)
        });

        for (var i = 0; i < 5; i++)
        {
            context.Employees.Add(new Employee { AccountId = Constants.CurrentUserAccountId});
            context.Services.Add(new Service() { AccountId = Constants.CurrentUserAccountId});
            context.Customers.Add(new Customer() { AccountId = Constants.CurrentUserAccountId});
                
            context.Employees.Add(new Employee { AccountId = Constants.RandomAccountId});
            context.Services.Add(new Service() { AccountId = Constants.RandomAccountId});
            context.Customers.Add(new Customer() { AccountId = Constants.RandomAccountId});
        }

        context.SaveChanges();
    }
}