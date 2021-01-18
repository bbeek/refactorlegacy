using System;
using System.Collections.Immutable;
using System.Linq;

namespace TheatricalPlays
{
    class StatementData
    {
        public string Customer { get; init; }
        public ImmutableList<PerformanceCalculator> Performances { get; init; }
        public decimal TotalAmount => GetTotalAmount();
        public int TotalVolumeCredits => Performances.Sum(perf => perf.VolumeCredits);

        private decimal GetTotalAmount()
        {
            decimal totalAmount = this.Performances.Sum(perf => perf.Amount);

            return totalAmount - GetDiscount(totalAmount);
        }

        private decimal GetDiscount(decimal totalAmount)
        {
            var discountPercentage = Math.Min(30, (this.TotalVolumeCredits / 10));
            return totalAmount * (discountPercentage / 100m);
        }
    }
}
