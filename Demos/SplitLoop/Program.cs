using System;

namespace SplitLoop
{
    class Program
    {
        static void Main(string[] args)
        {
            Movie movie = new Movie() { Title = "Transformer", Price = 2m };

            Rental rental = new Rental(){ Movie = movie, DaysRented = 3 };

            Customer customer = new Customer("John");
            customer.AddRental(rental);

            var statement = customer.Statement();
            System.Console.WriteLine(statement);
        }
    }
}
