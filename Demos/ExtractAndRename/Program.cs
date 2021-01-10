using System;

namespace ExtractAndRename
{
    class Program
    {
        static void Main(string[] args)
        {
            var order = new Order("John Doe", 50, 20);

            Console.WriteLine(order.PrintOwning());
        }
    }
}
