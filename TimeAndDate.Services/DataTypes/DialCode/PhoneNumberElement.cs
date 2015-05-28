using System;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace TimeAndDate.Services.DataTypes.DialCode
{
	public enum PhoneNumberElementType
	{
		/// <summary>
		/// The prefix you need to dial for international calls.
		/// </summary>
		InternationalPrefix,
		
		/// <summary>
		/// The country code for the called country.
		/// </summary>
		CountryPrefix,
		
		/// <summary>
		/// The prefix that should be dialed for national calls.
		/// </summary>
		NationalPrefix,
		
		/// <summary>
		/// The country uses local area or city codes, but the code 
		/// for the called location is not known.
		/// </summary>
		UnknownNationalPrefix,
		
		/// <summary>
		/// The national code for the dialed number.
		/// </summary>
		NationalCode,
		
		/// <summary>
		/// The area code for the dialed number.
		/// </summary>
		AreaCode,
		
		/// <summary>
		/// The dialed local number.
		/// </summary>
		LocalNumber
	}
}

