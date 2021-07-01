using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Xml;
using TimeAndDate.Services.Common;
using TimeAndDate.Services.DataTypes.Places;
using TimeAndDate.Services.DataTypes.Time;

namespace TimeAndDate.Services.DataTypes.OnThisDay
{
	public class Event
	{
		/// <summary>
		/// Identifier for the event.
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		public int Id { get; set; }

		/// <summary>
		/// Event name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Name { get; set; }

		/// <summary>
		/// Date of the event.
		/// </summary>
		/// <value>
		/// The date.
		/// </value>
		public TADTime Date { get; set; }

		/// <summary>
		/// Location of the event.
		/// </summary>
		/// <value>
		/// The location.
		/// </value>
		public string Location { get; set; }

		/// <summary>
		/// Event categories.
		/// </summary>
		/// <value>
		/// A list of categories.
		/// </value>
		public List<string> Category { get; set; }

		/// <summary>
		/// Countries related to the event.
		/// </summary>
		/// <value>
		/// A list of countries.
		/// </value>
		public List<Country> Country { get; set; }

		/// <summary>
		/// A short description of the event.
		/// </summary>
		/// <value>
		/// The description.
		/// </value>
		public string Description { get; set; }

		private Event ()
		{
			Category = new List<string> ();
			Country = new List<Country> ();
		}

		public static explicit operator Event (XmlNode node)
		{
			var model = new Event ();
			var id = node.Attributes ["id"];
			var name = node.SelectSingleNode ("name");
			var date = node.SelectSingleNode ("date");
			var location = node.SelectSingleNode ("location");
			var category = node.SelectSingleNode ("categories");
			var country = node.SelectSingleNode ("countries");
			var desc = node.SelectSingleNode ("description");

			if (id != null)
				model.Id = Int32.Parse (id.InnerText);

			if (name != null)
				model.Name = name.InnerText;

			if (date != null)
				model.Date = (TADTime) date;

			if (location != null)
				model.Location = location.InnerText;

			if (category != null)
			{
				foreach (XmlNode child in category.ChildNodes)
					model.Category.Add (child.InnerText);
			}


			if (country != null)
			{
				foreach (XmlNode child in country.ChildNodes)
					model.Country.Add ((Country) child);
			}

			if (desc != null)
				model.Description = desc.InnerText;

			return model;
		}
	}
}

