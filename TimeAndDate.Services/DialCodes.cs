using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Xml;
using TimeAndDate.Services.DataTypes.DialCode;
using TimeAndDate.Services.DataTypes.Places;

namespace TimeAndDate.Services
{
	public class DialCodes
	{
		public string Number { get; set; }
		
		public IList<Composition> Compositions { get; set; }
		
		public IList<Location> Locations { get; set; }
		
		public DialCodes ()
		{
			Compositions = new List<Composition> ();
			Locations = new List<Location> ();
		}
		
		public static explicit operator DialCodes (string result)
		{
			var model = new DialCodes ();
			var xml = new XmlDocument ();
			xml.LoadXml (result);
			
			var dataNode = xml.DocumentElement;
			var number = dataNode.SelectSingleNode ("number/full");
			
			if (number != null)
				model.Number = number.InnerText;
			
			var compositions = dataNode.SelectSingleNode ("composition");
			if (compositions != null)
				foreach (XmlNode composition in compositions)
					model.Compositions.Add ((Composition)composition);
			
			var locations = dataNode.GetElementsByTagName ("location");
			if(locations != null)
				foreach(XmlNode location in locations)
					model.Locations.Add((Location)location);
			
			return model;
		}
	}

}

