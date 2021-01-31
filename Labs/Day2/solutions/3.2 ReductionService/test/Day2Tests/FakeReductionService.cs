using Day2.Logic;
using Day2.Repositories;

namespace Day2Tests
{
    internal class FakeReductionService : ReductionService
    {
        private readonly bool earlyBird;
        private readonly bool endOfDay;
        public FakeReductionService(IPriceRepository repository, bool earlyBird, bool endOfDay) : base(repository)
        {
            this.earlyBird = earlyBird;
            this.endOfDay = endOfDay;
        }

        protected override bool IsInEarlyBirdPeriod()
        {
            return earlyBird;
        }

        protected override bool IsInEndOfDayPeriod()
        {
            return endOfDay;
        }
    }
}
