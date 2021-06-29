
using System;
using System.Xml;
using TimeAndDate.Services.Common;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;

namespace TimeAndDate.Services.DataTypes.Time
{
    public class TADDateTime
    {
        /// <value>
        /// Year.
        /// </value>
        public Int32 Year { get; set; }

        /// <value>
        /// Month.
        /// </value>
        public Int32 Month { get; set; }

        /// <value>
        /// Day.
        /// </value>
        public Int32 Day { get; set; }

        /// <value>
        /// Hour.
        /// </value>
        public Int32 Hour { get; set; }

        /// <value>
        /// Minute.
        /// </value>
        public Int32 Minute { get; set; }

        /// <value>
        /// Second.
        /// </value>
        public Int32 Second { get; set; }

        public TADDateTime() { }

        public TADDateTime(Int32 year, int month, int day, int hour, int minute, int second)
        {
            Year = year;
            Month = month;
            Day = day;
            Hour = hour;
            Minute = minute;
            Second = second;
        }
        public TADDateTime(Int32 year, int month, int day)
        {
            Year = year;
            Month = month;
            Day = day;
        }

        public TADDateTime(DateTimeOffset date)
        {
            Year = date.Year;
            Month = date.Month;
            Day = date.Day;
            Hour = date.Hour;
            Minute = date.Minute;
            Second = date.Second;
        }

        public TADDateTime(DateTime date)
        {
            Year = date.Year;
            Month = date.Month;
            Day = date.Day;
            Hour = date.Hour;
            Minute = date.Minute;
            Second = date.Second;
        }

        public DateTimeOffset ToStd()
        {
            return new DateTimeOffset(Year, Month, Day, Hour, Minute, Second, TimeSpan.FromMinutes(0));
        }

        public TADDateTime(string s)
        {
            var strlist = new List<string>();

            if (s.Contains("T"))
            {
                strlist = new List<string>(s.Split("T"));

                foreach (char ch in "+-Z")
                {
                    strlist[1] = strlist[1].Split(ch)[0];
                }
            }
            else
            {
                strlist.Add(s);
            }

            List<int> date_list = strlist[0].Split("-").Select(Int32.Parse).ToList<int>();
            List<int> time_list = strlist[1].Split(":").Select(Int32.Parse).ToList<int>();

            Year = date_list[0];
            Month = date_list[1];
            Day = date_list[2];
            Hour = time_list[0];
            Minute = time_list[1];
            Second = time_list[2];
        }

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }

            TADDateTime t1 = obj as TADDateTime;
            return Year == t1.Year &&
            Month == t1.Month &&
            Day == t1.Day &&
            Hour == t1.Hour &&
            Minute == t1.Minute &&
            Second == t1.Second;
        }

        public static explicit operator TADDateTime(XmlNode node)
        {
            var model = new TADDateTime();
            var year = node.SelectSingleNode("year");
            var month = node.SelectSingleNode("month");
            var day = node.SelectSingleNode("day");
            var hour = node.SelectSingleNode("hour");
            var minute = node.SelectSingleNode("minute");
            var second = node.SelectSingleNode("second");

            if (year != null)
            {
                model.Year = Int32.Parse(year.InnerText);
            }

            if (month != null)
            {
                model.Month = Int32.Parse(month.InnerText);
            }

            if (day != null)
            {
                model.Day = Int32.Parse(day.InnerText);
            }

            if (hour != null)
            {
                model.Hour = Int32.Parse(hour.InnerText);
            }

            if (minute != null)
            {
                model.Minute = Int32.Parse(minute.InnerText);
            }

            if (second != null)
            {
                model.Second = Int32.Parse(second.InnerText);
            }

            return model;
        }
    }
}

