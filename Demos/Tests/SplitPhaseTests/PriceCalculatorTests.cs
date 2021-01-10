using FluentAssertions;
using NUnit.Framework;
using SplitPhase;
using System;
using System.Collections.Generic;

namespace SplitPhaseTests
{
    public class PriceCalculatorTests
    {
        
        [Test]
        public void CalculatePrice_order_containing_single_whiskey_should_return_expected_price()
        {
            const string order = "-Whiskey 1";
            const decimal expectedPrice = 5.5m;

            var sut = new PriceCalculator();

            var outcome = sut.Calculate(order);
            outcome.Should().Be(expectedPrice);
        }

        [Test]
        public void CalculatePrice_order_containing_multiple_beers_should_return_expected_price()
        {
            const string order = "-Beer 3";
            const decimal expectedPrice = 7.5m;

            var sut = new PriceCalculator();

            var outcome = sut.Calculate(order);
            outcome.Should().Be(expectedPrice);
        }

        [Test]
        public void CalculatePrice_order_containing_unknown_product_should_throw_keynotfound()
        {
            const string order = "-Fanta 3";
            
            var sut = new PriceCalculator();

            Action act = () => sut.Calculate(order);
            act.Should().Throw<KeyNotFoundException>();
        }

        [Test]
        public void CalculatePrice_order_without_spaces_should_throw_keynotfound()
        {
            const string order = "-Beer3";

            var sut = new PriceCalculator();

            Action act = () => sut.Calculate(order);
            act.Should().Throw<KeyNotFoundException>();
        }

        [Test]
        public void CalculatePrice_order_without_markup_should_throw_indexoutofrange()
        {
            const string order = "Beer 3";

            var sut = new PriceCalculator();

            Action act = () => sut.Calculate(order);
            act.Should().Throw<IndexOutOfRangeException>();
        }
    }
}