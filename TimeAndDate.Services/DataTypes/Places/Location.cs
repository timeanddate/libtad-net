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
		
		public TimeSpan GetUTCOffsetFromLocalTime (DateTimeOffset localTime)
		{
			if (TimeChanges == null || (TimeChanges != null && TimeChanges.Count () == 0))
				throw new MissingTimeChangesException ("IncludeTimeChanges either set to false or no time changes for this location");		
			
			var firstNewLocalTime = TimeChanges.FirstOrDefault ().NewLocalTime.Year;
			if (localTime.Year > firstNewLocalTime || localTime.Year < firstNewLocalTime)
				throw new QueriedDateOutOfRangeException ("The year specified in localTime is outside the year available for this location");					
			
			TimeChange change;
			if (TimeChanges.Count () == 1)
				change = TimeChanges.SingleOrDefault ();
			else
				change = GetTimeChangeForLocalTime (TimeChanges.ToList (), localTime);
			
			TimeSpan span;
			if (localTime.ToMinuteCompare (change.OldLocalTime.DateTime) < 0)
			{
				span = change.OldLocalTime.DateTime - change.UtcTime.DateTime;
			}
			else
			{
				span = TimeSpan.FromSeconds (change.NewTotalOffset);
			}
							
			return span;
		}
					
		private static TimeChange GetTimeChangeForLocalTime (List<TimeChange> changes, DateTimeOffset localTime)
		{
			TimeChange timeChange = null;
			var numberOfChanges = changes.Count - 1;
			for (var i = 0; i <= numberOfChanges; i++)
			{
				var change = changes [i];
				
				// If the old local time was 02:00 and the new local time is 03:00
				// and the user ask for the timezone on 02:30 they will get this exception
				if (localTime.ToMinuteCompare (change.OldLocalTime.DateTime) == 1 && 
					localTime.ToMinuteCompare (change.NewLocalTime.DateTime) == -1)
					throw new LocalTimeDoesNotExistException ("The time and date requested falls between the old and new timezone");
								
				// If the date the user ask for is older (earlier) than the first time change
				var localTimeIsEarlierThanOldLocalTime = localTime.ToMinuteCompare (change.OldLocalTime.DateTime);
				if (localTimeIsEarlierThanOldLocalTime <= 0)
				{
					timeChange = change;
					break;					
				}
									
				// If the date the user ask for is newer or the same date as the new local time
				var localTimeIsLaterThanNewLocalTime = localTime.ToMinuteCompare (change.NewLocalTime.DateTime);
				if (localTimeIsLaterThanNewLocalTime >= 0)
				{
					// If this is the last time change then this is the timechange the user is in
					if (i == numberOfChanges)
					{ 
						timeChange = change;
					} 
					else
					{
						// If there is more time changes, and the date the user asks for is later than
						// the next date. Just continue to the next date. If not, 'change' is the 
						// timechange the localTime is in
						var next = changes [i + 1];
						if (localTime.ToMinuteCompare (next.NewLocalTime.DateTime) >= 0)
							continue; // Could be made recursive
						else
						{
							timeChange = change;
							break;
						}							
					}						
				}					
			}
								
			return timeChange;
		}
	}
}