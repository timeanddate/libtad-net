using System;
using System.Linq;
using NUnit.Framework;
using TimeAndDate.Services.Tests;
using TimeAndDate.Services.DataTypes.Astro;
using System.Collections.Generic;
using TimeAndDate.Services.DataTypes.Places;

namespace TimeAndDate.Services.Tests.IntegrationTests
{
	[TestFixture()]
	public class AstronomyServiceTests
	{
		[Test()]
		public void Calling_AstronomyService_WithoutEnddate_ReturnsCorrectAstronomyInfo ()
		{
			// Arrange
			var startDate = new DateTime (2014, 1, 1);
			var type = AstronomyObjectType.Sun;
			var place = new LocationId ("usa/anchorage");
			
			// Act
			var service = new AstronomyService (Config.AccessKey, Config.SecretKey);
			var result = service.GetAstronomicalInfo (type, place, startDate);
			var anchorage = result.SingleOrDefault ();
			
			// Assert
			Assert.IsTrue (anchorage.Objects.All (y => y.Days.All (z => z.Date.HasValue)));
			Assert.IsTrue (anchorage.Objects.All (y => y.Days.All (z => z.Date.Value.Date >= startDate.Date)));
			Assert.AreEqual ("United States", anchorage.Geography.Country.Name);
			Assert.AreEqual ("us", anchorage.Geography.Country.Id);
			Assert.AreEqual (61.188m, anchorage.Geography.Coordinates.Latitude);
			Assert.AreEqual (-149.887m, anchorage.Geography.Coordinates.Longitude);
			Assert.IsTrue (anchorage.Objects.All (x => x.Days.All (y => y.Events.All (z => z.Azimuth > 0m))));
			Assert.AreEqual ("18", anchorage.Id);
		}
		
		[Test()]
		public void Calling_AstronomyService_WithEnddate_Should_ReturnCorrectAstronomyInfo ()
		{
			// Arrange
			var startDate = new DateTime (2014, 1, 1);
			var endDate = new DateTime (2014, 1, 30);
			var type = AstronomyObjectType.Moon;
			var place = new LocationId("usa/anchorage");
			
			// Act
			var service = new AstronomyService (Config.AccessKey, Config.SecretKey);
			var result = service.GetAstronomicalInfo (type, place, startDate);
			
			// Assert
			Assert.IsTrue (result.All (x => x.Objects.All (y => y.Days.All (z => z.Date.HasValue))));
			Assert.IsTrue (result.All (x => x.Objects.All (y => y.Days.All (z => z.Date.Value.Date >= startDate && z.Date.Value.Date <= endDate))));
			Assert.IsTrue (result.All (x => x.Geography.Country.Name == "United States"));
		}
		
		[Test()]
		public void Calling_AstronomyService_WithoutEnddate_And_OnlyOneInclusions_Should_ReturnCorrectAstronomyInfo ()
		{
			// Arrange
			var startDate = new DateTime (2014, 1, 1);
			var type = AstronomyObjectType.Sun;
			var place = new LocationId("norway/oslo");
			
			// Act
			var service = new AstronomyService (Config.AccessKey, Config.SecretKey);
			service.Types = AstronomyEventClass.Meridian;
			var result = service.GetAstronomicalInfo (type, place, startDate);
			
			// Assert
			Assert.IsTrue (result.All (x => x.Objects.All (y => y.Days.All (z => z.Date.HasValue))));
			Assert.IsTrue (result.All (x => x.Objects.All (y => y.Days.ToList().Count == 1)));
			Assert.IsTrue (result.All (x => x.Objects.All (y => y.Days.All (z => z.Date.Value.Date >= startDate.Date))));
			Assert.IsTrue (result.All (x => x.Objects.All (y => y.Days.All (z => z.Events.All (a => 
               a.Type == AstronomyEventCode.Meridian || a.Type == AstronomyEventCode.AntiMeridian
			)))));
			                                                                                  
			Assert.IsTrue (result.All (x => x.Geography.Country.Name == "Norway"));
		}
		
