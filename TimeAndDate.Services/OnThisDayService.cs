using System;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Net;
using System.Collections.Generic;
using TimeAndDate.Services.Common;
using TimeAndDate.Services.DataTypes.OnThisDay;
using System.Xml;

namespace TimeAndDate.Services
{
	public class OnThisDayService : BaseService
	{
		/// <summary>
		/// Event types which should be returned.
		/// To combine multiple classes, use the binary OR operator.
		///
		/// Example:
		/// <c>service.Types = EventType.Birth | EventType.Death</c>
		/// </summary>
		/// <value>
		/// The event types.
		/// </value>
		public EventType Types { get; set; }

		/// <summary>
		/// The onthisday service can be used to retrieve a list of events, births and deaths for a specific month and day.
		/// </summary>
		/// <param name='accessKey'>
		/// Access key.
		/// </param>
		/// <param name='secretKey'>
		/// Secret key.
		/// </param>
		private OnThisDayService (string accessKey, string secretKey) : base(accessKey, secretKey, "onthisday") 
		{
			XmlElemName = "onthisday";
		}

		/// <summary>
		/// The onthisday service can be used to retrieve a list of events, births and deaths for a specific month and day.
		/// </summary>
		/// <returns>
		/// <c>OnThisDayResponse<c> containing requested information.
		/// </returns>
		/// <param name='month'>
		/// The month for which the events should be retrieved.
		/// </param>
		/// <param name='year'>
		/// The day for which the events should be retrieved.
		/// </param>
		public OnThisDayResponse EventsOnThisDay (int month, int day)
		{
			var args = GetArguments (month, day);
			return CallService<OnThisDayResponse> (args);
		}

		/// <summary>
		/// The onthisday service can be used to retrieve a list of events, births and deaths for a specific month and day.
		/// This overload uses the current month and day by default.
		/// </summary>
		/// <returns>
		/// <c>OnThisDayResponse<c> containing requested information.
		/// </returns>
		public OnThisDayResponse EventsOnThisDay ()
		{
			var args = GetArguments (null, null);
			return CallService<OnThisDayResponse> (args);
		}

		/// <summary>
		/// The onthisday service can be used to retrieve a list of events, births and deaths for a specific month and day.
		/// </summary>
		/// <returns>
		/// <c>OnThisDayResponse<c> containing requested information.
		/// </returns>
		/// <param name='month'>
		/// The month for which the events should be retrieved.
		/// </param>
		/// <param name='year'>
		/// The day for which the events should be retrieved.
		/// </param>
		public async Task<OnThisDayResponse> EventsOnThisDayAsync (int month, int day)
		{
			var args = GetArguments (month, day);
			return await CallServiceAsync<OnThisDayResponse> (args);
		}

		/// <summary>
		/// The onthisday service can be used to retrieve a list of events, births and deaths for a specific month and day.
		/// This overload uses the current month and day by default.
		/// </summary>
		/// <returns>
		/// <c>OnThisDayResponse<c> containing requested information.
		/// </returns>
		public async Task<OnThisDayResponse> EventsOnThisDayAsync ()
		{
			var args = GetArguments (null, null);
			return await CallServiceAsync<OnThisDayResponse> (args);
		}

		private NameValueCollection GetArguments (int? month, int? day)
		{
			var args = new NameValueCollection ();
			var types = GetEventTypes ();
			if (month != null)
				args.Set ("month", month.ToString());
			if (day != null)
				args.Set ("day", day.ToString());

			args.Set ("lang", Language);
			args.Set ("verbosetime", Constants.DefaultVerboseTimeValue.ToString ());

			if (!string.IsNullOrEmpty (types))
				args.Set ("types", types);

			return args;
		}

		protected override OnThisDayResponse FromString<OnThisDayResponse> (string result)
		{
			var xml = new XmlDocument ();
			xml.LoadXml (result);

			var node = xml.SelectSingleNode ("data");

			var events = node.SelectSingleNode ("events");
			var births = node.SelectSingleNode ("births");
			var deaths = node.SelectSingleNode ("deaths");

			var events_list = new List<Event> ();
			var births_list = new List<Person> ();
			var deaths_list = new List<Person> ();

			if (events != null)
			{
				foreach (XmlNode child in events.ChildNodes)
					events_list.Add ((Event) child);
			}

			if (births != null)
			{
				foreach (XmlNode child in births.ChildNodes)
					births_list.Add ((Person) child);
			}

			if (deaths != null)
			{
				foreach (XmlNode child in deaths.ChildNodes)
					deaths_list.Add ((Person) child);
			}

			var instance = Activator.CreateInstance(typeof(OnThisDayResponse), new object[] { events_list, births_list, deaths_list });
			return (OnThisDayResponse) instance;
		}

		private string GetEventTypes ()
		{
			var includedStrings = new List<string> ();
			foreach (EventType eventType in Enum.GetValues(typeof (EventType)))
			{
				if (Types.HasFlag (eventType))
				{
					includedStrings.Add (StringHelpers.ResolveEventType (x => (int)x.type == (int)eventType));
				}
			}

			var included = string.Join (",", includedStrings.ToArray ());
			return included;
		}
	}
}
