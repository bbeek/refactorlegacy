using System;
using System.Collections.Generic;
using System.Text;

namespace SplitLoop
{
    public class Customer
    {
        public string Name { get; }
        private List<Rental> Rentals = new List<Rental>();

        public Customer(string name)
        {
            this.Name = name;
        }

        public void AddRental(Rental rental)
        {
            Rentals.Add(rental);
        }

        /*
         * 1. Copy the loop.
         * 2. Identify and remove duplicate side effects
         * 3. Test
         * 4. Optional: Consider extracting functions on each of the loops
         */
        public string Statement()
        {
            var result = new StringBuilder();
            result.AppendLine($"Rental Record for {this.Name}");
            decimal totalAmount = 0;
            int frequentRenterPoints = 0;

            // determine amounts for each line
            foreach (var rental in Rentals)
            {
                decimal thisAmount = rental.Movie.Price;

                // Calculate 
                if (rental.DaysRented > 2)
                {
                    thisAmount += (rental.DaysRented - 2) * 1.5m;
                }

                // frequent renter points
                frequentRenterPoints++;

                // add bonus for a two day new release rental
                if (rental.Movie.IsNewRelease && rental.DaysRented > 1)
                {
                    frequentRenterPoints++;
                }

                // show figures for this rental
                result.AppendLine("\t" + rental.Movie.Title + "\t" + thisAmount.ToString());
                totalAmount += thisAmount;
            }

            // add footer lines
            result.AppendLine("Amount owed is " + totalAmount.ToString(""));
            result.AppendFormat("You earned {0} frequent renter points", frequentRenterPoints);

            return result.ToString();
        }
    }
}