using System;
using System.Xml;


namespace TimeAndDate.Services.DataTypes.Places
{
	public class Country
	{
		/// <summary>
		/// The ISO 3166-1-alpha-2 country code
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		public string Id { get; set; }
		
		/// <summary>
		/// Full name of the country.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Name { get; set; }
		
		public static explicit operator Country (XmlNode node)
		{
			var model = new Country ();
            if (node != null)
            {
				model.Name = node.InnerText;
				
                if (node.Attributes["id"] != null)
                {
					model.Id = node.Attributes ["id"].InnerText;
				}									
			}
			
			return model;	
		}
	}
}

