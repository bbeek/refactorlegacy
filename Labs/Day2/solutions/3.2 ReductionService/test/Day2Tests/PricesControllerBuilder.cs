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
        private bool isEarlyBird = false;
        private bool isEndOfDay = false;

        internal PricesController Build()
        {
            var stub = new Mock<IPriceRepository>();
            stub.Setup(f => f.IsHoliday(It.IsAny<DateTime?>())).ReturnsAsync(holiday);
            stub.Setup(f => f.RetrieveBasePrice(It.IsAny<string>())).ReturnsAsync(cost);

            var fakeReductionService = new FakeReductionService(stub.Object, isEarlyBird, isEndOfDay);

            return new PricesController(stub.Object, fakeReductionService);
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
            isEarlyBird = true;

            return this;
        }

        internal PricesControllerBuilder WithEndOfDayDiscountTime()
        {
            isEndOfDay = true;

            return this;
        }
    }
}
