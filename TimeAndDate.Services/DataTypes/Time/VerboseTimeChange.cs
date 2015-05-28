using System;
using System.Xml;
using TimeAndDate.Services.Common;

namespace TimeAndDate.Services.DataTypes.Time
{
	public class VerboseTimeChange
	{
		/// <summary>
		/// Time stamp of transition in UTC time, split up into components.
		/// </summary>
		/// <value>
		/// The UTC time.
		/// </value>
		public DateTime UtcTime { get; set; }
		
		/// <summary>
		/// Local time before transition, split up into components.
		/// </summary>
		/// <value>
		/// The old local time.
		/// </value>
		public DateTime OldLocalTime { get; set; }
		
		/// <summary>
		/// Local time after transition, split up into components.
		/// </summary>
		/// <value>
		/// The new local time.
		/// </value>
		public DateTime NewLocalTime { get; set; }
		
		public static explicit operator VerboseTimeChange (XmlNode node)
		{
			var model = new VerboseTimeChange ();
			model.UtcTime = node.SelectSingleNode ("utctime/datetime").MapXmlToDateTime ();
			model.OldLocalTime = node.SelectSingleNode ("oldlocaltime/datetime").MapXmlToDateTime ();
			model.NewLocalTime = node.SelectSingleNode ("newlocaltime/datetime").MapXmlToDateTime ();
			
			return model;
		}		
	}
}

