using Kalendario.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Kalendario.Persistence;

public class KalendarioDbContextFactory: DesignTimeDbContextFactoryBase<KalendarioDbContext>
{
    protected override KalendarioDbContext CreateNewInstance(DbContextOptions<KalendarioDbContext> options)
    {
        return new KalendarioDbContext(options);
    }
}