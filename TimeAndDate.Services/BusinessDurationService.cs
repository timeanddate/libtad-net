using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Xml;
using TimeAndDate.Services.Common;
using TimeAndDate.Services.DataTypes.BusinessDays;
using TimeAndDate.Services.DataTypes.Places;

namespace TimeAndDate.Services
{
	public class BusinessDurationService : BaseService
	{
		public bool IncludeLastDate { get; set; }
		public bool IncludeDays { get; set; }
		public BusinessDaysFilterType Filter { get; set; }

		public BusinessDurationService(string accessKey, string secretKey) : base(accessKey, secretKey, "businessduration")
		{
			IncludeLastDate = false;
			IncludeDays = false;
			Filter = BusinessDaysFilterType.WeekendHolidays;
			XmlElemName = "period";
		}

		public BusinessDuration GetDuration(DateTime startDate, DateTime endDate, LocationId placeId)
		{
			if (endDate.CompareTo(startDate) < 0)
				throw new ArgumentException("End Date cannot be earlier than Start Date");

			var args = GetArguments(startDate, endDate);
			args.Set("placeid", placeId.GetIdAsString());
			return CallService<BusinessDuration>(args);
		}

		public BusinessDuration GetDuration(DateTime startDate, DateTime endDate, string country, string state = "")
		{
			if (endDate.CompareTo(startDate) < 0)
				throw new ArgumentException("End Date cannot be earlier than Start Date");

			var args = GetArguments(startDate, endDate);
			args.Set("country", country);
			args.Set("state", state);
			return CallService<BusinessDuration>(args);
		}

		public async Task<BusinessDuration> GetDurationAsync(DateTime startDate, DateTime endDate, LocationId placeId)
		{
			if (endDate.CompareTo(startDate) < 0)
				throw new ArgumentException("End Date cannot be earlier than Start Date");

			var args = GetArguments(startDate, endDate);
			args.Set("placeid", placeId.GetIdAsString());
			return await CallServiceAsync<BusinessDuration>(args);
		}

		public async Task<BusinessDuration> GetDurationAsync(DateTime startDate, DateTime endDate, string country, string state = "")
		{
			if (endDate.CompareTo(startDate) < 0)
				throw new ArgumentException("End Date cannot be earlier than Start Date");

			var args = GetArguments(startDate, endDate);
			args.Set("country", country);
			args.Set("state", state);
			return await CallServiceAsync<BusinessDuration>(args);
		}

		private NameValueCollection GetArguments(DateTime startDate, DateTime endDate)
		{
			var args = new NameValueCollection();
			args.Set("filter", GetFilterTypes());
			args.Set("startdt", startDate.ToString("s", CultureInfo.InvariantCulture));
			args.Set("enddt", endDate.ToString("s", CultureInfo.InvariantCulture));
			args.Set("include", IncludeDays.ToNum());
			args.Set("includelastdate", IncludeLastDate.ToNum());
			args.Set("lang", Language);
			args.Set("verbosetime", Constants.DefaultVerboseTimeValue);

			return args;
		}

		protected override BusinessDuration FromString<BusinessDuration>(string result)
		{
			var xml = new XmlDocument();

			xml.LoadXml(result);

			var geoElem = xml.SelectSingleNode("data/geo");
			var periodElem = xml.SelectSingleNode("data/period");
			Geo geo = new Geo();
			Period period = new Period();

			if (geoElem != null)
				geo = (Geo)geoElem;

			if (periodElem != null)
				period = (Period)periodElem;

			var instance = Activator.CreateInstance(typeof(BusinessDuration), new object[] { geo, period });
			return (BusinessDuration)instance;
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
