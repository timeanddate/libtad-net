using System;
using System.Xml;


namespace TimeAndDate.Services.DataTypes.Time
{
	public class TimeChange
	{
		/// <summary>
		/// New DST offset in seconds. Value will be null if there is 
		/// no DST for this location.
		/// </summary>
		/// <value>
		/// The new daylight saving time or null.
		/// </value>
		public int? NewDaylightSavingTime { get; set; }
		
		/// <summary>
		/// New timezone offset to UTC in seconds if there is a timezone 
		/// change for this place. Otherwise the value will be null. 
		/// Time zones changes happen only very rarely, so the field will 
		/// be null on most occasions.
		/// </summary>
		/// <value>
		/// The new timezone offset or null.
		/// </value>
		public int? NewTimezoneOffset { get; set; }
		
		/// <summary>
		/// New total offset to UTC in seconds.
		/// </summary>
		/// <value>
		/// The new total offset in seconds
		/// </value>
		public int NewTotalOffset { get; set; }
		
		/// <summary>
		/// The UTC time of the transition
		/// </summary>
		/// <value>
		/// The UTC time.
		/// </value>
		public DateTimeOffset UtcTime { get; set; } 
		
		/// <summary>
		/// The old local time before the transition.
		/// </summary>
		/// <value>
		/// The old local time.
		/// </value>
		public DateTimeOffset OldLocalTime { get; set; }
		
		/// <summary>
		/// The new local time after the transition.
		/// </summary>
		/// <value>
		/// The new local time.
		/// </value>
		public DateTimeOffset NewLocalTime { get; set; }		
		
		public static explicit operator TimeChange (XmlNode node)
		{
			var model = new TimeChange ();
			var newdst = node.Attributes ["newdst"];
			var newzone = node.Attributes ["newzone"];
			var newoffset = node.Attributes ["newoffset"];
			var utctime = node.Attributes ["utctime"];
			var oldlocal = node.Attributes ["oldlocaltime"];
			var newlocal = node.Attributes ["newlocaltime"];
			
			if (newdst != null)
			{
				int dst;
				if (!Int32.TryParse (newdst.InnerText, out dst))
					model.NewDaylightSavingTime = null;
				else
					model.NewDaylightSavingTime = dst;
			}
				
			if (newzone != null)
			{
				int zone;
				if (!Int32.TryParse (newzone.InnerText, out zone))
					model.NewTimezoneOffset = null;
				else
					model.NewTimezoneOffset = zone;
			}		
			
			if (newoffset != null)
				model.NewTotalOffset = Int32.Parse (newoffset.InnerText);
			
			if (utctime != null)
				model.UtcTime = DateTimeOffset.Parse (utctime.InnerText);
			
			if (oldlocal != null)
				model.OldLocalTime = DateTimeOffset.Parse (oldlocal.InnerText, null, System.Globalization.DateTimeStyles.RoundtripKind);
			
			if (newlocal != null)
				model.NewLocalTime = DateTimeOffset.Parse(newlocal.InnerText, null, System.Globalization.DateTimeStyles.RoundtripKind);
						
			return model;
		}					
	}
}

