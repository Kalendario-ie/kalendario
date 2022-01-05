using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Kalendario.Api;
using Kalendario.Application.Authorization;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.IntegrationTests.Common;
using Kalendario.Core.Entities;
using Kalendario.Core.Infrastructure;
using Kalendario.Infrastructure.Extensions;
using Kalendario.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using Npgsql;
using NUnit.Framework;
using Respawn;

namespace Kalendario.Application.IntegrationTests;

[SetUpFixture]
public class Testing
{
    private static IConfigurationRoot _configuration = null!;
    private static IServiceScopeFactory _scopeFactory = null!;
    private static Checkpoint _checkpoint = null!;

    public static Mock<ICurrentUserService> currentUserServiceMock =
        TestMocks.CurrentUserServiceMock(Guid.Empty, Guid.Empty, false);

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile("appsettings.Local.json", true, true)
            .AddJsonFile("appsettings.Development.json", true, true)
            .AddEnvironmentVariables();

        _configuration = builder.Build();
        var webHostEnvironment = Mock.Of<IWebHostEnvironment>(w =>
            w.EnvironmentName == "Development" &&
            w.ApplicationName == "Kalendario.Api");

        var startup = new Startup(_configuration, webHostEnvironment);

        var services = new ServiceCollection();

        services.AddSingleton(webHostEnvironment);

        services.AddLogging();

        startup.ConfigureServices(services);

        // Replace service registration for ICurrentUserService
        // Remove existing registration
        var currentUserServiceDescriptor = services.FirstOrDefault(d =>
            d.ServiceType == typeof(ICurrentUserService));

        if (currentUserServiceDescriptor != null)
        {
            services.Remove(currentUserServiceDescriptor);
        }

        // Register testing version
        services.AddTransient(provider => currentUserServiceMock.Object);

        _scopeFactory = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>();

        _checkpoint = new Checkpoint
        {
            TablesToIgnore = new[] {"__EFMigrationsHistory"},
            SchemasToInclude = new[]
            {
                "public"
            },
            DbAdapter = DbAdapter.Postgres
        };

        EnsureDatabase();
    }

    private static void EnsureDatabase()
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Database.Migrate();
    }

    public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        return await mediator.Send(request);
    }

    public static void RunAsAnonymousUser()
    {
        currentUserServiceMock.Setup(m => m.UserId)
            .Returns(String.Empty);
        currentUserServiceMock.Setup(m => m.IsAuthenticated)
            .Returns(false);
    }

    public static async Task<string> RunAsDefaultUserAsync()
    {
        return await RunAsUserAsync("test@local", "Testing1234!", Array.Empty<string>());
    }

    public static async Task<string> RunAsAdministratorAsync(Type entityClass, string actionType,
        Guid accountId = default)
    {
        return await RunAsUserAsync("administrator@local", "Administrator1234!",
            new[] {AuthorizationHelper.RoleName(entityClass, actionType)}, accountId);
    }

    public static async Task<string> RunAsUserAsync(string userName, string password, string[] roles,
        Guid accountId = default)
    {
        using var scope = _scopeFactory.CreateScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var user = new ApplicationUser {UserName = userName, Email = userName};

        if (accountId != default)
        {
            user.AccountId = accountId;
        }

        var result = await userManager.CreateAsync(user, password);

        if (roles.Any())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }

            await userManager.AddToRolesAsync(user, roles);
        }

        if (result.Succeeded)
        {
            currentUserServiceMock.Setup(m => m.UserId)
                .Returns(user.Id);
            currentUserServiceMock.Setup(m => m.IsAuthenticated)
                .Returns(true);
            currentUserServiceMock.Setup(m => m.AccountId)
                .Returns(accountId);
            return user.Id;
        }

        var errors = string.Join(Environment.NewLine, result.ToApplicationResult().Errors);

        throw new Exception($"Unable to create {userName}.{Environment.NewLine}{errors}");
    }

    public static async Task ResetState()
    {
        await ResetDatabase();
        await SeedDatabase();
        ResetCurrentUserService();
    }

    public static async Task<TEntity?> FindAsync<TEntity>(params object[] keyValues)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.FindAsync<TEntity>(keyValues);
    }

    public static async Task<Guid> AddAsync<TEntity>(TEntity entity)
        where TEntity : BaseEntity
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Add(entity);

        await context.SaveChangesAsync();

        return entity.Id;
    }

    public static async Task<int> CountAsync<TEntity>() where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.Set<TEntity>().CountAsync();
    }

    public static async Task<List<TEntity>> WhereAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.Set<TEntity>().Where(predicate).ToListAsync();
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
    }

    private static async Task ResetDatabase()
    {
        await using var conn = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        await conn.OpenAsync();
        await _checkpoint.Reset(conn);
    }
    private static async Task SeedDatabase()
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Accounts.AddRangeAsync(await DeserializeFile<Account>("Accounts.json"));
        await context.SaveChangesAsync(CancellationToken.None);
    }

    private static void ResetCurrentUserService()
    {
        currentUserServiceMock.Setup(m => m.UserId)
            .Returns(string.Empty);
        currentUserServiceMock.Setup(m => m.IsAuthenticated)
            .Returns(false);
    }

    private static async Task<List<T>> DeserializeFile<T>(string fileName)
    {
        string accountsJson = await File.ReadAllTextAsync(@"Seed" + Path.DirectorySeparatorChar + fileName);
        return JsonConvert.DeserializeObject<List<T>>(accountsJson)!;
    }
}