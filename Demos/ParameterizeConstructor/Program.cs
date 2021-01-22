using System;

namespace ParameterizeConstructor
{
    class Program
    {
        static void Main(string[] args)
        {
            var flakyDiscount = new Discount();
            var basePrice = new Money(101);

            Console.WriteLine("Crazy discounts");
            Console.WriteLine($"Base price {basePrice}, now with discount {flakyDiscount.DiscountFor(basePrice)}");
            Console.WriteLine($"Base price {basePrice}, now with discount {flakyDiscount.DiscountFor(basePrice)}");
            Console.WriteLine($"Base price {basePrice}, now with discount {flakyDiscount.DiscountFor(basePrice)}");
            Console.WriteLine($"Base price {basePrice}, now with discount {flakyDiscount.DiscountFor(basePrice)}");
            Console.WriteLine($"Base price {basePrice}, now with discount {flakyDiscount.DiscountFor(basePrice)}");
        }
    }
}
