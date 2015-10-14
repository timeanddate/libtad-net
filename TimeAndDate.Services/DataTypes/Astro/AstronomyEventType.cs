using System;
namespace TimeAndDate.Services.DataTypes.Astro
{
	[Flags]
	public enum AstronomyEventType
	{
		Rise = 1,
		Set = 1 << 1
	}
}

