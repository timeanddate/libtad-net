using System;
using System.Xml;
using System.Globalization;


namespace TimeAndDate.Services.DataTypes.Time
{
	public class TADTimezone
	{
		/// <summary>
		/// Abbreviated timezone name.
		///
		/// Example: LHDT
		/// </summary>
		/// <value>
		/// The abbrevation.
		/// </value>
		public string Abbrevation { get; set; }
		
		/// <summary>
		/// Full timezone name.
		///
		/// Example: Lord Howe Daylight Time
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Name { get; set; }
		
		/// <summary>
		/// The timezone offset (from UTC) as TimeSpan.
		/// </summary>
		/// <value>
		/// The offset.
		/// </value>
		public TimeSpan Offset { get; set; }
		
		/// <summary>
		/// Basic timezone offset (without DST) in seconds.
		/// </summary>
		/// <value>
		/// The basic offset in seconds.
		/// </value>
		public int BasicOffset { get; set; }
		
		/// <summary>
		/// DST component of timezone offset in seconds.
		/// </summary>
		/// <value>
		/// The DST offset in seconds
		/// </value>
		public int DSTOffset { get; set; }
		
		/// <summary>
		/// Total offset from UTC in seconds.
		/// </summary>
		/// <value>
		/// The total offset in seconds.
		/// </value>
		public int TotalOffset { get; set; }
		
		public static explicit operator TADTimezone (XmlNode node)
		{
			var model = new TADTimezone ();
			var zoneabb = node.SelectSingleNode ("zoneabb");
			var name = node.SelectSingleNode ("zonename");
			var zoneoffset = node.SelectSingleNode ("zoneoffset");
			var zonedst = node.SelectSingleNode ("zonedst");
			var totaloffset = node.SelectSingleNode ("zonetotaloffset");
			var offset = node.Attributes ["offset"];
			
			if (zoneabb != null)
				model.Abbrevation = zoneabb.InnerText;
			
			if (name != null)
				model.Name = name.InnerText;
			
            		if (zoneoffset != null)
                		model.BasicOffset = Int32.Parse(zoneoffset.InnerText, CultureInfo.InvariantCulture);
			
			if (zonedst != null) 
                		model.DSTOffset = Int32.Parse(zonedst.InnerText, CultureInfo.InvariantCulture);
			
			if (totaloffset != null)
                		model.TotalOffset = Int32.Parse(totaloffset.InnerText, CultureInfo.InvariantCulture);
				
			if (offset != null && offset.InnerText.StartsWith ("-"))
				model.Offset = -TimeSpan.ParseExact (offset.InnerText, @"\-hh\:mm", CultureInfo.InvariantCulture);
			else if (offset != null)
				model.Offset = TimeSpan.ParseExact(offset.InnerText, @"\+hh\:mm", CultureInfo.InvariantCulture);				
			
			return model;
		}
	}
}

