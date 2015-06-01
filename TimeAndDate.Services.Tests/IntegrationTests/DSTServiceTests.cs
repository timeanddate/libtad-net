using System;
using System.Linq;
using NUnit.Framework;
using TimeAndDate.Services.Tests;
using TimeAndDate.Services;
using TimeAndDate.Services.DataTypes.DST;
using TimeAndDate.Services.DataTypes;

namespace TimeAndDate.Services.Tests.IntegrationTests
{
	[TestFixture()]
	public class DSTServiceTests
	{
		[Test()]
		public void Calling_DstService_Should_ReturnAllDst ()
		{
			// Arrage
			var expectedReturnedCount = 138;
			
			// Act
			var service = new DSTService (Config.AccessKey, Config.SecretKey);			
			var result = service.GetDaylightSavingTime ();
			var sampleCountry = result.SingleOrDefault (x => x.Region.Country.Name == "Norway");
			
			// Assert
			Assert.AreEqual (expectedReturnedCount, result.Count);
			
			HasValidSampleCountry (sampleCountry);
		}
		
		[Test()]
		public void Calling_DstService_WithYear_Should_ReturnAllDst ()
		{
			// Arrage
			var year = 2014;
			var expectedReturnedCount = 138;
			
			// Act
			var service = new DSTService (Config.AccessKey, Config.SecretKey);			
			var result = service.GetDaylightSavingTime (year);
			var sampleCountry = result.SingleOrDefault (x => x.Region.Country.Name == "Norway");
			
			// Assert
			Assert.AreEqual (expectedReturnedCount, result.Count);
			
			HasValidSampleCountry (sampleCountry);
		}
		
		[Test()]
		public void Calling_DstService_WithCountry_Should_ReturnAllDst ()
		{
			// Arrage
			var countryCode = "no";
			var country = "Norway";
			
			// Act
			var service = new DSTService (Config.AccessKey, Config.SecretKey);			
			var result = service.GetDaylightSavingTime (countryCode);
			var sampleCountry = result.SingleOrDefault (x => x.Region.Country.Name == "Norway");
						
			// Assert
			Assert.IsFalse (service.IncludeOnlyDstCountries);
			Assert.AreEqual (country, sampleCountry.Region.Country.Name);
			Assert.IsTrue (result.Count == 1);
			
			HasValidSampleCountry (sampleCountry);
		}
		
		[Test()]
		public void Calling_DstService_WithCountry_And_WithYear_Should_ReturnAllDst ()
		{
			// Arrage
			var countryCode = "no";
			var year = 2014;
			
			// Act
			var service = new DSTService (Config.AccessKey, Config.SecretKey);			
			var result = service.GetDaylightSavingTime (countryCode, year);
			var sampleCountry = result.SingleOrDefault (x => x.Region.Country.Name == "Norway");
			
			// Assert
			Assert.IsFalse (service.IncludeOnlyDstCountries);
			Assert.IsTrue (result.Count == 1);
			HasValidSampleCountry (sampleCountry);
		}
		
		[Test()]
		public void Calling_DstService_WithoutPlacesForEveryCountry_Should_ReturnAllDstWithoutPlaces ()
		{
			// Arrage
			var year = 2014;
			
			// Act
			var service = new DSTService (Config.AccessKey, Config.SecretKey);			
			service.IncludePlacesForEveryCountry = false;
			var result = service.GetDaylightSavingTime (year);		
			var sampleCountry = result.SingleOrDefault (x => x.Region.Country.Name == "Norway");
			
			// Assert
			Assert.IsFalse (service.IncludePlacesForEveryCountry);
			Assert.IsTrue (result.All (x => x.Region.Locations.ToList().Count == 0));
			
			HasValidSampleCountry (sampleCountry);
		}
		
		[Test()]
		public void Calling_DstService_WithPlacesForEveryCountry_Should_ReturnAnyDstWithPlaces ()
		{
			// Arrage
			var year = 2014;

			// Act
			var service = new DSTService (Config.AccessKey, Config.SecretKey);			
			var result = service.GetDaylightSavingTime (year);
			var sampleCountry = result.SingleOrDefault (x => x.Region.Country.Name == "Norway");
			
			// Assert
			Assert.IsTrue (service.IncludePlacesForEveryCountry);
			Assert.IsTrue (result.Any (x => x.Region.Locations.ToList().Count > 0));
			
			HasValidSampleCountry (sampleCountry);
		}
		
