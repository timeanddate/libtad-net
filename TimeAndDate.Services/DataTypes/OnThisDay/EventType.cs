using System;
namespace TimeAndDate.Services.DataTypes.OnThisDay
{
	// Simple bitflags so the user can easily put together a set of types
	// which should be included in the retrieval of events.
	[Flags]
	public enum EventType
	{
		Events = 1,
		Births = 1 << 1,
		Deaths = 1 << 2
	}
}
