using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Xml;
using TimeAndDate.Services.Common;
using TimeAndDate.Services.DataTypes.Places;
using TimeAndDate.Services.DataTypes.Time;

namespace TimeAndDate.Services.DataTypes.OnThisDay
{
	public class Person
	{
		/// <summary>
		/// Identifier for the person.
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		public int Id { get; set; }
		
		/// <summary>
		/// Full name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public Name Name { get; set; }
		
		/// <summary>
		/// Date of birth.
		/// </summary>
		/// <value>
		/// The date.
		/// </value>
		public TADTime BirthDate { get; set; } 

		/// <summary>
		/// Date of death, if applicable.
		/// </summary>
		/// <value>
		/// The date.
		/// </value>
		public TADTime DeathDate { get; set; } 
		
		/// <summary>
		/// Person categories.
		/// </summary>
		/// <value>
		/// A list of categories.
		/// </value>
		public List<string> Category { get; set; }
		
		/// <summary>
		/// Person nationalities.
		/// </summary>
		/// <value>
		/// A list of nationalities.
		/// </value>
		public List<string> Nationality { get; set; }
		
		private Person ()
		{
			Category = new List<string> ();
			Nationality = new List<string> ();
		}
		
		public static explicit operator Person (XmlNode node)
		{
			var model = new Person ();
			var id = node.Attributes ["id"];			
			var name = node.SelectSingleNode ("name");
			var birthdate = node.SelectSingleNode ("birthdate");
			var deathdate = node.SelectSingleNode ("deathdate");
			var category = node.SelectSingleNode ("categories");
			var nationality = node.SelectSingleNode ("nationalities");

			if (id != null)
				model.Id = Int32.Parse (id.InnerText);

			if (name != null)
				model.Name = (Name) name;

			if (birthdate != null)
				model.BirthDate = (TADTime) birthdate;

			if (deathdate != null)
				model.DeathDate = (TADTime) deathdate;

			if (category != null)
			{
				foreach (XmlNode child in category.ChildNodes)
					model.Category.Add (child.InnerText);
			}

			
			if (nationality != null)
			{
				foreach (XmlNode child in nationality.ChildNodes)
					model.Nationality.Add (child.InnerText);
			}
			
			return model;
		}			
	}
}

