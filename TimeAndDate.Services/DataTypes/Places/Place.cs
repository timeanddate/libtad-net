using System;
using System.Collections.Generic;
using System.Xml;

namespace TimeAndDate.Services.DataTypes.Places
{
	public class Place
	{
		/// <summary>
		/// Numerical id of the referenced place.
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		public int Id { get; set; }
		
		/// <summary>
		/// Textual id of the referenced place.
		/// </summary>
		/// <value>
		/// The urlid.
		/// </value>
		public string Urlid { get; set; }
		
		/// <summary>
		/// Geographical information about the location.
		/// </summary>
		/// <value>
		/// The geography.
		/// </value>
		public Geo Geography { get; set; }
		
		public static explicit operator Place (XmlNode node)
		{
			var model = new Place ();
			var id = node.Attributes ["id"];
			var urlid = node.Attributes ["urlid"];
			var geo = node.SelectSingleNode ("geo");
			
			if (id != null)
				model.Id = Int32.Parse (id.InnerText);
			
			if (urlid != null)
				model.Urlid = urlid.InnerText;
			
			if (geo != null)
				model.Geography = (Geo)geo;
			
			return model;
		}
	}

}

