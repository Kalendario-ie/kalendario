using System;
using AutoMapper;
using Kalendario.Application.Common.Mappings;
using Kalendario.Application.Test.Common;
using Kalendario.Persistence;

namespace Kalendario.Application.UnitTests.Common
{
    public class QueryTestFixture : IDisposable
    {
        public KalendarioDbContext Context { get; }

        public IMapper Mapper { get; }

        public QueryTestFixture()
        {
            Context = KalendarioContextFactory.Create();

            var configurationProvider = new MapperConfiguration(cfg => { cfg.AddProfile<MappingProfile>(); });

            Mapper = configurationProvider.CreateMapper();
        }

        public void Dispose() => KalendarioContextFactory.Destroy(Context);
    }
}