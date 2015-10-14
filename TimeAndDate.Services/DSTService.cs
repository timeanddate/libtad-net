using System;
using System.Collections.Specialized;
using TimeAndDate.Services.Common;
using System.Net;
using System.Collections.Generic;
using TimeAndDate.Services.DataTypes.DST;
using System.Xml;


namespace TimeAndDate.Services
{
	public class DSTService : BaseService
	{	
		/// <summary>
		/// Add a list of time changes during the year to the dstentry object. This listing e.g. 
		/// shows changes caused by daylight savings time.
		/// </summary>
		/// <value>
		/// <c>true</c> if include time changes; otherwise, <c>false</c>. <c>false</c> is default.
		/// </value>
		public bool IncludeTimeChanges { get; set; }
		
		/// <summary>
		/// Return only countries which actually observe DST in the queried year. Other countries 
		/// will be suppressed.
		/// </summary>
		/// <value>
		/// <c>true</c> if include only dst countries; otherwise, <c>false</c>. <c>true</c> is default.
		/// </value>
		public bool IncludeOnlyDstCountries { get; set; }
		
		/// <summary>
		/// For every timezone/country, list the individual places that belong to each record.
		/// </summary>
		/// <value>
		/// <c>true</c> if include places for every country; otherwise, <c>false</c>. <c>true</c> is default.
		/// </value>
		public bool IncludePlacesForEveryCountry { get; set; }
						
		/// <summary>
		/// The dstlist service can be used to obtain data about timezones in all  
		/// supported countries, eventual start and end date of daylight savings time,  
		/// and UTC offset for the timezones.
		///
		/// The result data is aggregated on country/timezone level. By default, only 
		/// information from countries that actually observe DST is returned without 
		/// listing the individually affected locations – see the parameters listplaces 
		/// and onlydst to change this behavior.
		/// </summary>
		/// <param name='accessKey'>
		/// Access key.
		/// </param>
		/// <param name='secretKey'>
		/// Secret key.
		/// </param>
		public DSTService (string accessKey, string secretKey) : base(accessKey, secretKey, "dstlist")
		{
			IncludeTimeChanges = false;
			IncludePlacesForEveryCountry = true;
			IncludeOnlyDstCountries = true;
			XmlElemName = "dstentry";
		}
		
		/// <summary>
		/// Gets the all entries with daylight saving time 
		/// </summary>
		/// <returns>
		/// The daylight saving time.
		/// </returns>
		public IList<DST> GetDaylightSavingTime ()
		{			
			var args = GetArguments ();
			return CallService (args, x => (DST)x);
		}
		
		/// <summary>
		/// Gets the daylight saving time by ISO3166-1-alpha-2 Country Code
		/// </summary>
		/// <returns>
		/// The daylight saving time.
		/// </returns>
		/// <param name='countryCode'>
		/// Country code.
		/// </param>
		public IList<DST> GetDaylightSavingTime (string countryCode)
		{
			if (string.IsNullOrEmpty (countryCode))
				throw new ArgumentException ("A required argument is null or empty");
			
			IncludeOnlyDstCountries = false;
			var args = GetArguments ();
			args.Set ("country", countryCode);
			
			return CallService (args, x => (DST)x);
		}
		
		/// <summary>
		/// Gets the daylight saving time by year.
		/// </summary>
		/// <returns>
		/// The daylight saving time.
		/// </returns>
		/// <param name='year'>
		/// Year.
		/// </param>
		public IList<DST> GetDaylightSavingTime (int year)
		{
			if (year <= 0)
				throw new ArgumentException ("A required argument is null or empty");
			
			var args = GetArguments ();
			args.Set ("year", year.ToString ());
			
			return CallService (args, x => (DST)x);
		}
		
		/// <summary>
		/// Gets the daylight saving time by country and year.
		/// </summary>
		/// <returns>
		/// The daylight saving time.
		/// </returns>
		/// <param name='countryCode'>
		/// ISO3166-1-alpha-2 Country Code
		/// </param>
		/// <param name='year'>
		/// Year.
		/// </param>
		public IList<DST> GetDaylightSavingTime (string countryCode, int year)
		{
			if (string.IsNullOrEmpty (countryCode) && year <= 0)
				throw new ArgumentException ("A required argument is null or empty");
			
			IncludeOnlyDstCountries = false;
			var args = GetArguments ();
			args.Set ("country", countryCode);
			args.Set ("year", year.ToString ());
			
			return CallService (args, x => (DST)x);
		}
		
		private NameValueCollection GetArguments ()
		{
			var args = new NameValueCollection ();
			args.Set ("lang", Language);			
			args.Set ("timechanges", IncludeTimeChanges.ToNum ());
			args.Set ("onlydst", IncludeOnlyDstCountries.ToNum ());
			args.Set ("listplaces", IncludePlacesForEveryCountry.ToNum ());
			args.Set ("verbosetime", Constants.DefaultVerboseTimeValue.ToString ());
			
			return args;
		}
	}
}

