using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;

namespace TheatricalPlays
{
    class Program
    {
        static void Main(string[] args)
        {
            var plays = JsonConvert.DeserializeObject<Dictionary<string, Play>>(File.ReadAllText("plays.json")).ToImmutableDictionary();
            var invoices = JsonConvert.DeserializeObject<IEnumerable<Invoice>>(File.ReadAllText("invoices.json"));

            var generator = new BillGenerator(plays);
            foreach(var invoice in invoices)
            {
                Console.WriteLine(generator.Statement(invoice));
                Console.WriteLine();
            }
        }

    }
}
