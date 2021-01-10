using System;

namespace SplitPhase
{
    class Program
    {
        static void Main(string[] args)
        {
            const string order = "-Whiskey 3";

            var totalAmount = new PriceCalculator().Calculate(order);
            Console.WriteLine("Your total amount is {0}", totalAmount);
        }
    }
}
