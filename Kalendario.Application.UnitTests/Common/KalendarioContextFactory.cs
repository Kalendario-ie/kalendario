using System;
using Kalendario.Application.UnitTests.Mocks;
using Kalendario.Core.Domain;
using Kalendario.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Kalendario.Application.Test.Common
{
    public class KalendarioContextFactory
    {
        public static KalendarioDbContext Create()
        {
            var options = new DbContextOptionsBuilder<KalendarioDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new KalendarioDbContext(options, new CurrentUserMock(), new DatetimeMock());
            PopulateDb(context);
            return context;
        }

        public static void Destroy(KalendarioDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        private static void PopulateDb(KalendarioDbContext context)
        {
            context.AddRange(new Account()
            {
                Id = Constants.CurrentUserAccountId
            }, new Account
            {
                Id = Constants.RandomAccountId
            });

            for (var i = 0; i < 5; i++)
            {
                context.Employees.Add(new Employee { AccountId = Constants.CurrentUserAccountId});
                context.Employees.Add(new Employee { AccountId = Constants.RandomAccountId});
                
                context.Services.Add(new Service() { AccountId = Constants.CurrentUserAccountId});
                context.Services.Add(new Service() { AccountId = Constants.RandomAccountId});
            }

            context.SaveChanges();
        }
    }
}