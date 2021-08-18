using System;

namespace TimeAndDate.Services.Tests
{
	public static class Config
	{
		public static string AccessKey = Environment.GetEnvironmentVariable("ACCESS_KEY");
		public static string SecretKey = Environment.GetEnvironmentVariable("SECRET_KEY");
	}
}

