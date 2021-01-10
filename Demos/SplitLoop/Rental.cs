using System;
using System.Collections.Generic;
using System.Text;

namespace SplitLoop
{
    public record Rental
    {
        public Movie Movie { get; init; }
        public int DaysRented { get; init; }

    }
}
