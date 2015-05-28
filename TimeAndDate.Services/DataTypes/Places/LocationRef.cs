using System;
using System.Xml;


namespace TimeAndDate.Services.DataTypes.Places
{
	public class LocationRef
	{
		/// <summary>
		/// The id of the location.
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		public string Id { get; set; }
		
		/// <summary>
		/// The name of the location
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Name { get; set; }
		
		/// <summary>
		/// The state of the location within the country (only if applicable).
		/// </summary>
		/// <value>
		/// The state.
		/// </value>
		public string State { get; set; }
		
		public static explicit operator LocationRef (XmlNode node)
		{
			var model = new LocationRef ();
			
			var id = node.Attributes ["id"];
			var name = node.Attributes ["name"];
			var state = node.Attributes ["state"];
			
			if (id != null)
				model.Id = id.InnerText;
			
			if (name != null)
				model.Name = name.InnerText;
			
			if (state != null)
				model.State = state.InnerText;
			
			return model;
		}
	}
}

