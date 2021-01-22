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
            PerformanceCalculator calculator = CreatePerformanceCalculator(performance, PlayFor(performance));

            return new EnrichedPerformance(calculator.Performance, calculator.Play, calculator.GetAmount(), calculator.GetVolumeCredits());
        }

        private PerformanceCalculator CreatePerformanceCalculator(Performance performance, Play play)
        {
            switch (play.Type)
            {
                case PlayType.Comedy: return new ComedyCalculator { Performance = performance, Play = play };
                case PlayType.Tragedy: return new TragedyCalculator { Performance = performance, Play = play };
                default: throw new Exception($"unknown type: {play.Type}");
            }
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

        private Play PlayFor(Performance perf)
        {
            return plays[perf.PlayId];
        }
    }
}
