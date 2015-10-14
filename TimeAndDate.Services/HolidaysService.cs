using System;
using System.Collections.Specialized;
using System.Net;
using System.Collections.Generic;
using TimeAndDate.Services.Common;
using TimeAndDate.Services.DataTypes.Holiday;
using System.Xml;

namespace TimeAndDate.Services
{
	public class HolidaysService : BaseService
	{	
		/// <summary>
		/// Holiday types which should be returned. 
		/// To combine multiple classes, use the binary OR operator.
		/// 
		/// Example: 
		/// <c>service.IncludedHolidayTypes = HolidayType.Local</c>
		/// </summary>
		/// <value>
		/// The holiday types.
		/// </value>
		public HolidayType Types { get; set; }	
		
		/// <summary>
		/// The holidays service can be used to retrieve the list of holidays for a country.
		/// </summary>
		/// <param name='accessKey'>
		/// Access key.
		/// </param>
		/// <param name='secretKey'>
		/// Secret key.
		/// </param>
		public HolidaysService (string accessKey, string secretKey) : base(accessKey, secretKey, "holidays") {	}
		
		/// <summary>
		/// The holidays service can be used to retrieve the list of holidays for a country.
		/// </summary>
		/// <returns>
		/// List of holidays for a given country
		/// </returns>
		/// <param name='countryCode'>
		/// Specify the ISO3166-1-alpha-2 Country Code for which you would like to retrieve the list of holidays.
		/// </param>
		/// <param name='year'>
		/// The year for which the holidays should be retrieved.
		/// </param>
		public IList<Holiday> HolidaysForCountry (string countryCode, int year)
		{
			if (string.IsNullOrEmpty (countryCode) && year <= 0)
				throw new ArgumentException ("A required argument is null or empty");
			
			return RetrieveHolidays (countryCode, year);
		}
		
		/// <summary>
		/// The holidays service can be used to retrieve the list of holidays for a country. 
		/// This overload uses the current year by default.
		/// </summary>
		/// <returns>
		/// Specify the ISO3166-1-alpha-2 Country Code for which you would like to retrieve the list of holidays.
		/// </returns>
		/// <param name='country'>
		/// Country.
		/// </param>
		public IList<Holiday> HolidaysForCountry (string country)
		{
			if (string.IsNullOrEmpty (country))
				throw new ArgumentException ("A required argument is null or empty");
			
			return RetrieveHolidays (country, DateTime.Now.Year);
		}
		
		private IList<Holiday> RetrieveHolidays (string country, int year)
		{
			var arguments = GetArguments (country, year);
			var result = CallService(arguments);
			return FromXml(result, "holiday", x => (Holiday)x);
		}				
		
		private NameValueCollection GetArguments (string country, int year)
		{
			var args = new NameValueCollection (AuthenticationOptions);
			var types = GetHolidayTypes ();
			args.Set ("country", country);			
			args.Set ("lang", Language);									
			args.Set ("version", Version.ToString ());
			args.Set ("verbosetime", Constants.DefaultVerboseTimeValue.ToString ());
			args.Set ("out", Constants.DefaultReturnFormat);
			
			if (!string.IsNullOrEmpty (types))
				args.Set ("types", types);
			
			if (year > 0)
				args.Set ("year", year.ToString ());
			
			return args;
		}
		
		private string GetHolidayTypes ()
		{
			var includedStrings = new List<string> ();
			foreach (HolidayType holidayType in Enum.GetValues(typeof (HolidayType)))
			{
				if (Types.HasFlag (holidayType))
				{
					includedStrings.Add (StringHelpers.ResolveHolidayType (x => x.Item1 == (int)holidayType).Item2);
				}
			}
			
			var included = string.Join (",", includedStrings.ToArray ());						
			return included;
		}
	}
}

