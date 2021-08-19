using System;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Net;
using TimeAndDate.Services.Common;
using System.Collections.Generic;
using System.Xml;
using TimeAndDate.Services.DataTypes.Places;


namespace TimeAndDate.Services
{
	public class PlacesService : BaseService
	{
		/// <summary>
		/// Return coordinates for the Geography object.
		/// </summary>
		/// <value>
		/// <c>true</c> to include coordinates; otherwise, <c>false</c>. <c>true</c> is default.
		/// </value>
		public bool IncludeCoordinates { get; set; }
				
		/// <summary>
		/// The places service can be used to retrieve the list of supported places. 
		/// The ids for the places are then used in the other services to indicate the 
		/// location to be queried.
		/// </summary>
		/// <param name='accessKey'>
		/// Access key.
		/// </param>
		/// <param name='secretKey'>
		/// Secret key.
		/// </param>
		public PlacesService (string accessKey, string secretKey) : base(accessKey, secretKey, "places")
		{
			IncludeCoordinates = true;
			XmlElemName = "place";
		}

		/// <summary>
		/// Gets list of supported places
		/// </summary>
		/// <returns>
		/// The places.
		/// </returns>
		public IList<Place> GetPlaces ()
		{
			var args = GetArguments ();
			return CallService (args, x => (Place)x);
		}
		
		/// <summary>
		/// Gets list of supported places
		/// </summary>
		/// <returns>
		/// The places.
		/// </returns>
		public async Task<IList<Place>> GetPlacesAsync ()
		{
			var args = GetArguments ();
			return await CallServiceAsync (args, x => (Place)x);
		}
		
		private NameValueCollection GetArguments ()
		{
			var args = new NameValueCollection ();
			args.Set ("lang", Language);			
			args.Set ("geo", IncludeCoordinates.ToNum ());
			args.Set ("verbosetime", Constants.DefaultVerboseTimeValue.ToString ());
			
			return args;
		}
	}
}

