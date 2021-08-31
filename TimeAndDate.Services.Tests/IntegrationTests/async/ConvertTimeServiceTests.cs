using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;
using TimeAndDate.Services.Tests;
using TimeAndDate.Services.DataTypes.Places;
using TimeAndDate.Services.DataTypes.Time;

namespace TimeAndDate.Services.Tests.IntegrationTests
{
	[TestFixture()]
	public class ConvertTimeTestsAsync
	{				
		public const string fromCountry = "Norway";
		public const string fromCity = "Oslo";
		public static LocationId fromCoords;
		public string fromFormat = String.Format ("{0}/{1}", fromCountry, fromCity).ToLower ();
		public static LocationId fromId;
		
		public const string toUsState = "Alaska";
		public const string toUsCountry = "USA";
		public const string toUsCity = "Anchorage";
		public string toUsFormat = String.Format ("{0}/{1}", toUsCountry, toUsCity).ToLower ();
		public LocationId toUsId;
		
		public const string toArticCountry = "Antarctica";
		public const string toArticCity = "Troll";
		public string toArticFormat = String.Format ("{0}/{1}", toArticCountry, toArticCity).ToLower ();
		public LocationId toArticId;
				
		[SetUp()]
		public void Setup ()
		{			
			fromCoords = new LocationId(new Coordinates (59.913m, 10.752m));
			fromId = new LocationId (fromFormat);
			toUsId = new LocationId (toUsFormat);
			toArticId = new LocationId (toArticFormat);
		}
		
		[Test()]
		public async Task Calling_ConvertTimeService_WithNoId_And_WithDateTime_Should_ReturnCorrectConvertedTime ()
		{
			// Arrange
			
			// Act
			var service = new ConvertTimeService (Config.AccessKey, Config.SecretKey);
			var result = await service.ConvertTimeAsync (fromId, DateTime.Now);
			var oslo = result.Locations.FirstOrDefault (x => x.Id == "187");
			
			// Assert			
			Assert.AreEqual (fromCity, oslo.Geography.Name);
			Assert.AreEqual (fromCountry, oslo.Geography.Country.Name);
			
			HasCorrectUtc (result.Utc.DateTime);
		}
		
		[Test()]
		public async Task Calling_ConvertTimeService_WithOneToId_And_WithDateTime_Should_ReturnCorrectConvertedTime ()
		{
			// Arrange
			var toId = new List<LocationId> ();
			toId.Add (toUsId);
			
			// Act
			var service = new ConvertTimeService (Config.AccessKey, Config.SecretKey);
			var result = await service.ConvertTimeAsync (fromId, DateTime.Now, toId);
			var anchorage = result.Locations.FirstOrDefault (x => x.Id == "18");
			var oslo = result.Locations.FirstOrDefault (x => x.Id == "187");
			
			// Assert
			Assert.AreEqual (toUsState, anchorage.Geography.State);
			Assert.AreEqual (toUsCity, anchorage.Geography.Name);
			
			Assert.AreEqual (oslo.Geography.Name, fromCity);
			Assert.AreEqual (oslo.Geography.Country.Name, fromCountry);
			
			HasCorrectUtc (result.Utc.DateTime);					
		}
		
		[Test()]
		public async Task Calling_ConvertTimeService_WithMultipleToIds_And_WithDateTime_Should_ReturnCorrectConvertedTime ()
		{
			// Arrange
			var toId = new List<LocationId> ();
			toId.Add (toUsId);
			toId.Add (toArticId);
			
			// Act
			var service = new ConvertTimeService (Config.AccessKey, Config.SecretKey);
			var result = await service.ConvertTimeAsync (fromId, DateTime.Now, toId);
			var anchorage = result.Locations.FirstOrDefault (x => x.Id == "18");
			var oslo = result.Locations.FirstOrDefault (x => x.Id == "187");
			var troll = result.Locations.FirstOrDefault (x => x.Id == "4365");
			
			// Assert
			Assert.AreEqual (toUsState, anchorage.Geography.State);
			Assert.AreEqual (toUsCity, anchorage.Geography.Name);
			
			Assert.AreEqual (troll.Geography.Country.Name, toArticCountry);
			Assert.AreEqual (troll.Geography.Name, toArticCity + " Station");
			
			Assert.AreEqual (oslo.Geography.Name, fromCity);
			Assert.AreEqual (oslo.Geography.Country.Name, fromCountry);

			HasCorrectUtc (result.Utc.DateTime);
		}
		
