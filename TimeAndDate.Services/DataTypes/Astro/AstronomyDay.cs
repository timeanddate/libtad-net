using System;
using System.Collections.Generic;
using System.Xml;
using TimeAndDate.Services.Common;

namespace TimeAndDate.Services.DataTypes.Astro
{
	public class AstronomyDay
	{

		/// <summary>
		/// Date for the current information.
		/// </summary>
		/// <value>
		/// The date.
		/// </value>
		public DateTime? Date { get; set; }
		
		/// <summary>
		/// Length of this day (time between sunrise and sunset). If the sun is not 
		/// up on this day, 00:00 will reported. If the sun does not set on this day, 
		/// the value will read 24:00. 
		/// </summary>
		/// <value>
		/// The length of the day.
		/// </value>
		public TimeSpan DayLength { get; set; }
		
		/// <summary>
		/// Moon phase for the day. Only if requested.
		/// </summary>
		/// <value>
		/// The moon phase.
		/// </value>
		public MoonPhase MoonPhase { get; set; }
		
		/// <summary>
		/// Lists all events during the day.
		/// </summary>
		/// <value>
		/// The events.
		/// </value>
		public List<AstronomyDayEvent> Events { get; set; }
		
		private AstronomyDay ()
		{
			Events = new List<AstronomyDayEvent> ();
		}
		
		public static explicit operator AstronomyDay (XmlNode node)
		{
			var model = new AstronomyDay ();
			
			var date = node.Attributes ["date"];
			var daylength = node.Attributes ["daylength"];
			var moonphase = node.Attributes ["moonphase"];
			var events = node.SelectNodes ("event");
			
			if (date != null)
				model.Date = DateTime.Parse (date.InnerText);
			
			if (daylength != null)
				model.DayLength = TimeSpan.Parse (daylength.InnerText);
			
			MoonPhase phase = MoonPhase.NotRequested;
			if (moonphase != null)
			if (!MoonPhase.TryParse (moonphase.InnerText, true, out phase))
				throw new MalformedXMLException ("The XML returned from Time and Date contained an unsupported moonphase: " + moonphase.InnerText);
			
			if (events != null)
				foreach (XmlNode day in events)
					model.Events.Add ((AstronomyDayEvent)day);
			
			return model;
		}
	}

}

