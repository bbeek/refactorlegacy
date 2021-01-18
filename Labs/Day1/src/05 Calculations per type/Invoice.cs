using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace TheatricalPlays
{
    public class Invoice
    {
        public string Customer { get; }
        public ImmutableList<Performance> Performances { get; }

        public Invoice(string customer, IEnumerable<Performance> performances)
        {
            Customer = customer ?? throw new ArgumentNullException(nameof(customer));
            Performances = performances?.ToImmutableList() ?? throw new ArgumentNullException(nameof(performances));
        }
    }

}