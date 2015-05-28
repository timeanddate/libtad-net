using System;
using NUnit.Framework;
using System.Linq;
using TimeAndDate.Services.Tests;
using TimeAndDate.Services;
using TimeAndDate.Services.DataTypes.Places;

namespace TimeAndDate.Services.Tests.IntegrationTests
{
	[TestFixture()]
	public class PlacesServiceTests
	{
		[Test()]
		public void Calling_PlacesServices_Should_ReturnListOfPlaces ()
		{
			// Arrange			
							
			// Act			
			var service = new PlacesService (Config.AccessKey, Config.SecretKey);
			var places = service.GetPlaces ();
			
			// Assert
			Assert.IsNotEmpty (places);
			Assert.IsTrue (places.Any (x => x.Geography.Coordinates.Latitude > 0.0m));
			Assert.IsTrue (places.Any (x => x.Geography.Coordinates.Longitude > 0.0m));
		}
		
		[Test()]
		public void Calling_PlacesServices_WithoutGeo_Should_ReturnListOfPlacesWithoutGeo ()
		{
			// Arrange
				
			// Act
			var service = new PlacesService (Config.AccessKey, Config.SecretKey);
			service.IncludeCoordinates = false;
			var places = service.GetPlaces ();
						
			// Assert
			Assert.IsNotEmpty (places);
			Assert.IsTrue (places.All (x => x.Geography.Coordinates == null));			
		}
	}
}

