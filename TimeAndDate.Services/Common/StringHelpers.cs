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
				Tuple.Create (2, "default", HolidayType.Default),
				Tuple.Create (4, "countrydefault", HolidayType.DefaultForCountry),
				Tuple.Create (8, "obs", HolidayType.Observances),
				Tuple.Create (16, "federal", HolidayType.Federal),
				Tuple.Create (32, "federallocal", HolidayType.FederalLocal),
				Tuple.Create (64, "local", HolidayType.Local),
				Tuple.Create (128, "flagday", HolidayType.FlagDays),
				Tuple.Create (256, "local2", HolidayType.LocalObservances),
				Tuple.Create (512, "obs1", HolidayType.ImportantObservances),
				Tuple.Create (1024, "obs2", HolidayType.CommonObservances),
				Tuple.Create (2048, "obs3", HolidayType.OtherObservances),
				Tuple.Create (4096, "weekday", HolidayType.Weekdays),
				Tuple.Create (8192, "buddhism", HolidayType.Buddhism),
				Tuple.Create (16384, "hebrew", HolidayType.Hebrew),
				Tuple.Create (32768, "hinduism", HolidayType.Hinduism),
				Tuple.Create (65536, "muslim", HolidayType.Muslim),
				Tuple.Create (131072, "orthodox", HolidayType.Orthodox),
				Tuple.Create (262144, "seasons", HolidayType.Seasons),
				Tuple.Create (524288, "tz", HolidayType.TimezoneEvents),
				Tuple.Create (1048576, "un", HolidayType.UnitedNations),
				Tuple.Create (2097152, "world", HolidayType.WorldWideObservances),
				Tuple.Create (4194304, "christian", HolidayType.Christian)
			};
			
			return list.SingleOrDefault (expr);
		}			
		
		// TODO:
		// This needs to be converted to a struct from Tuple for clarity
		internal static Tuple<int, string, AstronomyEventClass> ResolveAstronomyEventClass (Func<Tuple<int, string, AstronomyEventClass>, bool> expr)
		{
			var list = new List<Tuple<int, string, AstronomyEventClass>> ();
			list.Add (Tuple.Create (1, "all", AstronomyEventClass.All));
			list.Add (Tuple.Create (2, "daylength", AstronomyEventClass.DayLength));
			list.Add (Tuple.Create (4, "meridian", AstronomyEventClass.Meridian));
			list.Add (Tuple.Create (8, "phase", AstronomyEventClass.Phase));
			list.Add (Tuple.Create (16, "setrise", AstronomyEventClass.SetRise));
			list.Add (Tuple.Create (32, "twilight", AstronomyEventClass.AllTwilights));
			list.Add (Tuple.Create (64, "twilight6", AstronomyEventClass.CivilTwilight));
			list.Add (Tuple.Create (128, "twilight12", AstronomyEventClass.NauticalTwilight));
			list.Add (Tuple.Create (256, "twilight18", AstronomyEventClass.AstronomicalTwilight));
			
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