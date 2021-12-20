using System;
using Kalendario.Common;

namespace Kalendario.Application.UnitTests.Mocks
{
    public class DatetimeMock : IDateTime
    {
        public DatetimeMock()
        {
            Now = DateTime.Now;
        }

        public DateTime Now { get; }
    }
}