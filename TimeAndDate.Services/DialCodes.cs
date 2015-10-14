using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Xml;
using TimeAndDate.Services.DataTypes.DialCode;
using TimeAndDate.Services.DataTypes.Places;

namespace TimeAndDate.Services
{
	public class DialCodes
	{
		public string Number { get; set; }
		
		public IList<Composition> Compositions { get; set; }
		
		public IList<Location> Locations { get; set; }
		
		public DialCodes ()
		{
			Compositions = new List<Composition> ();
			Locations = new List<Location> ();
		}

		public DialCodes (List<Composition> comp, List<Location> locs, string num)
		{
			Compositions = comp;
			Locations = locs;
			Number = num;
		}
	}

}

