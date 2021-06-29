using System;
using System.Collections.Generic;
using System.Xml;
using TimeAndDate.Services.DataTypes.Astro;
using TimeAndDate.Services.DataTypes.Time;
using System.Linq;
using TimeAndDate.Services.Common;

namespace TimeAndDate.Services.DataTypes.Places
{
	public class Location
	{
		/// <summary>
		/// The id of the location.
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		public string Id { get; set; }
		
		/// <summary>
		/// Geographical information about the location.
		/// </summary>
		/// <value>
		/// The geography.
		/// </value>
		public Geo Geography { get; set; }
		
		/// <summary>
		/// Time information about the location. Only present 
		/// if requested.
		/// </summary>
		/// <value>
		/// The time.
		/// </value>
		public TADTime Time { get; set; }
		
		/// <summary>
		/// Time changes (daylight savings time). Only present 
		/// if requested and information is available.
		/// </summary>
		/// <value>
		/// The time changes.
		/// </value>
		public IList<TimeChange> TimeChanges { get; set; }
		
		/// <summary>
		/// Astronomical information – sunrise and sunset times. 
		/// Only for the timeservice and if requested.
		/// </summary>
		/// <value>
		/// The astronomy.
		/// </value>
		public IList<Astronomy> Astronomy { get; set; }
		
		private Location ()
		{
			TimeChanges = new List<TimeChange> ();
			Astronomy = new List<Astronomy> ();
		}
		
		public static explicit operator Location (XmlNode node)
		{
			var location = new Location ();
			if (node.Attributes ["id"] != null)
				location.Id = node.Attributes ["id"].InnerText;
						
			var geo = node.SelectSingleNode ("geo");
			var time = node.SelectSingleNode ("time");
			var timeChanges = node.SelectSingleNode ("timechanges");
			var astronomy = node.SelectSingleNode ("astronomy");			
			
			if (geo != null)
				location.Geography = (Geo)geo;
			
			if (time != null)
				location.Time = (TADTime)time;
			
			if (timeChanges != null)
				foreach (XmlNode child in timeChanges.ChildNodes)
					location.TimeChanges.Add ((TimeChange)child);
			
			if (astronomy != null)
				foreach (XmlNode child in astronomy.ChildNodes)
					location.Astronomy.Add ((Astronomy)child);
			
			return location;
		}
	}
}