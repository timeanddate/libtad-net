using NUnit.Framework;
using System;
using TimeAndDate.Services.DataTypes.Places;
using System.Linq;
using System.Globalization;
using TimeAndDate.Services.DataTypes.BusinessDays;

namespace TimeAndDate.Services.Tests.IntegrationTests
{
	[TestFixture()]
	public class BusinessDateServiceTests
	{
		[Test()]
		public void Calling_BusinessDateServiceWithAdd_ReturnsCorrectBusinessDate()
		{
			// Arrange
			var location = new LocationId("usa/anchorage");
			var startDate = new DateTime(2017, 12, 01);
			var days = 31;

			// Act
			var svc = new BusinessDateService(Config.AccessKey, Config.SecretKey);
			var res = svc.AddDays(startDate, days, location);
			var period = res.Periods.FirstOrDefault();

			// Assert
			Assert.AreEqual("Anchorage", res.Geography.Name);

			VerifyFirstPeriod(period, "add");
		}

		[Test()]
		public void Calling_BusinessDateServiceWithAdd_AndMultipleDays_ReturnsCorrectBusinessDate()
		{
			// Arrange
			var location = new LocationId("usa/anchorage");
			var startDate = new DateTime(2017, 12, 01);
			var days = new int[] { 31, 41 };

			// Act
			var svc = new BusinessDateService(Config.AccessKey, Config.SecretKey);
			var res = svc.AddDays(startDate, days, location);
			var period = res.Periods.FirstOrDefault();

			// Assert
			Assert.AreEqual("Anchorage", res.Geography.Name);
			VerifyFirstPeriod(res.Periods.ElementAt(0), "add");
			VerifySecondPeriod(res.Periods.ElementAt(1), "add");
		}

		[Test()]
		public void Calling_BusinessDateServiceWithAdd_AndInclude_ReturnsCorrectBusinessDate()
		{
			// Arrange
			var location = new LocationId("usa/anchorage");
			var startDate = new DateTime(2017, 12, 01);
			var days = 31;

			// Act
			var svc = new BusinessDateService(Config.AccessKey, Config.SecretKey);
			svc.IncludeDays = true;
			var res = svc.AddDays(startDate, days, location);
			var period = res.Periods.FirstOrDefault();

			// Assert
			Assert.AreEqual("Anchorage", res.Geography.Name);
			Assert.AreEqual(31, period.IncludedDays);
			Assert.AreEqual(92, period.CalendarDays);
			Assert.AreEqual(61, period.SkippedDays);

			Assert.AreEqual(0, period.Weekdays.MondayCount);
			Assert.AreEqual(0, period.Weekdays.TuesdayCount);
			Assert.AreEqual(0, period.Weekdays.WednesdayCount);
			Assert.AreEqual(0, period.Weekdays.ThursdayCount);
			Assert.AreEqual(0, period.Weekdays.FridayCount);
			Assert.AreEqual(14, period.Weekdays.SaturdayCount);
			Assert.AreEqual(13, period.Weekdays.SundayCount);
		}

		[Test()]
		public void Calling_BusinessDateServiceWithAdd_AndRepeat_ReturnsCorrectBusinessDate()
		{
			// Arrange
			var location = new LocationId("usa/anchorage");
			var startDate = new DateTime(2017, 12, 01);
			var days = 31;

			// Act
			var svc = new BusinessDateService(Config.AccessKey, Config.SecretKey);
			svc.Repeat = 5;
			var res = svc.AddDays(startDate, days, location);

			// Assert
			Assert.AreEqual("Anchorage", res.Geography.Name);
			Assert.AreEqual(5, res.Periods.Count);

			Assert.AreEqual("2017-12-01", res.Periods.ElementAt(0).StartDate.ISO);
			Assert.AreEqual("2018-01-18", res.Periods.ElementAt(0).EndDate.ISO);

			Assert.AreEqual("2018-01-18", res.Periods.ElementAt(1).StartDate.ISO);
			Assert.AreEqual("2018-03-05", res.Periods.ElementAt(1).EndDate.ISO);

			Assert.AreEqual("2018-03-05", res.Periods.ElementAt(2).StartDate.ISO);
			Assert.AreEqual("2018-04-18", res.Periods.ElementAt(2).EndDate.ISO);

			Assert.AreEqual("2018-04-18", res.Periods.ElementAt(3).StartDate.ISO);
			Assert.AreEqual("2018-06-01", res.Periods.ElementAt(3).EndDate.ISO);

			Assert.AreEqual("2018-06-01", res.Periods.ElementAt(4).StartDate.ISO);
			Assert.AreEqual("2018-07-17", res.Periods.ElementAt(4).EndDate.ISO);
		}

		[Test()]
		public void Calling_BusinessDateServiceWithAdd_AndInvalidRepeat_ThrowsException()
		{
			// Arrange
			var location = new LocationId("usa/anchorage");
			var startDate = new DateTime(2017, 12, 01);
			var days = new int[] { 31, 41 };

			// Act
			var svc = new BusinessDateService(Config.AccessKey, Config.SecretKey);
			svc.Repeat = 5;

			// Assert
			Assert.That(() => svc.AddDays(startDate, days, location),
						Throws.TypeOf<ArgumentException>());
		}

