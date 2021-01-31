using Day2.Controllers;
using Day2.Repositories;
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
        public TestingPricesController(int hour, IPriceRepository repository) : base(repository)
        {
            this.hour = hour;
        }

        protected override int GetHour()
        {
            return hour;
        }

    }
}
