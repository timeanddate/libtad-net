using System;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Net;
using TimeAndDate.Services.Common;
using System.Collections.Generic;
using System.Xml;
using TimeAndDate.Services.DataTypes.Account;


namespace TimeAndDate.Services
{
	public class AccountService : BaseService
	{
		/// <summary>
		/// The accounts service can be used to retrieve information about the
		/// API account in use, remaining request credits and current packages.
		/// </summary>
		/// <param name='accessKey'>
		/// Access key.
		/// </param>
		/// <param name='secretKey'>
		/// Secret key.
		/// </param>
		public AccountService (string accessKey, string secretKey) : base(accessKey, secretKey, "account")
		{
			XmlElemName = "account";
		}

		/// <summary>
		/// Gets information about account in use.
		/// </summary>
		/// <returns>
		/// The account.
		/// </returns>
		public Account GetAccount ()
		{
			return CallService<Account> (new NameValueCollection ());
		}

		/// <summary>
		/// Gets information about account in use.
		/// </summary>
		/// <returns>
		/// The account.
		/// </returns>
		public async Task<Account> GetAccountAsync ()
		{
			return await CallServiceAsync<Account> (new NameValueCollection ());
		}

		protected override Account FromString<Account> (string result)
		{
			var xml = new XmlDocument();
			xml.LoadXml (result);

			var node = xml.SelectSingleNode ("data");
			node = node.SelectSingleNode ("account");

			var instance = Activator.CreateInstance(typeof(Account), new object[] { node });
			return (Account) instance;
		}
	}
}
