using System;
using System.Collections.Generic;
using System.Xml;
using TimeAndDate.Services.DataTypes.Places;
using TimeAndDate.Services.DataTypes.Time;

namespace TimeAndDate.Services
{
	public class ConvertedTimes
	{
		/// <summary>
		/// UTC time stamp of requested time.
		/// </summary>
		/// <value>
		/// The UTC.
		/// </value>
		public TADTime Utc { get; set; }
		
		/// <summary>
		/// This element contains the time information for the locations mentioned in the request. 
		/// </summary>
		/// <value>
		/// The locations.
		/// </value>
		public IList<Location> Locations { get; set; }
				
		public ConvertedTimes ()
		{
			Locations = new List<Location> ();
		}

        	public ConvertedTimes(IList<Location> locations, TADTime utc)
        	{
			Utc = utc;
			Locations = locations;
		}
	}
}
