using System;
using System.Collections.Generic;
using System.Xml;
using TimeAndDate.Services.Common;
using System.Globalization;

namespace TimeAndDate.Services.DataTypes.Astro
{
	public class AstronomyDayEvent
	{
		/// <summary>
		/// Indicates the type of the event.
		/// </summary>
		/// <value>
		/// The type.
		/// </value>
		public AstronomyEventCode Type { get; set; }
		
		/// <summary>
		/// Local time at which the event is happening (including UTC offset). 
		/// The time does not include the seconds.
		/// </summary>
		/// <value>
		/// The ISO time.
		/// </value>
		public DateTimeOffset ISOTime { get; set; }
		
		/// <summary>
		/// UTC time at which the event is happening. The time does not include 
		/// the seconds.
		/// </summary>
		/// <value>
		/// The UTC time.
		/// </value>
		public DateTimeOffset UTCTime { get; set; }
		
		/// <summary>
		/// Altitude of the center of the queried astronomical object above an 
		/// ideal horizon.
		/// 
		/// Only for meridian type events.
		/// </summary>
		/// <value>
		/// The altitude. (degrees)
		/// </value>
		public decimal Altitude { get; set; }
		
		/// <summary>
		/// Horizontal direction of the astronomical object at set/rise time 
		/// (referring to true north). North is 0 degrees, east is 90 degrees, 
		/// south is 180 degrees and west is 270 degrees.
		/// 
		/// Only for rise and set type events.
		/// </summary>
		/// <value>
		/// The azimuth. (degrees)
		/// </value>
		public decimal Azimuth { get; set; }
		
		/// <summary>
		/// Distance of the earth's center to the center of the queried 
		/// astronomical object in kilometers.
		/// 
		/// Only for meridian type events.
		/// </summary>
		/// <value>
		/// The distance. (km)
		/// </value>
		public decimal Distance { get; set; }
		
		/// <summary>
		/// The fraction of the Moon's surface illuminated by the Sun's 
		/// rays as seen from the selected location.
		/// 
		/// Only for the moon for meridian type events.
		/// </summary>
		/// <value>
		/// The illuminated. (percent)
		/// </value>
		public decimal Illuminated { get; set; }
		
		public static explicit operator AstronomyDayEvent (XmlNode node)
		{
			var model = new AstronomyDayEvent ();
			var type = node.Attributes ["type"];
			var utctime = node.Attributes ["utctime"];
			var isotime = node.Attributes ["isotime"];
			var altitude = node.Attributes ["altitude"];
			var distance = node.Attributes ["distance"];
			var azimuth = node.Attributes ["azimuth"];
			var illuminated = node.Attributes ["illuminated"];
			
			if (type != null)
				model.Type = StringHelpers.ResolveAstronomyEventCode (type.InnerText);
			
			if (utctime != null)
				model.UTCTime = DateTimeOffset.Parse (utctime.InnerText);
			
			if (isotime != null)
				model.ISOTime = DateTimeOffset.Parse (isotime.InnerText);
			
			if (altitude != null)
				model.Altitude = decimal.Parse (altitude.InnerText, CultureInfo.InvariantCulture);
			
			if (distance != null)
				model.Distance = decimal.Parse (distance.InnerText, CultureInfo.InvariantCulture);
			
			if (azimuth != null)
				model.Azimuth = decimal.Parse (azimuth.InnerText, CultureInfo.InvariantCulture);
			
			if (illuminated != null)
				model.Illuminated = decimal.Parse (illuminated.InnerText, CultureInfo.InvariantCulture);
			
			return model;
		}
	}


}

