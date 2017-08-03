using System;
using TimeAndDate.Services.DataTypes.Places;

namespace TimeAndDate.Services.DataTypes.BusinessDays
{
    public class BusinessDays
    {
		/// <summary>
		/// Geographical information about the location.
		/// </summary>
		/// <value>
		/// The geography.
		/// </value>
		public Geo Geography { get; set; }

        public Period Period { get; set; }

		public BusinessDays()
        {
        }
    }
}
