namespace CustomerApp.UnitTests
{
    using System;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class StringExtensionsTests
    {
        [Test]
        public void GivenValidEnumString_ThenEnumValueReturned()
        {
            "Monday".TryParse<DayOfWeek>().Should().Be((true, DayOfWeek.Monday));
            "Sunday".TryParse<DayOfWeek>().Should().Be((true, DayOfWeek.Sunday));
        }

        [Test]
        public void GivenInvalidEnumString_ThenResultIsFalseAndDefaultEnumValue()
        {
            "this is not an attribute target".TryParse<AttributeTargets>()
                .Should()
                .Be((false, default));
        }
    }
}