		[Test()]
		public async Task Calling_ConvertTimeService_WithMultipleToIds_And_WithISO_Should_ReturnCorrectConvertedTime ()
		{
			// Arrange			
			var toId = new List<LocationId> ();
			toId.Add (toUsId);
			toId.Add (toArticId);
			
			// Act
			var service = new ConvertTimeService (Config.AccessKey, Config.SecretKey);
			var result = await service.ConvertTimeAsync (fromId, DateTime.Now.ToString ("s", CultureInfo.InvariantCulture), toId);
			var anchorage = result.Locations.FirstOrDefault (x => x.Id == "18");
			var oslo = result.Locations.FirstOrDefault (x => x.Id == "187");
			var troll = result.Locations.FirstOrDefault (x => x.Id == "4365");
			
			// Assert
			Assert.AreEqual (toUsState, anchorage.Geography.State);
			Assert.AreEqual (toUsCity, anchorage.Geography.Name);
			
			Assert.AreEqual (toArticCountry, troll.Geography.Country.Name);
			Assert.AreEqual (toArticCity + " Station", troll.Geography.Name);
			
			Assert.AreEqual (fromCity, oslo.Geography.Name);
			Assert.AreEqual (fromCountry, oslo.Geography.Country.Name);

			HasCorrectUtc (result.Utc.DateTime);
		}
		
		[Test()]
		public async Task Calling_ConvertTimeService_WithNoId_And_WithISO_Should_ReturnCorrectConvertedTime ()
		{
			// Arrange
			
			// Act
			var service = new ConvertTimeService (Config.AccessKey, Config.SecretKey);
			var result = await service.ConvertTimeAsync (fromId, DateTime.Now.ToString ("s", CultureInfo.InvariantCulture));
			var oslo = result.Locations.FirstOrDefault (x => x.Id == "187");
			
			// Assert						
			Assert.AreEqual (fromCity, oslo.Geography.Name);
			Assert.AreEqual (fromCountry, oslo.Geography.Country.Name);
			
			HasCorrectUtc (result.Utc.DateTime);
		}
		
		[Test()]
		public async Task Calling_ConvertTimeService_WithOneToId_And_WithISO_Should_ReturnCorrectConvertedTime ()
		{
			// Arrange
			var toId = new List<LocationId> ();
			toId.Add (toUsId);
			
			// Act
			var service = new ConvertTimeService (Config.AccessKey, Config.SecretKey);
			var result = await service.ConvertTimeAsync (fromId, DateTime.Now.ToString ("s", CultureInfo.InvariantCulture), toId);
			var anchorage = result.Locations.FirstOrDefault (x => x.Id == "18");
			var oslo = result.Locations.FirstOrDefault (x => x.Id == "187");
			
			// Assert
			Assert.AreEqual (toUsState, anchorage.Geography.State);
			Assert.AreEqual (toUsCity, anchorage.Geography.Name);			
			
			Assert.AreEqual (fromCity, oslo.Geography.Name);
			Assert.AreEqual (fromCountry, oslo.Geography.Country.Name);
			
			HasCorrectUtc (result.Utc.DateTime);
		}		
		
		[Test()]
		public async Task Calling_ConvertTimeService_WithoutTimeChanges_Should_NotReturnTimeChanges ()
		{
			// Arrange
			var toId = new List<LocationId> ();
			toId.Add (toUsId);
			
			// Act
			var service = new ConvertTimeService (Config.AccessKey, Config.SecretKey);
			service.IncludeTimeChanges = false;
			var result = await service.ConvertTimeAsync (fromId, DateTime.Now.ToString ("s", CultureInfo.InvariantCulture), toId);
			var anchorage = result.Locations.FirstOrDefault (x => x.Id == "18");
			var oslo = result.Locations.FirstOrDefault (x => x.Id == "187");
			
			// Assert
			Assert.AreEqual (toUsState, anchorage.Geography.State);
			Assert.AreEqual (toUsCity, anchorage.Geography.Name);			
			
			Assert.AreEqual (fromCity, oslo.Geography.Name);
			Assert.AreEqual (fromCountry, oslo.Geography.Country.Name);
			
			Assert.IsTrue (result.Locations.All (x => x.TimeChanges.Count == 0));			
		}		
		
		[Test()]
		public async Task Calling_ConvertTimeService_WithTimeChanges_Should_ReturnTimeChanges ()
		{
			// Arrange
			var toId = new List<LocationId> ();
			toId.Add (toUsId);
			
			// Act
			var service = new ConvertTimeService (Config.AccessKey, Config.SecretKey);
			service.IncludeTimeChanges = true; // Default
			var result = await service.ConvertTimeAsync (fromId, DateTime.Now.ToString ("s", CultureInfo.InvariantCulture), toId);
			var anchorage = result.Locations.FirstOrDefault (x => x.Id == "18");
			var oslo = result.Locations.FirstOrDefault (x => x.Id == "187");
			
			// Assert
			Assert.AreEqual (toUsState, anchorage.Geography.State);
			Assert.AreEqual (toUsCity, anchorage.Geography.Name);			
			
			Assert.AreEqual (fromCity, oslo.Geography.Name);
			Assert.AreEqual (fromCountry, oslo.Geography.Country.Name);
			
			Assert.IsTrue (result.Locations.All (x => x.TimeChanges != null));
		}		
		
