using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Xml;
using TimeAndDate.Services.Common;
using TimeAndDate.Services.DataTypes.Places;
using TimeAndDate.Services.DataTypes.Time;

namespace TimeAndDate.Services.DataTypes.Tides
{
	public class Station
	{
		/// <summary>
		/// The source station for the predicted tidal data.
		/// </summary>
		/// <value>
		/// The source station.
		/// </value>
		public StationInfo Source { get; set; }

		/// <summary>
		/// The part of the queried placeid that this location matches.
		/// </summary>
		/// <value>
		/// The matched query parameter.
		/// </value>
		public string MatchParam { get; set; }

		/// <summary>
		/// Requested tidal information.
		/// </summary>
		/// <value>
		/// List of tidal data points.
		/// </value>
		public IList<Tide> Result { get; set; }
				
		private Station ()
		{
			Result = new List<Tide>();
		}
		
		public static explicit operator Station (XmlNode node)
		{
			var model = new Station ();
			var source = node.SelectSingleNode ("source");
			var matchparam = node.Attributes ["matchparam"];
			var result = node.SelectSingleNode ("result");

			if (source != null)
				model.Source = (StationInfo)source;

			if (matchparam != null)
				model.MatchParam = matchparam.InnerText;

			if (result != null)
			{
				foreach (XmlNode child in result.ChildNodes)
					model.Result.Add ( (Tide)child );
			}
			
			return model;
		}			
	}
}

