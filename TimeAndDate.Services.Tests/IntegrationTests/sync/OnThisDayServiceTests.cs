
#define DISABLE_OPTIONS
//#undef DISABLE_OPTIONS

using System;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Linq;
using TimeAndDate.Services.Tests;
using TimeAndDate.Services.DataTypes.OnThisDay;
using TimeAndDate.Services.DataTypes.Time;

namespace TimeAndDate.Services.Tests.IntegrationTests
{
	// TBA: Not released yet.
	/*
	[TestFixture()]
	public class OnThisDayServiceTests
	{
		[Test()]
		public void Calling_OnThisDayService_With_A_Specific_Date_Should_ReturnEvents ()
		{
			// Arrange
			var month = 5;
			var day = 24;

			// Act
			var onthisdayService = new OnThisDayService (Config.AccessKey, Config.SecretKey);
			var result = onthisdayService.EventsOnThisDay (month, day);
			var firstEvent = result.Events.FirstOrDefault ();
			var firstBirth = result.Births.FirstOrDefault ();
			var firstDeath = result.Deaths.FirstOrDefault ();
			// Assert
			Assert.IsNotNull (firstEvent);
			Assert.AreEqual (firstEvent.Name, "The first Eurovision Song Contest is held");
			Assert.AreEqual (firstEvent.Id, 1340);
			Assert.AreEqual (firstEvent.Date.DateTime.Month, 5);
			Assert.AreEqual (firstEvent.Date.DateTime.Day, 24);

			Assert.IsNotNull (firstBirth);
			Assert.AreEqual (firstBirth.Name.First, "Bob");
			Assert.AreEqual (firstBirth.Name.Middle, "");
			Assert.AreEqual (firstBirth.Name.Last, "Dylan");
			Assert.AreEqual (firstBirth.Id, 1414);
			Assert.AreEqual (firstBirth.BirthDate.DateTime.Month, 5);
			Assert.AreEqual (firstBirth.BirthDate.DateTime.Day, 24);

			Assert.IsNotNull (firstDeath);
			Assert.AreEqual (firstDeath.Name.First, "Nicolaus");
			Assert.AreEqual (firstDeath.Name.Middle, "");
			Assert.AreEqual (firstDeath.Name.Last, "Copernicus");
			Assert.AreEqual (firstDeath.Id, 53695);
			Assert.AreEqual (firstDeath.DeathDate.DateTime.Month, 5);
			Assert.AreEqual (firstDeath.DeathDate.DateTime.Day, 24);
		}

		[Test()]
		public void Calling_OnThisDayService_With_Events_Only_Should_Return_Events_Only ()
		{
			// Arrange
			var month = 5;
			var day = 24;

			// Act
			var onthisdayService = new OnThisDayService (Config.AccessKey, Config.SecretKey);
			onthisdayService.Types = EventType.Events;
			var result = onthisdayService.EventsOnThisDay (month, day);

			// Assert
			Assert.Greater (result.Events.Count, 0);
			Assert.AreEqual (result.Births.Count, 0);
			Assert.AreEqual (result.Deaths.Count, 0);
		}

		[Test()]
		public void Calling_OnThisDayService_With_Births_Only_Should_Return_Births_Only ()
		{
			// Arrange
			var month = 5;
			var day = 24;

			// Act
			var onthisdayService = new OnThisDayService (Config.AccessKey, Config.SecretKey);
			onthisdayService.Types = EventType.Births;
			var result = onthisdayService.EventsOnThisDay (month, day);

			// Assert
			Assert.AreEqual (result.Events.Count, 0);
			Assert.Greater (result.Births.Count, 0);
			Assert.AreEqual (result.Deaths.Count, 0);
		}

		[Test()]
		public void Calling_OnThisDayService_With_Deaths_Only_Should_Return_Deaths_Only ()
		{
			// Arrange
			var month = 5;
			var day = 24;

			// Act
			var onthisdayService = new OnThisDayService (Config.AccessKey, Config.SecretKey);
			onthisdayService.Types = EventType.Deaths;
			var result = onthisdayService.EventsOnThisDay (month, day);

			// Assert
			Assert.AreEqual (result.Events.Count, 0);
			Assert.AreEqual (result.Births.Count, 0);
			Assert.Greater (result.Deaths.Count, 0);
		}
	}
	*/
}

