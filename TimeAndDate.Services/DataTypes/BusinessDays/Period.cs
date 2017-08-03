using System;
using System.Collections.Generic;
using System.Xml;
using TimeAndDate.Services.DataTypes.Holidays;
using TimeAndDate.Services.DataTypes.Time;

namespace TimeAndDate.Services.DataTypes.BusinessDays
{
	[Flags]
	public enum IncludeOrExcluded
	{
		Excluded,
		Included
	}

    public class Period
    {
		public IncludeOrExcluded IncludedOrExcluded { get; set; }

		public int IncludedDays { get; set; }

        public int CalendarDays { get; set; }

        public int SkippedDays { get; set; }

        public TADTime StartDate { get; set; }

        public TADTime EndDate { get; set; }

        public WeekdaysType Weekdays { get; set; }

		public List<Holiday> Holidays { get; set; }

		public Period() 
        {
            Weekdays = new WeekdaysType();
            Holidays = new List<Holiday>();
        }

        public static explicit operator Period(XmlNode node) 
        {
            var model = new Period();
            var includeddays = node.Attributes["includeddays"]?.Value;
            var calendardays = node.Attributes["calendardays"]?.Value;
            var skippeddays = node.Attributes["skippeddays"]?.Value;
            var startDate = node.SelectSingleNode("startdate");
            var endDate = node.SelectSingleNode("enddate");
            var holidays = node.SelectSingleNode("holidays");
            var weekdays = node.SelectSingleNode("weekdays");
            var holidaysList = holidays?.FirstChild?.ChildNodes;

            if (!String.IsNullOrEmpty(includeddays))
                model.IncludedDays = Int32.Parse(includeddays);

            if (!String.IsNullOrEmpty(calendardays))
                model.CalendarDays = Int32.Parse(calendardays);

            if (!String.IsNullOrEmpty(skippeddays))
                model.SkippedDays = Int32.Parse(skippeddays);

            if (startDate != null)
            {
				model.StartDate = (TADTime)startDate;
				var iso = model.StartDate.ISO;
				model.StartDate.ISO = iso.EndsWith("Z") ? iso.Substring(0, iso.Length - 1) : iso;
            }

            if (endDate != null) 
            {
				model.EndDate = (TADTime)endDate;
                var iso = model.EndDate.ISO;
                model.EndDate.ISO = iso.EndsWith("Z") ? iso.Substring(0, iso.Length - 1) : iso;
			}

            if(weekdays != null) 
            {
                switch(weekdays.Attributes["type"].Value) 
                {
                    case "excluded": model.Weekdays.FilterType = IncludeOrExcluded.Excluded; break;
                    case "included": model.Weekdays.FilterType = IncludeOrExcluded.Included; break;
                    default: break;
				}

                model.Weekdays.TotalCount = Int32.Parse(weekdays.Attributes["count"].Value);

                XmlNode wd = weekdays.FirstChild;
                for (var i = 0; i < weekdays.ChildNodes.Count; i++, wd = wd.NextSibling)
                {
                    switch(wd.Name)
                    {
                        case "mon":
                            model.Weekdays.MondayCount = Int32.Parse(wd.InnerText);
                            break;
						case "tue":
                            model.Weekdays.TuesdayCount = Int32.Parse(wd.InnerText);
							break;
						case "wed":
                            model.Weekdays.WednesdayCount = Int32.Parse(wd.InnerText);
							break;
						case "thu":
                            model.Weekdays.ThursdayCount = Int32.Parse(wd.InnerText);
							break;
						case "fri":
                            model.Weekdays.FridayCount = Int32.Parse(wd.InnerText);
							break;
						case "sat":
                            model.Weekdays.SaturdayCount = Int32.Parse(wd.InnerText);
							break;
						case "sun":
                            model.Weekdays.SundayCount = Int32.Parse(wd.InnerText);
                            break;
                        default:
                            break;
                    }
                }
            }

            if (holidays?.Attributes["type"] != null)
            {
                switch (holidays.Attributes["type"].Value)
                {
                    case "excluded":
                        model.IncludedOrExcluded = IncludeOrExcluded.Excluded;
                        break;
                    case "included":
                        model.IncludedOrExcluded = IncludeOrExcluded.Included;
                        break;
                    default:
                        model.IncludedOrExcluded = IncludeOrExcluded.Excluded;
                        break;
                }
            }

            if(holidaysList != null) 
            {
                foreach(XmlNode holiday in holidaysList) {
                    model.Holidays.Add((Holiday)holiday);
                }
            }

            return model;
        }
    }
}
