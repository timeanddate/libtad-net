using System;
using System.Collections.Generic;
using TimeAndDate.Services.DataTypes.BusinessDays;
using TimeAndDate.Services.DataTypes.Places;

namespace TimeAndDate.Services
{
	public class BusinessDuration
	{
		/// <summary>
		/// Geographical information about the location.
		/// </summary>
		/// <value>
		/// The geography.
		/// </value>
		public Geo Geography { get; set; }

		public Period Period { get; set; }

		public BusinessDuration(Geo geo, Period period)
		{
			Geography = geo;
            Period = period;
		}
	}
}
