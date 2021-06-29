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
        public TADDateTime UtcTime { get; set; }
		
		/// <summary>
		/// Local time before transition, split up into components.
		/// </summary>
		/// <value>
		/// The old local time.
		/// </value>
        public TADDateTime OldLocalTime { get; set; }
		
		/// <summary>
		/// Local time after transition, split up into components.
		/// </summary>
		/// <value>
		/// The new local time.
		/// </value>
        public TADDateTime NewLocalTime { get; set; }
		
		public static explicit operator VerboseTimeChange (XmlNode node)
		{
			var model = new VerboseTimeChange ();
            model.UtcTime = (TADDateTime)node.SelectSingleNode("utctime/datetime");
            model.OldLocalTime = (TADDateTime)node.SelectSingleNode("oldlocaltime/datetime");
            model.NewLocalTime = (TADDateTime)node.SelectSingleNode("newlocaltime/datetime");
			
			return model;
		}		
	}
}

