using System;
using NUnit.Framework;
using TimeAndDate.Services.DataTypes.Places;
using TimeAndDate.Services.DataTypes.BusinessDays;

namespace TimeAndDate.Services.Tests.IntegrationTests
{
	[TestFixture()]
	public class BusinessDurationServiceTests
    {
		[Test()]
		public void Calling_BusinessDurationService_ReturnsCorrectBusinessDuration()
		{
			// Arrange
			var location = new LocationId("usa/anchorage");
			var startDate = new DateTime(2017, 12, 1);
			var endDate = new DateTime(2018, 1, 31);

			// Act
			var svc = new BusinessDurationService(Config.AccessKey, Config.SecretKey);
			var res = svc.GetDuration(startDate, endDate, location);

			// Assert
			Assert.AreEqual("Anchorage", res.Geography.Name);

			Assert.AreEqual(61, res.Period.CalendarDays);
			Assert.AreEqual(21, res.Period.SkippedDays);
			Assert.AreEqual(40, res.Period.IncludedDays);

			Assert.AreEqual(9, res.Period.Weekdays.SaturdayCount);
			Assert.AreEqual(9, res.Period.Weekdays.SundayCount);
			Assert.AreEqual(3, res.Period.Holidays.Count);
		}

		[Test()]
		public void Calling_BusinessDurationService_WithCountryState_ReturnsCorrectBusinessDuration()
		{
			// Arrange
			var startDate = new DateTime(2017, 12, 1);
			var endDate = new DateTime(2018, 1, 31);

			// Act
			var svc = new BusinessDurationService(Config.AccessKey, Config.SecretKey);
			var res = svc.GetDuration(startDate, endDate, "us", "us-nv");

			// Assert
			Assert.AreEqual("Nevada", res.Geography.State);
		}

		[Test()]
		public void Calling_BusinessDurationService_WithInvalidEndDate_ThrowsException()
		{
			// Arrange
			var startDate = new DateTime(2017, 12, 1);
			var endDate = new DateTime(2017, 11, 30);

			// Act
			var svc = new BusinessDurationService(Config.AccessKey, Config.SecretKey);

			// Assert
			Assert.That(() => svc.GetDuration(startDate, endDate, "us", "us-nv"),
				Throws.TypeOf<ArgumentException>());
		}

		[Test()]
		public void Calling_BusinessDurationService_WithInclude_ReturnsCorrectBusinessDuration()
		{
			// Arrange
			var location = new LocationId("usa/anchorage");
			var startDate = new DateTime(2017, 12, 1);
			var endDate = new DateTime(2018, 1, 31);

			// Act
			var svc = new BusinessDurationService(Config.AccessKey, Config.SecretKey);
			svc.IncludeDays = true;

			var res = svc.GetDuration(startDate, endDate, location);

			// Assert
			Assert.AreEqual("Anchorage", res.Geography.Name);

			Assert.AreEqual(61, res.Period.CalendarDays);
			Assert.AreEqual(40, res.Period.SkippedDays);
			Assert.AreEqual(21, res.Period.IncludedDays);

			Assert.AreEqual(9, res.Period.Weekdays.SaturdayCount);
			Assert.AreEqual(9, res.Period.Weekdays.SundayCount);
			Assert.AreEqual(3, res.Period.Holidays.Count);
		}

		[Test()]
		public void Calling_BusinessDurationService_WithIncludeLastDate_ReturnsCorrectBusinessDuration()
		{
			// Arrange
			var location = new LocationId("usa/anchorage");
			var startDate = new DateTime(2017, 12, 1);
			var endDate = new DateTime(2018, 1, 31);

			// Act
			var svc = new BusinessDurationService(Config.AccessKey, Config.SecretKey);
			svc.IncludeLastDate = true;

			var res = svc.GetDuration(startDate, endDate, location);

			// Assert
			Assert.AreEqual("Anchorage", res.Geography.Name);

			Assert.AreEqual(62, res.Period.CalendarDays);
			Assert.AreEqual(21, res.Period.SkippedDays);
			Assert.AreEqual(41, res.Period.IncludedDays);
		}

		[Test()]
		public void Calling_BusinessDurationService_WithFilter_ReturnsCorrectBusinessDuration()
		{
			// Arrange
			var location = new LocationId("usa/anchorage");
			var startDate = new DateTime(2017, 12, 1);
			var endDate = new DateTime(2018, 1, 31);

			// Act
			var svc = new BusinessDurationService(Config.AccessKey, Config.SecretKey);
			svc.Filter = BusinessDaysFilterType.Monday | BusinessDaysFilterType.Tuesday;

			var res = svc.GetDuration(startDate, endDate, location);

			// Assert
			Assert.AreEqual("Anchorage", res.Geography.Name);

			Assert.AreEqual(61, res.Period.CalendarDays);
			Assert.AreEqual(18, res.Period.SkippedDays);
			Assert.AreEqual(43, res.Period.IncludedDays);

			Assert.AreEqual(9, res.Period.Weekdays.MondayCount);
			Assert.AreEqual(9, res.Period.Weekdays.TuesdayCount);
			Assert.AreEqual(0, res.Period.Weekdays.WednesdayCount);
			Assert.AreEqual(0, res.Period.Weekdays.ThursdayCount);
			Assert.AreEqual(0, res.Period.Weekdays.FridayCount);
			Assert.AreEqual(0, res.Period.Weekdays.SaturdayCount);
			Assert.AreEqual(0, res.Period.Weekdays.SundayCount);
		}
	}
}