		[Test()]
		public void Calling_AstronomyService_WithEnddate_And_SeveralInclusions_Should_ReturnsCorrectAstronomyInfo ()
		{
			// Arrange
			var startDate = new DateTime (2014, 1, 1);
			var endDate = new DateTime (2014, 1, 30);
			var type = AstronomyObjectType.Sun;
			var place = new LocationId ("norway/oslo");
			
			// Act
			var service = new AstronomyService (Config.AccessKey, Config.SecretKey);
			service.Types = AstronomyEventClass.AstronomicalTwilight | AstronomyEventClass.NauticalTwilight;
			var result = service.GetAstronomicalInfo (type, place, startDate, endDate);
			
			Assert.IsTrue (result.All (x => x.Objects.All (y => y.Days.All (z => z.Date.HasValue))));
			Assert.IsTrue (result.All (x => x.Objects.All (y => y.Days.ToList().Count > 1)));
			Assert.IsTrue (result.All (x => x.Objects.All (y => y.Days.All (z => z.Date.Value.Date >= startDate.Date))));
			Assert.IsTrue (result.All (x => x.Objects.All (y => y.Days.All (z => z.Events.All (a => 
               a.Type == AstronomyEventCode.AstronomicalTwilightEnds || a.Type == AstronomyEventCode.AstronomicalTwilightStarts
			   || a.Type == AstronomyEventCode.NauticalTwilightStarts || a.Type == AstronomyEventCode.NauticalTwilightEnds
			)))));
		}
		
		[Test()]
		public void Calling_AstronomyService_WithoutLatLong_Should_ReturnsCorrectAstronomyInfo ()
		{
			// Arrange
			var startDate = new DateTime (2014, 1, 1);
			var endDate = new DateTime (2014, 1, 30);
			var type = AstronomyObjectType.Sun;
			var place = new LocationId("norway/oslo");
			
			// Act
			var service = new AstronomyService (Config.AccessKey, Config.SecretKey);
			service.IncludeCoordinates = false;
			var result = service.GetAstronomicalInfo (type, place, startDate, endDate);
			
			Assert.IsTrue (result.All (x => x.Objects.All (y => y.Days.All (z => z.Date.HasValue))));
			Assert.IsTrue (result.All (x => x.Objects.All (y => y.Days.ToList ().Count > 1)));
			Assert.IsTrue (result.All (x => x.Objects.All (y => y.Days.All (z => z.Date.Value.Date >= startDate.Date))));
			Assert.IsTrue (result.All (x => x.Geography.Coordinates == null));
		}
		
		[Test()]
		public void Calling_AstronomyService_WithLatLong_Should_ReturnCorrectAstronomyInfo ()
		{
			// Arrange
			var startDate = new DateTime (2014, 1, 1);
			var type = AstronomyObjectType.Sun;
			var place = new LocationId ("norway/oslo");
			
			// Act
			var service = new AstronomyService (Config.AccessKey, Config.SecretKey);
			service.IncludeCoordinates = true;
			var result = service.GetAstronomicalInfo (type, place, startDate);
			
			// Assert
			Assert.IsTrue (result.All (x => x.Objects.All (y => y.Days.All (z => z.Date.HasValue))));
			Assert.IsTrue (result.All (x => x.Objects.All (y => y.Days.All (z => z.Date.Value.Date >= startDate.Date))));
			Assert.IsTrue (result.All (x => x.Geography.Coordinates.Latitude == 59.913m));
			Assert.IsTrue (result.All (x => x.Geography.Coordinates.Longitude == 10.740m));
		}
				
		
		[Test()]
		public void Calling_AstronomyService_WithoutISOTime_Should_ReturnsCorrectAstronomyInfo ()
		{
			// Arrange
			var startDate = new DateTime (2014, 1, 1);
			var endDate = new DateTime (2014, 1, 30);
			var type = AstronomyObjectType.Sun;
			var place = new LocationId("norway/oslo");
			
			// Act
			var service = new AstronomyService (Config.AccessKey, Config.SecretKey);
			service.IncludeISOTime = false;
			var result = service.GetAstronomicalInfo (type, place, startDate, endDate);
			
			Assert.IsTrue (result.All (x => x.Objects.All (y => y.Days.All (z => z.Date.HasValue))));
			Assert.IsTrue (result.All (x => x.Objects.All (y => y.Days.ToList ().Count > 1)));
			Assert.IsTrue (result.All (x => x.Objects.All (y => y.Days.All (z => z.Date.Value.Date >= startDate.Date))));
			Assert.IsTrue (result.All (x => x.Objects.All (y => y.Days.All (z => z.Events.All (a => a.ISOTime.Year == 1)))));
			Assert.IsTrue (result.All (x => x.Objects.All (y => y.Days.All (z => z.Events.All (a => a.ISOTime.Month == 1)))));
			Assert.IsTrue (result.All (x => x.Objects.All (y => y.Days.All (z => z.Events.All (a => a.ISOTime.Day == 1)))));						
		}
		
