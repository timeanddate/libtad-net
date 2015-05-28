using System;
namespace TimeAndDate.Services
{
	public class Coordinates
	{
		public decimal Latitude { get; private set; }
		
		public decimal Longitude { get; private set; }
		
		public Coordinates (decimal Latitude, decimal Longitude)
		{
			this.Latitude = Latitude;
			this.Longitude = Longitude;
		}
	}
}

