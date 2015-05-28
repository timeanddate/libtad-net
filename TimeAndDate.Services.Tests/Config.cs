using System;
using System.Configuration;
using System.Collections.Specialized;


namespace TimeAndDate.Services.Tests
{
	public static class Config
	{
		private static NameValueCollection conf = (NameValueCollection)ConfigurationManager.GetSection("tad");
		public static string AccessKey = conf["AccessKey"];
		public static string SecretKey = conf["SecretKey"];
	}
}

