#define DISABLE_OPTIONS
//#undef DISABLE_OPTIONS

using System;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Linq;
using TimeAndDate.Services.Tests;
using TimeAndDate.Services.DataTypes.Tides;
using TimeAndDate.Services.DataTypes.Time;
using TimeAndDate.Services.DataTypes.Places;

namespace TimeAndDate.Services.Tests.IntegrationTests
{

	[TestFixture()]
	public class TidesServiceTestsAsync
	{
		[Test()]
		public async Task Calling_TidesService_For_A_Specific_Date_Interval_Should_ReturnResults ()
		{
			// Arrange
			var location = new LocationId("norway/stavanger");
			var startDate = new DateTime(2021, 9, 8);
			var endDate = new DateTime(2021, 9, 8, 23, 59, 59);

			// Act
			var tidesService = new TidesService(Config.AccessKey, Config.SecretKey);
			tidesService.StartDate = startDate;
			tidesService.EndDate = endDate;
			var result = await tidesService.GetTidalDataAsync(location);

			// Assert
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual("Stavanger", result[0].Source.Name);
			Assert.AreEqual("Reference Station", result[0].Source.Type);
			Assert.AreEqual("norway/stavanger", result[0].MatchParam);
			
			foreach (Tide tide in result[0].Result)
			{
				Assert.AreEqual(9, tide.Time.DateTime.Month);
				Assert.AreEqual(8, tide.Time.DateTime.Day);
				Assert.That(tide.Phase == TidalPhase.High || tide.Phase == TidalPhase.Low);
			}
		}

		[Test()]
		public async Task Calling_TidesService_Without_OnlyHighLow_Shoud_Return_Intermediate_Points ()
		{
			// Arrange
			var location = new LocationId("norway/stavanger");

			// Act
			var tidesService = new TidesService(Config.AccessKey, Config.SecretKey);
			tidesService.OnlyHighLow = false;
			var result = await tidesService.GetTidalDataAsync(location);

			// Assert
			Assert.AreEqual(1, result.Count);
			Assert.That(
				result[0].Result.Any(x => (x.Phase == TidalPhase.Ebb) || (x.Phase == TidalPhase.Flood))
			);
		}

		[Test()]
		public async Task Requesting_Tidal_Data_For_A_Subordinate_Station_Should_Resolve_To_A_Subordinate_Station ()
		{
			// Arrange
			var location = new LocationId("norway/sola");

			// Act
			var tidesService = new TidesService(Config.AccessKey, Config.SecretKey);
			tidesService.Subordinate = true;
			var result = await tidesService.GetTidalDataAsync(location);

			// Assert
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual("Subordinate Station", result[0].Source.Type);
		}
		
		[Test()]
		public async Task Requesting_Tidal_Data_In_Intervals_of_60_minutes ()
		{
			// Arrange
			var location = new LocationId("norway/stavanger");
			
			// Act
			var tidesService = new TidesService(Config.AccessKey, Config.SecretKey);
			tidesService.Interval = 60;
			tidesService.OnlyHighLow = false;
			var result = await tidesService.GetTidalDataAsync(location);

			// Assert
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(24, result[0].Result.Count);
		}

		[Test()]
		public async Task Requesting_Tidal_Data_In_Intervals_of_30_minutes ()
		{
			// Arrange
			var location = new LocationId("norway/stavanger");
			
			// Act
			var tidesService = new TidesService(Config.AccessKey, Config.SecretKey);
			tidesService.Interval = 30;
			tidesService.OnlyHighLow = false;
			var result = await tidesService.GetTidalDataAsync(location);

			// Assert
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(48, result[0].Result.Count);
		}

		[Test()]
		public async Task Requesting_Tidal_Data_In_Intervals_of_15_minutes ()
		{
			// Arrange
			var location = new LocationId("norway/stavanger");
			
			// Act
			var tidesService = new TidesService(Config.AccessKey, Config.SecretKey);
			tidesService.Interval = 15;
			tidesService.OnlyHighLow = false;
			var result = await tidesService.GetTidalDataAsync(location);

			// Assert
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(96, result[0].Result.Count);
		}

		[Test()]
		public async Task Requesting_Tidal_Data_In_Intervals_of_5_minutes ()
		{
			// Arrange
			var location = new LocationId("norway/stavanger");
			
			// Act
			var tidesService = new TidesService(Config.AccessKey, Config.SecretKey);
			tidesService.Interval = 5;
			tidesService.OnlyHighLow = false;
			var result = await tidesService.GetTidalDataAsync(location);

			// Assert
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(288, result[0].Result.Count);
		}

	}
}

