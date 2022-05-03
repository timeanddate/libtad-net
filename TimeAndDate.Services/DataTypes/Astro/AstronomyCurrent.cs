using System;
using System.Globalization;
using System.Collections.Generic;
using System.Xml;
using TimeAndDate.Services.Common;
using TimeAndDate.Services.DataTypes.Time;


namespace TimeAndDate.Services.DataTypes.Astro
{
	public class AstronomyCurrent
	{
		/// <summary>
		/// Local time stamp for the data.
		/// Only returned if requested by specifying the parameter isotime.
		/// </summary>
		/// <value>
		/// The local time.
		/// </value>
		public TADDateTime IsoTime { get; set; }

		/// <summary>
		/// UTC time stamp for the data.
		/// Only returned if requested by specifying the parameter utctime.
		/// </summary>
		/// <value>
		/// The UTC time.
		/// </value>
		public TADDateTime UtcTime { get; set; }

		/// <summary>
		/// Altitude of the center of the queried astronomical object above an ideal horizon.
		/// </summary>
		/// <value>
		/// Altitude of the object.
		/// </value>
		public float Altitude { get; set; }

		/// <summary>
		/// Horizontal direction of the astronomical object at set/rise time (referring to true north).
		/// North is 0 degrees, east is 90 degrees, south is 180 degrees and west is 270 degrees;
		/// </summary>
		/// <value>
		/// Direction in degrees.
		/// </value>
		public float Azimuth { get; set; }

		/// <summary>
		/// Distance of the earth's center to the center of the queried astronomical object in kilometers.
		/// </summary>
		/// <value>
		/// Distance in kilometers.
		/// </value>
		public float Distance { get; set; }

		/// <summary>
		/// The fraction of the Moon's surface illuminated by the Sun's rays as seen from the selected location.
		/// Only available for the moon object.
		/// </summary>
		/// <value>
		/// Illumination in percent.
		/// </value>
		public float Illuminated { get; set; }

		/// <summary>
		/// The counterclockwise angle of the midpoint of the Moon's bright limb as seen from the selected location.
		/// Only available for the moon object.
		/// </summary>
		/// <value>
		/// Angle in degrees.
		/// <value>
		public float Posangle { get; set; }

		/// <summary>
		/// The current phase of the moon. Only available for the moon object.
		/// </summary>
		/// <value>
		/// The moon phase variant.
		/// </value>
		public MoonPhase Moonphase { get; set; }
	
		public static explicit operator AstronomyCurrent (XmlNode node)
		{
			var model = new AstronomyCurrent ();
			var name = node.Attributes ["name"];
			var days = node.SelectNodes ("day");
			var current = node.SelectSingleNode ("current");
			var results = node.SelectNodes ("result");

			var isotime = node.Attributes ["isotime"];
			var utctime = node.Attributes ["utctime"];
			var altitude = node.SelectSingleNode ("altitude");
			var azimuth = node.SelectSingleNode ("azimuth");
			var distance = node.SelectSingleNode ("distance");
			var illuminated = node.SelectSingleNode ("illuminated");
			var posangle = node.SelectSingleNode ("posangle");
			var moonphase = node.SelectSingleNode ("moonphase");

			MoonPhase phase = MoonPhase.NotRequested;
			if (moonphase != null && !MoonPhase.TryParse (moonphase.InnerText, true, out phase))
				throw new MalformedXMLException ("The XML returned from Time and Date contained an unsupported name: " + moonphase.InnerText);
							
			model.Moonphase = phase;	

			if (isotime != null)
				model.IsoTime = new TADDateTime (isotime.InnerText);

			if (utctime != null)
				model.UtcTime = new TADDateTime (utctime.InnerText);

			if (altitude != null)
				model.Altitude = Single.Parse(altitude.InnerText);

			if (azimuth != null)
				model.Azimuth = Single.Parse(azimuth.InnerText);

			if (distance != null)
				model.Distance = Single.Parse(distance.InnerText);

			if (illuminated != null)
				model.Illuminated = Single.Parse(illuminated.InnerText);

			if (posangle != null)
				model.Posangle = Single.Parse(posangle.InnerText);

			return model;
		}
	}
}
