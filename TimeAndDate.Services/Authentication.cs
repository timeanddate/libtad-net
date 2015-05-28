using System;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;


namespace TimeAndDate.Services
{
	internal class Authentication
	{
		private readonly string _accessKey;
		private readonly string _secretKey;
		private readonly string _service;
		
		internal Authentication (string service, string accessKey, string secretKey)
		{
			_accessKey = accessKey;
			_secretKey = secretKey;		
			_service = service;
		}
		
		internal NameValueCollection GetAuthenticationArgs (NameValueCollection seed)
		{
			return GetArgs (seed);
		}
		
		internal NameValueCollection GetAuthenticationArgs ()
		{
			return GetArgs (null);			
		}
		
		internal NameValueCollection GetArgs (NameValueCollection seed)
		{
			var timestamp = DateTime.UtcNow.ToString ("o");
			var message = _accessKey + _service + timestamp;
			var hmac = new HMACSHA1 (Encoding.ASCII.GetBytes (_secretKey));
			var hash = hmac.ComputeHash (Encoding.ASCII.GetBytes (message));
			var args = seed ?? new NameValueCollection ();
			
			args.Set ("accesskey", _accessKey);
			args.Set ("timestamp", timestamp);
			args.Set ("signature", Convert.ToBase64String (hash));
			
			return args;
		}
	}
}

