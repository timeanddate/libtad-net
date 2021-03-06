using System;
using TimeAndDate.Services.DataTypes.Time;
namespace TimeAndDate.Services.Common
{
	internal static class DateTimeUtils
	{
		/// <summary>
		/// Compares DateTime t1 with t2 like DateTime.CompareTo but without the TZ conversions
		/// </summary>
        	internal static int ToMinuteCompare(this TADDateTime t1, TADDateTime t2)
		{
            		var t1ticks = ((DateTimeOffset) t1).Ticks;
            		var t2ticks = ((DateTimeOffset) t2).Ticks;
			
            		if (t1ticks > t2ticks)
				return 1;
			if (t1ticks < t2ticks)
				return -1;
			
			return 0;			
		}
	}
}
