using System;
using System.Collections.Generic;
using System.Xml;


namespace TimeAndDate.Services.DataTypes.Places
{
	public class Region
	{
		/// <summary>
		/// The country.
		/// </summary>
		/// <value>
		/// The country.
		/// </value>
		public Country Country { get; set; }
		
		/// <summary>
		/// Textual description of a region.
		///
		/// Example: All locations
		///
		/// Example: most of Newfoundland and Labrador
		///
		/// Example: some regions of Nunavut Territory; small region of Ontario
		/// </summary>
		/// <value>
		/// The description.
		/// </value>
		public string Description { get; set; }
		
		/// <summary>
		/// Name of the biggest city within the region
		/// </summary>
		/// <value>
		/// The biggest place.
		/// </value>
		public string BiggestPlace { get; set; }
		
		/// <summary>
		/// A list of all locations referenced by this region. Only returned if 
		/// requested by specifying the parameter IncludePlacesForEveryCountry
		/// on DaylightSavingTimeService
		/// </summary>
		/// <value>
		/// The locations.
		/// </value>
		public IList<LocationRef> Locations { get; set; }
		
		private Region ()
		{
			Locations = new List<LocationRef> ();
		}
		
		public static explicit operator Region (XmlNode node)
		{
			var model = new Region ();
			var country = node.SelectSingleNode ("country");
			var desc = node.SelectSingleNode ("desc");
			var biggestplace = node.SelectSingleNode ("biggestplace");
			var locations = node.SelectSingleNode ("locations");
			
			if (country != null)
				model.Country = (Country)country;
			
			if (desc != null)
				model.Description = desc.InnerText;
			
			if (biggestplace != null)
				model.BiggestPlace = biggestplace.InnerText;
			
			if (locations != null && locations.ChildNodes != null)
				foreach (XmlNode location in locations.ChildNodes)
					model.Locations.Add ((LocationRef)location);
			
			return model;
		}
	}
}

