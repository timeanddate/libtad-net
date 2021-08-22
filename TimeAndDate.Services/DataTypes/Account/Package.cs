using System;
using System.Xml;
using TimeAndDate.Services.DataTypes.Time;

namespace TimeAndDate.Services.DataTypes.Account
{
	public class Package
	{
		/// <summary>
		/// Padded 4-digit product ID.
		/// </summary>
		/// <value>
		/// The product ID.
		/// </value>
		public string Id { get; set; }

		/// <summary>
		/// Name of the package.
		/// </summary>
		/// <value>
		/// Package name.
		/// </value>
		public string Name { get; set; }

		/// <summary>
		/// Purchase date of the package.
		/// </summary>
		/// <value>
		/// The date.
		/// </value>
		public TADDateTime PurchaseDate { get; set; }

		/// <summary>
		/// Expiration date of the package.
		/// </summary>
		/// <value>
		/// The date.
		/// </value>
		public TADDateTime ExpirationDate { get; set; }

		/// <summary>
		/// License parameters for the package.
		/// </summary>
		/// <value>
		/// The parameter.
		/// </value>
		public string Parameters { get; set; }

		public static explicit operator Package (XmlNode node)
		{
			var model = new Package ();
			var id = node.Attributes["productid"];
			var name = node.SelectSingleNode ("name");
			var purchaseDate = node.SelectSingleNode ("purchasedate");
			var expirationDate = node.SelectSingleNode ("validuntil");
			var parameters = node.SelectSingleNode ("parameters");

			if (id != null)
				model.Id = id.InnerText;

			if (name != null)
				model.Name = name.InnerText;

			if (purchaseDate != null)
			{
				var iso = purchaseDate.Attributes["iso"];
				if (iso != null)
					Console.WriteLine("\n" + iso.InnerText);
					model.PurchaseDate = new TADDateTime (iso.InnerText);
			}

			if (expirationDate != null)
			{
				var iso = expirationDate.Attributes["iso"];
				if (iso != null)
					model.ExpirationDate = new TADDateTime (iso.InnerText);
			}

			if (parameters != null)
				model.Parameters = parameters.InnerText;

			return model;
		}
	}
}
