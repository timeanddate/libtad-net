using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using TimeAndDate.Services.Common;
using TimeAndDate.Services.DataTypes.BusinessDays;
using TimeAndDate.Services.DataTypes.Places;
using System.Xml;

namespace TimeAndDate.Services
{
	public class BusinessDateService : BaseService
	{
		public bool IncludeDays { get; set; }

		public int Repeat { get; set; }

		public BusinessDaysFilterType Filter { get; set; }

		public BusinessDateService(string accessKey, string secretKey) : base(accessKey, secretKey, "businessdate")
		{
			IncludeDays = false;
			XmlElemName = "period";
			Filter = BusinessDaysFilterType.WeekendHolidays;
		}

		private async Task<BusinessDates> CommonCallService(string op, DateTime startDate, int[] days, string country, string state = "")
		{
			if (days.Length > 1 && Repeat > 0)
				throw new ArgumentException("Cannot set Repeat when querying for more than 1 day");

			var args = GetArguments(op, startDate, days);
			args.Set("country", country);

			if (!String.IsNullOrEmpty(state))
				args.Set("state", state);

			return await CallService<BusinessDates>(args);
		}

		private async Task<BusinessDates> CommonCallService(string op, DateTime startDate, int[] days, LocationId placeId)
		{
			if (days.Length > 1 && Repeat > 0)
				throw new ArgumentException("Cannot set Repeat when querying for more than 1 day");

			var args = GetArguments(op, startDate, days);
			args.Set("placeid", placeId.GetIdAsString());
			return await CallService<BusinessDates>(args);
		}

		/*
         * AddDays with overloads
         */

		public async Task<BusinessDates> AddDays(DateTime startDate, int[] days, string country, string state = "")
		{
			return await CommonCallService("add", startDate, days, country, state);
		}

		public async Task<BusinessDates> AddDays(DateTime startDate, int[] days, LocationId placeId)
		{
			return await CommonCallService("add", startDate, days, placeId);
		}

		public async Task<BusinessDates> AddDays(DateTime startDate, List<int> days, string country, string state = "")
		{
			return await AddDays(startDate, days.ToArray(), country, state);
		}

		public async Task<BusinessDates> AddDays(DateTime startDate, int days, string country, string state = "")
		{
			return await AddDays(startDate, new int[] { days }, country, state);
		}

		public async Task<BusinessDates> AddDays(DateTime startDate, List<int> days, LocationId placeId)
		{
			return await AddDays(startDate, days.ToArray(), placeId);
		}

		public async Task<BusinessDates> AddDays(DateTime startDate, int days, LocationId placeId)
		{
			return await AddDays(startDate, new int[] { days }, placeId);
		}

		/*
         * SubtractDays with overloads
         */

		public async Task<BusinessDates> SubtractDays(DateTime startDate, int[] days, string country, string state = "")
		{
			return await CommonCallService("subtract", startDate, days, country, state);
		}

		public async Task<BusinessDates> SubtractDays(DateTime startDate, int[] days, LocationId placeId)
		{
			return await CommonCallService("subtract", startDate, days, placeId);
		}

		public async Task<BusinessDates> SubtractDays(DateTime startDate, List<int> days, string country, string state = "")
		{
			return await SubtractDays(startDate, days.ToArray(), country, state);
		}

		public async Task<BusinessDates> SubtractDays(DateTime startDate, int days, string country, string state = "")
		{
			return await SubtractDays(startDate, new int[] { days }, country, state);
		}

		public async Task<BusinessDates> SubtractDays(DateTime startDate, List<int> days, LocationId placeId)
		{
			return await SubtractDays(startDate, days.ToArray(), placeId);
		}

		public async Task<BusinessDates> SubtractDays(DateTime startDate, int days, LocationId placeId)
		{
			return await SubtractDays(startDate, new int[] { days }, placeId);
		}

		private NameValueCollection GetArguments(string op, DateTime startDate, int[] days)
		{
			var args = new NameValueCollection();
			args.Set("filter", GetFilterTypes());
			args.Set("op", op);
			args.Set("startdt", startDate.ToString("s", CultureInfo.InvariantCulture));
			args.Set("days", string.Join(",", days));
			args.Set("include", IncludeDays.ToNum());
			args.Set("lang", Language);
			args.Set("verbosetime", Constants.DefaultVerboseTimeValue);

			if (Repeat > 0)
				args.Set("repeat", Repeat.ToString());

			return args;
		}

		protected override BusinessDates FromString<BusinessDates>(string result)
		{
			var xml = new XmlDocument();

			xml.LoadXml(result);

			var geoElem = xml.SelectSingleNode("data/geo");
			var periodElems = xml.GetElementsByTagName("period");
			Geo geo = new Geo();
			List<Period> periods = new List<Period>();

			if (geoElem != null)
				geo = (Geo)geoElem;

			if (periodElems != null)
			{
				foreach (XmlNode location in periodElems)
				{
					periods.Add((Period)location);
				}
			}

			var instance = Activator.CreateInstance(typeof(BusinessDates), new object[] { geo, periods });
			return (BusinessDates)instance;
		}

		private string GetFilterTypes()
		{
			var includedStrings = new List<string>();
			foreach (BusinessDaysFilterType filterType in Enum.GetValues(typeof(BusinessDaysFilterType)))
			{
				if (Filter.HasFlag(filterType))
				{
					includedStrings.Add(StringHelpers.ResolveBusinessDaysFilterType(x => (int)x.type == (int)filterType));
				}
			}

			var included = string.Join(",", includedStrings.ToArray());
			return included;
		}
	}
}
