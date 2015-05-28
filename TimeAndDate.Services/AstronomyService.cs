using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using TimeAndDate.Services.Common;
using System.Net;
using TimeAndDate.Services.DataTypes.Astro;
using System.Xml;
using TimeAndDate.Services.DataTypes.Places;

namespace TimeAndDate.Services
{
	public class AstronomyService : BaseService
	{				
		/// <summary>
		/// Selection of which astronomical events you are interested in. 
		/// To combine multiple classes, use the binary OR operator.
		/// 
		/// Example: 
		/// <c>service.Types = AstronomyEventClass.CivilTwilight | AstronomyEventClass.NauticalTwilight</c>
		/// </summary>
		/// <value>
		/// Default value: <c>default types</c>
		/// </value>
		public AstronomyEventClass Types { get; set; }
		
		/// <summary>
		/// Return longitude and latitude for the geo object.
		/// </summary>
		/// <value>
		/// <c>true</c> if return latitude and longitude; otherwise, <c>false</c>. <c>true</c> is default.
		/// </value>
		public bool IncludeCoordinates { get; set; }	
		
		/// <summary>
		/// Adds timestamps (local time) to all events.
		/// </summary>
		/// <value>
		/// <c>true</c> if include ISO time; otherwise, <c>false</c>. <c>false</c> is default.
		/// </value>
		public bool IncludeISOTime { get; set; }
		
		/// <summary>
		/// Adds UTC timestamps to all events.
		/// </summary>
		/// <value>
		/// <c>true</c> if include UTC time; otherwise, <c>false</c>.<c>false</c> is default.
		/// </value>
		public bool IncludeUTCTime { get; set; }
		
		/// <summary>
		/// Search radius for translating coordinates (parameter placeid) to locations. Coordinates that 
		/// could not be translated will yield results for the actual geographical position – if you would 
		/// like to query for times at an exact location, specify a radius of zero (0).
		/// </summary>
		/// <value>
		/// The radius in kilometers. Default is infinite, but only locations within the same country and time zone are considered.
		/// </value>
		public int Radius { get; set; }		
				
		/// <summary>
		/// The astronomy service can be used retrieve rise, set, noon and twilight times for sun and moon for all locations. 
		/// The service also exposes the azimuth of the events and altitude and distance (for the noon event).
		/// </summary>
		/// <param name='accessKey'>
		/// Access key.
		/// </param>
		/// <param name='secretKey'>
		/// Secret key.
		/// </param>
		public AstronomyService (string accessKey, string secretKey) : base(accessKey, secretKey, "astronomy")
		{
			IncludeCoordinates = true;
			IncludeISOTime = false;
			IncludeUTCTime = false;
		}
		
		/// <summary>
		/// Gets the specified object type (Moon, Sun) for a specified place by start date.
		/// </summary>
		/// <returns>
		/// A list of astronomical information.
		/// </returns>
		/// <param name='objectType'>
		/// The astronomical object type (Moon or Sun)
		/// </param>
		/// <param name='placeId'>
		/// Place identifier.
		/// </param>
		/// <param name='startDate'>
		/// Start date.
		/// </param>
		public IList<AstronomyLocation> GetAstronomicalInfo (AstronomyObjectType objectType, LocationId placeId, DateTime startDate)
		{
			if (placeId == null || startDate.Year == 0)
				throw new ArgumentException ("A required argument is null or empty");
			
			var id = placeId.GetIdAsString ();
			if (string.IsNullOrEmpty (id))
				throw new ArgumentException ("A required argument is null or empty");
			
			var args = new NameValueCollection (AuthenticationOptions);
			args.Set ("placeid", id);
			args.Set ("object", objectType.ToString ().ToLower ());
			args.Set ("startdt", startDate.ToString ("yyyy-MM-dd"));
			
			return RetrieveAstronomicalInfo (args);
		}								
		
