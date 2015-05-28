using System.Collections.Specialized;
using System.Collections.Generic;
using System.Web;

namespace TimeAndDate.Services.Common
{
	internal static class UriUtils
	{
		internal static string BuildUriString (NameValueCollection args)
		{
			var items = new List<string> ();
			foreach (var key in args.AllKeys) 
				items.Add (string.Concat (HttpUtility.UrlEncode (key), "=", HttpUtility.UrlEncode (args [key])));
			
			return string.Join ("&", items.ToArray());
		}
	}
}

