using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace TheatricalPlays
{
    public class BillGenerator
    {
        private readonly IReadOnlyDictionary<string, Play> plays;

        public BillGenerator(IReadOnlyDictionary<string, Play> plays)
        {
            this.plays = plays ?? throw new ArgumentNullException(nameof(plays));
        }

        public string Statement(Invoice invoice)
        {
            var result = new StringBuilder().AppendLine($"Statement for {invoice.Customer}");

            foreach (var perf in invoice.Performances)
            {
                // print line for this order
                result.Append($"  {PlayFor(perf).Name}: {Usd(AmountFor(perf))}");
                result.AppendLine($" ({perf.Audience} seats)");
            }

            result.AppendLine($"Amount owed is {Usd(GetTotalAmount(invoice))}");
            result.Append($"You earned {GetTotalVolumeCredits(invoice)} credits");
            return result.ToString();
        }

        private string Usd(decimal amount)
        {
            return (amount / 100).ToString("C", new CultureInfo("en-US"));
        }

        private decimal GetTotalAmount(Invoice invoice)
        {
            decimal totalAmount = 0;
            foreach (var perf in invoice.Performances)
            {
                totalAmount += AmountFor(perf);
            }

            var discountPercentage = Math.Min(30, (GetTotalVolumeCredits(invoice) / 10));
            totalAmount -= totalAmount * (discountPercentage / 100m);
            return totalAmount;
        }

        private int GetTotalVolumeCredits(Invoice invoice)
        {
            var volumeCredits = 0;
            foreach (var perf in invoice.Performances)
            {
                volumeCredits += VolumeCreditsFor(perf);
            }

            return volumeCredits;
        }

        private int VolumeCreditsFor(Performance performance)
        {
            var result = 0;
            result += Math.Max(performance.Audience - 30, 0);
            if (PlayType.Comedy == PlayFor(performance).Type) result += (int)Math.Floor((decimal)performance.Audience / 5);

            return result;
        }

        private Play PlayFor(Performance perf)
        {
            return plays[perf.PlayId];
        }

        private int AmountFor(Performance performance)
        {
            int result;

            switch (PlayFor(performance).Type)
            {
                case PlayType.Tragedy:
                    result = 40000;
                    if (performance.Audience > 30) result += 1000 * (performance.Audience - 30);

                    break;

                case PlayType.Comedy:
                    result = 30000;
                    if (performance.Audience > 20) result += 10000 + 500 * (performance.Audience - 20);
                    result += 300 * performance.Audience;
                    break;

                default:
                    throw new Exception($"unknown type: {PlayFor(performance).Type}");
            }

            return result;
        }
    }
}
