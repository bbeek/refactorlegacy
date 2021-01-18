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
        public string Customer { get; init; }
        public ImmutableList<EnrichedPerformance> Performances { get; init; }
        public decimal TotalAmount => GetTotalAmount();
        public int TotalVolumeCredits => GetTotalVolumeCredits();

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
