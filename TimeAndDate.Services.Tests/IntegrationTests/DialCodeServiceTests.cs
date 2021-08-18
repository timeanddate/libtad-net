using System;
using System.Threading.Tasks;
using System.Linq;
using NUnit.Framework;
using TimeAndDate.Services.DataTypes.DialCode;
using TimeAndDate.Services.DataTypes.Places;

namespace TimeAndDate.Services.Tests.IntegrationTests
{
	[TestFixture()]
	public class DialCodeServiceTests
	{				
		[Test()]
		public async Task Calling_DialCodeService_WithToId_Should_ReturnTheCorrectDialCode ()
		{
			// Arrange
			var osloId = new LocationId("norway/oslo");
			
			// Act
			var service = new DialCodeService (Config.AccessKey, Config.SecretKey);
			var result = await service.GetDialCode (osloId);
			
			// Assert
			Assert.IsTrue (result.Locations.All (x => x.Geography.Country.Name == "Norway"));
		}
		
		[Test()]
		public async Task Calling_DialCodeService_WithToId_And_WithFromId_Should_ReturnTheCorrectDialCode ()
		{
			// Arrange
			var osloId = new LocationId("norway/oslo");
			var newYorkId = new LocationId("usa/new-york");
			
			// Act
			var service = new DialCodeService (Config.AccessKey, Config.SecretKey);
			var result = await service.GetDialCode (osloId, newYorkId);
			
			// Assert
			Assert.IsTrue (result.Locations.Any (x => x.Geography.Country.Name == "Norway"));
			Assert.IsTrue (result.Locations.Any (x => x.Geography.Country.Name == "United States"));
		}
		
		[Test()]
		public async Task Calling_DialCodeService_WithToId_And_WithFromId_And_WithNumber_Should_ReturnTheCorrectDialCode ()
		{
			// Arrange
			var osloId = new LocationId ("norway/oslo");
			var newYorkId = new LocationId ("usa/new-york");
			var sampleNumber = 1234567;
			
			// Act
			var service = new DialCodeService (Config.AccessKey, Config.SecretKey);
			var result = await service.GetDialCode (osloId, newYorkId, sampleNumber);
			
			var intl = result.Compositions [0];
			var ctry = result.Compositions [1];
			var local = result.Compositions [2];
			
			var newYork = result.Locations [0];
			var oslo = result.Locations [1];
			
			// Assert
			Assert.AreEqual (intl.PhoneNumberElement, PhoneNumberElementType.InternationalPrefix);
			Assert.AreEqual ("011", intl.Number);
			Assert.IsNotNull (intl.Description);
			Assert.IsNotEmpty (intl.Description);
			
			Assert.AreEqual (local.PhoneNumberElement, PhoneNumberElementType.LocalNumber);
			Assert.AreEqual (sampleNumber.ToString (), local.Number);
			Assert.IsNotNull (local.Description);
			Assert.IsNotEmpty (local.Description);
			
			Assert.AreEqual (ctry.PhoneNumberElement, PhoneNumberElementType.CountryPrefix);
			Assert.AreEqual ("47", ctry.Number);
			Assert.IsNotNull (ctry.Description);
			Assert.IsNotEmpty (ctry.Description);
			
			Assert.IsNotNull (oslo.Time);
			Assert.IsNotNull (oslo.Time.ISO);
			Assert.IsNotNull (oslo.Time.Timezone);
			
			Assert.IsNotNull (newYork.Time);
			Assert.IsNotNull (newYork.Time.ISO);
			Assert.IsNotNull (newYork.Time.Timezone);		
			
			Assert.IsTrue (result.Locations.Any (x => x.Geography.Country.Name == "Norway"));
			Assert.IsTrue (result.Locations.Any (x => x.Geography.Country.Name == "United States"));
			
			Assert.AreEqual ("011 47 1234567", result.Number);
		}
		
		[Test()]
		public async Task Calling_DialCodeService_WithToId_And_WithoutLocation_Should_NotReturnLocations ()
		{
			// Arrange
			var osloId = new LocationId ("norway/oslo");
			
			// Act
			var service = new DialCodeService (Config.AccessKey, Config.SecretKey);
			service.IncludeLocations = false;
			var result = await service.GetDialCode (osloId);
			
			// Assert
			Assert.AreEqual (0, result.Locations.Count);
		}
		
		[Test()]
		public async Task Calling_DialCodeService_WithToId_And_WithoutLatLong_Should_NotReturnLocations ()
		{
			// Arrange
			var osloId = new LocationId ("norway/oslo");
			
			// Act
			var service = new DialCodeService (Config.AccessKey, Config.SecretKey);
			service.IncludeCoordinates = false;
			var result = await service.GetDialCode (osloId);
			
			// Assert
			Assert.IsTrue (result.Locations.All (x => x.Geography.Coordinates == null));
		}
		
		[Test()]
		public async Task Calling_DialCodeService_WithToId_And_WithoutTZInfo_Should_NotReturnTZInfo ()
		{
			// Arrange
			var osloId = new LocationId ("norway/oslo");
			
			// Act
			var service = new DialCodeService (Config.AccessKey, Config.SecretKey);
			service.IncludeTimezoneInformation = false;
			var result = await service.GetDialCode (osloId);
			
			// Assert
			Assert.IsTrue (result.Locations.All (x => x.Time.Timezone == null));
		}
		
		[Test()]
		public async Task Calling_DialCodeService_WithToId_And_WithoutCurrentTime_Should_NotReturnCurrentTime ()
		{
			// Arrange
			var osloId = new LocationId ("norway/oslo");
			
			// Act
			var service = new DialCodeService (Config.AccessKey, Config.SecretKey);
			service.IncludeCurrentTime = false;
			var result = await service.GetDialCode (osloId);
			
			// Assert
			Assert.IsTrue (result.Locations.All (x => x.Time == null));
		}
	}
}

