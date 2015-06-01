using System;
using System.Collections.Generic;
using System.Xml;
using TimeAndDate.Services.Common;


namespace TimeAndDate.Services.DataTypes.Astro
{
	public class AstronomyObjectDetails
	{
		/// <summary>
		/// Object name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public AstronomyObjectType Name { get; set; }
	
		/// <summary>
		/// Lists all the requested days where events are happening.
		/// </summary>
		/// <value>
		/// The days.
		/// </value>
		public List<AstronomyDay> Days { get; set; }
		
		private AstronomyObjectDetails ()
		{
			Days = new List<AstronomyDay> ();
		}
		
		public static explicit operator AstronomyObjectDetails (XmlNode node)
		{
			var model = new AstronomyObjectDetails ();
			var name = node.Attributes ["name"];
			var days = node.SelectNodes ("day");
			
			AstronomyObjectType type = AstronomyObjectType.Sun;
			if (name != null && !AstronomyObjectType.TryParse (name.InnerText, true, out type))
				throw new MalformedXMLException ("The XML returned from Time and Date contained an unsupported name: " + name.InnerText);
							
			model.Name = type;
			
			if (days != null)
				foreach (XmlNode day in days)
					model.Days.Add ((AstronomyDay)day);
			
			return model;
		}
	}
}

