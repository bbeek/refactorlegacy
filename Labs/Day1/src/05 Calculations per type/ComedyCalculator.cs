using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheatricalPlays
{
    class ComedyCalculator : PerformanceCalculator
    {
        public override int GetAmount()
        {
            var result = 30000;
            if (Performance.Audience > 20)
            {
                result += 10000 + 500 * (Performance.Audience - 20);
            }
            result += 300 * Performance.Audience;

            return result;
        }

        public override int GetVolumeCredits()
        {
            return base.GetVolumeCredits() + (int)Math.Floor((decimal)Performance.Audience / 5); ;
        }
    }
}
