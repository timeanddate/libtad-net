using System;
using System.Xml;
using System.Globalization;


namespace TimeAndDate.Services.DataTypes.Places
{
	public class Geo
	{
		/// <summary>
		/// The name of the location
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Name { get; set; }
		
		/// <summary>
		/// The state of the location within the country 
		/// (only if applicable).
		/// </summary>
		/// <value>
		/// The state.
		/// </value>
		public string State { get; set; }
		
		/// <summary>
		/// Country of the location.
		/// </summary>
		/// <value>
		/// The country.
		/// </value>
		public Country Country { get; set; }
		
		/// <summary>
		/// Geographical coordinates of the location.
		/// </summary>
		/// <value>
		/// The coordinates.
		/// </value>
		public Coordinates Coordinates { get; set; }
		
		public static explicit operator Geo (XmlNode node)
		{
			var model = new Geo ();
			var latitude = node.SelectSingleNode ("latitude");
			var longitude = node.SelectSingleNode ("longitude");
			var name = node.SelectSingleNode ("name");
			var state = node.SelectSingleNode ("state");
			var country = node.SelectSingleNode ("country");
			
			if (latitude != null && longitude != null)
			{
				var la = Decimal.Parse (latitude.InnerText, CultureInfo.InvariantCulture);			
				var lo = Decimal.Parse (longitude.InnerText, CultureInfo.InvariantCulture);			
				model.Coordinates = new Coordinates (la, lo);
			}
							
			if (name != null)
				model.Name = name.InnerText;
			
			if (state != null)
				model.State = state.InnerText;
			
			if (country != null)
				model.Country = (Country)country;
			
			return model;
		}
	}
}

