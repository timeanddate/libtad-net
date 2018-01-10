using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Xml;

namespace TimeAndDate.Services.DataTypes.Holidays
{
	public class HolidayState
	{
		/// <summary>
		/// Unique id of the state/subdivision.
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		public int Id { get; set; }
		
		/// <summary>
		/// Abbreviation of the state/subdivision.
		/// </summary>
		/// <value>
		/// The abbrevation.
		/// </value>
		public string Abbrevation { get; set; }
		
		/// <summary>
		/// Common name of the state/subdivision.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Name { get; set; }
		
		/// <summary>
		/// Eventual exception if the holiday does not affect 
		/// the whole state/subdivision.
		/// </summary>
		/// <value>
		/// The exception.
		/// </value>
		public string Exception { get; set; }
		
		public static explicit operator HolidayState (XmlNode node)
		{
			var model = new HolidayState ();
			var id = node.SelectSingleNode ("id");
			var abbr = node.SelectSingleNode ("abbrev");
			var name = node.SelectSingleNode ("name");
			var excp = node.SelectSingleNode ("exception");
			
			if (id != null)
				model.Id = Int32.Parse (id.InnerText);
			if (abbr != null)
				model.Abbrevation = abbr.InnerText;
			if (name != null)
				model.Name = name.InnerText;
			if (excp != null)
				model.Exception = excp.InnerText;
			
			return model;
		}
	}



}

