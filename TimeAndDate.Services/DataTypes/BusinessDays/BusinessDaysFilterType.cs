using System;
namespace TimeAndDate.Services.DataTypes.BusinessDays
{
	[Flags]
	public enum BusinessDaysFilterType
	{
		All = 1,
		Monday = 1 << 1,
		Tuesday = 1 << 2,
		Wednesday = 1 << 3,
		Thursday = 1 << 4,
		Friday = 1 << 5,
		Saturday = 1 << 6,
		Sunday = 1 << 7,
		Weekend = 1 << 8,
		Holidays = 1 << 9,
		WeekendHolidays = 1 << 10,
		None = 1 << 11
	}
}
