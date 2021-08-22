using System;
using System.Reflection;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeAndDate.Services.DataTypes.Astro;
using TimeAndDate.Services.DataTypes.Places;
using TimeAndDate.Services.DataTypes.Holidays;
using TimeAndDate.Services.DataTypes.BusinessDays;
using TimeAndDate.Services.DataTypes.OnThisDay;

namespace TimeAndDate.Services.Common
{
	internal static class StringHelpers
	{
		internal static string ToNum (this bool boolean)
		{
			return boolean ? "1" : "0";
		}
		
		internal static string ResolveHolidayType (Func<dynamic, bool> expr)
		{
			var x = new[] {
				new { arg = "all", type = HolidayType.All },
				new { arg = "default", type = HolidayType.Default },
				new { arg = "countrydefault", type = HolidayType.DefaultForCountry },
				new { arg = "obs", type = HolidayType.Observances },
				new { arg = "federal", type = HolidayType.Federal },
				new { arg = "federallocal", type = HolidayType.FederalLocal },
				new { arg = "local", type = HolidayType.Local },
				new { arg = "flagday", type = HolidayType.FlagDays },
				new { arg = "local2", type = HolidayType.LocalObservances },
				new { arg = "obs1", type = HolidayType.ImportantObservances },
				new { arg = "obs2", type = HolidayType.CommonObservances },
				new { arg = "obs3", type = HolidayType.OtherObservances },
				new { arg = "weekday", type = HolidayType.Weekdays },
				new { arg = "christian", type = HolidayType.Christian },
				new { arg = "buddhism", type = HolidayType.Buddhism },
				new { arg = "hebrew", type = HolidayType.Hebrew },
				new { arg = "hinduism", type = HolidayType.Hinduism },
				new { arg = "muslim", type = HolidayType.Muslim },
				new { arg = "orthodox", type = HolidayType.Orthodox },
				new { arg = "seasons", type = HolidayType.Seasons },
				new { arg = "tz", type = HolidayType.TimezoneEvents },
				new { arg = "un", type = HolidayType.UnitedNations },
				new { arg = "world", type = HolidayType.WorldWideObservances }
			}.ToList().SingleOrDefault(expr);

			return x.arg as string;
		}

		internal static string ResolveEventType (Func<dynamic, bool> expr)
		{
			var x = new[] {
				new { arg = "events", type = EventType.Events },
				new { arg = "births", type = EventType.Births },
				new { arg = "deaths", type = EventType.Deaths }
			}.ToList().SingleOrDefault(expr);

			return x.arg as string;
		}

		internal static string ResolveBusinessDaysFilterType(Func<dynamic, bool> expr)
		{
			var x = new[] {
				new { arg = "all", type = BusinessDaysFilterType.All },
				new { arg = "mon", type = BusinessDaysFilterType.Monday },
				new { arg = "tue", type = BusinessDaysFilterType.Tuesday },
				new { arg = "wed", type = BusinessDaysFilterType.Wednesday },
				new { arg = "thu", type = BusinessDaysFilterType.Thursday },
				new { arg = "fri", type = BusinessDaysFilterType.Friday },
				new { arg = "sat", type = BusinessDaysFilterType.Saturday },
				new { arg = "sun", type = BusinessDaysFilterType.Sunday },
				new { arg = "weekend", type = BusinessDaysFilterType.Weekend },
				new { arg = "holidays", type = BusinessDaysFilterType.Holidays },
				new { arg = "weekendholidays", type = BusinessDaysFilterType.WeekendHolidays },
				new { arg = "none", type = BusinessDaysFilterType.None }
			}.ToList().SingleOrDefault(expr);

			return x.arg as string;
		}

