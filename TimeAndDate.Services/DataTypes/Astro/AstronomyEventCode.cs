using System;
using System.Collections.Generic;

namespace TimeAndDate.Services.DataTypes.Astro
{
	public enum AstronomyEventCode
	{
		/// <summary>
		/// Astronomical twilight (-18°) start.
		/// </summary>
		AstronomicalTwilightStarts,
		
		/// <summary>
		/// Nautical twilight (-12°) start.
		/// </summary>
		NauticalTwilightStarts,
		
		/// <summary>
		/// Civil twilight (-6°) start.
		/// </summary>
		CivilTwilightStarts,
		
		/// <summary>
		/// Sun- or Moonrise.
		/// </summary>
		Rise,
		
		/// <summary>
		/// Meridian passing (noon).
		/// </summary>
		Meridian,
		
		/// <summary>
		/// Antimeridian passing (midnight).
		/// </summary>
		AntiMeridian,
		
		/// <summary>
		/// Sun- or Moonset.
		/// </summary>
		Set,
		
		/// <summary>
		/// Civil twilight (-6°) end.
		/// </summary>
		CivilTwilightEnds,
		
		/// <summary>
		/// Nautical twilight (-12°) end.
		/// </summary>
		NauticalTwilightEnds,
		
		/// <summary>
		/// Astronomical twilight (-18°) end.
		/// </summary>
		AstronomicalTwilightEnds,
		
		/// <summary>
		/// New moon.
		/// </summary>
		NewMoon,
		
		/// <summary>
		/// Moon in first quarter.
		/// </summary>
		FirstQuarter,
		
		/// <summary>
		/// Full moon.
		/// </summary>
		FullMoon,
		
		/// <summary>
		/// Moon in third quarter.
		/// </summary>
		ThirdQuarter
	}

}