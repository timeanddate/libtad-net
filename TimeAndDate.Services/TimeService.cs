using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.Text;
using System.Globalization;
using System.Xml;
using TimeAndDate.Services.Common;
using TimeAndDate.Services.DataTypes.Places;

namespace TimeAndDate.Services
{
	public class TimeService : BaseService
	{		
		/// <summary>
		/// Search radius for translating coordinates (parameter placeid) to locations. Coordinates that could not be translated will 
		/// yield results for the actual geographical position.
		/// </summary>
		/// <value>
		/// The radius in kilometers. Default is infinite, but only locations within the same country and time zone are considered.
		/// </value>
		public int Radius { get; set; }
		
		/// <summary>
		/// Return coordinates for the Geography object.
		/// </summary>
		/// <value>
		/// <c>true</c> if return coordinates; otherwise, <c>false</c>. <c>true</c> is default.
		/// </value>
		public bool IncludeCoordinates { get; set; }
		
		/// <summary>
		/// Controls if the astronomy element with information about sunrise and sunset shall be added to the result.
		/// </summary>
		/// <value>
		/// <c>true</c> if return sunrise and sunset; otherwise, <c>false</c>. <c>true</c> is default.
		/// </value>
		public bool IncludeSunriseAndSunset { get; set; }
		
		/// <summary>
		/// Adds current time under the location object.
		/// </summary>
		/// <value>
		/// <c>true</c> if add current time to location; otherwise, <c>false</c>. <c>true</c> is default.
		/// </value>
		public bool IncludeCurrentTimeToLocation { get; set; }
		
		/// <summary>
		/// Add a list of time changes during the year to the location object. This listing e.g. shows changes caused by daylight savings time.
		/// </summary>
		/// <value>
		/// <c>true</c> if add list of time changes; otherwise, <c>false</c>. <c>true</c> is default.
		/// </value>
		public bool IncludeListOfTimeChanges { get; set; }
		
		/// <summary>
		/// Add timezone information under the time object.
		/// </summary>
		/// <value>
		/// <c>true</c> if add timezone information; otherwise, <c>false</c>. <c>true</c> is default.
		/// </value>
		public bool IncludeTimezoneInformation { get; set; }
				
		/// <summary>
		/// The timeservice service can be used to retrieve the current time in one or more places. 
		/// Additionally, information about time zones and related changes and the time of sunrise 
		/// and sunset can be queried.
		/// </summary>
		/// <param name='accessKey'>
		/// Access key.
		/// </param>
		/// <param name='secretKey'>
		/// Secret key.
		/// </param>
		public TimeService (string accessKey, string secretKey) : base(accessKey, secretKey, "timeservice")
		{		
			IncludeCoordinates = true;
			IncludeSunriseAndSunset = true;
			IncludeCurrentTimeToLocation = true;
			IncludeListOfTimeChanges = true;
			IncludeTimezoneInformation = true;
		}
		
		/// <summary>
		/// Retrieves the current time for place by ID.
		/// </summary>
		/// <returns>
		/// The current time for place.
		/// </returns>
		/// <param name='placeId'>
		/// Place identifier.
		/// </param>
		public IList<Location> CurrentTimeForPlace (LocationId placeId)
		{
			if (placeId == null)				
				throw new ArgumentException ("A required argument is null or empty");
			
			var id = placeId.GetIdAsString ();
			if(string.IsNullOrEmpty(id))
				throw new ArgumentException ("A required argument is null or empty");
			
			return RetrieveCurrentTime (id);
		}
		
		private IList<Location> RetrieveCurrentTime (string placeid)
		{
			var arguments = GetArguments (placeid);
			var result = CallService(arguments);
			return FromXml (result);
		}
		
		private NameValueCollection GetArguments (string placeId)
		{
			var args = new NameValueCollection (AuthenticationOptions);
			args.Set ("geo", IncludeCoordinates.ToNum ());
			args.Set ("lang", Language);
			args.Set ("radius", Radius.ToString ());
			args.Set ("sun", IncludeSunriseAndSunset.ToNum ());
			args.Set ("time", IncludeCurrentTimeToLocation.ToNum ());
			args.Set ("timechanges", IncludeListOfTimeChanges.ToNum ());
			args.Set ("tz", IncludeTimezoneInformation.ToNum ());
			args.Set ("out", Constants.DefaultReturnFormat);
			args.Set ("placeid", placeId);
			args.Set ("version", Version.ToString ());
			args.Set ("verbosetime", Constants.DefaultVerboseTimeValue.ToString ());
			
			return args;
		}				
		
		private static IList<Location> FromXml(string result)
		{
			var list = new List<Location> ();
			var xml = new XmlDocument ();
			xml.LoadXml (result);
						
			var locationNodes = xml.GetElementsByTagName ("location");
			foreach (XmlNode location in locationNodes)
				list.Add ((Location)location);
			
			return list;
		}		
	}
}

