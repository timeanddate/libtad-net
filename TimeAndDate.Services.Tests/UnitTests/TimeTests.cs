using System;
using NUnit.Framework;
using System.Collections.Generic;
using TimeAndDate.Services.DataTypes.Time;
using TimeAndDate.Services.Common;


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

	    // Act
            var result_date = new TADDateTime(iso8601);

	    // Assert
            Assert.IsTrue(expected_date.Equals(result_date));
        }

	[Test]
	public void DateTime_From_String_Without_Time_Should_ReturnCorrectDate()
	{
		// Arrange
		var iso = "2011-03-27";
		var expected_date = new TADDateTime(2011, 3, 27);

		// Act
		var result_date = new TADDateTime(iso);

		// Assert
		Assert.IsTrue(expected_date.Equals(result_date));
	}

	[Test]
	public void DateTime_From_Invalid_String_Should_Fail()
	{
		// Arrange
		var iso = "T-4rqwr-';,-35-63";

		// Act, Assert
		Assert.Throws<InvalidIsoStringException>(() => { new TADDateTime(iso); });

	}
    }
}

