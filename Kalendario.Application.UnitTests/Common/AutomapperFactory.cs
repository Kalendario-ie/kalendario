using AutoMapper;
using Kalendario.Application.Common.Mappings;

namespace Kalendario.Application.UnitTests.Common;

public static class AutomapperFactory
{
    public static IMapper Create()
    {
        var configurationProvider = new MapperConfiguration(cfg => { cfg.AddProfile<MappingProfile>(); });
        return configurationProvider.CreateMapper();
    }
}