using CommonObjects;
using System;

namespace ExtractInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            var checkout = new Checkout(new ReceiptRepository());

            foreach(var line in checkout.CreateReceipt(new Money(12)).Format())
            {
                Console.WriteLine(line);
            }
        }
    }
}