		[Test()]
		public void Calling_DstService_WithoutTimeChanges_Should_NotReturnAnyTimeChanges ()
		{
			// Arrage
			var year = 2014;

			// Act
			var service = new DSTService (Config.AccessKey, Config.SecretKey);			
			service.IncludeTimeChanges = false;
			var result = service.GetDaylightSavingTime (year);
			var sampleCountry = result.SingleOrDefault (x => x.Region.Country.Name == "Norway");
			
			// Assert
			Assert.IsFalse (service.IncludeTimeChanges);
			Assert.IsTrue (result.All (x => x.TimeChanges.ToList().Count == 0));
			
			HasValidSampleCountry (sampleCountry);
		}
		
		[Test()]
		public void Calling_DstService_WithTimeChanges_Should_ReturnAnyTimeChanges ()
		{
			// Arrage
			var year = 2014;

			// Act
			var service = new DSTService (Config.AccessKey, Config.SecretKey);			
			service.IncludeTimeChanges = true;
			var result = service.GetDaylightSavingTime (year);
			var sampleCountry = result.SingleOrDefault (x => x.Region.Country.Name == "Norway");
			
			// Assert
			Assert.IsTrue (service.IncludeTimeChanges);
			Assert.IsTrue (result.Any (x => x.TimeChanges.ToList().Count > 0));
			
			HasValidSampleCountry (sampleCountry);
		}
		
		[Test()]
		public void Calling_DstService_WithOnlyDstCountries_Should_ReturnOnlyDstCountries ()
		{
			// Arrage
			var year = 2014;

			// Act
			var service = new DSTService (Config.AccessKey, Config.SecretKey);			
			service.IncludeOnlyDstCountries = true;
			var result = service.GetDaylightSavingTime (year);
			var sampleCountry = result.SingleOrDefault (x => x.Region.Country.Name == "Norway");
			
			// Assert
			Assert.IsTrue (service.IncludeOnlyDstCountries);
			Assert.AreEqual (138, result.Count);
			
			HasValidSampleCountry (sampleCountry);
		}
		
		[Test()]
		public void Calling_DstService_WithoutOnlyDstCountries_Should_ReturnAllCountries ()
		{
			// Arrage
			var year = 2014;

			// Act
			var service = new DSTService (Config.AccessKey, Config.SecretKey);			
			service.IncludeOnlyDstCountries = false;
			var result = service.GetDaylightSavingTime (year);
			var dstAllYear = result.Where (x => x.Special == DSTSpecialType.DaylightSavingTimeAllYear);
			var noDstAllYear = result.Where (x => x.Special == DSTSpecialType.NoDaylightSavingTime);
			var sampleCountry = result.SingleOrDefault (x => x.Region.Country.Name == "Norway");

			// Assert
			Assert.IsFalse (service.IncludeOnlyDstCountries);
			Assert.AreEqual (320, result.Count);
			Assert.Greater (dstAllYear.Count(), 0);
			Assert.Greater (noDstAllYear.Count(), 0);
			
			HasValidSampleCountry (sampleCountry);
		}
		
		public void HasValidSampleCountry (DST norway)
		{
			Assert.AreEqual ("Oslo", norway.Region.BiggestPlace);
			Assert.AreEqual ("no", norway.Region.Country.Id);
			
			Assert.Greater (norway.DstEnd.Year, 1);
			Assert.Greater (norway.DstStart.Year, 1);
			Assert.AreEqual ("CEST", norway.DstTimezone.Abbrevation);
			Assert.AreEqual (3600, norway.DstTimezone.BasicOffset);
			Assert.AreEqual (3600, norway.DstTimezone.DSTOffset);
			Assert.AreEqual (7200, norway.DstTimezone.TotalOffset);
			Assert.AreEqual ("Central European Summer Time", norway.DstTimezone.Name);
			Assert.AreEqual (2, norway.DstTimezone.Offset.Hours);
			Assert.AreEqual (0, norway.DstTimezone.Offset.Minutes);
			
			Assert.AreEqual ("CET", norway.StandardTimezone.Abbrevation);
			Assert.AreEqual (3600, norway.StandardTimezone.BasicOffset);
			Assert.AreEqual (0, norway.StandardTimezone.DSTOffset);
			Assert.AreEqual (3600, norway.StandardTimezone.TotalOffset);
			Assert.AreEqual ("Central European Time", norway.StandardTimezone.Name);
			Assert.AreEqual (1, norway.StandardTimezone.Offset.Hours);			
			Assert.AreEqual (0, norway.StandardTimezone.Offset.Minutes);			
			
			Assert.IsNotNull (norway.Region.Description);
			Assert.IsNotEmpty (norway.Region.Description);
		}
	}
}

