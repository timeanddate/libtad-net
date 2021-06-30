using System;
using System.Collections.Generic;
using System.Xml;

namespace TimeAndDate.Services.DataTypes.OnThisDay
{
	public class OnThisDayResponse
	{
		/// <summary>
		/// Events on this day.
		/// </summary>
		/// <value>
		/// List of requested events.
		/// </value>
		public List<Event> Events { get; set; }

		/// <summary>
		/// Births on this day.
		/// </summary>
		/// <value>
		/// List of requested births.
		/// </value>
		public List<Person> Births { get; set; }

		/// <summary>
		/// Deaths on this day.
		/// </summary>
		/// <value>
		/// List of requested deaths.
		/// </value>
		public List<Person> Deaths { get; set; }

		public OnThisDayResponse ()
		{
			Events = new List<Event> ();
			Births = new List<Person> ();
			Deaths = new List<Person> ();
		}

		public OnThisDayResponse(List<Event> events, List<Person> births, List<Person> deaths)
		{
			Events = events;
			Births = births;
			Deaths = deaths;
		}
	}
}
