using System;
namespace TimeAndDate.Services
{
	// These are just simple bitflags so the user can easily put together a set of types
	// which should be included or excluded in the retrieval of holidays
	[Flags]
	public enum HolidayType
	{
		All = 1,
		Default = 2,
		DefaultForCountry = 4,
		Observances = 8,
		Federal = 16,
		FederalLocal = 32,
		Local = 64,
		FlagDays = 128,
		LocalObservances = 256,
		ImportantObservances = 512,
		CommonObservances = 1024,
		OtherObservances = 2048,
		Weekdays = 4096,
		Christian = 4194304,
		Buddhism = 8192,
		Hebrew = 16384,
		Hinduism = 32768,
		Muslim  = 65536,
		Orthodox = 131072,
		Seasons = 262144,
		TimezoneEvents = 524288,
		UnitedNations = 1048576,
		WorldWideObservances = 2097152
	}
}

