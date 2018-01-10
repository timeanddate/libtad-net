using System;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.Globalization;
using TimeAndDate.Services.DataTypes.Astro;
using TimeAndDate.Services.DataTypes.Places;
using TimeAndDate.Services.Common;

namespace TimeAndDate.Services.Tests.IntegrationTests
{
	[TestFixture()]
	public class TimeServiceTests
	{		
		#region Location
		
		[Test]
		public void Calling_TimeService_WithNumericId_Should_ReturnCorrectLocation ()
		{
			// Arrange
			var placeId = 179;
			
			// Act
			var timeservice = new TimeService (Config.AccessKey, Config.SecretKey);
			var result = timeservice.CurrentTimeForPlace (new LocationId(placeId));
			var firstLocation = result.SingleOrDefault ();
			
			// Assert
			Assert.AreEqual (placeId.ToString (), firstLocation.Id);
		}
		
		[Test]
		public void Calling_TimeService_WithTextualId_Should_ReturnCorrectLocation ()
		{
			// Arrange
			var placeName = "norway/oslo";
			var placeId = "187";
			
			// Act
			var timeservice = new TimeService (Config.AccessKey, Config.SecretKey);
			var result = timeservice.CurrentTimeForPlace (new LocationId(placeName));
			var firstLocation = result.SingleOrDefault ();
			
			// Assert
			Assert.AreEqual (placeId, firstLocation.Id);
		}
		
		[Test]
		public void Calling_TimeService_WithCoordinates_Should_ReturnCorrectLocation ()
		{
			// Arrange
			var osloCoords = new Coordinates (59.914m, 10.752m);
			var expectedId = String.Format ("+{0}+{1}", osloCoords.Latitude.ToString (CultureInfo.InvariantCulture), 
			                                osloCoords.Longitude.ToString (CultureInfo.InvariantCulture));
			
			// Act
			var timeservice = new TimeService (Config.AccessKey, Config.SecretKey);
			var result = timeservice.CurrentTimeForPlace (new LocationId(osloCoords));
			var firstLocation = result.SingleOrDefault ();
			
			// Assert
			Assert.AreEqual (expectedId, firstLocation.Id);
			
		}
		
		#endregion
		
		#region Geo
		
		[Test]
		public void Calling_TimeService_WithCoordinates_Should_ReturnCorrectGeo ()
		{
			// Arrange
			var osloCoords = new Coordinates (59.914m, 10.752m);
			var expectedId = String.Format ("+{0}+{1}", osloCoords.Latitude.ToString (CultureInfo.InvariantCulture), 
			                                osloCoords.Longitude.ToString (CultureInfo.InvariantCulture));
			
			// Act
			var timeservice = new TimeService (Config.AccessKey, Config.SecretKey);
			var result = timeservice.CurrentTimeForPlace (new LocationId (osloCoords));
			var firstLocation = result.SingleOrDefault ();
			
			// Assert
			Assert.AreEqual (osloCoords.Latitude, firstLocation.Geography.Coordinates.Latitude);
			Assert.AreEqual (osloCoords.Longitude, firstLocation.Geography.Coordinates.Longitude);
			Assert.AreEqual (expectedId, firstLocation.Id);
			Assert.IsNull (firstLocation.Geography.Country);
			Assert.IsNull (firstLocation.Geography.State);
			Assert.IsNull (firstLocation.Geography.Name);
		}
		
		[Test]
		public void Calling_TimeService_WithTextualId_Should_ReturnCorrectGeo ()
		{
			// Arrange
			var expectedCountry = "Norway";
			var expectedPlace = "Oslo";
			var placeName = String.Format ("{0}/{1}", expectedCountry, expectedPlace).ToLower ();
			var expectedCountryId = "no";
			var expectedPlaceId = "187";
			
			// Act
			var timeservice = new TimeService (Config.AccessKey, Config.SecretKey);
			var result = timeservice.CurrentTimeForPlace (new LocationId(placeName));
			var firstLocation = result.SingleOrDefault ();
			
			// Assert
			Assert.AreEqual (expectedCountry, firstLocation.Geography.Country.Name);
			Assert.AreEqual (expectedCountryId, firstLocation.Geography.Country.Id);			
			Assert.AreEqual (expectedPlace, firstLocation.Geography.Name);
			Assert.AreEqual (expectedPlaceId, firstLocation.Id);			
		}
		
