using System;
using NUnit.Framework;
using System.Collections.Generic;
using TimeAndDate.Services.DataTypes.Astro;


namespace TimeAndDate.Services.Tests.UnitTests
{
	[TestFixture()]
	public class AstronomyTests
	{
		[Test]
		public void Astronomy_HasSeveralSunsets_Should_ReturnCorrectSunset ()
		{
			// Arrange		
			var astro = new Astronomy ();
			var events = new List<AstronomyEvent> ();
							
			var sunrise = new AstronomyEvent
			{
				Type = AstronomyEventType.Rise,
				Time = new DateTime().AddHours (1).AddMinutes(58)
			};
			
			var firstSunset = new AstronomyEvent
			{
				Type = AstronomyEventType.Set,
				Time = new DateTime().AddHours (0).AddMinutes(3)
			};
			
			var secondSunset = new AstronomyEvent
			{
				Type = AstronomyEventType.Set,
				Time = new DateTime().AddHours (23).AddMinutes(40)
			};
			
			events.Add (sunrise);			
			events.Add (firstSunset);			
			events.Add (secondSunset);
			
			astro.Events = events;
			
			
			// Act
			var rise = astro.Sunset;
			
			// Assert
			Assert.AreEqual (secondSunset.Time.Hour, rise.Value.Hour);
			Assert.AreEqual (secondSunset.Time.Minute, rise.Value.Minute);
		}		
		
		[Test]
		public void Astronomy_HasOneSunsets_Should_ReturnCorrectSunset ()
		{
			// Arrange		
			var astro = new Astronomy ();
			var events = new List<AstronomyEvent> ();
							
			var sunrise = new AstronomyEvent
			{
				Type = AstronomyEventType.Rise,
				Time = new DateTime().AddHours (1).AddMinutes(58)
			};
			
			var sunset = new AstronomyEvent
			{
				Type = AstronomyEventType.Set,
				Time = new DateTime().AddHours (0).AddMinutes(3)
			};		
			
			events.Add (sunrise);			
			events.Add (sunset);			
			
			astro.Events = events;
						
			// Act
			var set = astro.Sunset;
			
			// Assert
			Assert.AreEqual (sunset.Time.Hour, set.Value.Hour);
			Assert.AreEqual (sunset.Time.Minute, set.Value.Minute);
		}
		
		[Test]
		public void Astronomy_HasOneSunsetsNoSunrise_Should_ReturnCorrectSunset ()
		{
			// Arrange		
			var astro = new Astronomy ();
			var events = new List<AstronomyEvent> ();
			
			var sunset = new AstronomyEvent
			{
				Type = AstronomyEventType.Set,
				Time = new DateTime().AddHours (0).AddMinutes(3)
			};
						
			events.Add (sunset);			
			
			astro.Events = events;
			
			
			// Act
			var set = astro.Sunset;
			
			// Assert
			Assert.AreEqual (sunset.Time.Hour, set.Value.Hour);
			Assert.AreEqual (sunset.Time.Minute, set.Value.Minute);
		}		
	}
}

