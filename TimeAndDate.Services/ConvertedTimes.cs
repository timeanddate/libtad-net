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
				
		private ConvertedTimes ()
		{
			Locations = new List<Location> ();
		}
		
		public static explicit operator ConvertedTimes (string result)
		{
			var model = new ConvertedTimes ();
			var xml = new XmlDocument ();
			
			xml.LoadXml (result);
			
			var utc = xml.SelectSingleNode ("data/utc/time");
			var locations = xml.GetElementsByTagName ("location");
			
			if (utc != null)
				model.Utc = (TADTime)utc;
			
			if (locations != null)
			{
				foreach (XmlNode location in locations)
				{
					model.Locations.Add ((Location)location);
				}
			}
			
			return model;
		}
	}

}

