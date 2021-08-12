using System;
namespace TimeAndDate.Services.DataTypes.Astro
{
	[Flags]
	public enum AstronomyObjectType
	{
		/// <summary>
		/// The sun.
		/// </summary>
		Sun = 1,
		
		/// <summary>
		/// The moon.
		/// </summary>
		Moon = 1 << 1
	}
}

