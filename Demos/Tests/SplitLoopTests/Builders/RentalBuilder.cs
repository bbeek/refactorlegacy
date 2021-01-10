using SplitLoop;
using System;
using System.Collections.Generic;
using System.Text;

namespace SplitLoopTests.Builders
{
    internal class RentalBuilder
    {
        private string title = "A movie";
        private decimal price = 1.0m;
        private bool newRelease = false;
        private int daysRented = 1;

        internal Rental Build() => new Rental()
        {
            Movie = new Movie()
            {
                Title = title,
                Price = price,
                IsNewRelease = newRelease
            },
            DaysRented = daysRented
        };

        internal RentalBuilder HasPrice(decimal moviePrice)
        {
            price = moviePrice;

            return this;
        }

        internal RentalBuilder ForNewRelease()
        {
            newRelease = true;

            return this;
        }

        internal RentalBuilder IsRentedFor(int days)
        {
            daysRented = days;

            return this;
        }

        internal RentalBuilder WithTitle(string movieTitle)
        {
            title = movieTitle;

            return this;
        }
    }
}
