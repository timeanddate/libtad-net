using System;
namespace TimeAndDate.Services.DataTypes.Astro
{
	[Flags]
	public enum AstronomyEventClass
	{
		/// <summary>
		/// Combination of all known classes.
		/// </summary>
		All = 1,
		/// <summary>
		/// Day length. 
		/// </summary>
		DayLength = 2,
		/// <summary>
		/// Meridian (Noon, highest point) and Anti-Meridian (lowest point) events.
		/// </summary>
		Meridian = 4,
		/// <summary>
		/// Moon phase events. Additionally to the phase events (only occurring on 
		/// four days per lunar month), an additional attribute for the current moon 
		/// phase is reported for every day.
		/// </summary>
		Phase = 8,
		/// <summary>
		/// Set and rise events. Event times take atmospheric refraction into account.
		/// </summary>
		SetRise = 16,
		/// <summary>
		/// Combination of all 3 twilight classes.
		/// </summary>
		AllTwilights = 32,
		/// <summary>
		/// Civil twilight (-6°).
		/// </summary>
		CivilTwilight = 64,
		/// <summary>
		/// Nautical twilight (-12°).
		/// </summary>
		NauticalTwilight = 128,
		/// <summary>
		/// Astronomical twilight (-18°).
		/// </summary>
		AstronomicalTwilight = 256		
	}
}

