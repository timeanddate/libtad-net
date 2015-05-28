using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using TimeAndDate.Services.Common;

namespace TimeAndDate.Services.DataTypes.Astro
{
	public class Astronomy
	{
		/// <summary>
		/// Object name. Currently, the sun is the only supported astronomical object.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public AstronomyObjectType Name { get; set; } 
		
		/// <summary>
		/// Lists all sunrise/sunset events during the day.
		/// </summary>
		/// <value>
		/// The events.
		/// </value>
		public List<AstronomyEvent> Events { get; set; }
		
		/// <summary>
		/// This element is only present if there are no astronomical events. In this 
		/// case it will indicate if the sun is up or down the whole day.
		/// </summary>
		/// <value>
		/// The special.
		/// </value>
		public AstronomySpecial Special { get; set; }
		
		public DateTime? Sunset
		{
			get
			{
				var sets = Events.Where (x => x.Type == AstronomyEventType.Set).OrderBy (x => x.Time).ToList ();				
														
				if (sets.Count == 1)
				{
					return sets.SingleOrDefault ().Time;	
				} 
				
				if (sets.Count > 1)
				{
					return sets.LastOrDefault ().Time;
				}
				
				return null;
			}
		}
		
		/// <summary>
		/// This returns the hour and minute of the sunrise in DateTime format. If there is no sunrise, null will be
		/// returned, but the Special-property will say whether or not the sun is up or down. 
		/// </summary>
		/// <value>
		/// The sunrise in DateTime. Null if there is no sunrise that day. 
		/// </value>
		public DateTime? Sunrise
		{
			get
			{
				var rises = Events.Where (x => x.Type == AstronomyEventType.Rise).OrderBy (x => x.Time).ToList ();				
														
				if (rises.Count == 1)
				{
					return rises.SingleOrDefault ().Time;	
				} 
				
				if (rises.Count > 1)
				{
					return rises.LastOrDefault ().Time;
				}
				
				return null;
			}
		}
		
		public Astronomy ()
		{
			Events = new List<AstronomyEvent> ();
		}
		
		public static explicit operator Astronomy (XmlNode node)
		{
			var model = new Astronomy ();
			var name = node.Attributes ["name"];
			var events = node.SelectNodes ("event");
			
			AstronomyObjectType type;
			if (name != null && !AstronomyObjectType.TryParse (name.InnerText, true, out type))
				throw new MalformedXMLException (
					"The XML Received from Time and Date did not include an object name which complies with an AstronomyObjectType enum: " + 
					name.InnerText
				);				
							
			model.Name = type;
		
			if (node.SelectSingleNode ("special") != null)
				model.Special = (AstronomySpecial)node.SelectSingleNode ("special");
			
			if (events != null)
				foreach (XmlNode child in events)
					model.Events.Add ((AstronomyEvent)child);
						
			return model;
		}	
	}
}

