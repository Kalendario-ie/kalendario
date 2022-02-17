using System;
using System.Globalization;
using Kalendario.Api.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kalendario.Application.UnitTests.Converters;

[TestClass]
public class DateOnlyTypeConverterTest
{
    private DateOnlyTypeConverter _dateOnlyTypeConverter;

    [TestInitialize]
    public void Setup()
    {
        this._dateOnlyTypeConverter = new DateOnlyTypeConverter();
    }

    
    [TestMethod]
    public void ShouldAcceptDateWithZ()
    {
        var value = "2022-02-17";
        var date = this._dateOnlyTypeConverter.ConvertFrom(null, CultureInfo.InvariantCulture, value);
        Assert.IsInstanceOfType(date, typeof(DateOnly));
    }
}