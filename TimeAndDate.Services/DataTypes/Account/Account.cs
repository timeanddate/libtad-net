using System;
using System.Collections.Generic;
using System.Xml;
using TimeAndDate.Services.DataTypes.Time;

namespace TimeAndDate.Services.DataTypes.Account
{
	public class Account
	{
		/// <summary>
		/// Padded 8-digit account ID.
		/// </summary>
		/// <value>
		/// The account ID..
		/// </value>
		public string Id { get; set; }

		/// <summary>
		/// Timestamp of the request.
		/// </summary>
		/// <value>
		/// The timestamp.
		/// </value>
		public TADDateTime Timestamp { get; set; }

		/// <summary>
		/// Remaining request credits on the request account.
		/// </summary>
		/// <value>
		/// The request credits.
		/// </value>
		public int Credits { get; set; }

		/// <summary>
		/// Active packages owned by the request account.
		/// </summary>
		/// <value>
		/// The packages.
		/// </value>
		public List<Package> Packages { get; set; }

		public Account ()
		{
			Packages = new List<Package> ();
		}

		public Account (XmlNode node)
		{
			Packages = new List<Package> ();

			var id = node.Attributes ["id"];
			var timestamp = node.SelectSingleNode ("timestamp");
			var credits = node.SelectSingleNode ("requests");
			var packages = node.SelectNodes ("package");

			if (id != null)
				Id = id.InnerText;

			if (timestamp != null)
			{
				var iso = timestamp.Attributes["iso"];
				if (iso != null)
					Timestamp = new TADDateTime (iso.InnerText);
			}

			if (credits != null)
				Credits = Int32.Parse(credits.InnerText);

			if (packages != null)
			{
				foreach (XmlNode package in packages)
					Packages.Add ((Package) package);
			}
		}
	}
}
