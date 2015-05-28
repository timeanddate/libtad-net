using System;
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
		}
		
		/// <summary>
		/// Gets list of supported places
		/// </summary>
		/// <returns>
		/// The places.
		/// </returns>
		public IList<Place> GetPlaces ()
		{
			var arguments = GetArguments ();
			var query = UriUtils.BuildUriString (arguments);
			
			var uri = new UriBuilder (Constants.EntryPoint + ServiceName);
			uri.Query = query;
			using (var client = new WebClient())
			{
				client.Encoding = System.Text.Encoding.UTF8;
				var result = client.DownloadString (uri.Uri);
				XmlUtils.CheckForErrors (result);
				return FromXml (result);				
			}
		}
		
		private NameValueCollection GetArguments ()
		{
			var args = new NameValueCollection (AuthenticationOptions);
			args.Set ("lang", Language);			
			args.Set ("geo", IncludeCoordinates.ToNum ());
			args.Set ("version", Version.ToString ());
			args.Set ("out", Constants.DefaultReturnFormat);
			args.Set ("verbosetime", Constants.DefaultVerboseTimeValue.ToString ());
			
			return args;
		}
		
		private static IList<Place> FromXml(string result)
		{
			var list = new List<Place> ();
			
			var xml = new XmlDocument ();
			xml.LoadXml (result);
						
			var places = xml.GetElementsByTagName ("place");
			foreach (XmlNode place in places)
				list.Add ((Place)place);			
			
			return list;
		}
	}
}