		/// <summary>
		/// Gets the specified object type (Moon, Sun) for a specified place by start date.
		/// </summary>
		/// <returns>
		/// A list of astronomical information.
		/// </returns>
		/// <param name='objectType'>
		/// The astronomical object type (Moon or Sun)
		/// </param>
		/// <param name='placeId'>
		/// Place identifier.
		/// </param>
		/// <param name='startDate'>
		/// Start date.
		/// </param>
		/// <param name='endDate'>
		/// End date.
		/// </param>
		public IList<AstronomyLocation> GetAstronomicalInfo (AstronomyObjectType objectType, LocationId placeId, DateTime startDate, DateTime endDate)
		{
			if (placeId == null || startDate.Year == 0 || endDate.Year == 0)
				throw new ArgumentException ("A required argument is null or empty");
				
			var id = placeId.GetIdAsString ();
			if (string.IsNullOrEmpty (id))
				throw new ArgumentException ("A required argument is null or empty");
			
			if (endDate.Ticks < startDate.Ticks)
				throw new QueriedDateOutOfRangeException ("End date cannot be before Start date");
			
			var args = new NameValueCollection (AuthenticationOptions);
			args.Set ("placeid", id);
			args.Set ("object", objectType.ToString ().ToLower ());
			args.Set ("startdt", startDate.ToString ("yyyy-MM-dd"));
			args.Set ("enddt", endDate.ToString ("yyyy-MM-dd"));
			
			return RetrieveAstronomicalInfo (args);
		}
		
		
		private IList<AstronomyLocation> RetrieveAstronomicalInfo (NameValueCollection args)
		{
			var arguments = GetOptionalArguments (args);			
			
			var query = UriUtils.BuildUriString (arguments);
			
			var uri = new UriBuilder (Constants.EntryPoint + ServiceName)
			{
				Query = query	
			};
			
			using (var client = new WebClient())
			{
				client.Encoding = System.Text.Encoding.UTF8;
				var result = client.DownloadString (uri.Uri);
				XmlUtils.CheckForErrors (result);
				return FromXml (result);				
			}
		}
		
		private NameValueCollection GetOptionalArguments (NameValueCollection args)
		{
			var optionalArgs = new NameValueCollection (args);
			var types = GetAstronomyEventTypes ();
			
			optionalArgs.Set ("geo", IncludeCoordinates.ToNum ());
			optionalArgs.Set ("isotime", IncludeISOTime.ToNum ());
			optionalArgs.Set ("lang", Language);
			optionalArgs.Set ("radius", Radius.ToString());
			optionalArgs.Set ("utctime", IncludeUTCTime.ToNum ());				
			optionalArgs.Set ("out", Constants.DefaultReturnFormat);
			optionalArgs.Set ("verbosetime", Constants.DefaultVerboseTimeValue.ToString ());
			
			if(!string.IsNullOrEmpty(types))
				optionalArgs.Set ("types", types);
			
			return optionalArgs;
		}
		
		private static IList<AstronomyLocation> FromXml(string result)
		{
			var list = new List<AstronomyLocation> ();
			var xml = new XmlDocument ();
			xml.LoadXml (result);
			
			
			var dataNode = xml.DocumentElement;			
			
			var locations = dataNode.GetElementsByTagName ("location");
			foreach (XmlNode location in locations)
				list.Add ((AstronomyLocation)location);
			
			return list;
		}
		
		private string GetAstronomyEventTypes ()
		{
			var includedStrings = new List<string> ();
			foreach (AstronomyEventClass astroClass in Enum.GetValues(typeof (AstronomyEventClass)))
			{
				if (Types.HasFlag (astroClass))
				{
					includedStrings.Add (StringHelpers.ResolveAstronomyEventClass (x => x.Item1 == (int)astroClass).Item2);
				}
			}			
			
			var included = string.Join (",", includedStrings.ToArray ());			
			return included;
		}
	}
}

