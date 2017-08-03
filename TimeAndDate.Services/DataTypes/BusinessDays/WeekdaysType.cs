using System;
namespace TimeAndDate.Services.DataTypes.BusinessDays
{
    public class WeekdaysType
    {
        public IncludeOrExcluded FilterType { get; set; }

        public int TotalCount { get; set; }

        public int MondayCount { get; set; }

        public int TuesdayCount { get; set; }

        public int WednesdayCount { get; set; }

        public int ThursdayCount { get; set; }

        public int FridayCount { get; set; }

        public int SaturdayCount { get; set; }

        public int SundayCount { get; set; }
    }
}
