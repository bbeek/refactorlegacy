using Day2.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day2Tests
{
    class TestingPricesController : PricesController
    {
        private readonly int hour;
        private readonly bool holiday;
        private readonly double cost;

        public TestingPricesController(int hour, bool holiday, double cost)
        {
            this.hour = hour;
            this.holiday = holiday;
            this.cost = cost;
        }

        protected override int GetHour()
        {
            return hour;
        }

        protected override Task<bool> IsHoliday(DateTime? skidate)
        {
            return Task.FromResult(holiday);
        }

        protected override Task<double> RetrieveBasePrice(string type)
        {
            return Task.FromResult(cost);
        }
    }
}
