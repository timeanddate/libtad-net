using System;
using System.Xml;
using TimeAndDate.Services.Common;
using System.Collections.Specialized;

namespace TimeAndDate.Services
{
	public abstract class BaseService 
	{
		/// <summary>
		/// Specify the API Version. 
		/// 
		/// WARNING: KNOW WHAT YOU'RE DOING IF YOU CHANGE THIS
		/// </summary>
		/// <value>
		/// The version. <c>1</c> is the default.
		/// </value>
		public int Version { get; set; }
		
		/// <summary>
		/// The preferred language for the texts. An error will be raised if the language code cannot be recognized. In case the text 
		/// for a specific event cannot be retrieved in the requested language it will be returned in english instead. This is also 
		/// the default language.
		/// </summary>
		/// <value>
		/// ISO639 language code. <c>en</c> is default..
		/// </value>
		public string Language { get; set; }
		
		protected readonly string ServiceName;
		
		protected readonly NameValueCollection AuthenticationOptions;
		
		protected BaseService (string accessKey, string secretKey, string serviceName)
		{
			var auth = new Authentication (serviceName, accessKey, secretKey);
			AuthenticationOptions = auth.GetAuthenticationArgs ();
			InMemStore.Store (typeof(Authentication).Name, new AuthOptions (accessKey, secretKey));
			
			ServiceName = serviceName;
			Version = Constants.DefaultVersion;
			Language = Constants.DefaultLanguage;

		}				
	}
}

