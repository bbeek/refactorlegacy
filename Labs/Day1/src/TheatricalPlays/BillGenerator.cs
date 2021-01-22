using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace TheatricalPlays
{
    public class BillGenerator
    {
        public string Statement(Invoice invoice, IReadOnlyDictionary<string, Play> plays)
        {
            decimal totalAmount = 0;
            var volumeCredits = 0;

            IFormatProvider format = new CultureInfo("en-US");

            var result = new StringBuilder().AppendLine($"Statement for {invoice.Customer}");

            foreach (var perf in invoice.Performances)
            {
                var play = plays[perf.PlayId];
                int thisAmount;

                switch (play.Type)
                {
                    case PlayType.Tragedy:
                        thisAmount = 40000;
                        if (perf.Audience > 30)
                        {
                            thisAmount += 1000 * (perf.Audience - 30);
                        }
                        break;

                    case PlayType.Comedy:
                        thisAmount = 30000;
                        if (perf.Audience > 20)
                        {
                            thisAmount += 10000 + 500 * (perf.Audience - 20);
                        }
                        thisAmount += 300 * perf.Audience;
                        break;

                    default:
                        throw new Exception($"unknown type: {play.Type}");
                }

                // add volume credits
                volumeCredits += Math.Max(perf.Audience - 30, 0);
                // add extra credit for every ten comedy attendees
                if (PlayType.Comedy == play.Type) volumeCredits += (int)Math.Floor((decimal)perf.Audience / 5);

                // print line for this order
                result.Append($"  {play.Name}: {(thisAmount / 100).ToString("C", format)}");
                result.AppendLine($" ({perf.Audience} seats)");
                totalAmount += thisAmount;
            }

            var discountPercentage = Math.Min(30, (volumeCredits / 10));
            totalAmount -= (totalAmount * (discountPercentage/100m));

            result.AppendLine($"Amount owed is {(totalAmount / 100).ToString("C", format)}");
            result.Append($"You earned {volumeCredits} credits");
            return result.ToString();
        }
    }
}
