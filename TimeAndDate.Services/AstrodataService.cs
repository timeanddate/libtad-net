using System;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Net;
using System.Collections.Generic;
using TimeAndDate.Services.Common;
using TimeAndDate.Services.DataTypes.Astro;
using TimeAndDate.Services.DataTypes.Time;
using TimeAndDate.Services.DataTypes.Places;
using System.Xml;

namespace TimeAndDate.Services
{
	public class AstrodataService : BaseService
	{
		/// <summary>
		/// Specify whether or not the intervals should be considered the local time for the place(s) or UTC time.
		/// </summary>
		/// <value>
		/// <c>true</c> if intervals are local time; otherwise, <c>false</c>. <c>false</c> is default.
		/// </value>
		public bool LocalTime { get; set; }

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
		/// The Astrodata Service can be used to retrieve the altitude, azimuth and distance to the Moon and the Sun for all locations in our database.
		/// The service also returns the moon phase, the fraction of the Moon's illuminated side as well as the midpoint angle of the Moon's bright limb at any point in time.
		/// Unlike the Astro Event Service, the Astro Position Service can be queried on a specific point in time, down to the second.
		/// </summary>
		/// <param name='accessKey'>
		/// Access key.
		/// </param>
		/// <param name='secretKey'>
		/// Secret key.
		/// </param>
		public AstrodataService (string accessKey, string secretKey) : base(accessKey, secretKey, "astrodata") 
		{
			XmlElemName = "location";
		}

		/// <summary>
		/// Gets the specified object types for the given interval(s).
		/// </summary>
		/// <returns>
		/// A list of astronomical information.
		/// </returns>
		/// <param name='objectType'>
		/// Specify which astronomical object you are interested in. Combine types by using the binary OR operator.
		/// </param>
		/// <param name='placeId'>
		/// Place identifier.
		/// </param>
		/// <param name='interval'>
		/// Specify the point in time you would like to calculate data for.
		/// </param>
		public IList<AstronomyLocation> GetAstroData (AstronomyObjectType objectType, LocationId placeId, TADDateTime interval)
		{
			var list = new List<TADDateTime> ();
			list.Add (interval);

			var args = GetArguments (objectType, placeId, list);
			return CallService<AstronomyLocation> (args, x => (AstronomyLocation)x);
		}

		/// <summary>
		/// Gets the specified object types for the given interval(s).
		/// This overload accepts a list of intervals.
		/// </summary>
		/// <returns>
		/// A list of astronomical information.
		/// </returns>
		/// <param name='objectType'>
		/// Specify which astronomical object you are interested in. Combine types by using the binary OR operator.
		/// </param>
		/// <param name='placeId'>
		/// Place identifier.
		/// </param>
		/// <param name='interval'>
		/// Specify the points in time you would like to calculate data for.
		/// </param>
		public IList<AstronomyLocation> GetAstroData (AstronomyObjectType objectType, LocationId placeId, List<TADDateTime> interval)
		{
			var args = GetArguments (objectType, placeId, interval);
			return CallService<AstronomyLocation> (args, x => (AstronomyLocation)x);
		}

		/// <summary>
		/// Gets the specified object types for the given interval(s).
		/// </summary>
		/// <returns>
		/// A list of astronomical information.
		/// </returns>
		/// <param name='objectType'>
		/// Specify which astronomical object you are interested in. Combine types by using the binary OR operator.
		/// </param>
		/// <param name='placeId'>
		/// Place identifier.
		/// </param>
		/// <param name='interval'>
		/// Specify the point in time you would like to calculate data for.
		/// </param>
		public async Task<IList<AstronomyLocation>> GetAstroDataAsync (AstronomyObjectType objectType, LocationId placeId, TADDateTime interval)
		{
			var list = new List<TADDateTime> ();
			list.Add (interval);

			var args = GetArguments (objectType, placeId, list);
			return await CallServiceAsync<AstronomyLocation> (args, x => (AstronomyLocation)x);
		}

		/// <summary>
		/// Gets the specified object types for the given interval(s).
		/// This overload accepts a list of intervals.
		/// </summary>
		/// <returns>
		/// A list of astronomical information.
		/// </returns>
		/// <param name='objectType'>
		/// Specify which astronomical object you are interested in. Combine types by using the binary OR operator.
		/// </param>
		/// <param name='placeId'>
		/// Place identifier.
		/// </param>
		/// <param name='interval'>
		/// Specify the points in time you would like to calculate data for.
		/// </param>
		public async Task<IList<AstronomyLocation>> GetAstroDataAsync (AstronomyObjectType objectType, LocationId placeId, List<TADDateTime> interval)
		{
			var args = GetArguments (objectType, placeId, interval);
			return await CallServiceAsync<AstronomyLocation> (args, x => (AstronomyLocation)x);
		}

		private NameValueCollection GetArguments (AstronomyObjectType objectType, LocationId locationId, List<TADDateTime> interval)
		{
			var args = new NameValueCollection ();
			var objectTypes = GetObjectTypes (objectType);

			if (!string.IsNullOrEmpty (objectTypes))
				args.Set("object", objectTypes);

			args.Set ("placeid", locationId.GetIdAsString());
			args.Set ("interval", GetInterval(interval));

			args.Set ("lang", Language);
			args.Set ("geo", IncludeCoordinates.ToNum());
			args.Set ("localtime", LocalTime.ToNum());
			args.Set ("isotime", IncludeISOTime.ToNum());
			args.Set ("utctime", IncludeUTCTime.ToNum());
			args.Set ("verbosetime", Constants.DefaultVerboseTimeValue.ToString ());

			return args;
		}

		private string GetObjectTypes (AstronomyObjectType objectType)
		{
			var includedStrings = new List<string> ();
			foreach (AstronomyObjectType type in Enum.GetValues(typeof (AstronomyObjectType)))
			{
				if (objectType.HasFlag (type))
				{
					includedStrings.Add (StringHelpers.ResolveAstronomyObjectType (x => (int) x.type == (int) type));
				}
			}

			var included = string.Join(",", includedStrings.ToArray ());
			return included;
		}

		private string GetInterval (List<TADDateTime> interval)
		{
			var includedStrings = new List<string> ();
			foreach (TADDateTime datetime in interval)
			{
				includedStrings.Add ((string) datetime);
			}

			var included = string.Join (",", includedStrings.ToArray ());
			return included;
		}
	}
}
