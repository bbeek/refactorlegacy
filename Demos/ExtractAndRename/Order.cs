using System;

namespace ExtractAndRename
{
    public class Order
    {
        private readonly int quantity;
        private readonly decimal itemPrice;
        private readonly string customerName;

        public Order(string customerName, int quantity, decimal itemPrice)
        {
            this.quantity = quantity;
            this.itemPrice = itemPrice;
            this.customerName = customerName;
        }

        /*
         * Extract variable
         * Extract function
         * Rename variable
         * Rename function
         * Rename class
         */
        public string PrintOwning()
        {
            var banner = PrintBanner();

            // base price - discount + shipping
            var outstanding = quantity * itemPrice -
                Math.Max(0m, quantity - 500) * itemPrice * 0.05m +
                Math.Min(quantity * itemPrice * 0.1m, 100);

            return banner + @$"Name: {customerName}
Amount: {outstanding}";
        }

        private static string PrintBanner()
        {
            return @"
****************************************
******      Order receipt           ****
****************************************
";
        }
    }
}