		[Test]
		public void Calling_TimeService_WithNumericId_Should_ReturnCorrectGeo ()
		{
			// Arrange
			var expectedCountry = "Norway";
			var expectedPlace = "Oslo";
			var expectedCountryId = "no";
			var placeId = 187;
			
			// Act
			var timeservice = new TimeService (Config.AccessKey, Config.SecretKey);
			var result = timeservice.CurrentTimeForPlace (new LocationId(placeId));
			var firstLocation = result.SingleOrDefault ();
			
			// Assert
			Assert.AreEqual (expectedCountry, firstLocation.Geography.Country.Name);
			Assert.AreEqual (expectedCountryId, firstLocation.Geography.Country.Id);			
			Assert.AreEqual (expectedPlace, firstLocation.Geography.Name);
			Assert.AreEqual (placeId.ToString(), firstLocation.Id);			
		}
		
		#endregion
		
		#region Time
		
		[Test]
		public void Calling_TimeService_WithNumericId_Should_ReturnCorrectTime ()
		{
			// Arrange
			var placeId = 187;
			var now = DateTime.Now;
			
			// Act
			var timeservice = new TimeService (Config.AccessKey, Config.SecretKey);
			var result = timeservice.CurrentTimeForPlace (new LocationId(placeId));
			var firstLocation = result.SingleOrDefault ();			
			
			// Assert
			Assert.AreEqual (now.Year, firstLocation.Time.DateTime.Year);
		}
		
		[Test]
		public void Calling_TimeService_WithTextualId_Should_ReturnCorrectTime ()
		{
			// Arrange
			var placeName = "norway/oslo";
			var now = DateTime.Now;
			
			// Act
			var timeservice = new TimeService (Config.AccessKey, Config.SecretKey);
			var result = timeservice.CurrentTimeForPlace (new LocationId(placeName));
			var firstLocation = result.SingleOrDefault ();			
			
			// Assert
			Assert.AreEqual (now.Year, firstLocation.Time.DateTime.Year); 
		}
		
				
		[Test]
		public void Calling_TimeService_WithCoordinates_Should_ReturnCorrectTime ()
		{
			// Arrange
			var osloCoords = new Coordinates (59.914m, 10.752m);
			var expectedId = String.Format ("+{0}+{1}", osloCoords.Latitude.ToString (CultureInfo.InvariantCulture), 
			                                osloCoords.Longitude.ToString (CultureInfo.InvariantCulture));
			var now = DateTime.Now;
			
			// Act
			var timeservice = new TimeService (Config.AccessKey, Config.SecretKey);
			timeservice.IncludeCoordinates = true;
			var result = timeservice.CurrentTimeForPlace (new LocationId (osloCoords));
			var firstLocation = result.SingleOrDefault ();		
			
			// Assert
			Assert.AreEqual (now.Year, firstLocation.Time.DateTime.Year);
			Assert.AreEqual (expectedId, firstLocation.Id);
		}
		
		#endregion
		
		#region Timezone
				
		[Test]
		public void Calling_TimeService_WithNumericId_Should_ReturnCorrectTimezone ()
		{
			// Arrange
			var placeId = 187;
			var expectedTimezoneAbbr1 = "CEST";
			var expectedTimezoneAbbr2 = "CET";
			var expectedTimezoneName1 = "Central European Summer Time";
			var expectedTimezoneName2 = "Central European Time";
			var expectedOffsetHour1 = 1;
			var expectedOffsetHour2 = 2;
			var expectedOffsetMinute = 0;
			var expectedBasicOffset = 3600;
			var expectedDstOffset1 = 3600;
			var expectedDstOffset2 = 0;
			var expectedTotalOffset1 = 7200;
			var expectedTotalOffset2 = 3600;


			// Act
			var timeservice = new TimeService (Config.AccessKey, Config.SecretKey);
			var result = timeservice.CurrentTimeForPlace (new LocationId(placeId));
			var firstLocation = result.SingleOrDefault ();			
			var abbr = firstLocation.Time.Timezone.Abbrevation;

			// Assert
			Assert.That(firstLocation.Time.Timezone.Abbrevation, Is.EqualTo(expectedTimezoneAbbr1) | Is.EqualTo(expectedTimezoneAbbr2));
			Assert.That(firstLocation.Time.Timezone.Name, Is.EqualTo(expectedTimezoneName1) | Is.EqualTo(expectedTimezoneName2));
			Assert.That(firstLocation.Time.Timezone.Offset.Hours, Is.EqualTo(expectedOffsetHour1) | Is.EqualTo(expectedOffsetHour2));
			Assert.That(firstLocation.Time.Timezone.DSTOffset, Is.EqualTo(expectedDstOffset1) | Is.EqualTo(expectedDstOffset2));
			Assert.That(firstLocation.Time.Timezone.TotalOffset, Is.EqualTo(expectedTotalOffset1) | Is.EqualTo(expectedTotalOffset2));
			Assert.AreEqual (expectedOffsetMinute, firstLocation.Time.Timezone.Offset.Minutes);
			Assert.AreEqual (expectedBasicOffset, firstLocation.Time.Timezone.BasicOffset);
		}
		
