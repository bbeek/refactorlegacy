using System;
using System.Collections.Generic;
using TheatricalPlays;

namespace TheatricalPlaysTests.Builders
{
    internal class InvoiceBuilder
    {
        private string customer = "TestCustomer";
        private List<Performance> performances = new List<Performance>();

        internal InvoiceBuilder ForCustomer(string customer)
        {
            this.customer = customer;
            return this;
        }

        internal InvoiceBuilder WithPerformance(string playId, int audience)
        {
            performances.Add(new Performance(playId, audience));

            return this;
        }

        internal Invoice Build()
        {
            return new Invoice(customer, performances);
        }
    }
}