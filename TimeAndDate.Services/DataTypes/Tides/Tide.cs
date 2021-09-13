using System;
using System.Xml;
using System.Globalization;
using TimeAndDate.Services.Common;
using TimeAndDate.Services.DataTypes.Time;

namespace TimeAndDate.Services.DataTypes.Tides
{
	public class Tide
	{
		public TADTime Time { get; set; }
		public float Amplitude { get; set; }
		public TidalPhase Phase { get; set; }

		public static explicit operator Tide (XmlNode node)
		{
			var model = new Tide ();
			var time = node.SelectSingleNode ("time");
			var amplitude = node.SelectSingleNode ("amplitude");
			var phase = node.SelectSingleNode ("phase");

			if (time != null)
				model.Time = (TADTime)time;
			
			if (amplitude != null)
				model.Amplitude = float.Parse(amplitude.InnerText, CultureInfo.InvariantCulture);

			if (phase != null)
				model.Phase = StringHelpers.ResolveTidalPhase (phase.InnerText);

			return model;
		}

	}
}
