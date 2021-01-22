using System;

namespace ReplaceGlobalReference
{
    public class MarketingCampaign
    {
        public bool IsActive()
        {
            return MilliSeconds() % 2 == 0;
        }

        private long MilliSeconds()
        {
            return (long)GetDateTime().TimeOfDay.TotalMilliseconds;
        }

        public bool IsCrazySalesDay()
        {
            return DayOfTheWeek().Equals(DayOfWeek.Friday); ;
        }

        private DayOfWeek DayOfTheWeek()
        {
            return GetDateTime().DayOfWeek;
        }

        protected virtual DateTime GetDateTime()
        {
            return DateTime.Now;
        }
    }
}
