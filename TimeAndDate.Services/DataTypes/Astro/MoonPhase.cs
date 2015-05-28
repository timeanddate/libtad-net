using System;
using System.Collections.Generic;

namespace TimeAndDate.Services.DataTypes.Astro
{
	public enum MoonPhase
	{
		/// <summary>
		/// The not requested moon phases.
		/// </summary>
		NotRequested,
		
		/// <summary>
		/// New moon.
		/// </summary>
		NewMoon,
		
		/// <summary>
		/// Waxing crescent moon.
		/// </summary>
		WaxingCrescent,
		
		/// <summary>
		/// Moon in first quarter.
		/// </summary>
		FirstQuarter,
		
		/// <summary>
		/// Waxing gibbous moon.
		/// </summary>
		WaxingGibbous,
		
		/// <summary>
		/// Full moon.
		/// </summary>
		FullMoon,
		
		/// <summary>
		/// Waning gibbous moon.
		/// </summary>
		WaningGibbous,
		
		/// <summary>
		/// Moon in third quarter.
		/// </summary>
		ThirdQuarter,
		
		/// <summary>
		/// Waning crescent moon.
		/// </summary>
		WaningCrescent
	}
}

