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
            return RenderPlainText(CreateStatementData(invoice));
        }

        public string HtmlStatement(Invoice invoice)
        {
            return RenderHtml(CreateStatementData(invoice));
        }

        private string RenderHtml(StatementData data)
        {
            var result = new StringBuilder().AppendLine($"<h1>Statement for {data.Customer}</h1>");
            result.AppendLine("<table>");
            result.Append("<tr><th>play</th><th>seats</th><th>cost</th></tr>");
            foreach (var perf in data.Performances)
            {
                result.Append($"  <tr><td>{perf.Play.Name}</td><td>{perf.Audience}</td>");
                result.AppendLine($"<td>{Usd(perf.Amount)}</td></tr>");
            }
            result.AppendLine("</table>");
            result.AppendLine($"<p>Amount owed is <em>{Usd(data.TotalAmount)}</em></p>");
            result.AppendLine($"<p>You earned <em>{data.TotalVolumeCredits}</em> credits</p>");
            return result.ToString();
        }

        private StatementData CreateStatementData(Invoice invoice)
        {
            return new StatementData { Customer = invoice.Customer, Performances = invoice.Performances.ConvertAll(EnrichPerformance) };
        }

        private EnrichedPerformance EnrichPerformance(Performance performance)
        {
            return new EnrichedPerformance(performance, PlaysFor(performance), AmountFor(performance), VolumeCreditsFor(performance));
        }

        private string RenderPlainText(StatementData data)
        {
            var result = new StringBuilder().AppendLine($"Statement for {data.Customer}");

            foreach (var perf in data.Performances)
            {
                // print line for this order
                result.Append($"  {perf.Play.Name}: {Usd(perf.Amount)}");
                result.AppendLine($" ({perf.Audience} seats)");
            }

            result.AppendLine($"Amount owed is {Usd(data.TotalAmount)}");
            result.Append($"You earned {data.TotalVolumeCredits} credits");
            return result.ToString();
        }

        private string Usd(decimal amount)
        {
            return (amount / 100).ToString("C", new CultureInfo("en-US"));
        }

        private Play PlaysFor(Performance perf)
        {
            return plays[perf.PlayId];
        }

        private int VolumeCreditsFor(Performance performance)
        {
            var result = 0;
            result += Math.Max(performance.Audience - 30, 0);
            if (PlayType.Comedy == PlaysFor(performance).Type) result += (int)Math.Floor((decimal)performance.Audience / 5);

            return result;
        }

        private int AmountFor(Performance performance)
        {
            int result;

            switch (PlaysFor(performance).Type)
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
                    throw new Exception($"unknown type: {PlaysFor(performance).Type}");
            }

            return result;
        }
    }
}
