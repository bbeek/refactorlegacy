using CommonObjects;
using System.Collections.Generic;

namespace ExtractInterface
{
    public class Receipt
    {
        public Money Amount { get; set; }
        public Money Tax { get; set; }
        public Money Total { get; set; }

        public IEnumerable<string> Format()
        {
            return new List<string>() {
                    "Receipt",
                    "=======",
                    $"Item 1 ... {Amount}",
                    $"Tax    ... {Tax}",
                    "----------------",
                    $"Total  ... {Total}"
            };
        }
    }
}
