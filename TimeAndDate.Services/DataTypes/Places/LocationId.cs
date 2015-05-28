using System;
namespace TimeAndDate.Services.DataTypes.Places
{
	/// <summary>
	/// LocationId class represents the different kinds of IDs that Time and Date API
	/// does support. See <see cref="https://services.timeanddate.com/api/doc/v2/type-locationid.html" />
	/// for detailed information.
	/// </summary>
	public class LocationId
	{
		public string TextualId { get; private set; }
		
		public int? NumericId;
		
		public Coordinates CoordinatesId { get; private set; }
				
		/// <summary>
		/// Create a LocationId based on a textual ID
		/// </summary>
		/// <param name='textualId'>
		/// Can be country code, country name, city, etc
		/// </param>
		public LocationId (string textualId)
		{
			TextualId = textualId;
		}
		
		/// <summary>
		/// Create a LocationId based on an internal integer ID
		/// </summary>
		/// <param name='numericId'>
		/// Usually an integer ID that is returned from a previous API call
		/// </param>
		public LocationId (int numericId)
		{
			NumericId = numericId;
		}
		
		/// <summary>
		/// Create a LocationId based on coordinates
		/// </summary>
		/// <param name='coordinates'>
		/// Provide an coordinate object to LocationId.
		/// </param>
		public LocationId (Coordinates coordinates)
		{
			CoordinatesId = coordinates;
		}
		
		public static explicit operator LocationId (Place place)
		{
			return new LocationId (place.Id);
		}
	}
}

