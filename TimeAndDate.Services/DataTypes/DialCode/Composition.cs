using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Xml;
using TimeAndDate.Services.Common;

namespace TimeAndDate.Services.DataTypes.DialCode
{
	public class Composition 
	{
		/// <summary>
		/// Type of the phone number element.
		/// </summary>
		/// <value>
		/// The phone number element.
		/// </value>
		public PhoneNumberElementType PhoneNumberElement { get; set; }
				
		/// <summary>
		/// The actual number part. May contain characters as variable if 
		/// no number was supplied to the service (for the local-number part).
		/// </summary>
		/// <value>
		/// The number.
		/// </value>
		public string Number { get; set; }
		
		/// <summary>
		/// Textual description of the composition part.
		/// </summary>
		/// <value>
		/// The description.
		/// </value>
		public string Description { get; set; }
		
		public static explicit operator Composition (XmlNode node)
		{
			var model = new Composition ();
			var id = node.Attributes ["id"];
			var number = node.Attributes ["number"];
			var desc = node.Attributes ["description"];
			
			if (id != null)
				model.PhoneNumberElement = GetTypeByNode (id);						
			
			if (number != null)
				model.Number = number.InnerText;
			
			if (desc != null)
				model.Description = desc.InnerText;
			
			return model;
		}
		
		private static PhoneNumberElementType GetTypeByNode (XmlAttribute node)
		{
			var str = node.InnerText;
			switch (str)
			{
			case "international-prefix":
				return PhoneNumberElementType.InternationalPrefix;
			case "country-prefix":
				return PhoneNumberElementType.CountryPrefix;
			case "national-prefix":
				return PhoneNumberElementType.NationalPrefix;
			case "unknown-national-prefix":
				return PhoneNumberElementType.UnknownNationalPrefix;
			case "national-code":
				return PhoneNumberElementType.NationalCode;
			case "area-code":
				return PhoneNumberElementType.AreaCode;
			case "local-number":
				return PhoneNumberElementType.LocalNumber;
			default:
				throw new MalformedXMLException (
					"The XML Received from Time and Date did not include an object name which complies with an AstronomyObjectType enum: " + 
					str
				);
			}
		}
	}
}

