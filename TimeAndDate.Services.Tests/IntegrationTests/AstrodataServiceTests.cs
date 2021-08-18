#define DISABLE_OPTIONS
//#undef DISABLE_OPTIONS

using System;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;
using TimeAndDate.Services.Tests;
using TimeAndDate.Services.DataTypes.Astro;
using TimeAndDate.Services.DataTypes.Places;
using TimeAndDate.Services.DataTypes.Time;

namespace TimeAndDate.Services.Tests.IntegrationTests
{
	[TestFixture()]
	public class AstrodataServiceTests
	{
		[Test()]
		public async Task Calling_AstrodataService_For_Every_Object_Returns_The_Correct_Object ()
		{
			// Arrange
			var astrodataService = new AstrodataService (Config.AccessKey, Config.SecretKey);
			var datetime = new TADDateTime (2020, 3, 5);

			foreach (AstronomyObjectType objectType in Enum.GetValues(typeof (AstronomyObjectType))) {
				// Act
				var result = await astrodataService.GetAstroData(objectType, new LocationId(3), datetime);
				var firstResult = result.FirstOrDefault ();

				// Assert
				Assert.AreEqual (objectType, firstResult.Objects[0].Name);
			}
		}

		[Test()]
		public async Task Calling_AstrodataService_With_Multiple_Intervals_Returns_Expected_Amount_Of_Results ()
		{
			// Arrange
			var astrodataService = new AstrodataService (Config.AccessKey, Config.SecretKey);
			List<TADDateTime> date_list = new List<TADDateTime> ();
			for (int i = 1; i < 6; i++)
			{
				var date = new TADDateTime (2020 + i, i * 2, i * 5);
				date_list.Add (date);
			}

			// Act
			var result = await astrodataService.GetAstroData(AstronomyObjectType.Moon, new LocationId(3), date_list);

			// Assert
			Assert.AreEqual (5, result[0].Objects[0].Result.Count);
		}

		[Test()]
		public async Task Calling_AstrodataService_With_Language_Returns_Expected_Country_Name ()
		{
			// Arrange
			var astrodataService = new AstrodataService (Config.AccessKey, Config.SecretKey);
			var time = new TADDateTime (2020, 3, 15);
			astrodataService.Language = "es";

			// Act
			var result = await astrodataService.GetAstroData(AstronomyObjectType.Sun, new LocationId(187), time);

			// Assert
			Assert.AreEqual ("Noruega", result[0].Geography.Country.Name);
		}

		[Test()]
		public async Task Calling_AstrodataService_Returns_The_Expected_Geographical_Information ()
		{
			// Arrange
			var astrodataService = new AstrodataService (Config.AccessKey, Config.SecretKey);
			astrodataService.IncludeCoordinates = true;
			var time = new TADDateTime (2020, 3, 15);

			// Act
			var result = (await astrodataService.GetAstroData(AstronomyObjectType.Sun, new LocationId(3), time))[0].Geography;

			// Assert
			Assert.AreEqual ("Acapulco", result.Name);
			Assert.AreEqual ("Guerrero", result.State);
			Assert.AreEqual ("mx", result.Country.Id);
			Assert.AreEqual ("Mexico", result.Country.Name);
			Assert.AreEqual (16.860, result.Coordinates.Latitude);
			Assert.AreEqual (-99.877, result.Coordinates.Longitude);
		}

		[Test()]
		public async Task Calling_AstrodataService_With_Multiple_Object_Returns_Expected_Result_Count ()
		{
			// Arrange
			var astrodataService = new AstrodataService (Config.AccessKey, Config.SecretKey);
			var time = new TADDateTime (2020, 3, 15);
			var objectType = AstronomyObjectType.Sun | AstronomyObjectType.Moon;

			// Act
			var result = await astrodataService.GetAstroData(objectType, new LocationId(3), time);

			// Assert
			Assert.AreEqual (2, result[0].Objects.Count);
			Assert.AreEqual (AstronomyObjectType.Sun, result[0].Objects[0].Name);
			Assert.AreEqual (AstronomyObjectType.Moon, result[0].Objects[1].Name);
		}

		[Test()]
		public async Task Results_With_And_Without_Localtime_Should_Return_Inequal_Values ()
		{
			// Arrange
			var astrodataService = new AstrodataService (Config.AccessKey, Config.SecretKey);
			var time = new TADDateTime (2020, 3, 5);

			// Act
			var result = (await astrodataService.GetAstroData(AstronomyObjectType.Moon, new LocationId(3), time))[0].Objects[0].Result[0];
			astrodataService.LocalTime = true;
			var result_local = (await astrodataService.GetAstroData(AstronomyObjectType.Moon, new LocationId(3), time))[0].Objects[0].Result[0];

			// Assert
			Assert.AreNotEqual (result.Azimuth, result_local.Azimuth);
			Assert.AreNotEqual (result.Altitude, result_local.Altitude);
			Assert.AreNotEqual (result.Distance, result_local.Distance);
			Assert.AreNotEqual (result.Illuminated, result_local.Illuminated);
			Assert.AreNotEqual (result.Posangle, result_local.Posangle);
		}
	}
}