		[Test()]
		public void Calling_BusinessDateServiceWithAdd_AndCountryState_ReturnsCorrectBusinessDate()
		{
			// Arrange
			var startDate = new DateTime(2017, 12, 01);
			var days = 31;

			// Act
			var svc = new BusinessDateService(Config.AccessKey, Config.SecretKey);
			var res = svc.AddDays(startDate, days, "us", "us-nv");
			var period = res.Periods.FirstOrDefault();

			// Assert
			Assert.AreEqual("Nevada", res.Geography.State);
			Assert.AreEqual("United States", res.Geography.Country.Name);
		}

		[Test()]
		public void Calling_BusinessDateServiceWithAdd_AndMultipleDays_AndFilter_ReturnsCorrectBusinessDate()
		{
			// Arrange
			var location = new LocationId("usa/anchorage");
			var startDate = new DateTime(2017, 12, 01);
			var days = new int[] { 31, 41 };

			// Act
			var svc = new BusinessDateService(Config.AccessKey, Config.SecretKey);
			svc.Filter = BusinessDaysFilterType.Monday | BusinessDaysFilterType.Tuesday;

			var res = svc.AddDays(startDate, days, location);

			// Assert
			//Assert.IsTrue(res.Periods.All((x) => x.))
			Assert.IsTrue(res.Periods.All(x => x.Weekdays.MondayCount > 0));
			Assert.IsTrue(res.Periods.All(x => x.Weekdays.TuesdayCount > 0));
			Assert.IsTrue(res.Periods.All(x => x.Weekdays.WednesdayCount == 0));
			Assert.IsTrue(res.Periods.All(x => x.Weekdays.ThursdayCount == 0));
			Assert.IsTrue(res.Periods.All(x => x.Weekdays.FridayCount == 0));
			Assert.IsTrue(res.Periods.All(x => x.Weekdays.SaturdayCount == 0));
			Assert.IsTrue(res.Periods.All(x => x.Weekdays.SundayCount == 0));
		}

		[Test()]
		public void Calling_BusinessDateServiceWithSub_ReturnsCorrectBusinessDate()
		{
			// Arrange
			var location = new LocationId("usa/anchorage");
			var startDate = new DateTime(2018, 2, 1);
			var days = 31;

			// Act
			var svc = new BusinessDateService(Config.AccessKey, Config.SecretKey);
			var res = svc.SubtractDays(startDate, days, location);
			var period = res.Periods.FirstOrDefault();

			// Assert
			Assert.AreEqual("Anchorage", res.Geography.Name);

			VerifyFirstPeriod(period, "sub");
		}

		[Test()]
		public void Calling_BusinessDateServiceWithSub_AndMultipleDays_ReturnsCorrectBusinessDate()
		{
			// Arrange
			var location = new LocationId("usa/anchorage");
			var startDate = new DateTime(2018, 2, 1);
			var days = new int[] { 31, 41 };

			// Act
			var svc = new BusinessDateService(Config.AccessKey, Config.SecretKey);
			var res = svc.SubtractDays(startDate, days, location);
			var period = res.Periods.FirstOrDefault();

			// Assert
			Assert.AreEqual("Anchorage", res.Geography.Name);
			VerifyFirstPeriod(res.Periods.ElementAt(0), "sub");
			VerifySecondPeriod(res.Periods.ElementAt(1), "sub");
		}

		private void VerifyFirstPeriod(Period period, string op)
		{
			Assert.AreEqual(31, period.IncludedDays);
			Assert.AreEqual(48, period.CalendarDays);
			Assert.AreEqual(17, period.SkippedDays);

			if (op == "add")
			{
				Assert.AreEqual("2017-12-01", period.StartDate.ISO);
				Assert.AreEqual("2018-01-18", period.EndDate.ISO);
			}
			else if (op == "sub")
			{
				Assert.AreEqual("2017-12-15", period.EndDate.ISO);
				Assert.AreEqual("2018-02-01", period.StartDate.ISO);
			}

			Assert.AreEqual(0, period.Weekdays.MondayCount);
			Assert.AreEqual(0, period.Weekdays.TuesdayCount);
			Assert.AreEqual(0, period.Weekdays.WednesdayCount);
			Assert.AreEqual(0, period.Weekdays.ThursdayCount);
			Assert.AreEqual(0, period.Weekdays.FridayCount);
			Assert.AreEqual(7, period.Weekdays.SaturdayCount);
			Assert.AreEqual(7, period.Weekdays.SundayCount);

			Assert.IsNotEmpty(period.Holidays);
		}

		private void VerifySecondPeriod(Period period, string op)
		{
			Assert.AreEqual(41, period.IncludedDays);
			Assert.AreEqual(14, period.CalendarDays);
			Assert.AreEqual(4, period.SkippedDays);

			if (op == "add")
			{
				Assert.AreEqual("2018-01-18", period.StartDate.ISO);
				Assert.AreEqual("2018-02-01", period.EndDate.ISO);
			}
			else if (op == "sub")
			{
				Assert.AreEqual("2017-12-01", period.EndDate.ISO);
				Assert.AreEqual("2017-12-15", period.StartDate.ISO);
			}

			Assert.AreEqual(0, period.Weekdays.MondayCount);
			Assert.AreEqual(0, period.Weekdays.TuesdayCount);
			Assert.AreEqual(0, period.Weekdays.WednesdayCount);
			Assert.AreEqual(0, period.Weekdays.ThursdayCount);
			Assert.AreEqual(0, period.Weekdays.FridayCount);
			Assert.AreEqual(2, period.Weekdays.SaturdayCount);
			Assert.AreEqual(2, period.Weekdays.SundayCount);

			Assert.IsEmpty(period.Holidays);
		}
	}
}
