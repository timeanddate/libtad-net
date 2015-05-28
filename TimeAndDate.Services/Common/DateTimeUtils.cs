using System;
namespace TimeAndDate.Services.Common
{
	internal static class DateTimeUtils
	{
		/// <summary>
		/// Compares DateTime t1 with t2 like DateTime.CompareTo but without the TZ conversions
		/// </summary>
		internal static int ToMinuteCompare (this DateTimeOffset t1, DateTimeOffset t2)
		{
			var t1ticks = t1.Ticks;
			var t2ticks = t2.Ticks;
			
			if (t1ticks > t2.Ticks)
				return 1;
			if (t1ticks < t2ticks)
				return -1;
			
			return 0;			
		}
	}
}