		[Test()]
		public void Calling_AstronomyService_WithISOTime_Should_ReturnsCorrectAstronomyInfo ()
		{
			// Arrange
			var startDate = new DateTime (2014, 1, 1);
			var endDate = new DateTime (2014, 1, 30);
			var type = AstronomyObjectType.Sun;
			var place = new LocationId("norway/oslo");
			
			// Act
			var service = new AstronomyService (Config.AccessKey, Config.SecretKey);
			service.IncludeISOTime = true;
			var result = service.GetAstronomicalInfo (type, place, startDate, endDate);
			
			Assert.IsTrue (result.All (x => x.Objects.All (y => y.Days.All (z => z.Date.HasValue))));
			Assert.IsTrue (result.All (x => x.Objects.All (y => y.Days.ToList ().Count > 1)));
			Assert.IsTrue (result.All (x => x.Objects.All (y => y.Days.All (z => z.Date.Value.Date >= startDate.Date))));
			Assert.IsTrue (result.All (x => x.Objects.All (y => y.Days.All (z => !z.Events.All (a => a.ISOTime.Year == 1)))));
			Assert.IsTrue (result.All (x => x.Objects.All (y => y.Days.All (z => z.Events.Any (a => a.ISOTime.Month == 1)))));
		}
		
		[Test()]
		public void Calling_AstronomyService_WithoutUTCTime_Should_ReturnsCorrectAstronomyInfo ()
		{
			// Arrange
			var startDate = new DateTime (2014, 3, 2);
			var endDate = new DateTime (2014, 3, 30);
			var type = AstronomyObjectType.Moon;
			var place = new LocationId("norway/oslo");
			
			// Act
			var service = new AstronomyService (Config.AccessKey, Config.SecretKey);
			service.IncludeUTCTime = false;
			var result = service.GetAstronomicalInfo (type, place, startDate, endDate);
			
			Assert.IsTrue (result.All (x => x.Objects.All (y => y.Days.All (z => z.Date.HasValue))));
			Assert.IsTrue (result.All (x => x.Objects.All (y => y.Days.ToList ().Count > 1)));
			Assert.IsTrue (result.All (x => x.Objects.All (y => y.Days.All (z => z.Date.Value.Date >= startDate.Date))));
			Assert.IsTrue (result.All (x => x.Objects.All (y => y.Days.All (z => z.Events.All (a => a.UTCTime.Year == 1)))));
			Assert.IsTrue (result.All (x => x.Objects.All (y => y.Days.All (z => z.Events.All (a => a.UTCTime.Month == 1)))));
			Assert.IsTrue (result.All (x => x.Objects.All (y => y.Days.All (z => z.Events.All (a => a.UTCTime.Day == 1)))));					
		}
		
		[Test()]
		public void Calling_AstronomyService_WithUTCTime_Should_ReturnsCorrectAstronomyInfo ()
		{
			// Arrange
			var startDate = new DateTime (2014, 2, 3);
			var type = AstronomyObjectType.Sun;
			var place = new LocationId ("norway/oslo");
			
			// Act
			var service = new AstronomyService (Config.AccessKey, Config.SecretKey);
			service.IncludeUTCTime = true;
			var result = service.GetAstronomicalInfo (type, place, startDate);
			
			Assert.IsTrue (result.All (x => x.Objects.All (y => y.Days.All (z => z.Date.HasValue))));		
			Assert.IsTrue (result.All (x => x.Objects.All (y => y.Days.ToList ().Count >= 1)));
			Assert.IsTrue (result.All (x => x.Objects.All (y => y.Days.All (z => z.Date.Value.Date >= startDate.Date))));
			Assert.IsTrue (result.All (x => x.Objects.All (y => y.Days.All (z => z.Events.All (a => a.UTCTime.Year == 2014)))));
			Assert.IsTrue (result.All (x => x.Objects.All (y => y.Days.All (z => z.Events.All (a => a.UTCTime.Month == 2)))));
			Assert.IsTrue (result.All (x => x.Objects.All (y => y.Days.All (z => z.Events.All (a => a.UTCTime.Day == 3)))));				
		}
		
		[Test()]
		public void Calling_AstronomyService_WithRadiusTime_Should_ReturnsCorrectAstronomyInfo ()
		{
			// Arrange
			var startDate = new DateTime (2014, 1, 1);
			var type = AstronomyObjectType.Sun;
			var drammenCoords = new LocationId(new Coordinates (59.743m, 10.204m));	
			
			// Act
			var service = new AstronomyService (Config.AccessKey, Config.SecretKey);
			service.Radius = 50;
			var result = service.GetAstronomicalInfo (type, drammenCoords, startDate);
			
			Assert.IsTrue (result.All (x => x.Objects.All (y => y.Days.All (z => z.Date.HasValue))));		
			Assert.IsTrue (result.All (x => x.Objects.All (y => y.Days.ToList ().Count >= 1)));
			Assert.IsTrue (result.All (x => x.Geography.Country.Name == "Norway"));
			Assert.IsTrue (result.All (x => x.Geography.Name == "Drammen"));
		}
	}
}