		[Test]
		public void Calling_TimeService_WithTextualId_Should_ReturnCorrectTimezone ()
		{
			// Arrange
			var placeName = "norway/oslo";
			var expectedTimezoneAbbr1 = "CEST";
			var expectedTimezoneAbbr2 = "CET";
			var expectedTimezoneName1 = "Central European Summer Time";
			var expectedTimezoneName2 = "Central European Time";
			var expectedOffsetHour1 = 1;
			var expectedOffsetHour2 = 2;
			var expectedOffsetMinute = 0;
			var expectedBasicOffset = 3600;
			var expectedDstOffset1 = 3600;
			var expectedDstOffset2 = 0;
			var expectedTotalOffset1 = 7200;
			var expectedTotalOffset2 = 3600;


			// Act
			var timeservice = new TimeService (Config.AccessKey, Config.SecretKey);
			var result = timeservice.CurrentTimeForPlace (new LocationId(placeName));
			var firstLocation = result.SingleOrDefault ();

			// Assert
			Assert.That(firstLocation.Time.Timezone.Abbrevation, Is.EqualTo(expectedTimezoneAbbr1) | Is.EqualTo(expectedTimezoneAbbr2));
			Assert.That(firstLocation.Time.Timezone.Name, Is.EqualTo(expectedTimezoneName1) | Is.EqualTo(expectedTimezoneName2));
			Assert.That(firstLocation.Time.Timezone.Offset.Hours, Is.EqualTo(expectedOffsetHour1) | Is.EqualTo(expectedOffsetHour2));
			Assert.That(firstLocation.Time.Timezone.DSTOffset, Is.EqualTo(expectedDstOffset1) | Is.EqualTo(expectedDstOffset2));
			Assert.That(firstLocation.Time.Timezone.TotalOffset, Is.EqualTo(expectedTotalOffset1) | Is.EqualTo(expectedTotalOffset2));
			Assert.AreEqual (expectedOffsetMinute, firstLocation.Time.Timezone.Offset.Minutes);
			Assert.AreEqual (expectedBasicOffset, firstLocation.Time.Timezone.BasicOffset);
		}
		
				
		[Test]
		public void Calling_TimeService_WithCoordinates_Should_ReturnCorrectTimezone ()
		{
			// Arrange
			var osloCoords = new Coordinates (59.914m, 10.752m);			
			var expectedTimezoneAbbr1 = "CEST";
			var expectedTimezoneAbbr2 = "CET";
			var expectedTimezoneName1 = "Central European Summer Time";
			var expectedTimezoneName2 = "Central European Time";
			var expectedOffsetHour1 = 1;
			var expectedOffsetHour2 = 2;
			var expectedOffsetMinute = 0;
			var expectedBasicOffset1 = 3600;
			var expectedBasicOffset2 = 0;
			var expectedDstOffset1 = 3600;
			var expectedDstOffset2 = 0;
			var expectedTotalOffset1 = 7200;
			var expectedTotalOffset2 = 3600;


			// Act
			var timeservice = new TimeService (Config.AccessKey, Config.SecretKey);
			var result = timeservice.CurrentTimeForPlace (new LocationId(osloCoords));
			var firstLocation = result.SingleOrDefault ();

			// Assert
			Assert.That(firstLocation.Time.Timezone.Abbrevation, Is.EqualTo(expectedTimezoneAbbr1) | Is.EqualTo(expectedTimezoneAbbr2));
			Assert.That(firstLocation.Time.Timezone.Name, Is.EqualTo(expectedTimezoneName1) | Is.EqualTo(expectedTimezoneName2));
			Assert.That(firstLocation.Time.Timezone.Offset.Hours, Is.EqualTo(expectedOffsetHour1) | Is.EqualTo(expectedOffsetHour2));
			Assert.That(firstLocation.Time.Timezone.BasicOffset, Is.EqualTo(expectedBasicOffset1) | Is.EqualTo(expectedBasicOffset2));
			Assert.That(firstLocation.Time.Timezone.DSTOffset, Is.EqualTo(expectedDstOffset1) | Is.EqualTo(expectedDstOffset2));
			Assert.That(firstLocation.Time.Timezone.TotalOffset, Is.EqualTo(expectedTotalOffset1) | Is.EqualTo(expectedTotalOffset2));
			Assert.AreEqual (expectedOffsetMinute, firstLocation.Time.Timezone.Offset.Minutes);
		}
		
