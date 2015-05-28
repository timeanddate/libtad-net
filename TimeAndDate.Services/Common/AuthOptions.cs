using System;
namespace TimeAndDate.Services.Common
{
	internal class AuthOptions
	{
		public string AccessKey { get; private set;	}
		public string SecretKey { get; private set;	}
		
		internal AuthOptions (string accessKey, string secretKey)
		{
			AccessKey = accessKey;
			SecretKey = secretKey;
		}
	}
}

