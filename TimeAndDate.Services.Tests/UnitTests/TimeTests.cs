using System;
using NUnit.Framework;
using System.Collections.Generic;
using TimeAndDate.Services.DataTypes.Time;


namespace TimeAndDate.Services.Tests.UnitTests
{
    [TestFixture()]
    public class TimeTests
    {
        [Test]
        public void DateTime_From_String_Should_ReturnCorrectDate()
        {
            // Arrange		
            var iso8601 = "2011-03-27T01:00:00";
            var expected_date = new TADDateTime(2011, 3, 27, 1, 0, 0);
            var result_date = new TADDateTime(iso8601);

            Assert.IsTrue(expected_date.Equals(result_date));
        }
    }
}

