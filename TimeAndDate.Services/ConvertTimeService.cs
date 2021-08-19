using System;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Collections.Generic;
using TimeAndDate.Services.Common;
using System.Net;
using System.Globalization;
using TimeAndDate.Services.DataTypes.Places;
using System.Xml;
using TimeAndDate.Services.DataTypes.Time;

namespace TimeAndDate.Services
{
	public class ConvertTimeService : BaseService
	{
		/// <summary>
		/// Search radius for translating coordinates (parameters fromid and toid) 
		/// to locations. Coordinates that could not be translated will yield results 
		/// for the actual geographical position.
		/// </summary>
		/// <value>
		/// The radius in kilometers.
		/// </value>
		public int Radius { get; set; }
		
		/// <summary>
		/// Add a list of time changes during the year to the location object. This listing 
		/// e.g. shows changes caused by daylight savings time.
		/// </summary>
		/// <value>
		/// <c>true</c> if include time changes; otherwise, <c>false</c>. <c>true</c> is default.
		/// </value>
		public bool IncludeTimeChanges { get; set; }
		
		/// <summary>
		/// Add timezone information under the time object.
		/// </summary>
		/// <value>
		/// <c>true</c> if include timezone information; otherwise, <c>false</c>.<c>true</c> is default.
		/// </value>
		public bool IncludeTimezoneInformation { get; set; }
				
		/// <summary>
		/// The converttime service can be used to convert any time from UTC or any of 
		/// the supported locations to any other of the supported locations.
		/// </summary>
		/// <param name='accessKey'>
		/// Access key.
		/// </param>
		/// <param name='secretKey'>
		/// Secret key.
		/// </param>
		public ConvertTimeService (string accessKey, string secretKey) : base(accessKey, secretKey, "converttime")
		{								
			IncludeTimeChanges = true;
			IncludeTimezoneInformation = true;
		}

		/// <summary>
		/// Converts the time by using a LocationId, a ISO-string and optionally a list of IDs to convert to.
		/// </summary>
		/// <returns>
		/// The converted time.
		/// </returns>
		/// <param name='fromId'>
		/// The places identifier
		/// </param>
		/// <param name='iso'>
		/// ISO 8601-formatted string.
		/// </param>
		/// <param name='toIds'>
		/// The place IDs to convert to.
		/// </param>
		public ConvertedTimes ConvertTime (LocationId fromId, string iso, IList<LocationId> toIds = null)
		{			
			if (fromId == null || string.IsNullOrEmpty (iso))
				throw new ArgumentException ("A required argument is null or empty");
			
			var id = fromId.GetIdAsString ();
			if (string.IsNullOrEmpty (id))
				throw new ArgumentException ("A required argument is null or empty");
				
			var arguments = GetCommonArguments (id);
			arguments.Set ("iso", iso);

			if (toIds != null)
				arguments.Add (GetArgumentsForToIds (toIds));			

			return CallService<ConvertedTimes>(arguments);
		}		
		
		/// <summary>
		/// Converts the time by using a LocationId, a DateTime and optionally a list of IDs to convert to.
		/// </summary>
		/// <returns>
		/// The converted time.
		/// </returns>
		/// <param name='fromId'>
		/// The places identifier
		/// </param>
		/// <param name='Date'>
		/// Date.
		/// </param>
		/// <param name='ToIds'>
		/// The place IDs to convert to.
		/// </param>
		public ConvertedTimes ConvertTime (LocationId fromId, DateTime date, IList<LocationId> toIds = null)
		{	
			return ConvertTime (fromId, date.ToString ("s", CultureInfo.InvariantCulture), toIds);
		}					

		/// <summary>
		/// Converts the time by using a LocationId, a ISO-string and optionally a list of IDs to convert to.
		/// </summary>
		/// <returns>
		/// The converted time.
		/// </returns>
		/// <param name='fromId'>
		/// The places identifier
		/// </param>
		/// <param name='iso'>
		/// ISO 8601-formatted string.
		/// </param>
		/// <param name='toIds'>
		/// The place IDs to convert to.
		/// </param>
		public async Task<ConvertedTimes> ConvertTimeAsync (LocationId fromId, string iso, IList<LocationId> toIds = null)
		{			
			if (fromId == null || string.IsNullOrEmpty (iso))
				throw new ArgumentException ("A required argument is null or empty");
			
			var id = fromId.GetIdAsString ();
			if (string.IsNullOrEmpty (id))
				throw new ArgumentException ("A required argument is null or empty");
				
			var arguments = GetCommonArguments (id);
			arguments.Set ("iso", iso);

			if (toIds != null)
				arguments.Add (GetArgumentsForToIds (toIds));			

			return await CallServiceAsync<ConvertedTimes>(arguments);
		}		
		
		/// <summary>
		/// Converts the time by using a LocationId, a DateTime and optionally a list of IDs to convert to.
		/// </summary>
		/// <returns>
		/// The converted time.
		/// </returns>
		/// <param name='fromId'>
		/// The places identifier
		/// </param>
		/// <param name='Date'>
		/// Date.
		/// </param>
		/// <param name='ToIds'>
		/// The place IDs to convert to.
		/// </param>
		public async Task<ConvertedTimes> ConvertTimeAsync (LocationId fromId, DateTime date, IList<LocationId> toIds = null)
		{	
			return await ConvertTimeAsync (fromId, date.ToString ("s", CultureInfo.InvariantCulture), toIds);
		}					
			
		private NameValueCollection GetArgumentsForToIds (IList<LocationId> toIds)
		{							  
			var args = new NameValueCollection ();
			var list = new List<string> ();
			
			foreach (var id in toIds)
			{
				var idstr = id.GetIdAsString ();
				if (!string.IsNullOrEmpty (idstr) && !idstr.Contains (","))
					list.Add (idstr);
				else if(!string.IsNullOrEmpty(idstr) && idstr.Contains (","))
					throw new IdFormatException ("Place ID cannot contain any commas");				
			}				
			
			var arr = list.ToArray ();
			var str = string.Join (",", arr);
			
			args.Set ("toid", str);
			
			return args;
		}
		
		private NameValueCollection GetCommonArguments (string fromId)
		{
			var args = new NameValueCollection ();
			args.Set ("timechanges", IncludeTimeChanges.ToNum ());
			args.Set ("tz", IncludeTimezoneInformation.ToNum ());
			args.Set ("fromid", fromId);
			args.Set ("lang", Language);			
			args.Set ("radius", Radius.ToString ());
			args.Set ("verbosetime", Constants.DefaultVerboseTimeValue.ToString ());			
			return args;
		}		

		protected override ConvertedTimes FromString<ConvertedTimes> (string result)
		{
			var xml = new XmlDocument ();

			xml.LoadXml (result);

			var utc = xml.SelectSingleNode ("data/utc/time");
			var locations = xml.GetElementsByTagName ("location");
			var locationList = new List<Location>();
			TADTime tad = new TADTime();

			if (utc != null)
				tad = (TADTime)utc;

			if (locations != null)
			{
				foreach (XmlNode location in locations)
				{
					locationList.Add ((Location)location);
				}
			}

			var instance = Activator.CreateInstance(typeof(ConvertedTimes), new object[] { locationList, tad });
			return (ConvertedTimes) instance;
		}
	}
}

