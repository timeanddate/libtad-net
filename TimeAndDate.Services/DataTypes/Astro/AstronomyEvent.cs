using System;
using System.Xml;
using System.Collections.Generic;
using TimeAndDate.Services.Common;

namespace TimeAndDate.Services.DataTypes.Astro
{
	public class AstronomyEvent
	{		
		/// <summary>
		/// Indicates the type of the event.
		/// </summary>
		/// <value>
		/// The type.
		/// </value>
		public AstronomyEventType Type { get; set; }
		
		/// <summary>
		/// Local time at which the event is happening
		/// </summary>
		/// <value>
		/// The time.
		/// </value>
		public DateTime Time { get; set; }
		
		public static explicit operator AstronomyEvent (XmlNode node)
		{
			var model = new AstronomyEvent ();
			var type = node.Attributes ["type"];
			var hour = node.Attributes ["hour"];
			var minute = node.Attributes ["minute"];
			
			if (type != null)
			{
				AstronomyEventType etype;
				if (!AstronomyEventType.TryParse (type.InnerText, true, out etype)) 
					throw new MalformedXMLException ("The XML Received from Time and Date did not include an " +
						"event type which complies with an AstronomyEventType enum: " + type.InnerText);
				
				model.Type = etype;
			}
				
			int h = 0, m = 0;
			if (hour != null)
				h = Int32.Parse (hour.InnerText);
			
			if (minute != null)
				m = Int32.Parse (minute.InnerText);
			model.Time = new DateTime ().AddHours (h).AddMinutes (m);
			
			return model;
		}
	}

}

