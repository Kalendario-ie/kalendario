using System;
using AutoMapper;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Common.Mappings;
using Kalendario.Application.Test.Common;
using Kalendario.Common;
using Kalendario.Persistence;
using Moq;

namespace Kalendario.Application.UnitTests.Common;

public class QueryTestFixture : IDisposable
{
    public KalendarioDbContext Context { get; }

    public IMapper Mapper { get; }

    public readonly Mock<ICurrentUserService> CurrentUserServiceMock;

    public readonly Mock<IDateTime> DateTimeMock;

    public QueryTestFixture()
    {
        this.CurrentUserServiceMock = Mocks.CurrentUserServiceMock();
        this.DateTimeMock = Mocks.DatetimeMock();
        Context = KalendarioContextFactory.Create(CurrentUserServiceMock.Object, DateTimeMock.Object);
        KalendarioContextFactory.PopulateDb(Context);
        Mapper = AutomapperFactory.Create();
    }

    public void Dispose() => KalendarioContextFactory.Destroy(Context);
}