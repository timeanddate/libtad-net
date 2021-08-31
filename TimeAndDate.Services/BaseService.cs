using System;
using System.Threading.Tasks;
using System.Xml;
using TimeAndDate.Services.Common;
using System.Collections.Specialized;
using System.Net;
using System.Collections.Generic;

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

		protected string XmlElemName;
		
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

		private string SendRequest(NameValueCollection args) 
		{
			args.Set ("out", Constants.DefaultReturnFormat);
			args.Set ("version", Constants.DefaultVersion.ToString ());
			args.Add (AuthenticationOptions);

			var query = UriUtils.BuildUriString (args);

			var uri = new UriBuilder (Constants.EntryPoint + ServiceName)
			{
				Query = query	
			};

			using (var client = new WebClient ())
			{
				client.Encoding = System.Text.Encoding.UTF8;
				client.Headers.Add(HttpRequestHeader.UserAgent, Constants.DefaultUserAgent);
				var result = client.DownloadString (uri.Uri);
				XmlUtils.CheckForErrors (result);

				return result;
			}	
		}

		protected IList<T> CallService<T>(NameValueCollection args, Func<XmlNode, T> parser) 
		{
			var result = SendRequest (args);
			return FromXml(result, XmlElemName, parser);
		}

		protected T CallService<T>(NameValueCollection args)
		{
			return FromString<T> (SendRequest (args));
		}

		private async Task<string> SendRequestAsync(NameValueCollection args) 
		{
			args.Set ("out", Constants.DefaultReturnFormat);
			args.Set ("version", Constants.DefaultVersion.ToString ());
			args.Add (AuthenticationOptions);

			var query = UriUtils.BuildUriString (args);

			var uri = new UriBuilder (Constants.EntryPoint + ServiceName)
			{
				Query = query	
			};

			using (var client = new WebClient ())
			{
				client.Encoding = System.Text.Encoding.UTF8;
				client.Headers.Add(HttpRequestHeader.UserAgent, Constants.DefaultUserAgent);
				var result = await client.DownloadStringTaskAsync (uri.Uri);
				XmlUtils.CheckForErrors (result);

				return result;
			}	
		}

		protected async Task<IList<T>> CallServiceAsync<T>(NameValueCollection args, Func<XmlNode, T> parser) 
		{
			var result = await SendRequestAsync (args);
			return FromXml(result, XmlElemName, parser);
		}

		protected async Task<T> CallServiceAsync<T>(NameValueCollection args)
		{
			return FromString<T> (await SendRequestAsync (args));
		}

		protected IList<T> FromXml<T>(string result, string elem, Func<XmlNode, T> parser) 
		{
			var list = new List<T> ();
			var xml = new XmlDocument ();
			xml.LoadXml (result);

			var locationNodes = xml.GetElementsByTagName (elem);
			foreach (XmlNode location in locationNodes)
				list.Add (parser(location));

			return list;
		}

		protected virtual T FromString<T>(string result)
		{
			return default(T);	
		}
	}
}

