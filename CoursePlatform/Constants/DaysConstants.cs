using System;

namespace CoursesPlatform.Services
{
    public class DaysConstants
    {
        private DateTime date;

        public DaysConstants(DateTime startDate)
        {
            date = startDate;
        }

        public DateTime OneDayDifference
        {
            get
            {
                return date.AddHours(-16);
            }
        }

        public DateTime SevenDaysDifference
        {
            get
            {
                return date.AddDays(-SevenDays);
            }
        }

        public DateTime ThirtyDaysDifference
        {
            get
            {
                return date.AddDays(-ThirtyDays);
            }
        }

        public int OneDay = 1;

        public int SevenDays = 7;

        public int ThirtyDays = 30;

        public string Day = "day";

        public string Week = "week";

        public string Month = "month";
    }
}
