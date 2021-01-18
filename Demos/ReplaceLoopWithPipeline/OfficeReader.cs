using System;
using System.Collections.Generic;
using System.Linq;

namespace ReplaceLoopWithPipeline
{
    public class OfficeReader
    {
        /*
         * 1. Create a new variable for the loop's collection
         * 2. Starting from the top, take each bit of behaviour in the loop 
         *    and replace it with a collection pipeline operation
         * 3. Test after each change.
         * 4. Once all behaviour is removed from the loop, remove it.
         */

        public IEnumerable<Office> GetNorwayOffices(string input)
        {
            var lines = input.Split("\n");
            var firstLine = true;
            var result = new List<Office>();
            foreach(var line in lines)
            {
                if (firstLine)
                {
                    firstLine = false;
                    continue;
                }
                if (string.IsNullOrEmpty(line.Trim()))
                {
                    continue;
                }
                var record = line.Split(",");
                if (record[1].Trim().Equals("Norway"))
                {
                    result.Add(new Office { City = record[0].Trim(), Phone = record[2].Trim(), Address = record[3].Trim() });
                }
            }

            return result;
        }
    }
}
