using System;
using System.Reflection;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using TimeAndDate.Services.DataTypes.Astro;
using System.Text;
using TimeAndDate.Services.DataTypes.Places;

namespace TimeAndDate.Services.Common
{
	internal static class StringHelpers
	{
		internal static string ToNum (this bool boolean)
		{
			return boolean ? "1" : "0";
		}
		
		// TODO:
		// This needs to be converted to a struct from Tuple for clarity
		internal static Tuple<int, string, HolidayType> ResolveHolidayType (Func<Tuple<int, string, HolidayType>, bool> expr)
		{
			var list = new List<Tuple<int, string, HolidayType>> ()
			{
				Tuple.Create (1, "all", HolidayType.All),
				Tuple.Create (1 << 1, "default", HolidayType.Default),
				Tuple.Create (1 << 2, "countrydefault", HolidayType.DefaultForCountry),
				Tuple.Create (1 << 3, "obs", HolidayType.Observances),
				Tuple.Create (1 << 4, "federal", HolidayType.Federal),
				Tuple.Create (1 << 5, "federallocal", HolidayType.FederalLocal),
				Tuple.Create (1 << 6, "local", HolidayType.Local),
				Tuple.Create (1 << 7, "flagday", HolidayType.FlagDays),
				Tuple.Create (1 << 8, "local2", HolidayType.LocalObservances),
				Tuple.Create (1 << 9, "obs1", HolidayType.ImportantObservances),
				Tuple.Create (1 << 10, "obs2", HolidayType.CommonObservances),
				Tuple.Create (1 << 11, "obs3", HolidayType.OtherObservances),
				Tuple.Create (1 << 12, "weekday", HolidayType.Weekdays),
				Tuple.Create (1 << 13, "christian", HolidayType.Christian),
				Tuple.Create (1 << 14, "buddhism", HolidayType.Buddhism),
				Tuple.Create (1 << 15, "hebrew", HolidayType.Hebrew),
				Tuple.Create (1 << 16, "hinduism", HolidayType.Hinduism),
				Tuple.Create (1 << 17, "muslim", HolidayType.Muslim),
				Tuple.Create (1 << 18, "orthodox", HolidayType.Orthodox),
				Tuple.Create (1 << 19, "seasons", HolidayType.Seasons),
				Tuple.Create (1 << 20, "tz", HolidayType.TimezoneEvents),
				Tuple.Create (1 << 21, "un", HolidayType.UnitedNations),
				Tuple.Create (1 << 22, "world", HolidayType.WorldWideObservances)
			};
			
			return list.SingleOrDefault (expr);
		}			
		
		// TODO:
		// This needs to be converted to a struct from Tuple for clarity
		internal static Tuple<int, string, AstronomyEventClass> ResolveAstronomyEventClass (Func<Tuple<int, string, AstronomyEventClass>, bool> expr)
		{
			var list = new List<Tuple<int, string, AstronomyEventClass>> ();
			list.Add (Tuple.Create (1, "all", AstronomyEventClass.All));
			list.Add (Tuple.Create (1 << 1, "daylength", AstronomyEventClass.DayLength));
			list.Add (Tuple.Create (1 << 2, "meridian", AstronomyEventClass.Meridian));
			list.Add (Tuple.Create (1 << 3, "phase", AstronomyEventClass.Phase));
			list.Add (Tuple.Create (1 << 4, "setrise", AstronomyEventClass.SetRise));
			list.Add (Tuple.Create (1 << 5, "twilight", AstronomyEventClass.AllTwilights));
			list.Add (Tuple.Create (1 << 6, "twilight6", AstronomyEventClass.CivilTwilight));
			list.Add (Tuple.Create (1 << 7, "twilight12", AstronomyEventClass.NauticalTwilight));
			list.Add (Tuple.Create (1 << 8, "twilight18", AstronomyEventClass.AstronomicalTwilight));
			
			return list.SingleOrDefault (expr);
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