		#endregion
				
		#region TimeChanges
		
		[Test]
		public void Calling_TimeService_WithNumericId_Should_ReturnCorrectTimeChanges ()
		{
			// Arrange
			var placeId = 187;			
			var expectedFirstNewOffset = 7200;
			var expectedFirstNewDstOffset = 3600;
			var expectedSecondNewOffset = 3600;
			
			// Act
			var timeservice = new TimeService (Config.AccessKey, Config.SecretKey);
			var result = timeservice.CurrentTimeForPlace (new LocationId(placeId));
			var firstLocation = result.SingleOrDefault ();		
			var firstChange = firstLocation.TimeChanges.FirstOrDefault ();
			var secondChange = firstLocation.TimeChanges.Skip (1).FirstOrDefault ();
			
			// Assert
			Assert.IsTrue (firstChange.NewDaylightSavingTime.HasValue);
			Assert.AreEqual (expectedFirstNewDstOffset, firstChange.NewDaylightSavingTime.Value);
			Assert.AreEqual (expectedFirstNewOffset, firstChange.NewTotalOffset);					
			Assert.AreEqual (expectedSecondNewOffset, secondChange.NewTotalOffset);
		}
		
		[Test]
		public void Calling_TimeService_WithTextualId_Should_ReturnCorrectTimeChanges ()
		{
			// Arrange
			var placeName = "norway/oslo";
			var expectedFirstNewOffset = 7200;
			var expectedFirstNewDstOffset = 3600;
			var expectedSecondNewOffset = 3600;
			
			// Act
			var timeservice = new TimeService (Config.AccessKey, Config.SecretKey);
			var result = timeservice.CurrentTimeForPlace (new LocationId(placeName));
			var firstLocation = result.SingleOrDefault ();		
			var firstChange = firstLocation.TimeChanges.FirstOrDefault ();
			var secondChange = firstLocation.TimeChanges.Skip (1).FirstOrDefault ();
			
			// Assert
			Assert.IsTrue (firstChange.NewDaylightSavingTime.HasValue);
			Assert.AreEqual (expectedFirstNewDstOffset, firstChange.NewDaylightSavingTime.Value);
			Assert.AreEqual (expectedFirstNewOffset, firstChange.NewTotalOffset);					
			Assert.AreEqual (expectedSecondNewOffset, secondChange.NewTotalOffset);
		}
		
				
		[Test]
		public void Calling_TimeService_WithCoordinates_Should_ReturnCorrectTimeChanges ()
		{
			// Arrange
			var osloCoords = new Coordinates (59.914m, 10.752m);	
			var expectedFirstNewOffset = 7200;
			var expectedFirstNewDstOffset = 3600;
			var expectedSecondNewOffset = 3600;
			
			// Act
			var timeservice = new TimeService (Config.AccessKey, Config.SecretKey);
			var result = timeservice.CurrentTimeForPlace (new LocationId(osloCoords));
			var firstLocation = result.SingleOrDefault ();		
			var firstChange = firstLocation.TimeChanges.FirstOrDefault ();
			var secondChange = firstLocation.TimeChanges.Skip (1).FirstOrDefault ();
			
			// Assert
			Assert.IsTrue (firstChange.NewDaylightSavingTime.HasValue);
			Assert.AreEqual (expectedFirstNewDstOffset, firstChange.NewDaylightSavingTime.Value);
			Assert.AreEqual (expectedFirstNewOffset, firstChange.NewTotalOffset);					
			Assert.AreEqual (expectedSecondNewOffset, secondChange.NewTotalOffset);
		}
		
		#endregion
		
		#region AstronomyTests			
		
