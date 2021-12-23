using System;

namespace CoursesPlatform.Services
{
    public class Days
    {
        private DateTime date;
        public Days(DateTime startDate)
        {
            this.date = startDate;
        }
        public DateTime OneDays
        {
            get
            {
                return date.AddDays(-1);
            }
        }
        public DateTime SevenDay
        {
            get
            {
                return date.AddDays(-7);
            }
        }
        public DateTime ThirtyDays
        {
            get
            {
                return date.AddDays(-30);
            }
        }
    }
}
