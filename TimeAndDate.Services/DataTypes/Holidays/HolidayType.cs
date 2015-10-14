using System;
namespace TimeAndDate.Services
{
	// These are just simple bitflags so the user can easily put together a set of types
	// which should be included or excluded in the retrieval of holidays
	[Flags]
	public enum HolidayType
	{
		All = 1,
		Default = 1 << 1,
		DefaultForCountry = 1 << 2,
		Observances = 1 << 3,
		Federal = 1 << 4,
		FederalLocal = 1 << 5,
		Local = 1 << 6,
		FlagDays = 1 << 7,
		LocalObservances = 1 << 8,
		ImportantObservances = 1 << 9,
		CommonObservances = 1 << 10,
		OtherObservances = 1 << 11,
		Weekdays = 1 << 12,
		Christian = 1 << 13,
		Buddhism = 1 << 14,
		Hebrew = 1 << 15,
		Hinduism = 1 << 16,
		Muslim  = 1 << 17,
		Orthodox = 1 << 18,
		Seasons = 1 << 19,
		TimezoneEvents = 1 << 20,
		UnitedNations = 1 << 21,
		WorldWideObservances = 1 << 22
	}
}

