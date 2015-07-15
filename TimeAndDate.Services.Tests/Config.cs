using System;
using System.Configuration;
using System.Collections.Specialized;


namespace TimeAndDate.Services.Tests
{
	public static class Config
	{
		public static string AccessKey = ConfigurationManager.AppSettings["TADaccessKey"];
		public static string SecretKey = ConfigurationManager.AppSettings["TADsecretKey"];
	}
}

