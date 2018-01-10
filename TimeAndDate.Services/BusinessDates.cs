using System;
using System.Collections.Generic;
using TimeAndDate.Services.DataTypes.BusinessDays;
using TimeAndDate.Services.DataTypes.Places;

namespace TimeAndDate.Services
{
	public class BusinessDates
	{
		/// <summary>
		/// Geographical information about the location.
		/// </summary>
		/// <value>
		/// The geography.
		/// </value>
		public Geo Geography { get; set; }

		public List<Period> Periods { get; set; }

		public BusinessDates(Geo geo, List<Period> periods)
		{
			Geography = geo;
			Periods = periods;
		}
	}
}
