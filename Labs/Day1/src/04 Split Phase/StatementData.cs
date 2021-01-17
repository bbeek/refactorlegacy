using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheatricalPlays
{
    class StatementData
    {
        public string Customer { get; }
        public ImmutableList<EnrichedPerformance> Performances { get; }
        public decimal TotalAmount => GetTotalAmount();
        public int TotalVolumeCredits => GetTotalVolumeCredits();

        public StatementData(string customer, ImmutableList<EnrichedPerformance> performances)
        {
            Customer = customer;
            Performances = performances;
        }

        private decimal GetTotalAmount()
        {
            decimal totalAmount = this.Performances.Sum(perf => perf.Amount);

            var discountPercentage = Math.Min(30, (this.TotalVolumeCredits / 10));
            totalAmount -= totalAmount * (discountPercentage / 100m);
            return totalAmount;
        }

        private int GetTotalVolumeCredits()
        {
            return this.Performances.Sum(perf => perf.VolumeCredits);
        }
    }
}