		[Test]
		// This test checks on sunset, but the check criteria can have changed from day to day
		// TODO to find a way around this
		public void Calling_TimeService_WithNumericId_Should_ReturnCorrectAstronomy ()
		{
			// Arrange
			var placeId = 187;
			var expectedObjectName = AstronomyObjectType.Sun;
			var expectedRise = AstronomyEventType.Rise;
			var expectedSet = AstronomyEventType.Set;
			
			// Act
			var timeservice = new TimeService (Config.AccessKey, Config.SecretKey);
			var result = timeservice.CurrentTimeForPlace (new LocationId(placeId));
			var firstLocation = result.SingleOrDefault ();		
			var firstObject = firstLocation.Astronomy.SingleOrDefault ();
			var rise = firstObject.Events.FirstOrDefault ();
			var set = firstObject.Events.Skip (1).FirstOrDefault ();
			
			// Assert
			Assert.AreEqual (expectedObjectName, firstObject.Name);
			Assert.AreEqual (expectedRise, rise.Type);
			Assert.AreEqual (expectedSet, set.Type);
			
			// Sunrise in Oslo is most likely never before 3 and never after 10
			// Sunset in Oslo is most likely never before 14 and never after 22
			Assert.IsTrue (rise.Time.Hour >= 3 && rise.Time.Hour <= 10);
			Assert.IsTrue (set.Time.Hour >= 14 && set.Time.Hour <= 22);
		}
		
		[Test]
		// This test checks on sunset, but the check criteria can have changed from day to day
		// TODO to find a way around this
		public void Calling_TimeService_WithTextualId_Should_ReturnCorrectAstronomy ()
		{
			// Arrange
			var placeName = "norway/oslo";
			var expectedObjectName = AstronomyObjectType.Sun;
			var expectedRise = AstronomyEventType.Rise;
			var expectedSet = AstronomyEventType.Set;

			// Act
			var timeservice = new TimeService (Config.AccessKey, Config.SecretKey);
			var result = timeservice.CurrentTimeForPlace (new LocationId(placeName));
			var firstLocation = result.SingleOrDefault ();		
			var firstObject = firstLocation.Astronomy.FirstOrDefault ();
			var rise = firstObject.Events.FirstOrDefault ();
			var set = firstObject.Events.Skip (1).FirstOrDefault ();
			
			// Assert
			Assert.AreEqual (expectedObjectName, firstObject.Name);
			Assert.AreEqual (expectedRise, rise.Type);
			Assert.AreEqual (expectedSet, set.Type);		
			
			// Sunrise in Oslo is most likely never before 3 and never after 10
			// Sunset in Oslo is most likely never before 14 and never after 22
			Assert.IsTrue (rise.Time.Hour >= 3 && rise.Time.Hour <= 10);
			Assert.IsTrue (set.Time.Hour >= 14 && set.Time.Hour <= 22);
		}
		
		[Test] 
		// This test checks on sunset, but the check criteria can have changed from day to day
		// TODO to find a way around this
		public void Calling_TimeService_WithCoordinates_Should_ReturnCorrectAstronomy ()
		{
			// Arrange
			var osloCoords = new Coordinates (59.914m, 10.752m);	
			var expectedObjectName = AstronomyObjectType.Sun;
			var expectedRise = AstronomyEventType.Rise;
			var expectedSet = AstronomyEventType.Set;
			
			// Act
			var timeservice = new TimeService (Config.AccessKey, Config.SecretKey);
			var result = timeservice.CurrentTimeForPlace (new LocationId(osloCoords));
			var firstLocation = result.SingleOrDefault ();		
			var firstObject = firstLocation.Astronomy.SingleOrDefault ();
			var rise = firstObject.Events.FirstOrDefault ();
			var set = firstObject.Events.Skip (1).FirstOrDefault ();
			
			// Assert
			Assert.AreEqual (expectedObjectName, firstObject.Name);
			Assert.AreEqual (expectedRise, rise.Type);
			Assert.AreEqual (expectedSet, set.Type);
			
			// Sunrise in Oslo is most likely never before 3 and never after 10
			// Sunset in Oslo is most likely never before 14 and never after 22
			Assert.IsTrue (rise.Time.Hour >= 3 && rise.Time.Hour <= 10);
			Assert.IsTrue (set.Time.Hour >= 14 && set.Time.Hour <= 22);
		}	
		
		#endregion			

