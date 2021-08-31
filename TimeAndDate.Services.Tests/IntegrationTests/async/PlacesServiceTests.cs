using System;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Linq;
using TimeAndDate.Services.Tests;
using TimeAndDate.Services;
using TimeAndDate.Services.DataTypes.Places;

namespace TimeAndDate.Services.Tests.IntegrationTests
{
	[TestFixture()]
	public class PlacesServiceTestsAsync
	{
		[Test()]
		public async Task Calling_PlacesServices_Should_ReturnListOfPlaces ()
		{
			// Arrange			
							
			// Act			
			var service = new PlacesService (Config.AccessKey, Config.SecretKey);
			var places = await service.GetPlacesAsync ();
			
			// Assert
			Assert.Greater(places.Count, 0);
			Assert.IsTrue (places.Any (x => x.Geography.Coordinates.Latitude > 0.0m));
			Assert.IsTrue (places.Any (x => x.Geography.Coordinates.Longitude > 0.0m));
		}
		
		[Test()]
		public async Task Calling_PlacesServices_WithoutGeo_Should_ReturnListOfPlacesWithoutGeo ()
		{
			// Arrange
				
			// Act
			var service = new PlacesService (Config.AccessKey, Config.SecretKey);
			service.IncludeCoordinates = false;
			var places = await service.GetPlacesAsync ();
						
			// Assert
			Assert.Greater(places.Count, 0);
			Assert.IsTrue (places.All (x => x.Geography.Coordinates == null));			
		}
	}
}

