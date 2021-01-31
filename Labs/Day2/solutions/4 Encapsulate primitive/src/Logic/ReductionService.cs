using Day2.Repositories;
using System;
using System.Threading.Tasks;

namespace Day2.Logic
{
    public class ReductionService
    {
        private readonly IPriceRepository repository;

        public ReductionService(IPriceRepository repository)
        {
            this.repository = repository;
        }

        public async Task<int> FindPotentialReduction(int? age, DateTime? skiDate)
        {
            int reduction = 0;
            bool isHoliday = await repository.IsHoliday(skiDate);

            if (skiDate.HasValue)
            {

                if (!isHoliday && (int)skiDate.Value.DayOfWeek == 1)
                {
                    reduction = 35;
                }
            }

            if (age == null || age <= 64)
            {
                if (IsInEndOfDayPeriod())
                {
                    reduction += 5;
                }
            }
            else
            {
                if (IsInEarlyBirdPeriod())
                {
                    reduction += 15;
                }
            }

            return reduction;
        }

        protected virtual bool IsInEarlyBirdPeriod()
        {
            return DateTime.Now.Hour < 9;
        }

        protected virtual bool IsInEndOfDayPeriod()
        {
            return DateTime.Now.Hour > 15;
        }
    }
}
