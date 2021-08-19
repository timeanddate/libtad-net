using System;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Linq;
using TimeAndDate.Services.Tests;
using TimeAndDate.Services;

namespace TimeAndDate.Services.Tests.IntegrationTests
{
	[TestFixture()]
	public class AccountServiceTestsAsync
	{
		[Test()]
		public async Task Calling_AccountServices_Should_Return_An_Account ()
		{
			// Arrange

			// Act
			var service = new AccountService (Config.AccessKey, Config.SecretKey);
			var account = await service.GetAccountAsync ();

			// Assert
			Assert.IsNotNull(account.Id);
			Assert.IsNotNull(account.Timestamp);
			Assert.IsNotNull(account.Credits);
			Assert.IsNotNull(account.Packages);
		}
	}
}
