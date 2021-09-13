using System;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using TimeAndDate.Services.Common;
using TimeAndDate.Services.DataTypes.Tides;
using TimeAndDate.Services.DataTypes.Places;
using TimeAndDate.Services.DataTypes.Time;
using System.Xml;

namespace TimeAndDate.Services
{
    public class TidesService : BaseService
    {
        /// <summary>
        /// Whether to return every point per interval, or just the highest and lowest points.
        /// </summary>
        public bool? OnlyHighLow { get; set; }

        /// <summary>
        /// Start of the requested time interval.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// End of the requested time interval.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Search for tidal stations within the radius from the requested place.
        /// </summary>
        public int? Radius { get; set; }

        /// <summary>
        /// Whether or not to resolve subordinate or just reference stations.
        /// </summary>
        public bool? Subordinate { get; set; }

        /// <summary>
        /// How many minutes between each data point. Supported values: 5 min, 15 min, 30 min, 60 min.
        /// </summary>
        public int? Interval { get; set; }

        /// <summary>
        /// Whether input and output timestamps should be resolved to local time or not.
        /// </summary>
        public bool? LocalTime { get; set; }

        /// <summary>
        /// </summary>
        /// <param name='accessKey'>
        /// Access key.
        /// </param>
        /// <param name='secretKey'>
        /// Secret key.
        /// </param>
        public TidesService(string accessKey, string secretKey) : base(accessKey, secretKey, "tides")
        {
            XmlElemName = "station";
        }

        /// <summary>
        /// The Tides service can be used to retrieve predicted tidal data over a given time interval for one or multiple places.
        /// </summary>
        /// <returns>
        /// List of <c>Station<c> containing requested information.
        /// </returns>
        /// <param name='placeid'>
        /// The location for which the tidal data should be predicted for.
        /// </param>
        public IList<Station> GetTidalData(LocationId placeid)
        {
            var args = GetArguments(new List<LocationId> { placeid });
            return CallService(args, x => (Station)x);
        }

        /// <summary>
        /// The Tides service can be used to retrieve predicted tidal data over a given time interval for one or multiple places.
        /// </summary>
        /// <returns>
        /// List of <c>Station<c> containing requested information.
        /// </returns>
        /// <param name='placeid'>
        /// The locations for which the tidal data should be predicted for.
        /// </param>
        public IList<Station> GetTidalData(List<LocationId> placeid)
        {
            var args = GetArguments(placeid);
            return CallService(args, x => (Station)x);
        }

        /// <summary>
        /// The Tides service can be used to retrieve predicted tidal data over a given time interval for one or multiple places.
        /// </summary>
        /// <returns>
        /// List of <c>Station<c> containing requested information.
        /// </returns>
        /// <param name='placeid'>
        /// The location for which the tidal data should be predicted for.
        /// </param>
        public async Task<IList<Station>> GetTidalDataAsync(LocationId placeid)
        {
            var args = GetArguments(new List<LocationId> { placeid });
            return await CallServiceAsync(args, x => (Station)x);
        }

        /// <summary>
        /// The Tides service can be used to retrieve predicted tidal data over a given time interval for one or multiple places.
        /// </summary>
        /// <returns>
        /// List of <c>Station<c> containing requested information.
        /// </returns>
        /// <param name='placeid'>
        /// The location for which the tidal data should be predicted for.
        /// </param>
	public async Task<IList<Station>> GetTidalDataAsync(List<LocationId> placeid)
	{
		var args = GetArguments(placeid);
		return await CallServiceAsync(args, x => (Station)x);
	}

        private NameValueCollection GetArguments(IList<LocationId> placeid)
        {
            var args = new NameValueCollection();

            args.Set("placeid", string.Join(",", placeid.Select(x => x.GetIdAsString())));

            if (OnlyHighLow is bool onlyHighLow)
                args.Set("onlyhighlow", onlyHighLow.ToNum());

            if (StartDate is DateTime startDate)
                args.Set("startdt", startDate.ToString("s", CultureInfo.InvariantCulture));

            if (EndDate is DateTime endDate)
                args.Set("enddt", endDate.ToString("s", CultureInfo.InvariantCulture));

            if (Radius is int radius)
                args.Set("radius", radius.ToString());

            if (Subordinate is bool subordinate)
                args.Set("subordinate", subordinate.ToNum());

            if (Interval is int interval)
                args.Set("interval", interval.ToString());

            if (LocalTime is bool localTime)
                args.Set("localtime", localTime.ToNum());

            return args;
        }
    }
}
