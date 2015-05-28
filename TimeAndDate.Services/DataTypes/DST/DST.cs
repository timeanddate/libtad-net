using System;
using System.Collections.Generic;
using System.Xml;
using TimeAndDate.Services.DataTypes.Time;
using TimeAndDate.Services.DataTypes.Places;
using System.Diagnostics;

namespace TimeAndDate.Services.DataTypes.DST
{
	public class DST
	{		
		/// <summary>
		/// The geographical region where this information is valid. 
		/// Contains country, a textual description of the region and 
		/// the name of the biggest place.
		/// </summary>
		/// <value>
		/// The region.
		/// </value>
		public Region Region { get; set; }
		
		/// <summary>
		/// Information about the standard timezone. This element is 
		/// always returned.
		/// </summary>
		/// <value>
		/// The standard timezone.
		/// </value>
		public TADTimezone StandardTimezone { get; set; }
		
		/// <summary>
		/// Information about the daylight savings timezone. Suppressed, 
		/// if there are no DST changes in the queried year.
		///
		/// Please note that if the region is on daylight savings time 
		/// for the whole year, this information will be returned in the 
		/// stdtimezone element. Additionally, the Special element will 
		/// be set to DaylightSavingTimeAllYear.
		/// </summary>
		/// <value>
		/// The dst timezone.
		/// </value>
		public TADTimezone DstTimezone { get; set; }
		
		/// <summary>
		/// Indicates if the region does not observe DST at all, or is on 
		/// DST all year long.
		/// </summary>
		/// <value>
		/// The special.
		/// </value>
		public DSTSpecialType Special { get; set; }
		
		/// <summary>
		/// Starting date of daylight savings time. Suppressed, if there 
		/// are no DST changes in the queried year.
		/// </summary>
		/// <value>
		/// The dst start.
		/// </value>
		public DateTime DstStart { get; set; }
		
		/// <summary>
		/// Ending date of daylight savings time. Suppressed, if there are 
		/// no DST changes in the queried year.
		/// </summary>
		/// <value>
		/// The dst end.
		/// </value>
		public DateTime DstEnd { get; set; }
		
		/// <summary>
		/// Time changes (daylight savings time). Only present if requested 
		/// and information is available.
		/// </summary>
		/// <value>
		/// The time changes.
		/// </value>
		public IList<TimeChange> TimeChanges { get; set; }
		
		private DST ()
		{
			TimeChanges = new List<TimeChange> ();
		}
		
		public static explicit operator DST (XmlNode node)
		{
			var model = new DST ();
			var region = node.SelectSingleNode ("region");
			var stdtimezone = node.SelectSingleNode ("stdtimezone");
			var dsttimezone = node.SelectSingleNode ("dsttimezone");
			var dstend = node.SelectSingleNode ("dstend");
			var dststart = node.SelectSingleNode ("dststart");
			var timechanges = node.SelectSingleNode ("timechanges");			
			var specialNode = node.SelectSingleNode ("special");			
			var special = specialNode != null ? specialNode.Attributes ["type"] : null;
			
			if (region != null)
				model.Region = (Region)region;
			
			if (stdtimezone != null)
				model.StandardTimezone = (TADTimezone)stdtimezone;
			
			if (dsttimezone != null)
				model.DstTimezone = (TADTimezone)dsttimezone;
			
			if (dstend != null)
				model.DstEnd = DateTime.Parse (dstend.InnerText);
			
			if (dststart != null)
				model.DstStart = DateTime.Parse (dststart.InnerText);		
			
			if (special != null && special.InnerText == "nodst")
				model.Special = DSTSpecialType.NoDaylightSavingTime;
			else if (special != null && special.InnerText == "allyear")
				model.Special = DSTSpecialType.DaylightSavingTimeAllYear;
			
			if (timechanges != null)
				foreach (XmlNode timechange in timechanges.ChildNodes)
					model.TimeChanges.Add ((TimeChange)timechange);
			
			return model;
		}
	}
}

