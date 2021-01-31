using Day2.Controllers;
using Day2.Repositories;
using Moq;
using System;

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
            var stub = new Mock<IPriceRepository>();
            stub.Setup(f => f.IsHoliday(It.IsAny<DateTime?>())).ReturnsAsync(holiday);
            stub.Setup(f => f.RetrieveBasePrice(It.IsAny<string>())).ReturnsAsync(cost);
            return new TestingPricesController(hour, stub.Object);
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
