using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Xml;
using TimeAndDate.Services.Common;

namespace TimeAndDate.Services.DataTypes.OnThisDay
{
	public class Name
	{
		/// <summary>
		/// First name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string First { get; set; }
		
		/// <summary>
		/// Middle name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Middle { get; set; }
		
		/// <summary>
		/// Last name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Last { get; set; } 
		
		public static explicit operator Name (XmlNode node)
		{
			var model = new Name ();
			var first = node.SelectSingleNode ("first");
			var middle = node.SelectSingleNode ("middle");
			var last = node.SelectSingleNode ("last");

			if (first != null)
				model.First = first.InnerText;

			if (middle != null)
				model.Middle = middle.InnerText;

			if (last != null)
				model.Last = last.InnerText;

			return model;
		}			
	}
}

