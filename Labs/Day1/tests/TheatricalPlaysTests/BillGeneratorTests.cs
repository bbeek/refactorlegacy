using FluentAssertions;
using System;
using TheatricalPlays;
using TheatricalPlaysTests.Builders;
using Xunit;

namespace TheatricalPlaysTests
{
    public class BillGeneratorTests
    {

        [Fact]
        public void RenderPlainTextTest()
        {
            var plays = new PlaysBuilder().Build();


            var invoice = new InvoiceBuilder()
                                .ForCustomer("BigCo")
                                .WithPerformance("hamlet", 55)
                                .WithPerformance("as-like", 35)
                                .WithPerformance("othello", 40)
                                .Build();

            var generator = new BillGenerator();

            var statement = generator.Statement(invoice, plays);


            statement.Should().Be(@"Statement for BigCo
  Hamlet: $650.00 (55 seats)
  As You Like It: $580.00 (35 seats)
  Othello: $500.00 (40 seats)
Amount owed is $1,660.80
You earned 47 credits");
        }

        [Fact]
        public void Statement_unknown_playtype_should_throw_exception()
        {
            const string invalidPlayId = "invalid";
            var plays = new PlaysBuilder()
                                .WithPlay(invalidPlayId, (PlayType)99)
                                .Build();
            var invoice = new InvoiceBuilder()
                                .WithPerformance(invalidPlayId, 1)
                                .Build();
            var generator = new BillGenerator();

            Action act = () => generator.Statement(invoice, plays);

            act.Should().Throw<Exception>().WithMessage("unknown type: 99");
        }
        
    }
}
