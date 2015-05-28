using System;
using System.Collections.Generic;
using System.Xml;
using TimeAndDate.Services.DataTypes.Places;

namespace TimeAndDate.Services.DataTypes.Astro
{
	public class AstronomyLocation
	{
		/// <summary>
		/// The id of the location.
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		public string Id {get; set; }
		
		/// <summary>
		/// Geographical information about the location.
		/// </summary>
		/// <value>
		/// The geography.
		/// </value>
		public Geo Geography { get; set; }
		
		/// <summary>
		/// Requested astronomical information.
		/// </summary>
		/// <value>
		/// The objects.
		/// </value>
		public List<AstronomyObjectDetails> Objects { get; set; }
		
		private AstronomyLocation ()
		{
			Objects = new List<AstronomyObjectDetails> ();
		}
		
		public static explicit operator AstronomyLocation (XmlNode node)
		{
			var model = new AstronomyLocation ();
			
			var id = node.Attributes ["id"];
			var geo = node.SelectSingleNode ("geo");
			var astro = node.SelectSingleNode ("astronomy");
			
			if (id != null)
				model.Id = id.InnerText;
			
			if (geo != null)
				model.Geography = (Geo)geo;
						
			if (astro != null)
				foreach (XmlNode astroObject in astro.ChildNodes)
					model.Objects.Add ((AstronomyObjectDetails)astroObject);
			
			return model;
		}
	}
}

