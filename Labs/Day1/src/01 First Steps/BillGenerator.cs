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
            decimal totalAmount = 0;
            var volumeCredits = 0;

            IFormatProvider format = new CultureInfo("en-US");

            var result = new StringBuilder().AppendLine($"Statement for {invoice.Customer}");

            foreach (var perf in invoice.Performances)
            {
                // add volume credits
                volumeCredits += Math.Max(perf.Audience - 30, 0);
                // add extra credit for every ten comedy attendees
                if (PlayType.Comedy == PlayFor(perf).Type) volumeCredits += (int)Math.Floor((decimal)perf.Audience / 5);

                // print line for this order
                result.Append($"  {this.PlayFor(perf).Name}: {(AmountFor(perf) / 100).ToString("C", format)}");
                result.AppendLine($" ({perf.Audience} seats)");
                totalAmount += AmountFor(perf);
            }

            var discountPercentage = Math.Min(30, (volumeCredits / 10));
            totalAmount -= totalAmount * (discountPercentage/100m);
            result.AppendLine($"Amount owed is {(totalAmount / 100).ToString("C", format)}");
            result.Append($"You earned {volumeCredits} credits");
            return result.ToString();
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
                    if (performance.Audience > 30) 
					{
						result += 1000 * (performance.Audience - 30);
					}
                    break;

                case PlayType.Comedy:
                    result = 30000;
                    if (performance.Audience > 20) 
					{
						result += 10000 + 500 * (performance.Audience - 20);
                    }
					result += 300 * performance.Audience;
                    break;

                default:
                    throw new Exception($"unknown type: {PlayFor(performance).Type}");
            }

            return result;
        }
    }
}