		[Test()]
		public async Task Calling_ConvertTimeService_WithoutTimezone_Should_NotReturnTZInformation ()
		{
			// Arrange
			var toId = new List<LocationId> ();
			toId.Add (toUsId);
			
			// Act
			var service = new ConvertTimeService (Config.AccessKey, Config.SecretKey);
			service.IncludeTimezoneInformation = false;
			var result = await service.ConvertTimeAsync (fromId, DateTime.Now.ToString ("s", CultureInfo.InvariantCulture), toId);
			var anchorage = result.Locations.FirstOrDefault (x => x.Id == "18");
			var oslo = result.Locations.FirstOrDefault (x => x.Id == "187");
			
			// Assert
			Assert.AreEqual (toUsState, anchorage.Geography.State);
			Assert.AreEqual (toUsCity, anchorage.Geography.Name);			
			
			Assert.AreEqual (fromCity, oslo.Geography.Name);
			Assert.AreEqual (fromCountry, oslo.Geography.Country.Name);
			
			Assert.IsTrue (result.Locations.All (x => x.Time.Timezone == null));
		}	
		
		[Test()]
		public async Task Calling_ConvertTimeService_WithTimezone_Should_ReturnTZInformation ()
		{
			// Arrange
			var toId = new List<LocationId> ();
			toId.Add (toUsId);
			
			// Act
			var service = new ConvertTimeService (Config.AccessKey, Config.SecretKey);
			service.IncludeTimezoneInformation = true; // Default
			var result = await service.ConvertTimeAsync (fromId, DateTime.Now.ToString ("s", CultureInfo.InvariantCulture), toId);
			var anchorage = result.Locations.FirstOrDefault (x => x.Id == "18");
			var oslo = result.Locations.FirstOrDefault (x => x.Id == "187");
			
			// Assert
			Assert.AreEqual (toUsState, anchorage.Geography.State);
			Assert.AreEqual (toUsCity, anchorage.Geography.Name);			
			
			Assert.AreEqual (fromCity, oslo.Geography.Name);
			Assert.AreEqual (fromCountry, oslo.Geography.Country.Name);
			
			Assert.IsTrue (result.Locations.All (x => x.Time.Timezone != null));
		}	
		
		[Test()]
		public async Task Calling_ConvertTimeService_WithRadius_Should_ReturnCorrectLocation ()
		{
			// Arrange
			var toId = new List<LocationId> ();
			toId.Add (toUsId);
					
			
			// Act
			var service = new ConvertTimeService (Config.AccessKey, Config.SecretKey);
			service.Radius = 50;
			var result = await service.ConvertTimeAsync (fromCoords, DateTime.Now.ToString ("s", CultureInfo.InvariantCulture), toId);
			var anchorage = result.Locations.FirstOrDefault (x => x.Id == "18");
			var oslo = result.Locations.FirstOrDefault (x => x.Id == "187");
			
			// Assert
			Assert.AreEqual (toUsState, anchorage.Geography.State);
			Assert.AreEqual (toUsCity, anchorage.Geography.Name);			
			
			Assert.AreEqual (fromCity, oslo.Geography.Name);
			Assert.AreEqual (fromCountry, oslo.Geography.Country.Name);
			
			Assert.IsTrue (result.Locations.All (x => x.Time.Timezone != null));
		}	
		
        	public void HasCorrectLocation(TADDateTime date, Location location)
		{
			Assert.AreEqual (date.Year, location.Time.DateTime.Year);			
			Assert.AreEqual (date.Month, location.Time.DateTime.Month);
			Assert.AreEqual (date.Day, location.Time.DateTime.Day);
			Assert.AreEqual (date.Hour, location.Time.DateTime.Hour);
			Assert.AreEqual (date.Minute, location.Time.DateTime.Minute);
		}
		
        	public void HasCorrectUtc(TADDateTime date)
		{
			Assert.AreEqual (DateTime.UtcNow.Year, date.Year);
			Assert.AreEqual (DateTime.UtcNow.Month, date.Month);
			Assert.AreEqual (DateTime.UtcNow.Day, date.Day);
			Assert.AreEqual (DateTime.UtcNow.Hour, date.Hour);
			Assert.AreEqual (DateTime.UtcNow.Minute, date.Minute);
		}
	}
}

