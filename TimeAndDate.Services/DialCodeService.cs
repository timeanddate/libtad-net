using System;
using System.Threading.Tasks;
using System.Collections.Specialized;
using TimeAndDate.Services.Common;
using System.Net;
using TimeAndDate.Services.DataTypes.Places;
using System.Xml;
using TimeAndDate.Services.DataTypes.DialCode;
using System.Collections.Generic;


namespace TimeAndDate.Services
{
	public class DialCodeService : BaseService
	{		
		/// <summary>
		/// Return detailed information about the supplied locations.
		/// </summary>
		/// <value>
		/// <c>true</c> if include detailed information; otherwise, <c>false</c>. <c>true</c> is default.
		/// </value>
		public bool IncludeLocations { get; set; }
		
		/// <summary>
		/// Return coordinates for the Geography object.
		/// </summary>
		/// <value>
		/// <c>true</c> if include coordinates; otherwise, <c>false</c>. <c>true</c> is default.
		/// </value>
		public bool IncludeCoordinates { get; set; }
		
		/// <summary>
		/// Adds current time under the location object.
		/// </summary>
		/// <value>
		/// <c>true</c> if include current time; otherwise, <c>false</c>. <c>true</c> is default.
		/// </value>
		public bool IncludeCurrentTime { get; set; }
		
		/// <summary>
		/// Add timezone information under the time object.
		/// </summary>
		/// <value>
		/// <c>true</c> if include timezone information; otherwise, <c>false</c>. <c>true</c> is default.
		/// </value>
		public bool IncludeTimezoneInformation { get; set; }	
		
		private int? _number;
		
		/// <summary>
		/// The dialcode service can be used determine which phone number shall be used to call a specific location.
		/// </summary>
		/// <param name='accessKey'>
		/// Access key.
		/// </param>
		/// <param name='secretKey'>
		/// Secret key.
		/// </param>
		public DialCodeService (string accessKey, string secretKey) : base(accessKey, secretKey, "dialcode")
		{ 
			IncludeCurrentTime = true;
			IncludeLocations = true;
			IncludeCoordinates = true;
			IncludeTimezoneInformation = true;
		}
		
		/// <summary>
		/// Gets the dial code for the location you want to call
		/// </summary>
		/// <returns>
		/// The dial code.
		/// </returns>
		/// <param name='toLocation'>
		/// To location.
		/// </param>
		public async Task<DialCodes> GetDialCode (LocationId toLocation)
		{
			if (toLocation == null)
				throw new ArgumentException ("A required argument is null or empty");
			
			var id = toLocation.GetIdAsString ();
			if (string.IsNullOrEmpty (id))
				throw new ArgumentException ("A required argument is null or empty");
			
			var opts = GetOptionalArguments ();
			opts.Set ("toid", id);
			
			return await RetrieveDialCode (opts);
		}
		
		/// <summary>
		/// Gets the dial code for the location you want to call, from where
		/// </summary>
		/// <returns>
		/// The dial code.
		/// </returns>
		/// <param name='toLocation'>
		/// To location.
		/// </param>
		/// <param name='fromLocation'>
		/// From location.
		/// </param>
		public async Task<DialCodes> GetDialCode (LocationId toLocation, LocationId fromLocation)
		{
			if (toLocation == null || fromLocation == null)
				throw new ArgumentException ("A required argument is null or empty");
			
			var toId = toLocation.GetIdAsString ();
			var fromId = fromLocation.GetIdAsString ();
			if (string.IsNullOrEmpty (toId) ||string.IsNullOrEmpty(fromId))
				throw new ArgumentException ("A required argument is null or empty");
			
			var opts = GetOptionalArguments ();
			opts.Set ("toid", toId);
			opts.Set ("fromid", fromId);
			
			return await RetrieveDialCode (opts);
		}
		
		/// <summary>
		/// Gets the dial code for the location you want to call, from where with number
		/// </summary>
		/// <returns>
		/// The dial code.
		/// </returns>
		/// <param name='toLocation'>
		/// To location.
		/// </param>
		/// <param name='fromLocation'>
		/// From location.
		/// </param>
		/// <param name='number'>
		/// Number.
		/// </param>
		public async Task<DialCodes> GetDialCode (LocationId toLocation, LocationId fromLocation, int number)
		{
			_number = number;
			return await GetDialCode (toLocation, fromLocation);
		}
		
		private async Task<DialCodes> RetrieveDialCode(NameValueCollection args)
		{
			var result = await CallService<DialCodes>(args);
			return (DialCodes)result;
		}
		
		private NameValueCollection GetOptionalArguments ()
		{
			var args = new NameValueCollection ();
			
			args.Set ("locinfo", IncludeLocations.ToNum ());
			args.Set ("geo", IncludeCoordinates.ToNum ());
			args.Set ("time", IncludeCurrentTime.ToNum ());
			args.Set ("tz", IncludeTimezoneInformation.ToNum ());
			args.Set ("verbosetime", Constants.DefaultVerboseTimeValue.ToString ());
			
			if (_number.HasValue)
				args.Set ("number", _number.Value.ToString());
			
			return args;
		}

		protected override DialCodes FromString<DialCodes> (string result)
		{
			var xml = new XmlDocument ();
			xml.LoadXml (result);

			var dataNode = xml.DocumentElement;
			var number = dataNode.SelectSingleNode ("number/full");
			var num = "";
			var comp = new List<Composition> ();
			var locs = new List<Location> ();

			if (number != null)
				num = number.InnerText;

			var compositions = dataNode.SelectSingleNode ("composition");
			if (compositions != null)
				foreach (XmlNode composition in compositions)
					comp.Add ((Composition)composition);

			var locations = dataNode.GetElementsByTagName ("location");
			if(locations != null)
				foreach(XmlNode location in locations)
					locs.Add((Location)location);

			var instance = Activator.CreateInstance(typeof(DialCodes), new object[] { comp, locs, num });
			return (DialCodes)instance;
		}
	}
}

