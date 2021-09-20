using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using TimeAndDate.Services.Common;
using TimeAndDate.Services.DataTypes.Places;
using TimeAndDate.Services.DataTypes.Time;

namespace TimeAndDate.Services.DataTypes.Tides
{
    public class StationInfo
    {
        /// <summary>
        /// Station name.
        /// </summary>
        /// <value>
        /// The name of the station.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Latitude coordinate of the station.
        /// </summary>
        /// <value>
        /// The coordinate value.
        /// </value>
        public float Latitude { get; set; }

        /// <summary>
        /// Longitude coordinate of the station.
        /// </summary>
        /// <value>
        /// The coordinate value.
        /// </value>
        public float Longitude { get; set; }

        /// <summary>
        /// Station type.
        /// </summary>
        /// <value>
        /// Either reference or subordinate station.
        /// </value>
        public string Type { get; set; }

        /// <summary>
        /// Distance between request place and this station.
        /// </summary>
        /// <value>
        /// The distance in kilometers.
        /// </value>
        public float Distance { get; set; }

        public static explicit operator StationInfo(XmlNode node)
        {
            var model = new StationInfo();
            var name = node.SelectSingleNode("name");
            var latitude = node.SelectSingleNode("latitude");
            var longitude = node.SelectSingleNode("longitude");
            var type = node.SelectSingleNode("type");
            var distance = node.SelectSingleNode("distance");

            if (name != null)
                model.Name = name.InnerText;

            if (latitude != null)
                model.Latitude = float.Parse(latitude.InnerText, CultureInfo.InvariantCulture);

            if (longitude != null)
                model.Longitude = float.Parse(longitude.InnerText, CultureInfo.InvariantCulture);

            if (type != null)
                model.Type = type.InnerText;

            if (distance != null)
                model.Distance = float.Parse(distance.InnerText, CultureInfo.InvariantCulture);

            return model;
        }
    }
}
