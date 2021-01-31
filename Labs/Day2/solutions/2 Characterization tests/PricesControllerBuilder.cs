using Day2.Controllers;

namespace Day2Tests
{
    internal class PricesControllerBuilder
    {
        private const double DAYPRICE = 35d;
        private const double NIGHTPRICE = 19d;

        private bool holiday = true;
        private double cost = DAYPRICE;
        private int hour = 12;

        internal PricesController Build()
        {
            return new TestingPricesController(hour, holiday, cost);
        }

        internal PricesControllerBuilder WithRequestedDateIsNotAHoliday()
        {
            this.holiday = false;

            return this;
        }

        internal PricesControllerBuilder WithDayTicket()
        {
            this.cost = DAYPRICE;

            return this;
        }

        internal PricesControllerBuilder WithNightTicket()
        {
            this.cost = NIGHTPRICE;

            return this;
        }

        internal PricesControllerBuilder WithEarlyBirdDiscountTime()
        {
            hour = 8;

            return this;
        }

        internal PricesControllerBuilder WithEndOfDayDiscountTime()
        {
            hour = 16;

            return this;
        }
    }
}