		internal static string ResolveAstronomyEventClass (Func<dynamic, bool> expr)
		{
			var x = new[] {
				new { arg = "all", type = AstronomyEventClass.All },
				new { arg = "current", type = AstronomyEventClass.Current },
				new { arg = "daylength", type = AstronomyEventClass.DayLength },
				new { arg = "meridian", type = AstronomyEventClass.Meridian },
				new { arg = "phase", type = AstronomyEventClass.Phase },
				new { arg = "setrise", type = AstronomyEventClass.SetRise },
				new { arg = "twilight", type = AstronomyEventClass.AllTwilights },
				new { arg = "twilight6", type = AstronomyEventClass.CivilTwilight },
				new { arg = "twilight12", type = AstronomyEventClass.NauticalTwilight },
				new { arg = "twilight18", type = AstronomyEventClass.AstronomicalTwilight }
			}.ToList().SingleOrDefault(expr);

			return x.arg as string;
		}

		internal static string ResolveAstronomyObjectType (Func<dynamic, bool> expr)
		{
			var x = new[] {
				new { arg = "sun", type = AstronomyObjectType.Sun },
				new { arg = "moon", type = AstronomyObjectType.Moon },
				new { arg = "mercury", type = AstronomyObjectType.Mercury},
				new { arg = "venus", type = AstronomyObjectType.Venus},
				new { arg = "mars", type = AstronomyObjectType.Mars},
				new { arg = "jupiter", type = AstronomyObjectType.Jupiter},
				new { arg = "saturn", type = AstronomyObjectType.Saturn},
				new { arg = "uranus", type = AstronomyObjectType.Uranus},
				new { arg = "neptune", type = AstronomyObjectType.Neptune},
				new { arg = "pluto", type = AstronomyObjectType.Pluto},
			}.ToList().SingleOrDefault(expr);

			return x.arg as string;
		}
		
		internal static AstronomyEventCode ResolveAstronomyEventCode (string eventCode)
		{
			switch (eventCode)
			{
			case "twi18_start":
				return AstronomyEventCode.AstronomicalTwilightStarts;
			case "twi12_start":
				return AstronomyEventCode.NauticalTwilightStarts;
			case "twi6_start":
				return AstronomyEventCode.CivilTwilightStarts;
			case "rise":
				return AstronomyEventCode.Rise;
			case "meridian":
				return AstronomyEventCode.Meridian;
			case "antimeridian":
				return AstronomyEventCode.AntiMeridian;
			case "set":
				return AstronomyEventCode.Set;
			case "twi6_end":
				return AstronomyEventCode.CivilTwilightEnds;
			case "twi12_end":
				return AstronomyEventCode.NauticalTwilightEnds;
			case "twi18_end":
				return AstronomyEventCode.AstronomicalTwilightEnds;
			case "newmoon":
				return AstronomyEventCode.NewMoon;
			case "firstquarter":
				return AstronomyEventCode.FirstQuarter;
			case "fullmoon":
				return AstronomyEventCode.FullMoon;
			case "thirdquarter":
				return AstronomyEventCode.ThirdQuarter;
			default:
				throw new ArgumentException ("EventCode does not conform to enum AstronomyEventCode");
			}
		}
		
		private static string PlaceIdByCoordinates(decimal latitude, decimal longitude)
		{
			var coords = new StringBuilder ();
			if (latitude >= 0)
				coords.Append ("+");
			
			coords.Append (latitude.ToString (CultureInfo.InvariantCulture));
			
			if (longitude >= 0)
				coords.Append ("+");
			
			coords.Append (longitude.ToString (CultureInfo.InvariantCulture));
			
			return coords.ToString();
		}
		
		internal static string GetIdAsString(this LocationId placeId)
		{
			var id = string.Empty;
			if (placeId.NumericId.HasValue)
				id = placeId.NumericId.Value.ToString ();
			else if (!String.IsNullOrEmpty(placeId.TextualId))
				id = placeId.TextualId;
			else if (placeId.CoordinatesId != null)
				id = StringHelpers.PlaceIdByCoordinates (placeId.CoordinatesId.Latitude, placeId.CoordinatesId.Longitude);
			
			return id;
		}
	}
}