		public void Calling_TimeService_And_GettingUTCOffset_WithNonExistingLocalTime_Should_ThrowException ()
		{
			// Arrange
			var placeId = 187;			
			var localTime = new DateTime (2015, 3, 29, 2, 30, 0);
			
			// Act
			var timeservice = new TimeService (Config.AccessKey, Config.SecretKey);
			var result = timeservice.CurrentTimeForPlace (new LocationId (placeId));
			var firstLocation = result.SingleOrDefault ();


			// Assert
			Assert.That (() => firstLocation.GetUTCOffsetFromLocalTime(localTime),
						 Throws.TypeOf<LocalTimeDoesNotExistException>());
		}
		
		public void Calling_TimeService_And_GettingUTCOffset_WithWrongYear_Should_ThrowException ()
		{
			// Arrange
			var placeId = 187;			
			var localTime = new DateTime (2014, 3, 29, 2, 30, 0);
			
			// Act
			var timeservice = new TimeService (Config.AccessKey, Config.SecretKey);
			var result = timeservice.CurrentTimeForPlace (new LocationId (placeId));
			var firstLocation = result.SingleOrDefault ();


			// Assert
			Assert.That (() => firstLocation.GetUTCOffsetFromLocalTime(localTime), 
			            Throws.TypeOf<QueriedDateOutOfRangeException>());
		}
		
		[Test]
		public void Calling_TimeService_And_GettingUTCOffset_Should_ReturnCorrectOffset ()
		{
			// Arrange
			var placeId = 187;			
			var localWinterTime = new DateTime (DateTime.UtcNow.Year, 2, 15, 2, 30, 0);
			var localSummerTime = new DateTime (DateTime.UtcNow.Year, 7, 15, 2, 30, 0);
			var localFallTime = new DateTime (DateTime.UtcNow.Year, 11, 15, 2, 30, 0);
			
			// Act
			var timeservice = new TimeService (Config.AccessKey, Config.SecretKey);
			var result = timeservice.CurrentTimeForPlace (new LocationId (placeId));
			var firstLocation = result.SingleOrDefault ();	
			
			var utcWinterOffset = firstLocation.GetUTCOffsetFromLocalTime (localWinterTime);
			var utcSummerOffset = firstLocation.GetUTCOffsetFromLocalTime (localSummerTime);
			var utcFallOffset = firstLocation.GetUTCOffsetFromLocalTime (localFallTime);
			
			// Assert
			Assert.AreEqual (TimeSpan.FromHours (1), utcWinterOffset);
			Assert.AreEqual (TimeSpan.FromHours (2), utcSummerOffset);
			Assert.AreEqual (TimeSpan.FromHours (1), utcFallOffset);
		}
		
		[Test]
		public void Calling_TimeService_And_GettingUTCOffset_WithEdgeCases_Should_ReturnCorrectOffset ()
		{
			// Arrange
			var placeId = "usa/anchorage";			
			var beforeDstStart = new DateTimeOffset (DateTime.UtcNow.Year, 2, 15, 1, 0, 0, TimeSpan.FromHours (0));
			var afterDstStart = new DateTimeOffset (DateTime.UtcNow.Year, 4, 15, 3, 0, 0, TimeSpan.FromHours (0));
			var beforeDstEnd = new DateTimeOffset (DateTime.UtcNow.Year, 10, 15, 1, 0, 0, TimeSpan.FromHours (0));
			var afterDstEnd = new DateTimeOffset (DateTime.UtcNow.Year, 12, 15, 2, 0, 0, TimeSpan.FromHours (0));
			
			// Act
			var timeservice = new TimeService (Config.AccessKey, Config.SecretKey);
			var result = timeservice.CurrentTimeForPlace (new LocationId (placeId));
			var firstLocation = result.SingleOrDefault ();	
			
			var beforeDstStartOffset = firstLocation.GetUTCOffsetFromLocalTime (beforeDstStart);
			var afterDstStartOffset = firstLocation.GetUTCOffsetFromLocalTime (afterDstStart);
			var beforeDstEndOffset = firstLocation.GetUTCOffsetFromLocalTime (beforeDstEnd);
			var afterDstEndOffset = firstLocation.GetUTCOffsetFromLocalTime (afterDstEnd);
			
			// Assert
			Assert.AreEqual (TimeSpan.FromHours (-9), beforeDstStartOffset);
			Assert.AreEqual (TimeSpan.FromHours (-8), afterDstStartOffset);
			Assert.AreEqual (TimeSpan.FromHours (-8), beforeDstEndOffset);
			Assert.AreEqual (TimeSpan.FromHours (-9), afterDstEndOffset);
		}
	}
}