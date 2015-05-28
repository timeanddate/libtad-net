using System;
using System.Xml;
using System.Collections.Generic;
using TimeAndDate.Services.Common;

namespace TimeAndDate.Services.DataTypes.Astro
{
	public class AstronomySpecial
	{
		/// <summary>
		/// Indicates if the sun is up or down all day.
		/// </summary>
		/// <value>
		/// The type.
		/// </value>
		public AstronomyEventType Type { get; set; } 
		
		public static explicit operator AstronomySpecial (XmlNode node)
		{
			var model = new AstronomySpecial ();
			
			if (node.Attributes ["type"] != null)
			{
				AstronomyEventType etype;
				if (!AstronomyEventType.TryParse (node.Attributes ["type"].InnerText, true, out etype)) 
					throw new MalformedXMLException ("The XML Received from Time and Date did not include an event type which complies with an AstronomyEventType enum");
				model.Type = etype;
			}
			
			return model;
		}
	}

}

