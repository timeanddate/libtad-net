using System;
namespace TimeAndDate.Services.DataTypes.DST
{
	/// <summary>
	/// Indicates if the region does not observe DST at all, or is on DST all year long.
	/// </summary>
	public enum DSTSpecialType
	{
		NoDaylightSavingTime,
		DaylightSavingTimeAllYear
	}
}

