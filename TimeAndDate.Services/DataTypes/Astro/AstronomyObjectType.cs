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
		Moon = 1 << 1,

		/// <summary>
		/// Mercury.
		/// </summary>
		Mercury = 1 << 2,

		/// <summary>
		/// Venus.
		/// </summary>
		Venus = 1 << 3,

		/// <summary>
		/// Mars.
		/// </summary>
		Mars = 1 << 4,

		/// <summary>
		/// Jupiter.
		/// </summary>
		Jupiter = 1 << 5,

		/// <summary>
		/// Saturn.
		/// </summary>
		Saturn = 1 << 6,

		/// <summary>
		/// Uranus.
		/// </summary>
		Uranus = 1 << 7,

		/// <summary>
		/// Neptune.
		/// </summary>
		Neptune = 1 << 8,

		/// <summary>
		/// Pluto.
		/// </summary>
		Pluto = 1 << 9
	}
}

