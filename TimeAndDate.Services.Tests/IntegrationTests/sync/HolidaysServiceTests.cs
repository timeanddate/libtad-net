#define DISABLE_OPTIONS
//#undef DISABLE_OPTIONS

using System;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Linq;
using TimeAndDate.Services.Tests;
using TimeAndDate.Services.DataTypes.Holidays;
using TimeAndDate.Services.DataTypes.Time;

namespace TimeAndDate.Services.Tests.IntegrationTests
{
	[TestFixture()]
	public class HolidaysServiceTests
	{
		[Test()]
		public void Calling_HolidaysService_WithCountry_And_WithYear_Should_ReturnHolidays ()
		{
			// Arrange
			var country = "us";
			var year = 2014;
			var expectedHoliday = "New Year's Day";
			var expectedUid = "0007d600000007de";
            		var expectedUrl = new Uri("https://www.timeanddate.com/holidays/us/new-year-day");
			var expectedId = 2006;
            		var expectedDate = new TADDateTime(2014, 1, 1);
			
			// Act
			var holidaysService = new HolidaysService (Config.AccessKey, Config.SecretKey);			
			var result = holidaysService.HolidaysForCountry (country, year);
			var firstHoliday = result.FirstOrDefault ();
			// Assert
			Assert.IsNotNull (firstHoliday);
			Assert.AreEqual (expectedHoliday, firstHoliday.Name);
			Assert.AreEqual (expectedUid, firstHoliday.Uid);
			Assert.AreEqual (expectedUrl, firstHoliday.Url);
									
			Assert.AreEqual (expectedId, firstHoliday.Id);
            		Assert.IsTrue(expectedDate.Equals(firstHoliday.Date.DateTime));
		}
		
		[Test()]
		public void Calling_HolidaysService_WithCountry_And_WithYear_Should_ReturnHolidaysWithStates ()
		{
			// Arrange
            		var country = "gb-sct";
            		var year = 2015;
            		var expectedState = "Scotland";
			
			// Act
			var holidaysService = new HolidaysService (Config.AccessKey, Config.SecretKey);			
			var result = holidaysService.HolidaysForCountry (country, year);
			var holidaysWithSpecificStates = result.Where (x => x.States != null && x.States.Count() > 0).ToList ();
			var firstHoliday = holidaysWithSpecificStates.FirstOrDefault ();
			var firstState = firstHoliday.States.FirstOrDefault ();
			
			// Assert
			Assert.IsNotNull (firstHoliday);
			Assert.IsNotNull (firstHoliday.States);
			Assert.AreEqual (expectedState, firstState.Name);
		}
		
		[Test()]
		public void Calling_HolidaysService_WithSpecifiedTypes_Should_ReturnHolidaysWithCorrectTypes ()
		{
			// Arrange
			var country = "us";
			var year = 2014;
            		var expectedCount = 25;
			
			// Act
			var holidaysService = new HolidaysService (Config.AccessKey, Config.SecretKey);			
			holidaysService.Types = HolidayType.Christian | HolidayType.Buddhism;
			var result = holidaysService.HolidaysForCountry (country, year);
			var sample = result.FirstOrDefault ();
			
			// Assert
			Assert.AreEqual (expectedCount, result.Count);
			Assert.IsTrue (result.All (x => x.Types.Contains ("Christian") || x.Types.Contains ("Buddhism")));
			Assert.IsTrue (result.All (x => x.Country.Id == country));			
			Assert.IsNotNull (sample.Description);
			Assert.IsNotEmpty (sample.Description);
			Assert.IsNotNull (sample.Uid);
			Assert.IsNotEmpty (sample.Uid);
			Assert.IsNotNull (sample.Url);
			Assert.IsNotNull (sample.Url.AbsoluteUri);
			Assert.IsNotEmpty (sample.Url.AbsoluteUri);
		}
	}
}

