using System;
using NUnit.Framework;
using System.Linq;
using TimeAndDate.Services.Tests;
using TimeAndDate.Services;

namespace TimeAndDate.Services.Tests.IntegrationTests
{
	[TestFixture()]
	public class AccountServiceTests
	{
		[Test()]
		public void Calling_AccountServices_Should_Return_An_Account ()
		{
			// Arrange

			// Act
			var service = new AccountService (Config.AccessKey, Config.SecretKey);
			var account = service.GetAccount ();

			// Assert
			Assert.IsNotNull(account.Id);
			Assert.IsNotNull(account.Timestamp);
			Assert.IsNotNull(account.Credits);
			Assert.IsNotNull(account.Packages);
		}
	}
}
