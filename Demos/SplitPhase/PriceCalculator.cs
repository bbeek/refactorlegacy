using System.Collections.Generic;

namespace SplitPhase
{
    public class PriceCalculator
    {
        private readonly Dictionary<string, decimal> priceList = new Dictionary<string, decimal>
            {
                { "Whiskey", 5.5m },
                { "Beer", 2.5m },
                { "Wine", 4.0m },
                { "Cola", 2.0m }
            };

/*
 * Extract second method
 * Test
 * create intermediate data structure (+ add to second method)
 * for each parameter, move to intermediate data structure.
 * extract first method
 */

        public decimal Calculate(string order)
        {
            var orderData = order.Split(" ");
            var productPrice = priceList[orderData[0].Split("-")[1]];
            var orderPrice = int.Parse(orderData[1]) * productPrice;
            return orderPrice;
        }
    }

}