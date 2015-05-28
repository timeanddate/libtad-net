using System;
using System.Xml;
using TimeAndDate.Services.Common;
using System.Globalization;

namespace TimeAndDate.Services.DataTypes.Time
{
	public class TADTime
	{
		/// <summary>
		/// ISO representation of date and time, timezone included if 
		/// different from UTC. If time is not applicable, only the 
		/// date is shown.
		///
		/// Example: 2011-06-08T09:18:16+02:00
		///
		/// Example: 2011-06-08T07:18:16 (UTC time)
		///
		/// Example: 2011-06-08 (only date)
		/// </summary>
		/// <value>
		/// The IS.
		/// </value>
		public string ISO { get; set; }		
		
		/// <summary>
		/// Date and time representation of the ISO string.
		/// </summary>
		/// <value>
		/// The date time.
		/// </value>
		public DateTimeOffset DateTime { get; set; }
		
		/// <summary>
		/// Timezone information. Element is only present if different 
		/// from UTC and requested by specifying the 
		/// IncludeTimezoneInformation parameter.
		/// </summary>
		/// <value>
		/// The timezone.
		/// </value>
		public TADTimezone Timezone { get; set; }
		
		public static explicit operator TADTime (XmlNode node)
		{
			var model = new TADTime ();
			var iso = node.Attributes ["iso"];
			var timezone = node.SelectSingleNode ("timezone");
			var datetime = node.SelectSingleNode ("datetime");
			
			if (iso != null) 
				model.ISO = iso.InnerText.EndsWith("Z") ? iso.InnerText : iso.InnerText + "Z";
						
			if (timezone != null)
				model.Timezone = (TADTimezone)timezone;
			
			if (datetime != null)
			{
				var year = Int32.Parse (datetime.SelectSingleNode ("year").InnerText);
				var month = Int32.Parse (datetime.SelectSingleNode ("month").InnerText);
				var day = Int32.Parse (datetime.SelectSingleNode ("day").InnerText);
				var hour = Int32.Parse (datetime.SelectSingleNode ("hour").InnerText);
				var minute = Int32.Parse (datetime.SelectSingleNode ("minute").InnerText);
				var second = Int32.Parse (datetime.SelectSingleNode ("second").InnerText);
				
				model.DateTime = new DateTimeOffset (year, month, day, hour, minute, second, TimeSpan.FromHours(0));
			} 
			else if (datetime == null && iso != null)
				model.DateTime = DateTimeOffset.Parse (model.ISO, null, DateTimeStyles.AssumeUniversal);
			
			return model;
		}			
	}
}

