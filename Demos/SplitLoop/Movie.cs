using System;
using System.Collections.Generic;
using System.Text;

namespace SplitLoop
{
    public record Movie
    {
        public string Title { get; init; }
        public decimal Price { get; init; }

        public bool IsNewRelease { get; init; }
    }
}
